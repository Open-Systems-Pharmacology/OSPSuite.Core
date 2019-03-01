using System.Collections.Generic;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Base;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Presentation.DTO.ParameterIdentifications;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Views.ParameterIdentifications;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.RepositoryItems;
using OSPSuite.Utility.Format;

namespace OSPSuite.UI.Views.ParameterIdentifications
{
   public partial class ParameterIdentificationOutputMappingView : BaseUserControl, IParameterIdentificationOutputMappingView
   {
      private IParameterIdentificationOutputMappingPresenter _presenter;
      private readonly GridViewBinder<OutputMappingDTO> _gridViewBinder;
      private readonly RepositoryItemButtonEdit _removeButtonRepository = new UxRemoveButtonRepository();
      private readonly UxRepositoryItemComboBox _outputRepository;
      private readonly UxRepositoryItemComboBox _observedDataRepository;
      private readonly UxRepositoryItemScalings _scalingRepository;

      public ParameterIdentificationOutputMappingView()
      {
         InitializeComponent();
         gridView.AllowsFiltering = false;
         _gridViewBinder = new GridViewBinder<OutputMappingDTO>(gridView);
         _outputRepository = new UxRepositoryItemComboBox(gridView);
         _observedDataRepository = new UxRepositoryItemComboBox(gridView);
         _scalingRepository = new UxRepositoryItemScalings(gridView);
      }

      public void AttachPresenter(IParameterIdentificationOutputMappingPresenter presenter)
      {
         _presenter = presenter;
      }

      public void BindTo(IEnumerable<OutputMappingDTO> outputMappingList)
      {
         _gridViewBinder.BindToSource(outputMappingList);
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();

         _gridViewBinder.AutoBind(x => x.Output)
            .WithRepository(allOutputsRepository)
            .WithShowButton(ShowButtonModeEnum.ShowAlways)
            .WithCaption(Captions.ParameterIdentification.Outputs)
            .WithOnValueUpdating((dto, e) => OnEvent(() => _presenter.OutputSelectionChanged(dto, e.NewValue, e.OldValue)));

         _gridViewBinder.AutoBind(x => x.ObservedData)
            .WithRepository(allObservedDataRepository)
            .WithFormat(observedDataDisplay)
            .WithShowButton(ShowButtonModeEnum.ShowAlways)
            .WithCaption(Captions.ParameterIdentification.ObservedData)
            .WithOnValueUpdating((dto, e) => OnEvent(() => _presenter.ObservedDataSelectionChanged(dto, e.NewValue, e.OldValue)));

         _gridViewBinder.Bind(x => x.Scaling)
            .WithCaption(Captions.Scaling)
            .WithRepository(x => _scalingRepository)
            .WithShowButton(ShowButtonModeEnum.ShowAlways);

         _gridViewBinder.Bind(x => x.Weight);

         _gridViewBinder.AddUnboundColumn()
            .WithCaption(UIConstants.EMPTY_COLUMN)
            .WithShowButton(ShowButtonModeEnum.ShowAlways)
            .WithRepository(x => _removeButtonRepository)
            .WithFixedWidth(UIConstants.Size.EMBEDDED_BUTTON_WIDTH);

         _gridViewBinder.Changed += NotifyViewChanged;

         btnAddOutput.Click += (o, e) => OnEvent(_presenter.AddOutputMapping);
         _removeButtonRepository.ButtonClick += (o, e) => OnEvent(() => _presenter.RemoveOutputMapping(_gridViewBinder.FocusedElement));

         gridView.FocusedRowChanged += (o, e) => OnEvent(gridViewRowChanged, e);

      }

      private IFormatter<DataRepository> observedDataDisplay(OutputMappingDTO outputMappingDTO) => new WeightedObservedDataFormatter(outputMappingDTO);

      private void gridViewRowChanged(FocusedRowChangedEventArgs e)
      {
         var selectedItem = _gridViewBinder.ElementAt(e.FocusedRowHandle);
         if (selectedItem == null) return;
         _presenter.Select(selectedItem);
      }

      private RepositoryItem allOutputsRepository(OutputMappingDTO dto)
      {
         return RepositoryItemFor(_presenter.AllAvailableOutputs, _outputRepository);
      }

      private RepositoryItem allObservedDataRepository(OutputMappingDTO dto)
      {
         return RepositoryItemFor(_presenter.AllObservedDataFor(dto), _observedDataRepository);
      }

      protected RepositoryItem RepositoryItemFor<T>(IEnumerable<T> allItems, UxRepositoryItemComboBox listRepositoryItems)
      {
         listRepositoryItems.FillComboBoxRepositoryWith(allItems);
         return listRepositoryItems;
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         layoutItemGridOutputs.TextVisible = false;
         layoutItemAddOutput.AdjustLargeButtonSize();
         btnAddOutput.InitWithImage(ApplicationIcons.Add, Captions.ParameterIdentification.AddOutput);
      }

      public override bool HasError => _gridViewBinder.HasError;

      public void CloseEditor()
      {
         gridView.CloseEditor();
      }
   }
}