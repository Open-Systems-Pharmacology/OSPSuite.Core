using OSPSuite.Utility.Collections;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation.Presenters.ContextMenus
{
   public interface ITabbedMdiChildViewContextMenuFactory : IContextMenuFactory<IMdiChildView>
   {
   }

   public class TabbedMdiChildViewContextMenuFactory : ContextMenuFactory<IMdiChildView>, ITabbedMdiChildViewContextMenuFactory
   {
      public TabbedMdiChildViewContextMenuFactory(IRepository<IContextMenuSpecificationFactory<IMdiChildView>> contextMenuSpecFactoryRepository)
         : base(contextMenuSpecFactoryRepository)
      {
      }
   }
}