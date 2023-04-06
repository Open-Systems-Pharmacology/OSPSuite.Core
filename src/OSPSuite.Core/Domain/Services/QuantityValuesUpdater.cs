using System;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Services
{
   public interface IQuantityValuesUpdater
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

   public class QuantityValuesUpdater : IQuantityValuesUpdater
   {
      private readonly IKeywordReplacerTask _keywordReplacerTask;
      private readonly ICloneManagerForModel _cloneManagerForModel;
      private readonly IFormulaFactory _formulaFactory;
      private readonly IConcentrationBasedFormulaUpdater _concentrationBasedFormulaUpdater;
      private readonly IParameterFactory _parameterFactory;

      public QuantityValuesUpdater(
         IKeywordReplacerTask keywordReplacerTask,
         ICloneManagerForModel cloneManagerForModel,
         IFormulaFactory formulaFactory,
         IConcentrationBasedFormulaUpdater concentrationBasedFormulaUpdater,
         IParameterFactory parameterFactory)
      {
         _keywordReplacerTask = keywordReplacerTask;
         _cloneManagerForModel = cloneManagerForModel;
         _formulaFactory = formulaFactory;
         _concentrationBasedFormulaUpdater = concentrationBasedFormulaUpdater;
         _parameterFactory = parameterFactory;
      }

      public void UpdateQuantitiesValues(ModelConfiguration modelConfiguration)
      {
         updateMoleculeAmountFromMoleculeStartValues(modelConfiguration);

         updateParameterFromIndividualValues(modelConfiguration);

         updateParameterFromExpressionProfiles(modelConfiguration);

         //PSV are applied last
         updateParameterValueFromParameterStartValues(modelConfiguration);
      }

      private void updateParameterFromExpressionProfiles(ModelConfiguration modelConfiguration)
      {
         modelConfiguration.SimulationConfiguration.ExpressionProfiles?.SelectMany(x => x).Each(x => updateParameterValueFromStartValue(modelConfiguration, x, getParameter));
      }

      private void updateParameterFromIndividualValues(ModelConfiguration modelConfiguration)
      {
         //Order by distribution to ensure that distributed parameter are loaded BEFORE their sub parameters.
         //not use descending otherwise parameter without distribution are returned fist
         modelConfiguration.SimulationConfiguration.Individual?.OrderByDescending(x => x.DistributionType)
            .Each(x => updateParameterValueFromStartValue(modelConfiguration, x, getOrAddModelParameter));
      }

      private void updateParameterValueFromParameterStartValues(ModelConfiguration modelConfiguration)
      {
         modelConfiguration.SimulationConfiguration.ParameterStartValues.SelectMany(x => x)
            .Each(psv => updateParameterValueFromStartValue(modelConfiguration, psv, getParameter));
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

         var name = individualParameter.Name;
         var dimension = individualParameter.Dimension;
         var displayUnit = individualParameter.DisplayUnit;
         var distributionType = individualParameter.DistributionType;

         //if the distribution is undefined or the value is set, we create a default parameter to ensure that the value will take precedence.
         //Otherwise, we create a distributed parameter and assume that required sub-parameters will be created as well
         parameter = distributionType == null || individualParameter.Value != null ? 
            _parameterFactory.CreateParameter(name, dimension: dimension, displayUnit: displayUnit) : 
            _parameterFactory.CreateDistributedParameter(name, distributionType.Value, dimension: dimension, displayUnit: displayUnit);

         simulationConfiguration.AddBuilderReference(parameter, individualParameter);
         return parameter.WithUpdatedMetaFrom(individualParameter)
            .WithParentContainer(parentContainer);
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

      private void updateMoleculeAmountFromMoleculeStartValues(ModelConfiguration modelConfiguration)
      {
         var (model, simulationConfiguration) = modelConfiguration;
         foreach (var moleculeStartValue in simulationConfiguration.AllPresentMoleculeValues())
         {
            //this can happen if the molecule start value contains entry for container that do not exist in the model
            var container = moleculeStartValue.ContainerPath.Resolve<IContainer>(model.Root);
            if (container == null || container.Mode != ContainerMode.Physical)
               continue;

            var molecule = container.EntityAt<IMoleculeAmount>(moleculeStartValue.MoleculeName);
            if (molecule == null)
               throw new ArgumentException(Error.CouldNotFindMoleculeInContainer(moleculeStartValue.MoleculeName, moleculeStartValue.ContainerPath.PathAsString));

            if (moleculeStartValue.Formula != null)
            {
               //use a clone here because we want a different instance for each molecule
               updateMoleculeAmountFormula(molecule, _cloneManagerForModel.Clone(moleculeStartValue.Formula));
               _keywordReplacerTask.ReplaceIn(molecule, model.Root);
            }
            else if (startValueShouldBeSetAsConstantFormula(moleculeStartValue, molecule))
            {
               updateMoleculeAmountFormula(molecule, createConstantFormula(moleculeStartValue));
            }

            molecule.ScaleDivisor = moleculeStartValue.ScaleDivisor;
            molecule.NegativeValuesAllowed = moleculeStartValue.NegativeValuesAllowed;
         }
      }

      /// <summary>
      ///    Ensures that the molecule is using the <paramref name="moleculeFormulaToUse" /> for its start value,
      ///    either directly or via the StartValue parameter.
      /// </summary>
      /// <remarks>
      ///    The <paramref name="moleculeFormulaToUse" /> can be changed since this is suppose to be a clone of the
      ///    original molecule start value
      /// </remarks>
      private void updateMoleculeAmountFormula(IMoleculeAmount molecule, IFormula moleculeFormulaToUse)
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

      private IFormula createConstantFormula(MoleculeStartValue moleculeStartValue)
      {
         return _formulaFactory.ConstantFormula(moleculeStartValue.Value.Value, moleculeStartValue.Dimension);
      }

      private bool startValueShouldBeSetAsConstantFormula(MoleculeStartValue moleculeStartValue, IMoleculeAmount molecule)
      {
         if (!moleculeStartValue.Value.HasValue)
            return false;

         var msvValue = moleculeStartValue.Value.Value;
         double? currentConstantMoleculeValue;

         if (moleculeStartValue.IsAmountBased())
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