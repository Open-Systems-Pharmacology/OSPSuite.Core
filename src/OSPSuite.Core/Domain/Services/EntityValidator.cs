using System.Collections.Generic;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Validation;
using OSPSuite.Utility.Visitor;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.SensitivityAnalyses;

namespace OSPSuite.Core.Domain.Services
{
   /// <summary>
   ///    Is reponsible to perform a validation of all rules defined in an entity. If the entity is a container, vlidate all
   ///    its children as well
   /// </summary>
   public interface IEntityValidator
   {
      /// <summary>
      ///    Validates the given entity (for a container, all its children as well).
      /// </summary>
      ValidationResult Validate(IObjectBase objectBase);
   }

   public class EntityValidator : IEntityValidator,
      IVisitor<IEntity>,
      IVisitor<IParameter>,
      IVisitor<ParameterIdentification>,
      IVisitor<SensitivityAnalysis>
   {
      protected ValidationResult _validationResult;
      private IReadOnlyList<string> _compoundNames;
      private readonly IParameterIdentificationValidator _parameterIdentificationValidator;
      private readonly ISensitivityAnalysisValidator _sensitivityAnalysisValidator;

      public EntityValidator(IParameterIdentificationValidator parameterIdentificationValidator, ISensitivityAnalysisValidator sensitivityAnalysisValidator)
      {
         _parameterIdentificationValidator = parameterIdentificationValidator;
         _sensitivityAnalysisValidator = sensitivityAnalysisValidator;
      }

      public ValidationResult Validate(IObjectBase objectBase)
      {
         var objectsToValidate = new List<IObjectBase>();
         IReadOnlyList<string> compoundNames = new List<string>();
         var simulation = objectBase as ISimulation;
         if (simulation != null)
         {
            objectsToValidate.Add(simulation.Model.Root);
            objectsToValidate.Add(simulation.SimulationSettings);
            compoundNames = simulation.CompoundNames;
         }
         else
            objectsToValidate.Add(objectBase);

         return validateEntities(objectsToValidate, compoundNames);
      }

      private ValidationResult validateEntities(IReadOnlyList<IObjectBase> entities, IReadOnlyList<string> compoundNames)
      {
         try
         {
            _validationResult = new ValidationResult();
            _compoundNames = compoundNames;
            entities.Each(entity => entity.AcceptVisitor(this));
            return _validationResult;
         }
         finally
         {
            _validationResult = null;
         }
      }

      public void Visit(IEntity entity)
      {
         var brokenRules = entity.Validate();
         if (brokenRules.IsEmpty) return;

         brokenRules.All().Each(rule => addRuleToValidation(rule, entity));
      }

      public void Visit(IParameter parameter)
      {
         //only validate visible parmaeter
         if (!parameter.Visible)
            return;

         //do not validate parameter defined in molecule amount for other molecule than drug (value cannot be set by user and some values are invalid)
         if (parentIsAMoleculeButNotDrug(parameter.ParentContainer))
            return;

         Visit((IEntity) parameter);
      }

      private bool parentIsAMoleculeButNotDrug(IContainer parent)
      {
         return parent != null
                && parent.ContainerType == ContainerType.Molecule
                && !parent.NameIsOneOf(_compoundNames);
      }

      private void addRuleToValidation(IBusinessRule rule, IObjectBase objectBase)
      {
         addValidationMessage(NotificationType.Error, objectBase, rule.Description);
      }

      private void addValidationMessage(NotificationType type, IObjectBase objectBase, string message)
      {
         _validationResult.AddMessage(type, objectBase, message);
      }

      public void Visit(ParameterIdentification parameterIdentification)
      {
         _validationResult.AddMessagesFrom(_parameterIdentificationValidator.Validate(parameterIdentification));
      }

      public void Visit(SensitivityAnalysis sensitivityAnalysis)
      {
         _validationResult.AddMessagesFrom(_sensitivityAnalysisValidator.Validate(sensitivityAnalysis));
      }
   }
}