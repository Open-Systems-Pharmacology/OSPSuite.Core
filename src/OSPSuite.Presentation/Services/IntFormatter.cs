using OSPSuite.Utility.Format;

namespace OSPSuite.Presentation.Services
{
   public class IntFormatter: NumericFormatter<int>
   {
      public IntFormatter() : base(NumericFormatterOptions.Instance)
      {
      }
   }
}