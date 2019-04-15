using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Visitor;

namespace OSPSuite.Core.Domain.Services
{
   public interface IFormulaTask
   {
      /// <summary>
      ///    Checks that all formula having the same origin id are indeed the same formula.
      ///    If not, reset the origin id. Discrepancy can happen when a formula witk key words was clone.
      ///    After the cloning operation, the orign id was set but used object path are not the same anymore
      /// </summary>
      void CheckFormulaOriginIn(IModel model);

      /// <summary>
      ///    Returns true if the two objects represents the same formula (same type, save value or object references etc..)
      ///    otherwise false
      /// </summary>
      bool FormulasAreTheSame(IFormula firstFormula, IFormula secondFormula);

      /// <summary>
      ///    Resolves all dynamic formulas defined in <paramref name="model" />
      /// </summary>
      void ExpandDynamicFormulaIn(IModel model);

      /// <summary>
      ///    Resolves all dynamic formulas defined in <paramref name="container" />
      /// </summary>
      void ExpandDynamicFormulaIn(IContainer container);

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
      private readonly ICache<string, IList<ExplicitFormula>> _originIdToFormulaCache = new Cache<string, IList<ExplicitFormula>>();

      public FormulaTask(
         IObjectPathFactory objectPathFactory, 
         IObjectBaseFactory objectBaseFactory, 
         IAliasCreator aliasCreator, 
         IDimensionFactory dimensionFactory)
      {
         _objectPathFactory = objectPathFactory;
         _objectBaseFactory = objectBaseFactory;
         _aliasCreator = aliasCreator;
         _dimensionFactory = dimensionFactory;
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
            var firstObjectPathCache = new Cache<string, IFormulaUsablePath>(x => x.Alias);
            firstObjectPathCache.AddRange(firstExplicit.ObjectPaths);
            var secondObjectPathCache = new Cache<string, IFormulaUsablePath>(x => x.Alias);
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

      public void ExpandDynamicFormulaIn(IModel model)
      {
         ExpandDynamicFormulaIn(model.Root);
      }

      public void ExpandDynamicFormulaIn(IContainer container)
      {
         var allFormulaUsable = container.GetAllChildren<IFormulaUsable>().ToEntityDescriptorMapList();
         var allEntityUsingDynamicFormula = container.GetAllChildren<IUsingFormula>(x => x.Formula.IsDynamic());

         foreach (var entityUsingFormula in allEntityUsingDynamicFormula)
         {
            var dynamicFormula = entityUsingFormula.Formula.DowncastTo<DynamicFormula>();
            // Check if circular reference will be created.
            if (dynamicFormula.Criteria.IsSatisfiedBy(entityUsingFormula))
               throw new CircularReferenceInSumFormulaException(dynamicFormula.Name, entityUsingFormula.Name);

            entityUsingFormula.Formula = dynamicFormula.ExpandUsing(allFormulaUsable, _objectPathFactory, _objectBaseFactory);
         }
      }

      public string AddParentVolumeReferenceToFormula(IFormula formula)
      {
         string volumeAlias = _aliasCreator.CreateAliasFrom(Constants.VOLUME_ALIAS, formula.ObjectPaths.Select(p => p.Alias));

         //possible reference
         var volumeReferencePath = _objectPathFactory.CreateFormulaUsablePathFrom(ObjectPath.PARENT_CONTAINER, Constants.Parameters.VOLUME)
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
         if (usingFormula == null) return;
         addFormulaToCache(usingFormula.Formula);
      }

      private void addFormulaToCache(IFormula formula)
      {
         var explicitFormula = formula as ExplicitFormula;
         if (explicitFormula == null) return;

         if (string.IsNullOrEmpty(explicitFormula.OriginId)) return;

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