using System.Collections.Generic;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Validation;
using OSPSuite.Utility.Visitor;

namespace OSPSuite.Core.Domain.Services
{
   /// <summary>
   ///    Is responsible to perform a validation of all rules defined in an entity. If the entity is a container, validate all
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
      private readonly IParameterIdentificationValidator _parameterIdentificationValidator;
      private readonly ISensitivityAnalysisValidator _sensitivityAnalysisValidator;

      public EntityValidator(IParameterIdentificationValidator parameterIdentificationValidator,
         ISensitivityAnalysisValidator sensitivityAnalysisValidator)
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
         }
         else
            objectsToValidate.Add(objectBase);

         return validateEntities(objectsToValidate);
      }

      private ValidationResult validateEntities(IReadOnlyList<IObjectBase> entities)
      {
         try
         {
            _validationResult = new ValidationResult();
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
         //only validate visible parameter
         if (!parameter.Visible)
            return;

         Visit((IEntity) parameter);
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