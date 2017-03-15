using DevExpress.XtraBars;
using DevExpress.XtraTab;

namespace OSPSuite.UI.Views
{
   public interface ITabbedView
   {
      XtraTabControl TabControl { get; }
   }

   public interface IViewWithPopup
   {
      BarManager PopupBarManager { get;}
   }
}