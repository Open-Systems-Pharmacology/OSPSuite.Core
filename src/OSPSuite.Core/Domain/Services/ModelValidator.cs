using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility.Visitor;

namespace OSPSuite.Core.Domain.Services
{
   internal enum ResolveErrorBehavior
   {
      // add an error to the log when an object path cannot be resolved
      Error,

      // Delete the object and add a warning
      DeleteAndWarning,

      // Delete and no warning
      Delete
   }

   public interface IModelValidator
   {
      ValidationResult Validate(IObjectBase objectToValidate, IBuildConfiguration buildConfiguration);
   }

   /// <summary>
   ///    Base class for Validation tasks in ModelCore
   /// </summary>
   internal abstract class ModelValidator : IModelValidator, IVisitor
   {
      private readonly IObjectTypeResolver _objectTypeResolver;
      private readonly IObjectPathFactory _objectPathFactory;
      private readonly IEnumerable<string> _keywords;
      private IBuildConfiguration _buildConfiguration;
      private ValidationResult _result;

      protected ModelValidator(IObjectTypeResolver objectTypeResolver, IObjectPathFactory objectPathFactory)
      {
         _objectTypeResolver = objectTypeResolver;
         _objectPathFactory = objectPathFactory;
         _keywords = ObjectPathKeywords.All;
      }

      /// <summary>
      ///    try to resolve all the references used in the formula from the usingFormula object.
      ///    if one reference cannot be resolved, a validation message with info on the missing reference.
      /// </summary>
      /// <param name="usingFormulaToCheck">The using formula object to check.</param>
      /// <param name="resolveErrorBehavior">Specifies the behavior of the method when a reference is not found</param>
      protected void CheckReferences(IUsingFormula usingFormulaToCheck, ResolveErrorBehavior resolveErrorBehavior = ResolveErrorBehavior.Error)
      {
         CheckFormulaIn(usingFormulaToCheck, usingFormulaToCheck.Formula, resolveErrorBehavior);
      }

      protected void CheckParameter(IParameter parameterToCheck, ResolveErrorBehavior resolveErrorBehavior = ResolveErrorBehavior.Error)
      {
         CheckReferences(parameterToCheck, resolveErrorBehavior);
         if (parameterToCheck.RHSFormula == null) return;
         CheckFormulaIn(parameterToCheck, parameterToCheck.RHSFormula, resolveErrorBehavior);
      }

      protected void CheckFormulaIn(IUsingFormula entity, IFormula formulaToCheck, ResolveErrorBehavior resolveErrorBehavior)
      {
         var entityAbsolutePath = _objectPathFactory.CreateAbsoluteObjectPath(entity).ToPathString();
         var builder = _buildConfiguration.BuilderFor(entity);
         var objectWithError = builder ?? entity;

         // Dynamic formula may contain object path that will be resolved per instance. It cannot be checked here
         if (formulaToCheck.IsDynamic())
            return;


         if (formulaToCheck.IsBlackBox())
         {
            addNotificationType(NotificationType.Error, objectWithError, Validation.FormulaIsBlackBoxIn(entity.Name, entityAbsolutePath));
            return;
         }

         foreach (var objectPath in formulaToCheck.ObjectPaths)
         {
            CheckPath(entity, objectPath, resolveErrorBehavior);
         }
      }

      protected void CheckPath(IUsingFormula entity, IObjectPath objectPathToCheck, ResolveErrorBehavior resolveErrorBehavior)
      {
         var builder = _buildConfiguration.BuilderFor(entity);
         var objectWithError = builder ?? entity;
         var entityAbsolutePath = _objectPathFactory.CreateAbsoluteObjectPath(entity).ToString();
         var entityType = _objectTypeResolver.TypeFor(entity);

         if (containsKeyWords(objectPathToCheck))
         {
            addNotificationType(NotificationType.Error, objectWithError, Validation.PathContainsReservedKeywords(entity.Name, entityType, entityAbsolutePath, objectPathToCheck.ToPathString()));
            return;
         }

         //found, we continue
         if (objectPathToCheck.Resolve<IFormulaUsable>(entity) != null) return;

         var message = Validation.ErrorUnableToFindReference(entity.Name, entityType, entityAbsolutePath, objectPathToCheck.ToPathString());

         if (resolveErrorBehavior == ResolveErrorBehavior.Error)
            addNotificationType(NotificationType.Error, objectWithError, message);
         else
         {
            var parent = entity.ParentContainer;
            parent?.RemoveChild(entity);

            if (resolveErrorBehavior == ResolveErrorBehavior.Delete)
               return;

            addNotificationType(NotificationType.Warning, objectWithError, message);
         }
      }

      private void addNotificationType(NotificationType notificationType, IObjectBase builder, string notification)
      {
         //Although the builder is defined in the configuration, we do not know (yet) from which building block it is coming from=>hence nulls
         _result.AddMessage(notificationType, builder, notification);
      }

      private bool containsKeyWords(IEnumerable<string> reference) => _keywords.Any(reference.Contains);

      /// <summary>
      ///    Starts a validation run for the specified object to validate.
      /// </summary>
      /// <param name="objectToValidate">The object to validate.</param>
      /// <param name="buildConfiguration">Build configuration used to create the model</param>
      public ValidationResult Validate(IObjectBase objectToValidate, IBuildConfiguration buildConfiguration)
      {
         try
         {
            _result = new ValidationResult();
            _buildConfiguration = buildConfiguration;
            objectToValidate.AcceptVisitor(this);
            return _result;
         }
         finally
         {
            _buildConfiguration = null;
            _result = null;
         }
      }
   }

   internal class ValidatorForQuantities : ModelValidator, IVisitor<IQuantity>, IVisitor<IParameter>
   {
      public ValidatorForQuantities(IObjectTypeResolver objectTypeResolver, IObjectPathFactory objectPathFactory)
         : base(objectTypeResolver, objectPathFactory)
      {
      }

      public void Visit(IQuantity quantity)
      {
         CheckReferences(quantity);
      }

      public void Visit(IParameter parameter)
      {
         CheckParameter(parameter);
      }
   }

   internal class ValidatorForReactionsAndTransports : ModelValidator,
      IVisitor<IReaction>,
      IVisitor<ITransport>
   {
      public ValidatorForReactionsAndTransports(IObjectTypeResolver objectTypeResolver, IObjectPathFactory objectPathFactory)
         : base(objectTypeResolver, objectPathFactory)
      {
      }

      public void Visit(IReaction reaction)
      {
         CheckReferences(reaction);
      }

      public void Visit(ITransport transport)
      {
         CheckReferences(transport);
      }
   }

   internal class ValidatorForObserversAndEvents : ModelValidator,
      IVisitor<IEvent>,
      IVisitor<IObserver>,
      IVisitor<IEventAssignment>
   {
      public ValidatorForObserversAndEvents(IObjectTypeResolver objectTypeResolver, IObjectPathFactory objectPathFactory)
         : base(objectTypeResolver, objectPathFactory)
      {
      }

      public void Visit(IEvent oneEvent)
      {
         CheckReferences(oneEvent);
      }

      public void Visit(IObserver observer)
      {
         CheckReferences(observer, ResolveErrorBehavior.DeleteAndWarning);
      }

      public void Visit(IEventAssignment eventAssignment)
      {
         CheckReferences(eventAssignment);
         CheckPath(eventAssignment, eventAssignment.ObjectPath, ResolveErrorBehavior.Error);
      }
   }
}