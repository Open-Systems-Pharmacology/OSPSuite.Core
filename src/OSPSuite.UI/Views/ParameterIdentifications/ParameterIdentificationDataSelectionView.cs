using OSPSuite.Assets;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Views;
using OSPSuite.Presentation.Views.ParameterIdentifications;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Views.ParameterIdentifications
{
   public partial class ParameterIdentificationDataSelectionView : BaseUserControl, IParameterIdentificationDataSelectionView
   {
      private IParameterIdentificationDataSelectionPresenter _presenter;

      public ParameterIdentificationDataSelectionView()
      {
         InitializeComponent();
      }

      public void AttachPresenter(IParameterIdentificationDataSelectionPresenter presenter)
      {
         _presenter = presenter;
      }

      public void AddSimulationSelectionView(IView view)
      {
         panelSimulationSelection.FillWith(view);
      }

      public void AddOutputMappingView(IView view)
      {
         panelOutputMapping.FillWith(view);
      }

      public void AddWeightedObservedDataCollectorView(IView view)
      {
         panelObservedData.FillWith(view);
      }

      public override string Caption => Captions.ParameterIdentification.DataSelection;

      public override ApplicationIcon ApplicationIcon => ApplicationIcons.Simulation;
   }
}