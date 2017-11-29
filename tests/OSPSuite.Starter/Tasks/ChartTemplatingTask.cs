using System.Collections.Generic;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Chart.Mappers;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Starter.Tasks
{
   internal class ChartTemplatingTask : Presentation.Services.Charts.ChartTemplatingTask
   {
      public ChartTemplatingTask(IApplicationController applicationController, IChartTemplatePersistor chartTemplatePersistor, ICloneManager cloneManager, ICurveChartToCurveChartTemplateMapper chartTemplateMapper, IChartFromTemplateService chartFromTemplateService, IChartUpdater chartUpdater, IDialogCreator dialogCreator)
         : base(applicationController, chartTemplatePersistor, cloneManager, chartTemplateMapper, chartFromTemplateService, chartUpdater, dialogCreator)
      {
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