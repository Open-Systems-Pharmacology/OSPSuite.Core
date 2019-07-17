using System.Collections.Generic;
using System.Data;
using System.Linq;
using OSPSuite.Core.Commands.Core;
using OSPSuite.TeXReporting.Data;
using OSPSuite.TeXReporting.Items;
using OSPSuite.TeXReporting.TeX.Converter;

namespace OSPSuite.Infrastructure.Reporting
{
   public class HistoryReporter : OSPSuiteTeXReporter<IHistoryManager>
   {
      private const string ID = "ID";
      private const string STATE = "State";
      private bool _verbose;

      public override IReadOnlyCollection<object> Report(IHistoryManager historyManager, OSPSuiteTracker tracker)
      {
         _verbose = tracker.Settings.Verbose;
         List<object> additionalObjects;

         var objectsToReport = new List<object>
         {
            new Chapter("History"),
            new Section("History Table"),
            new InLandscape(new[]
            {
               arrangeTable(historyManager.ToDataTable(),
                  out additionalObjects)
            })
         };
         objectsToReport.AddRange(additionalObjects);
         return objectsToReport;
      }

      private DataTable arrangeTable(DataTable dt, out List<object> additionalObjects)
      {
         const string CHECK_EMPTY_FILTER = "[{0}] IS NOT NULL AND [{0}] <> ''";
         const string DESCRIPTION = "Description";
         const string COMMENT = "Comment";
         const string EXTENDED_DESCRIPTION = "Extended Description";
         const string HT_D = "HT_D";
         const string HT_ED = "HT_ED";
         const string HT_C = "HT_C";

         DataTable extendedDescriptionTable = null;
         DataTable commentTable = null;

         additionalObjects = new List<object>();

         var ndt = dt.Copy();
         ndt.Columns.Add(ID, typeof (int)).AsHidden();
         ndt.BeginLoadData();
         for (int i = 0; i < ndt.Rows.Count; i++)
         {
            ndt.Rows[i][ID] = i;
         }
         ndt.EndLoadData();

         var historyTable = ndt.DefaultView.ToTable(false, ndt.Columns.Cast<DataColumn>()
            .Where(x => !x.ColumnName.Contains(DESCRIPTION) && x.ColumnName != COMMENT)
            .Select(x => x.ColumnName)
            .ToArray());
         historyTable.TableName = "History";

         var dataSet = new DataSet();
         ndt.DefaultView.RowFilter = string.Format(CHECK_EMPTY_FILTER, DESCRIPTION);
         var descriptionTable = ndt.DefaultView.ToTable(false, ndt.Columns.Cast<DataColumn>()
            .Where(x => x.ColumnName == ID || x.ColumnName == DESCRIPTION)
            .Select(x => x.ColumnName)
            .ToArray());

         dataSet.Tables.Add(historyTable);
         dataSet.Tables.Add(descriptionTable);
         dataSet.Relations.Add(new DataRelation(HT_D, historyTable.Columns[ID], descriptionTable.Columns[ID]));

         if (_verbose)
         {
            ndt.DefaultView.RowFilter = string.Format(CHECK_EMPTY_FILTER, EXTENDED_DESCRIPTION);
            extendedDescriptionTable = ndt.DefaultView.ToTable(false, ndt.Columns.Cast<DataColumn>()
               .Where(
                  x =>
                     x.ColumnName == ID ||
                     x.ColumnName == EXTENDED_DESCRIPTION)
               .Select(x => x.ColumnName)
               .ToArray());
            ndt.DefaultView.RowFilter = string.Format(CHECK_EMPTY_FILTER, COMMENT);
            commentTable = ndt.DefaultView.ToTable(false, ndt.Columns.Cast<DataColumn>()
               .Where(
                  x => x.ColumnName == ID || x.ColumnName == COMMENT)
               .Select(x => x.ColumnName)
               .ToArray());
            dataSet.Tables.Add(extendedDescriptionTable);
            dataSet.Relations.Add(new DataRelation(HT_ED, historyTable.Columns[ID], extendedDescriptionTable.Columns[ID]));
            dataSet.Tables.Add(commentTable);
            dataSet.Relations.Add(new DataRelation(HT_C, historyTable.Columns[ID], commentTable.Columns[ID]));
         }

         //ClearLargeStrings
         {
            List<object> addObjs;
            descriptionTable = clearLargeStrings(descriptionTable, DESCRIPTION, HT_D, out addObjs);
            additionalObjects.AddRange(addObjs);
         }
         if (_verbose)
         {
            {
               List<object> addObjs;
               extendedDescriptionTable = clearLargeStrings(extendedDescriptionTable, EXTENDED_DESCRIPTION, HT_ED,
                  out addObjs);
               additionalObjects.AddRange(addObjs);
            }
            {
               List<object> addObjs;
               commentTable = clearLargeStrings(commentTable, COMMENT, HT_C, out addObjs);
               additionalObjects.AddRange(addObjs);
            }
         }

         if (additionalObjects.Count > 0)
         {
            historyTable = historyTable.Copy();
            var newDataSet = new DataSet();
            newDataSet.Tables.Add(historyTable);
            newDataSet.Tables.Add(descriptionTable);
            newDataSet.Relations.Add(new DataRelation(HT_D, historyTable.Columns[ID], descriptionTable.Columns[ID]));
            if (_verbose)
            {
               if (extendedDescriptionTable != null)
               {
                  newDataSet.Tables.Add(extendedDescriptionTable);
                  newDataSet.Relations.Add(new DataRelation(HT_ED, historyTable.Columns[ID],
                     extendedDescriptionTable.Columns[ID]));
               }
               if (commentTable != null)
               {
                  newDataSet.Tables.Add(commentTable);
                  newDataSet.Relations.Add(new DataRelation(HT_C, historyTable.Columns[ID], commentTable.Columns[ID]));
               }
            }
         }

         return historyTable;
      }

      private DataTable clearLargeStrings(DataTable dt, string columnName, string relationName, out List<object> additionalObjects)
      {
         var ndt = dt.Clone();
         ndt.Columns[columnName].DataType = typeof (Text);

         additionalObjects = new List<object>();

         //fill new data table
         foreach (DataRow row in dt.Rows)
         {
            var cValue = row[columnName].ToString();
            if (textIsToBig(cValue)) 
            {
               var refSection = new Section($"{columnName} for {STATE} {row.GetParentRow(relationName)[STATE]}");
               additionalObjects.Add(refSection);
               additionalObjects.Add(cValue);
               ndt.Rows.Add(new[]
               {
                  new Text("see {0}", new Reference(refSection)) {Converter = NoConverter.Instance},
                  row[ID]
               });
            }
            else
            {
               ndt.Rows.Add(new[] {new Text(cValue), row[ID]});
            }
         }
         return ndt;
      }

      private static bool textIsToBig(string text)
      {
         return text.Length >= 500 || text.Count(c => c == '\n') > 30;
      }
   }
}