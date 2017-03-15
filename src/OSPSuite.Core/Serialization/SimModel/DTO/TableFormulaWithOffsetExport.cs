namespace OSPSuite.Core.Serialization.SimModel.DTO
{
   public class TableFormulaWithOffsetExport : FormulaExport
   {
      /// <summary>
      /// Id of referenced table object (= object with table formula)
      /// </summary>
      public int TableObjectId { get; set; }

      /// <summary>
      /// Id of referenced offset object (e.g. start time of table application)
      /// </summary>
      public int OffsetObjectId { get; set; }
   }
}
