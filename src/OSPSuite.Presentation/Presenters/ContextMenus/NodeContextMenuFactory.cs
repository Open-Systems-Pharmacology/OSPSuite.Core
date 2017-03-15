using OSPSuite.Utility.Extensions;
using OSPSuite.Presentation.Nodes;

namespace OSPSuite.Presentation.Presenters.ContextMenus
{
   public abstract class NodeContextMenuFactory<T> : IContextMenuSpecificationFactory<ITreeNode>
   {
      public IContextMenu CreateFor(ITreeNode treeNode, IPresenterWithContextMenu<ITreeNode> presenter)
      {
         return CreateFor(treeNode.TagAsObject.DowncastTo<T>(), presenter);
      }

      public abstract IContextMenu CreateFor(T objectRequestingContextMenu, IPresenterWithContextMenu<ITreeNode> presenter);

      public virtual bool IsSatisfiedBy(ITreeNode treeNode, IPresenterWithContextMenu<ITreeNode> presenter)
      {
         return treeNode.TagAsObject != null && treeNode.TagAsObject.IsAnImplementationOf<T>();
      }
   }
}