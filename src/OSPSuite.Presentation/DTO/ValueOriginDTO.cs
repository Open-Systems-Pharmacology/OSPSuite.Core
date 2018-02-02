using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Extensions;

namespace OSPSuite.Presentation.DTO
{
   public class ValueOriginDTO : IWithValueOrigin
   {
      public string Caption { get; set; } = Captions.ValueOrigin.FormatForLabel(addColon: false);

      private readonly IWithValueOrigin _withValueOrigin;

      public ValueOriginDTO(IWithValueOrigin withValueOrigin)
      {
         _withValueOrigin = withValueOrigin;
      }

      public ValueOrigin ValueOrigin => _withValueOrigin.ValueOrigin;

      public void UpdateValueOriginFrom(ValueOrigin sourceValueOrigin)
      {
         _withValueOrigin.UpdateValueOriginFrom(sourceValueOrigin);
      }
   }
}