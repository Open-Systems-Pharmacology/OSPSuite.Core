using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OSPSuite.Core.Batch.Mappers;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Infrastructure.Services
{
   public class SimulationResultsExporter : ISimulationResultsExporter
   {
      private readonly IDataRepositoryTask _dataRepositoryTask;
      private readonly IQuantityPathToQuantityDisplayPathMapper _quantityDisplayPathMapper;
      private readonly ISimulationResultsToBatchSimulationExportMapper _simulationExportMapper;

      public SimulationResultsExporter(IDataRepositoryTask dataRepositoryTask, IQuantityPathToQuantityDisplayPathMapper quantityDisplayPathMapper, ISimulationResultsToBatchSimulationExportMapper simulationExportMapper)
      {
         _dataRepositoryTask = dataRepositoryTask;
         _quantityDisplayPathMapper = quantityDisplayPathMapper;
         _simulationExportMapper = simulationExportMapper;
      }

      public Task ExportToCsvAsync(ISimulation simulation, DataRepository results, string fileName)
      {
         var dataTable = _dataRepositoryTask.ToDataTable(results, x => _quantityDisplayPathMapper.DisplayPathAsStringFor(simulation, x)).First();
         return Task.Run(() => dataTable.ExportToCSV(fileName));
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