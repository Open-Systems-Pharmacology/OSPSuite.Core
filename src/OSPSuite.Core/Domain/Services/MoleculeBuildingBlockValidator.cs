using System.Collections.Generic;
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
      ValidationResult Validate(MoleculeBuildingBlock moleculeBuildingBlock);

      ValidationResult Validate(IReadOnlyCollection<MoleculeBuilder> molecules);
   }

   internal class MoleculeBuildingBlockValidator : IMoleculeBuildingBlockValidator
   {
      public ValidationResult Validate(MoleculeBuildingBlock moleculeBuildingBlock) => Validate(moleculeBuildingBlock.ToList());

      public ValidationResult Validate(IReadOnlyCollection<MoleculeBuilder> molecules)
      {
         var validationResults = new ValidationResult();
         foreach (var molecule in molecules.Where(m => m.IsFloating))
         {
            foreach (var parameter in molecule.Parameters.Where(parameterIsInvalid))
            {
               validationResults.AddMessage(NotificationType.Error, parameter, Error.FloatingMoleculeParameterNotDefined(molecule.Name, parameter.Name, parameter.Value), molecule.BuildingBlock);
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