using OSPSuite.Utility.Format;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.DTO;

namespace OSPSuite.Presentation.Services
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