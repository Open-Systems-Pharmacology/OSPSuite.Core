using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using NPOI.HPSF;

namespace OSPSuite.Presentation.Importer.Core
{
   public interface IColumnDescription
   {
      int Index { get; set; } //the unparsed name

      IList<string> ExistingValues { get; set; }

      Type ColumnDataType { get; set;  } //this could even be an enum of our choosing
   }

   public class ColumnDescription
   {
      public enum MeasurementLevel
      {
         NOT_SET,
         DISCRETE,
         NUMERIC
      }
      public ColumnDescription(int index)
      {
         Index = index;
         Level = MeasurementLevel.NOT_SET;
         ExistingValues = new List<string>();
      }

      //not sure we are going to be needing this kind of constructor
      public ColumnDescription(int index, List<string> existingValues, MeasurementLevel columnDataType )
      {
         Index = index;
         ExistingValues = existingValues;
         Level = columnDataType;
      }
      public int Index { get; private set; }
      public IList<string> ExistingValues { get; set; }
      public MeasurementLevel Level { get; set; }
   }
}
