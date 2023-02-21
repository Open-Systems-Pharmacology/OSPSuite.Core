using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Presentation.Presenters.Nodes;
using OSPSuite.Presentation.UICommands;
using OSPSuite.Utility.Container;

namespace OSPSuite.Presentation.Presenters.ContextMenus
{
   public static class ObservedDataClassificationCommonContextMenuItems
   {
      /// <summary>
      ///    Creates an IMenuBarButton that edits metadata on all the leaf nodes of type ObservedDataNode
      /// </summary>
      /// <param name="classificationNode">The node whose leaf nodes contain the observed data to be edited</param>
      /// <param name="container">the container from which commands will be resolved</param>
      /// <returns>The MenuBarButton</returns>
      public static IMenuBarButton EditMultipleMetaData(ITreeNode classificationNode, IContainer container)
      {
         var repositories = classificationNode.AllNodes<ObservedDataNode>().Select(x => x.Tag.Repository);

         return CreateMenuButton.WithCaption(MenuNames.EditAllMetaData)
            .WithCommandFor<EditMultipleMetaDataUICommand, IEnumerable<DataRepository>>(repositories, container)
            .WithIcon(ApplicationIcons.Edit);
      }

      /// <summary>
      ///    Creates an IMenuBarButton that sets the Color Grouping option for charts
      /// </summary>
      /// <param name="userSettings">The user settings that contain Color Grouping option</param>
      /// <returns>The MenuBarButton</returns>
      public static IMenuBarButton ColorGroupObservedData(IPresentationUserSettings userSettings)
      {
         return CreateMenuCheckButton.WithCaption(MenuNames.ColorGroupObservedData)
            .WithChecked(userSettings.ColorGroupObservedDataFromSameFolder)
            .WithCheckedAction(colorGroup => userSettings.ColorGroupObservedDataFromSameFolder = colorGroup);
      }
   }
}