using OSPSuite.Core.Domain;
using OSPSuite.Presentation.DTO;
using OSPSuite.Utility.Format;

namespace OSPSuite.Presentation.Formatters
{
   public class ValueDTOFormatter : IFormatter<ValueDTO>
   {
      private readonly DoubleFormatter _doubleFormatter;

      public ValueDTOFormatter()
      {
         _doubleFormatter = new DoubleFormatter();
      }

      public string Format(ValueDTO valueDTO)
      {
         return _doubleFormatter.Format(valueDTO.DisplayValue, valueDTO.DisplayUnit);
      }
   }
}