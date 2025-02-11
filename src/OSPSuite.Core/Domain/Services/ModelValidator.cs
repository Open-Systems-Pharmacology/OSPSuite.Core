using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility.Extensions;
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

   internal interface IModelValidator
   {
      ValidationResult Validate(ModelConfiguration modelConfiguration);
   }

   /// <summary>
   ///    Base class for Validation tasks in ModelCore
   /// </summary>
   internal abstract class ModelValidator : IModelValidator, IVisitor
   {
      private readonly IObjectTypeResolver _objectTypeResolver;
      private readonly IObjectPathFactory _objectPathFactory;
      private readonly IEnumerable<string> _keywords;
      private SimulationBuilder _simulationBuilder;
      protected ValidationResult _result;

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
         var builder = _simulationBuilder?.BuilderFor(entity);
         var objectWithError = builder ?? entity;
         void checkPathInEntity(ObjectPath objectPath) => CheckPath(entity, objectPath, resolveErrorBehavior);

         switch (formulaToCheck)
         {
            // Dynamic formula may contain object path that will be resolved per instance. It cannot be checked here
            case DynamicFormula _:
               return;
            case BlackBoxFormula _:
               addNotificationType(NotificationType.Error, objectWithError, Validation.FormulaIsBlackBoxIn(entity.Name, entityAbsolutePath));
               return;

            //Table formula with offset may use an x argument that is not defined. A dynamic check will be done in the formula itself
            //so in this case, we only check the path to the table that should be defined
            case TableFormulaWithXArgument formulaWithXArgument:
               checkPathInEntity(formulaWithXArgument.FormulaUsablePathBy(formulaWithXArgument.TableObjectAlias));
               return;
         }

         //in all other cases, we check the object path used in the formula
         formulaToCheck.ObjectPaths.Each(checkPathInEntity);
      }

      protected void CheckPath(IUsingFormula entity, ObjectPath objectPathToCheck, ResolveErrorBehavior resolveErrorBehavior)
      {
         var builder = _simulationBuilder?.BuilderFor(entity);
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

      private void addNotificationType(NotificationType notificationType, IObjectBase invalidObject, string notification)
      {
         var builder = invalidObject as IBuilder;
         _result.AddMessage(notificationType, invalidObject, notification, builder?.BuildingBlock);
      }

      private bool containsKeyWords(IEnumerable<string> reference) => _keywords.Any(reference.Contains);

      /// <summary>
      ///    Starts a validation run for the specified object to validate.
      /// </summary>
      public ValidationResult Validate(ModelConfiguration modelConfiguration)
      {
         try
         {
            var (model, simulationBuilder) = modelConfiguration;
            _result = new ValidationResult();
            _simulationBuilder = simulationBuilder;
            model.AcceptVisitor(this);
            return _result;
         }
         finally
         {
            _simulationBuilder = null;
            _result = null;
         }
      }
   }

   internal class ValidatorForForFormula : ModelValidator
   {
      public ValidatorForForFormula(IObjectTypeResolver objectTypeResolver, IObjectPathFactory objectPathFactory)
         : base(objectTypeResolver, objectPathFactory)
      {
      }

      public bool IsFormulaValid(IUsingFormula usingFormulaToCheck)
      {
         try
         {
            _result = new ValidationResult();
            CheckReferences(usingFormulaToCheck);
            return _result.ValidationState == ValidationState.Valid;
         }
         finally
         {
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
      IVisitor<Reaction>,
      IVisitor<Transport>
   {
      public ValidatorForReactionsAndTransports(IObjectTypeResolver objectTypeResolver, IObjectPathFactory objectPathFactory)
         : base(objectTypeResolver, objectPathFactory)
      {
      }

      public void Visit(Reaction reaction)
      {
         CheckReferences(reaction);
      }

      public void Visit(Transport transport)
      {
         CheckReferences(transport);
      }
   }

   internal class ValidatorForObserversAndEvents : ModelValidator,
      IVisitor<Event>,
      IVisitor<Observer>,
      IVisitor<EventAssignment>
   {
      public ValidatorForObserversAndEvents(IObjectTypeResolver objectTypeResolver, IObjectPathFactory objectPathFactory)
         : base(objectTypeResolver, objectPathFactory)
      {
      }

      public void Visit(Event oneEvent)
      {
         CheckReferences(oneEvent);
      }

      public void Visit(Observer observer)
      {
         CheckReferences(observer, ResolveErrorBehavior.DeleteAndWarning);
      }

      public void Visit(EventAssignment eventAssignment)
      {
         CheckReferences(eventAssignment);
         CheckPath(eventAssignment, eventAssignment.ObjectPath, ResolveErrorBehavior.Error);
      }
   }

   internal class ModelNameValidator : IModelValidator
   {
      public ValidationResult Validate(ModelConfiguration modelConfiguration)
      {
         var (model, _) = modelConfiguration;
         var result = new ValidationResult();

         var allTopContainerNames = model.Root.GetChildren<IContainer>().AllNames();
         if (!allTopContainerNames.Contains(model.Name))
            return result;

         result.AddMessage(NotificationType.Error, model, Validation.ModelNameCannotBeNamedLikeATopContainer(allTopContainerNames));
         return result;
      }
   }
}