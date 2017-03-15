using System.Collections.Generic;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core
{
   public abstract class concern_for_OutputSelectionUpdater : ContextSpecification<IOutputSelectionUpdater>
   {
      protected List<QuantitySelection> _mappedOutputs;
      protected IModelCoreSimulation _simulation;
      protected OutputSelections _outputSelection;
      protected ISimulationPersistableUpdater _simulationPersistableUpdater;

      protected override void Context()
      {
         _simulationPersistableUpdater= A.Fake<ISimulationPersistableUpdater>();
         sut = new OutputSelectionUpdater(_simulationPersistableUpdater);
         _mappedOutputs = new List<QuantitySelection>();
         _simulation = A.Fake<IModelCoreSimulation>();
         _outputSelection = new OutputSelections();
         _simulation.BuildConfiguration.SimulationSettings.OutputSelections = _outputSelection;
      }
   }

   public class When_updating_the_selected_output_of_a_given_simulation_containing_output_not_used : concern_for_OutputSelectionUpdater
   {
      private QuantitySelection _simulationOutput1;
      private QuantitySelection _simulationOutput2;
      private QuantitySelection _simulationOutput3;
      private QuantitySelection _selectedOutput3;
      private QuantitySelection _selectedOutput4;

      protected override void Context()
      {
         base.Context();
         _simulationOutput1 = new QuantitySelection("OUTPUT1", QuantityType.Drug);
         _simulationOutput2 = new QuantitySelection("OUTPUT2", QuantityType.Drug);
         _simulationOutput3 = new QuantitySelection("OUTPUT3", QuantityType.Drug);
         _selectedOutput3 = new QuantitySelection("OUTPUT3", QuantityType.Drug);
         _selectedOutput4 = new QuantitySelection("OUTPUT4", QuantityType.Drug);

         _outputSelection.AddOutput(_simulationOutput1);
         _outputSelection.AddOutput(_simulationOutput2);
         _outputSelection.AddOutput(_simulationOutput3);

         _mappedOutputs.Add(_simulationOutput1);
         _mappedOutputs.Add(_selectedOutput3);
         _mappedOutputs.Add(_selectedOutput4);
      }

      protected override void Because()
      {
         sut.UpdateOutputsIn(_simulation, _mappedOutputs);
      }

      [Observation]
      public void should_remove_the_outputs_not_used_and_add_the_one_not_mapped()
      {
         _outputSelection.AllOutputs.ShouldOnlyContain(_simulationOutput1, _simulationOutput3, _selectedOutput4);
      }

      [Observation]
      public void should_update_the_persistable_flag_in_all_underlying_quantities()
      {
         A.CallTo(() => _simulationPersistableUpdater.UpdateSimulationPersistable(_simulation)).MustHaveHappened();
      }
   }
}