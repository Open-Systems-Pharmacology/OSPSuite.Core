using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Presentation.Presenters.ContextMenus
{
   public interface IContextMenuFactory<TObjectRequestingContextMenu>
   {
      IContextMenu CreateFor(TObjectRequestingContextMenu objectRequestingContextMenu, IPresenterWithContextMenu<TObjectRequestingContextMenu> presenter);
   }

   public class ContextMenuFactory<TObjectRequestingContextMenu> : IContextMenuFactory<TObjectRequestingContextMenu>
   {
      private readonly IList<IContextMenuSpecificationFactory<TObjectRequestingContextMenu>> _contextMenuSpecFactoryRepository;

      public ContextMenuFactory(IRepository<IContextMenuSpecificationFactory<TObjectRequestingContextMenu>> contextMenuSpecFactoryRepository)
      {
         _contextMenuSpecFactoryRepository = contextMenuSpecFactoryRepository.All().ToList();
      }

      public IContextMenu CreateFor(TObjectRequestingContextMenu objectRequestingContextMenu, IPresenterWithContextMenu<TObjectRequestingContextMenu> presenter)
      {
         foreach (var factory in _contextMenuSpecFactoryRepository)
         {
            if (factory.IsSatisfiedBy(objectRequestingContextMenu, presenter))
               return factory.CreateFor(objectRequestingContextMenu, presenter);
         }

         return new EmptyContextMenu();
      }
   }
}