using System.Collections.Generic;
using System.Drawing;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Presentation.Presenters.Commands
{
   public interface IHistoryBrowserConfiguration
   {
      /// <summary>
      ///    add a dynamic column that will be displayed in the view
      /// </summary>
      /// <param name="dynamicColumnName">Name of dynamic column to add</param>
      /// <param name="dynamicColumnCaption">Caption displayed in the UI</param>
      void AddDynamicColumn(string dynamicColumnName, string dynamicColumnCaption);

      IEnumerable<string> AllDynamicColumnNames { get; }
      Color LabelColor { get; set; }
      Color NotReversibleColor { get; set; }
   }

   public class HistoryBrowserConfiguration : IHistoryBrowserConfiguration
   {
      //cache: key is name of column, value is description
      private readonly ICache<string, string> _dynamicColumns;
      public Color LabelColor { get; set; }
      public Color NotReversibleColor { get; set; }

      public HistoryBrowserConfiguration()
      {
         _dynamicColumns = new Cache<string, string>();
         LabelColor = Color.YellowGreen;
         NotReversibleColor = Color.LightGray;
      }

      public void AddDynamicColumn(string dynamicColumnName, string dynamicColumnCaption)
      {
         _dynamicColumns[dynamicColumnName] = dynamicColumnCaption;
         HistoryColumns.CreateColumn(dynamicColumnName, dynamicColumnCaption, true);
      }

      public IEnumerable<string> AllDynamicColumnNames
      {
         get { return _dynamicColumns.Keys; }
      }
   }
}