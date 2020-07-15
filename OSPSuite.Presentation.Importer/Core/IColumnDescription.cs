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
      public enum MeasurmentLevel
      {
         DISCRETE,
         NUMERIC
      }
      public ColumnDescription(int index)
      {
         Index = index;
      }

      public ColumnDescription(int index, List<string> existingValues, MeasurmentLevel columnDataType )
      {
         Index = index;
         ExistingValues = existingValues;
         MeasurmentLevel Level = columnDataType;
      }
      public int Index { get; set; }
      public IList<string> ExistingValues { get; set; }
      public MeasurmentLevel Level { get; set; }
   }
}
