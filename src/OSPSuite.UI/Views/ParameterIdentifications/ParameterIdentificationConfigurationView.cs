using OSPSuite.Assets;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.Core.Domain.ParameterIdentifications.Algorithms;
using OSPSuite.Presentation.DTO.ParameterIdentifications;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Views;
using OSPSuite.Presentation.Views.ParameterIdentifications;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Views.ParameterIdentifications
{
   public partial class ParameterIdentificationConfigurationView : BaseUserControl, IParameterIdentificationConfigurationView
   {
      private IParameterIdentificationConfigurationPresenter _presenter;
      private ScreenBinder<ParameterIdentificationConfigurationDTO> _screenBinder;
      private ScreenBinder<IParameterIdentificationConfigurationPresenter> _presenterBinder;

      public ParameterIdentificationConfigurationView()
      {
         InitializeComponent();
      }

      public void AttachPresenter(IParameterIdentificationConfigurationPresenter presenter)
      {
         _presenter = presenter;
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _screenBinder = new ScreenBinder<ParameterIdentificationConfigurationDTO>();
         _presenterBinder = new ScreenBinder<IParameterIdentificationConfigurationPresenter>();

         _screenBinder.Bind(x => x.LLOQMode)
            .To(cbLLOQMode)
            .WithValues(x => _presenter.AllLLOQModes)
            .AndDisplays(x => x.DisplayName);

         _screenBinder.Bind(x => x.LLOQModeDescription)
            .To(lblLLOQModeDescription);

         _screenBinder.Bind(x => x.RemoveLLOQMode)
            .To(cbLLOQUsage)
            .WithValues(_presenter.AllRemoveLLOQModes)
            .AndDisplays(x => x.DisplayName);

         _screenBinder.Bind(x => x.LLOQUsageDescription)
            .To(lblLLOQUsageDescription);

         _screenBinder.Bind(x => x.CalculateJacobian)
            .To(chkCalculateJacobian)
            .WithCaption(Captions.ParameterIdentification.CalculateSensitivity);

         _screenBinder.Bind(x => x.OptimizationAlgorithm)
            .To(cbAlgorithm)
            .WithValues(x => _presenter.Algorithms)
            .AndDisplays(x => x.DisplayName)
            .OnValueUpdating += (o, e) => OnEvent(() => updateAlgorithm(e.NewValue));

         _presenterBinder.Bind(x => x.RunModePresenter)
            .To(cbOptionSelection)
            .WithValues(x => x.Options())
            .AndDisplays(x => x.RunModeDisplayName);

         _screenBinder.Changed += NotifyViewChanged;
         _presenterBinder.Changed += NotifyViewChanged;
      }

      private void updateAlgorithm(IOptimizationAlgorithm newValue)
      {
         _presenter.ChangeOptimizationAlgorithm(newValue);
      }

      public void BindTo(ParameterIdentificationConfigurationDTO parameterIdentificationConfigurationDTO)
      {
         _screenBinder.BindToSource(parameterIdentificationConfigurationDTO);
         _presenterBinder.BindToSource(_presenter);
      }

      public void AddAlgorithmOptionsView(IExtendedPropertiesView view)
      {
         panelAlgorithmProperties.FillWith(view);
      }

      public void UpdateOptimizationsView(IView view)
      {
         panelOptions.FillWith(view);
      }

      public override string Caption => Captions.ParameterIdentification.Configuration;

      public override ApplicationIcon ApplicationIcon => ApplicationIcons.Settings;

      public override void InitializeResources()
      {
         base.InitializeResources();
         layoutItemLLOQMode.Text = Captions.ParameterIdentification.LLOQMode.FormatForLabel(checkCase: false);
         layoutItemRemoveLLOQMode.Text = Captions.ParameterIdentification.RemoveLLOQMode.FormatForLabel(checkCase: false);
         layoutItemAlgorithmSelection.Text = Captions.ParameterIdentification.Algorithm.FormatForLabel();

         layoutItemLLOQModeDescription.TextVisible = true;
         layoutItemLLOQModeDescription.Text = " ";
         lblLLOQModeDescription.AsDescription();
         
         layoutItemLLOQUsageDescription.TextVisible = true;
         layoutItemLLOQUsageDescription.Text = " ";
         lblLLOQUsageDescription.AsDescription();

         layoutGroupGeneral.Text = Captions.ParameterIdentification.General;
         layoutItemAlgorithmOptions.Text = Captions.ParameterIdentification.AlgorithmParameters.FormatForLabel();

         layoutGroupOptions.Text = Captions.ParameterIdentification.Options;
         layoutItemOptionPanel.TextVisible = false;
         layoutItemOptionSelection.TextVisible = false;
      }
   }
}