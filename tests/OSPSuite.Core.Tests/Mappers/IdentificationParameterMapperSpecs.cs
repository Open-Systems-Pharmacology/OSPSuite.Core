using System.Collections.Generic;
using System.Threading.Tasks;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Core.Services;
using OSPSuite.Core.Snapshots;
using OSPSuite.Core.Snapshots.Mappers;
using OSPSuite.Helpers;
using OSPSuite.Utility.Extensions;
using IdentificationParameter = OSPSuite.Core.Domain.ParameterIdentifications.IdentificationParameter;
using ParameterIdentification = OSPSuite.Core.Domain.ParameterIdentifications.ParameterIdentification;

namespace OSPSuite.Core.Mappers
{
   public abstract class concern_for_IdentificationParameterMapper : ContextSpecificationAsync<Helpers.Snapshots.IdentificationParameterMapper>
   {
      protected ParameterMapper _parameterMapper;
      protected IdentificationParameter _identificationParameter;
      protected OSPSuite.Core.Snapshots.IdentificationParameter _snapshot;
      protected ISimulation _simulation;
      private IParameter _parameter1;
      private IParameter _parameter2;
      private IParameter _parameter3;
      private IParameter _startValueParameter;
      protected Snapshots.Parameter _snapshotStartValueParameter;
      protected ParameterSelection _parameterSelection1;
      protected ParameterSelection _parameterSelection2;
      private ParameterSelection _parameterSelection3;
      private ParameterSelection _gfrSelection;
      protected IIdentificationParameterFactory _identificationParameterFactory;
      protected IOSPSuiteLogger _logger;
      protected ParameterIdentificationContext _parameterIdentificationContext;
      private ParameterIdentification _parameterIdentification;
      private TestProject _project;
      protected IIdentificationParameterTask _identificationParameterTask;
      private SnapshotContext _snapshotContext;
      private IParameter _gfrFraction;
      private IParameter _renalClearances;
      private ParameterSelection _renalSelection;

      protected override Task Context()
      {
         _parameterMapper = A.Fake<ParameterMapper>();
         _identificationParameterFactory = A.Fake<IIdentificationParameterFactory>();
         _logger = A.Fake<IOSPSuiteLogger>();
         _identificationParameterTask = A.Fake<IIdentificationParameterTask>();
         sut = new Helpers.Snapshots.IdentificationParameterMapper(_parameterMapper, _identificationParameterFactory, _identificationParameterTask, _logger);

         _identificationParameter = new IdentificationParameter
         {
            IsFixed = true,
            UseAsFactor = true,
            Scaling = Scalings.Linear
         };


         _startValueParameter = DomainHelperForSpecs.ConstantParameterWithValue(1).WithName(Constants.Parameters.START_VALUE);

         _identificationParameter.Add(_startValueParameter);
         _identificationParameter.Name = "PARAM";
         _parameter1 = DomainHelperForSpecs.ConstantParameterWithValue(1).WithName("P1");
         _parameter2 = DomainHelperForSpecs.ConstantParameterWithValue(1).WithName("P2");
         _parameter3 = DomainHelperForSpecs.ConstantParameterWithValue(1).WithName("P3");
         _gfrFraction = DomainHelperForSpecs.ConstantParameterWithValue(1).WithName("P4");
         _renalClearances = DomainHelperForSpecs.ConstantParameterWithValue(1).WithName("P5");
         _simulation = A.Fake<ISimulation>().WithName("S");
         _simulation.Model.Root = new Container
         {
            _parameter1,
            _parameter2,
            new Container { _parameter3 }.WithName(ParameterMapper.Applications),
            new Container
            {
               new Container
               {
                  _gfrFraction
               }.WithName("Glomerular Filtration-GFR"),
               new Container
               {
                  _renalClearances
               }.WithName("Renal Clearances-test")
            }.WithName("Alprazolam")
         };

         _identificationParameter.Scaling = Scalings.Linear;
         _parameterSelection1 = new ParameterSelection(_simulation, _parameter1.Name);
         _parameterSelection2 = new ParameterSelection(_simulation, _parameter2.Name);
         _parameterSelection3 = new ParameterSelection(_simulation, new ObjectPath(ParameterMapper.Applications, _parameter3.Name));
         _gfrSelection = new ParameterSelection(_simulation, new ObjectPath("Alprazolam", "Glomerular Filtration-GFR", _gfrFraction.Name));
         _renalSelection = new ParameterSelection(_simulation, new ObjectPath("Alprazolam", "Renal Clearances-test", _renalClearances.Name));
         _identificationParameter.AddLinkedParameter(_parameterSelection1);
         _identificationParameter.AddLinkedParameter(_parameterSelection2);
         _identificationParameter.AddLinkedParameter(_parameterSelection3);
         _identificationParameter.AddLinkedParameter(_gfrSelection);
         _identificationParameter.AddLinkedParameter(_renalSelection);

         _snapshotStartValueParameter = new Snapshots.Parameter();
         A.CallTo(() => _parameterMapper.MapToSnapshot(_startValueParameter)).Returns(_snapshotStartValueParameter);

         _project = new TestProject();
         _project.Add(_simulation);
         _snapshotContext = new SnapshotContext(_project, GetSnapshotContextVersion());
         _parameterIdentification = new ParameterIdentification();
         _parameterIdentificationContext = new ParameterIdentificationContext(_parameterIdentification, _snapshotContext);
         return _completed;
      }

