using System.Linq;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Services
{
   public interface ISimulationResultsCreator
   {
      SimulationResults CreateResultsFrom(DataRepository dataRepository);

      SimulationResults CreateResultsWithSensitivitiesFrom(DataRepository dataRepository, ISimModelBatch simModelBatch, string[] parameters);
   }

   public class SimulationResultsCreator : ISimulationResultsCreator
   {
      public SimulationResults CreateResultsFrom(DataRepository dataRepository)
      {
         return createResultsFrom(dataRepository, null, new string[] { });
      }

      public SimulationResults CreateResultsWithSensitivitiesFrom(DataRepository dataRepository, ISimModelBatch simModelBatch, string[] parameters)
      {
         return createResultsFrom(dataRepository, simModelBatch, parameters);
      }

      private SimulationResults createResultsFrom(DataRepository dataRepository, ISimModelBatch simModel, string[] parameters)
      {
         if (dataRepository.IsNull() || dataRepository.BaseGrid == null)
            return new NullSimulationResults();

         var results = new SimulationResults { Time = valuesFrom(dataRepository.BaseGrid, null, new string[] { }) };

         foreach (var columnsForIndividual in dataRepository.AllButBaseGrid().GroupBy(selectIndividualIndex))
         {
            var individualResults = new IndividualResults { IndividualId = columnsForIndividual.Key, Time = results.Time };
            foreach (var dataColumn in columnsForIndividual)
            {
               individualResults.Add(valuesFrom(dataColumn, pathWithoutIndividualIndex(dataColumn, columnsForIndividual.Key), simModel, parameters));
            }

            individualResults.UpdateQuantityTimeReference();
            results.Add(individualResults);
         }

         return results;
      }

      private string pathWithoutIndividualIndex(DataColumn dataColumn, int index)
      {
         if (dataColumn?.QuantityInfo?.Path == null)
            return string.Empty;

         var path = dataColumn.QuantityInfo.Path.ToList();

         if (path.Count == 0)
            return string.Empty;

         if (path.Last() == index.ConvertedTo<string>())
            path.RemoveAt(path.Count - 1);

         //remove simulation name
         if (path.Count > 0)
            path.RemoveAt(0);

         return path.ToPathString();
      }

      private int selectIndividualIndex(DataColumn column)
      {
         if (column?.QuantityInfo?.Path == null)
            return 0;

         var pathList = column.QuantityInfo.Path.ToList();
         if (pathList.Count == 0)
            return 0;

         var lastEntry = pathList.Last();
         int.TryParse(lastEntry, out var index);
         return index;
      }

      private QuantityValues valuesFrom(DataColumn dataColumn, ISimModelBatch simModel, string[] parameters)
      {
         var quantityPath = dataColumn?.QuantityInfo?.PathAsString ?? string.Empty;
         return valuesFrom(dataColumn, quantityPath, simModel, parameters);
      }

      private QuantityValues valuesFrom(DataColumn dataColumn, string quantityPath, ISimModelBatch simModel, string[] parameters)
      {
         return new QuantityValues
         {
            ColumnId = dataColumn?.Id ?? string.Empty,
            QuantityPath = quantityPath ?? string.Empty,
            Values = dataColumn?.Values?.ToArray() ?? new float[0],
            Sensitivities = parameters.ToDictionary(p => p, p => simModel?.SensitivityValuesFor(quantityPath, p))
         };
      }
   }
}