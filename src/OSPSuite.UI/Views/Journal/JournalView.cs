using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Forms;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout.Utils;
using DevExpress.XtraRichEdit;
using OSPSuite.Assets;
using OSPSuite.Core.Journal;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.DTO.Journal;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Presenters.Journal;
using OSPSuite.Presentation.Views;
using OSPSuite.Presentation.Views.Journal;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.RepositoryItems;
using OSPSuite.UI.Services;

namespace OSPSuite.UI.Views.Journal
{
   public partial class JournalView : BaseContainerUserControl, IJournalView, IViewWithPopup
   {
      private readonly IToolTipCreator _toolTipCreator;
      private readonly IImageListRetriever _imageListRetriever;
      private IJournalPresenter _presenter;
      private readonly GridViewBinder<JournalPageDTO> _gridViewBinder;
      private float _rowFontSize;
      private readonly DateTimeFormatter _dateTimeFormatter;
      private readonly RepositoryItemRichTextEdit _descriptionRepository;
      private IGridViewColumn _columnTags;
      private readonly RepositoryItemTextEdit _titleRepository;
      public BarManager PopupBarManager { get; private set; }

      public JournalView(IToolTipCreator toolTipCreator, IImageListRetriever imageListRetriever)
      {
         _toolTipCreator = toolTipCreator;
         _imageListRetriever = imageListRetriever;
         InitializeComponent();
         _gridViewBinder = new GridViewBinder<JournalPageDTO>(gridView);
         gridView.ShouldUseColorForDisabledCell = false;
         gridView.ShowColumnChooser = true;
         _rowFontSize = AppearanceObject.DefaultFont.Size;
         PopupBarManager = new BarManager { Form = this, Images = imageListRetriever.AllImagesForContextMenu };
         _toolTipCreator = toolTipCreator;
         var toolTipController = new ToolTipController();
         toolTipController.GetActiveObjectInfo += onToolTipControllerGetActiveObjectInfo;
         toolTipController.Initialize(imageListRetriever);
         gridControl.ToolTipController = toolTipController;

         gridView.MouseDown += (o, e) => OnEvent(onGridViewMouseDown, e);
         gridView.DoubleClick += (o, e) => OnEvent(onGridViewDoubleClicked, e);
         gridView.RowCellStyle += (o, e) => OnEvent(onRowCellStyle, e);
         gridView.CustomDrawRowPreview += (o, e) => OnEvent(onCustomDrawRowPreview, e);
         gridView.ShowFilterPopupListBox += (o, e) => OnEvent(onShowFilterPopupListBox, e);
         gridView.MeasurePreviewHeight += (o, e) => OnEvent(onMeasurePreviewHeight, e);

         _titleRepository = new RepositoryItemTextEdit();

         _dateTimeFormatter = new DateTimeFormatter();
         _descriptionRepository = new RepositoryItemRichTextEdit { DocumentFormat = DocumentFormat.Html };
      }

      private void onMeasurePreviewHeight(RowHeightEventArgs e)
      {
         if (gridView.PreviewRowEdit == null)
            return;

         var journalPageDTO = _gridViewBinder.ElementAt(e.RowHandle);
         if (journalPageDTO == null)
            return;

         //empiric value (might be a way to retrieve it dynamically but could not find it)
         e.RowHeight = journalPageDTO.LineCount * 15;
      }

      private void onShowFilterPopupListBox(FilterPopupListBoxEventArgs e)
      {
         if (e.Column != _columnTags.XtraColumn) return;

         e.ComboBox.BeginUpdate();

         try
         {
            e.ComboBox.Items.Clear();
            foreach (var tag in _presenter.AvailableTags)
            {
               var filterInfo = new ColumnFilterInfo(getFilterString(e.Column.FieldName, tag), getFilterDisplayText(tag));
               e.ComboBox.Items.Add(new FilterItem(tag, filterInfo));
            }
         }
         finally
         {
            e.ComboBox.EndUpdate();
         }
      }

      private string getFilterString(string fieldName, string tag)
      {
         return $"[{fieldName}] LIKE '%{tag}%'";
      }

      private string getFilterDisplayText(string tag)
      {
         return $"Is tagged with '{tag}'";
      }

