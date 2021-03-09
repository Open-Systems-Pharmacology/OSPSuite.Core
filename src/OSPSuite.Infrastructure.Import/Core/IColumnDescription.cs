using System.Collections.Generic;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class ColumnDescription
   {
      public enum MeasurementLevel
      {
         NotSet,
         Discrete,
         Numeric
      }

      public ColumnDescription(int index)
      {
         Index = index;
         Level = MeasurementLevel.NotSet;
         _existingValues = new List<string>();
      }

      //not sure we are going to be needing this kind of constructor
      public ColumnDescription(int index, List<string> existingValues, MeasurementLevel columnDataType)
      {
         Index = index;
         _existingValues = existingValues;
         Level = columnDataType;
      }

      public int Index { get; private set; }

      public IReadOnlyList<string> ExistingValues => (IReadOnlyList<string>)_existingValues;

      public MeasurementLevel Level { get; set; }

      protected readonly IList<string> _existingValues;

      public void AddExistingValues(string elementAt)
      {
         _existingValues.Add(elementAt);
      }
   }
}