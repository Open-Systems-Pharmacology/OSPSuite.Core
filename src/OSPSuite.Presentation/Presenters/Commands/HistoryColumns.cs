using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Presentation.Presenters.Commands
{
   public static class HistoryColumns
   {
      private static readonly ICache<string, ColumnProperties> _allColumns = new Cache<string, ColumnProperties>(x => x.Name);

      public static ColumnProperties State = CreateColumn("State", "State After Action", true);
      public static ColumnProperties CommandType = CreateColumn("CommandType", "Command Type", true);
      public static ColumnProperties ObjectType = CreateColumn("ObjectType", "Object Type", true);
      public static ColumnProperties Description = CreateColumn("Description", "Description", true);
      public static ColumnProperties User = CreateColumn("User", "User", true);
      public static ColumnProperties Time = CreateColumn("Time", "Time", true);
      public static ColumnProperties Id = CreateColumn("Id", "Id", false);
      public static ColumnProperties Comment = CreateColumn("Comment", "Comment", false);

      public static readonly int FixedColumnIndex = Comment.Index;

      public static IEnumerable<ColumnProperties> All()
      {
         return _allColumns;
      }

      public static ColumnProperties ColumnByName(string name)
      {
         ColumnProperties column;
         if (_allColumns.Contains(name))
            column = _allColumns[name];
         else
         {
            column = new ColumnProperties(_allColumns.Count(), name, true);
            _allColumns.Add(column);
         }
         return column;
      }

      internal static ColumnProperties CreateColumn(string name, string caption, bool isVisible)
      {
         var column = ColumnByName(name);
         column.Caption = caption;
         column.IsVisible = isVisible;

         return column;
      }
   }
}