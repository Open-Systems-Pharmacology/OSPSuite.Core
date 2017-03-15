using DevExpress.XtraBars.Ribbon;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Mappers;

namespace OSPSuite.UI.Views
{
   public class StatusBarView : IStatusBarView
   {
      private readonly RibbonBarManager _barManager;
      private readonly IStatusBarElementToBarItemMapper _mapper;

      public StatusBarView(RibbonBarManager barManager, IStatusBarElementToBarItemMapper mapper)
      {
         _barManager = barManager;
         _mapper = mapper;
      }

      public void AddItem(StatusBarElement elementToAdd)
      {
         statusBar.ItemLinks.Add(_mapper.MapFrom(elementToAdd));
      }

      public IStatusBarElementExpression BarElementExpressionFor(StatusBarElement statusBarElement)
      {
         if (statusBar.ItemLinks.Count > statusBarElement.Index)
            return new StatusBarElementExpression(statusBar.ItemLinks[statusBarElement.Index]);
         return new NullStatusBarElementExpression();
      }

      private RibbonStatusBar statusBar
      {
         get
         {
            return _barManager.Ribbon.StatusBar;
            //return _barManager.StatusBar ?? _barManager.CreateStatusBar();
         }
      }
   }

}