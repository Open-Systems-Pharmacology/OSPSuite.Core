using System.Data;
using System.Globalization;
using System.Linq;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Core.Commands.Core
{
   public static class HistoryManagerExtensions
   {
      /// <summary>
      ///    Columns as defined in the template excel file. Needs to be adjusted whenever the template has changed
      /// </summary>
      internal static class ReportColumn
      {
         public const int State = 0;
         public const int User = 1;
         public const int Time = 2;
         public const int CommandType = 3;
         public const int ObjectType = 4;
         public const int Description = 5;
         public const int ExtendedDescription = 6;
         public const int Comment = 7;

         //Number of columns defined in the report
         public const int LastColumnIndex = Comment;
      }

      /// <summary>
      ///    Create a DataTable containing all visible commands defined in the history
      /// </summary>
      public static DataTable ToDataTable(this IHistoryManager historyManager)
      {
         var dataTable = new DataTable();
         var commandExpander = new CommandsExpander();
         int nextAvailableColumn = ReportColumn.LastColumnIndex + 1;

         //cache containg the column index of a dynamic property . The key is the name of the property
         var dynamicColumnMap = new Cache<string, int>();

         dataTable.Columns.Add("State", typeof (string));
         dataTable.Columns.Add("User", typeof (string));
         dataTable.Columns.Add("Time", typeof (string));
         dataTable.Columns.Add("Command Type", typeof (string));
         dataTable.Columns.Add("Object Type", typeof (string));
         dataTable.Columns.Add("Description", typeof (string));
         dataTable.Columns.Add("Extended Description", typeof (string));
         dataTable.Columns.Add("Comment", typeof (string));

         foreach (var history in historyManager.History)
         {
            var allCommands = commandExpander.ExpandsAndKeep(history.Command).Where(c => c.Visible);
            foreach (var command in allCommands)
            {
               var row = dataTable.NewRow();
               row[ReportColumn.State] = history.State;
               row[ReportColumn.User] = history.User;
               row[ReportColumn.Time] = history.DateTime.ToString(CultureInfo.InvariantCulture);
               row[ReportColumn.CommandType] = command.CommandType;
               row[ReportColumn.ObjectType] = command.ObjectType;
               row[ReportColumn.Description] = command.Description;
               row[ReportColumn.ExtendedDescription] = command.ExtendedDescription;
               row[ReportColumn.Comment] = command.Comment;

               foreach (var extendedProperty in command.AllExtendedProperties)
               {
                  if (!dynamicColumnMap.Contains(extendedProperty))
                  {
                     dynamicColumnMap.Add(extendedProperty, nextAvailableColumn++);
                     dataTable.Columns.Add(extendedProperty, typeof (string));
                  }
                  row[dynamicColumnMap[extendedProperty]] = command.ExtendedPropertyValueFor(extendedProperty);
               }
               dataTable.Rows.Add(row);
            }
         }

         return dataTable;
      }
   }
}