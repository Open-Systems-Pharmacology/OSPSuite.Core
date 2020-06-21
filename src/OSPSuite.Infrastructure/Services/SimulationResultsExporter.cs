using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OSPSuite.Core.Batch.Mappers;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;

namespace OSPSuite.Infrastructure.Services
{
   public class SimulationResultsExporter : ISimulationResultsExporter
   {
      private readonly IDataRepositoryExportTask _dataRepositoryExportTask;
      private readonly IQuantityPathToQuantityDisplayPathMapper _quantityDisplayPathMapper;
      private readonly ISimulationResultsToBatchSimulationExportMapper _simulationExportMapper;

      public SimulationResultsExporter(IDataRepositoryExportTask dataRepositoryExportTask, IQuantityPathToQuantityDisplayPathMapper quantityDisplayPathMapper, ISimulationResultsToBatchSimulationExportMapper simulationExportMapper)
      {
         _dataRepositoryExportTask = dataRepositoryExportTask;
         _quantityDisplayPathMapper = quantityDisplayPathMapper;
         _simulationExportMapper = simulationExportMapper;
      }

      public Task ExportToCsvAsync(ISimulation simulation, DataRepository results, string fileName)
      {
         var options = new DataColumnExportOptions
         {
            ColumnNameRetriever = x => _quantityDisplayPathMapper.DisplayPathAsStringFor(simulation, x),
            UseDisplayUnit = false
         };
         return _dataRepositoryExportTask.ExportToCsvAsync(results, fileName, options);
      }

      public Task ExportToJsonAsync(ISimulation simulation, DataRepository results, string fileName)
      {
         return Task.Run(() =>
         {
            var exportResults = _simulationExportMapper.MapFrom(simulation, results);
            // serialize JSON directly to a file
            using (var file = File.CreateText(fileName))
            {
               var serializer = new JsonSerializer();
               serializer.Serialize(file, exportResults);
            }
         });
      }
   }
}