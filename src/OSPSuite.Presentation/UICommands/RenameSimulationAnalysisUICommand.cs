using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Core;
using OSPSuite.Utility.Events;

namespace OSPSuite.Presentation.UICommands
{
   public class RenameSimulationAnalysisUICommand : RenameObjectBaseUICommand<ISimulationAnalysis>
   {
      public RenameSimulationAnalysisUICommand(IOSPSuiteExecutionContext context, IEventPublisher eventPublisher, IApplicationController applicationController)
         : base(context, eventPublisher, applicationController)
      {
      }

      protected override IEnumerable<string> ForbiddenNamesFor(ISimulationAnalysis analysis)
      {
         return analysis.Analysable?.Analyses.AllNames() ?? Enumerable.Empty<string>();
      }
   }
}
