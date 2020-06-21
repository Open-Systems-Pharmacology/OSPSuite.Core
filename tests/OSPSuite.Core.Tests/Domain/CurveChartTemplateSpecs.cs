using System.Drawing;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_CurveChartTemplate : ContextSpecification<CurveChartTemplate>
   {
      protected override void Context()
      {
         sut = new CurveChartTemplate();
      }
   }

   public class When_upading_a_curve_chart_template_from_another_one : concern_for_CurveChartTemplate
   {
      private ICloneManager _cloneManager;
      private CurveChartTemplate _sourceTemplate;
      private CurveTemplate _cloneCurve;

      protected override void Context()
      {
         base.Context();
         _cloneManager= A.Fake<ICloneManager>();
         _sourceTemplate = new CurveChartTemplate {Name = "TOTO",ChartSettings = {BackColor = Color.Red}};
         _sourceTemplate.AddAxis(new Axis(AxisTypes.X));
         var curveTemplate = new CurveTemplate{Name = "XX"};
         _cloneCurve = new CurveTemplate();
         _sourceTemplate.Curves.Add(curveTemplate);
         A.CallTo(() => _cloneManager.Clone(curveTemplate)).Returns(_cloneCurve);

      }
      protected override void Because()
      {
         sut.UpdatePropertiesFrom(_sourceTemplate,_cloneManager);
      }

      [Observation]
      public void should_have_updated_the_basic_properties()
      {
         sut.ChartSettings.BackColor.ShouldBeEqualTo(Color.Red);
         sut.Name.ShouldBeEqualTo("TOTO");
      }

      [Observation]
      public void should_have_created_a_clone_of_the_axis_properties()
      {
         sut.Axes.Count.ShouldBeEqualTo(1);
      }

      [Observation]
      public void should_have_created_a_clone_of_the_curve_properties()
      {
         sut.Curves.ShouldContain(_cloneCurve);
      }
   }

   public class When_adding_a_new_axis_by_type_to_a_curve_chart_template : concern_for_CurveChartTemplate
   {
      protected override void Because()
      {
         sut.AddNewAxisFor(AxisTypes.Y);
      }

      [Observation]
      public void should_have_an_axis_with_this_type()
      {
         sut.HasAxis(AxisTypes.Y).ShouldBeTrue();
      }
   }
}	