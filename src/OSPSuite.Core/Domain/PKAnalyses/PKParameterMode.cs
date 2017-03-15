using System;

namespace OSPSuite.Core.Domain.PKAnalyses
{
   [Flags]
   public enum PKParameterMode
   {
      Single = 1 << 0,
      Multi = 1 << 1,
      Always = Single | Multi
   }

   public static class PKParameterModeExtensions
   {
      public static bool Is(this PKParameterMode type1, PKParameterMode type2)
      {
         return (type1 & type2) != 0;
      }
   }
}