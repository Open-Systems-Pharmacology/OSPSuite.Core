using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Visitor;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain.Builder
{
   public interface ISimulationSettings : IBuildingBlock, IWithChartTemplates
   {
      SolverSettings Solver { get; set; }

      OutputSchema OutputSchema { get; set; }

      double RandomSeed { get; }

      /// <summary>
      ///    Quantities selected that will be exported by the simulation engine
      /// </summary>
      OutputSelections OutputSelections { get; set; }
   }

   public class SimulationSettings : BuildingBlock, ISimulationSettings
   {
      private readonly ICache<string, CurveChartTemplate> _chartTemplates;
      public virtual SolverSettings Solver { get; set; }
      public virtual OutputSchema OutputSchema { get; set; }
      public double RandomSeed { get; set; }
      public virtual OutputSelections OutputSelections { get;  set; }

      public SimulationSettings()
      {
         Icon = IconNames.SIMULATION_SETTINGS;
         RandomSeed = Environment.TickCount;
         _chartTemplates = new Cache<string, CurveChartTemplate>(x => x.Name,x=>null);
         OutputSelections = new OutputSelections();
      }

      public CurveChartTemplate DefaultChartTemplate
      {
         get { return ChartTemplates.FirstOrDefault(x => x.IsDefault) ?? ChartTemplates.OrderBy(x => x.Name).FirstOrDefault(); }
      }

      public CurveChartTemplate ChartTemplateByName(string templateName)
      {
         return _chartTemplates[templateName];
      }

      public void RemoveAllChartTemplates()
      {
         _chartTemplates.Clear();
      }

      public IEnumerable<CurveChartTemplate> ChartTemplates => _chartTemplates;

      public void AddChartTemplate(CurveChartTemplate chartTemplate)
      {
         _chartTemplates[chartTemplate.Name] = chartTemplate;
      }

      public void RemoveChartTemplate(string chartTemplateName)
      {
         _chartTemplates.Remove(chartTemplateName);
      }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);
         var sourceSimulationSettings = source as ISimulationSettings;
         if (sourceSimulationSettings == null) return;
         Solver = cloneManager.Clone(sourceSimulationSettings.Solver);
         OutputSchema = cloneManager.Clone(sourceSimulationSettings.OutputSchema);
         OutputSelections = cloneManager.Clone(sourceSimulationSettings.OutputSelections);
         _chartTemplates.Clear();
         sourceSimulationSettings.ChartTemplates.Each(t => AddChartTemplate(cloneManager.Clone(t)));
      }

      public override void AcceptVisitor(IVisitor visitor)
      {
         base.AcceptVisitor(visitor);
         OutputSchema?.AcceptVisitor(visitor);
         Solver?.AcceptVisitor(visitor);
      }
   }
}