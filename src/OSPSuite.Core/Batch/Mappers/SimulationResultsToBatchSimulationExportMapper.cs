using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Batch.Mappers
{
   public interface ISimulationResultsToBatchSimulationExportMapper
   {
      BatchSimulationExport MapFrom(ISimulation simulation, DataRepository results);
   }

   public class SimulationResultsToBatchSimulationExportMapper : ISimulationResultsToBatchSimulationExportMapper
   {
      private readonly IObjectPathFactory _objectPathFactory;

      public SimulationResultsToBatchSimulationExportMapper(IObjectPathFactory objectPathFactory)
      {
         _objectPathFactory = objectPathFactory;
      }

      public BatchSimulationExport MapFrom(ISimulation simulation, DataRepository results)
      {
         var simulationExport = new BatchSimulationExport
         {
            Name = simulation.Name,
            Times = displayValuesFor(results.BaseGrid),
            ParameterValues = parameterValuesFor(simulation.Model),
            AbsTol = simulation.SimulationSettings.Solver.AbsTol,
            RelTol = simulation.SimulationSettings.Solver.RelTol,
         };

         results.AllButBaseGrid().Each(c => simulationExport.OutputValues.Add(quantityResultsFrom(c)));

         return simulationExport;
      }

      private List<ParameterValue> parameterValuesFor(IModel simulationModel)
      {
         return simulationModel.Root.GetAllChildren<IParameter>().Select(p => new ParameterValue
         {
            Path = _objectPathFactory.CreateAbsoluteObjectPath(p).PathAsString,
            Value = p.Value
         }).ToList();
      }

      private BatchOutputValues quantityResultsFrom( DataColumn column)
      {
         return new BatchOutputValues
         {
            Path = column.PathAsString,
            //ComparisonThreshold should alwyas have a value. If for some reason it is not set, using 0 ensures that value should be compared exactly
            ComparisonThreshold = column.DataInfo.ComparisonThreshold.GetValueOrDefault(0),
            Values = displayValuesFor(column),
            Dimension = column.Dimension.Name
         };
      }

      private float[] displayValuesFor(DataColumn column)
      {
         return column.ConvertToDisplayValues(column.Values).ToArray();
      }
   }
}