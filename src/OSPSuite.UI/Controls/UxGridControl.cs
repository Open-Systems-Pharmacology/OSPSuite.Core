using System.ComponentModel;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Registrator;
using DevExpress.XtraGrid.Views.Base;

namespace OSPSuite.UI.Controls
{
   [ToolboxItem(true)]
   public class UxGridControl : GridControl
   {
      protected override BaseView CreateDefaultView()
      {
         return CreateView("UxGridView");
      }

      protected override void RegisterAvailableViewsCore(InfoCollection collection)
      {
         base.RegisterAvailableViewsCore(collection);
         collection.Add(new UxGridViewInfoRegistrator());
      }
   }

   public class UxGridViewInfoRegistrator : GridInfoRegistrator
   {
      public override string ViewName => "UxGridView";

      public override BaseView CreateView(GridControl grid)
      {
         return new UxGridView(grid);
      }
   }
}