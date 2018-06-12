using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.ViewInfo;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.Presentation.Presenters;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Services;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.UI.Binders
{
   public class ValueOriginBinder<T> : IDisposable where T : class, IWithValueOrigin
   {
      private readonly IValueOriginPresenter _valueOriginPresenter;
      private readonly IImageListRetriever _imageListRetriever;
      private readonly IToolTipCreator _toolTipCreator;
      private GridViewBinder<T> _gridViewBinder;
      private readonly RepositoryItemPopupContainerEdit _repositoryItemPopupContainerEdit = new RepositoryItemPopupContainerEdit();
      private readonly PopupContainerControl _popupControl = new PopupContainerControl();
      private UxGridView _gridView;
      private Action<T, ValueOrigin> _onValueOriginUpdated;
      private IGridViewColumn _valueOriginColumn;
      private Func<T, bool> _valueOriginEditableFunc = x => true;
      private const int IMAGE_OFFSET = 2;

      public ValueOriginBinder(IValueOriginPresenter valueOriginPresenter, IImageListRetriever imageListRetriever, IToolTipCreator toolTipCreator)
      {
         _valueOriginPresenter = valueOriginPresenter;
         _imageListRetriever = imageListRetriever;
         _toolTipCreator = toolTipCreator;
         _popupControl.FillWith(_valueOriginPresenter.BaseView);
         _repositoryItemPopupContainerEdit.Buttons[0].Kind = ButtonPredefines.Combo;
         _repositoryItemPopupContainerEdit.PopupControl = _popupControl;
         _repositoryItemPopupContainerEdit.CloseOnOuterMouseClick = false;
         _repositoryItemPopupContainerEdit.QueryDisplayText += (o, e) => queryDisplayText(e);
         _repositoryItemPopupContainerEdit.CloseUp += (o, e) => closeUp(e);
         _repositoryItemPopupContainerEdit.CloseUpKey = new KeyShortcut(Keys.Enter);
         _repositoryItemPopupContainerEdit.AllowDropDownWhenReadOnly = DefaultBoolean.True;
      }

      private void onToolTipControllerGetActiveObjectInfo(object sender, ToolTipControllerGetActiveObjectInfoEventArgs e)
      {
         var column = _gridView.ColumnAt(e);
         if (!ColumnIsValueOrigin(column))
            return;

         var withValueOrigin = _gridViewBinder.ElementAt(e);
         if (withValueOrigin == null)
            return;

         var superToolTip = _toolTipCreator.ToolTipFor(withValueOrigin.ValueOrigin);
         if (superToolTip == null)
            return;

         e.Info = _toolTipCreator.ToolTipControlInfoFor(withValueOrigin, superToolTip);
      }

      private void closeUp(CloseUpEventArgs e)
      {
         var cancel = e.CloseMode == PopupCloseMode.Cancel;
         updateValueOrigin(cancel);
      }

      private void updateValueOrigin(bool canceled)
      {
         //Ensure that we save the edited value only if edit was not canceled
         if (!canceled && _valueOriginPresenter.ValueOriginChanged)
            _onValueOriginUpdated(_gridViewBinder.FocusedElement, _valueOriginPresenter.UpdatedValueOrigin);

         _gridView.CloseEditor();
      }

      /// <summary>
      ///    Initializes the binding to <see cref="ValueOrigin" />
      /// </summary>
      /// <param name="gridViewBinder">
      ///    The gridViewBinder that will be extended to receive binding for <see cref="ValueOrigin" />
      /// </param>
      /// <param name="onValueOriginUpdated">
      ///    This method is called whenever the <see cref="ValueOrigin" /> was changed by the
      ///    user
      /// </param>
      /// <param name="valueOriginEditableFunc">
      ///    Optional method that specifies whether the <see cref="ValueOrigin" /> of a bound
      ///    object can be edited or not.
      /// </param>
      /// <param name="defaultColumnWidth">
      ///    Default column width for the value origin column. If null, no default witdh will be
      ///    set
      /// </param>
      public void InitializeBinding(
         GridViewBinder<T> gridViewBinder,
         Action<T, ValueOrigin> onValueOriginUpdated,
         Func<T, bool> valueOriginEditableFunc = null,
         int? defaultColumnWidth = UIConstants.Size.EMBEDDED_DESCRIPTION_WIDTH)
      {
         _gridViewBinder = gridViewBinder;
         _gridView = _gridViewBinder.GridView.DowncastTo<UxGridView>();
         _onValueOriginUpdated = onValueOriginUpdated;

         if (valueOriginEditableFunc != null)
            _valueOriginEditableFunc = valueOriginEditableFunc;

         _gridView.ShowingEditor += onShowingEditor;
         _gridView.RowCellStyle += updateRowCellStyle;
         _gridView.CustomDrawCell += customDrawCell;

         _valueOriginColumn = _gridViewBinder.Bind(x => x.ValueOrigin)
            .WithCaption(Captions.ValueOrigin)
            .WithEditRepository(editRepositoryFor)
            .WithEditorConfiguration((editor, withValueOrigin) => { _valueOriginPresenter.Edit(withValueOrigin.ValueOrigin); });

         if (defaultColumnWidth.HasValue)
            _valueOriginColumn.WithWidth(defaultColumnWidth.Value);

         initializeToolTip(_gridView.GridControl);
      }

      private void customDrawCell(object sender, RowCellCustomDrawEventArgs e)
      {
         var valueOrigin = withValueOriginAt(e)?.ValueOrigin;
         if (valueOrigin == null)
            return;

         var cellInfo = e.Cell as GridCellInfo;
         var info = cellInfo?.ViewInfo as TextEditViewInfo;
         if (info == null)
            return;

         info.ContextImage = imageFor(valueOrigin);
         info.CalcViewInfo();
      }

      private Image imageFor(ValueOrigin valueOrigin)
      {
         var sourceImage = valueOrigin.Source.Icon.ToImage(IconSizes.Size16x16);
         if (valueOrigin.Method == ValueOriginDeterminationMethods.Undefined)
            return sourceImage;

         var methodImage = valueOrigin.Method.Icon.ToImage(IconSizes.Size16x16);
         if (valueOrigin.Source == ValueOriginSources.Undefined)
            return methodImage;

         var resultImage = new Bitmap(sourceImage.Width + methodImage.Width + IMAGE_OFFSET, sourceImage.Height);

         using (var g = Graphics.FromImage(resultImage))
         {
            g.DrawImage(sourceImage, new PointF(0, 0));
            g.DrawImage(methodImage, new PointF(sourceImage.Width + IMAGE_OFFSET, 0));
         }

         return resultImage;
      }

      private void updateRowCellStyle(object sender, RowCellStyleEventArgs e)
      {
         var withValueOrigin = withValueOriginAt(e);
         if (withValueOrigin == null)
            return;

         var canEditValueOrigin = _valueOriginEditableFunc(withValueOrigin);
         _gridView.AdjustAppearance(e, canEditValueOrigin);
      }

      private T withValueOriginAt(RowCellCustomDrawEventArgs e) => withValueOriginAt(e.Column, e.RowHandle);

      private T withValueOriginAt(CustomRowCellEventArgs e) => withValueOriginAt(e.Column, e.RowHandle);

      private T withValueOriginAt(GridColumn column, int rowHandle)
      {
         if (!ColumnIsValueOrigin(column))
            return null;

         return _gridViewBinder.ElementAt(rowHandle);
      }

      public bool ValueOriginVisible
      {
         get => _valueOriginColumn.Visible;
         set => _valueOriginColumn.UpdateVisibility(value);
      }

      private void onShowingEditor(object sender, CancelEventArgs e)
      {
         if (!ColumnIsValueOrigin(_gridView.FocusedColumn))
            return;

         var withValueOrigin = _gridViewBinder.FocusedElement;
         e.Cancel = !_valueOriginEditableFunc(withValueOrigin);
      }

      private void initializeToolTip(GridControl gridControl)
      {
         if (gridControl == null)
            return;

         if (gridControl.ToolTipController == null)
         {
            var toolTipController = new ToolTipController();
            toolTipController.Initialize(_imageListRetriever);
            gridControl.ToolTipController = toolTipController;
         }

         gridControl.ToolTipController.GetActiveObjectInfo += onToolTipControllerGetActiveObjectInfo;
      }

      private RepositoryItem editRepositoryFor(T withValueOrigin)
      {
         return _repositoryItemPopupContainerEdit;
      }

      private void queryDisplayText(QueryDisplayTextEventArgs e)
      {
         var withValueOrigin = _gridViewBinder.FocusedElement;
         if (withValueOrigin == null) return;
         e.DisplayText = withValueOrigin.ValueOrigin.Display;
      }

      protected virtual void Cleanup()
      {
         _valueOriginPresenter.Dispose();
         _valueOriginEditableFunc = null;
         _onValueOriginUpdated = null;
      }

      public bool ColumnIsValueOrigin(GridColumn column) => Equals(column, _valueOriginColumn.XtraColumn);

      #region Disposable properties

      private bool _disposed;

      public void Dispose()
      {
         if (_disposed) return;

         Cleanup();
         GC.SuppressFinalize(this);
         _disposed = true;
      }

      ~ValueOriginBinder()
      {
         Cleanup();
      }

      #endregion
   }
}