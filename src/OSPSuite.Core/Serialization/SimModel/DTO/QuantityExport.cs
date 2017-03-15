namespace OSPSuite.Core.Serialization.SimModel.DTO
{
   public class QuantityExport
   {
      public int FormulaId { get; set; }
      public double? Value { get; set; }
      public int Id { get; set; }
      public string EntityId { set; get; }
      public string Path { get; set; }
      public string Unit { get; set; }
      public string Name { get; set; }
      public bool Persistable { get; set; }
   }
}