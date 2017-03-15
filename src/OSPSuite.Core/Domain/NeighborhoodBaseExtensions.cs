namespace OSPSuite.Core.Domain
{
   public static class NeighborhoodBaseExtensions
   {
      public static TNeighborhood WithFirstNeighbor<TNeighborhood>(this TNeighborhood neighborhood, IContainer firstNeighbor) where TNeighborhood : INeighborhoodBase
      {
         neighborhood.FirstNeighbor = firstNeighbor;
         return neighborhood;
      }

      public static TNeighborhood WithSecondNeighbor<TNeighborhood>(this TNeighborhood neighborhood, IContainer secondNeighbor) where TNeighborhood : INeighborhoodBase
      {
         neighborhood.SecondNeighbor= secondNeighbor;
         return neighborhood;
      }

   }
}