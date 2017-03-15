namespace OSPSuite.Presentation.DTO
{
   public class CellValueChangedDTO
   {
      public int RowIndex { get; set; }
      public int ColumnIndex { get; set; }
      public float OldDisplayValue { get; set; }
      public float NewDisplayValue { get; set; }
   }
}