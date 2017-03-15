using System;

namespace OSPSuite.Core.Domain
{
   [Flags]
   public enum TransportType
   {
      Efflux= 2 << 0,
      Influx= 2 << 1,
      PgpLike= 2 << 2,
      Secretion= 2 << 3,
      Elimination= 2 << 4,
      Diffusion= 2 << 5,
      Convection= 2 << 6,
      Passive = Secretion | Elimination | Diffusion|Convection,
      Active = Influx| Efflux| PgpLike,
   }

   public static class TransportTypeExtensions
   {
      public static bool Is(this TransportType type1, TransportType type2)
      {
         return (type1 & type2) != 0;
      }
   }
}