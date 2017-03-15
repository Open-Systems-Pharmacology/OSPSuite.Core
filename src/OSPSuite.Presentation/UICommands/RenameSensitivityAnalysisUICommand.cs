using System.Collections.Generic;
using OSPSuite.Utility.Events;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.Presentation.Core;

namespace OSPSuite.Presentation.UICommands
{
   public class RenameSensitivityAnalysisUICommand : RenameObjectBaseUICommand<SensitivityAnalysis>
   {
      public RenameSensitivityAnalysisUICommand(IOSPSuiteExecutionContext context, IEventPublisher eventPublisher, IApplicationController applicationController) : base(context, eventPublisher, applicationController)
      {
      }

      protected override IEnumerable<string> ForbiddenNamesFor(SensitivityAnalysis sensitivityAnalysis)
      {
         return _context.Project.AllSensitivityAnalyses.AllNames();
      }
   }
}