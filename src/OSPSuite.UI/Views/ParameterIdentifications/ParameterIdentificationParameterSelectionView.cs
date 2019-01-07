using OSPSuite.Assets;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Views;
using OSPSuite.Presentation.Views.ParameterIdentifications;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Views.ParameterIdentifications
{
   public partial class ParameterIdentificationParameterSelectionView : BaseUserControl, IParameterIdentificationParameterSelectionView
   {
      private IParameterIdentificationParameterSelectionPresenter _presenter;

      public ParameterIdentificationParameterSelectionView()
      {
         InitializeComponent();
      }

      public void AttachPresenter(IParameterIdentificationParameterSelectionPresenter presenter)
      {
         _presenter = presenter;
      }

      public void AddSimulationParametersView(IView view)
      {
         panelSimulationParameters.FillWith(view);
      }

      public void AddIdentificationParametersView(IView view)
      {
         panelIdentificationParameters.FillWith(view);
      }

      public void AddLinkedParametersView(IView view)
      {
         panelLinkedParameters.FillWith(view);
      }

      public string LinkedParametersCaption
      {
         get => layoutItemLinkedParameters.Text;
         set => layoutItemLinkedParameters.Text = value;
      }

      public override string Caption => Captions.ParameterIdentification.ParameterSelection;

      public override ApplicationIcon ApplicationIcon => ApplicationIcons.Parameter;

      public override void InitializeResources()
      {
         base.InitializeResources();
         layoutItemAddIdentificationParameter.AsAddButton();
         layoutItemAddLinkedParameter.AsAddButton();
         layoutItemIdentificationParameters.Text = Captions.ParameterIdentification.IdentificationParameters;
         layoutItemLinkedParameters.Text = Captions.ParameterIdentification.LinkedParameters;
      }

      public override void InitializeBinding()
      {
         btnAddIdentificationParameter.Click += (o, e) => OnEvent(_presenter.AddIdentificationParameter);
         btnAddLinkedParameter.Click += (o, e) => OnEvent(_presenter.AddLinkedParameter);
      }
   }
}