using System.Linq;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.Utility.Extensions;
using DevExpress.XtraEditors.Controls;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Presenters.SensitivityAnalyses;
using OSPSuite.Presentation.Views;
using OSPSuite.Presentation.Views.SensitivityAnalyses;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Services;

namespace OSPSuite.UI.Views.SensitivityAnalyses
{
   public partial class SensitivityAnalysisParameterSelectionView : BaseUserControl, ISensitivityAnalysisParameterSelectionView
   {
      private ISensitivityAnalysisParameterSelectionPresenter _presenter;
      private readonly IImageListRetriever _imageListRetriever;
      private ScreenBinder<ISensitivityAnalysisParameterSelectionPresenter> _screenBinder;

      public SensitivityAnalysisParameterSelectionView(IImageListRetriever imageListRetriever)
      {
         _imageListRetriever = imageListRetriever;
         InitializeComponent();
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         layoutItemAddParameters.AsAddButton();
         layoutItemRemoveParameters.AsRemoveButton();
         layoutItemAddAllConstantParameters.AsAddButton(Captions.SensitivityAnalysis.AddAllConstants);
         layoutItemSelectSimulation.TextVisible = false;
         cbSimulationSelector.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
         btnAddAllConstantParameters.ToolTip = Captions.SensitivityAnalysis.AddAllConstantsDescription;
         cbSimulationSelector.SetImages(_imageListRetriever);
         _screenBinder = new ScreenBinder<ISensitivityAnalysisParameterSelectionPresenter>();
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         btnAddParameters.Click += (o, e) => OnEvent(() => _presenter.AddSelectedParameters());
         btnRemoveParameters.Click += (o, e) => OnEvent(() => _presenter.RemoveParameters());
         btnAddAllConstantParameters.Click += (o, e) => OnEvent(() => _presenter.AddAllConstantParameters());

         _screenBinder.Bind(x => x.Simulation).To(cbSimulationSelector)
            .WithImages(x => _presenter.IconFor(x))
            .WithValues(x => x.AllSimulations)
            .OnValueUpdating += (o, e) => OnEvent(changeSimulation);
      }

      private void changeSimulation()
      {
         if (_presenter.ValidateSimulationChange(cbSimulationSelector.EditValue.DowncastTo<ISimulation>()))
            _presenter.ChangeSimulation(cbSimulationSelector.SelectedItem.DowncastTo<ImageComboBoxItem>().Value.DowncastTo<ISimulation>());

         selectActiveSimulation(_presenter.Simulation);
      }

      public void AttachPresenter(ISensitivityAnalysisParameterSelectionPresenter presenter)
      {
         _presenter = presenter;
      }

      public void AddSimulationParametersView(IView view)
      {
         pnlParameterSelection.FillWith(view);
      }

      private void selectActiveSimulation(ISimulation simulation)
      {
         cbSimulationSelector.SelectedItem = cbSimulationSelector.Properties.Items.FirstOrDefault(x => Equals(x.Value, simulation));
      }

      public void AddSensitivityParametersView(IView view)
      {
         pnlSensitivityParameters.FillWith(view);
      }

      public void BindTo(SensitivityAnalysisParameterSelectionPresenter sensitivityAnalysisParameterSelectionPresenter)
      {
         _screenBinder.BindToSource(sensitivityAnalysisParameterSelectionPresenter);
      }

      public override ApplicationIcon ApplicationIcon => ApplicationIcons.Parameter;
      public override string Caption => Captions.SensitivityAnalysis.ParameterSelection;

   }
}