      protected virtual SnapshotVersion GetSnapshotContextVersion()
      {
         return SnapshotVersions.Current;
      }
   }

   public class When_mapping_an_identification_parameter_to_snapshot : concern_for_IdentificationParameterMapper
   {
      protected override async Task Because()
      {
         _snapshot = await sut.MapToSnapshot(_identificationParameter);
      }

      [Observation]
      public void should_return_a_snapshot_with_the_expected_properties()
      {
         _snapshot.IsFixed.ShouldBeEqualTo(_identificationParameter.IsFixed);
         _snapshot.UseAsFactor.ShouldBeEqualTo(_identificationParameter.UseAsFactor);
         _snapshot.Scaling.ShouldBeEqualTo(_identificationParameter.Scaling);
         _snapshot.Name.ShouldBeEqualTo(_identificationParameter.Name);
      }

      [Observation]
      public void should_map_underlying_parameters()
      {
         _snapshot.Parameters.ShouldContain(_snapshotStartValueParameter);
      }

      [Observation]
      public void should_return_one_entry_for_each_linked_parameter()
      {
         _snapshot.LinkedParameters.ShouldContain(_parameterSelection1.FullQuantityPath, _parameterSelection2.FullQuantityPath);
      }
   }

   public class When_mapping_an_identification_parameter_snapshot_to_identification_parameter : concern_for_IdentificationParameterMapper
   {
      private IdentificationParameter _newParameterIdentification;

      protected override async Task Context()
      {
         await base.Context();
         _snapshot = await sut.MapToSnapshot(_identificationParameter);
         var newIdentificationParameter = new IdentificationParameter();
         A.CallTo(() => _identificationParameterFactory.CreateFor(A<IEnumerable<ParameterSelection>>._, _parameterIdentificationContext.ParameterIdentification)).Returns(newIdentificationParameter);
      }

      protected override async Task Because()
      {
         _newParameterIdentification = await sut.MapToModel(_snapshot, _parameterIdentificationContext);
      }

      [Observation]
      public void should_return_an_identification_parameter_with_the_expected_properties()
      {
         _newParameterIdentification.IsFixed.ShouldBeEqualTo(_identificationParameter.IsFixed);
         _newParameterIdentification.UseAsFactor.ShouldBeEqualTo(_identificationParameter.UseAsFactor);
         _newParameterIdentification.Scaling.ShouldBeEqualTo(_identificationParameter.Scaling);
         _newParameterIdentification.Name.ShouldBeEqualTo(_identificationParameter.Name);
      }

      [Observation]
      public void should_call_the_update_range_method_if_the_identification_parameter_is_using_factor()
      {
         A.CallTo(() => _identificationParameterTask.UpdateParameterRange(_newParameterIdentification)).MustHaveHappened();
      }
   }

   public class When_mapping_an_identification_parameter_snapshot_to_identification_parameter_that_cannot_be_created : concern_for_IdentificationParameterMapper
   {
      private IdentificationParameter _newParameterIdentification;

      protected override async Task Context()
      {
         await base.Context();
         _snapshot = await sut.MapToSnapshot(_identificationParameter);
         A.CallTo(() => _identificationParameterFactory.CreateFor(A<IEnumerable<ParameterSelection>>._, _parameterIdentificationContext.ParameterIdentification)).Returns(null);
      }

      protected override async Task Because()
      {
         _newParameterIdentification = await sut.MapToModel(_snapshot, _parameterIdentificationContext);
      }

      [Observation]
      public void should_return_null()
      {
         _newParameterIdentification.ShouldBeNull();
      }

      [Observation]
      public void should_log_a_warning()
      {
         A.CallTo(() => _logger.AddToLog(A<string>._, LogLevel.Warning, A<string>._)).MustHaveHappened();
      }
   }

