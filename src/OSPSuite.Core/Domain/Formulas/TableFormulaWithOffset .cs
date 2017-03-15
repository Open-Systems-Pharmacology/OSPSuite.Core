using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain.Formulas
{
   /// <summary>
   ///    Used for calculating value of table formula using given offset in the X-dimension
   ///    Must have exactly two references: to some object with table formula and
   ///    to some another object used for offset calculating
   ///    The resulting formula value is:
   ///    TableFormulaWithOffset(X) = Table(X-Offset)
   /// </summary>
   public class TableFormulaWithOffset : Formula
   {
      public string TableObjectAlias { get; set; }
      public string OffsetObjectAlias { get; set; }

      /// <summary>
      ///    Add path to the object with table formula
      /// </summary>
      /// <param name="tableObjectPath"></param>
      public void AddTableObjectPath(IFormulaUsablePath tableObjectPath)
      {
         TableObjectAlias = tableObjectPath.Alias;
         AddObjectPath(tableObjectPath);
      }

      /// <summary>
      ///    Add path to the object with offset formula
      /// </summary>
      /// <param name="offsetObjectPath"></param>
      public void AddOffsetObjectPath(IFormulaUsablePath offsetObjectPath)
      {
         OffsetObjectAlias = offsetObjectPath.Alias;
         AddObjectPath(offsetObjectPath);
      }

      /// <summary>
      ///    Return table formula object
      /// </summary>
      /// <param name="refObject">Entity using formula</param>
      /// <returns></returns>
      public IFormulaUsable GetTableObject(IUsingFormula refObject)
      {
         return GetReferencedEntityByAlias(TableObjectAlias, refObject);
      }

      /// <summary>
      ///    Return offset formula object
      /// </summary>
      /// <param name="refObject">Entity using formula</param>
      /// <returns></returns>
      public IFormulaUsable GetOffsetObject(IUsingFormula refObject)
      {
         return GetReferencedEntityByAlias(OffsetObjectAlias, refObject);
      }

      /// <summary>
      ///    Return the value of the table object for 0-offset value
      /// </summary>
      protected override double CalculateFor(IEnumerable<IObjectReference> usedObjects, IUsingFormula dependentObject)
      {
         var tableObject = GetTableObject(dependentObject);

         TableFormula tableFormula = null;
         var usingFormula = tableObject as IUsingFormula;
         if (usingFormula != null)
            tableFormula = usingFormula.Formula as TableFormula;

         if (tableFormula != null)
            return tableFormula.ValueAt(0 - GetOffsetObject(dependentObject).Value);

         throw new OSPSuiteException(Error.TableFormulaWithOffsetUsesNonTableFormulaObject);
      }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);

         var tableFormulaWithOffset = source as TableFormulaWithOffset;
         if (tableFormulaWithOffset == null) return;

         TableObjectAlias = tableFormulaWithOffset.TableObjectAlias;
         OffsetObjectAlias = tableFormulaWithOffset.OffsetObjectAlias;
      }
   }
}