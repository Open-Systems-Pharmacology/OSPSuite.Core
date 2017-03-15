using System.Collections.Generic;
using System.IO;
using OSPSuite.Utility;
using OSPSuite.Utility.Collections;
using OSPSuite.Core;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Presentation.Serialization;
using OSPSuite.Presentation.Settings;

namespace OSPSuite.Presentation.Charts
{
   public interface IChartLayoutTemplateRepository : IRepository<ChartEditorLayoutTemplate>
   {
   }

   public class ChartLayoutTemplateRepository : StartableRepository<ChartEditorLayoutTemplate>, IChartLayoutTemplateRepository
   {
      private readonly IApplicationConfiguration _configuration;
      private readonly ICache<string, ChartEditorLayoutTemplate> _allChartLayoutTemplate = new Cache<string, ChartEditorLayoutTemplate>(x => x.Name);
      private readonly DataPersistor _settingsPersistor;

      public ChartLayoutTemplateRepository(IApplicationConfiguration configuration, IOSPSuiteXmlSerializerRepository modellingXmlSerializerRepository)
      {
         _configuration = configuration;
         _settingsPersistor = new DataPersistor(modellingXmlSerializerRepository);
      }

      public override IEnumerable<ChartEditorLayoutTemplate> All()
      {
         Start();
         return _allChartLayoutTemplate;
      }

      protected override void DoStart()
      {
         var directory = new DirectoryInfo(_configuration.ChartLayoutTemplateFolderPath);
         if (!directory.Exists) return;
         foreach (var fileInfo in directory.GetFiles("*.xml", SearchOption.TopDirectoryOnly))
         {
            var chartEditorSettings = _settingsPersistor.Load<ChartEditorAndDisplaySettings>(fileInfo.FullName);
            addTemplate(FileHelper.FileNameFromFileFullPath(fileInfo.FullName), chartEditorSettings);
         }
      }

      private void addTemplate(string name, ChartEditorAndDisplaySettings chartEditorSettings)
      {
         var chartLayoutTemplate = new ChartEditorLayoutTemplate {Name = name, Settings = chartEditorSettings};
         _allChartLayoutTemplate[chartLayoutTemplate.Name] = chartLayoutTemplate;
      }
   }
}