using OSPSuite.Utility.Collections;
using OSPSuite.Presentation.Nodes;

namespace OSPSuite.Presentation.Presenters.ContextMenus
{
   public interface ITreeNodeContextMenuFactory : IContextMenuFactory<ITreeNode>
   {
   }

   public class TreeNodeContextMenuFactory : ContextMenuFactory<ITreeNode>, ITreeNodeContextMenuFactory
   {
      public TreeNodeContextMenuFactory(IRepository<IContextMenuSpecificationFactory<ITreeNode>> contextMenuSpecFactoryRepository)
         : base(contextMenuSpecFactoryRepository)
      {
      }
   }
}