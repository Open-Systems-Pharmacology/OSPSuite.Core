using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Utility.Extensions;

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
      ValidationResult Validate( MoleculeBuildingBlock moleculeBuildingBlock);
      ValidationResult Validate(IReadOnlyList<MoleculeBuildingBlock> moleculeBuildingBlockList);
   }

   internal class MoleculeBuildingBlockValidator : IMoleculeBuildingBlockValidator
   {
      public ValidationResult Validate(MoleculeBuildingBlock moleculeBuildingBlock) => Validate(new[]{moleculeBuildingBlock });

      private void validate(MoleculeBuildingBlock moleculeBuildingBlock, ValidationResult validationResults)
      {
         foreach (var molecule in moleculeBuildingBlock.Where(m => m.IsFloating))
         {
            foreach (var parameter in molecule.Parameters.Where(parameterIsInvalid))
            {
               validationResults.AddMessage(NotificationType.Error, parameter, Error.FloatingMoleculeParameterNotDefined(molecule.Name, parameter.Name, parameter.Value), moleculeBuildingBlock);
            }
         }
      }

      public ValidationResult Validate(IReadOnlyList<MoleculeBuildingBlock> moleculeBuildingBlockList)
      {
         var validationResults = new ValidationResult();
         moleculeBuildingBlockList.Each(moleculeBuildingBlock => validate(moleculeBuildingBlock, validationResults));
         return validationResults;
      }

      private bool parameterIsInvalid(IParameter parameter)
      {
         return parameter.Formula.IsConstant() && double.IsNaN(parameter.Value);
      }
   }
}