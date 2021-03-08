using System.Data;
using OSPSuite.Core.Domain;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.R.Domain
{
   public class AgingData
   {
      public int[] IndividualIds { get; set; }
      public string[] ParameterPaths { get; set; }
      public double[] Times { get; set; }
      public double[] Values { get; set; }

      public DataTable ToDataTable()
      {
         verifyData();
         var dataTable = new DataTable();
         dataTable.AddColumn<int>(Constants.Population.INDIVIDUAL_ID_COLUMN);
         dataTable.AddColumn<string>(Constants.Population.PARAMETER_PATH_COLUMN);
         dataTable.AddColumn<double>(Constants.Population.TIME_COLUMN);
         dataTable.AddColumn<double>(Constants.Population.VALUE_COLUMN);


         for (var i = 0; i < IndividualIds.Length; i++)
         {
            var row = dataTable.NewRow();
            row[Constants.Population.INDIVIDUAL_ID_COLUMN] = IndividualIds[i];
            row[Constants.Population.PARAMETER_PATH_COLUMN] = ParameterPaths[i];
            row[Constants.Population.TIME_COLUMN] = Times[i];
            row[Constants.Population.VALUE_COLUMN] = Values[i];
            dataTable.Rows.Add(row);
         }

         return dataTable;
      }

      private void verifyData()
      {
         if (IndividualIds == null)
            raiseUndefinedError("IndividualIds");

         if (ParameterPaths == null)
            raiseUndefinedError("ParameterPaths");

         if (Times == null)
            raiseUndefinedError("Times");

         if (Values == null)
            raiseUndefinedError("Values");

         if (IndividualIds.Length != ParameterPaths.Length || IndividualIds.Length != Times.Length || IndividualIds.Length != Values.Length)
            throw new InvalidArgumentException($"AgingData values do not have the expected length ({IndividualIds.Length})");
      }

      private void raiseUndefinedError(string type) => throw new InvalidArgumentException($"'{type}' is undefined");
   }
}