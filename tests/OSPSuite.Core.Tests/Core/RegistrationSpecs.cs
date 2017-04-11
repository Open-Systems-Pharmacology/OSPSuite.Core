using System.Threading;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Helpers;
using OSPSuite.Utility.Container;

namespace OSPSuite.Core
{
   public class When_performing_core_registration : ContextForIntegration<CoreRegister>
   {
      [Observation]
      public void should_be_able_to_retrieve_parameter_identification_run_factories_for_all_registered_mode()
      {
         var parameterIdentificationRunFactory = IoC.Resolve<IParameterIdentificationRunFactory>();
         var cancellationToken = new CancellationToken();
         var parameterIdentification = new ParameterIdentification();

         createFor<MultipleParameterIdentificationRunMode>(parameterIdentification, parameterIdentificationRunFactory, cancellationToken);
         createFor<StandardParameterIdentificationRunMode>(parameterIdentification, parameterIdentificationRunFactory, cancellationToken);
         createFor<CategorialParameterIdentificationRunMode>(parameterIdentification, parameterIdentificationRunFactory, cancellationToken);
      }

      private static void createFor<T>(ParameterIdentification parameterIdentification, IParameterIdentificationRunFactory parameterIdentificationRunFactory, CancellationToken cancellationToken) where T : ParameterIdentificationRunMode, new()
      {
         parameterIdentification.Configuration.RunMode = new T();
         parameterIdentificationRunFactory.CreateFor(parameterIdentification, cancellationToken);
      }
   }
}