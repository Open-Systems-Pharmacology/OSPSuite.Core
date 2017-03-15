using System;

namespace OSPSuite.Core.Domain
{
   [Flags]
   public enum QuantityType
   {
      Undefined = 2 << 0,
      Drug = 2 << 1,
      Metabolite = 2 << 2,
      Enzyme = 2 << 3,
      Transporter = 2 << 4,
      Complex = 2 << 5,
      OtherProtein = 2 << 6,
      Observer = 2 << 7,
      Parameter = 2 << 8,
      BaseGrid = 2 << 9,
      Time = BaseGrid,
      Protein = OtherProtein | Enzyme | Transporter,
      Molecule = Drug | Metabolite | Protein | Complex
   }

   public static class QuantityTypeExtensions
   {
      public static bool Is(this QuantityType type1, QuantityType type2)
      {
         return (type1 & type2) != 0;
      }
   }
}