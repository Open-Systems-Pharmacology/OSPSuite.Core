using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.UICommands;

namespace OSPSuite.Presentation.Presenters.ContextMenus
{
   public static class ComparisonCommonContextMenuItems
   {
      public static IMenuBarButton CompareObjectsMenu(IReadOnlyList<IObjectBase> objectsToCompare, IOSPSuiteExecutionContext context)
      {
         return CompareObjectsMenu(objectsToCompare, objectsToCompare.AllNames(), context);
      }

      public static IMenuBarButton CompareObjectsMenu(IReadOnlyList<IObjectBase> objectsToCompare, IReadOnlyList<string> objectNames, IOSPSuiteExecutionContext context)
      {
         if (objectsToCompare.Count != 2)
            return null;

         if (objectsToCompare.Count != objectNames.Count)
            return null;

         var menu =  CreateMenuButton.WithCaption(MenuNames.CompareObjects(context.TypeFor(objectsToCompare[0])))
            .WithCommandFor<CompareObjectsUICommand, IReadOnlyList<IObjectBase>>(objectsToCompare)
            .WithIcon(ApplicationIcons.Comparison);

         var compareObjectCommand = menu.Command.DowncastTo<CompareObjectsUICommand>();
         compareObjectCommand.ObjectNames = objectNames;
         return menu;
      }
   }
}