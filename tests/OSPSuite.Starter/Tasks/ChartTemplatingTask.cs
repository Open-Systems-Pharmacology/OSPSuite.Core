using System.Collections.Generic;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Chart.Mappers;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.UI.Controls;

namespace OSPSuite.Starter.Tasks
{
   internal class ChartTemplatingTask : OSPSuite.Presentation.Services.Charts.ChartTemplatingTask
   {
      public ChartTemplatingTask(IApplicationController applicationController, IChartTemplatePersistor chartTemplatePersistor, ICloneManager cloneManager, ICurveChartToCurveChartTemplateMapper chartTemplateMapper, IChartFromTemplateService chartFromTemplateService, IOSPSuiteExecutionContext executionContext)
         : base(applicationController, chartTemplatePersistor, cloneManager, chartTemplateMapper, chartFromTemplateService)
      {
      }

      protected override string AskForInput(string caption, string s, string defaultName, List<string> usedNames)
      {
         return InputBoxDialog.Show("New name", "New Name", string.Empty);
      }

      protected override ICommand ReplaceTemplatesCommand(IWithChartTemplates withChartTemplates, IEnumerable<CurveChartTemplate> curveChartTemplates)
      {
         withChartTemplates.RemoveAllChartTemplates();
         curveChartTemplates.Each(withChartTemplates.AddChartTemplate);

         return new OSPSuiteEmptyCommand<IOSPSuiteExecutionContext>();
      }

      public override ICommand AddChartTemplateCommand(CurveChartTemplate template, IWithChartTemplates withChartTemplates)
      {
         withChartTemplates.AddChartTemplate(template);
         return new OSPSuiteEmptyCommand<IOSPSuiteExecutionContext>();
      }

      public override ICommand UpdateChartTemplateCommand(CurveChartTemplate template, IWithChartTemplates withChartTemplates, string templateName)
      {
         withChartTemplates.AddChartTemplate(template);
         return new OSPSuiteEmptyCommand<IOSPSuiteExecutionContext>();
      }
   }
}