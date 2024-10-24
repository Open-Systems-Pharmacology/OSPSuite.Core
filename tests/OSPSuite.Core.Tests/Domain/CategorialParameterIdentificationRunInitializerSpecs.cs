﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_CategorialParameterIdentificationRunInitializer : ContextSpecification<CategorialParameterIdentificationRunInitializer>
   {
      private ICloneManagerForModel _cloneManager;
      private ICoreSimulationFactory _simulationFactory;
      private ICategorialParameterIdentificationDescriptionCreator _descriptionCreator;
      protected ParameterIdentification _parameterIdentification;
      protected ISimulation _simulation;
      protected ISimulation _clonedSimulation;
      protected IParameterIdentificationRun _parameterIdentificationRun;
      private ICoreUserSettings _coreUserSettings;
      private Utility.Container.IContainer _container;

      protected override void Context()
      {
         _cloneManager = A.Fake<ICloneManagerForModel>();
         _simulationFactory = A.Fake<ICoreSimulationFactory>();
         _parameterIdentificationRun = A.Fake<IParameterIdentificationRun>();
         _coreUserSettings= A.Fake<ICoreUserSettings>();
         _descriptionCreator = A.Fake<ICategorialParameterIdentificationDescriptionCreator>();
         _container = A.Fake<Utility.Container.IContainer>();
         A.CallTo(() => _container.Resolve<ICoreSimulationFactory>()).Returns(_simulationFactory);
         _coreUserSettings.MaximumNumberOfCoresToUse = 1;
         sut = new CategorialParameterIdentificationRunInitializer(_cloneManager, _parameterIdentificationRun, _container, _descriptionCreator, _coreUserSettings);

         _parameterIdentification = new ParameterIdentification();
         _parameterIdentification.Configuration.RunMode = new CategorialParameterIdentificationRunMode();

         _simulation = A.Fake<ISimulation>();
         _clonedSimulation = A.Fake<ISimulation>();

         A.CallTo(() => _simulationFactory.CreateWithCalculationMethodsFrom(_simulation, A<IEnumerable<CalculationMethodWithCompoundName>>._)).Returns(_clonedSimulation);
         A.CallTo(() => _cloneManager.Clone(_parameterIdentification)).ReturnsLazily(() =>
         {
            var pi = new ParameterIdentification();
            pi.AddSimulation(_simulation);
            var identificationParameter = new IdentificationParameter();
            identificationParameter.AddLinkedParameter(new ParameterSelection(_simulation, A.Fake<QuantitySelection>()));
            pi.AddIdentificationParameter(identificationParameter);
            pi.AddOutputMapping(new OutputMapping
            {
               OutputSelection = new SimulationQuantitySelection(_simulation, A.Fake<QuantitySelection>())
            });
            pi.Configuration.RunMode = new CategorialParameterIdentificationRunMode();
            return pi;
         });
      }
   }

   public class When_initializing_categorial_parameter_identification_run : concern_for_CategorialParameterIdentificationRunInitializer
   {
      private ParameterIdentification _result;
      private CalculationMethodCombination _combination;

      protected override void Context()
      {
         base.Context();
         _combination = new CalculationMethodCombination(new List<CalculationMethodWithCompoundName>());
         sut.Initialize(_parameterIdentification, 0,_combination,isSingleCategory:true);
         A.CallTo(() => _clonedSimulation.CompoundNames).Returns(new[] {"compound1"});
      }

      protected override void Because()
      {
         _result = sut.InitializeRun(CancellationToken.None).Result;
      }

      [Observation]
      public void all_identification_parameters_should_reference_cloned_simulation()
      {
         _result.AllIdentificationParameters.SelectMany(idParameter => idParameter.AllLinkedParameters).All(parameter => Equals(_clonedSimulation, parameter.Simulation)).ShouldBeTrue();
         _result.AllIdentificationParameters.SelectMany(idParameter => idParameter.AllLinkedParameters).All(parameter => Equals(_simulation, parameter.Simulation)).ShouldBeFalse();
      }

      [Observation]
      public void all_output_mappings_should_reference_cloned_simulation()
      {
         _result.AllOutputMappings.All(mapping => mapping.UsesSimulation(_clonedSimulation)).ShouldBeTrue();
         _result.AllOutputMappings.All(mapping => mapping.UsesSimulation(_simulation)).ShouldBeFalse();
      }
   }
}