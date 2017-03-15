using System;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace OSPSuite.Core.Domain.Services
{
   public interface IQuantityValuesUpdater
   {
      /// <summary>
      ///    Update the values of all <see cref="Quantity" /> defined in the <paramref name="model" /> based on the values
      ///    provided in the <paramref name="buildConfiguration" />.
      ///    More specifically, <see cref="Parameter" /> values or formula as well as <see cref="MoleculeAmount" /> start values
      ///    or formula will be updated.
      /// </summary>
      /// <remarks>
      ///    This completly overrides the defaults defined in the <see cref="MoleculeBuildingBlock" /> and
      ///    <see cref="SpatialStructure" />.
      /// </remarks>
      /// <param name="model">The model whose quantities will be upadated</param>
      /// <param name="buildConfiguration">The build configuration containing molecule and paramter startvalues used to update.</param>
      void UpdateQuantitiesValues(IModel model, IBuildConfiguration buildConfiguration);
   }

   public class QuantityValuesUpdater : IQuantityValuesUpdater
   {
      private readonly IKeywordReplacerTask _keywordReplacerTask;
      private readonly ICloneManagerForModel _cloneManagerForModel;
      private readonly IFormulaFactory _formulaFactory;
      private readonly IConcentrationBasedFormulaUpdater _concentrationBasedFormulaUpdater;

      public QuantityValuesUpdater(IKeywordReplacerTask keywordReplacerTask, ICloneManagerForModel cloneManagerForModel,
         IFormulaFactory formulaFactory, IConcentrationBasedFormulaUpdater concentrationBasedFormulaUpdater)
      {
         _keywordReplacerTask = keywordReplacerTask;
         _cloneManagerForModel = cloneManagerForModel;
         _formulaFactory = formulaFactory;
         _concentrationBasedFormulaUpdater = concentrationBasedFormulaUpdater;
      }

      public void UpdateQuantitiesValues(IModel model, IBuildConfiguration buildConfiguration)
      {
         updateParameterValueFromParameterStartValues(model, buildConfiguration);
         updateMoleculeAmoutFromMoleculeStartValues(model, buildConfiguration);
      }

      private void updateParameterValueFromParameterStartValues(IModel model, IBuildConfiguration buildConfiguration)
      {
         foreach (var parameterStartValue in buildConfiguration.ParameterStartValues)
         {
            var pathInModel = _keywordReplacerTask.CreateModelPathFor(parameterStartValue.Path, model.Root);
            var parameter = pathInModel.Resolve<IParameter>(model.Root);

            //this can happen if the parameter belongs to a molecule locale properties and the molecule was not created in the container
            if (parameter == null)
               continue;

            if (parameterStartValue.Formula != null)
            {
               parameter.Formula = _cloneManagerForModel.Clone(parameterStartValue.Formula);
               _keywordReplacerTask.ReplaceIn(parameter, model.Root);
            }
            else
            {
               var constantFormula = parameter.Formula as ConstantFormula;
               var parameterValue = parameterStartValue.StartValue.GetValueOrDefault(double.NaN);
               if (constantFormula == null)
               {
                  if (parameterStartValue.OverrideFormulaWithValue)
                     parameter.Formula = _formulaFactory.ConstantFormula(parameterValue, parameter.Dimension);
                  else
                     parameter.Value = parameterValue;
               }
               else
               {
                  constantFormula.Value = parameterValue;
               }
            }

            //Ensure that parameter in simulation appears no to have been fixed by the user when updated from PSV
            if (parameterStartValue.OverrideFormulaWithValue)
               parameter.IsFixedValue = false;
         }
      }

      private void updateMoleculeAmoutFromMoleculeStartValues(IModel model, IBuildConfiguration buildConfiguration)
      {
         foreach (var moleculeStartValue in buildConfiguration.AllPresentMoleculeValues())
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

      private IFormula createConstantFormula(IMoleculeStartValue moleculeStartValue)
      {
         return _formulaFactory.ConstantFormula(moleculeStartValue.StartValue.Value, moleculeStartValue.Dimension);
      }

      private bool startValueShouldBeSetAsConstantFormula(IMoleculeStartValue moleculeStartValue, IMoleculeAmount molecule)
      {
         if (!moleculeStartValue.StartValue.HasValue)
            return false;

         var msvValue = moleculeStartValue.StartValue.Value;
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