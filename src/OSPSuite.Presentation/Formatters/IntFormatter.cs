using OSPSuite.Utility.Format;

namespace OSPSuite.Presentation.Formatters
{
   public class IntFormatter: NumericFormatter<int>
   {
      public IntFormatter() : base(NumericFormatterOptions.Instance)
      {
      }
   }
}