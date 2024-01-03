namespace OSPSuite.Core.Domain.Services
{
   internal class PathAndValueCreator
   {
      private readonly IEntityPathResolver _entityPathResolver;

      public PathAndValueCreator(IEntityPathResolver entityPathResolver)
      {
         _entityPathResolver = entityPathResolver;
      }

      /// <summary>
      ///    Return the ObjectPath for the <paramref name="container" />. Prepend the ParentPath of the root container if it
      ///    exists.
      /// </summary>
      protected ObjectPath ObjectPathForContainer(IContainer container)
      {
         var objectPath = _entityPathResolver.ObjectPathFor(container);
         var rootContainer = container.RootContainer;
         if (rootContainer.ParentPath != null)
            objectPath.AddAtFront(rootContainer.ParentPath);

         return objectPath;
      }
   }
}