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
      public void AddTableObjectPath(IFormulaUsablePath tableObjectPath)
      {
         TableObjectAlias = tableObjectPath.Alias;
         AddObjectPath(tableObjectPath);
      }

      /// <summary>
      ///    Adds path to the object with x-argument
      /// </summary>
      public void AddXArgumentObjectPath(IFormulaUsablePath xArgumentObjectPath)
      {
         XArgumentAlias = xArgumentObjectPath.Alias;
         AddObjectPath(xArgumentObjectPath);
      }

      /// <summary>
      ///    Returns table formula object
      /// </summary>
      /// <param name="refObject">Entity using formula</param>
      public IQuantity GetTableObject(IUsingFormula refObject)
      {
         return GetReferencedEntityByAlias(TableObjectAlias, refObject) as IQuantity;
      }

      /// <summary>
      ///    Returns reference object used for table formula
      /// </summary>
      /// <param name="refObject">Entity using formula</param>
      public IQuantity GetXArgumentObject(IUsingFormula refObject)
      {
         return GetReferencedEntityByAlias(XArgumentAlias, refObject) as IQuantity;
      }

      /// <summary>
      ///    Returns the value of the table object for x = default value of table formula
      /// </summary>
      protected override double CalculateFor(IEnumerable<IObjectReference> usedObjects, IUsingFormula dependentObject)
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