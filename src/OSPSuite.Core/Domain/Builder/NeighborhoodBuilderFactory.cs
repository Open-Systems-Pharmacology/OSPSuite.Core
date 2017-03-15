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
      INeighborhoodBuilder Create();
      /// <summary>
      /// Creates a new <see cref="NeighborhoodBuilder"/> between <paramref name="firstNeighbor"/> and <paramref name="secondNeighbor"/>.
      /// </summary>
      /// <param name="firstNeighbor">The first neighbor.</param>
      /// <param name="secondNeighbor">The second neighbor.</param>
      ///<returns> the new  <see cref="NeighborhoodBuilder"/></returns>
      INeighborhoodBuilder CreateBetween(IContainer firstNeighbor, IContainer secondNeighbor);
   }

   /// <summary>
   /// Factory class to create well defined new <see cref="NeighborhoodBuilder"/>
   /// </summary>
   public class NeighborhoodBuilderFactory : INeighborhoodBuilderFactory
   {
      private readonly IObjectBaseFactory _objectBaseFactory;

      /// <summary>
      /// Initializes a new instance of the <see cref="NeighborhoodBuilderFactory"/> class.
      /// </summary>
      /// <param name="objectBaseFactory">The object base factory used to create new objects.</param>
      public NeighborhoodBuilderFactory(IObjectBaseFactory objectBaseFactory)
      {
         _objectBaseFactory = objectBaseFactory;
      }

      /// <summary>
      /// Creates a new instance of a <see cref="NeighborhoodBuilder"/>.
      /// </summary>
      ///<returns> the new  <see cref="NeighborhoodBuilder"/></returns>
      public INeighborhoodBuilder Create()
      {
         var neighborhoodBuilder = CreateNeighborhoodBuilder().WithMode(ContainerMode.Logical);
         var moelculePropertiers = CreateMoleculeProperties()
            .WithName(Constants.MOLECULE_PROPERTIES)
            .WithMode(ContainerMode.Logical);

         neighborhoodBuilder.Add(moelculePropertiers);

         return neighborhoodBuilder;
      }

      protected INeighborhoodBuilder CreateNeighborhoodBuilder()
      {
         return _objectBaseFactory.Create<INeighborhoodBuilder>();
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
      public INeighborhoodBuilder CreateBetween(IContainer firstNeighbor, IContainer secondNeighbor)
      {
         var neighborhoohBuilder = Create();
         neighborhoohBuilder.FirstNeighbor = firstNeighbor;
         neighborhoohBuilder.SecondNeighbor = secondNeighbor;
         return neighborhoohBuilder;
      }
   }
}