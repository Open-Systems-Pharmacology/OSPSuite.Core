namespace OSPSuite.Core.Domain
{
   public static class NeighborhoodExtensions
   {
      public static Neighborhood WithFirstNeighbor(this Neighborhood neighborhood, IContainer firstNeighbor) 
      {
         neighborhood.FirstNeighbor = firstNeighbor;
         return neighborhood;
      }

      public static Neighborhood WithSecondNeighbor(this Neighborhood neighborhood, IContainer secondNeighbor) 
      {
         neighborhood.SecondNeighbor= secondNeighbor;
         return neighborhood;
      }

   }
}