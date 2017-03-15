using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Extensions;

namespace OSPSuite.Core.Services
{
   public interface ICircularReferenceChecker
   {
      /// <summary>
      ///    Returns <c>true</c> if the usage of <paramref name="path" /> in the formula of <paramref name="referenceObject" />
      ///    would result in circular references otherwise <c>false</c>
      /// </summary>
      bool HasCircularReference(IObjectPath path, IEntity referenceObject);

      /// <summary>
      ///    Check the given <paramref name="model" /> for circular references and returns any problem that may have been found
      ///    during check
      /// </summary>
      ValidationResult CheckCircularReferencesIn(IModel model, IBuildConfiguration buildConfiguration);
   }

   internal class CircularReferenceChecker : ICircularReferenceChecker
   {
      private readonly IObjectPathFactory _objectPathFactory;
      private readonly IObjectTypeResolver _objectTypeResolver;
      private readonly Cache<IEntity, List<IEntity>> _entityReferenceCache;

      public CircularReferenceChecker(IObjectPathFactory objectPathFactory, IObjectTypeResolver objectTypeResolver)
      {
         _objectPathFactory = objectPathFactory;
         _objectTypeResolver = objectTypeResolver;
         _entityReferenceCache = new Cache<IEntity, List<IEntity>>(x => new List<IEntity>());
      }

      public bool HasCircularReference(IObjectPath path, IEntity referenceObject)
      {
         try
         {
            var referencedObject = path.TryResolve<IUsingFormula>(referenceObject);
            if (referencedObject == null)
               return false;

            if (referencedObject == referenceObject)
               return true;

            buildEntityReferenceCache(referencedObject);
            return _entityReferenceCache[referencedObject].Contains(referenceObject);
         }
         finally
         {
            _entityReferenceCache.Clear();
         }
      }

      public ValidationResult CheckCircularReferencesIn(IModel model, IBuildConfiguration buildConfiguration)
      {
         var validationResult = new ValidationResult();

         try
         {
            var allUsingFormulas = model.Root.GetAllChildren<IUsingFormula>();
            allUsingFormulas.Each(buildEntityReferenceCache);
            allUsingFormulas.Each(x => checkCircularReferencesIn(x, buildConfiguration, validationResult));
            return validationResult;
         }
         finally
         {
            _entityReferenceCache.Clear();
         }
      }

      private void buildEntityReferenceCache(IUsingFormula usingFormula)
      {
         if (_entityReferenceCache.Contains(usingFormula))
            return;

         var references = new List<IEntity>();
         _entityReferenceCache[usingFormula] = references;

         if (usingFormula.Formula == null)
            return;

         foreach (var objectPath in usingFormula.Formula.ObjectPaths)
         {
            var referencedObject = objectPath.TryResolve<IUsingFormula>(usingFormula);
            if (referencedObject == null)
               continue;

            references.Add(referencedObject);
            buildEntityReferenceCache(referencedObject);
            _entityReferenceCache[usingFormula].AddRange(_entityReferenceCache[referencedObject]);
         }
      }

      private void checkCircularReferencesIn(IUsingFormula usingFormula, IBuildConfiguration buildConfiguration, ValidationResult validationResult)
      {
         var references = _entityReferenceCache[usingFormula];
         if (!references.Contains(usingFormula))
            return;

         var entityAbsolutePath = _objectPathFactory.CreateAbsoluteObjectPath(usingFormula).ToPathString();
         var builder = buildConfiguration.BuilderFor(usingFormula);
         var objectWithError = builder ?? usingFormula;
         var entityType = _objectTypeResolver.TypeFor(usingFormula);
         var allReferencesName = references.Distinct().AllNames();
         validationResult.AddMessage(NotificationType.Error, objectWithError, Validation.CircularReferenceFoundInFormula(usingFormula.Name, entityType, entityAbsolutePath, allReferencesName));
      }
   }
}