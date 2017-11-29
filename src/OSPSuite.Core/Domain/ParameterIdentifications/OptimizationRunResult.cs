using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.ParameterIdentifications
{
   public class OptimizationRunResult
   {
      /// <summary>
      ///    List of simulation results. One <see cref="DataRepository" /> per <see cref="ISimulation" /> defined in the
      ///    <see cref="ParameterIdentification" />
      /// </summary>
      private readonly List<DataRepository> _simulationResults = new List<DataRepository>();

      /// <summary>
      ///    Parameter values used to produce the simulation results
      /// </summary>
      private readonly List<OptimizedParameterValue> _values = new List<OptimizedParameterValue>();

      public ResidualsResult ResidualsResult { get; set; }

      public void AddResult(DataRepository dataRepository)
      {
         _simulationResults.Add(dataRepository);
      }

      public void AddValue(OptimizedParameterValue value)
      {
         _values.Add(value);
      }

      public virtual IReadOnlyList<OptimizedParameterValue> Values
      {
         get => _values;
         set
         {
            _values.Clear();
            _values.AddRange(value);
         }
      }

      public virtual IReadOnlyList<DataRepository> SimulationResults
      {
         get => _simulationResults;
         set
         {
            _simulationResults.Clear();
            _simulationResults.AddRange(value);
            updateResultsOrigin();
         }
      }

      private void updateResultsOrigin()
      {
         _simulationResults.SelectMany(x => x.AllButBaseGrid()).Each(x => x.DataInfo.Origin = ColumnOrigins.CalculationAuxiliary);
      }

      public virtual double TotalError => ResidualsResult?.TotalError ?? double.PositiveInfinity;

      public virtual IReadOnlyCollection<OutputResiduals> AllOutputResiduals => ResidualsResult?.AllOutputResiduals ?? new List<OutputResiduals>();

      public virtual IReadOnlyList<Residual> AllResiduals => ResidualsResult?.AllResiduals ?? new List<Residual>();

      public virtual IReadOnlyList<double> AllResidualValues
      {
         get { return AllOutputResiduals.SelectMany(y => y).Select(y => y.Value).ToArray(); }
      }

      public virtual DataColumn SimulationResultFor(string fullOutputPath)
      {
         return SimulationResults.SelectMany(x => x.Columns).FirstOrDefault(x => string.Equals(x.PathAsString, fullOutputPath));
      }

      public virtual void RemoveResidual(OutputResiduals residual) => ResidualsResult?.RemoveResidual(residual);

      public virtual IEnumerable<Residual> AllResidualsFor(string fullOutputPath)
      {
         return ResidualsResult.AllOutputResidualsFor(fullOutputPath).SelectMany(x => x.Residuals);
      }
   }
}