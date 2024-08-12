﻿using System;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Services
{
   internal interface IQuantityValuesUpdater
   {
      /// <summary>
      ///    Update the values of all <see cref="Quantity" /> defined in the model based on the values
      ///    provided in the <paramref name="modelConfiguration" />.
      ///    More specifically, <see cref="Parameter" /> values or formula as well as <see cref="MoleculeAmount" /> start values
      ///    or formula will be updated.
      /// </summary>
      /// <remarks>
      ///    This completely overrides the defaults defined in the <see cref="MoleculeBuildingBlock" /> and
      ///    <see cref="SpatialStructure" />.
      /// </remarks>
      /// <param name="modelConfiguration">The model whose quantities will be updated</param>
      ValidationResult UpdateQuantitiesValues(ModelConfiguration modelConfiguration);
   }

   internal class QuantityValuesUpdater : IQuantityValuesUpdater
   {
      private readonly IKeywordReplacerTask _keywordReplacerTask;
      private readonly ICloneManagerForModel _cloneManagerForModel;
      private readonly IFormulaFactory _formulaFactory;
      private readonly IConcentrationBasedFormulaUpdater _concentrationBasedFormulaUpdater;
      private readonly IParameterValueToParameterMapper _parameterValueToParameterMapper;
      private readonly ValidatorForForFormula _formulaValidator;

      public QuantityValuesUpdater(
         IKeywordReplacerTask keywordReplacerTask,
         ICloneManagerForModel cloneManagerForModel,
         IFormulaFactory formulaFactory,
         IConcentrationBasedFormulaUpdater concentrationBasedFormulaUpdater,
         IParameterValueToParameterMapper parameterValueToParameterMapper,
         ValidatorForForFormula formulaValidator)
      {
         _keywordReplacerTask = keywordReplacerTask;
         _cloneManagerForModel = cloneManagerForModel;
         _formulaFactory = formulaFactory;
         _concentrationBasedFormulaUpdater = concentrationBasedFormulaUpdater;
         _parameterValueToParameterMapper = parameterValueToParameterMapper;
         _formulaValidator = formulaValidator;
      }

      public ValidationResult UpdateQuantitiesValues(ModelConfiguration modelConfiguration)
      {
         var valueUpdater = new ValueUpdaterParams(modelConfiguration);
         updateMoleculeAmountFromInitialConditions(modelConfiguration);

         //Add expressions profile before individual as some settings might be overwritten in the individual for aging
         updateParameterFromExpressionProfiles(valueUpdater);

         updateParameterFromIndividualValues(valueUpdater);

         //PV are applied last
         updateParameterValueFromParameterValues(valueUpdater);

         return valueUpdater.ValidationResult;
      }

      private void updateParameterFromExpressionProfiles(ValueUpdaterParams valueUpdater)
      {
         var addOrUpdateParameter = addOrUpdateParameterFromParameterValue(valueUpdater);
         valueUpdater.ModelConfiguration.SimulationConfiguration.ExpressionProfiles?.SelectMany(x => x.ExpressionParameters)
            .Each(addOrUpdateParameter);
      }

      private void updateParameterFromIndividualValues(ValueUpdaterParams valueUpdater)
      {
         var addOrUpdateParameter = addOrUpdateParameterFromParameterValue(valueUpdater);
         //Order by distribution to ensure that distributed parameter are loaded BEFORE their sub parameters.
         //note: use descending otherwise parameter without distribution are returned fist
         valueUpdater.ModelConfiguration.SimulationConfiguration.Individual?.OrderByDescending(x => x.DistributionType)
            .Each(addOrUpdateParameter);
      }

      private void updateParameterValueFromParameterValues(ValueUpdaterParams valueUpdater)
      {
         var addOrUpdateParameter = addOrUpdateParameterFromParameterValue(valueUpdater);
         valueUpdater.ModelConfiguration.SimulationBuilder.ParameterValues
            .Each(addOrUpdateParameter);
      }

      private IParameter getOrAddModelParameter(ValueUpdaterParams valueUpdater, ParameterValue parameterValue)
      {
         var (modelConfiguration, validationResult) = valueUpdater;
         var parameter = getParameter(modelConfiguration, parameterValue);
         if (parameter != null)
            return parameter.WithUpdatedMetaFrom(parameterValue);

         var (model, simulationBuilder, replacementContext) = modelConfiguration;
         //Parameter does not exist in the model. We will create it if possible
         var parentContainerPathInModel = _keywordReplacerTask.CreateModelPathFor(parameterValue.ContainerPath, replacementContext);
         var parentContainer = parentContainerPathInModel.Resolve<IContainer>(model.Root);

         //container does not exist, we do not add new structure to the existing model. Only parameters
         if (parentContainer == null)
         {
            validationResult.AddMessage(NotificationType.Warning, parameterValue, Warning.ContainerNotFoundParameterWillNotBeCreated(parentContainerPathInModel, parameterValue.ParameterName));
            return null;
         }

         parameter = _parameterValueToParameterMapper.MapFrom(parameterValue);

         simulationBuilder.AddBuilderReference(parameter, parameterValue);
         return parameter.WithParentContainer(parentContainer);
      }

      private IParameter getParameter(ModelConfiguration modelConfiguration, PathAndValueEntity pathAndValueEntity)
      {
         var (model, _, replacementContext) = modelConfiguration;
         var pathInModel = _keywordReplacerTask.CreateModelPathFor(pathAndValueEntity.Path, replacementContext);
         return pathInModel.Resolve<IParameter>(model.Root);
      }

      private Action<ParameterValue> addOrUpdateParameterFromParameterValue(ValueUpdaterParams valueUpdater) => parameterValue =>
      {
         var parameter = getOrAddModelParameter(valueUpdater, parameterValue);
         //this can happen if the parameter does not exist in the model
         if (parameter == null)
            return;

         var (model, _, replacementContext) = valueUpdater.ModelConfiguration;

         //Formula is defined, we update in the parameter instance
         if (parameterValue.Formula != null)
         {
            parameter.Formula = _cloneManagerForModel.Clone(parameterValue.Formula);

            //ensures that the parameter is seen as using the formula
            parameter.IsFixedValue = false;
            _keywordReplacerTask.ReplaceIn(parameter, replacementContext);
         }

         //If the value is defined, this will be used instead of the formula (even if set previously)
         if (!parameterValue.Value.IsValid())
            return;

         var actualParameterValue = parameterValue.Value.Value;
         if (parameter.Formula is ConstantFormula constantFormula)
         {
            constantFormula.Value = actualParameterValue;
            return;
         }

         //now we have a non constant formula. Let's try to see if the reference can be resolved. If yes, we will simply set the value of the parameter
         //Otherwise, we will create a new constant formula with the value

         if (_formulaValidator.IsFormulaValid(parameter))
            parameter.Value = actualParameterValue;
         else
            parameter.Formula = _formulaFactory.ConstantFormula(actualParameterValue, parameter.Dimension);
      };

      private void updateMoleculeAmountFromInitialConditions(ModelConfiguration modelConfiguration)
      {
         var (model, simulationBuilder, replacementContext) = modelConfiguration;
         foreach (var initialCondition in simulationBuilder.AllPresentMoleculeValues())
         {
            //this can happen if the initial condition contains entry for container that do not exist in the model
            var container = initialCondition.ContainerPath.Resolve<IContainer>(model.Root);
            if (container == null || container.Mode != ContainerMode.Physical)
               continue;

            var molecule = container.EntityAt<MoleculeAmount>(initialCondition.MoleculeName);
            if (molecule == null)
               throw new ArgumentException(Error.CouldNotFindMoleculeInContainer(initialCondition.MoleculeName, initialCondition.ContainerPath.PathAsString));

            if (initialCondition.Formula != null)
            {
               //use a clone here because we want a different instance for each molecule
               updateMoleculeAmountFormula(molecule, _cloneManagerForModel.Clone(initialCondition.Formula));
               _keywordReplacerTask.ReplaceIn(molecule, replacementContext);
            }
            else if (startValueShouldBeSetAsConstantFormula(initialCondition, molecule))
            {
               updateMoleculeAmountFormula(molecule, createConstantFormula(initialCondition));
            }

            molecule.ScaleDivisor = initialCondition.ScaleDivisor;
            molecule.NegativeValuesAllowed = initialCondition.NegativeValuesAllowed;
         }
      }

      /// <summary>
      ///    Ensures that the molecule is using the <paramref name="moleculeFormulaToUse" /> for its start value,
      ///    either directly or via the StartValue parameter.
      /// </summary>
      /// <remarks>
      ///    The <paramref name="moleculeFormulaToUse" /> can be changed since this is suppose to be a clone of the
      ///    original initial condition
      /// </remarks>
      private void updateMoleculeAmountFormula(MoleculeAmount molecule, IFormula moleculeFormulaToUse)
      {
         if (moleculeFormulaToUse.IsAmountBased())
         {
            molecule.Formula = moleculeFormulaToUse;
            return;
         }

         //molecule is concentration based
         var startValue = molecule.EntityAt<IParameter>(Constants.Parameters.START_VALUE);
         startValue.Formula = moleculeFormulaToUse;
         startValue.Dimension = startValue.Formula.Dimension;
         _concentrationBasedFormulaUpdater.UpdateRelativePathForStartValueMolecule(molecule, moleculeFormulaToUse);

         molecule.Formula = _formulaFactory.CreateMoleculeAmountReferenceToStartValue(startValue);
      }

      private IFormula createConstantFormula(InitialCondition initialCondition)
      {
         return _formulaFactory.ConstantFormula(initialCondition.Value.Value, initialCondition.Dimension);
      }

      private bool startValueShouldBeSetAsConstantFormula(InitialCondition initialCondition, MoleculeAmount molecule)
      {
         if (!initialCondition.Value.HasValue)
            return false;

         var msvValue = initialCondition.Value.Value;
         double? currentConstantMoleculeValue;

         if (initialCondition.IsAmountBased())
            currentConstantMoleculeValue = calculateConstantValueFor(molecule);
         else
         {
            var startValue = molecule.EntityAt<IParameter>(Constants.Parameters.START_VALUE);
            currentConstantMoleculeValue = calculateConstantValueFor(startValue);
         }

         //this was not defined using a constant formula, the value should be set
         if (currentConstantMoleculeValue == null)
            return true;

         //Formula is constant. Start value should be set if value does not equal current constant value
         return !ValueComparer.AreValuesEqual(currentConstantMoleculeValue.Value, msvValue);
      }

      private static double? calculateConstantValueFor(IUsingFormula usingFormula)
      {
         if (usingFormula.Formula.IsConstant())
            return usingFormula.Formula.Calculate(usingFormula);
         return null;
      }

      private class ValueUpdaterParams
      {
         public ModelConfiguration ModelConfiguration { get; }
         public ValidationResult ValidationResult { get; }

         public ValueUpdaterParams(ModelConfiguration modelConfiguration)
         {
            ModelConfiguration = modelConfiguration;
            ValidationResult = new ValidationResult();
         }

         public void Deconstruct(out ModelConfiguration modelConfiguration, out ValidationResult validationResult)
         {
            modelConfiguration = ModelConfiguration;
            validationResult = ValidationResult;
         }
      }
   }
}