﻿using DevExpress.XtraTab;
using OSPSuite.Assets;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Views;

namespace OSPSuite.UI.Extensions
{
   public static class TabbedViewExtensions
   {
      public static XtraTabPage PageFrom(this ITabbedView view, ISubPresenterItem subPresenterItem)
      {
         return view.TabControl.TabPages[subPresenterItem.Index];
      }

      public static XtraTabPage AddTabbedView(this ITabbedView view, ISubPresenterItem subPresenterItem, IView viewToAdd)
      {
         return view.AddTabbedView(subPresenterItem.Index, viewToAdd);
      }

      public static XtraTabPage AddTabbedView(this ITabbedView view, int viewIndex, IView viewToAdd)
      {
         return view.TabControl.AddPageFrom(viewToAdd, viewIndex);
      }

      public static void SetTabEnabled(this ITabbedView view, ISubPresenterItem subPresenterItem, bool enabled)
      {
         view.PageFrom(subPresenterItem).PageEnabled = enabled;
      }

      public static void SetTabVisibility(this ITabbedView view, ISubPresenterItem subPresenterItem, bool visible)
      {
         var tab = view.PageFrom(subPresenterItem);
         tab.SuspendLayout();
         tab.PageVisible = visible;
         tab.ResumeLayout(true);
      }

      public static void SetTabIcon(this ITabbedView view, ISubPresenterItem subPresenterItem, ApplicationIcon icon)
      {
         view.PageFrom(subPresenterItem).ImageOptions.SvgImage = icon;
         view.PageFrom(subPresenterItem).ImageOptions.SvgImageSize = UIConstants.ICON_SIZE_TAB;
      }

      public static void ActivateTab(this ITabbedView view, ISubPresenterItem subPresenterItem)
      {
         view.SetTabVisibility(subPresenterItem, true);
         view.SetTabEnabled(subPresenterItem, true);
         view.TabControl.SelectedTabPage = view.PageFrom(subPresenterItem);
      }

      public static bool IsTabVisible(this ITabbedView view, ISubPresenterItem subPresenterItem)
      {
         return view.PageFrom(subPresenterItem).PageVisible;
      }
   }
}