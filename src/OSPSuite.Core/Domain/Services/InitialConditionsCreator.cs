using System;
using System.Collections.Generic;
using System.Linq;
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
      ///    The <paramref name="containers" /> list are containers where an initial condition will be created for each using the
      ///    container path
      /// </summary>
      void AddToExpressionProfile(ExpressionProfileBuildingBlock buildingBlock, IReadOnlyList<IContainer> containers, MoleculeBuilder molecule);

      /// <summary>
      ///    Creates a new initial conditions for the <paramref name="molecule" /> using the path of the
      ///    <paramref name="container" />.
      ///    If supplied, the <paramref name="defaultStartFormula" /> will be used as the formula for the initial condition.
      /// </summary>
      InitialCondition CreateInitialCondition(IContainer container, MoleculeBuilder molecule, IFormula defaultStartFormula = null);

      /// <summary>
      ///    Creates a new initial conditions with <paramref name="moleculeAmountPath" /> using properties from the
      ///    <paramref name="moleculeAmount" />
      /// </summary>
      /// <returns></returns>
      InitialCondition CreateInitialCondition(ObjectPath moleculeAmountPath, MoleculeAmount moleculeAmount);
   }

   internal class InitialConditionsCreator : IInitialConditionsCreator
   {
      private readonly IObjectBaseFactory _objectBaseFactory;
      private readonly IIdGenerator _idGenerator;
      private readonly ICloneManagerForBuildingBlock _cloneManagerForBuildingBlock;
      private readonly IEntityPathResolver _entityPathResolver;

      public InitialConditionsCreator(
         IObjectBaseFactory objectBaseFactory, IEntityPathResolver entityPathResolver, IIdGenerator idGenerator,
         ICloneManagerForBuildingBlock cloneManagerForBuildingBlock)
      {
         _objectBaseFactory = objectBaseFactory;
         _cloneManagerForBuildingBlock = cloneManagerForBuildingBlock;
         _idGenerator = idGenerator;
         _entityPathResolver = entityPathResolver;
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

      public InitialCondition CreateInitialCondition(IContainer container, MoleculeBuilder molecule, IFormula defaultStartFormula = null)
      {
         var initialCondition = createInitialConditionWithValue(container, molecule);
         setInitialConditionFormula(defaultStartFormula ?? molecule.DefaultStartFormula, initialCondition, formula => formula);
         return initialCondition;
      }

      public InitialCondition CreateInitialCondition(ObjectPath moleculeAmountPath, MoleculeAmount moleculeAmount)
      {
         var containerPath = moleculeAmountPath.Clone<ObjectPath>();
         containerPath.RemoveAt(containerPath.Count - 1);

         var initialCondition = CreateInitialCondition(containerPath, moleculeAmountPath.Last(), moleculeAmount.Dimension, moleculeAmount.DisplayUnit, moleculeAmount.ValueOrigin);
         initialCondition.Value = moleculeAmount.Value;
         initialCondition.ScaleDivisor = moleculeAmount.ScaleDivisor;
         return initialCondition;
      }

      private InitialCondition createInitialConditionForBuildingBlock(IBuildingBlock initialConditionsBuildingBlock, IContainer container, MoleculeBuilder molecule)
      {
         var initialCondition = createInitialConditionWithValue(container, molecule);
         setInitialConditionFormula(molecule.DefaultStartFormula, initialCondition, formula => _cloneManagerForBuildingBlock.Clone(formula, initialConditionsBuildingBlock.FormulaCache));
         return initialCondition;
      }

      private InitialCondition createInitialConditionWithValue(IContainer container, MoleculeBuilder molecule)
      {
         var initialCondition = CreateInitialCondition(_entityPathResolver.ObjectPathFor(container), molecule.Name, molecule.Dimension, molecule.DisplayUnit);
         initialCondition.Value = molecule.GetDefaultInitialCondition();
         return initialCondition;
      }

      private void addMoleculesFrom(InitialConditionsBuildingBlock initialConditionsBuildingBlock, IContainer container, IEnumerable<MoleculeBuilder> molecules)
      {
         foreach (var molecule in molecules)
         {
            initialConditionsBuildingBlock.Add(createInitialConditionForBuildingBlock(initialConditionsBuildingBlock, container, molecule));
         }
      }

      public void AddToExpressionProfile(ExpressionProfileBuildingBlock buildingBlock, IReadOnlyList<IContainer> containers, MoleculeBuilder molecule)
      {
         containers.Each(container => { buildingBlock.AddInitialCondition(createInitialConditionForBuildingBlock(buildingBlock, container, molecule)); });
      }

      private void setInitialConditionFormula(IFormula formula, InitialCondition initialCondition, Func<IFormula, IFormula> createFormulaFrom)
      {
         if (!formula.IsConstant())
            initialCondition.Formula = createFormulaFrom(formula);
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