using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Services
{
   public interface ICircularReferenceChecker
   {
      /// <summary>
      ///    Returns <c>true</c> if the usage of <paramref name="path" /> in the formula of <paramref name="referenceObject" />
      ///    would result in circular references otherwise <c>false</c>
      /// </summary>
      bool HasCircularReference(ObjectPath path, IEntity referenceObject);
   }

   internal interface IModelCircularReferenceChecker
   {
      /// <summary>
      ///    Check the given <paramref name="modelConfiguration" /> for circular references and returns any problem that may have
      ///    been found
      ///    during check
      /// </summary>
      ValidationResult CheckCircularReferencesIn(ModelConfiguration modelConfiguration);
   }

   internal class CircularReferenceChecker : ICircularReferenceChecker, IModelCircularReferenceChecker
   {
      private readonly IObjectPathFactory _objectPathFactory;
      private readonly IObjectTypeResolver _objectTypeResolver;

      public CircularReferenceChecker(IObjectPathFactory objectPathFactory, IObjectTypeResolver objectTypeResolver)
      {
         _objectPathFactory = objectPathFactory;
         _objectTypeResolver = objectTypeResolver;
      }

      public bool HasCircularReference(ObjectPath path, IEntity referenceObject)
      {
         var referencedObject = path.TryResolve<IUsingFormula>(referenceObject);
         if (referencedObject == null)
            return false;

         if (referencedObject == referenceObject)
            return true;

         var entityReferenceCache = newEntityReferenceCache();
         buildEntityReferenceCache(referencedObject, entityReferenceCache);
         return entityReferenceCache[referencedObject].Contains(referenceObject);
      }

      public ValidationResult CheckCircularReferencesIn(ModelConfiguration modelConfiguration)
      {
         var validationResult = new ValidationResult();
         var (model, simulationBuilder) = modelConfiguration;

         checkFormulas(model, simulationBuilder, validationResult);
         checkEvents(model, simulationBuilder, validationResult);

         return validationResult;
      }

      private static Cache<IEntity, List<IEntity>> newEntityReferenceCache() => new Cache<IEntity, List<IEntity>>(x => new List<IEntity>());

      private void checkEvents(IModel model, SimulationBuilder simulationBuilder, ValidationResult validationResult)
      {
         checkReferencesInEvents(model, simulationBuilder, validationResult, newEntityReferenceCache());
      }

      private void checkFormulas(IModel model, SimulationBuilder simulationBuilder, ValidationResult validationResult)
      {
         checkReferencesInAllFormulas(model, simulationBuilder, validationResult, newEntityReferenceCache());
      }

      private void checkReferencesInAllFormulas(IModel model, SimulationBuilder simulationBuilder, ValidationResult validationResult, Cache<IEntity, List<IEntity>> entityReferenceCache)
      {
         var allUsingFormulas = model.Root.GetAllChildren<IUsingFormula>();
         allUsingFormulas.Each(x => buildEntityReferenceCache(x, entityReferenceCache));
         allUsingFormulas.Each(x => checkCircularReferencesIn(x, simulationBuilder, validationResult, (entityType, entityAbsolutePath, allReferencesName) => Validation.CircularReferenceFoundInFormula(x.Name, entityType, entityAbsolutePath, allReferencesName), entityReferenceCache));
      }

      private void checkReferencesInEvents(IModel model, SimulationBuilder simulationBuilder, ValidationResult validationResult, Cache<IEntity, List<IEntity>> entityReferenceCache)
      {
         model.Root.GetAllChildren<Event>().Each(@event => checkCircularReferencesInEventAssignments(simulationBuilder, validationResult, @event, entityReferenceCache));
      }

      private void checkCircularReferencesInEventAssignments(SimulationBuilder simulationBuilder, ValidationResult validationResult, Event @event, Cache<IEntity, List<IEntity>> entityReferenceCache)
      {
         var allEventAssignments = @event.GetAllChildren<EventAssignment>().Where(x => !x.UseAsValue).ToList();
         allEventAssignments.Each(assignment => buildAssignmentEntityCache(assignment, assignment.ObjectPath.TryResolve<IUsingFormula>(assignment), entityReferenceCache));
         allEventAssignments.Each(x => checkCircularReferencesInEventAssignment(simulationBuilder, validationResult, @event, x, entityReferenceCache));
      }

      private void checkCircularReferencesInEventAssignment(SimulationBuilder simulationBuilder, ValidationResult validationResult, Event @event, EventAssignment x, Cache<IEntity, List<IEntity>> entityReferenceCache)
      {
         var changedEntity = x.ObjectPath.TryResolve<IUsingFormula>(x);
         checkCircularReferencesIn(changedEntity, simulationBuilder, validationResult, (entityType, entityAbsolutePath, allReferencesName) => Validation.CircularReferenceFoundInEventAssignment(@event.Name, changedEntity.Name, entityType, entityAbsolutePath, allReferencesName), entityReferenceCache);
      }

      private void buildAssignmentEntityCache(EventAssignment assignment, IEntity changedEntity, Cache<IEntity, List<IEntity>> entityReferenceCache)
      {
         if (changedEntity == null)
            return;

         var references = entityReferenceCache.Contains(changedEntity) ? entityReferenceCache[changedEntity] : new List<IEntity>();
         entityReferenceCache[changedEntity] = references;

         foreach (var objectPath in assignment.Formula.ObjectPaths)
         {
            // formula references will be resolved before assignment, so after assignment, the path will not be  used
            // that means the referenced object will only resolve relevant to the assignment
            var referencedObject = objectPath.TryResolve<IUsingFormula>(assignment);

            if (referencedObject == null)
               continue;

            references.Add(referencedObject);
            buildEntityReferenceCache(referencedObject, entityReferenceCache);
            entityReferenceCache[changedEntity].AddRange(entityReferenceCache[referencedObject]);
         }
      }

      private void buildEntityReferenceCache(IUsingFormula usingFormula, Cache<IEntity, List<IEntity>> entityReferenceCache)
      {
         if (entityReferenceCache.Contains(usingFormula))
            return;

         var references = new List<IEntity>();
         entityReferenceCache[usingFormula] = references;

         if (usingFormula.Formula == null)
            return;

         foreach (var objectPath in usingFormula.Formula.ObjectPaths)
         {
            var referencedObject = objectPath.TryResolve<IUsingFormula>(usingFormula);
            if (referencedObject == null)
               continue;

            references.Add(referencedObject);
            buildEntityReferenceCache(referencedObject, entityReferenceCache);
            entityReferenceCache[usingFormula].AddRange(entityReferenceCache[referencedObject]);
         }
      }

      private void checkCircularReferencesIn(IUsingFormula usingFormula, SimulationBuilder simulationBuilder, ValidationResult validationResult, Func<string, string, IReadOnlyList<string>, string> circularReferenceFoundIn, Cache<IEntity, List<IEntity>> entityReferenceCache)
      {
         var references = entityReferenceCache[usingFormula];
         if (!references.Contains(usingFormula))
            return;

         var entityAbsolutePath = _objectPathFactory.CreateAbsoluteObjectPath(usingFormula).ToPathString();
         var builder = simulationBuilder.BuilderFor(usingFormula);
         var objectWithError = builder ?? usingFormula;
         var entityType = _objectTypeResolver.TypeFor(usingFormula);
         var allReferencesName = references.Distinct().AllNames();
         validationResult.AddMessage(NotificationType.Error, objectWithError, circularReferenceFoundIn(entityType, entityAbsolutePath, allReferencesName));
      }
   }
}