using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Core.Services;

namespace OSPSuite.Presentation.UICommands
{
   public class StopParameterIdentificationUICommand : ActiveObjectUICommand<ParameterIdentification>
   {
      private readonly IParameterIdentificationRunner _parameterIdentificationRunner;

      public StopParameterIdentificationUICommand(IParameterIdentificationRunner parameterIdentificationRunner, IActiveSubjectRetriever activeSubjectRetriever) : base(activeSubjectRetriever)
      {
         _parameterIdentificationRunner = parameterIdentificationRunner;
      }

      protected override void PerformExecute()
      {
         _parameterIdentificationRunner.Stop();
      }
   }
}