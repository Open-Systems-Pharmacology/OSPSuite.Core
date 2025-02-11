using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Visitor;
using static OSPSuite.Core.Domain.Constants;
using static OSPSuite.Core.Domain.Constants.Parameters;
using static OSPSuite.Core.Domain.ObjectPath;
using static OSPSuite.Core.Domain.ObjectPathKeywords;

namespace OSPSuite.Core.Domain.Services
{
   public interface IFormulaTask
   {
      /// <summary>
      ///    Checks that all formula having the same origin id are indeed the same formula.
      ///    If not, reset the origin id. Discrepancy can happen when a formula with key words was clone.
      ///    After the cloning operation, the origin id was set but used object path are not the same anymore
      /// </summary>
      void CheckFormulaOriginIn(IModel model);

      /// <summary>
      ///    Returns true if the two objects represents the same formula (same type, save value or object references etc..)
      ///    otherwise false
      /// </summary>
      bool FormulasAreTheSame(IFormula firstFormula, IFormula secondFormula);

      /// <summary>
      ///    Expands all dynamic references defined in <paramref name="model" />
      /// </summary>
      void ExpandDynamicReferencesIn(IModel model);

      /// <summary>
      ///    Expands all dynamic references defined in <paramref name="rootContainer" />
      /// </summary>
      /// <param name="rootContainer">Root container of the spatial structure</param>
      void ExpandDynamicReferencesIn(IContainer rootContainer);

      /// <summary>
      ///    Resolves all dynamic formulas defined in <paramref name="model" />
      /// </summary>
      void ExpandDynamicFormulaIn(IModel model);

      /// <summary>
      ///    Resolves all dynamic formulas defined in <paramref name="rootContainer" />
      /// </summary>
      void ExpandDynamicFormulaIn(IContainer rootContainer);

      /// <summary>
      ///    Adds a reference to the parent container volume in the object path used by the <paramref name="formula" /> and
      ///    return the alias used
      /// </summary>
      string AddParentVolumeReferenceToFormula(IFormula formula);
   }

