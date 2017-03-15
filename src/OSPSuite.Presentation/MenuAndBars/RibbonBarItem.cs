using System;
using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.MenuAndBars
{
   [Flags]
   public enum ItemStyle
   {
      Large = 2 << 0,
      Small = 2 << 1,
      All = Large | Small
   }

   public interface IRibbonBarItem
   {
      /// <summary>
      /// Style of the button in the ribbon
      /// </summary>
      ItemStyle ItemStyle { get; set; }

      /// <summary>
      /// The undrlying menu bar item
      /// </summary>
      IMenuBarItem MenuBarItem { get; }

      /// <summary>
      /// Does this ribbon starts a group
      /// </summary>
      bool BeginGroup { get; set; }

      /// <summary>
      /// Caption of the ribbon button. If not set, the caption of the underlying MenuBarItem will be taken
      /// </summary>
      string Caption { get; set; }

      /// <summary>
      /// Caption of the ribbon button. If not set, the icon of the underlying MenuBarItem will be taken
      /// </summary>
      ApplicationIcon Icon { get; set; }

      /// <summary>
      ///All menu items for the given ribbon item
      /// </summary>
      IEnumerable<IMenuBarItem> AllMenuItems();

      /// <summary>
      /// add an item as menu element for the ribbon
      /// </summary>
      void AddMenuElement(IMenuBarItem menuItem);
   }

   public class RibbonBarItem : IRibbonBarItem
   {
      private readonly IList<IMenuBarItem> _allMenuItems;
      private string _caption;

      public RibbonBarItem(IMenuBarItem menuBarItem)
      {
         _allMenuItems = new List<IMenuBarItem>();
         ItemStyle = ItemStyle.All;
         MenuBarItem = menuBarItem;
         BeginGroup = false;
         _caption = null;
         Icon = null;
      }

      public ItemStyle ItemStyle { get; set; }
      public IMenuBarItem MenuBarItem { get; private set; }
      public bool BeginGroup { get; set; }
      public ApplicationIcon Icon { get; set; }

      public string Caption
      {
         get { return _caption ?? MenuBarItem.Caption; }
         set { _caption = value; }
      }


      public IEnumerable<IMenuBarItem> AllMenuItems()
      {
         return _allMenuItems.All();
      }

      public void AddMenuElement(IMenuBarItem menuItem)
      {
         _allMenuItems.Add(menuItem);
      }
   }
}