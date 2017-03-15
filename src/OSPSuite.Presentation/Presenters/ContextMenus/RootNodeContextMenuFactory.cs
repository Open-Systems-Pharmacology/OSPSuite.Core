using OSPSuite.Utility.Extensions;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Presentation.Presenters.Nodes;
using OSPSuite.Presentation.Repositories;

namespace OSPSuite.Presentation.Presenters.ContextMenus
{
   public abstract class RootNodeContextMenuFactory : IContextMenuSpecificationFactory<ITreeNode>
   {
      private readonly RootNodeType _rootNodeType;
      protected readonly IMenuBarItemRepository _repository;

      protected RootNodeContextMenuFactory(RootNodeType rootNodeType, IMenuBarItemRepository repository)
      {
         _rootNodeType = rootNodeType;
         _repository = repository;
      }

      public IContextMenu CreateFor(ITreeNode treeNode, IPresenterWithContextMenu<ITreeNode> presenter)
      {
         return CreateFor(treeNode.DowncastTo<ITreeNode<RootNodeType>>(), presenter);
      }

      public abstract IContextMenu CreateFor(ITreeNode<RootNodeType> treeNode, IPresenterWithContextMenu<ITreeNode> presenter);

      public virtual bool IsSatisfiedBy(ITreeNode treeNode, IPresenterWithContextMenu<ITreeNode> presenter)
      {
         var rootNode = treeNode as ITreeNode<RootNodeType>;
         return rootNode != null && _rootNodeType.Equals(rootNode.Tag);
      }
   }
}