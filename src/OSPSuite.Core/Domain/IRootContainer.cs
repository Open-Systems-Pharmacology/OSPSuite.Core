namespace OSPSuite.Core.Domain
{
   /// <summary>
   /// Marker interface representing a container that should be used as "root container" to resolve objects
   /// with their path in a hierarchy. The root container does not have to be the top container
   /// </summary>
   public interface IRootContainer : IContainer
   {
      
   }
}