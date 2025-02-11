using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Exceptions;

namespace OSPSuite.Core.Domain.Formulas
{
   public class TableFormulaWithXArgument : Formula
   {
      public string TableObjectAlias { get; set; }
      public string XArgumentAlias { get; set; }

      /// <summary>
      ///    Adds path to the object with table formula
      /// </summary>
      public void AddTableObjectPath(FormulaUsablePath tableObjectPath)
      {
         TableObjectAlias = tableObjectPath.Alias;
         AddObjectPath(tableObjectPath);
      }

      /// <summary>
      ///    Adds path to the object with x-argument
      /// </summary>
      public void AddXArgumentObjectPath(FormulaUsablePath xArgumentObjectPath)
      {
         XArgumentAlias = xArgumentObjectPath.Alias;
         AddObjectPath(xArgumentObjectPath);
      }

      /// <summary>
      ///    Returns table formula object
      /// </summary>
      /// <param name="refObject">Entity using formula</param>
      public IQuantity GetTableObject(IUsingFormula refObject)
         => GetReferencedEntityByAlias<IQuantity>(TableObjectAlias, refObject);

      /// <summary>
      ///    Returns reference object used for table formula
      /// </summary>
      /// <param name="refObject">Entity using formula</param>
      public IQuantity GetXArgumentObject(IUsingFormula refObject)
         => GetReferencedEntityByAlias<IQuantity>(XArgumentAlias, refObject);

      /// <summary>
      ///    Returns the value of the table object for x = default value of table formula
      /// </summary>
      protected override double CalculateFor(IReadOnlyList<IObjectReference> usedObjects, IUsingFormula dependentObject)
      {
         var tableObject = GetTableObject(dependentObject);
         if (tableObject == null)
            throw new OSPSuiteException(Error.UnableToFindEntityWithAlias(TableObjectAlias));

         var tableFormula = tableObject.Formula as TableFormula;

         if (tableFormula == null)
            return tableObject.Value;

         var xArgumentObject = GetXArgumentObject(dependentObject);
         if (xArgumentObject == null)
            throw new OSPSuiteException(Error.UnableToFindEntityWithAlias(XArgumentAlias));

         return tableFormula.ValueAt(xArgumentObject.Value);
      }

      protected override void ValidateObjectReference(ObjectReference objectReference, FormulaUsablePath reference, IEntity dependentEntity)
      {
         //We do not want to throw a validation error if the alias is the XArgs as it can be undefined 
         if (reference.Alias == XArgumentAlias)
            return;

         base.ValidateObjectReference(objectReference, reference, dependentEntity);
      }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);

         var tableFormulaWithReference = source as TableFormulaWithXArgument;
         if (tableFormulaWithReference == null) return;

         TableObjectAlias = tableFormulaWithReference.TableObjectAlias;
         XArgumentAlias = tableFormulaWithReference.XArgumentAlias;
      }
   }
}