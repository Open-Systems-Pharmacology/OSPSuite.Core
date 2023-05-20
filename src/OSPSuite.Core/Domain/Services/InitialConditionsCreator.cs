using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Domain.Services
{
   public interface IInitialConditionsCreator : IEmptyStartValueCreator<InitialCondition>
   {
      InitialConditionsBuildingBlock CreateFrom(SpatialStructure spatialStructure, MoleculeBuildingBlock moleculeBuildingBlock);

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
   }

   internal class InitialConditionsCreator : IInitialConditionsCreator
   {
      private readonly IObjectBaseFactory _objectBaseFactory;
      private readonly IObjectPathFactory _objectPathFactory;
      private readonly IIdGenerator _idGenerator;
      private readonly ICloneManagerForBuildingBlock _cloneManagerForBuildingBlock;

      public InitialConditionsCreator(
         IObjectBaseFactory objectBaseFactory, IObjectPathFactory objectPathFactory, IIdGenerator idGenerator,
         ICloneManagerForBuildingBlock cloneManagerForBuildingBlock)
      {
         _objectBaseFactory = objectBaseFactory;
         _objectPathFactory = objectPathFactory;
         _cloneManagerForBuildingBlock = cloneManagerForBuildingBlock;
         _idGenerator = idGenerator;
      }

      public InitialConditionsBuildingBlock CreateFrom(SpatialStructure spatialStructure, MoleculeBuildingBlock moleculeBuildingBlock)
      {
         var initialConditions = _objectBaseFactory.Create<InitialConditionsBuildingBlock>().WithName(DefaultNames.InitialConditions);
         initialConditions.SpatialStructureId = spatialStructure.Id;
         initialConditions.MoleculeBuildingBlockId = moleculeBuildingBlock.Id;
         foreach (var container in spatialStructure.PhysicalContainers)
         {
            addMoleculesFrom(initialConditions, container, moleculeBuildingBlock);
         }

         return initialConditions;
      }

      private void addMoleculesFrom(InitialConditionsBuildingBlock moleculesStartValuesBuildingBlock, IEntity container, IEnumerable<MoleculeBuilder> molecules)
      {
         foreach (var molecule in molecules)
         {
            var initialCondition = CreateInitialCondition(_objectPathFactory.CreateAbsoluteObjectPath(container), molecule.Name, molecule.Dimension, molecule.DisplayUnit);
            setInitialCondition(molecule, initialCondition);
            setInitialConditionFormula(molecule.DefaultStartFormula, initialCondition, moleculesStartValuesBuildingBlock);
            moleculesStartValuesBuildingBlock.Add(initialCondition);
         }
      }

      private void setInitialConditionFormula(IFormula formula, InitialCondition initialCondition, IBuildingBlock moleculesStartValues)
      {
         if (!formula.IsConstant())
            initialCondition.Formula = _cloneManagerForBuildingBlock.Clone(formula, moleculesStartValues.FormulaCache);
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