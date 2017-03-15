namespace OSPSuite.Core.Serialization.SimModel.DTO
{
   /// <summary>
   /// Exports a assihnment in a event for an object
   /// </summary>
   public class AssigmentExport
   {
      public int ObjectId { get; set; }
      public int NewFormulaId { get; set; }
      public bool UseAsValue { get; set; }
   }
}