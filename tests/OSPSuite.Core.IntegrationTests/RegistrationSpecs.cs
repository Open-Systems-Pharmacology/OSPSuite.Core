using System.Threading;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Core.Services;
using OSPSuite.Infrastructure.Container.Castle;
using OSPSuite.Presentation;
using OSPSuite.Presentation.Mappers;
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

   public class When_performing_presentation_registration : StaticContextSpecification
   {
      private CastleWindsorContainer _container;

      protected override void Context()
      {
         _container = new CastleWindsorContainer();
         _container.RegisterImplementationOf(A.Fake<IPathToPathElementsMapper>());
         _container.RegisterImplementationOf(A.Fake<IDisplayNameProvider>());
         _container.RegisterImplementationOf(A.Fake<IPathAndValueEntityToPathElementsMapper>());
      }

      protected override void Because()
      {
         _container.AddRegister(x => x.FromType<PresenterRegister>());
      }

      [Observation]
      public void should_leave_the_registration_of_the_diff_item_mapper_to_the_application()
      {
         _container.ResolveAll<IDiffItemToDiffItemDTOMapper>().ShouldBeEmpty();
      }
   }
}