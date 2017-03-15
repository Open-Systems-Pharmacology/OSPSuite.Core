using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Commands
{
   public class CellValueChanged
   {
      public int RowIndex { get; set; }
      public string ColumnId { get; set; }
      /// <summary>
      /// old value in base unit
      /// </summary>
      public float OldValue { get; set; }

      /// <summary>
      /// new value in base unit
      /// </summary>
      public float NewValue { get; set; }

      public CellValueChanged Clone()
      {
         return MemberwiseClone().DowncastTo<CellValueChanged>();
      }
   }
}