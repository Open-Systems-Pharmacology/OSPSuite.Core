using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Services
{
   public interface IInitialConditionsCreator : IEmptyStartValueCreator<InitialCondition>
   {
      InitialConditionsBuildingBlock CreateFrom(SpatialStructure spatialStructure, IReadOnlyList<MoleculeBuilder> molecules);

      /// <summary>
      ///    Creates an initial condition
      /// </summary>
      /// <param name="containerPath">The container path for the molecule</param>
      /// <param name="moleculeName">The name of the molecule</param>
      /// <param name="dimension">The dimension of the initial condition</param>
      /// <param name="displayUnit">
      ///    The display unit of the start value. If not set, the default unit of the
      ///    <paramref name="dimension" />will be used
      /// </param>
      /// <param name="valueOrigin">The value origin for the value</param>
      /// <returns>an InitialCondition object</returns>
      InitialCondition CreateInitialCondition(ObjectPath containerPath, string moleculeName, IDimension dimension, Unit displayUnit = null, ValueOrigin valueOrigin = null);

      /// <summary>
      ///    Creates a new initial conditions for the <paramref name="molecule" /> in the <paramref name="containers" /> and adds
      ///    them to the <paramref name="buildingBlock" />, initializing the default values and formulae.
      /// </summary>
      void CreateForExpressionInContainers(ExpressionProfileBuildingBlock buildingBlock, IReadOnlyList<IContainer> containers, MoleculeBuilder molecule);
   }

   internal class InitialConditionsCreator : IInitialConditionsCreator
   {
      private readonly IObjectBaseFactory _objectBaseFactory;
      private readonly IEntityPathResolver _entityPathResolver;
      private readonly IIdGenerator _idGenerator;
      private readonly ICloneManagerForBuildingBlock _cloneManagerForBuildingBlock;

      public InitialConditionsCreator(
         IObjectBaseFactory objectBaseFactory, IEntityPathResolver entityPathResolver, IIdGenerator idGenerator,
         ICloneManagerForBuildingBlock cloneManagerForBuildingBlock)
      {
         _objectBaseFactory = objectBaseFactory;
         _entityPathResolver = entityPathResolver;
         _cloneManagerForBuildingBlock = cloneManagerForBuildingBlock;
         _idGenerator = idGenerator;
      }

      public InitialConditionsBuildingBlock CreateFrom(SpatialStructure spatialStructure, IReadOnlyList<MoleculeBuilder> molecules)
      {
         var initialConditions = _objectBaseFactory.Create<InitialConditionsBuildingBlock>().WithName(DefaultNames.InitialConditions);

         foreach (var container in spatialStructure.PhysicalContainers)
         {
            addMoleculesFrom(initialConditions, container, molecules);
         }

         return initialConditions;
      }

      private InitialCondition createInitialCondition(IBuildingBlock initialConditionsBuildingBlock, IEntity container, MoleculeBuilder molecule)
      {
         var initialCondition = CreateInitialCondition(_entityPathResolver.ObjectPathFor(container), molecule.Name, molecule.Dimension, molecule.DisplayUnit);
         setInitialCondition(molecule, initialCondition);
         setInitialConditionFormula(molecule.DefaultStartFormula, initialCondition, initialConditionsBuildingBlock);
         return initialCondition;
      }

      private void addMoleculesFrom(InitialConditionsBuildingBlock initialConditionsBuildingBlock, IEntity container, IEnumerable<MoleculeBuilder> molecules)
      {
         foreach (var molecule in molecules)
         {
            initialConditionsBuildingBlock.Add(createInitialCondition(initialConditionsBuildingBlock, container, molecule));
         }
      }

      public void CreateForExpressionInContainers(ExpressionProfileBuildingBlock buildingBlock, IReadOnlyList<IContainer> containers, MoleculeBuilder molecule)
      {
         containers.Each(container => { buildingBlock.AddInitialCondition(createInitialCondition(buildingBlock, container, molecule)); });
      }

      private void setInitialConditionFormula(IFormula formula, InitialCondition initialCondition, IBuildingBlock buildingBlock)
      {
         if (!formula.IsConstant())
            initialCondition.Formula = _cloneManagerForBuildingBlock.Clone(formula, buildingBlock.FormulaCache);
      }

      private static void setInitialCondition(MoleculeBuilder moleculeBuilder, InitialCondition initialCondition)
      {
         initialCondition.Value = moleculeBuilder.GetDefaultInitialCondition();
      }

      public InitialCondition CreateInitialCondition(ObjectPath containerPath, string moleculeName, IDimension dimension, Unit displayUnit = null, ValueOrigin valueOrigin = null)
      {
         var initialCondition = new InitialCondition
         {
            Id = _idGenerator.NewId(),
            IsPresent = true,
            ContainerPath = containerPath,
            Name = moleculeName,
            Dimension = dimension,
            DisplayUnit = displayUnit ?? dimension.DefaultUnit,
            NegativeValuesAllowed = false,
         };

         initialCondition.ValueOrigin.UpdateAllFrom(valueOrigin);
         return initialCondition;
      }

      public InitialCondition CreateEmptyStartValue(IDimension dimension)
      {
         return CreateInitialCondition(ObjectPath.Empty, string.Empty, dimension);
      }
   }
}