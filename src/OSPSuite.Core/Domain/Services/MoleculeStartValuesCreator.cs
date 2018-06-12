using System.Collections.Generic;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Domain.Services
{
   public interface IMoleculeStartValuesCreator : IEmptyStartValueCreator<IMoleculeStartValue>
   {
      IMoleculeStartValuesBuildingBlock CreateFrom(ISpatialStructure spatialStructure, IMoleculeBuildingBlock moleculeBuildingBlock);

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
      IMoleculeStartValue CreateMoleculeStartValue(IObjectPath containerPath, string moleculeName, IDimension dimension, Unit displayUnit = null, ValueOrigin valueOrigin = null);
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

      public IMoleculeStartValuesBuildingBlock CreateFrom(ISpatialStructure spatialStructure, IMoleculeBuildingBlock moleculeBuildingBlock)
      {
         var moleculesStartValues = _objectBaseFactory.Create<IMoleculeStartValuesBuildingBlock>();
         moleculesStartValues.SpatialStructureId = spatialStructure.Id;
         moleculesStartValues.MoleculeBuildingBlockId = moleculeBuildingBlock.Id;
         foreach (var container in spatialStructure.PhysicalContainers)
         {
            addMoleculesFrom(moleculesStartValues, container, moleculeBuildingBlock);
         }

         return moleculesStartValues;
      }

      private void addMoleculesFrom(IMoleculeStartValuesBuildingBlock moleculesStartValuesBuildingBlock, IEntity container, IEnumerable<IMoleculeBuilder> molecules)
      {
         foreach (var molecule in molecules)
         {
            var moleculeStartValue = CreateMoleculeStartValue(_objectPathFactory.CreateAbsoluteObjectPath(container), molecule.Name, molecule.Dimension, molecule.DisplayUnit);
            setMoleculeStartValue(molecule, moleculeStartValue);
            setMoleculeStartValueFormula(molecule.DefaultStartFormula, moleculeStartValue, moleculesStartValuesBuildingBlock);
            moleculesStartValuesBuildingBlock.Add(moleculeStartValue);
         }
      }

      private void setMoleculeStartValueFormula(IFormula formula, IMoleculeStartValue moleculeStartValue, IBuildingBlock moleculesStartValues)
      {
         if (!formula.IsConstant())
            moleculeStartValue.Formula = _cloneManagerForBuildingBlock.Clone(formula, moleculesStartValues.FormulaCache);
      }

      private static void setMoleculeStartValue(IMoleculeBuilder moleculeBuilder, IMoleculeStartValue moleculeStartValue)
      {
         moleculeStartValue.StartValue = moleculeBuilder.GetDefaultMoleculeStartValue();
      }

      public IMoleculeStartValue CreateMoleculeStartValue(IObjectPath containerPath, string moleculeName, IDimension dimension, Unit displayUnit = null, ValueOrigin valueOrigin = null)
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

      public IMoleculeStartValue CreateEmptyStartValue(IDimension dimension)
      {
         return CreateMoleculeStartValue(ObjectPath.Empty, string.Empty, dimension);
      }
   }
}