using System;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Mappers;
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
      void UpdateQuantitiesValues(ModelConfiguration modelConfiguration);
   }

   internal class QuantityValuesUpdater : IQuantityValuesUpdater
   {
      private readonly IKeywordReplacerTask _keywordReplacerTask;
      private readonly ICloneManagerForModel _cloneManagerForModel;
      private readonly IFormulaFactory _formulaFactory;
      private readonly IConcentrationBasedFormulaUpdater _concentrationBasedFormulaUpdater;
      private readonly IIndividualParameterToParameterMapper _individualParameterToParameterMapper;

      public QuantityValuesUpdater(
         IKeywordReplacerTask keywordReplacerTask,
         ICloneManagerForModel cloneManagerForModel,
         IFormulaFactory formulaFactory,
         IConcentrationBasedFormulaUpdater concentrationBasedFormulaUpdater,
         IIndividualParameterToParameterMapper individualParameterToParameterMapper)
      {
         _keywordReplacerTask = keywordReplacerTask;
         _cloneManagerForModel = cloneManagerForModel;
         _formulaFactory = formulaFactory;
         _concentrationBasedFormulaUpdater = concentrationBasedFormulaUpdater;
         _individualParameterToParameterMapper = individualParameterToParameterMapper;
      }

      public void UpdateQuantitiesValues(ModelConfiguration modelConfiguration)
      {
         updateMoleculeAmountFromInitialConditions(modelConfiguration);

         //Add expressions profile before individual as some settings might be overwritten in the individual for aging
         updateParameterFromExpressionProfiles(modelConfiguration);

         updateParameterFromIndividualValues(modelConfiguration);

         //PV are applied last
         updateParameterValueFromParameterValues(modelConfiguration);
      }

      private void updateParameterFromExpressionProfiles(ModelConfiguration modelConfiguration)
      {
         modelConfiguration.SimulationConfiguration.ExpressionProfiles?.SelectMany(x => x.ExpressionParameters).Each(x => updateParameterValueFromStartValue(modelConfiguration, x, getParameter));
      }

      private void updateParameterFromIndividualValues(ModelConfiguration modelConfiguration)
      {
         //Order by distribution to ensure that distributed parameter are loaded BEFORE their sub parameters.
         //note: use descending otherwise parameter without distribution are returned fist
         modelConfiguration.SimulationConfiguration.Individual?.OrderByDescending(x => x.DistributionType)
            .Each(x => updateParameterValueFromStartValue(modelConfiguration, x, getOrAddModelParameter));
      }

      private void updateParameterValueFromParameterValues(ModelConfiguration modelConfiguration)
      {
         modelConfiguration.SimulationBuilder.ParameterValues
            .Each(pv => updateParameterValueFromStartValue(modelConfiguration, pv, getParameter));
      }

      private IParameter getOrAddModelParameter(ModelConfiguration modelConfiguration, IndividualParameter individualParameter)
      {
         var parameter = getParameter(modelConfiguration, individualParameter);
         if (parameter != null)
            return parameter.WithUpdatedMetaFrom(individualParameter);

         var (model, simulationConfiguration) = modelConfiguration;
         //Parameter does not exist in the model. We will create it if possible
         var parentContainerPathInModel = _keywordReplacerTask.CreateModelPathFor(individualParameter.ContainerPath, model.Root);
         var parentContainer = parentContainerPathInModel.Resolve<IContainer>(model.Root);

         //container does not exist, we do not add new structure to the existing model. Only parameters
         if (parentContainer == null)
            return null;

         parameter = _individualParameterToParameterMapper.MapFrom(individualParameter);

         simulationConfiguration.AddBuilderReference(parameter, individualParameter);
         return parameter.WithParentContainer(parentContainer);
      }

      private IParameter getParameter(ModelConfiguration modelConfiguration, PathAndValueEntity pathAndValueEntity)
      {
         var (model, _) = modelConfiguration;
         var pathInModel = _keywordReplacerTask.CreateModelPathFor(pathAndValueEntity.Path, model.Root);
         return pathInModel.Resolve<IParameter>(model.Root);
      }

      private void updateParameterValueFromStartValue<T>(ModelConfiguration modelConfiguration, T pathAndValueEntity, Func<ModelConfiguration, T, IParameter> getParameterFunc) where T : PathAndValueEntity
      {
         var parameter = getParameterFunc(modelConfiguration, pathAndValueEntity);
         //this can happen if the parameter does not exist in the model
         if (parameter == null)
            return;

         var (model, _) = modelConfiguration;

         //Formula is defined, we update in the parameter instance
         if (pathAndValueEntity.Formula != null)
         {
            parameter.Formula = _cloneManagerForModel.Clone(pathAndValueEntity.Formula);

            //ensures that the parameter is seen as using the formula
            parameter.IsFixedValue = false;
            _keywordReplacerTask.ReplaceIn(parameter, model.Root);
         }

         //If the value is defined, this will be used instead of the formula (even if set previously)
         if (pathAndValueEntity.Value != null)
         {
            var parameterValue = pathAndValueEntity.Value.Value;
            if (parameter.Formula is ConstantFormula constantFormula)
               constantFormula.Value = parameterValue;
            else
               parameter.Value = parameterValue;
         }
      }

      private void updateMoleculeAmountFromInitialConditions(ModelConfiguration modelConfiguration)
      {
         var (model, simulationConfiguration) = modelConfiguration;
         foreach (var initialCondition in simulationConfiguration.AllPresentMoleculeValues())
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
               _keywordReplacerTask.ReplaceIn(molecule, model.Root);
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
   }
}