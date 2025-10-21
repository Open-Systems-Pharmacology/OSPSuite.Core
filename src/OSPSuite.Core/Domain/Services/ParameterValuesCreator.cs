using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility.Extensions;

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
      private readonly IProjectRetriever _projectRetriever;
      private readonly ICloneManagerForBuildingBlock _cloneManager;

      public ParameterValuesCreator(IIdGenerator idGenerator, IEntityPathResolver entityPathResolver, IProjectRetriever projectRetriever, ICloneManagerForBuildingBlock cloneManager)
      {
         _idGenerator = idGenerator;
         _entityPathResolver = entityPathResolver;
         _projectRetriever = projectRetriever;
         _cloneManager = cloneManager;
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
         spatialStructure.PhysicalContainers.SelectMany(container => createLocalFrom(container, molecules)).ToList();

      public IReadOnlyList<ParameterValue> CreateExpressionFrom(IContainer physicalContainer, IReadOnlyList<MoleculeBuilder> molecules) => 
         physicalContainer.GetAllContainersAndSelf<IContainer>(x => x.Mode.Is(ContainerMode.Physical)).SelectMany(container => createExpressionFrom(container, molecules)).ToList();

      private IEnumerable<ParameterValue> createExpressionFrom(IContainer container, IReadOnlyList<MoleculeBuilder> molecules) => molecules.SelectMany(x => createExpressionFrom(container, x));

      private void updateFromExpression(IReadOnlyList<ParameterValue> parameterValues, IReadOnlyList<ExpressionParameter> expressionParameters)
      {
         var formulaCache = new FormulaCache();
         parameterValues.Each(parameterValue =>
         {
            var expressionParameter = expressionSourceFor(expressionParameters.AllByName(parameterValue.Name).ToList(), parameterValue);

            updateFromExpression(expressionParameter, parameterValue, formulaCache);
         });
      }

      private void updateFromExpression(ExpressionParameter expressionParameter, ParameterValue parameterValue, FormulaCache formulaCache)
      {
         if (expressionParameter == null)
            return;

         if (expressionParameter.Value.HasValue)
            setParameterValueValue(expressionParameter.Value.Value, parameterValue);
         else if (shouldCloneFormulaFor(parameterValue)) 
            setParameterValueFormula(expressionParameter.Formula, parameterValue);
      }

      public ParameterValue CreateParameterValue(ObjectPath parameterPath, IParameter parameter)
      {
         var parameterValue = CreateParameterValue(parameterPath, 0.0, parameter.Dimension, parameter.DisplayUnit, parameter.ValueOrigin,
            parameter.IsDefault);

         if (shouldSetValue(parameter))
            setParameterValueValue(parameter.Value, parameterValue);
         else
            setParameterValueFormula(parameter.Formula, parameterValue);

         return parameterValue;
      }

      private void setParameterValueFormula(IFormula formula, ParameterValue parameterValue)
      {
         parameterValue.Formula = _cloneManager.Clone(formula, new FormulaCache());
         parameterValue.Value = null;
      }

      private static void setParameterValueValue(double value, ParameterValue parameterValue)
      {
         parameterValue.Value = value;
         parameterValue.Formula = null;
      }

      private bool shouldCloneFormulaFor(ParameterValue parameterValue) => parameterValue.Formula == null || parameterValue.Formula.IsConstant();

      private static ExpressionParameter expressionSourceFor(List<ExpressionParameter> nameMatchedExpressionParameters, ParameterValue formulaTarget)
      {
         var formulaSource = pathMatchedExpressionParameterFor(nameMatchedExpressionParameters, formulaTarget);

         if (formulaSource != null || !hasCompartment(formulaTarget)) 
            return formulaSource;
         
         var potentialSources = compartmentMatchedExpressionParametersFor(formulaTarget, nameMatchedExpressionParameters);
         return !potentialSources.Any() ? null : mostFrequentFormulaExpression(potentialSources);
      }

      private static ExpressionParameter mostFrequentFormulaExpression(IReadOnlyList<ExpressionParameter> potentialSources)
      {
         // Group the potential sources by formula name, order the groups by the count, flatten the groups, take the first expression parameter
         return potentialSources.GroupBy(formulaGroupingName).OrderByDescending(x => x.Count()).SelectMany(x => x).First();
      }

      /// <summary>
      /// Returns a name for the <paramref name="expressionParameter"/> formula. Expression parameters with value return the value as string
      /// </summary>
      private static string formulaGroupingName(ExpressionParameter expressionParameter) => expressionParameter.Formula == null ? expressionParameter.Value.ToString() : expressionParameter.Formula.Name;

      private static IReadOnlyList<ExpressionParameter> compartmentMatchedExpressionParametersFor(ParameterValue formulaTarget, List<ExpressionParameter> nameMatchedExpressionParameters) =>
         nameMatchedExpressionParameters.Where(x => hasCompartment(x) && Equals(compartmentFor(x), compartmentFor(formulaTarget))).ToList();

      private static string compartmentFor(ParameterValue formulaTarget) => formulaTarget.Path[compartmentIndex(formulaTarget)];

      private static ExpressionParameter pathMatchedExpressionParameterFor(List<ExpressionParameter> nameMatchedExpressionParameters, ParameterValue formulaTarget) =>
         nameMatchedExpressionParameters.FirstOrDefault(x => Equals(x.Path, formulaTarget.Path));

      /// <summary>
      /// When dealing with ExpressionParameters, the path takes the form "Organism|ArterialBlood|Plasma|GABRG2|Initial concentration" for local parameters.
      /// The compartment is always third last.
      /// </summary>
      /// <returns>index of the third-to-last object path</returns>
      private static int compartmentIndex(PathAndValueEntity x) => x.Path.Count - 3;

      /// <summary>
      /// When dealing with ExpressionParameters, the path takes the form "Organism|ArterialBlood|Plasma|GABRG2|Initial concentration" for local parameters.
      /// </summary>
      /// <returns>True if the path has more than 5 elements, indicating that compartment is present</returns>
      private static bool hasCompartment(ParameterValue formulaTarget) => formulaTarget.Path.Count >= 5;

      private IEnumerable<ParameterValue> createLocalFrom(IContainer container, IReadOnlyList<MoleculeBuilder> molecules) =>
         molecules.SelectMany(x => createLocalFrom(container, x));

      private IEnumerable<ParameterValue> createLocalFrom(IContainer container, MoleculeBuilder molecule) => 
         molecule.Parameters.Where(x => isLocalAndSatisfiesCriteria(x, container)).Select(x => CreateParameterValue(objectPathForParameterInContainer(container, x.Name, molecule.Name), x));

      private IEnumerable<ParameterValue> createExpressionFrom(IContainer container, MoleculeBuilder molecule)
      {
         var expressionParameters = _projectRetriever.CurrentProject.All<ExpressionProfileBuildingBlock>().Where(x => string.Equals(x.MoleculeName, molecule.Name)).SelectMany(x => x.ExpressionParameters).ToList();
         var expressionParameterNames = expressionParameters.AllNames().Distinct().ToList();
         var parameterValues = molecule.Parameters.Where(x => isLocalExpressionAndSatisfiesCriteria(x, container, expressionParameterNames))
            .Select(x => CreateParameterValue(objectPathForParameterInContainer(container, x.Name, molecule.Name), x)).ToList();

         // For newly created parameterValues that do not already have formulas, check for formulas in similar expression parameters
         updateFromExpression(parameterValues, expressionParameters);

         return parameterValues;
      }

      private bool isLocalExpressionAndSatisfiesCriteria(IParameter parameter, IContainer container, List<string> expressionParameterNames) => 
         parameter.BuildMode == ParameterBuildMode.Local && satisfiesContainerCriteria(parameter, container) && expressionParameterNames.Contains(parameter.Name);

      private bool isLocalAndSatisfiesCriteria(IParameter parameter, IContainer container) => 
         isLocalWithConstantFormula(parameter) && satisfiesContainerCriteria(parameter, container);

      private static bool isLocalWithConstantFormula(IParameter parameter) => parameter.BuildMode == ParameterBuildMode.Local && parameter.Formula.IsConstant();

      private static bool satisfiesContainerCriteria(IParameter parameter, IContainer container) => parameter.ContainerCriteria?.IsSatisfiedBy(container) ?? true;

      private ObjectPath objectPathForParameterInContainer(IContainer container, string parameterName, string moleculeName)
      {
         var pathForParameterInContainer = _entityPathResolver.ObjectPathFor(container);
         pathForParameterInContainer.AddRange(new[] { moleculeName, parameterName });
         return pathForParameterInContainer;
      }

      

      private static bool shouldSetValue(IParameter parameter)
      {
         return parameter.IsFixedValue || parameter.Formula == null || parameter.Formula.IsConstant();
      }

      public ParameterValue CreateEmptyStartValue(IDimension dimension) => CreateParameterValue(ObjectPath.Empty, 0.0, dimension);
   }
}