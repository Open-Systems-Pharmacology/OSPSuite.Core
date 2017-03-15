using System.Collections.Generic;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.MenuAndBars
{
   public interface IButtonGroup
   {
      string Id { get; set; }
      string Caption { get; set; }
      IEnumerable<IRibbonBarItem> Buttons { get; }
      void AddButton(IRibbonBarItem buttonToAdd);
      void WithLock(bool locked);
   }

   public class ButtonGroup : IButtonGroup
   {
      private readonly IList<IRibbonBarItem> _buttons;
      public string Caption { get; set; }
      public string Id { get; set; }

      public ButtonGroup()
      {
         _buttons = new List<IRibbonBarItem>();
      }

      public IEnumerable<IRibbonBarItem> Buttons => _buttons.All();

      public void AddButton(IRibbonBarItem buttonToAdd)
      {
         _buttons.Add(buttonToAdd);
      }

      public void WithLock(bool locked)
      {
         Buttons.Each(x => x.WithLock(locked));
      }
   }
}