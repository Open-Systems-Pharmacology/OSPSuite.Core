using System.ComponentModel;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout.Utils;
using OSPSuite.Assets;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Presenters.Parameters;
using OSPSuite.Presentation.Views.Parameters;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Core;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.RepositoryItems;

namespace OSPSuite.UI.Views.Parameters
{
   public partial class TableFormulaView : BaseUserControlWithValueInGrid, ITableFormulaView
   {
      private ITableFormulaPresenter _presenter;
      private readonly GridViewBinder<ValuePointDTO> _gridViewBinder;
      private readonly ScreenBinder<TableFormulaDTO> _screenBinder;
      protected IGridViewColumn _columnX;
      protected IGridViewColumn _columnY;
      private readonly UxRepositoryItemButtonEdit _removePointRepository = new UxRemoveButtonRepository();
      private IGridViewColumn _removeColumn;
      private bool _editable;
      private string _importDescription;
      private RepositoryItem _uxRepositoryItemCheckEdit;
      private IGridViewBoundColumn _columnRestartSolver;

      public TableFormulaView()
      {
         InitializeComponent();
         InitializeWithGrid(gridView);
         gridView.AllowsFiltering = false;
         _editable = true;
         _gridViewBinder = new GridViewBinder<ValuePointDTO>(gridView);
         _screenBinder = new ScreenBinder<TableFormulaDTO>();
         gridView.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;

         gridView.EditorShowMode = EditorShowMode.Default;
         gridView.ShowColumnChooser = true;
         gridView.OptionsNavigation.AutoFocusNewRow = true;
      }

      public override void InitializeBinding()
      {
         gridView.ShowingEditor += onShowingEditor;

         _columnX = _gridViewBinder.AutoBind(x => x.X)
            .WithOnValueUpdating((o, e) => _presenter.SetXValue(o, e.NewValue));

         _columnY = _gridViewBinder.AutoBind(x => x.Y)
            .WithOnValueUpdating((o, e) => _presenter.SetYValue(o, e.NewValue));

         _columnRestartSolver = _gridViewBinder.Bind(dto => dto.RestartSolver)
            .WithRepository(dto => _uxRepositoryItemCheckEdit)
            .WithToolTip(ToolTips.RestartSolver)
            .WithFixedWidth(UIConstants.Size.EMBEDDED_CHECK_BOX_WIDTH)
            .WithOnValueUpdating((o, e) => OnEvent(() => _presenter.SetRestartSolver(o, e.NewValue)));

         _removeColumn = _gridViewBinder.AddUnboundColumn()
            .WithCaption(Captions.EmptyColumn)
            .WithShowButton(ShowButtonModeEnum.ShowAlways)
            .WithRepository(dto => _removePointRepository)
            .WithFixedWidth(UIConstants.Size.EMBEDDED_BUTTON_WIDTH);

         _gridViewBinder.Changed += () => OnEvent(_presenter.ViewChanged);
         btnImportPoints.Click += (o, e) => OnEvent(_presenter.ImportTable);
         btnAddPoint.Click += (o, e) => OnEvent(_presenter.AddPoint);
         _removePointRepository.ButtonClick += (o, e) => OnEvent(() => _presenter.RemovePoint(_gridViewBinder.FocusedElement));

         _screenBinder.Bind(x => x.UseDerivedValues).To(chkUseDerivedValues).OnValueUpdating += (o, e) => _presenter.SetUseDerivedValues(e.NewValue);
      }

      private void onShowingEditor(object sender, CancelEventArgs e) => e.Cancel = !_editable;

      protected override bool ColumnIsValue(GridColumn column) => _columnX.XtraColumn == column || _columnX.XtraColumn == column;

      protected override bool ColumnIsButton(GridColumn column) => _removeColumn.XtraColumn == column;

      public void AttachPresenter(ITableFormulaPresenter presenter) => _presenter = presenter;

      public override void InitializeResources()
      {
         base.InitializeResources();
         lblImportDescription.AsDescription();
         btnImportPoints.Image = ApplicationIcons.LoadFromTemplate.ToImage(IconSizes.Size16x16);
         btnImportPoints.ImageLocation = ImageLocation.MiddleCenter;
         btnAddPoint.Image = ApplicationIcons.Create.ToImage(IconSizes.Size16x16);
         btnAddPoint.ImageLocation = ImageLocation.MiddleCenter;
         layoutItemImportPoints.AdjustButtonSizeWithImageOnly(layoutControl1);
         layoutItemButtonAddPoint.AdjustButtonSizeWithImageOnly(layoutControl1);
         lblImportDescription.Text = string.Empty;
         btnAddPoint.ToolTip = Captions.AddPoint;

         chkUseDerivedValues.Text = Captions.UseDerivedValues;
         chkUseDerivedValues.ToolTip = ToolTips.UseDerivedValues;
         _uxRepositoryItemCheckEdit = new UxRepositoryItemCheckEdit(gridView);
      }

      public void ShowRestartSolver(bool show) => _columnRestartSolver.Visible = show;

      public void ShowUseDerivedValues(bool show) => layoutItemUseDerivedValues.Visibility = LayoutVisibilityConvertor.FromBoolean(show);

      public void Clear() => _gridViewBinder.DeleteBinding();

      public void BindTo(TableFormulaDTO tableFormulaDTO)
      {
         _gridViewBinder.BindToSource(tableFormulaDTO.AllPoints);
         _screenBinder.BindToSource(tableFormulaDTO);
      }

      public void EditPoint(ValuePointDTO pointToEdit)
      {
         var rowHandle = _gridViewBinder.RowHandleFor(pointToEdit);
         gridView.FocusedRowHandle = rowHandle;
         gridView.ShowEditor();
      }

      public bool ImportVisible
      {
         set => layoutItemImportPoints.Visibility = LayoutVisibilityConvertor.FromBoolean(value);
      }

      public string YCaption
      {
         set => _columnY.Caption = value;
      }

      public string XCaption
      {
         set => _columnX.Caption = value;
      }

      public bool Editable
      {
         set
         {
            _editable = value;
            layoutItemImportPoints.Visibility = LayoutVisibilityConvertor.FromBoolean(value);
            layoutItemButtonAddPoint.Visibility = layoutItemImportPoints.Visibility;
            gridView.MultiSelect = !value;
            _removeColumn.UpdateVisibility(value);
         }
         get => _editable;
      }

      public string Description
      {
         get => _importDescription;
         set
         {
            _importDescription = value;
            lblImportDescription.Text = _importDescription.FormatForDescription();
         }
      }

      public string ImportToolTip
      {
         get => btnImportPoints.ToolTip;
         set => btnImportPoints.ToolTip = value;
      }

      public override bool HasError => _gridViewBinder.HasError;
   }


}