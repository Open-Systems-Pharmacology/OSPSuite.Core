using System.Collections.Generic;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Importer
{
   public class DimensionInfo 
   {
      public IDimension Dimension { get; set; }
      public bool IsMainDimension { get; set; }
      public IList<IParameter> ConversionParameters { get; private set; }
      public DimensionInfo()
      {
         ConversionParameters = new List<IParameter>();
      }
   }
}