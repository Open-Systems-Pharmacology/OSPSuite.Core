namespace OSPSuite.Core.Domain
{
   public interface INeighborhoodBase : IContainer
   {
      /// <summary>
      /// First neighboor in the neighborhood. 
      /// </summary>
      IContainer FirstNeighbor { get; set; }

      /// <summary>
      ///  Second neighboor in the neighborhood. 
      /// </summary>
      IContainer SecondNeighbor { get; set; }
   }
}