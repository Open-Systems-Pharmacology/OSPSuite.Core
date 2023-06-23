using System.Collections.Generic;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;

namespace OSPSuite.Starter.Domain
{
   public class TestSimulation : ModelCoreSimulation, ISimulation
   {
      public double? TotalDrugMassPerBodyWeightFor(string compoundName)
      {
         return null;
      }

      public IEnumerable<CurveChart> Charts
      {
         get { yield break; }
      }

      public OutputMappings OutputMappings { get; set; } = new OutputMappings();
      public DataRepository ResultsDataRepository { get; set; }

      public void RemoveUsedObservedData(DataRepository dataRepository)
      {
      }

      public void RemoveOutputMappings(DataRepository dataRepository)
      {
      }

      public OutputSchema OutputSchema => Settings.OutputSchema;

      public void AddChartTemplate(CurveChartTemplate chartTemplate)
      {
         Settings.AddChartTemplate(chartTemplate);
      }

      public void RemoveChartTemplate(string chartTemplateName)
      {
         Settings.RemoveChartTemplate(chartTemplateName);
      }

      public IEnumerable<CurveChartTemplate> ChartTemplates => Settings.ChartTemplates;

      public CurveChartTemplate DefaultChartTemplate => Settings.DefaultChartTemplate;

      public bool IsLoaded { get; set; }

      public void RemoveAnalysis(ISimulationAnalysis simulationAnalysis)
      {
      }

      public IEnumerable<ISimulationAnalysis> Analyses { get; } = new List<ISimulationAnalysis>();

      public void AddAnalysis(ISimulationAnalysis simulationAnalysis)
      {
      }

      public bool HasUpToDateResults => true;
      public bool ComesFromPKSim => false;

      public bool UsesObservedData(DataRepository observedData)
      {
         return true;
      }

      public bool HasChanged { get; set; }
   }
}