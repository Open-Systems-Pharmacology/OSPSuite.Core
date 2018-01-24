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
   /// <summary>
   ///    Service responsible for creation of ParameterStartValues-building blocks
   ///    based on building blocks pair {spatial structure, molecules}
   /// </summary>
   public interface IParameterStartValuesCreator : IEmptyStartValueCreator<IParameterStartValue>
   {
      /// <summary>
      ///    Creates new ParameterStartValues-building block
      /// </summary>
      /// <param name="spatialStructure">Spatial structure to be used</param>
      /// <param name="moleculeBuildingBlock">Molecules to be used</param>
      /// <returns>Default parameter start values</returns>
      IParameterStartValuesBuildingBlock CreateFrom(ISpatialStructure spatialStructure, IMoleculeBuildingBlock moleculeBuildingBlock);

      /// <summary>
      ///    Creates and returns a new parameter start value based on <paramref name="parameter">parameter</paramref>
      /// </summary>
      /// <param name="parameterPath">The path of the parameter</param>
      /// <param name="parameter">The IParameter object that has the start value and dimension to use</param>
      /// <returns>A new IParameterStartValue</returns>
      IParameterStartValue CreateParameterStartValue(IObjectPath parameterPath, IParameter parameter);

      /// <summary>
      ///    Creates and returns a new parameter start value with <paramref name="startValue">startValue</paramref> as StartValue
      ///    and <paramref name="dimension">dimension</paramref> as dimension
      /// </summary>
      /// <param name="parameterPath">the path of the startvalue</param>
      /// <param name="startValue">the value to be used as StartValue</param>
      /// <param name="dimension">the dimension of the startvalue</param>
      /// <param name="displayUnit">
      ///    The display unit of the start value. If not set, the default unit of the
      ///    <paramref name="dimension" />will be used
      /// </param>
      /// <param name="valueOrigin">Value origin for this parameter start value</param>
      /// <param name="isDefault">Value indicating if the value stored is the default value from the parameter.</param>
      /// <returns>A new IParameterStartValue</returns>
      IParameterStartValue CreateParameterStartValue(IObjectPath parameterPath, double startValue, IDimension dimension, Unit displayUnit = null, ValueOrigin valueOrigin = null, bool isDefault = false);
   }

   internal class ParameterStartValuesCreator : IParameterStartValuesCreator
   {
      private readonly IObjectBaseFactory _objectBaseFactory;
      private readonly IObjectPathFactory _objectPathFactory;
      private readonly IIdGenerator _idGenerator;

      public ParameterStartValuesCreator(IObjectBaseFactory objectBaseFactory, IObjectPathFactory objectPathFactory, IIdGenerator idGenerator)
      {
         _objectBaseFactory = objectBaseFactory;
         _objectPathFactory = objectPathFactory;
         _idGenerator = idGenerator;
      }

      public IParameterStartValuesBuildingBlock CreateFrom(ISpatialStructure spatialStructure, IMoleculeBuildingBlock moleculeBuildingBlock)
      {
         var paramsStartValues = _objectBaseFactory.Create<IParameterStartValuesBuildingBlock>();
         paramsStartValues.SpatialStructureId = spatialStructure.Id;
         paramsStartValues.MoleculeBuildingBlockId = moleculeBuildingBlock.Id;

         addMoleculeParameterValues(paramsStartValues, spatialStructure, moleculeBuildingBlock);

         addTopContainersParameterValues(paramsStartValues, spatialStructure.TopContainers, moleculeBuildingBlock);

         addContainerListParameterValue(paramsStartValues, spatialStructure.Neighborhoods, moleculeBuildingBlock);

         addGlobalMoleculeParameterValues(paramsStartValues, spatialStructure.GlobalMoleculeDependentProperties, moleculeBuildingBlock);

         return paramsStartValues;
      }

      private void addGlobalMoleculeParameterValues(IParameterStartValuesBuildingBlock parameterStartValuesBuildingBlock, IContainer globalMoleculeDependentProperties, IMoleculeBuildingBlock moleculeBuildingBlock)
      {
         var globalParameter = globalMoleculeDependentProperties.GetChildren<IParameter>(parameterValueShouldBeSet);
         foreach (var parameter in globalParameter)
         {
            foreach (var moleculeBuilder in moleculeBuildingBlock)
            {
               parameterStartValuesBuildingBlock.Add(globalMoleculeParameterValueFor(moleculeBuilder, parameter));
            }
         }
      }

      /// <summary>
      ///    Adds neighborhood value parameters.
      /// </summary>
      private void addContainerListParameterValue(IParameterStartValuesBuildingBlock parameterStartValuesBuildingBlock,
         IEnumerable<IContainer> neighborhoodBuilders, IMoleculeBuildingBlock moleculeBuildingBlock)
      {
         foreach (var neighborhood in neighborhoodBuilders)
         {
            var parameters = neighborhood.GetChildren<IParameter>(parameterValueShouldBeSet);
            parameters.Each(param => parameterStartValuesBuildingBlock.Add(containerParameterValueFor(param)));

            var moleculeProperties = neighborhood.GetSingleChildByName<IContainer>(Constants.MOLECULE_PROPERTIES);
            if (moleculeProperties == null) continue;

            foreach (
               var paramValue in
               moleculeBuildingBlock.SelectMany(
                  moleculeBuilder => getMoleculePropertiesParameterValues(moleculeProperties, moleculeBuilder)))
            {
               parameterStartValuesBuildingBlock.Add(paramValue);
            }
         }
      }

      private void addTopContainersParameterValues(IParameterStartValuesBuildingBlock parameterStartValuesBuildingBlock, IEnumerable<IContainer> topContainers, IMoleculeBuildingBlock moleculeBuildingBlock)
      {
         topContainers.Each(c => addContainerListParameterValue(parameterStartValuesBuildingBlock, c.GetAllContainersAndSelf<IContainer>(child => !child.IsNamed(Constants.MOLECULE_PROPERTIES)), moleculeBuildingBlock));
      }

      private void addMoleculeParameterValues(IParameterStartValuesBuildingBlock parameterStartValuesBuildingBlock,
         ISpatialStructure spatialStructure, IEnumerable<IMoleculeBuilder> moleculeBuilderCollection)
      {
         foreach (var paramValue in moleculeBuilderCollection.SelectMany(moleculeBuilder => getMoleculeParameterValues(spatialStructure, moleculeBuilder)))
         {
            parameterStartValuesBuildingBlock.Add(paramValue);
         }
      }

      private IEnumerable<IParameterStartValue> getMoleculePropertiesParameterValues(IContainer moleculeProperties, IMoleculeBuilder moleculeBuilder)
      {
         foreach (var parameter in moleculeProperties.GetChildren<IParameter>(parameterValueShouldBeSet))
         {
            var path = _objectPathFactory.CreateAbsoluteObjectPath(parameter);
            path.Replace(Constants.MOLECULE_PROPERTIES, moleculeBuilder.Name);
            yield return CreateParameterStartValue(path, parameter);
         }
      }

      private IEnumerable<IParameterStartValue> getMoleculeParameterValues(ISpatialStructure spatialStructure, IMoleculeBuilder moleculeBuilder)
      {
         foreach (var parameter in moleculeBuilder.Parameters)
         {
            //check if parameter value should be set
            if (!parameterValueShouldBeSet(parameter))
               continue;

            //"Property"-Parameter are defined in the molecule itself
            if (parameter.BuildMode == ParameterBuildMode.Property)
               continue;

            //"Global"-Parameter are defined once per molecule
            if (parameter.BuildMode == ParameterBuildMode.Global)
            {
               yield return globalMoleculeParameterValueFor(moleculeBuilder, parameter);
               continue;
            }

            //"Local"-Parameter are defined per physical container and
            // per molecule
            if (parameter.BuildMode == ParameterBuildMode.Local)
            {
               foreach (var container in spatialStructure.PhysicalContainers)
                  yield return localMoleculeParameterValueFor(moleculeBuilder, parameter, container);

               continue;
            }

            //unknown build mode - should never happen
            throw new ArgumentException(Error.UnknownParameterBuildMode);
         }
      }

      private IParameterStartValue containerParameterValueFor(IParameter parameter)
      {
         var parameterPath = _objectPathFactory.CreateAbsoluteObjectPath(parameter);
         return CreateParameterStartValue(parameterPath, parameter);
      }

      public IParameterStartValue CreateParameterStartValue(IObjectPath parameterPath, double startValue, IDimension dimension, Unit displayUnit = null, ValueOrigin valueOrigin = null, bool isDefault = false)
      {
         var psv = new ParameterStartValue
         {
            StartValue = startValue,
            Dimension = dimension,
            Id = _idGenerator.NewId(),
            Path = parameterPath,
            DisplayUnit = displayUnit ?? dimension.DefaultUnit,
            IsDefault = isDefault
         };

         psv.ValueOrigin.UpdateAllFrom(valueOrigin);
         return psv;
      }

      public IParameterStartValue CreateParameterStartValue(IObjectPath parameterPath, IParameter parameter)
      {
         return CreateParameterStartValue(parameterPath, parameter.Value, parameter.Dimension, parameter.DisplayUnit, parameter.ValueOrigin, parameter.IsDefault);
      }

      private IParameterStartValue localMoleculeParameterValueFor(IMoleculeBuilder moleculeBuilder, IParameter parameter, IContainer container)
      {
         var parameterPath = _objectPathFactory.CreateAbsoluteObjectPath(container)
            .AndAdd(moleculeBuilder.Name)
            .AndAdd(parameter.Name);

         return CreateParameterStartValue(parameterPath, parameter);
      }

      private IParameterStartValue globalMoleculeParameterValueFor(IMoleculeBuilder moleculeBuilder, IParameter parameterTemplate)
      {
         var parameterPath = _objectPathFactory.CreateObjectPathFrom(moleculeBuilder.Name, parameterTemplate.Name);
         return CreateParameterStartValue(parameterPath, parameterTemplate);
      }

      /// <summary>
      ///    Check if parameter value should be set in the parameter start values.
      /// </summary>
      private bool parameterValueShouldBeSet(IParameter parameter)
      {
         return parameter.Formula.IsConstant();
      }

      public IParameterStartValue CreateEmptyStartValue(IDimension dimension)
      {
         return CreateParameterStartValue(ObjectPath.Empty, 0.0, dimension);
      }
   }
}