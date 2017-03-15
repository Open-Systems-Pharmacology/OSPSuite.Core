using OSPSuite.Assets;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Views.ParameterIdentifications;
using OSPSuite.UI.Controls;

namespace OSPSuite.UI.Views.ParameterIdentifications
{
   public partial class MultipleParameterIdentificationRunModeView : BaseUserControl, IMultipleParameterIdentificationRunModeView
   {
      private readonly ScreenBinder<MultipleParameterIdentificationRunMode> _screenBinder;

      public MultipleParameterIdentificationRunModeView()
      {
         InitializeComponent();
         _screenBinder = new ScreenBinder<MultipleParameterIdentificationRunMode>();
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
   
         _screenBinder.Bind(x => x.NumberOfRuns)
            .To(tbNumberOfRuns);

         _screenBinder.Changed += NotifyViewChanged;
      }

      public void BindTo(MultipleParameterIdentificationRunMode multipleOptimization)
      {
         _screenBinder.BindToSource(multipleOptimization);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         numberOfRunsControlItem.Text = Captions.ParameterIdentification.NumberOfRuns.FormatForLabel();
      }

      public void AttachPresenter(IMultipleParameterIdentificationRunModePresenter parameterIdentificationRunModePresenter)
      {
         
      }
   }
}
