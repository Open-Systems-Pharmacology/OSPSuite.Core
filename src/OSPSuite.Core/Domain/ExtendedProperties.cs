using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain
{
   public class ExtendedProperties : Cache<string, IExtendedProperty>
   {
      public ExtendedProperties() : base(x => x.Name)
      {
      }

      public ExtendedProperties Clone()
      {
         var clone = new ExtendedProperties();
         this.Each(ep => clone.Add(ep.Clone()));
         return clone;
      }

      public void UpdateFrom(ExtendedProperties extendedProperties)
      {
         Clear();
         extendedProperties.Each(ep => Add(ep.Clone()));
      }
   }
}