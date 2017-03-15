using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Views.ParameterIdentifications;
using OSPSuite.UI.Controls;

namespace OSPSuite.UI.Views.ParameterIdentifications
{
   public partial class StandardParameterIdentificationRunModeView : BaseUserControl, IStandardParameterIdentificationRunModeView
   {
      public StandardParameterIdentificationRunModeView()
      {
         InitializeComponent();
      }

      public void AttachPresenter(IStandardParameterIdentificationRunModePresenter presenter)
      {
         
      }
   }
}
