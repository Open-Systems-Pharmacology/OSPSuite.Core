using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain.ParameterIdentifications;

namespace OSPSuite.Core.Domain.Services.ParameterIdentifications
{
   public class CategorialParameterIdentificationRunInitializer : ParameterIdentifcationRunInitializer
   {
      private readonly ICoreSimulationFactory _simulationFactory;
      private readonly ICategorialParameterIdentificationDescriptionCreator _descriptionCreator;
      private CalculationMethodCombination _calculationMethodCombination;
      private bool _isSingleCategory;
      private CategorialParameterIdentificationRunMode _runMode;

      public CategorialParameterIdentificationRunInitializer(ICloneManagerForModel cloneManager, IParameterIdentificationRun parameterIdentificationRun, ICoreSimulationFactory simulationFactory, ICategorialParameterIdentificationDescriptionCreator descriptionCreator) : base(cloneManager, parameterIdentificationRun)
      {
         _simulationFactory = simulationFactory;
         _descriptionCreator = descriptionCreator;
      }

      public void Initialize(ParameterIdentification parameterIdentification, int runIndex, CalculationMethodCombination calculationMethodCombination, bool isSingleCategory)
      {
         Initialize(parameterIdentification, runIndex);
         _calculationMethodCombination = calculationMethodCombination;
         _isSingleCategory = isSingleCategory;
         _runMode = parameterIdentification.Configuration.RunMode.DowncastTo<CategorialParameterIdentificationRunMode>();
      }

      public override Task<ParameterIdentification> InitializeRun()
      {
         return Task.Run(() =>
         {
            var newParameterIdentification = _cloneManager.Clone(ParameterIdentification);
            newParameterIdentification.AllSimulations.ToList().Each(originalSimulation =>
            {
               var newSimulation = createNewSimulationFrom(originalSimulation, _calculationMethodCombination.CalculationMethods);
               updateReferencesInParameterIdentification(newParameterIdentification, newSimulation, originalSimulation);
               newParameterIdentification.Description = _descriptionCreator.CreateDescriptionFor(_calculationMethodCombination, _runMode, _isSingleCategory);
            });
            return newParameterIdentification;
         });
      }

      private ISimulation createNewSimulationFrom(ISimulation simulation, IEnumerable<CalculationMethodWithCompoundName> combination)
      {
         return _simulationFactory.CreateWithCalculationMethodsFrom(simulation, combination);
      }

      private void updateReferencesInParameterIdentification(ParameterIdentification parameterIdentification, ISimulation newSimulation, ISimulation oldSimulation)
      {
         parameterIdentification.SwapSimulations(oldSimulation, newSimulation);
      }
   }
}