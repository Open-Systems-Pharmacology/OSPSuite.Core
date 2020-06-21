using FakeItEasy;
using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;

namespace OSPSuite.Core.Services
{
   public abstract class concern_for_WatermakStatusChecker : ContextSpecification<IWatermarkStatusChecker>
   {
      protected IApplicationSettings _applicationSettings;
      protected IDialogCreator _dialogCreator;
      protected IApplicationConfiguration _applicationConfiguration;
      protected string _productName = "APP";
      protected string _location = "Settings";

      protected override void Context()
      {
         _applicationSettings= A.Fake<IApplicationSettings>();
         _dialogCreator= A.Fake<IDialogCreator>(); 
         _applicationConfiguration= A.Fake<IApplicationConfiguration>();
         A.CallTo(() => _applicationConfiguration.ProductName).Returns(_productName);
         A.CallTo(() => _applicationConfiguration.WatermarkOptionLocation).Returns(_location);
         sut = new WatermarkStatusChecker(_applicationSettings,_dialogCreator,_applicationConfiguration);
      }
   }

   public class When_checking_the_watermark_status_for_application_settings_already_having_a_value : concern_for_WatermakStatusChecker
   {
      protected override void Context()
      {
         base.Context();
         _applicationSettings.UseWatermark = true;
      }

      protected override void Because()
      {
         sut.CheckWatermarkStatus();
      }

      [Observation]
      public void should_not_ask_the_user_to_update_the_use_watermark_flag()
      {
         A.CallTo(_dialogCreator).WithReturnType<ViewResult>().MustNotHaveHappened();         
      }
   }

   public class When_checking_the_watermark_status_for_application_settings_without_any_defined_values_and_the_user_does_not_want_to_use_watermark : concern_for_WatermakStatusChecker
   {
      protected override void Context()
      {
         base.Context();
         _applicationSettings.UseWatermark = null;
         A.CallTo(() => _dialogCreator.MessageBoxYesNo(Captions.ShouldWatermarkBeUsedForChartExportToClipboard(_productName, _location), ViewResult.No)).Returns(ViewResult.No);
      }

      protected override void Because()
      {
         sut.CheckWatermarkStatus();
      }

      [Observation]
      public void should_not_use_watermark()
      {
         _applicationSettings.UseWatermark.Value.ShouldBeFalse(); 
      }
   }

   public class When_checking_the_watermark_status_for_application_settings_without_any_defined_values_and_the_user_wants_to_use_watermark : concern_for_WatermakStatusChecker
   {
      protected override void Context()
      {
         base.Context();
         _applicationSettings.UseWatermark = null;
         A.CallTo(() => _dialogCreator.MessageBoxYesNo(Captions.ShouldWatermarkBeUsedForChartExportToClipboard(_productName, _location), ViewResult.No)).Returns(ViewResult.Yes);
      }

      protected override void Because()
      {
         sut.CheckWatermarkStatus();
      }

      [Observation]
      public void should_use_watermark()
      {
         _applicationSettings.UseWatermark.Value.ShouldBeTrue();
      }
   }
}	