using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OSPSuite.Presentation.UICommands
{
   public class RunMultipleParameterIdentificationsUICommand : ActiveObjectUICommand<IReadOnlyList<ParameterIdentification>>
   {
      private readonly IParameterIdentificationRunner _parameterIdentificationRunner;

      public RunMultipleParameterIdentificationsUICommand(
         IParameterIdentificationRunner parameterIdentificationRunner,
         IActiveSubjectRetriever activeSubjectRetriever)
         : base(activeSubjectRetriever)
      {
         _parameterIdentificationRunner = parameterIdentificationRunner;
      }

      protected override async void PerformExecute()
      {
         var tasks = Subject.Select(pi => _parameterIdentificationRunner.SecureAwait(x => x.Run(pi)));

         await Task.WhenAll(tasks);
      }
   }
}