using System.Collections.Generic;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Domain.Services
{
   public interface IMoleculeStartValuesCreator : IEmptyStartValueCreator<MoleculeStartValue>
   {
      MoleculeStartValuesBuildingBlock CreateFrom(ISpatialStructure spatialStructure, MoleculeBuildingBlock moleculeBuildingBlock);

      /// <summary>
      ///    Creates a molecule start value
      /// </summary>
      /// <param name="containerPath">The container path for the molecule</param>
      /// <param name="moleculeName">The name of the molecule</param>
      /// <param name="dimension">The dimension of the molecule start value</param>
      /// <param name="displayUnit">
      ///    The display unit of the start value. If not set, the default unit of the
      ///    <paramref name="dimension" />will be used
      /// </param>
      /// <param name="valueOrigin">The value origin for the value</param>
      /// <returns>a MoleculeStartValue object</returns>
      MoleculeStartValue CreateMoleculeStartValue(ObjectPath containerPath, string moleculeName, IDimension dimension, Unit displayUnit = null, ValueOrigin valueOrigin = null);
   }

   internal class MoleculeStartValuesCreator : IMoleculeStartValuesCreator
   {
      private readonly IObjectBaseFactory _objectBaseFactory;
      private readonly IObjectPathFactory _objectPathFactory;
      private readonly IIdGenerator _idGenerator;
      private readonly ICloneManagerForBuildingBlock _cloneManagerForBuildingBlock;

      public MoleculeStartValuesCreator(
         IObjectBaseFactory objectBaseFactory, IObjectPathFactory objectPathFactory, IIdGenerator idGenerator,
         ICloneManagerForBuildingBlock cloneManagerForBuildingBlock)
      {
         _objectBaseFactory = objectBaseFactory;
         _objectPathFactory = objectPathFactory;
         _cloneManagerForBuildingBlock = cloneManagerForBuildingBlock;
         _idGenerator = idGenerator;
      }

      public MoleculeStartValuesBuildingBlock CreateFrom(ISpatialStructure spatialStructure, MoleculeBuildingBlock moleculeBuildingBlock)
      {
         var moleculesStartValues = _objectBaseFactory.Create<MoleculeStartValuesBuildingBlock>();
         moleculesStartValues.SpatialStructureId = spatialStructure.Id;
         moleculesStartValues.MoleculeBuildingBlockId = moleculeBuildingBlock.Id;
         foreach (var container in spatialStructure.PhysicalContainers)
         {
            addMoleculesFrom(moleculesStartValues, container, moleculeBuildingBlock);
         }

         return moleculesStartValues;
      }

      private void addMoleculesFrom(MoleculeStartValuesBuildingBlock moleculesStartValuesBuildingBlock, IEntity container, IEnumerable<IMoleculeBuilder> molecules)
      {
         foreach (var molecule in molecules)
         {
            var moleculeStartValue = CreateMoleculeStartValue(_objectPathFactory.CreateAbsoluteObjectPath(container), molecule.Name, molecule.Dimension, molecule.DisplayUnit);
            setMoleculeStartValue(molecule, moleculeStartValue);
            setMoleculeStartValueFormula(molecule.DefaultStartFormula, moleculeStartValue, moleculesStartValuesBuildingBlock);
            moleculesStartValuesBuildingBlock.Add(moleculeStartValue);
         }
      }

      private void setMoleculeStartValueFormula(IFormula formula, MoleculeStartValue moleculeStartValue, IBuildingBlock moleculesStartValues)
      {
         if (!formula.IsConstant())
            moleculeStartValue.Formula = _cloneManagerForBuildingBlock.Clone(formula, moleculesStartValues.FormulaCache);
      }

      private static void setMoleculeStartValue(IMoleculeBuilder moleculeBuilder, MoleculeStartValue moleculeStartValue)
      {
         moleculeStartValue.Value = moleculeBuilder.GetDefaultMoleculeStartValue();
      }

      public MoleculeStartValue CreateMoleculeStartValue(ObjectPath containerPath, string moleculeName, IDimension dimension, Unit displayUnit = null, ValueOrigin valueOrigin = null)
      {
         var msv = new MoleculeStartValue
         {
            Id = _idGenerator.NewId(),
            IsPresent = true,
            ContainerPath = containerPath,
            Name = moleculeName,
            Dimension = dimension,
            DisplayUnit = displayUnit ?? dimension.DefaultUnit,
            NegativeValuesAllowed = false,
         };

         msv.ValueOrigin.UpdateAllFrom(valueOrigin);
         return msv;
      }

      public MoleculeStartValue CreateEmptyStartValue(IDimension dimension)
      {
         return CreateMoleculeStartValue(ObjectPath.Empty, string.Empty, dimension);
      }
   }
}