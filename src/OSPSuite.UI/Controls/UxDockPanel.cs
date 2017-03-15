using System;
using System.Windows.Forms;
using DevExpress.XtraBars.Docking;
using OSPSuite.Assets;
using OSPSuite.Presentation.Regions;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Controls
{
   public class UxDockPanel : DockPanel, IRegion
   {
      public UxDockPanel()
      {
         ID = Guid.NewGuid();
         Options.ShowCloseButton = true;
      }

      public void Add(object view)
      {
         this.FillWith(view as Control);
      }

      public void ToggleVisibility()
      {
         if (Visibility == DockVisibility.Hidden)
            Visibility = DockVisibility.Visible;
         else
            Visibility = DockVisibility.Hidden;
      }

      public bool IsVisible => Visibility == DockVisibility.Visible;

      public void InitializeWith(RegionName regionName)
      {
         Text = regionName.Caption;
         ImageIndex = ApplicationIcons.IconIndex(regionName.Icon);
      }
   }
}