using System.Collections.Generic;
using System.Linq;
using OSPSuite.Presentation.Nodes;

namespace OSPSuite.Presentation.Presenters.ContextMenus
{
   public abstract class MultipleNodeContextMenuFactory<T> : IContextMenuSpecificationFactory<IReadOnlyList<ITreeNode>>
   {
      public virtual IContextMenu CreateFor(IReadOnlyList<ITreeNode> treeNodes, IPresenterWithContextMenu<IReadOnlyList<ITreeNode>> presenter)
      {
         return CreateFor(AllTagsFor(treeNodes), presenter);
      }

      protected abstract IContextMenu CreateFor(IReadOnlyList<T> objectRequestingContextMenu, IPresenterWithContextMenu<IReadOnlyList<ITreeNode>> presenter);

      public virtual bool IsSatisfiedBy(IReadOnlyList<ITreeNode> treeNodes, IPresenterWithContextMenu<IReadOnlyList<ITreeNode>> presenter)
      {
         return AllTagsFor(treeNodes).Count == treeNodes.Count;
      }

      protected virtual IReadOnlyList<T> AllTagsFor(IReadOnlyList<ITreeNode> treeNodes)
      {
         return treeNodes.Select(x => x.TagAsObject).Where(x => x != null).OfType<T>().ToList();
      }
   }
}