using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;

namespace OSPSuite.Core.Domain.Services
{
   /// <summary>
   ///    Service responsible for creation of ParameterValues-building blocks
   ///    based on building blocks pair {spatial structure, molecules}
   /// </summary>
   public interface IParameterValuesCreator : IEmptyStartValueCreator<ParameterValue>
   {
      /// <summary>
      ///    Creates and returns a new parameter value based on <paramref name="parameter">parameter</paramref>
      /// </summary>
      /// <param name="parameterPath">The path of the parameter</param>
      /// <param name="parameter">The Parameter object that has the start value and dimension to use</param>
      /// <returns>A new ParameterValue</returns>
      ParameterValue CreateParameterValue(ObjectPath parameterPath, IParameter parameter);

      /// <summary>
      ///    Creates and returns a new parameter value with <paramref name="value">startValue</paramref> as StartValue
      ///    and <paramref name="dimension">dimension</paramref> as dimension
      /// </summary>
      /// <param name="parameterPath">the path of the parameter</param>
      /// <param name="value">the value to be used as starting value</param>
      /// <param name="dimension">the dimension of the parameter</param>
      /// <param name="displayUnit">
      ///    The display unit of the start value. If not set, the default unit of the
      ///    <paramref name="dimension" />will be used
      /// </param>
      /// <param name="valueOrigin">Value origin for this parameter value</param>
      /// <param name="isDefault">Value indicating if the value stored is the default value from the parameter.</param>
      /// <returns>A new ParameterValue</returns>
      ParameterValue CreateParameterValue(ObjectPath parameterPath, double value, IDimension dimension, Unit displayUnit = null,
         ValueOrigin valueOrigin = null, bool isDefault = false);

      /// <summary>
      ///    Create and return a list of parameter values based on the <paramref name="spatialStructure" /> and
      ///    <paramref name="molecules" />. The list includes parameter values for all physical containers
      ///    in the spatial structure and all molecule parameters that are local and have constant formula
      /// </summary>
      IReadOnlyList<ParameterValue> CreateFrom(SpatialStructure spatialStructure, IReadOnlyList<MoleculeBuilder> molecules);

      /// <summary>
      ///    Create and return a list of parameter values based on the <paramref name="physicalContainer" /> and
      ///    <paramref name="molecules" />.
      ///    The returned values will only include parameters for those containers and molecules that are relevant for expression
      ///    profile.
      /// </summary>
      /// <returns></returns>
      IReadOnlyList<ParameterValue> CreateExpressionFrom(IContainer physicalContainer, IReadOnlyList<MoleculeBuilder> molecules);
   }

   internal class ParameterValuesCreator : IParameterValuesCreator
   {
      private readonly IIdGenerator _idGenerator;
      private readonly IEntityPathResolver _entityPathResolver;

      public ParameterValuesCreator(IIdGenerator idGenerator, IEntityPathResolver entityPathResolver)
      {
         _idGenerator = idGenerator;
         _entityPathResolver = entityPathResolver;
      }

      public ParameterValue CreateParameterValue(ObjectPath parameterPath, double value, IDimension dimension,
         Unit displayUnit = null, ValueOrigin valueOrigin = null, bool isDefault = false)
      {
         var parameterValue = new ParameterValue
         {
            Value = value,
            Dimension = dimension,
            Id = _idGenerator.NewId(),
            Path = parameterPath,
            DisplayUnit = displayUnit ?? dimension.DefaultUnit,
            IsDefault = isDefault
         };

         parameterValue.ValueOrigin.UpdateAllFrom(valueOrigin);
         return parameterValue;
      }

      public IReadOnlyList<ParameterValue> CreateFrom(SpatialStructure spatialStructure, IReadOnlyList<MoleculeBuilder> molecules) =>
         spatialStructure.PhysicalContainers.SelectMany(container => createFrom(container, molecules,
            (x, c) => isLocalWithConstantFormula(x) && satisfiesContainerCriteria(x, c))).ToList();

      public IReadOnlyList<ParameterValue> CreateExpressionFrom(IContainer physicalContainer, IReadOnlyList<MoleculeBuilder> molecules) =>
         physicalContainer.GetAllContainersAndSelf<IContainer>(x => x.Mode.Is(ContainerMode.Physical)).SelectMany(container => createFrom(container, molecules,
            (x, c) => x.IsExpression() && satisfiesContainerCriteria(x, c))).ToList();

      private IEnumerable<ParameterValue> createFrom(IContainer container, IReadOnlyList<MoleculeBuilder> molecules, Func<IParameter, IContainer, bool> createFor) =>
         molecules.SelectMany(x => createFrom(container, x, createFor));

      private IEnumerable<ParameterValue> createFrom(IContainer container, MoleculeBuilder molecule, Func<IParameter, IContainer, bool> createFor) =>
         molecule.Parameters.Where(x => createFor(x, container)).Select(x => CreateParameterValue(objectPathForParameterInContainer(container, x.Name, molecule.Name), x));

      private static bool isLocalWithConstantFormula(IParameter parameter) => parameter.BuildMode == ParameterBuildMode.Local && parameter.Formula.IsConstant();

      private static bool satisfiesContainerCriteria(IParameter parameter, IContainer container) => parameter.ContainerCriteria?.IsSatisfiedBy(container) ?? false;

      private ObjectPath objectPathForParameterInContainer(IContainer container, string parameterName, string moleculeName)
      {
         var pathForParameterInContainer = _entityPathResolver.ObjectPathFor(container);
         pathForParameterInContainer.AddRange(new[] { moleculeName, parameterName });
         return pathForParameterInContainer;
      }

      public ParameterValue CreateParameterValue(ObjectPath parameterPath, IParameter parameter)
      {
         return CreateParameterValue(parameterPath, parameter.Value, parameter.Dimension, parameter.DisplayUnit, parameter.ValueOrigin,
            parameter.IsDefault);
      }

      public ParameterValue CreateEmptyStartValue(IDimension dimension) => CreateParameterValue(ObjectPath.Empty, 0.0, dimension);
   }
}