   public class When_updating_a_v11_snapshot : concern_for_IdentificationParameterMapper
   {
      private IdentificationParameter _newParameterIdentification;

      protected override async Task Context()
      {
         await base.Context();
         _snapshot = await sut.MapToSnapshot(_identificationParameter);
         A.CallTo(() => _identificationParameterFactory.CreateFor(A<IEnumerable<ParameterSelection>>._, _parameterIdentificationContext.ParameterIdentification)).ReturnsLazily(x => createNewIdentificationParameter(x.Arguments.Get<IEnumerable<ParameterSelection>>(0)));
         renameApplications(_simulation.Model);
         renameRenalClearances(_simulation.Model);
      }

      protected override SnapshotVersion GetSnapshotContextVersion()
      {
         return SnapshotVersions.V11;
      }

      private IdentificationParameter createNewIdentificationParameter(IEnumerable<ParameterSelection> parameterSelections)
      {
         var newIdentificationParameter = new IdentificationParameter();

         parameterSelections.Each(newIdentificationParameter.AddLinkedParameter);
         return newIdentificationParameter;
      }

      private void renameRenalClearances(IModel model)
      {
         model.Root.GetAllChildren<IContainer>().Each(container =>
         {
            if (container.Name.Equals("Glomerular Filtration-GFR"))
               container.Name = "Glomerular Filtration-GFR-Alprazolam";

            if (container.Name.Equals("Renal Clearances-test"))
               container.Name = "Renal Clearances-test-Alprazolam";
         });
      }

      private static void renameApplications(IModel model)
      {
         model.Root.GetAllChildren<IContainer>().Each(container =>
         {
            if (container.Name.Equals(ParameterMapper.Applications))
               container.Name = "Events";
         });
      }

      protected override async Task Because()
      {
         _newParameterIdentification = await sut.MapToModel(_snapshot, _parameterIdentificationContext);
      }

      [Observation]
      public void should_change_the_parameter_selection_paths()
      {
         _newParameterIdentification.AllLinkedParameters.Count.ShouldBeEqualTo(5);
         _newParameterIdentification.AllLinkedParameters[2].PathArray.ShouldContain("Events");
         _newParameterIdentification.AllLinkedParameters[3].PathArray.ShouldContain("Glomerular Filtration-GFR-Alprazolam");
         _newParameterIdentification.AllLinkedParameters[4].PathArray.ShouldContain("Renal Clearances-test-Alprazolam");
      }
   }

   public abstract class concern_for_IdentificationParameterMapper_with_application_parameters : concern_for_IdentificationParameterMapper
   {
      protected IdentificationParameter _newParameterIdentification;
      protected Container _applicationContainer;
      protected Container _eventGroupContainer;

      protected override async Task Context()
      {
         await base.Context();
         AddApplicationLinkedParameters();
         _snapshot = await sut.MapToSnapshot(_identificationParameter);
         UpdateSimulationStructure();
         A.CallTo(() => _identificationParameterFactory.CreateFor(A<IEnumerable<ParameterSelection>>._, _parameterIdentificationContext.ParameterIdentification)).ReturnsLazily(x => createNewIdentificationParameter(x.Arguments.Get<IEnumerable<ParameterSelection>>(0)));
      }

      protected abstract void AddApplicationLinkedParameters();

      protected virtual void UpdateSimulationStructure()
      {
      }

      protected Container CreateApplicationContainer(string parameterName)
      {
         var applicationParameter = DomainHelperForSpecs.ConstantParameterWithValue(1).WithName(parameterName);
         return new Container { new Container { applicationParameter }.WithName("ProtocolSchemaItem") }.WithName("Application_1");
      }

      protected void MoveApplicationUnderNoFormulationContainer()
      {
         _eventGroupContainer.RemoveChild(_applicationContainer);
         _eventGroupContainer.Add(new Container { _applicationContainer }.WithName(ParameterMapper.NoFormulation));
      }

      private IdentificationParameter createNewIdentificationParameter(IEnumerable<ParameterSelection> parameterSelections)
      {
         var newIdentificationParameter = new IdentificationParameter();

         parameterSelections.Each(newIdentificationParameter.AddLinkedParameter);
         return newIdentificationParameter;
      }

      protected override async Task Because()
      {
         _newParameterIdentification = await sut.MapToModel(_snapshot, _parameterIdentificationContext);
      }
   }

