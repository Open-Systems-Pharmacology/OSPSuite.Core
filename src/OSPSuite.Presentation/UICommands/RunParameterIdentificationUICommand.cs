using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Services;

namespace OSPSuite.Presentation.UICommands
{
   public class RunParameterIdentificationUICommand : ActiveObjectUICommand<ParameterIdentification>
   {
      private readonly IParameterIdentificationRunner _parameterIdentificationRunner;

      public RunParameterIdentificationUICommand(IParameterIdentificationRunner parameterIdentificationRunner, IActiveSubjectRetriever activeSubjectRetriever) : base(activeSubjectRetriever)
      {
         _parameterIdentificationRunner = parameterIdentificationRunner;
      }

      protected override async void PerformExecute()
      {
         await _parameterIdentificationRunner.SecureAwait(x => x.Run(Subject));
      }
   }
}