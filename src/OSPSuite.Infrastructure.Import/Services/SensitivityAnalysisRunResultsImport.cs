using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.SensitivityAnalyses;

namespace OSPSuite.Core.Importer.Services
{
   public class SensitivityAnalysisRunResultsImport : ImportLogger
   {
      public virtual SensitivityAnalysisRunResult SensitivityAnalysisRunResult { get; }
      public virtual IList<PKParameterSensitivitiesImportFile> PKParameterSensitivitiesImportFiles { get; }

      public SensitivityAnalysisRunResultsImport()
      {
         SensitivityAnalysisRunResult = new SensitivityAnalysisRunResult();
         PKParameterSensitivitiesImportFiles = new List<PKParameterSensitivitiesImportFile>();
      }

      /// <summary>
      ///    Status of import action. Its value indicates whether the import was successful or not
      /// </summary>
      public override NotificationType Status => PKParameterSensitivitiesImportFiles.Aggregate(base.Status, (notification, simResultsFile) => notification | simResultsFile.Status);

      public override IEnumerable<string> Log
      {
         get
         {
            var log = new List<string>(base.Log);
            foreach (var sensitivityAnalysisRunResultFile in PKParameterSensitivitiesImportFiles)
            {
               log.AddRange(sensitivityAnalysisRunResultFile.Log);
            }

            return log;
         }
      }
   }
}