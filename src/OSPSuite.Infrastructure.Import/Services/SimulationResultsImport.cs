using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;

namespace OSPSuite.Infrastructure.Import.Services
{
   public class SimulationResultsImport : ImportLogger
   {
      public virtual SimulationResults SimulationResults { get; }
      public virtual IList<SimulationResultsImportFile> SimulationResultsFiles { get; }

      public SimulationResultsImport()
      {
         SimulationResults = new SimulationResults();
         SimulationResultsFiles = new List<SimulationResultsImportFile>();
      }

      /// <summary>
      ///    Status of import action. Its value indicates whether the import was successful or not
      /// </summary>
      public override NotificationType Status => SimulationResultsFiles.Aggregate(base.Status, (notification, simResultsFile) => notification | simResultsFile.Status);

      public override IEnumerable<string> Log
      {
         get
         {
            var log = new List<string>(base.Log);
            foreach (var simulationResultsFile in SimulationResultsFiles)
            {
               log.AddRange(simulationResultsFile.Log);
            }

            return log;
         }
      }

      public virtual bool HasError => Status.Is(NotificationType.Error);
   }
}