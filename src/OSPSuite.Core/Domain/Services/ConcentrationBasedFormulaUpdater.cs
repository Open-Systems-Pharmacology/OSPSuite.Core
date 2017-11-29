using System.Globalization;
using System.Linq;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Domain.Services
{
   /// <summary>
   /// This service is in charge of creating a new formula based on the one defined in the given entity. The created formula will  be "amount based". That means that the original formula
   /// will be multiplied with the Volume of the container containing the entity
   /// </summary>
   public interface IConcentrationBasedFormulaUpdater
   {
      /// <summary>
      /// Creates a formula in <see cref="Constants.Dimension.AMOUNT_PER_TIME"/> based on the concentration based formula of the given <paramref name="process"/> 
      /// </summary>
      ExplicitFormula CreateAmountBaseFormulaFor(IProcess process);

      /// <summary>
      /// Creates a formula in <see cref="Constants.Dimension.AMOUNT"/> based on the given <paramref name="concentrationFormula"/>.
      /// </summary>
      ExplicitFormula CreateAmountBaseFormulaFor(IFormula concentrationFormula);

      /// <summary>
      /// Updates the relative <see cref="ObjectPath"/> used in the <paramref name="moleculeFormulaToUse"/> to ensure that references can be resolved.
      /// This is required because the formula was designed for the <paramref name="molecule"/> but will be silently attached to the start value parameter
      /// defined under the <paramref name="molecule"/>.
      /// </summary>
      void UpdateRelativePathForStartValueMolecule(IMoleculeAmount molecule, IFormula moleculeFormulaToUse);
   }

   public class ConcentrationBasedFormulaUpdater : IConcentrationBasedFormulaUpdater
   {
      private readonly ICloneManagerForModel _cloneManagerForModel;
      private readonly IObjectBaseFactory _objectBaseFactory;
      private readonly IDimensionFactory _dimensionFactory;
      private readonly IFormulaTask _formulaTask;

      public ConcentrationBasedFormulaUpdater(ICloneManagerForModel cloneManagerForModel,  
         IObjectBaseFactory objectBaseFactory,IDimensionFactory dimensionFactory,  IFormulaTask formulaTask)
      {
         _cloneManagerForModel = cloneManagerForModel;
         _objectBaseFactory = objectBaseFactory;
         _dimensionFactory = dimensionFactory;
         _formulaTask = formulaTask;
      }

      private ExplicitFormula updateFormulaToAmountBase(IFormula originalFormula, IDimension amountDimension)
      {
         ExplicitFormula formulaInAmount;
         if (originalFormula.IsExplicit())
            formulaInAmount = _cloneManagerForModel.Clone(originalFormula.DowncastTo<ExplicitFormula>());
         else
            formulaInAmount = _objectBaseFactory.Create<ExplicitFormula>()
               .WithFormulaString(originalFormula.Calculate(null).ToString(CultureInfo.InvariantCulture));

         formulaInAmount.Dimension = amountDimension;

         var volumeAlias = _formulaTask.AddParentVolumeReferenceToFormula(formulaInAmount);
         formulaInAmount.FormulaString = $"({formulaInAmount.FormulaString})*{volumeAlias}";
          return formulaInAmount;
      }

      public ExplicitFormula CreateAmountBaseFormulaFor(IProcess process)
      {
         return updateFormulaToAmountBase(process.Formula, _dimensionFactory.Dimension(Constants.Dimension.AMOUNT_PER_TIME));
      }

      public ExplicitFormula CreateAmountBaseFormulaFor(IFormula concentrationFormula)
      {
         return updateFormulaToAmountBase(concentrationFormula, _dimensionFactory.Dimension(Constants.Dimension.AMOUNT));
      }

      public void UpdateRelativePathForStartValueMolecule(IMoleculeAmount molecule, IFormula moleculeFormulaToUse)
      {
         foreach (var objectPath in moleculeFormulaToUse.ObjectPaths.Where(x=>x.Any()))
         {
            var firstEntry = objectPath.ElementAt(0);
            if(string.Equals(firstEntry,ObjectPath.PARENT_CONTAINER) || molecule.ContainsName(firstEntry))
               objectPath.AddAtFront(ObjectPath.PARENT_CONTAINER);
         }

      }
   }
}