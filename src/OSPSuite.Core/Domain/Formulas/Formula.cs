﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Formulas
{
   /// <summary>
   ///    Interface for formulas used both in building blocks and models.
   ///    <para></para>
   /// </summary>
   public interface IFormula : IObjectBase, IWithDimension
   {
      /// <summary>
      ///    Object path to IFormulaUsable-entities used in current formula
      ///    <para></para>
      /// </summary>
      IReadOnlyList<FormulaUsablePath> ObjectPaths { get; }

      /// <summary>
      ///    Concrete IFormulaUsable-entities used by current formula
      ///    <para></para>
      ///    Is only set if formula is used in a model
      /// </summary>
      IReadOnlyList<IObjectReference> ObjectReferences { get; }

      /// <summary>
      ///    Returns if all referenced in the formula have been found (i.e. the formula holds references to real objects with
      ///    which the formula can be calculated)
      /// </summary>
      bool AreReferencesResolved { get; }

      /// <summary>
      ///    Obtains concrete <see cref="ObjectReferences" /> from the <see cref="ObjectPaths" /> for the model passed Is applied
      ///    on formulas used in model during formula finalization process or
      ///    <para></para>
      ///    after cloning models/simulations
      /// </summary>
      void ResolveObjectPathsFor(IEntity dependentEntity);

      /// <summary>
      ///    Calculates the formula value for a formula used within any building block
      ///    <para></para>
      /// </summary>
      /// <param name="refObject"> Object for which the formula should be calculated </param>
      double Calculate(IUsingFormula refObject);

      /// <summary>
      ///    Tries to calculate the formula value for a formula used within any building block
      ///    <para></para>
      /// </summary>
      /// <param name="refObject"> Object for which the formula should be calculated </param>
      (double value, bool success) TryCalculate(IUsingFormula refObject);

      /// <summary>
      ///    Adds a new path reference to the Formulas.
      /// </summary>
      /// <param name="newPath"> The new reference. </param>
      void AddObjectPath(FormulaUsablePath newPath);

      /// <summary>
      ///    Removes the specified path reference from formula.
      /// </summary>
      /// <param name="pathToRemove"> The reference to remove. </param>
      void RemoveObjectPath(FormulaUsablePath pathToRemove);

      /// <summary>
      ///    Removes all object paths defined in the formula
      /// </summary>
      void ClearObjectPaths();
   }

   public abstract class Formula : ObjectBase, IFormula
   {
      private readonly List<IObjectReference> _objectReferences = new List<IObjectReference>();
      private List<FormulaUsablePath> _objectPaths = new List<FormulaUsablePath>();
      public virtual IDimension Dimension { get; set; } = Constants.Dimension.NO_DIMENSION;

      public virtual IReadOnlyList<IObjectReference> ObjectReferences => _objectReferences;

      public virtual void ResolveObjectPathsFor(IEntity dependentEntity)
      {
         foreach (var objectReference in _objectReferences)
         {
            objectReference.Object.PropertyChanged -= onReferencePropertyChanged;
         }

         _objectReferences.Clear();

         foreach (var reference in ObjectPaths)
         {
            var objectReference = new ObjectReference(reference.Resolve<IFormulaUsable>(dependentEntity), reference.Alias);
            ValidateObjectReference(objectReference, reference, dependentEntity);

            //Object can be null if the object could not be found and the underlying check did not throw
            if (objectReference.Object != null)
            {
               objectReference.Object.PropertyChanged += onReferencePropertyChanged;
               _objectReferences.Add(objectReference);
            }
         }
      }

      protected virtual void ValidateObjectReference(ObjectReference objectReference, FormulaUsablePath reference, IEntity dependentEntity)
      {
         if (objectReference.Object == null)
            throw new UnableToResolvePathException(reference, dependentEntity);
      }

      private void onReferencePropertyChanged(object sender, PropertyChangedEventArgs e)
      {
         if (e.PropertyName.Equals("Value"))
         {
            OnChanged();
         }
      }

      public virtual IReadOnlyList<FormulaUsablePath> ObjectPaths
      {
         get => _objectPaths;
         set
         {
            checkAliases(value.Select(path => path.Alias));
            _objectPaths = value.ToList();
         }
      }

      public virtual bool AreReferencesResolved => (ObjectReferences.Count == ObjectPaths.Count);

      private void checkAliases(IEnumerable<string> aliases)
      {
         //make sure aliases are unique
         var all = aliases.ToList();
         if (all.Distinct().Count() != all.Count)
            throw new OSPSuiteAliasException("Aliases not unique in \n" + all.ToString("\n"));
      }

      /// <summary>
      ///    Adds a new reference to the Formulas.
      /// </summary>
      /// <param name="newPath"> The new reference. </param>
      public virtual void AddObjectPath(FormulaUsablePath newPath)
      {
         _objectPaths.Add(newPath);
      }

      /// <summary>
      ///    Removes the specified reference from formula.
      /// </summary>
      /// <param name="pathToRemove"> The reference to remove. </param>
      public virtual void RemoveObjectPath(FormulaUsablePath pathToRemove)
      {
         _objectPaths.Remove(pathToRemove);
      }

      public virtual void ClearObjectPaths()
      {
         _objectPaths.Clear();
         _objectReferences.Clear();
      }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);

         var srcFormula = source as Formula;
         if (srcFormula == null) return;

         srcFormula.ObjectPaths.Each(path => _objectPaths.Add(path.Clone<FormulaUsablePath>()));
         Dimension = srcFormula.Dimension;
      }

      /// <summary>
      ///    Can be used for all formulas both in model and in building blocks
      /// </summary>
      public virtual double Calculate(IUsingFormula refObject)
      {
         try
         {
            var usedObjects = GetUsedObjectsFrom(refObject);
            return CalculateFor(usedObjects, refObject);
         }
         catch (OSPSuiteException ex)
         {
            var errorMessage = ex.FullMessage();
            if (refObject != null)
               errorMessage = $"Cannot evaluate formula for {refObject.Name}\n{errorMessage}";

            throw new OSPSuiteException(errorMessage);
         }
      }

      /// <summary>
      ///    Can be used for all formulas both in model and in building blocks
      /// </summary>
      public virtual (double value, bool success) TryCalculate(IUsingFormula refObject)
      {
         var usedObjects = GetUsedObjectsFrom(refObject, throwOnMissingRef: false);
         return TryCalculateFor(usedObjects, refObject);
      }

      protected IReadOnlyList<IObjectReference> GetUsedObjectsFrom(IUsingFormula refObject, bool throwOnMissingRef = true)
      {
         if (AreReferencesResolved)
            return ObjectReferences;

         // References are not resolved - get objects from object paths
         var usedObjects = new List<IObjectReference>();

         foreach (var path in ObjectPaths)
         {
            var usingObject = path.Resolve<IFormulaUsable>(refObject);
            if (usingObject != null)
            {
               usedObjects.Add(new ObjectReference(usingObject, path.Alias));
               continue;
            }

            if (throwOnMissingRef)
               throw new OSPSuiteException($"Cannot evaluate formula for '{refObject.Name}' with path '{path}'");
         }

         return usedObjects;
      }

      protected abstract double CalculateFor(IReadOnlyList<IObjectReference> usedObjects, IUsingFormula dependentObject);

      protected virtual (double value, bool success) TryCalculateFor(IReadOnlyList<IObjectReference> usedObjects, IUsingFormula dependentObject)
      {
         //default implementation common for all formula except some specific implementation
         try
         {
            return (CalculateFor(usedObjects, dependentObject), true);
         }
         catch (OSPSuiteException)
         {
            return (double.NaN, false);
         }
      }

      protected IFormulaUsable GetReferencedEntityByAlias(string alias, IUsingFormula refObject) 
         => GetUsedObjectsFrom(refObject).First(x => string.Equals(x.Alias, alias)).Object;

      protected T GetReferencedEntityByAlias<T>(string alias, IUsingFormula refObject) where T : class, IFormulaUsable 
         => GetReferencedEntityByAlias(alias, refObject) as T;
    }

   public abstract class FormulaWithFormulaString : Formula
   {
      private string _formulaString;

      public string FormulaString
      {
         get => _formulaString;
         set
         {
            _formulaString = value;
            OnChanged();
         }
      }

      /// <summary>
      ///    Validates the formula with the given formulaString.
      /// </summary>
      /// <param name="formulaString">
      ///    Formula string to validate for the current <see cref="Formula" /> object
      /// </param>
      /// <exception cref="OSPSuiteException">
      ///    is thrown if the given <paramref name="formulaString" /> given cannot be successfully parsed
      /// </exception>
      public virtual void Validate(string formulaString)
      {
         var formulaParser = ExplicitFormulaParserCreator(UsedVariableNames, new string[0]);
         formulaParser.FormulaString = formulaString ?? string.Empty;
         formulaParser.Parse();
      }

      /// <summary>
      ///    Validates the current formula string. Throws an
      /// </summary>
      /// <exception cref="OSPSuiteException"> is thrown if the current FormulaString cannot be successfully parsed </exception>
      public virtual void Validate()
      {
         Validate(FormulaString);
      }

      public virtual (bool valid, string validationMessage) IsValid()
      {
         return IsValid(FormulaString);
      }

      public virtual (bool valid, string validationMessage) IsValid(string formulaString)
      {
         try
         {
            Validate(formulaString);
            return (true, string.Empty);
         }
         catch (OSPSuiteException e)
         {
            return (false, e.Message);
         }
      }

      protected virtual IEnumerable<string> UsedVariableNames => ObjectPaths.Select(path => path.Alias);

      public static Func<IEnumerable<string>, IEnumerable<string>, IExplicitFormulaParser> ExplicitFormulaParserCreator { get; set; } = (v, p) => new ExplicitFormulaParser(v, p);
   }
}