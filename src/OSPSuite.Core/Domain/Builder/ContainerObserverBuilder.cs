namespace OSPSuite.Core.Domain.Builder
{
   /// <summary>
   /// Configuration for  molecule-specific observers which will be created in the
   /// molecule subcontainer of some model container, e.g. <para></para>
   ///  - average concentration in an organ<para></para>
   /// </summary>
   public interface IContainerObserverBuilder : IObserverBuilder
   {
     
   }

   public class ContainerObserverBuilder : ObserverBuilder, IContainerObserverBuilder
   {
   }
}