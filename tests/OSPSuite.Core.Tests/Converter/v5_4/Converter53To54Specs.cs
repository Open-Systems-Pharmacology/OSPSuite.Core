using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Converter.v5_4;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Serialization.Exchange;

namespace OSPSuite.Converter.v5_4
{
   public abstract class concern_for_Converter53To54 : ContextSpecification<Converter531To541>
   {
      protected IEventGroupBuildingBlock _eventGroubBuildingBlock;
      protected IEventGroupBuilder _eventGroup;
      protected IApplicationBuilder _rootApplication;
      protected IApplicationBuilder _application;
      protected IContainer _container;
      protected bool _converted;

      protected override void Context()
      {
         sut = new Converter531To541();
         _eventGroubBuildingBlock = new EventGroupBuildingBlock();
         _eventGroup = new EventGroupBuilder().WithName("EG");
         _rootApplication = new ApplicationBuilder().WithName("App").WithContainerType(ContainerType.Other);
         _eventGroubBuildingBlock.Add(_eventGroup);
         _eventGroubBuildingBlock.Add(_rootApplication);
         _container = new Container().WithName("PSI").WithParentContainer(_eventGroup);
         _application = new ApplicationBuilder().WithName("App").WithContainerType(ContainerType.Other).WithParentContainer(_eventGroup);
      }
   }

   public class When_converting_an_event_group_building_block : concern_for_Converter53To54
   {
      protected override void Because()
      {
         (_, _converted) = sut.Convert(_eventGroubBuildingBlock);
      }

      [Observation]
      public void Should_change_container_type_of_applicationbuilders_to_application()
      {
         _rootApplication.ContainerType.ShouldBeEqualTo(ContainerType.Application);
         _application.ContainerType.ShouldBeEqualTo(ContainerType.Application);
      }

      [Observation]
      public void Should_not_change_container_type_of_other_to_application()
      {
         _eventGroup.ContainerType.ShouldBeEqualTo(ContainerType.EventGroup);
         _container.ContainerType.ShouldBeEqualTo(ContainerType.Other);
      }

      [Observation]
      public void should_notify_that_a_conversion_happened()
      {
         _converted.ShouldBeTrue();
      }
   }

   public class When_converting_a_SimulationTransfer : concern_for_Converter53To54
   {
      private IModelCoreSimulation _simulation;
      private IModel _model;
      private IContainer _applications;
      private IContainer _organism;
      private IEventGroup _simApp;
      private IEventGroup _globalApp;

      protected override void Context()
      {
         base.Context();
         _model = new Model();
         _model.Root = new Container().WithName("root");
         _applications = new Container().WithName("EG");
         _organism = new Container().WithName("Org");
         _model.Root.AddChildren(_applications, _organism);
         _simApp = new EventGroup().WithName("App");
         _applications.Add(_simApp);
         _globalApp = new EventGroup().WithName("App").WithParentContainer(_model.Root);

         _simulation = new ModelCoreSimulation {BuildConfiguration = new BuildConfiguration(), Model = _model};

         _simulation.BuildConfiguration.EventGroups = _eventGroubBuildingBlock;
      }

      protected override void Because()
      {
         sut.Convert(new SimulationTransfer {Simulation = _simulation});
      }

      [Observation]
      public void Should_change_container_type_of_applications_to_application()
      {
         _simApp.ContainerType.ShouldBeEqualTo(ContainerType.Application);
         _globalApp.ContainerType.ShouldBeEqualTo(ContainerType.Application);
      }
   }

   public class When_converting_an_object_that_does_not_require_conversion : concern_for_Converter53To54
   {
      [Observation]
      public void should_not_return_that_a_conversion_happened()
      {
         var (_, converted) = sut.Convert(new Parameter());
         converted.ShouldBeFalse();
      }
   }
}