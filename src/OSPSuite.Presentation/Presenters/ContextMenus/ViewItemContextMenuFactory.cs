using OSPSuite.Utility.Collections;
using OSPSuite.Presentation.Core;

namespace OSPSuite.Presentation.Presenters.ContextMenus
{
   public interface IViewItemContextMenuFactory : IContextMenuFactory<IViewItem>
   {
   }

   public class ViewItemContextMenuFactory : ContextMenuFactory<IViewItem>, IViewItemContextMenuFactory
   {
      public ViewItemContextMenuFactory(IRepository<IContextMenuSpecificationFactory<IViewItem>> contextMenuSpecFactoryRepository)
         : base(contextMenuSpecFactoryRepository)
      {
      }
   }
}