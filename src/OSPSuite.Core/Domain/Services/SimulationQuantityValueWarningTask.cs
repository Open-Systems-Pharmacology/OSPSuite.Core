using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Extensions;

namespace OSPSuite.Core.Domain.Services
{
   public interface ISimulationQuantityValueWarningTask
   {
      /// <summary>
      ///    Adds warnings during simulation creation for all <paramref name="optimizedParameters" /> to the
      ///    <paramref name="creationResult" />
      ///    if the <see cref="ICoreUserSettings" /> indicate enable warnings for non-finite parameters
      /// </summary>
      void WarnForOptimizedLocalMoleculeParameters(IReadOnlyList<IParameter> optimizedParameters, CreationResult creationResult);

      /// <summary>
      ///    Adds warnings during simulation creation for all non-finite (NaN or Infinity) parameters in the
      ///    <paramref name="model" /> to the <paramref name="creationResult" />
      ///    if the <see cref="ICoreUserSettings" /> indicate enable warnings for non-finite parameters
      /// </summary>
      void WarnForNonFiniteQuantities(IModel model, CreationResult creationResult);

      /// <summary>
      ///    Adds warnings before simulation starts for all non-finite (NaN or Infinity) parameters in the
      ///    <paramref name="model" /> to the <paramref name="runValidationResult" />
      ///    if the <see cref="ICoreUserSettings" /> indicate enable warnings for non-finite parameters
      /// </summary>
      void WarnForNonFiniteQuantities(IModel model, RunValidationResult runValidationResult);
   }

   public class SimulationQuantityValueWarningTask : ISimulationQuantityValueWarningTask
   {
      private readonly ICoreUserSettings _userSettings;
      private readonly IObjectTypeResolver _objectTypeResolver;

      public SimulationQuantityValueWarningTask(ICoreUserSettings userSettings, IObjectTypeResolver objectTypeResolver)
      {
         _userSettings = userSettings;
         _objectTypeResolver = objectTypeResolver;
      }

      public void WarnForOptimizedLocalMoleculeParameters(IReadOnlyList<IParameter> optimizedParameters, CreationResult creationResult)
      {
         if (!_userSettings.WarnForNonFiniteQuantities)
            return;

         creationResult.Add(new ValidationResult(optimizedParameters.Select(x =>
         {
            var (builder, buildingBlock) = builderAndBuildingBlockFor(x, creationResult.SimulationBuilder);
            return new ValidationMessage(NotificationType.Warning, Warning.RemovedParameterDueToNanAtTimeZero(x.EntityPath()), builder, buildingBlock);
         })));
      }

      public void WarnForNonFiniteQuantities(IModel model, CreationResult creationResult)
      {
         var simulationBuilder = creationResult.SimulationBuilder;

         warnForNonFiniteQuantities(model, creationResult, simulationBuilder);
      }

      private void warnForNonFiniteQuantities(IModel model, WithValidationResult creationResult, SimulationBuilder simulationBuilder = null)
      {
         if (!_userSettings.WarnForNonFiniteQuantities)
            return;

         creationResult.Add(new ValidationResult(allNanQuantities<IParameter>(model).Select(x => createValidationMessageForNan(x, simulationBuilder))));
         creationResult.Add(new ValidationResult(allNanQuantities<MoleculeAmount>(model).Select(x => createValidationMessageForNan(x, simulationBuilder))));
         creationResult.Add(new ValidationResult(allInfiniteQuantities<IParameter>(model).Select(x => createValidationMessageForInf(x, simulationBuilder))));
         creationResult.Add(new ValidationResult(allInfiniteQuantities<MoleculeAmount>(model).Select(x => createValidationMessageForInf(x, simulationBuilder))));
      }

      public void WarnForNonFiniteQuantities(IModel model, RunValidationResult runValidationResult) => warnForNonFiniteQuantities(model, runValidationResult);

      private static IEnumerable<T> allInfiniteQuantities<T>(IModel model) where T : class, IQuantity => model.Root.GetAllChildren<T>().Where(x => double.IsInfinity(x.Value));

      private static IEnumerable<T> allNanQuantities<T>(IModel model) where T : class, IQuantity => model.Root.GetAllChildren<T>().Where(x => double.IsNaN(x.Value));

      private ValidationMessage createValidationMessageForNan<T>(T quantity, SimulationBuilder simulationBuilder) where T : IQuantity
      {
         var (builder, buildingBlock) = builderAndBuildingBlockFor(quantity, simulationBuilder);
         return new ValidationMessage(NotificationType.Warning, Warning.QuantityIsNanAtTimeZero(quantity.EntityPath(), _objectTypeResolver.TypeFor<T>().SplitToUpperCase()), builder, buildingBlock);
      }

      private ValidationMessage createValidationMessageForInf<T>(T quantity, SimulationBuilder simulationBuilder) where T : IQuantity
      {
         var (builder, buildingBlock) = builderAndBuildingBlockFor(quantity, simulationBuilder);
         return new ValidationMessage(NotificationType.Warning, Warning.QuantityIsInfinityAtTimeZero(quantity.EntityPath(), _objectTypeResolver.TypeFor<T>().SplitToUpperCase()), builder, buildingBlock);
      }

      private (IEntity builder, IBuildingBlock buildingBlock) builderAndBuildingBlockFor(IQuantity quantity, SimulationBuilder simulationBuilder)
      {
         if (simulationBuilder == null)
            return (quantity, null);

         var builder = simulationBuilder.BuilderFor(quantity);
         var buildingBlock = builder == null ? null : simulationBuilder.BuilderSourceFor(builder).BuildingBlock;
         return (builder, buildingBlock);
      }
   }
}