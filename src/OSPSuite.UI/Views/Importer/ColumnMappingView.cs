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
using OSPSuite.Core.Import;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.Presentation.Presenters.Importer;
using OSPSuite.Presentation.Views;
using OSPSuite.Presentation.Views.Importer;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.RepositoryItems;
using OSPSuite.UI.Services;
using OSPSuite.Utility.Format;

namespace OSPSuite.UI.Views.Importer
{
   public partial class ColumnMappingView : BaseUserControl, IColumnMappingView
   {
      private readonly IImageListRetriever _imageListRetriever;
      private IColumnMappingPresenter _presenter;
      private readonly GridViewBinder<ColumnMappingDTO> _gridViewBinder;
      private readonly RepositoryItemButtonEdit _removeButtonRepository = new UxRemoveButtonRepository();
      private readonly RepositoryItemButtonEdit _disabledRemoveButtonRepository = new UxRemoveButtonRepository();

      private readonly RepositoryItemButtonEdit _addButtonRepository =
         new UxRepositoryItemButtonImage(ApplicationIcons.Add, Captions.AddInformationDescription);

      private readonly RepositoryItemPopupContainerEdit _repositoryItemPopupContainerEdit = new RepositoryItemPopupContainerEdit();
      private readonly RepositoryItemPopupContainerEdit _disabledPopupContainerEdit = new RepositoryItemPopupContainerEdit();
      private readonly PopupContainerControl _popupControl = new PopupContainerControl();
      private readonly SettingsFormatter _settingsFormatter = new SettingsFormatter();

      public ColumnMappingView(IImageListRetriever imageListRetriever)
      {
         _imageListRetriever = imageListRetriever;
         InitializeComponent();
         _gridViewBinder = new GridViewBinder<ColumnMappingDTO>(columnMappingGridView);
         columnMappingGridView.OptionsView.ShowGroupPanel = false;
         columnMappingGridView.OptionsMenu.EnableColumnMenu = false;
         columnMappingGridView.MultiSelect = false;
         columnMappingGridView.CellValueChanged += (o, e) => OnEvent(_presenter.ValidateMapping);

         columnMappingGridView.OptionsView.ShowButtonMode = ShowButtonModeEnum.ShowOnlyInEditor;
         columnMappingGridView.OptionsBehavior.EditorShowMode = EditorShowMode.MouseDown;
         columnMappingGridView.MouseDown += (o, e) => OnEvent(onMouseDown, o, e);
         columnMappingGrid.ToolTipController = new ToolTipController().Initialize(imageListRetriever);
         columnMappingGrid.ToolTipController.GetActiveObjectInfo += (o, e) => OnEvent(onGetActiveObjectInfo, o, e);
         var unitInformationTip = new SuperToolTip();
         unitInformationTip.Items.Add(Captions.UnitInformationDescription);

         _repositoryItemPopupContainerEdit.Buttons[0].Kind = ButtonPredefines.Combo;
         _repositoryItemPopupContainerEdit.PopupControl = _popupControl;
         _repositoryItemPopupContainerEdit.CloseOnOuterMouseClick = false;
         _repositoryItemPopupContainerEdit.QueryDisplayText += (o, e) => queryDisplayText(e);
         _repositoryItemPopupContainerEdit.CloseUp += (o, e) => closeUp(e);
         _repositoryItemPopupContainerEdit.CloseUpKey = new KeyShortcut(Keys.Enter);
         _repositoryItemPopupContainerEdit.AllowDropDownWhenReadOnly = DefaultBoolean.True;

         _disabledPopupContainerEdit.Enabled = false;
         _disabledPopupContainerEdit.QueryDisplayText += (o, e) => e.DisplayText = " ";
      }

      private void queryDisplayText(QueryDisplayTextEventArgs e)
      {
         var withValueOrigin = _gridViewBinder.FocusedElement;
         if (withValueOrigin == null) return;
         e.DisplayText = withValueOrigin.MappingName;
      }

      private RepositoryItem repositoryItemPopupContainerEdit(ColumnMappingDTO model)
      {
         if (!(model.Source is MappingDataFormatParameter))
            return _disabledPopupContainerEdit;

         _presenter.SetSubEditorSettings(model);
         return _repositoryItemPopupContainerEdit;
      }

      private void closeUp(CloseUpEventArgs e)
      {
         if (e.CloseMode == PopupCloseMode.Cancel)
            return;

         _presenter.UpdateDescriptrionForModel();
      }

      public void FillSubView(IView view)
      {
         _popupControl.FillWith(view);
      }

      public void AttachPresenter(IColumnMappingPresenter presenter)
      {
         _presenter = presenter;
      }

      private RepositoryItem editButtonRepository(ColumnMappingDTO model)
      {
         var repo = new UxRepositoryItemComboBox(columnMappingGridView) {AllowNullInput = DefaultBoolean.True};
         repo.Items.Clear();

         repo.Items.Add(new ComboBoxItem(""));
         return repo;
      }

      private RepositoryItem descriptionRepository(ColumnMappingDTO model)
      {
         var descriptionRepository = new UxRepositoryItemImageComboBox(columnMappingGridView, _imageListRetriever)
         {
            AutoComplete = true,
            AllowNullInput = DefaultBoolean.True,
            NullText = Captions.Importer.NoneEditorNullText,
            CloseUpKey = new KeyShortcut(Keys.Enter)
         };
         fillComboBoxItems(descriptionRepository, _presenter.GetAvailableRowsFor(model));
         return descriptionRepository;
      }

