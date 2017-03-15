using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace OSPSuite.Core.Domain.Services
{
   public interface IMoleculeBuildingBlockValidator
   {
      /// <summary>
      ///    Validates the molecule building block an ensure that all floating molecules have
      ///    well defined constant parameters (i.e. no constant parameter remains with a value equal to NaN
      /// </summary>
      /// <param name="moleculeBuildingBlock">Molecule building block to validate</param>
      /// <returns>The validation results corresponding to the validation</returns>
      ValidationResult Validate(IMoleculeBuildingBlock moleculeBuildingBlock);
   }

   internal class MoleculeBuildingBlockValidator : IMoleculeBuildingBlockValidator
   {
      public ValidationResult Validate(IMoleculeBuildingBlock moleculeBuildingBlock)
      {
         var validationResults = new ValidationResult();

         foreach (var molecule in moleculeBuildingBlock.Where(m => m.IsFloating))
         {
            foreach (var parameter in molecule.Parameters.Where(parameterIsInvalid))
            {
               validationResults.AddMessage(NotificationType.Error, parameter, Error.FloatingMoleculeParameterNotDefined(molecule.Name, parameter.Name, parameter.Value), moleculeBuildingBlock);
            }
         }

         return validationResults;
      }

      private bool parameterIsInvalid(IParameter parameter)
      {
         return parameter.Formula.IsConstant() && double.IsNaN(parameter.Value);
      }
   }
}