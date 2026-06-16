using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Domain;

public abstract class concern_for_Project : ContextSpecification<Project>
{
   protected CurveChartTemplate _chartTemplate;

   protected override void Context()
   {
      _chartTemplate = new CurveChartTemplate {Name = "Template"};
      sut = new TestProject();
   }
}

public class When_adding_a_chart_template_to_a_project : concern_for_Project
{
   protected override void Because()
   {
      sut.AddChartTemplate(_chartTemplate);
   }

   [Observation]
   public void should_mark_the_project_as_changed()
   {
      sut.HasChanged.ShouldBeTrue();
   }
}

public class When_removing_a_chart_template_from_a_project : concern_for_Project
{
   protected override void Context()
   {
      base.Context();
      sut.AddChartTemplate(_chartTemplate);
      sut.HasChanged = false;
   }

   protected override void Because()
   {
      sut.RemoveChartTemplate(_chartTemplate.Name);
   }

   [Observation]
   public void should_mark_the_project_as_changed()
   {
      sut.HasChanged.ShouldBeTrue();
   }
}

public class When_removing_all_chart_templates_from_a_project : concern_for_Project
{
   protected override void Context()
   {
      base.Context();
      sut.AddChartTemplate(_chartTemplate);
      sut.HasChanged = false;
   }

   protected override void Because()
   {
      sut.RemoveAllChartTemplates();
   }

   [Observation]
   public void should_mark_the_project_as_changed()
   {
      sut.HasChanged.ShouldBeTrue();
   }
}
