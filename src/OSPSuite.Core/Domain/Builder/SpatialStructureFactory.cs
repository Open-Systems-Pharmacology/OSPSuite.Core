using OSPSuite.Assets;

namespace OSPSuite.Core.Domain.Builder
{
   public interface ISpatialStructureFactory
   {
      SpatialStructure Create();

      /// <summary>
      ///    Creates the container holding the global molecule dependent properties of a spatial structure.
      ///    It is not part of the spatial structure created by <see cref="Create" /> and needs to be added explicitly
      /// </summary>
      IContainer CreateGlobalMoleculeDependentProperties();
   }

   public class SpatialStructureFactory : ISpatialStructureFactory
   {
      private readonly IObjectBaseFactory _objectBaseFactory;

      public SpatialStructureFactory(IObjectBaseFactory objectBaseFactory)
      {
         _objectBaseFactory = objectBaseFactory;
      }

      public virtual SpatialStructure Create()
      {
         var spatialStructure = CreateSpatialStructure();
         var neighborhoods = CreateNeighborhoods()
            .WithName(Constants.NEIGHBORHOODS)
            .WithMode(ContainerMode.Logical);
         spatialStructure.NeighborhoodsContainer = neighborhoods;

         var eventContainer = _objectBaseFactory.Create<IContainer>()
            .WithName(Constants.EVENTS)
            .WithMode(ContainerMode.Logical);

         spatialStructure.Add(eventContainer);

         return spatialStructure;
      }

      protected virtual IContainer CreateNeighborhoods()
      {
         return _objectBaseFactory.Create<IContainer>();
      }

      public IContainer CreateGlobalMoleculeDependentProperties()
      {
         return _objectBaseFactory.Create<IContainer>()
            .WithName(Constants.MOLECULE_PROPERTIES)
            .WithMode(ContainerMode.Logical)
            .WithContainerType(ContainerType.Molecule);
      }

      protected virtual SpatialStructure CreateSpatialStructure()
      {
         return _objectBaseFactory.Create<SpatialStructure>().WithName(DefaultNames.SpatialStructure);
      }
   }
}