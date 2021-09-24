using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Services.ParameterIdentifications
{
   public class CategorialParameterIdentificationRunInitializer : ParameterIdentificationRunInitializer
   {
      private readonly Utility.Container.IContainer _container;
      private readonly ICategorialParameterIdentificationDescriptionCreator _descriptionCreator;
      private readonly ICoreUserSettings _coreUserSettings;
      private CalculationMethodCombination _calculationMethodCombination;
      private bool _isSingleCategory;
      private CategorialParameterIdentificationRunMode _runMode;

      public CategorialParameterIdentificationRunInitializer(
         ICloneManagerForModel cloneManager,
         IParameterIdentificationRun parameterIdentificationRun,
         Utility.Container.IContainer container,
         ICategorialParameterIdentificationDescriptionCreator descriptionCreator,
         ICoreUserSettings coreUserSettings) : base(cloneManager,
         parameterIdentificationRun)
      {
         _container = container;
         _descriptionCreator = descriptionCreator;
         _coreUserSettings = coreUserSettings;
      }

      public void Initialize(ParameterIdentification parameterIdentification, int runIndex, CalculationMethodCombination calculationMethodCombination, bool isSingleCategory)
      {
         Initialize(parameterIdentification, runIndex);
         _calculationMethodCombination = calculationMethodCombination;
         _isSingleCategory = isSingleCategory;
         _runMode = parameterIdentification.Configuration.RunMode.DowncastTo<CategorialParameterIdentificationRunMode>();
      }

      public override Task<ParameterIdentification> InitializeRun(CancellationToken cancellationToken)
      {
         var parallelOptions = new ParallelOptions
         {
            CancellationToken = cancellationToken,
            MaxDegreeOfParallelism = _coreUserSettings.MaximumNumberOfCoresToUse
         };

         return Task.Run(() =>
         {
            var newParameterIdentification = _cloneManager.Clone(ParameterIdentification);
            Parallel.ForEach(newParameterIdentification.AllSimulations.ToList(), parallelOptions, originalSimulation =>
            {
               parallelOptions.CancellationToken.ThrowIfCancellationRequested();
               var newSimulation = createNewSimulationFrom(originalSimulation, _calculationMethodCombination.CalculationMethods);
               updateReferencesInParameterIdentification(newParameterIdentification, newSimulation, originalSimulation);
            });

            newParameterIdentification.Description = _descriptionCreator.CreateDescriptionFor(_calculationMethodCombination, _runMode, _isSingleCategory);
            return newParameterIdentification;
         }, cancellationToken);
      }

      private ISimulation createNewSimulationFrom(ISimulation simulation, IEnumerable<CalculationMethodWithCompoundName> combination)
      {
         //Always instantiate a new factory as some parameters might be global 
         return _container.Resolve<ICoreSimulationFactory>().CreateWithCalculationMethodsFrom(simulation, combination);
      }

      private void updateReferencesInParameterIdentification(ParameterIdentification parameterIdentification, ISimulation newSimulation, ISimulation oldSimulation)
      {
         parameterIdentification.SwapSimulations(oldSimulation, newSimulation);
      }
   }
}