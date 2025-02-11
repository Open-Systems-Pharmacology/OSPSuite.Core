namespace OSPSuite.Core.Domain.Builder
{
   /// <summary>
   ///    Factory class to create well defined new <see cref="NeighborhoodBuilder" />
   /// </summary>
   public interface INeighborhoodBuilderFactory
   {
      /// <summary>
      ///    Creates a new instance of a <see cref="NeighborhoodBuilder" />.
      /// </summary>
      /// <returns> the new  <see cref="NeighborhoodBuilder" /></returns>
      NeighborhoodBuilder Create();

      /// <summary>
      ///    Creates a new <see cref="NeighborhoodBuilder" /> between <paramref name="firstNeighbor" /> and
      ///    <paramref name="secondNeighbor" />.
      /// </summary>
      /// <param name="firstNeighbor">The first neighbor.</param>
      /// <param name="secondNeighbor">The second neighbor.</param>
      /// <returns> the new  <see cref="NeighborhoodBuilder" /></returns>
      NeighborhoodBuilder CreateBetween(IContainer firstNeighbor, IContainer secondNeighbor);

      /// <summary>
      ///    Creates a new <see cref="NeighborhoodBuilder" /> between <paramref name="firstNeighborPath" /> and
      ///    <paramref name="secondNeighborPath" />.
      /// </summary>
      /// <param name="firstNeighborPath">The first neighbor path.</param>
      /// <param name="secondNeighborPath">The second neighbor path.</param>
      /// <param name="parentPath">
      ///    Optional parent path that will be added to the path created for first and second neighbor
      ///    (useful for dynamic structure)
      /// </param>
      /// <returns> the new  <see cref="NeighborhoodBuilder" /></returns>
      NeighborhoodBuilder CreateBetween(ObjectPath firstNeighborPath, ObjectPath secondNeighborPath, ObjectPath parentPath = null);
   }

   /// <summary>
   ///    Factory class to create well defined new <see cref="NeighborhoodBuilder" />
   /// </summary>
   public class NeighborhoodBuilderFactory : INeighborhoodBuilderFactory
   {
      private readonly IObjectBaseFactory _objectBaseFactory;
      private readonly IObjectPathFactory _objectPathFactory;

      /// <summary>
      ///    Initializes a new instance of the <see cref="NeighborhoodBuilderFactory" /> class.
      /// </summary>
      public NeighborhoodBuilderFactory(IObjectBaseFactory objectBaseFactory, IObjectPathFactory objectPathFactory)
      {
         _objectBaseFactory = objectBaseFactory;
         _objectPathFactory = objectPathFactory;
      }

      /// <summary>
      ///    Creates a new instance of a <see cref="NeighborhoodBuilder" />.
      /// </summary>
      /// <returns> the new  <see cref="NeighborhoodBuilder" /></returns>
      public NeighborhoodBuilder Create()
      {
         var neighborhoodBuilder = CreateNeighborhoodBuilder().WithMode(ContainerMode.Logical);
         var moleculeProperties = CreateMoleculeProperties()
            .WithName(Constants.MOLECULE_PROPERTIES)
            .WithMode(ContainerMode.Logical);

         neighborhoodBuilder.Add(moleculeProperties);

         return neighborhoodBuilder;
      }

      protected NeighborhoodBuilder CreateNeighborhoodBuilder() => _objectBaseFactory.Create<NeighborhoodBuilder>();

      protected IContainer CreateMoleculeProperties() => _objectBaseFactory.Create<IContainer>();

      public NeighborhoodBuilder CreateBetween(IContainer firstNeighbor, IContainer secondNeighbor)
      {
         return CreateBetween(_objectPathFactory.CreateAbsoluteObjectPath(firstNeighbor), _objectPathFactory.CreateAbsoluteObjectPath(secondNeighbor));
      }

      public NeighborhoodBuilder CreateBetween(ObjectPath firstNeighborPath, ObjectPath secondNeighborPath, ObjectPath parentPath = null)
      {
         var neighborhoodBuilder = Create();
         neighborhoodBuilder.FirstNeighborPath = mergePath(parentPath, firstNeighborPath);
         neighborhoodBuilder.SecondNeighborPath = mergePath(parentPath, secondNeighborPath) ;
         return neighborhoodBuilder;
      }

      private ObjectPath mergePath(ObjectPath parentPath, ObjectPath objectPath)
      {
         return parentPath == null ? objectPath : new ObjectPath(parentPath, objectPath);
      }
   }
}