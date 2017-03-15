using OSPSuite.Assets;
using OSPSuite.Utility.Format;

namespace OSPSuite.Presentation.DTO.ParameterIdentifications
{
   public interface IRunPropertyDTO
   {
      string Name { get; }
      string FormattedValue { get; }
      ApplicationIcon Icon { get; }
   }

   public class RunPropertyDTO<T> : IRunPropertyDTO
   {
      private readonly IFormatter<T> _formatter;
      public string Name { get; }
      public T Value { get; set; }
      public ApplicationIcon Icon { get; set; }
      public string FormattedValue => _formatter.Format(Value);

      public RunPropertyDTO(string name, T value, IFormatter<T> formatter = null, ApplicationIcon icon = null)
      {
         _formatter = formatter ?? new DefaultFormatter<T>();
         Name = name;
         Value = value;
         Icon = icon;
      }
   }
}