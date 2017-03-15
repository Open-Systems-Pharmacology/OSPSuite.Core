using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Events;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Presentation.Core;

namespace OSPSuite.Presentation.UICommands
{
   public class RenameParameterIdentificationUICommand : RenameObjectBaseUICommand<ParameterIdentification>
   {
      public RenameParameterIdentificationUICommand(IOSPSuiteExecutionContext context, IEventPublisher eventPublisher, IApplicationController applicationController) 
         : base(context, eventPublisher, applicationController)
      {
      }
      protected override IEnumerable<string> ForbiddenNamesFor(ParameterIdentification parameterIdentification)
      {
         return _context.Project.AllParameterIdentifications.Select(x => x.Name);
      }
   }
}