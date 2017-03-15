namespace OSPSuite.Presentation.MenuAndBars
{
   public static class CreateButtonGroup
   {
      public static IButtonGroup WithCaption(string caption)
      {
         return new ButtonGroup { Caption = caption };
      }

      public static IButtonGroup WithButton(this IButtonGroup buttonGroup, IRibbonBarItem ribbonBarItem)
      {
         buttonGroup.AddButton(ribbonBarItem);
         return buttonGroup;
      }

      public static IButtonGroup WithId(this IButtonGroup buttonGroup, string id)
      {
         buttonGroup.Id = id;
         return buttonGroup;
      }
   }
}