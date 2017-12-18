using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain
{
   public class ValueOrigin
   {
      /// <summary>
      ///    Origin of the value
      /// </summary>
      public ValueOriginType Type { get; set; }

      /// <summary>
      ///    Optional description explaining the quantity value
      /// </summary>
      public string Description { get; set; }

      public ValueOrigin Clone()
      {
         var clone = new ValueOrigin();
         clone.UpdateFrom(this);
         return clone;
      }

      public void UpdateFrom(ValueOrigin valueOrigin)
      {
         if (valueOrigin == null)
            return;

         Type = valueOrigin.Type;
         Description = valueOrigin.Description;
      }

      public string Display
      {
         get
         {
            if (Type == null && string.IsNullOrEmpty(Description))
               return Captions.ValueOrigins.Undefined;

            return new[]
            {
               Type?.Display, Description
            }.Where(x => !string.IsNullOrWhiteSpace(x)).ToString("-");
         }
      }

      public override string ToString() => Display;
   }
}