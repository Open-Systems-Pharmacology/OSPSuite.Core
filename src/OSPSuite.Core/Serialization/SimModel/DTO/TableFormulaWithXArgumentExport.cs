namespace OSPSuite.Core.Serialization.SimModel.DTO
{
   public class TableFormulaWithXArgumentExport : FormulaExport
   {
      /// <summary>
      /// Id of referenced table object (= object with table formula)
      /// </summary>
      public int TableObjectId { get; set; }

      /// <summary>
      /// Id of x-argument object (e.g. pH for solubility table )
      /// </summary>
      public int XArgumentObjectId { get; set; }
   }
}