      private void onGridViewDoubleClicked(EventArgs e)
      {
         var pt = gridView.GridControl.PointToClient(MousePosition);

         var hi = gridView.CalcHitInfo(pt);
         if (!hi.InDataRow)
            return;

         _presenter.Edit(_gridViewBinder.ElementAt(hi.RowHandle));
      }

      private void onToolTipControllerGetActiveObjectInfo(object sender, ToolTipControllerGetActiveObjectInfoEventArgs e)
      {
         var journalPageDTO = _gridViewBinder.ElementAt(e);
         if (journalPageDTO == null) return;

         var superToolTip = _toolTipCreator.ToolTipFor(journalPageDTO);
         e.Info = _toolTipCreator.ToolTipControlInfoFor(journalPageDTO, superToolTip);
      }

      private void onGridViewMouseDown(MouseEventArgs e)
      {
         var journalPageDTO = _gridViewBinder.ElementAt(gridView.RowHandleAt(e));
         if (journalPageDTO != null)
            _presenter.Select(journalPageDTO);

         if (e.Button == MouseButtons.Right)
            _presenter.CreatePopupMenuFor(journalPageDTO).At(e.Location);
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();

         bind(x => x.UniqueIndex)
            .WithCaption(UIConstants.EMPTY_COLUMN)
            .WithFixedWidth(UIConstants.Size.EMBEDDED_BUTTON_WIDTH);

         bind(x => x.Title)
            .WithRepository(configureTitleRepository)
            .WithCaption(Captions.Journal.Title);

         var colCreatedAt = bind(x => x.CreatedAt)
            .WithFormat(_dateTimeFormatter)
            .WithCaption(Captions.Journal.CreatedAt);

         _columnTags = bind(x => x.TagsDisplay)
            .WithCaption(Captions.Journal.Tags);

         //since we are overriding the default behavior of tag filtering, we need to use list instead of the default
         _columnTags.XtraColumn.OptionsFilter.FilterPopupMode = FilterPopupMode.List;

         var colUpdatedAt = bind(x => x.UpdatedAt)
            .WithFormat(_dateTimeFormatter)
            .WithCaption(Captions.Journal.UpdatedAt);

         var colUpdatedBy = bind(x => x.UpdatedBy)
            .WithCaption(Captions.Journal.UpdatedBy);

         var colCreatedBy = bind(x => x.CreatedBy)
            .WithCaption(Captions.Journal.CreatedBy);

         var colDescription = bind(x => x.Description)
            .WithRepository(x => _descriptionRepository)
            .WithCaption(Captions.Journal.Description);

         colDescription.Visible = false;
         colDescription.XtraColumn.OptionsColumn.ShowInCustomizationForm = false;
         colUpdatedBy.Visible = false;
         colCreatedBy.Visible = false;
         colUpdatedAt.Visible = false;

         colCreatedAt.XtraColumn.SortOrder = ColumnSortOrder.Descending;
         gridView.PreviewFieldName = colDescription.XtraColumn.FieldName;
      }

      private JournalPage pageFromRowHandle(int rowHandle)
      {
         return _gridViewBinder.ElementAt(rowHandle).JournalPage;
      }

      private IEnumerable<int> visibleRowHandles()
      {
         for (var i = 0; i < gridView.RowCount; i++)
         {
            yield return gridView.GetVisibleRowHandle(i);
         }
      }

      private RepositoryItem configureTitleRepository(JournalPageDTO dto)
      {
         if (_presenter.AllItemsHaveTheSameOrigin)
            return _titleRepository;

         var titleWithImageRepository = new UxRepositoryItemImageComboBox(gridView, _imageListRetriever);
         return titleWithImageRepository.AddItem(dto.Title, dto.Origin.Icon);
      }

      private IGridViewAutoBindColumn<JournalPageDTO, T> bind<T>(Expression<Func<JournalPageDTO, T>> expression)
      {
         var column = _gridViewBinder.AutoBind(expression);
         column.XtraColumn.OptionsColumn.AllowEdit = false;
         column.XtraColumn.OptionsColumn.AllowFocus = false;
         return column;
      }

      public void AttachPresenter(IJournalPresenter presenter)
      {
         _presenter = presenter;
      }

