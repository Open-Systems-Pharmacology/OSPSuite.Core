using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Validation;
using OSPSuite.Utility.Visitor;

namespace OSPSuite.Core.Domain.Services
{
   /// <summary>
   ///    Is responsible to perform a validation of all rules defined in an entity. If the entity is a container, validate all
   ///    its children as well.
   ///    This class is not thread-safe. See <seealso cref="IEntityValidatorFactory" /> to create thread instances
   /// </summary>
   public class EntityValidator :
      IVisitor<IEntity>,
      IVisitor<IParameter>,
      IVisitor<OutputSchema>,
      IVisitor<ParameterIdentification>,
      IVisitor<SensitivityAnalysis>
   {
      private ValidationResult _validationResult;
      private readonly IParameterIdentificationValidator _parameterIdentificationValidator;
      private readonly ISensitivityAnalysisValidator _sensitivityAnalysisValidator;

      public EntityValidator(
         IParameterIdentificationValidator parameterIdentificationValidator,
         ISensitivityAnalysisValidator sensitivityAnalysisValidator)
      {
         _parameterIdentificationValidator = parameterIdentificationValidator;
         _sensitivityAnalysisValidator = sensitivityAnalysisValidator;
      }

      public ValidationResult Validate(IObjectBase objectBase)
      {
         var objectsToValidate = new List<IObjectBase>();

         if (objectBase is IModelCoreSimulation simulation)
         {
            objectsToValidate.Add(simulation.Model.Root);
            objectsToValidate.Add(simulation.Settings);
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

         Visit((IEntity)parameter);
      }

      public void Visit(OutputSchema outputSchema)
      {
         //For output schema, we calculate the number of overall generated points and if it's bigger than the max suggested, warning
         var numberOfGeneratedPoints = Convert.ToInt32(outputSchema.Intervals.Sum(x => x.Resolution.Value * (x.EndTime.Value - x.StartTime.Value)));
         if (numberOfGeneratedPoints > Constants.MAX_NUMBER_OF_SUGGESTED_OUTPUT_POINTS)
            addValidationMessage(NotificationType.Warning, outputSchema, Warning.LargeNumberOfOutputPoints(numberOfGeneratedPoints));
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