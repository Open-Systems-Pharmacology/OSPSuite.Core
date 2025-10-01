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
   ///    its children as well
   /// </summary>
   public interface IEntityValidator
   {
      /// <summary>
      ///    Validates the given entity (for a container, all its children as well).
      /// </summary>
      ValidationResult Validate(IObjectBase objectBase);
   }

   public class EntityValidator : IEntityValidator
   {
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
         var toValidate = new List<IObjectBase>();
         if (objectBase is IModelCoreSimulation sim)
         {
            toValidate.Add(sim.Model.Root);
            toValidate.Add(sim.Settings);
         }
         else
         {
            toValidate.Add(objectBase);
         }

         var collector = new Collector(_parameterIdentificationValidator, _sensitivityAnalysisValidator);

         foreach (var entity in toValidate)
            entity.AcceptVisitor(collector);

         return collector.Result;
      }
       
      private sealed class Collector :
         IVisitor<IEntity>,
         IVisitor<IParameter>,
         IVisitor<OutputSchema>,
         IVisitor<ParameterIdentification>,
         IVisitor<SensitivityAnalysis>
      {
         private readonly IParameterIdentificationValidator _piv;
         private readonly ISensitivityAnalysisValidator _sav;

         public ValidationResult Result { get; } = new ValidationResult();

         public Collector(IParameterIdentificationValidator piv, ISensitivityAnalysisValidator sav)
         {
            _piv = piv;
            _sav = sav;
         }

         public void Visit(IEntity entity)
         {
            var broken = entity.Validate();
            if (broken.IsEmpty) return;
            broken.All().Each(rule => Result.AddMessage(NotificationType.Error, entity, rule.Description));
         }

         public void Visit(IParameter parameter)
         {
            if (parameter.Visible)
               Visit((IEntity)parameter);
         }

         public void Visit(OutputSchema outputSchema)
         {
            var points = Convert.ToInt32(outputSchema.Intervals.Sum(x => x.Resolution.Value * (x.EndTime.Value - x.StartTime.Value)));
            if (points > Constants.MAX_NUMBER_OF_SUGGESTED_OUTPUT_POINTS)
               Result.AddMessage(NotificationType.Warning, outputSchema, Warning.LargeNumberOfOutputPoints(points));
         }

         public void Visit(ParameterIdentification pi)
            => Result.AddMessagesFrom(_piv.Validate(pi));

         public void Visit(SensitivityAnalysis sa)
            => Result.AddMessagesFrom(_sav.Validate(sa));
      }
   }
}