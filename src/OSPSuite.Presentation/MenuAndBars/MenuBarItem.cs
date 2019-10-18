using System;
using OSPSuite.Assets;
using OSPSuite.Presentation.Core;

namespace OSPSuite.Presentation.MenuAndBars
{
   public interface IMenuBarItem
   {
      /// <summary>
      ///    Caption of menu element to be displayed
      /// </summary>
      string Caption { get; set; }

      /// <summary>
      ///    Name of element that should be unique over all the menus
      /// </summary>
      string Name { get; set; }

      /// <summary>
      ///    Description displayed as tool tip text
      /// </summary>
      string Description { get; set; }

      /// <summary>
      ///    set whether the button begins a group or not
      /// </summary>
      bool BeginGroup { get; set; }

      /// <summary>
      ///    Is the button enabled?
      /// </summary>
      bool Enabled { get; set; }

      /// <summary>
      ///    Is the button visible?
      /// </summary>
      bool Visible { get; set; }

      /// <summary>
      ///    Event is raised whenever the enable state of the element has changed
      /// </summary>
      event Action<bool> EnabledChanged;

      /// <summary>
      ///    Event is raised whenever the visibility of the element has changed
      /// </summary>
      event Action<bool> VisibilityChanged;

      /// <summary>
      ///    Icon used in the menu
      /// </summary>
      ApplicationIcon Icon { get; set; }

      /// <summary>
      ///    Shortcut for the given item
      /// </summary>
      Keys Shortcut { get; set; }

      /// <summary>
      ///    Is this menu available for developer mode only?
      /// </summary>
      bool IsForDeveloper { get; set; }

      /// <summary>
      ///    Id of the element as integer that only plays a roll for deserialization (can be left as is for non persistant menu)
      /// </summary>
      int Id { get; set; }

      /// <summary>
      ///    DevExpress actions are always accessible via shortcut even if they are not the active ribbon. This lock
      ///    prevents the execution of the shortcut action when the menu item is hidden
      /// </summary>
      bool Locked { set; get; }
   }

   public abstract class MenuBarItem : IMenuBarItem
   {
      private bool _enabled;
      private bool _visible;
      public string Caption { get; set; }
      public string Description { get; set; }
      public bool BeginGroup { get; set; }
      public string Name { get; set; }

      public event Action<bool> EnabledChanged = delegate { };
      public event Action<bool> VisibilityChanged = delegate { };
      public Keys Shortcut { get; set; } = Keys.None;
      public ApplicationIcon Icon { get; set; }
      public bool IsForDeveloper { get; set; }
      public int Id { get; set; }
      public bool Locked { get; set; }

      protected MenuBarItem()
      {
         Name = Guid.NewGuid().ToString();
         BeginGroup = false;
         IsForDeveloper = false;
         Icon = null;
         _enabled = true;
         _visible = true;
      }

      public bool Enabled
      {
         get => _enabled;
         set
         {
            _enabled = value;
            EnabledChanged(value);
         }
      }

      public bool Visible
      {
         get => _visible;
         set
         {
            _visible = value;
            VisibilityChanged(value);
         }
      }
   }
}