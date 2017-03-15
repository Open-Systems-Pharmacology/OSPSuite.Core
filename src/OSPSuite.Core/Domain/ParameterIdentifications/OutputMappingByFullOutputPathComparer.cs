using System.Collections.Generic;

namespace OSPSuite.Core.Domain.ParameterIdentifications
{
   public class OutputMappingByFullOutputPathComparer : IEqualityComparer<OutputMapping>
   {
      public bool Equals(OutputMapping x, OutputMapping y)
      {
         return Equals(x?.FullOutputPath, y?.FullOutputPath);
      }

      public int GetHashCode(OutputMapping x)
      {
         return x.FullOutputPath.GetHashCode();
      }
   }
}