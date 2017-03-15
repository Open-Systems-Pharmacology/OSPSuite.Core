using System.Collections.Generic;
using OSPSuite.Utility.Collections;
using OSPSuite.Presentation.Nodes;

namespace OSPSuite.Presentation.Presenters.ContextMenus
{
   public interface IMultipleTreeNodeContextMenuFactory : IContextMenuFactory<IReadOnlyList<ITreeNode>>
   {
   }

   public class MultipleTreeNodeContextMenuFactory : ContextMenuFactory<IReadOnlyList<ITreeNode>>, IMultipleTreeNodeContextMenuFactory
   {
      public MultipleTreeNodeContextMenuFactory(IRepository<IContextMenuSpecificationFactory<IReadOnlyList<ITreeNode>>> contextMenuSpecFactoryRepository)
         : base(contextMenuSpecFactoryRepository)
      {
      }
   }
}