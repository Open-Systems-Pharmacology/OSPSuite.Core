using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Extensions;

public abstract class concern_for_WithChartTemplatesExtensions : StaticContextSpecification
{
   protected IWithChartTemplates _withChartTemplates;
   protected CurveChartTemplate _timeProfileTemplate;
   protected CurveChartTemplate _defaultTimeProfileTemplate;
   protected CurveChartTemplate _predictedVsObservedTemplate;

   protected override void Context()
   {
      _timeProfileTemplate = new CurveChartTemplate {Name = "A time profile", ChartType = CurveChartTypes.TimeProfile};
      _defaultTimeProfileTemplate = new CurveChartTemplate {Name = "Z time profile", ChartType = CurveChartTypes.TimeProfile, IsDefault = true};
      _predictedVsObservedTemplate = new CurveChartTemplate {Name = "Predicted vs observed", ChartType = CurveChartTypes.PredictedVsObserved};

      _withChartTemplates = A.Fake<IWithChartTemplates>();
      A.CallTo(() => _withChartTemplates.ChartTemplates).Returns(new[] {_timeProfileTemplate, _defaultTimeProfileTemplate, _predictedVsObservedTemplate});
   }
}

public class When_retrieving_the_default_chart_template_for_a_chart_type_with_a_flagged_default : concern_for_WithChartTemplatesExtensions
{
   [Observation]
   public void should_return_the_template_flagged_as_default_among_the_templates_of_this_type()
   {
      _withChartTemplates.DefaultChartTemplateFor(CurveChartTypes.TimeProfile).ShouldBeEqualTo(_defaultTimeProfileTemplate);
   }
}

public class When_retrieving_the_default_chart_template_for_a_chart_type_without_a_flagged_default : concern_for_WithChartTemplatesExtensions
{
   [Observation]
   public void should_return_the_first_template_of_this_type_by_name()
   {
      _withChartTemplates.DefaultChartTemplateFor(CurveChartTypes.PredictedVsObserved).ShouldBeEqualTo(_predictedVsObservedTemplate);
   }
}

public class When_retrieving_the_default_chart_template_for_a_chart_type_without_any_template : concern_for_WithChartTemplatesExtensions
{
   [Observation]
   public void should_return_null()
   {
      _withChartTemplates.DefaultChartTemplateFor(CurveChartTypes.ResidualVsTime).ShouldBeNull();
   }
}
