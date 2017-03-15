using System.Collections.Generic;
using OSPSuite.Core.Diagram;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Presentation.Presenters.ContextMenus
{
   public interface IMultipleBaseNodeContextMenuFactory : IContextMenuFactory<IReadOnlyList<IBaseNode>>
   {

   }

   public class MultipleBaseNodeContextMenuFactory : ContextMenuFactory<IReadOnlyList<IBaseNode>>, IMultipleBaseNodeContextMenuFactory
   {
      public MultipleBaseNodeContextMenuFactory(IRepository<IContextMenuSpecificationFactory<IReadOnlyList<IBaseNode>>> contextMenuSpecFactoryRepository)
         : base(contextMenuSpecFactoryRepository)
      {
      }
   }
}
