using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Extensions
{
   public static class DataTableExtensions
   {
      public static DataColumn AddColumn(this DataTable dataTable, string columnName)
      {
         return AddColumn<string>(dataTable, columnName);
      }

      public static DataColumn AddColumn(this DataTable dataTable, string columnName, Type type)
      {
         return AddColumn(dataTable, new DataColumn(columnName, type));
      }

      public static DataColumn AddColumn(this DataTable dataTable, DataColumn dataColumn)
      {
         dataTable.Columns.Add(dataColumn);
         return dataColumn;
      }

      public static DataColumn AddColumn<T>(this DataTable dataTable, string columnName)
      {
         return AddColumn(dataTable, columnName, typeof(T));
      }

      public static void AddColumns<T>(this DataTable dataTable, params string[] columnNames)
      {
         AddColumns<T>(dataTable, columnNames as IEnumerable<string>);
      }

      public static void AddColumns<T>(this DataTable dataTable, IEnumerable<string> columnNames)
      {
         columnNames.Each(name => dataTable.AddColumn<T>(name));
      }

      public static IReadOnlyList<T> AllValuesInColumn<T>(this DataTable dataTable, string columnName)
      {
         return dataTable.Rows.Cast<DataRow>().Select(row => row[columnName].ConvertedTo<T>()).ToList();
      }

      public static IReadOnlyList<T> AllValuesInColumn<T>(this DataView dataView, string columnName)
      {
         return dataView.Cast<DataRowView>().Select(row => row[columnName].ConvertedTo<T>()).ToList();
      }

      public static DataTable SubTable(this DataTable dataTable, IReadOnlyCollection<string> columnNames, bool distinctValues = true)
      {
         if (columnNames.Count == 0)
            return new DataTable();

         return dataTable.DefaultView.ToTable(distinctValues, columnNames.ToArray());
      }
   }
}