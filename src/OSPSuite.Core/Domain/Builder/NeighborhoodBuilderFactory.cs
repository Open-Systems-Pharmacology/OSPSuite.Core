namespace OSPSuite.Core.Domain.Builder
{
   /// <summary>
   /// Factory class to create well defined new <see cref="NeighborhoodBuilder"/>
   /// </summary>
   public interface INeighborhoodBuilderFactory
   {
      /// <summary>
      /// Creates a new instance of a <see cref="NeighborhoodBuilder"/>.
      /// </summary>
      ///<returns> the new  <see cref="NeighborhoodBuilder"/></returns>
      NeighborhoodBuilder Create();
      /// <summary>
      /// Creates a new <see cref="NeighborhoodBuilder"/> between <paramref name="firstNeighbor"/> and <paramref name="secondNeighbor"/>.
      /// </summary>
      /// <param name="firstNeighbor">The first neighbor.</param>
      /// <param name="secondNeighbor">The second neighbor.</param>
      ///<returns> the new  <see cref="NeighborhoodBuilder"/></returns>
      NeighborhoodBuilder CreateBetween(IContainer firstNeighbor, IContainer secondNeighbor);
   }

   /// <summary>
   /// Factory class to create well defined new <see cref="NeighborhoodBuilder"/>
   /// </summary>
   public class NeighborhoodBuilderFactory : INeighborhoodBuilderFactory
   {
      private readonly IObjectBaseFactory _objectBaseFactory;
      private readonly IObjectPathFactory _objectPathFactory;

      /// <summary>
      /// Initializes a new instance of the <see cref="NeighborhoodBuilderFactory"/> class.
      /// </summary>
      public NeighborhoodBuilderFactory(IObjectBaseFactory objectBaseFactory, IObjectPathFactory objectPathFactory)
      {
         _objectBaseFactory = objectBaseFactory;
         _objectPathFactory = objectPathFactory;
      }

      /// <summary>
      /// Creates a new instance of a <see cref="NeighborhoodBuilder"/>.
      /// </summary>
      ///<returns> the new  <see cref="NeighborhoodBuilder"/></returns>
      public NeighborhoodBuilder Create()
      {
         var neighborhoodBuilder = CreateNeighborhoodBuilder().WithMode(ContainerMode.Logical);
         var moleculeProperties = CreateMoleculeProperties()
            .WithName(Constants.MOLECULE_PROPERTIES)
            .WithMode(ContainerMode.Logical);

         neighborhoodBuilder.Add(moleculeProperties);

         return neighborhoodBuilder;
      }

      protected NeighborhoodBuilder CreateNeighborhoodBuilder()
      {
         return _objectBaseFactory.Create<NeighborhoodBuilder>();
      }

      protected IContainer CreateMoleculeProperties()
      {
         return _objectBaseFactory.Create<IContainer>();
      }

      /// <summary>
      /// Creates a new <see cref="NeighborhoodBuilder"/> between <paramref name="firstNeighbor"/> and <paramref name="secondNeighbor"/>.
      /// </summary>
      /// <param name="firstNeighbor">The first neighbor.</param>
      /// <param name="secondNeighbor">The second neighbor.</param>
      ///<returns> the new  <see cref="NeighborhoodBuilder"/></returns>
      public NeighborhoodBuilder CreateBetween(IContainer firstNeighbor, IContainer secondNeighbor)
      {
         var neighborhoodBuilder = Create();
         neighborhoodBuilder.FirstNeighborPath = _objectPathFactory.CreateAbsoluteObjectPath(firstNeighbor);
         neighborhoodBuilder.SecondNeighborPath = _objectPathFactory.CreateAbsoluteObjectPath(secondNeighbor);
         return neighborhoodBuilder;
      }
   }
}