using System.Collections.Generic;
using System.ComponentModel;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout.Utils;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Presenters.ObservedData;
using OSPSuite.Presentation.Views.ObservedData;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.RepositoryItems;

namespace OSPSuite.UI.Views.ObservedData
{
   public partial class DataRepositoryMetaDataView : BaseUserControl, IDataRepositoryMetaDataView
   {
      private readonly RepositoryItemButtonEdit _removeButtonRepository = new UxRemoveButtonRepository();
      private IDataRepositoryMetaDataPresenter _presenter;
      private readonly GridViewBinder<MetaDataDTO> _gridViewBinder;
      private readonly UxRepositoryItemComboBox _repositoryForPredefinedValues;
      private readonly RepositoryItemTextEdit _readOnlyRepository;
      private readonly RepositoryItemTextEdit _stantdardEditRepository = new RepositoryItemTextEdit();
      private IGridViewColumn _colName;
      private IGridViewColumn _colValue;
      private readonly RepositoryItem _disabledRemoveButtonRepository = new UxRemoveButtonRepository();
      private readonly ScreenBinder<IParameter> _molWeightBinder;
      private readonly ScreenBinder<IParameter> _lowerLimitOfQuantificationBinder;

      public DataRepositoryMetaDataView()
      {
         InitializeComponent();
         _gridViewBinder = new GridViewBinder<MetaDataDTO>(gridView);
         _molWeightBinder = new ScreenBinder<IParameter>();
         _lowerLimitOfQuantificationBinder = new ScreenBinder<IParameter>();
         gridView.AllowsFiltering = false;
         _repositoryForPredefinedValues = new UxRepositoryItemComboBox(gridView);
         _readOnlyRepository = new RepositoryItemTextEdit {ReadOnly = true};
         gridView.ShowingEditor += (o, e) => OnEvent(onShowingEditor, e);
         gridView.RowCellStyle += (o, e) => OnEvent(updateRowCellStyle, e);
         _disabledRemoveButtonRepository.Enabled = false;
         ActiveControl = gridControl;
      }

      private void onShowingEditor(CancelEventArgs e)
      {
         //make sure cell does not get the focus
         var dto = _gridViewBinder.FocusedElement;
         if (dto == null) return;

         if (gridView.FocusedColumn == _colName.XtraColumn)
            e.Cancel = !dto.NameEditable;

         else if (gridView.FocusedColumn == _colValue.XtraColumn)
            e.Cancel = dto.ValueReadOnly;
      }

      public void AttachPresenter(IDataRepositoryMetaDataPresenter presenter)
      {
         _presenter = presenter;
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _colName = _gridViewBinder.AutoBind(x => x.Name)
            .WithCaption(Captions.Name)
            .WithRepository(nameRepository)
            .WithOnValueUpdating(onNameChanged);

         _colValue = _gridViewBinder.AutoBind(x => x.Value)
            .WithCaption(Captions.Value)
            .WithRepository(valueRepository)
            .WithShowButton(ShowButtonModeEnum.ShowAlways)
            .WithEditorConfiguration(configureValueRepository)
            .WithOnValueUpdating(onValueChanged);

         _gridViewBinder.AddUnboundColumn()
            .WithCaption(UIConstants.EMPTY_COLUMN)
            .WithShowButton(ShowButtonModeEnum.ShowAlways)
            .WithRepository(removeRepository)
            .WithFixedWidth(UIConstants.Size.EMBEDDED_BUTTON_WIDTH);

         _molWeightBinder.Bind(x => x.ValueInDisplayUnit)
            .To(tbMolWeight)
            .WithFormat(dto => new UnitFormatter(dto.DisplayUnit))
            .OnValueUpdating += (o, e) => OnEvent(() => _presenter.SetMolWeight(e.OldValue, e.NewValue));

         _lowerLimitOfQuantificationBinder.Bind(x => x.ValueInDisplayUnit)
            .To(tbLowerLimitOfQuantification)
            .WithFormat(dto => new UnitFormatter(dto.DisplayUnit));

         btnAddRow.Click += (o, e) => OnEvent(_presenter.NewMetaDataAdded);
         _removeButtonRepository.ButtonClick += (o, e) => OnEvent(_presenter.RemoveMetaData, _gridViewBinder.FocusedElement);
      }

      private void configureValueRepository(BaseEdit baseEdit, MetaDataDTO dto)
      {
         if (!dto.HasListOfValues)
            return;

         baseEdit.FillComboBoxEditorWith(dto.ListOfValues);
      }

      private RepositoryItem removeRepository(MetaDataDTO dto)
      {
         return dto.NameEditable ? _removeButtonRepository : _disabledRemoveButtonRepository;
      }

      private RepositoryItem nameRepository(MetaDataDTO dto)
      {
         return dto.NameEditable ? _stantdardEditRepository : _readOnlyRepository;
      }

      private RepositoryItem valueRepository(MetaDataDTO dto)
      {
         return dto.ValueReadOnly
            ? _readOnlyRepository
            : dto.HasListOfValues ? _repositoryForPredefinedValues : _stantdardEditRepository;
      }

      private void onNameChanged(MetaDataDTO metaDataDTO, PropertyValueSetEventArgs<string> e)
      {
         _presenter.MetaDataNameChanged(metaDataDTO, e.OldValue);
      }

      private void onValueChanged(MetaDataDTO metaDataDTO, PropertyValueSetEventArgs<string> e)
      {
         _presenter.MetaDataValueChanged(metaDataDTO, e.OldValue);
      }

      public void BindToMetaData(IEnumerable<MetaDataDTO> metaData)
      {
         _gridViewBinder.BindToSource(metaData);
      }

      public void BindToLLOQ(IParameter lowerLimitsOfQuantification)
      {
         layoutItemLowerLimitOfQuantification.Visibility = LayoutVisibilityConvertor.FromBoolean(true);
         _lowerLimitOfQuantificationBinder.BindToSource(lowerLimitsOfQuantification);
      }

      public void BindToMolWeight(IParameter molWeightParameter)
      {
         _molWeightBinder.BindToSource(molWeightParameter);
      }

      public bool MolWeightVisible
      {
         set
         {
            if (layoutItemMolWeight.Visible != value)
               layoutItemMolWeight.Visibility = LayoutVisibilityConvertor.FromBoolean(value);
         }
         get { return layoutItemMolWeight.Visible; }
      }

      public bool MolWeightEditable
      {
         get { return tbMolWeight.Properties.Enabled; }
         set { tbMolWeight.Properties.Enabled = value; }
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         Caption = Captions.MetaData;
         btnAddRow.InitWithImage(ApplicationIcons.Create, text: Captions.AddMetaData);
         layoutItemAddRow.AdjustLargeButtonSize();
         layoutItemMolWeight.Text = Constants.Parameters.MOL_WEIGHT.FormatForLabel();
         layoutItemLowerLimitOfQuantification.Text = Captions.LLOQ.FormatForLabel(checkCase:false);
      }

      private void updateRowCellStyle(RowCellStyleEventArgs e)
      {
         var dto = _gridViewBinder.ElementAt(e.RowHandle);
         if (dto == null) return;

         if (e.Column == _colName.XtraColumn)
            gridView.AdjustAppearance(e, isEnabled: dto.NameEditable);

         else if (e.Column == _colValue.XtraColumn)
            gridView.AdjustAppearance(e, isEnabled: !dto.ValueReadOnly);
      }

      public override ApplicationIcon ApplicationIcon
      {
         get { return ApplicationIcons.Parameters; }
      }

      public override bool HasError
      {
         get { return _gridViewBinder.HasError; }
      }
   }
}