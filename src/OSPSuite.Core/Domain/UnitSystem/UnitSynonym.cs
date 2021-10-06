using System;

namespace OSPSuite.Core.Domain.UnitSystem
{
   public class UnitSynonym 
   {
      public string Name { get; }

      [Obsolete("For serialization")]
      public UnitSynonym()
      {
      }

      public UnitSynonym(string name)
      {
         Name = name;
      }
   }
}