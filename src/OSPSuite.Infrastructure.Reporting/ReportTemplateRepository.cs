using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using OSPSuite.Core;
using OSPSuite.Core.Reporting;
using OSPSuite.Utility;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Infrastructure.Reporting
{
   public class ReportTemplateRepository : StartableRepository<ReportTemplate>, IReportTemplateRepository
   {
      private readonly IApplicationConfiguration _configuration;
      private readonly ICache<string, ReportTemplate> _allReportTemplates = new Cache<string, ReportTemplate>(x => x.DisplayName);

      public ReportTemplateRepository(IApplicationConfiguration configuration)
      {
         _configuration = configuration;
      }

      protected override void DoStart()
      {
         var directory = new DirectoryInfo(_configuration.TeXTemplateFolderPath);
         if (!directory.Exists) return;
         foreach (var fileInfo in directory.GetFiles("*.json", SearchOption.AllDirectories))
         {
            var template = templateFrom(fileInfo.FullName);
            if (template == null) continue;
            template.Path = Path.Combine(_configuration.TeXTemplateFolderPath, FileHelper.FileNameFromFileFullPath(fileInfo.FullName));
            _allReportTemplates.Add(template);
         }
      }

      private ReportTemplate templateFrom(string jsonFile)
      {
         var serializer = new JsonSerializer();
         using (var sr = new StreamReader(jsonFile))
         using (var reader = new JsonTextReader(sr))
         {
            return serializer.Deserialize<ReportTemplate>(reader);
         }
      }

      public override IEnumerable<ReportTemplate> All()
      {
         Start();
         return _allReportTemplates;
      }
   }
}