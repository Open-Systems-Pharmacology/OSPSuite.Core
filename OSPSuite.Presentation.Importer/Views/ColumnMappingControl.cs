using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.Utils.Menu;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Menu;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using OSPSuite.Assets;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.Presentation.Importer.Core.DataFormat;
using OSPSuite.Presentation.Importer.Presenters;
using OSPSuite.UI;
using OSPSuite.UI.Controls;
using OSPSuite.UI.RepositoryItems;
using OSPSuite.UI.Services;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Importer.Views
{
   public partial class ColumnMappingControl : BaseUserControl, IColumnMappingControl
   {
      private readonly IImageListRetriever _imageListRetriever;
      private readonly Dictionary<int, ImageComboBoxEdit> _editorsForEditing = new Dictionary<int, ImageComboBoxEdit>();
      private IColumnMappingPresenter _presenter;
      private readonly GridViewBinder<ColumnMappingViewModel> _gridViewBinder;
      private readonly RepositoryItemButtonEdit _removeButtonRepository = new UxRemoveButtonRepository();
      private readonly RepositoryItemButtonEdit _disabledRemoveButtonRepository = new UxRemoveButtonRepository();

      private readonly RepositoryItemButtonEdit _unitButtonRepository =
         new UxRepositoryItemButtonImage(ApplicationIcons.UnitInformation, Captions.UnitInformationDescription);

      private readonly RepositoryItemButtonEdit _disabledUnitButtonRepository = new UxRepositoryItemButtonImage(ApplicationIcons.EmptyIcon);
      private readonly RepositoryItemButtonEdit _addButtonRepository = new UxRepositoryItemButtonImage(ApplicationIcons.Add, Captions.AddInformationDescription);

      public ColumnMappingControl(IImageListRetriever imageListRetriever)
      {
         _imageListRetriever = imageListRetriever;
         InitializeComponent();
         _gridViewBinder = new GridViewBinder<ColumnMappingViewModel>(uxGridView);
         uxGridView.OptionsView.ShowGroupPanel = false;
         uxGridView.OptionsMenu.EnableColumnMenu = false;
         uxGridView.CellValueChanged += (s, e) => _presenter.ValidateMapping();

         uxGridView.OptionsView.ShowButtonMode = ShowButtonModeEnum.ShowOnlyInEditor;
         uxGridView.OptionsBehavior.EditorShowMode = EditorShowMode.MouseUp;
         uxGridView.MouseDown += onMouseDown;
         uxGrid.ToolTipController = new ToolTipController();
         uxGrid.ToolTipController.GetActiveObjectInfo += (o, e) => this.DoWithinExceptionHandler(() => onGetActiveObjectInfo(o, e));
         var unitInformationTip = new SuperToolTip();
         unitInformationTip.Items.Add(Captions.UnitInformationDescription);
      }

      public void AttachPresenter(IColumnMappingPresenter presenter)
      {
         _presenter = presenter;
      }

      private RepositoryItemImageComboBox valueRepository(ColumnMappingViewModel model)
      {
         var repo = new RepositoryItemImageComboBox
         {
            AutoComplete = true,
            AllowNullInput = DefaultBoolean.True,
            CloseUpKey = new KeyShortcut(Keys.Enter)
         };
         fillComboBoxItems(repo, _presenter.GetAvailableOptionsFor(model));
         return repo;
      }

      private RepositoryItemImageComboBox nameRepository(ColumnMappingViewModel model)
      {
         var repo = new RepositoryItemImageComboBox
         {
            AllowNullInput = DefaultBoolean.True
         };
         fillComboBoxItem
         (
            repo, 
            new ColumnMappingOption() 
            { 
               Label = model.MappingName, 
               IconIndex = model.Icon,
               Description = model.MappingName
            } 
         );
         return repo;
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _gridViewBinder.AutoBind(x => x.MappingName)
            .WithCaption(Captions.Name)
            .WithRepository(nameRepository)
            .AsReadOnly();

         _gridViewBinder.AutoBind(x => x.Description)
            .WithCaption(Captions.Description)
            .WithRepository(valueRepository)
            .WithOnValueUpdating(onValueChanged)
            .WithShowButton(ShowButtonModeEnum.ShowAlways);

         _gridViewBinder.AddUnboundColumn()
            .WithCaption(UIConstants.EMPTY_COLUMN)
            .WithShowButton(ShowButtonModeEnum.ShowOnlyInEditor)
            .WithRepository(removeRepository)
            .WithFixedWidth(UIConstants.Size.BUTTON_WIDTH);

         _gridViewBinder.AddUnboundColumn()
            .WithCaption(UIConstants.EMPTY_COLUMN)
            .WithShowButton(ShowButtonModeEnum.ShowOnlyInEditor)
            .WithRepository(unitRepository)
            .WithFixedWidth(UIConstants.Size.BUTTON_WIDTH);

         _removeButtonRepository.ButtonClick += (o, e) =>
         {
            uxGridView.ActiveEditor.EditValue = ColumnMappingFormatter.Ignored();
            _presenter.ClearRow(_gridViewBinder.FocusedElement);
         };
         _unitButtonRepository.ButtonClick += (o, e) => _presenter.ChangeUnitsOnRow(_gridViewBinder.FocusedElement);
         _addButtonRepository.ButtonClick += (o, e) => _presenter.AddGroupBy(_gridViewBinder.FocusedElement.Source as AddGroupByFormatParameter);
         _disabledRemoveButtonRepository.Buttons[0].Enabled = false;
         _disabledUnitButtonRepository.Buttons[0].Enabled = false;
      }

      private RepositoryItem removeRepository(ColumnMappingViewModel model)
      {
         return model.Source == null || model.Source is IgnoredDataFormatParameter || model.Source is AddGroupByFormatParameter ? _disabledRemoveButtonRepository : _removeButtonRepository;
      }

      private RepositoryItem unitRepository(ColumnMappingViewModel model)
      {
         if (model.Source is MappingDataFormatParameter)
            return _unitButtonRepository;
         if (model.Source is AddGroupByFormatParameter)
            return _addButtonRepository;
         return _disabledUnitButtonRepository;
      }

      public void Rebind()
      { 
         _gridViewBinder.Rebind();
      }

      public void SetMappingSource(IList<ColumnMappingViewModel> mappings)
      {
         _gridViewBinder.BindToSource(mappings);
      }

      private void onGetActiveObjectInfo(object sender, ToolTipControllerGetActiveObjectInfoEventArgs e)
      {
         if (sender != uxGrid.ToolTipController) return;
         var view = uxGrid.GetViewAt(e.ControlMousePosition) as GridView;
         if (view == null) return;
         var hitInfo = view.CalcHitInfo(e.ControlMousePosition);

         if (!hitInfo.InRow) return;
         e.Info = new ToolTipControlInfo(hitInfo.RowHandle, string.Empty)
         {
            SuperTip = generateToolTipControlInfo(hitInfo.RowHandle),
            ToolTipType = ToolTipType.SuperTip
         };
         uxGrid.ToolTipController.ShowHint(e.Info);
      }

      private SuperToolTip generateToolTipControlInfo(int index)
      {
         var superToolTip = new SuperToolTip();
         var tooltip = _presenter.ToolTipDescriptionFor(index);
         superToolTip.Items.AddTitle(tooltip.Title);
         superToolTip.Items.Add(tooltip.Description);
         return superToolTip;
      }

      private void fillComboBoxItem(RepositoryItemImageComboBox editor, ColumnMappingOption option)
      {
         editor.Items.Clear();
         editor.SmallImages = _imageListRetriever.AllImages16x16;
         editor.NullText = Captions.Importer.NoneEditorNullText;

         editor.Items.Add(new ImageComboBoxItem(option.Description)
         {
            Description = option.Label,
            ImageIndex = option.IconIndex
         });
      }

      private void fillComboBoxItems(RepositoryItemImageComboBox editor, IEnumerable<ColumnMappingOption> options)
      {
         editor.Items.Clear();
         editor.NullText = Captions.Importer.NoneEditorNullText;
         foreach (var option in options)
         {
            editor.Items.Add(new ImageComboBoxItem(option.Description)
            {
               Description = option.Label,
            });
         }

         editor.KeyDown += clearSelectionOnDeleteForComboBoxEdit;
         if (editor.Items.Count != 0) return;
         editor.NullText = Captions.Importer.NothingSelectableEditorNullText;
         editor.Enabled = false;
      }

      private void onMouseDown(object sender, MouseEventArgs mouseEventArgs)
      {
         if (mouseEventArgs.Button != MouseButtons.Right) return;
         if (!(sender is GridView mv)) return;

         var menu = new GridViewColumnMenu(mv);
         menu.Items.Clear();
         menu.Items.Add(new DXMenuItem(Captions.Importer.ResetMapping, onCreateAutoMappingClick));
         menu.Items.Add(new DXMenuItem(Captions.Importer.ClearMapping, onClearMappingClick));
         menu.Show(mouseEventArgs.Location);
      }

      private static void clearSelectionOnDeleteForComboBoxEdit(object sender, KeyEventArgs e)
      {
         var comboBoxEdit = sender as ImageComboBoxEdit;
         var gridControl = comboBoxEdit?.Parent as UxGridControl;
         var view = gridControl?.FocusedView as ColumnView;
         if (view == null) return;

         if (e.KeyCode == Keys.Delete)
            view.ActiveEditor.EditValue = Captions.Importer.NoneEditorNullText;
      }

      private void onValueChanged(ColumnMappingViewModel model, PropertyValueSetEventArgs<string> e)
      {
         _presenter.SetDescriptionForRow(model);
      }

      private void onCreateAutoMappingClick(object sender, EventArgs eventArgs)
      {
         _presenter.ResetMapping();
      }

      private void onClearMappingClick(object sender, EventArgs eventArgs)
      {
         _presenter.ClearMapping();
      }
   }
}