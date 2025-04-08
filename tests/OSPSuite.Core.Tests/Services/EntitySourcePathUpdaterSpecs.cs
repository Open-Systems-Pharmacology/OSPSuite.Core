using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Extensions;
using OSPSuite.Helpers;
using static OSPSuite.Core.Domain.Constants;

namespace OSPSuite.Core.Services
{
   public abstract class concern_for_EntitySourcePathUpdater : ContextSpecification<IEntitySourcePathUpdater>
   {
      protected IEntityPathResolver _entityPathResolver;
      protected Model _model;
      protected SimulationBuilder _simulationBuilder;
      protected Container _root;

      protected override void Context()
      {
         _entityPathResolver = new EntityPathResolverForSpecs();
         sut = new EntitySourcePathUpdater(_entityPathResolver);
         _root = new Container().WithName("Sim`")
            .WithMode(ContainerMode.Logical)
            .WithContainerType(ContainerType.Simulation);

         _model = new Model {Root = _root};

         _simulationBuilder = new SimulationBuilder(new SimulationConfiguration());
      }
   }

   public class When_updating_the_entity_path_for_an_entity_that_was_not_tracked : concern_for_EntitySourcePathUpdater
   {
      [Observation]
      public void should_not_crash()
      {
         sut.UpdateEntityPath(_model, _simulationBuilder);
      }
   }

   public class When_updating_the_entity_path_for_an_entity_that_was_tracked : concern_for_EntitySourcePathUpdater
   {
      private Parameter _parameter;
      private EntitySource _entitySource;

      protected override void Context()
      {
         base.Context();
         _parameter = new Parameter().WithName("P1").WithId("ParamId");
         _root.Add(new Container().WithName(ORGANISM).WithChild(_parameter));
         _entitySource = new EntitySource("BBId", "Parameter", "sourceId", null);
         _simulationBuilder.AddEntitySource(_parameter.Id, _entitySource);
      }

      protected override void Because()
      {
         sut.UpdateEntityPath(_model, _simulationBuilder);
      }

      [Observation]
      public void should_return_the_expected_path()
      {
         _entitySource.EntityPath.ShouldBeEqualTo(new[] {ORGANISM, _parameter.Name}.ToPathString());
      }
   }
}