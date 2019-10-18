using System.Collections.Generic;
using System.IO;
using OSPSuite.Core;
using OSPSuite.Core.Serialization;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Presentation.Settings;
using OSPSuite.Utility;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Presentation.Charts
{
   public interface IChartLayoutTemplateRepository : IRepository<ChartEditorLayoutTemplate>
   {
   }

   public class ChartLayoutTemplateRepository : StartableRepository<ChartEditorLayoutTemplate>, IChartLayoutTemplateRepository
   {
      private readonly IApplicationConfiguration _configuration;
      private readonly IDataPersistor _dataPersistor;

      private readonly ICache<string, ChartEditorLayoutTemplate> _allChartLayoutTemplate = new Cache<string, ChartEditorLayoutTemplate>(x => x.Name);

      public ChartLayoutTemplateRepository(IApplicationConfiguration configuration, IDataPersistor dataPersistor)
      {
         _configuration = configuration;
         _dataPersistor = dataPersistor;
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
            var chartEditorSettings = _dataPersistor.Load<ChartEditorAndDisplaySettings>(fileInfo.FullName);
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