   public class When_updating_a_v12_snapshot_with_application_parameters : concern_for_IdentificationParameterMapper_with_application_parameters
   {
      protected override void AddApplicationLinkedParameters()
      {
         _applicationContainer = CreateApplicationContainer("Infusion time");
         _eventGroupContainer = new Container { _applicationContainer }.WithName("iv 0.001 mg (5 min)");
         var oralEventGroupContainer = new Container { new Container { CreateApplicationContainer("Dose") }.WithName("Oral solution") }.WithName("po 10 mg");
         _simulation.Model.Root.Add(new Container { _eventGroupContainer, oralEventGroupContainer }.WithName(Constants.EVENTS));

         _identificationParameter.AddLinkedParameter(new ParameterSelection(_simulation, new ObjectPath(Constants.EVENTS, "iv 0.001 mg (5 min)", "Application_1", "ProtocolSchemaItem", "Infusion time")));
         _identificationParameter.AddLinkedParameter(new ParameterSelection(_simulation, new ObjectPath(Constants.EVENTS, "po 10 mg", "Oral solution", "Application_1", "ProtocolSchemaItem", "Dose")));
      }

      protected override void UpdateSimulationStructure()
      {
         MoveApplicationUnderNoFormulationContainer();
      }

      protected override SnapshotVersion GetSnapshotContextVersion()
      {
         return SnapshotVersions.V12;
      }

      [Observation]
      public void should_insert_the_no_formulation_container_for_applications_defined_without_formulation()
      {
         _newParameterIdentification.AllLinkedParameters[5].Path.ShouldBeEqualTo("Events|iv 0.001 mg (5 min)|No formulation|Application_1|ProtocolSchemaItem|Infusion time");
      }

      [Observation]
      public void should_not_change_the_path_for_applications_defined_with_a_formulation()
      {
         _newParameterIdentification.AllLinkedParameters[6].Path.ShouldBeEqualTo("Events|po 10 mg|Oral solution|Application_1|ProtocolSchemaItem|Dose");
      }
   }

   public class When_updating_a_v11_snapshot_with_an_application_parameter_defined_without_formulation : concern_for_IdentificationParameterMapper_with_application_parameters
   {
      protected override void AddApplicationLinkedParameters()
      {
         _applicationContainer = CreateApplicationContainer("Infusion time");
         _eventGroupContainer = new Container { _applicationContainer }.WithName("iv 0.001 mg (5 min)");
         _simulation.Model.Root.Container(ParameterMapper.Applications).Add(_eventGroupContainer);

         _identificationParameter.AddLinkedParameter(new ParameterSelection(_simulation, new ObjectPath(ParameterMapper.Applications, "iv 0.001 mg (5 min)", "Application_1", "ProtocolSchemaItem", "Infusion time")));
      }

      protected override void UpdateSimulationStructure()
      {
         _simulation.Model.Root.GetAllChildren<IContainer>().Each(container =>
         {
            if (container.Name.Equals(ParameterMapper.Applications))
               container.Name = Constants.EVENTS;

            if (container.Name.Equals("Glomerular Filtration-GFR"))
               container.Name = "Glomerular Filtration-GFR-Alprazolam";

            if (container.Name.Equals("Renal Clearances-test"))
               container.Name = "Renal Clearances-test-Alprazolam";
         });

         MoveApplicationUnderNoFormulationContainer();
      }

      protected override SnapshotVersion GetSnapshotContextVersion()
      {
         return SnapshotVersions.V11;
      }

      [Observation]
      public void should_replace_applications_with_events_and_insert_the_no_formulation_container()
      {
         _newParameterIdentification.AllLinkedParameters[5].Path.ShouldBeEqualTo("Events|iv 0.001 mg (5 min)|No formulation|Application_1|ProtocolSchemaItem|Infusion time");
      }
   }

   public class When_updating_a_current_snapshot_with_an_application_parameter : concern_for_IdentificationParameterMapper_with_application_parameters
   {
      protected override void AddApplicationLinkedParameters()
      {
         _applicationContainer = CreateApplicationContainer("Infusion time");
         _eventGroupContainer = new Container { _applicationContainer }.WithName("iv 0.001 mg (5 min)");
         _simulation.Model.Root.Add(new Container { _eventGroupContainer }.WithName(Constants.EVENTS));

         _identificationParameter.AddLinkedParameter(new ParameterSelection(_simulation, new ObjectPath(Constants.EVENTS, "iv 0.001 mg (5 min)", "Application_1", "ProtocolSchemaItem", "Infusion time")));
      }

      [Observation]
      public void should_not_change_the_parameter_selection_paths()
      {
         _newParameterIdentification.AllLinkedParameters[5].Path.ShouldBeEqualTo("Events|iv 0.001 mg (5 min)|Application_1|ProtocolSchemaItem|Infusion time");
      }
   }
}