      public void BindTo(IEnumerable<JournalPageDTO> allWorkingJournalItemDTOs)
      {
         _gridViewBinder.BindToSource(allWorkingJournalItemDTOs);
         gridView.BestFitColumns();
      }

      public void UpdateLayout()
      {
         gridView.LayoutChanged();
      }

      public void AddPreviewView(IView view)
      {
         AddViewTo(layoutItemPreview, view);
      }

      public void AddSearchView(IView view)
      {
         AddViewTo(layoutitemSearch, view);
      }

      public JournalPageDTO SelectedJournalPage
      {
         get => _gridViewBinder.FocusedElement;
         set
         {
            if (value == null) return;
            var rowHandle = _gridViewBinder.RowHandleFor(value);
            gridView.FocusedRowHandle = rowHandle;
         }
      }

      public bool SearchVisible
      {
         get => layoutGroupSearch.Visible;
         set
         {
            layoutGroupSearch.Visibility = LayoutVisibilityConvertor.FromBoolean(value);
            gridView.PreviewRowEdit = value ? _descriptionRepository : null;
         }
      }

      public IEnumerable<JournalPage> VisibleJournalPages()
      {
         return visibleRowHandles().Select(pageFromRowHandle);
      }

      private void setViewFontSize(float rowFontSize, float previewFontSize)
      {
         if (previewFontSize > 0)
            gridView.Appearance.Preview.Font = new Font(AppearanceObject.DefaultFont.FontFamily, AppearanceObject.DefaultFont.Size + previewFontSize);

         if (rowFontSize > 0)
         {
            _rowFontSize += rowFontSize;
            gridView.Appearance.Row.Font = new Font(AppearanceObject.DefaultFont.FontFamily, _rowFontSize);
         }
      }

      public override void InitializeResources()
      {
         base.InitializeResources();

         gridView.OptionsSelection.EnableAppearanceHideSelection = false;
         gridView.OptionsView.AutoCalcPreviewLineCount = true;
         gridView.OptionsView.EnableAppearanceEvenRow = true;
         gridView.OptionsView.ShowGroupPanel = false;
         gridView.OptionsView.ShowHorizontalLines = DefaultBoolean.False;
         gridView.OptionsView.ShowIndicator = false;
         gridView.OptionsView.ShowPreview = true;
         gridView.OptionsView.ShowVerticalLines = DefaultBoolean.False;
         gridView.PreviewIndent = 0;
         setViewFontSize(1, 0);
         layoutItemGridView.TextVisible = false;
         layoutItemPreview.TextVisible = false;
         layoutitemSearch.TextVisible = false;
      }

      private void onCustomDrawRowPreview(RowObjectCustomDrawEventArgs e)
      {
         if (rowHasFocus(e.RowHandle))
         {
            e.Appearance.BackColor = focusedRowBackColor;
            e.Appearance.ForeColor = focusedRowForeColor;
         }
         else
            setDefaultRowBackColor(e.Appearance, e.RowHandle);
      }

      private void onRowCellStyle(RowCellStyleEventArgs e)
      {
         var journalPageDTO = _gridViewBinder.ElementAt(e.RowHandle);
         var fontStyle = journalPageDTO.Edited ? FontStyle.Bold : FontStyle.Regular;
         e.Appearance.Font = new Font(e.Appearance.Font.FontFamily, _rowFontSize, fontStyle);

         if (rowHasFocus(e.RowHandle))
            e.Appearance.BackColor = focusedRowBackColor;
         else
            setDefaultRowBackColor(e.Appearance, e.RowHandle);
      }

      private bool rowHasFocus(int rowHandle)
      {
         return rowHandle == gridView.FocusedRowHandle && gridView.GridControl.Focused;
      }

      private Color focusedRowBackColor => gridView.PaintAppearance.FocusedRow.BackColor;

      private Color focusedRowForeColor => gridView.PaintAppearance.FocusedRow.ForeColor;

      private void setDefaultRowBackColor(AppearanceObject appearance, int rowHandle)
      {
         var rowAppearance = rowHandle % 2 == 0 ? gridView.PaintAppearance.EvenRow : gridView.PaintAppearance.Row;
         appearance.BackColor = rowAppearance.BackColor;
      }
   }
}