      private RepositoryItemImageComboBox nameRepository(ColumnMappingDTO model)
      {
         var entry = new ColumnMappingOption()
         {
            Label = model.MappingName,
            IconIndex = model.Icon,
            Description = model.MappingName
         };
         var repo = new UxRepositoryItemImageComboBox(columnMappingGridView, _imageListRetriever) {AllowNullInput = DefaultBoolean.True};
         fillComboBoxItem
         (
            repo,
            entry
         );
         return repo;
      }

      private void onValueChanged(ColumnMappingDTO model)
      {
         _presenter.SetDescriptionForRow(model);
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();

         _gridViewBinder.AutoBind(x => x.MappingName)
            .WithCaption(Captions.Importer.MappingName)
            .WithRepository(nameRepository)
            .AsReadOnly();

         _gridViewBinder.AutoBind(x => x.ExcelColumn)
            .WithCaption(Captions.Importer.ExcelColumn)
            .WithRepository(descriptionRepository)
            .WithOnValueUpdated((o, e) => onValueChanged(o))
            .WithShowButton(ShowButtonModeEnum.ShowAlways);

         _gridViewBinder.AutoBind(x => x.Source)
            .WithCaption(Captions.Importer.ExtraColumn)
            .WithFormat(_settingsFormatter)
            .WithRepository(editButtonRepository)
            .WithEditRepository(repositoryItemPopupContainerEdit);

         _gridViewBinder.AddUnboundColumn()
            .WithCaption(UIConstants.EMPTY_COLUMN)
            .WithShowButton(ShowButtonModeEnum.ShowAlways)
            .WithRepository(removeRepository)
            .WithFixedWidth(UIConstants.Size.BUTTON_WIDTH);

         _removeButtonRepository.ButtonClick += (o, e) => OnEvent(() =>
         {
            _presenter.ClearRow(_gridViewBinder.FocusedElement);
            columnMappingGridView.ActiveEditor.EditValue = Captions.Importer.NoneEditorNullText;
            columnMappingGridView.CloseEditor();
         });


         _addButtonRepository.ButtonClick += (o, e) =>
            OnEvent(() => _presenter.AddGroupBy(_gridViewBinder.FocusedElement.Source as AddGroupByFormatParameter));
         _disabledRemoveButtonRepository.Buttons[0].Enabled = false;
      }

      private RepositoryItem removeRepository(ColumnMappingDTO model)
      {
         if (model.Source is AddGroupByFormatParameter)
         {
            if (string.IsNullOrEmpty(model.Source.ColumnName))
            {
               return _disabledRemoveButtonRepository;
            }

            return _addButtonRepository;
         }

         return model.Source == null || model.Source is AddGroupByFormatParameter
            ? _disabledRemoveButtonRepository
            : _removeButtonRepository;
      }

      public void RefreshData()
      {
         columnMappingGridView.RefreshData();
      }

      public void SetMappingSource(IList<ColumnMappingDTO> mappings)
      {
         _gridViewBinder.BindToSource(mappings);
      }

      public void CloseEditor()
      {
         columnMappingGridView.CloseEditor();
      }

      private void onGetActiveObjectInfo(object sender, ToolTipControllerGetActiveObjectInfoEventArgs e)
      {
         if (sender != columnMappingGrid.ToolTipController) return;
         var view = columnMappingGrid.GetViewAt(e.ControlMousePosition) as GridView;
         if (view == null) return;
         var hitInfo = view.CalcHitInfo(e.ControlMousePosition);

         if (!hitInfo.InRow) return;
         e.Info = new ToolTipControlInfo(hitInfo.RowHandle, string.Empty)
         {
            SuperTip = generateToolTipControlInfo(hitInfo.RowHandle),
            ToolTipType = ToolTipType.SuperTip
         };
         columnMappingGrid.ToolTipController.ShowHint(e.Info);
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

      private void fillComboBoxItems(RepositoryItemImageComboBox editor, IEnumerable<string> options)
      {
         editor.Items.Clear();
         editor.NullText = Captions.Importer.NoneEditorNullText;
         foreach (var option in options)
         {
            editor.Items.Add(new ImageComboBoxItem(option));
         }

         editor.KeyDown += (s, a) => OnEvent(clearSelectionOnDeleteForComboBoxEdit, s, a);
         if (editor.Items.Count != 0) return;
         editor.NullText = Captions.Importer.NothingSelectableEditorNullText;
         editor.Enabled = false;
      }

      private void onMouseDown(object sender, MouseEventArgs mouseEventArgs)
      {
         if (mouseEventArgs.Button != MouseButtons.Right)
            return;

         if (!(sender is GridView mv))
            return;

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

      private void onCreateAutoMappingClick(object sender, EventArgs eventArgs)
      {
         _presenter.ResetMapping();
      }

      private void onClearMappingClick(object sender, EventArgs eventArgs)
      {
         _presenter.ClearMapping();
      }
   }

   //TODO move to presenter file and test
   class SettingsFormatter : IFormatter<DataFormatParameter>
   {
      public string Format(DataFormatParameter model)
      {
         if (model == null)
            return "";

         if (model is MappingDataFormatParameter mapping)
         {
            var str = $"Units: {mapping.MappedColumn.Unit.SelectedUnit}";
            if (!string.IsNullOrEmpty(mapping.MappedColumn.LloqColumn))
            {
               str += $", LLOQ: {mapping.MappedColumn.LloqColumn}";
            }

            if (!string.IsNullOrEmpty(mapping.MappedColumn.ErrorStdDev))
            {
               str += $", Error: {mapping.MappedColumn.ErrorStdDev}";
            }

            return str;
         }

         return " ";
      }
   }
}