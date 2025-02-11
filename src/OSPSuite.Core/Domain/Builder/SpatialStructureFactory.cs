using OSPSuite.Assets;

namespace OSPSuite.Core.Domain.Builder
{
   public interface ISpatialStructureFactory
   {
      SpatialStructure Create();
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

         var moleculeProperties = CreateGlobalMoleculeDependentProperties()
            .WithName(Constants.MOLECULE_PROPERTIES)
            .WithMode(ContainerMode.Logical)
            .WithContainerType(ContainerType.Molecule);

         spatialStructure.GlobalMoleculeDependentProperties = moleculeProperties;

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

      protected virtual IContainer CreateGlobalMoleculeDependentProperties()
      {
         return _objectBaseFactory.Create<IContainer>();
      }

      protected virtual SpatialStructure CreateSpatialStructure()
      {
         return _objectBaseFactory.Create<SpatialStructure>().WithName(DefaultNames.SpatialStructure);
      }
   }
}