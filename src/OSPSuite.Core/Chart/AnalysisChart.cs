using System.Collections.Generic;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Chart
{
   public abstract class AnalysisChart : ChartWithObservedData, ISimulationAnalysis
   {
      public IAnalysable Analysable { get; set; }
   }

   public abstract class AnalysisChartWithLocalRepositories : AnalysisChart
   {
      private readonly List<DataRepository> _dataRepositories;
      public virtual IReadOnlyList<DataRepository> DataRepositories => _dataRepositories;

      protected AnalysisChartWithLocalRepositories()
      {
         _dataRepositories = new List<DataRepository>();
      }

      public virtual void AddRepository(DataRepository dataRepository)
      {
         _dataRepositories.Add(dataRepository);
      }

      public virtual void ClearDataRepositories()
      {
         _dataRepositories.Clear();
      }

      public virtual void AddRepositories(IEnumerable<DataRepository> dataRepositories)
      {
         dataRepositories.Each(AddRepository);
      }
   }
}