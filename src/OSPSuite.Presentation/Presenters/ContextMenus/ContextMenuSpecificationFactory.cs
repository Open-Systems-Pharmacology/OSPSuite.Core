namespace OSPSuite.Presentation.Presenters.ContextMenus
{
   public interface IContextMenuSpecificationFactory<TObjectRequestingContextMenu> : IContextMenuFactory<TObjectRequestingContextMenu>
   {
      bool IsSatisfiedBy(TObjectRequestingContextMenu objectRequestingContextMenu, IPresenterWithContextMenu<TObjectRequestingContextMenu> presenter);
   }
}