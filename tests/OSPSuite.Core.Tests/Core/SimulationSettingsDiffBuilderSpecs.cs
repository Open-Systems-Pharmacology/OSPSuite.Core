using System.Drawing;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Container;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core
{
   public class When_compareing_similar_simulation_settings : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var outputSchemaFactory = IoC.Resolve<IOutputSchemaFactory>();
         var simSettings1 = new SimulationSettings().WithName("Setting");
         var simSettings2 = new SimulationSettings().WithName("Setting");

         simSettings1.AddChartTemplate(new CurveChartTemplate().WithName("Temp"));
         simSettings1.OutputSchema = outputSchemaFactory.CreateDefault();
         simSettings1.OutputSelections = new OutputSelections();
         simSettings1.OutputSelections.AddOutput(new QuantitySelection("root|comp1|Drug", QuantityType.Drug));
         simSettings1.Solver = new SolverSettings();

         simSettings2.AddChartTemplate(new CurveChartTemplate().WithName("Temp"));
         simSettings2.OutputSchema = outputSchemaFactory.CreateDefault();
         simSettings2.OutputSelections = new OutputSelections();
         simSettings2.OutputSelections.AddOutput(new QuantitySelection("root|comp1|Drug", QuantityType.Drug));
         simSettings2.Solver = new SolverSettings();

         _object1 = simSettings1;
         _object2 = simSettings2;
      }

      [Observation]
      public void should_return_no_differences()
      {
         _report.ShouldBeEmpty();
      }
   }

   public class When_comparing_simulation_settings_with_different_outputs : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var outputSchemaFactory = IoC.Resolve<IOutputSchemaFactory>();
         var simSettings1 = new SimulationSettings().WithName("Setting");
         var simSettings2 = new SimulationSettings().WithName("Setting");

         simSettings1.AddChartTemplate(new CurveChartTemplate().WithName("Temp"));
         simSettings1.OutputSchema = outputSchemaFactory.CreateDefault();
         simSettings1.OutputSelections = new OutputSelections();
         simSettings1.OutputSelections.AddOutput(new QuantitySelection("root|comp1|Drug", QuantityType.Drug));
         simSettings1.Solver = new SolverSettings();

         simSettings2.AddChartTemplate(new CurveChartTemplate().WithName("Temp"));
         simSettings2.OutputSchema = outputSchemaFactory.CreateDefault();
         simSettings2.OutputSelections = new OutputSelections();
         simSettings2.OutputSelections.AddOutput(new QuantitySelection("root|comp1|Metab", QuantityType.Drug));
         simSettings2.Solver = new SolverSettings();

         _object1 = simSettings1;
         _object2 = simSettings2;
      }

      [Observation]
      public void Shoukld_return_the_differences()
      {
         _report.Count.ShouldBeEqualTo(2);
      }
   }

   public class When_compareing_simulation_settings_with_different_output_types : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var outputSchemaFactory = IoC.Resolve<IOutputSchemaFactory>();
         var simSettings1 = new SimulationSettings().WithName("Setting");
         var simSettings2 = new SimulationSettings().WithName("Setting");

         simSettings1.AddChartTemplate(new CurveChartTemplate().WithName("Temp"));
         simSettings1.OutputSchema = outputSchemaFactory.CreateDefault();
         simSettings1.OutputSelections = new OutputSelections();
         simSettings1.OutputSelections.AddOutput(new QuantitySelection("root|comp1|Drug", QuantityType.Drug));
         simSettings1.Solver = new SolverSettings();

         simSettings2.AddChartTemplate(new CurveChartTemplate().WithName("Temp"));
         simSettings2.OutputSchema = outputSchemaFactory.CreateDefault();
         simSettings2.OutputSelections = new OutputSelections();
         simSettings2.OutputSelections.AddOutput(new QuantitySelection("root|comp1|Drug", QuantityType.Observer));
         simSettings2.Solver = new SolverSettings();

         _object1 = simSettings1;
         _object2 = simSettings2;
      }

      [Observation]
      public void Shoukld_return_the_differences()
      {
         _report.Count.ShouldBeEqualTo(1);
      }
   }

   public class When_compareing_simulation_settings_with_different_named_Templates : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var outputSchemaFactory = IoC.Resolve<IOutputSchemaFactory>();
         var simSettings1 = new SimulationSettings().WithName("Setting");
         var simSettings2 = new SimulationSettings().WithName("Setting");

         simSettings1.AddChartTemplate(new CurveChartTemplate().WithName("Temp"));
         simSettings1.OutputSchema = outputSchemaFactory.CreateDefault();
         simSettings1.OutputSelections = new OutputSelections();
         simSettings1.OutputSelections.AddOutput(new QuantitySelection("root|comp1|Drug", QuantityType.Drug));
         simSettings1.Solver = new SolverSettings();

         simSettings2.AddChartTemplate(new CurveChartTemplate().WithName("Tump"));
         simSettings2.OutputSchema = outputSchemaFactory.CreateDefault();
         simSettings2.OutputSelections = new OutputSelections();
         simSettings2.OutputSelections.AddOutput(new QuantitySelection("root|comp1|Drug", QuantityType.Drug));
         simSettings2.Solver = new SolverSettings();

         _object1 = simSettings1;
         _object2 = simSettings2;
      }

      [Observation]
      public void Shoukld_return_the_differences()
      {
         _report.Count.ShouldBeEqualTo(2);
      }
   }

   public class When_compareing_simulation_settings_with_different_Solver_Setting_options : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var outputSchemaFactory = IoC.Resolve<IOutputSchemaFactory>();
         var solverSettingsFactory = IoC.Resolve<ISolverSettingsFactory>();
         var simSettings1 = new SimulationSettings().WithName("Setting");
         var simSettings2 = new SimulationSettings().WithName("Setting");

         simSettings1.AddChartTemplate(new CurveChartTemplate().WithName("Temp"));
         simSettings1.OutputSchema = outputSchemaFactory.CreateDefault();
         simSettings1.OutputSelections = new OutputSelections();
         simSettings1.OutputSelections.AddOutput(new QuantitySelection("root|comp1|Drug", QuantityType.Drug));
         simSettings1.Solver = solverSettingsFactory.CreateCVODE();
         simSettings1.Solver.HMin = 1;

         simSettings2.AddChartTemplate(new CurveChartTemplate().WithName("Temp"));
         simSettings2.OutputSchema = outputSchemaFactory.CreateDefault();
         simSettings2.OutputSelections = new OutputSelections();
         simSettings2.OutputSelections.AddOutput(new QuantitySelection("root|comp1|Drug", QuantityType.Drug));
         simSettings2.Solver = solverSettingsFactory.CreateCVODE();

         _object1 = simSettings1;
         _object2 = simSettings2;
      }

      [Observation]
      public void Shoukld_return_the_differences()
      {
         _report.Count.ShouldBeEqualTo(1);
      }
   }

   public class When_compareing_simulation_settings_with_different_Chart_Templates : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var outputSchemaFactory = IoC.Resolve<IOutputSchemaFactory>();
         var simSettings1 = new SimulationSettings().WithName("Setting");
         var simSettings2 = new SimulationSettings().WithName("Setting");

         var chartTemplate = new CurveChartTemplate {ChartSettings = {BackColor = Color.AliceBlue}}.WithName("Temp");
         var curveTemplate = new CurveTemplate {Name = "Tada", CurveOptions = {Symbol = Symbols.Circle}};
         chartTemplate.Curves.Add(curveTemplate);

         simSettings1.AddChartTemplate(chartTemplate);
         simSettings1.OutputSchema = outputSchemaFactory.CreateDefault();
         simSettings1.OutputSelections = new OutputSelections();
         simSettings1.OutputSelections.AddOutput(new QuantitySelection("root|comp1|Drug", QuantityType.Drug));
         simSettings1.Solver = new SolverSettings();

         chartTemplate = new CurveChartTemplate {ChartSettings = {BackColor = Color.Aquamarine}}.WithName("Temp");
         curveTemplate = new CurveTemplate {Name = "Tada", CurveOptions = {Symbol = Symbols.Diamond}};
         chartTemplate.Curves.Add(curveTemplate);

         simSettings2.AddChartTemplate(chartTemplate);
         simSettings2.OutputSchema = outputSchemaFactory.CreateDefault();
         simSettings2.OutputSelections = new OutputSelections();
         simSettings2.OutputSelections.AddOutput(new QuantitySelection("root|comp1|Drug", QuantityType.Drug));
         simSettings2.Solver = new SolverSettings();

         _object1 = simSettings1;
         _object2 = simSettings2;
      }

      [Observation]
      public void Shoukld_return_the_differences()
      {
         _report.Count.ShouldBeEqualTo(2);
      }
   }

   public class When_compareing_simulation_settings_with_different_OutPutIntervals : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var outputSchemaFactory = IoC.Resolve<IOutputSchemaFactory>();
         var simSettings1 = new SimulationSettings().WithName("Setting");
         var simSettings2 = new SimulationSettings().WithName("Setting");

         var chartTemplate = new CurveChartTemplate() {ChartSettings = {BackColor = Color.Aquamarine}}.WithName("Temp");

         var curveTemplate = new CurveTemplate() {Name = "Tada"};
         curveTemplate.CurveOptions.Symbol = Symbols.Circle;

         chartTemplate.Curves.Add(curveTemplate);
         simSettings1.AddChartTemplate(chartTemplate);
         simSettings1.OutputSchema = outputSchemaFactory.CreateDefault();
         simSettings1.OutputSelections = new OutputSelections();
         simSettings1.OutputSelections.AddOutput(new QuantitySelection("root|comp1|Drug", QuantityType.Drug));
         simSettings1.Solver = new SolverSettings();

         chartTemplate = new CurveChartTemplate() {ChartSettings = {BackColor = Color.Aquamarine}}.WithName("Temp");

         curveTemplate = new CurveTemplate() {Name = "Tada"};
         curveTemplate.CurveOptions.Symbol = Symbols.Circle;

         chartTemplate.Curves.Add(curveTemplate);

         simSettings2.AddChartTemplate(chartTemplate);
         simSettings2.OutputSchema = outputSchemaFactory.Create(10, 120, 10);
         simSettings2.OutputSelections = new OutputSelections();
         simSettings2.OutputSelections.AddOutput(new QuantitySelection("root|comp1|Drug", QuantityType.Drug));
         simSettings2.Solver = new SolverSettings();

         _object1 = simSettings1;
         _object2 = simSettings2;
      }

      [Observation]
      public void should_return_the_differences()
      {
         _report.Count.ShouldBeEqualTo(3);
      }
   }
}