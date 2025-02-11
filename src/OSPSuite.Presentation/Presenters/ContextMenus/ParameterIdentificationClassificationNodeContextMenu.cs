using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Presenters.Nodes;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace OSPSuite.Presentation.Presenters.ContextMenus
{
   internal class ParameterIdentificationClassificationNodeContextMenu : ExplorerClassificationNodeContextMenu
   {
      public ParameterIdentificationClassificationNodeContextMenu(ClassificationNode classificationNode, IExplorerPresenter presenter, IContainer container)
         : base(classificationNode, presenter, container)
      {
      }
   }

   public class ParameterIdentificationGroupingFolderTreeNodeContextMenuFactory : ClassificationNodeContextMenuFactory
   {
      private readonly IContainer _container;

      public ParameterIdentificationGroupingFolderTreeNodeContextMenuFactory(IContainer container) : base(ClassificationType.ParameterIdentification)
      {
         _container = container;
      }

      protected override IContextMenu CreateFor(ClassificationNode classificationNode, IExplorerPresenter presenter)
      {
         return new ParameterIdentificationClassificationNodeContextMenu(classificationNode, presenter, _container);
      }
   }
}