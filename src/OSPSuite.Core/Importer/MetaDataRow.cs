using System;
using System.Collections.Generic;
using System.Data;

namespace OSPSuite.Core.Importer
{
   public class MetaDataRow : DataRow
   {
      public MetaDataRow(DataRowBuilder builder)
         : base(builder)
      {
      }

      public new MetaDataTable Table
      {
         get { return (MetaDataTable) base.Table; }
      }

      /// <summary>
      ///    This method checks whether all conditions are fullfilled.
      /// </summary>
      /// <param name="conditions">Dictionary of column name, value pairs.</param>
      /// <returns>True, if all conditions are fullfilled.</returns>
      public bool CheckConditions(Dictionary<string, string> conditions)
      {
         var valid = false;
         foreach (var condition in conditions)
         {
            if (!Table.Columns.ContainsName(condition.Key))
               throw new Exception($"Unknown column {condition.Key}.");

            valid = (this[condition.Key].ToString() == condition.Value);
         }
         return valid;
      }
   }
}