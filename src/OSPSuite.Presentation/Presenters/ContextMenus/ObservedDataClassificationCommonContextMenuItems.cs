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

namespace OSPSuite.Presentation.Presenters.ContextMenus
{
   public static class ObservedDataClassificationCommonContextMenuItems
   {
      /// <summary>
      ///    Creates an IMenuBarButton that edits metadata on all the leaf nodes of type ObservedDataNode
      /// </summary>
      /// <param name="classificationNode">The node whose leaf nodes contain the observed data to be edited</param>
      /// <returns>The MenuBarButton</returns>
      public static IMenuBarButton CreateEditMultipleMetaDataMenuButton(ITreeNode classificationNode)
      {
         var repositories = classificationNode.AllNodes<ObservedDataNode>().Select(x => x.Tag.Repository);

         return CreateMenuButton.WithCaption(MenuNames.EditAllMetaData)
            .WithCommandFor<EditMultipleMetaDataUICommand, IEnumerable<DataRepository>>(repositories)
            .WithIcon(ApplicationIcons.Edit);
      }
   }
}