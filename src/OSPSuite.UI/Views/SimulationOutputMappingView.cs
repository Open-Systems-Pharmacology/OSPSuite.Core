using System.Collections.Generic;
using DevExpress.Utils;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Base;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Data;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.DTO.ParameterIdentifications;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.Importer;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.RepositoryItems;
using OSPSuite.Utility.Format;

namespace OSPSuite.UI.Views
{
   public partial class SimulationOutputMappingView : BaseUserControl, ISimulationOutputMappingView
   {
      private ISimulationOutputMappingPresenter _presenter;
      private readonly GridViewBinder<SimulationOutputMappingDTO> _gridViewBinder;
      private readonly RepositoryItemButtonEdit _removeButtonRepository = new UxRemoveButtonRepository();
      private readonly UxRepositoryItemComboBox _outputRepository;
      private readonly UxRepositoryItemComboBox _singleObservedDataRepository;
      private readonly UxRepositoryItemScalings _scalingRepository;

      public SimulationOutputMappingView()
      {
         InitializeComponent();
         gridView.AllowsFiltering = false;
         _gridViewBinder = new GridViewBinder<SimulationOutputMappingDTO>(gridView);
         _outputRepository = new UxRepositoryItemComboBox(gridView)
         {
            AllowNullInput = DefaultBoolean.True,
            NullText = Captions.SimulationUI.NoneEditorNullText
         };
         _singleObservedDataRepository = new UxRepositoryItemComboBox(gridView);
         _scalingRepository = new UxRepositoryItemScalings(gridView);
      }

      public void AttachPresenter(ISimulationOutputMappingPresenter presenter)
      {
         _presenter = presenter;
      }

      public void BindTo(IEnumerable<SimulationOutputMappingDTO> outputMappingList) //THIS IS NOT EXACTLY CORRECT, WE ARE NOT BINDING TO THIS 
      {
         _gridViewBinder.BindToSource(outputMappingList);
      }

      public void RefreshGrid()
      {
         gridView.RefreshData();
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();

         _gridViewBinder.AutoBind(x => x.ObservedData) 
            .WithRepository(singleObservedDataRepository)
            .WithFormat(observedDataDisplay)
            .WithCaption(Captions.SimulationUI.ObservedData)
            .AsReadOnly();

         _gridViewBinder.AutoBind(x => x.Output)
            .WithRepository(allOutputsRepository)
            .WithShowButton(ShowButtonModeEnum.ShowAlways)
            .WithCaption(Captions.SimulationUI.Outputs);

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

         _removeButtonRepository.ButtonClick += (o, e) => OnEvent(() => _presenter.RemoveOutputMapping(_gridViewBinder.FocusedElement));
      }

      
      private RepositoryItemComboBox singleObservedDataRepository(SimulationOutputMappingDTO outputMappingDTO)
      {
         _singleObservedDataRepository.FillComboBoxRepositoryWith(outputMappingDTO.ObservedData);
         return _singleObservedDataRepository;
      }

      private IFormatter<DataRepository> observedDataDisplay(SimulationOutputMappingDTO outputMappingDTO) => new WeightedObservedDataFormatter(outputMappingDTO);

      private RepositoryItem allOutputsRepository(SimulationOutputMappingDTO dto)
      {
         return RepositoryItemFor(_presenter.AllAvailableOutputs, _outputRepository);
      }

      //this is only called once, not sure we really need it...we could also pass the single value of the observed data here, but seems an overkill
      protected RepositoryItem RepositoryItemFor<T>(IEnumerable<T> allItems, UxRepositoryItemComboBox listRepositoryItems)
      {
         listRepositoryItems.FillComboBoxRepositoryWith(allItems);
         return listRepositoryItems;
      }

      public override string Caption => Captions.SimulationUI.DataSelection;
   }
}
