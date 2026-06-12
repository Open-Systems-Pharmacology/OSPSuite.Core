using System.Linq;
using FakeItEasy;
using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Chart.ParameterIdentifications;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Services.Charts;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_ChartTemplateMenuPresenter : ContextSpecification<ChartTemplateMenuPresenter>
   {
      protected IChartTemplatingTask _chartTemplatingTask;
      protected IWithChartTemplates _withChartTemplates;
      protected CurveChartTemplate _timeProfileTemplate;
      protected CurveChartTemplate _predictedVsObservedTemplate;
      protected IMenuBarSubMenu _chartTemplateMenu;

      protected override void Context()
      {
         _chartTemplatingTask = A.Fake<IChartTemplatingTask>();
         _timeProfileTemplate = new CurveChartTemplate {Name = "TimeProfileTemplate", ChartType = CurveChartTypes.TimeProfile};
         _predictedVsObservedTemplate = new CurveChartTemplate {Name = "PredictedVsObservedTemplate", ChartType = CurveChartTypes.PredictedVsObserved};

         _withChartTemplates = A.Fake<IWithChartTemplates>();
         A.CallTo(() => _withChartTemplates.ChartTemplates).Returns(new[] {_timeProfileTemplate, _predictedVsObservedTemplate});

         sut = new ChartTemplateMenuPresenter(_chartTemplatingTask);
      }

      protected IMenuBarSubMenu ApplyTemplateMenu => subMenuByCaption(_chartTemplateMenu, MenuNames.ApplyChartTemplate);

      protected IMenuBarSubMenu UpdateTemplateMenu => subMenuByCaption(subMenuByCaption(_chartTemplateMenu, MenuNames.FromCurrentChart), MenuNames.UpdateExistingTemplate);

      private static IMenuBarSubMenu subMenuByCaption(IMenuBarSubMenu menu, string caption)
      {
         return menu?.AllItems().OfType<IMenuBarSubMenu>().FirstOrDefault(x => string.Equals(x.Caption, caption));
      }
   }

   public class When_creating_the_chart_template_menu_for_a_predicted_vs_observed_chart : concern_for_ChartTemplateMenuPresenter
   {
      protected override void Because()
      {
         _chartTemplateMenu = sut.CreateChartTemplateButton(_withChartTemplates, () => new ParameterIdentificationPredictedVsObservedChart(), template => { });
      }

      [Observation]
      public void should_only_offer_to_apply_templates_created_from_charts_of_the_same_type()
      {
         ApplyTemplateMenu.AllItems().Select(x => x.Caption).ShouldOnlyContain(_predictedVsObservedTemplate.Name);
      }

      [Observation]
      public void should_only_offer_to_update_templates_created_from_charts_of_the_same_type()
      {
         UpdateTemplateMenu.AllItems().Select(x => x.Caption).ShouldOnlyContain(_predictedVsObservedTemplate.Name);
      }
   }

   public class When_creating_the_chart_template_menu_for_a_time_profile_chart : concern_for_ChartTemplateMenuPresenter
   {
      protected override void Because()
      {
         _chartTemplateMenu = sut.CreateChartTemplateButton(_withChartTemplates, () => new CurveChart(), template => { });
      }

      [Observation]
      public void should_only_offer_to_apply_time_profile_templates()
      {
         ApplyTemplateMenu.AllItems().Select(x => x.Caption).ShouldOnlyContain(_timeProfileTemplate.Name);
      }

      [Observation]
      public void should_only_offer_to_update_time_profile_templates()
      {
         UpdateTemplateMenu.AllItems().Select(x => x.Caption).ShouldOnlyContain(_timeProfileTemplate.Name);
      }
   }

   public class When_the_user_manages_the_templates_from_the_chart_template_menu : concern_for_ChartTemplateMenuPresenter
   {
      protected override void Context()
      {
         base.Context();
         _chartTemplateMenu = sut.CreateChartTemplateButton(_withChartTemplates, () => new ParameterIdentificationPredictedVsObservedChart(), template => { });
      }

      protected override void Because()
      {
         _chartTemplateMenu.AllItems().OfType<IMenuBarButton>().First(x => string.Equals(x.Caption, MenuNames.ManageTemplates)).Command.Execute();
      }

      [Observation]
      public void should_manage_the_templates_created_for_the_type_of_the_active_chart()
      {
         A.CallTo(() => _chartTemplatingTask.ManageTemplates(_withChartTemplates, CurveChartTypes.PredictedVsObserved)).MustHaveHappened();
      }
   }

   public class When_creating_the_chart_template_menu_for_a_chart_type_without_any_matching_template : concern_for_ChartTemplateMenuPresenter
   {
      protected override void Because()
      {
         _chartTemplateMenu = sut.CreateChartTemplateButton(_withChartTemplates, () => new ParameterIdentificationResidualVsTimeChart(), template => { });
      }

      [Observation]
      public void should_indicate_that_no_template_is_available()
      {
         ApplyTemplateMenu.AllItems().Select(x => x.Caption).ShouldOnlyContain(MenuNames.NoTemplateAvailable);
      }

      [Observation]
      public void should_not_offer_to_update_any_template()
      {
         UpdateTemplateMenu.ShouldBeNull();
      }
   }
}
