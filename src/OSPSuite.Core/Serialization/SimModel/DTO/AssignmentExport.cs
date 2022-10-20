namespace OSPSuite.Core.Serialization.SimModel.DTO
{
   /// <summary>
   /// Exports a assignment in a event for an object
   /// </summary>
   public class AssignmentExport
   {
      public int ObjectId { get; set; }
      public int NewFormulaId { get; set; }
      public bool UseAsValue { get; set; }
   }
}