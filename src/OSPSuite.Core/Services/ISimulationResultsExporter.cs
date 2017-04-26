using System.Threading.Tasks;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;

namespace OSPSuite.Core.Services
{
   public interface ISimulationResultsExporter
   {
      Task ExportToCsvAsync(ISimulation simulation, DataRepository results, string fileName);
      Task ExportToJsonAsync(ISimulation simulation, DataRepository results, string fileName);
   }
}