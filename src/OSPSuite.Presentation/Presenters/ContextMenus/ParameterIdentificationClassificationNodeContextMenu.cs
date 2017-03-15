using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Presenters.Nodes;

namespace OSPSuite.Presentation.Presenters.ContextMenus
{
   internal class ParameterIdentificationClassificationNodeContextMenu : ClassificationNodeContextMenu<IExplorerPresenter>
   {
      public ParameterIdentificationClassificationNodeContextMenu(ClassificationNode classificationNode, IExplorerPresenter presenter)
         : base(classificationNode, presenter)
      {
      }
   }

   public class ParameterIdentificationGroupingFolderTreeNodeContextMenuFactory : ClassificationNodeContextMenuFactory
   {
      public ParameterIdentificationGroupingFolderTreeNodeContextMenuFactory() : base(ClassificationType.ParameterIdentification)
      {
      }

      protected override IContextMenu CreateFor(ClassificationNode classificationNode, IExplorerPresenter presenter)
      {
         return new ParameterIdentificationClassificationNodeContextMenu(classificationNode, presenter);
      }
   }
}