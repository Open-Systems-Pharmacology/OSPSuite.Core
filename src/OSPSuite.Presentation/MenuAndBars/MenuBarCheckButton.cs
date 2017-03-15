using System;

namespace OSPSuite.Presentation.MenuAndBars
{
   public interface IMenuBarCheckItem : IMenuBarButton
   {
      bool Checked { get; set; }
      event Action<bool> CheckedChanged;
   }

   public class MenuBarCheckButton : MenuBarButton, IMenuBarCheckItem
   {
      private bool _checked;
      public event Action<bool> CheckedChanged = delegate { };

      public bool Checked
      {
         set { _checked = value; CheckedChanged(value); }
         get { return _checked; }
      }

      public override void Click()
      {
         Checked = !Checked;
      }
   }
}