   public class FormulaTask : IFormulaTask,
      IVisitor<IUsingFormula>,
      IVisitor<IParameter>
   {
      private readonly IObjectPathFactory _objectPathFactory;
      private readonly IObjectBaseFactory _objectBaseFactory;
      private readonly IAliasCreator _aliasCreator;
      private readonly IDimensionFactory _dimensionFactory;
      private readonly IEntityPathResolver _entityPathResolver;
      private readonly ICache<string, IList<ExplicitFormula>> _originIdToFormulaCache = new Cache<string, IList<ExplicitFormula>>();

      public FormulaTask(
         IObjectPathFactory objectPathFactory,
         IObjectBaseFactory objectBaseFactory,
         IAliasCreator aliasCreator,
         IDimensionFactory dimensionFactory,
         IEntityPathResolver entityPathResolver)
      {
         _objectPathFactory = objectPathFactory;
         _objectBaseFactory = objectBaseFactory;
         _aliasCreator = aliasCreator;
         _dimensionFactory = dimensionFactory;
         _entityPathResolver = entityPathResolver;
      }

      public void CheckFormulaOriginIn(IModel model)
      {
         try
         {
            model.AcceptVisitor(this);
            foreach (var formulasWithSameOrigin in _originIdToFormulaCache)
            {
               resetOriginIdIfFormulasAreNotTheSame(formulasWithSameOrigin);
            }
         }
         finally
         {
            _originIdToFormulaCache.Clear();
         }
      }

      public bool FormulasAreTheSame(IFormula firstFormula, IFormula secondFormula)
      {
         if (firstFormula == null && secondFormula == null)
            return true;

         if (firstFormula == null || secondFormula == null)
            return false;

         var firstType = firstFormula.GetType();
         var secondType = secondFormula.GetType();
         if (firstType != secondType)
            return false;

         //nothing more to check for distributed formula or black box formula
         if (firstFormula.IsDistributed() || firstFormula.IsBlackBox() || firstFormula.IsDynamic())
            return true;

         if (firstFormula.IsConstant())
         {
            var firstConstFormula = firstFormula.DowncastTo<ConstantFormula>();
            var secondConstFormula = secondFormula.DowncastTo<ConstantFormula>();
            return firstConstFormula.Value == secondConstFormula.Value;
         }

         if (firstFormula.IsExplicit())
         {
            var firstExplicit = firstFormula.DowncastTo<ExplicitFormula>();
            var secondExplicit = secondFormula.DowncastTo<ExplicitFormula>();

            if (!string.Equals(firstExplicit.FormulaString, secondExplicit.FormulaString))
               return false;

            //check that formula have the same references using the same alias
            var firstObjectPathCache = new Cache<string, FormulaUsablePath>(x => x.Alias);
            firstObjectPathCache.AddRange(firstExplicit.ObjectPaths);
            var secondObjectPathCache = new Cache<string, FormulaUsablePath>(x => x.Alias);
            secondObjectPathCache.AddRange(secondExplicit.ObjectPaths);


            if (firstObjectPathCache.Count() != secondObjectPathCache.Count())
               return false;

            foreach (var keyValue in firstObjectPathCache.KeyValues)
            {
               if (!secondObjectPathCache.Contains(keyValue.Key))
                  return false;
               var path = secondObjectPathCache[keyValue.Key];
               if (!path.Equals(keyValue.Value))
                  return false;
            }
         }

         return true;
      }

      public void ExpandDynamicReferencesIn(IModel model) => ExpandDynamicReferencesIn(model.Root);

      public void ExpandDynamicReferencesIn(IContainer rootContainer)
      {
         ExpandNeighborhoodReferencesIn(rootContainer);
         ExpandLumenSegmentReferencesIn(rootContainer);
      }

      private bool referencesNeighborhood(ObjectPath path) => path.Contains(NBH);

      /// <summary>
      ///    Ensures that all object paths referencing neighborhoods between containers are expanded
      /// </summary>
      /// <remarks>Internal for testing</remarks>
      internal void ExpandNeighborhoodReferencesIn(IContainer rootContainer)
      {
         var neighborhoodsContainer = rootContainer.Container(NEIGHBORHOODS);
         void updatePath(EntityFormulaPath entityFormulaPath) => updateNeighborhoodReferencingPath(entityFormulaPath, neighborhoodsContainer);
         rootContainer.GetPathsReferencing(referencesNeighborhood).Each(updatePath);
      }

      private void updateNeighborhoodReferencingPath(EntityFormulaPath entityFormulaPath, IContainer neighborhoods)
      {
         var (entity, path, formula) = entityFormulaPath;
         var pathAsList = path.ToList();
         var firstIndex = pathAsList.FindIndex(x => x == NBH);
         var lastIndex = pathAsList.FindLastIndex(x => x == NBH);

         //Only one occurrence of the marker, this is not a valid path. We do not change anything as it won't be resolved later on
         if (firstIndex == lastIndex)
            return;

         //We retrieve the path to first container, and second container 
         var pathToFirstContainer = pathAsList.Take(firstIndex).ToList();
         //+1 and -1 in order to skip the NBH tags
         var pathToSecondContainer = pathAsList.Skip(firstIndex + 1).Take(lastIndex - firstIndex - 1).ToList();
         //This will need to be saved and added back to the path once we have figured out the actual neighborhood path
         var restOfPath = pathAsList.Skip(lastIndex + 1).ToList();

         //we use resolve to that an exception is thrown
         var container1 = getContainerOrThrow(pathToFirstContainer, entity);
         var container2 = getContainerOrThrow(pathToSecondContainer, entity);

         var allNeighborhoods = neighborhoods?.GetAllChildren<Neighborhood>() ?? Array.Empty<Neighborhood>();
         var allNeighborhoodsConnectedToContainer1 = container1.GetNeighborhoods(allNeighborhoods);
         var neighborhoodsBetweenContainer1AndContainer2 = container2.GetNeighborhoods(allNeighborhoodsConnectedToContainer1);
         if (neighborhoodsBetweenContainer1AndContainer2.Count == 0)
            throw new OSPSuiteException(Error.CouldNotFindNeighborhoodBetween(container1.EntityPath(), container2.EntityPath(), formula?.Name, entity.EntityPath()));

         //recreate the path for this neighborhood and add the rest of the path. Validation of this path will be done later
         var neighborhoodPath = _objectPathFactory.CreateAbsoluteObjectPath(neighborhoodsBetweenContainer1AndContainer2[0]);
         neighborhoodPath.AddRange(restOfPath);
         path.ReplaceWith(neighborhoodPath);
      }

      private static bool referencesLumenSegment(ObjectPath path) => path.Contains(LUMEN_SEGMENT);

      private static bool referencesLumenNavigation(ObjectPath path) => path.Contains(LUMEN_NEXT_SEGMENT) || path.Contains(LUMEN_PREVIOUS_SEGMENT);

      /// <summary>
      ///    Ensures that all object paths referencing lumen segments are expanded
      /// </summary>
      /// <remarks>Internal for testing</remarks>
      internal void ExpandLumenSegmentReferencesIn(IContainer rootContainer)
      {
         //Lumen Segments
         rootContainer.GetPathsReferencing(referencesLumenSegment).Each(x => updateLumenSegmentReferencingPath(x, rootContainer));

         //Previous or next lumen segment. We create a list so that we can find by index
         var allLumenSegmentsList = Compartment.AllLumenSegments.ToList();
         void updatePath(EntityFormulaPath entityFormulaPath) => updateLumenNavigationSegmentReferencingPath(entityFormulaPath, allLumenSegmentsList);
         rootContainer.GetPathsReferencing(referencesLumenNavigation).Each(updatePath);
      }

      private void updateLumenSegmentReferencingPath(EntityFormulaPath entityFormulaPath, IContainer rootContainer)
      {
         var (entity, path) = entityFormulaPath;
         var pathAsList = path.ToList();
         var firstIndex = pathAsList.IndexOf(LUMEN_SEGMENT);
         var lastIndex = pathAsList.LastIndexOf(LUMEN_SEGMENT);

         if (firstIndex == 0)
            throw new OSPSuiteException(Error.KeywordCannotBeInFirstPosition(LUMEN_SEGMENT, path.ToPathString()));

         if (firstIndex != lastIndex)
            throw new OSPSuiteException(Error.KeywordCanOnlyBeUsedOnce(LUMEN_SEGMENT, path.ToPathString()));

         //let's get the path BEFORE the LUMEN_SEGMENT and update the formula path accordingly
         var pathToContainer = pathAsList.Take(firstIndex).ToList();
         var container = getContainerOrThrow(pathToContainer, entity);
         //Point to our absolute ORGANISM|LUMEN|container path
         var lumenSegmentPath = new List<string> {rootContainer.Name, ORGANISM, Organ.LUMEN, container.Name};
         //we add the rest of the path that was provided after the keyword
         lumenSegmentPath.AddRange(pathAsList.Skip(lastIndex + 1));
         path.ReplaceWith(lumenSegmentPath);
      }

      private void updateLumenNavigationSegmentReferencingPath(EntityFormulaPath entityFormulaPath, List<string> allLumenSegments)
      {
         var (entity, path) = entityFormulaPath;
         var pathAsList = path.ToList();
         var indexNext = pathAsList.IndexOf(LUMEN_NEXT_SEGMENT);
         var indexPrevious = pathAsList.IndexOf(LUMEN_PREVIOUS_SEGMENT);

         if (indexNext != -1 && indexPrevious != -1)
            throw new OSPSuiteException(Error.LumenNavigationKeywordLCanOnlyBeUsedOnce(path.ToPathString()));

         var indexKeyword = indexNext != -1 ? indexNext : indexPrevious;
         //to know which direction we want to go in the segments array
         var step = indexNext != -1 ? 1 : -1;

         //We retrieve the path to the lumen segment of interest
         var pathToCurrentLumenSegment = pathAsList.Take(indexKeyword).ToList();

         //This will need to be saved and added back to the path once we have figured out the actual path
         var restOfPath = pathAsList.Skip(indexKeyword + 1).ToList();

         //we use resolve to that an exception is thrown
         var currentLumenSegment = getContainerOrThrow(pathToCurrentLumenSegment, entity);

         //Do we have an actual lumen segment?
         var currentSegmentIndex = allLumenSegments.IndexOf(currentLumenSegment.Name);

         //not Lumen segment?
         var lumen = currentLumenSegment.ParentContainer;
         if (currentSegmentIndex == -1 || !lumen.IsNamed(Organ.LUMEN))
            throw new OSPSuiteException(Error.ContainerIsNotLumenSegment(currentLumenSegment.EntityPath()));

         //are we first segment and going backwards or last segment and going forward?
         var navigationIndex = currentSegmentIndex + step;
         if (navigationIndex < 0 || navigationIndex >= allLumenSegments.Count)
            throw new OSPSuiteException(Error.CannotNavigateBeyondLumenSegment(pathAsList[indexKeyword], currentLumenSegment.EntityPath()));

         //we can now reconstruct the path to the next or previous lumen segment
         var targetSegmentPath = _objectPathFactory.CreateAbsoluteObjectPath(lumen).AndAdd(allLumenSegments[navigationIndex]);
         targetSegmentPath.AddRange(restOfPath);
         path.ReplaceWith(targetSegmentPath);
      }

      private IContainer getContainerOrThrow(IReadOnlyList<string> path, IEntity entity)
      {
         //we use resolve to that an exception is thrown
         var container = new ObjectPath(path).Resolve<IContainer>(entity);
         if (container == null)
            throw new OSPSuiteException(Error.CouldNotFindContainerWithPath(path.ToPathString(), _entityPathResolver.FullPathFor(entity)));

         return container;
      }

      public void ExpandDynamicFormulaIn(IModel model) => ExpandDynamicFormulaIn(model.Root);

      public void ExpandDynamicFormulaIn(IContainer rootContainer)
      {
         var allFormulaUsable = rootContainer.GetAllChildren<IFormulaUsable>();
         var allEntityUsingDynamicFormula = rootContainer.GetAllChildren<IUsingFormula>(x => x.Formula.IsDynamic());

         allEntityUsingDynamicFormula.Each(entityUsingFormula =>
         {
            var dynamicFormula = entityUsingFormula.Formula.DowncastTo<DynamicFormula>();
            // Check if circular reference will be created.
            if (dynamicFormula.Criteria.IsSatisfiedBy(entityUsingFormula))
               throw new CircularReferenceInSumFormulaException(dynamicFormula.Name, entityUsingFormula.Name);

            dynamicFormula.Criteria = updateDynamicFormulaCriteria(dynamicFormula, entityUsingFormula);

            //this needs to be done after updating the criteria as dynamic tags might be added to the criteria and would 
            //render the descriptor map list invalid
            var allFormulaUsableDescriptor = allFormulaUsable.ToEntityDescriptorMapList();
            entityUsingFormula.Formula = dynamicFormula.ExpandUsing(allFormulaUsableDescriptor, _objectPathFactory, _objectBaseFactory);
         });
      }

      private DescriptorCriteria updateDynamicFormulaCriteria(DynamicFormula formula, IUsingFormula usingFormula)
      {
         var criteria = formula.Criteria;
         var parent = usingFormula.ParentContainer;
         if (parent == null)
            return criteria;

         var modifiedCriteria = modifyInParentFormulaCriteria(criteria, parent);
         modifiedCriteria = modifyInChildrenFormulaCriteria(modifiedCriteria, parent);

         return modifiedCriteria;
      }

      private DescriptorCriteria modifyInParentFormulaCriteria(DescriptorCriteria criteria, IContainer parent) => modifyDynamicCriteria<InParentCondition>(criteria, parent).criteria;

      private DescriptorCriteria modifyInChildrenFormulaCriteria(DescriptorCriteria criteria, IContainer parent)
      {
         var (modifyCriteria, modified) = modifyDynamicCriteria<InChildrenCondition>(criteria, parent);
         if (!modified)
            return criteria;

         //in case of IN CHILDREN, we need to exclude the direct children from the parent from the search.
         //The only way to do this is to add a condition to exclude the children explicitly. We create a random string as tag
         //to avoid collision and set it in all DIRECT only children  so that they will be excluded 

         var uniqueTag = ShortGuid.NewGuid();
         parent.GetChildren<IFormulaUsable>().Each(x => x.AddTag(uniqueTag));
         modifyCriteria.Add(new NotMatchTagCondition(uniqueTag));

         return modifyCriteria;
      }

      private (DescriptorCriteria criteria, bool modified) modifyDynamicCriteria<T>(DescriptorCriteria criteria, IContainer parent) where T : TagCondition
      {
         var allDynamicTags = criteria.Where(x => x.IsAnImplementationOf<T>()).ToList();

         if (!allDynamicTags.Any())
            return (criteria, false);

         //because we need to restrict operations by adding criteria automatically, only AND makes sense
         if (criteria.Operator != CriteriaOperator.And)
            throw new OSPSuiteException(Error.InParentTagCanOnlyBeUsedWithAndOperator);

         //we clone the criteria and remove all instances of the dynamic condition. Then we add the criteria to the parent specifically
         var modifiedCriteria = criteria.Clone();
         allDynamicTags.Each(x => modifiedCriteria.RemoveByTag<T>(x.Tag));

         //add to the formula the link to parent. We use the consolidated path here so that we do not deal with the root container as criteria
         var parentPath = _entityPathResolver.PathFor(parent).ToPathArray();
         parentPath.Each(x => modifiedCriteria.Add(new InContainerCondition(x)));

         return (modifiedCriteria, true);
      }

      public string AddParentVolumeReferenceToFormula(IFormula formula)
      {
         var volumeAlias = _aliasCreator.CreateAliasFrom(VOLUME_ALIAS, formula.ObjectPaths.Select(p => p.Alias));

         //possible reference
         var volumeReferencePath = _objectPathFactory.CreateFormulaUsablePathFrom(PARENT_CONTAINER, VOLUME)
            .WithAlias(volumeAlias)
            .WithDimension(_dimensionFactory.Dimension(Constants.Dimension.VOLUME));

         //do we have one already?
         var volumeReference = formula.ObjectPaths.FirstOrDefault(x => Equals(x.PathAsString, volumeReferencePath.PathAsString));

         //was not defined yet
         if (volumeReference == null)
            formula.AddObjectPath(volumeReferencePath);
         else
            volumeAlias = volumeReference.Alias;

         //return the used alias
         return volumeAlias;
      }

      private void resetOriginIdIfFormulasAreNotTheSame(IList<ExplicitFormula> formulasWithSameOrigin)
      {
         if (formulasWithSameOrigin.Count <= 1) return;
         var firstFormula = formulasWithSameOrigin[0];

         var firstObjectPaths = firstFormula.ObjectPaths.ToList();
         if (firstObjectPaths.Count == 0) return;

         //starts with 1 because we compare each formula with the first one
         for (int i = 1; i < formulasWithSameOrigin.Count; i++)
         {
            var currentFormula = formulasWithSameOrigin[i];
            var currentObjectPaths = currentFormula.ObjectPaths.ToList();
            if (!firstObjectPaths.ListEquals(currentObjectPaths))
               currentFormula.OriginId = string.Empty;
         }
      }

      public void Visit(IUsingFormula usingFormula)
      {
         addFormulaToCache(usingFormula?.Formula);
      }

      private void addFormulaToCache(IFormula formula)
      {
         if (!(formula is ExplicitFormula explicitFormula))
            return;

         if (string.IsNullOrEmpty(explicitFormula.OriginId))
            return;

         listFor(explicitFormula.OriginId).Add(explicitFormula);
      }

      private IList<ExplicitFormula> listFor(string originId)
      {
         if (!_originIdToFormulaCache.Contains(originId))
            _originIdToFormulaCache.Add(originId, new List<ExplicitFormula>());

         return _originIdToFormulaCache[originId];
      }

      public void Visit(IParameter parameter)
      {
         Visit(parameter as IUsingFormula);
         addFormulaToCache(parameter.RHSFormula);
      }
   }
}