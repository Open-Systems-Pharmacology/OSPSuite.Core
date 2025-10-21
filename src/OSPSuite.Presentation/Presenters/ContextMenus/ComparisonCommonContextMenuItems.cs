using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.UICommands;
using OSPSuite.Utility.Extensions;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace OSPSuite.Presentation.Presenters.ContextMenus
{
   public static class ComparisonCommonContextMenuItems
   {
      public static IMenuBarButton CompareObjectsMenu(IReadOnlyList<IObjectBase> objectsToCompare, IOSPSuiteExecutionContext context, IContainer container)
      {
         return CompareObjectsMenu(objectsToCompare, objectsToCompare.AllNames(), context, container);
      }

      public static IMenuBarButton CompareBuildingBlocksMenu(IReadOnlyList<IBuildingBlock> buildingBlocksToCompare, IOSPSuiteExecutionContext context, IContainer container)
      {
         return CompareObjectsMenu(buildingBlocksToCompare, buildingBlocksToCompare.Select(x => x.DisplayName).ToList(), context, container);
      }

      public static IMenuBarButton CompareObjectsMenu(IReadOnlyList<IObjectBase> objectsToCompare, IReadOnlyList<string> objectNames, IOSPSuiteExecutionContext context, IContainer container)
      {
         if (objectsToCompare.Count != 2)
            return null;

         if (objectsToCompare.Count != objectNames.Count)
            return null;

         var menu = CreateMenuButton.WithCaption(MenuNames.CompareObjects(context.TypeFor(objectsToCompare[0])))
            .WithCommandFor<CompareObjectsUICommand, IReadOnlyList<IObjectBase>>(objectsToCompare, container)
            .WithIcon(ApplicationIcons.Comparison);

         var compareObjectCommand = menu.Command.DowncastTo<CompareObjectsUICommand>();
         compareObjectCommand.ObjectNames = objectNames;
         return menu;
      }
   }
}