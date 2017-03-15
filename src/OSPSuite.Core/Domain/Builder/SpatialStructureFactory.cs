namespace OSPSuite.Core.Domain.Builder
{
   public interface ISpatialStructureFactory
   {
      ISpatialStructure Create();
   }

   public class SpatialStructureFactory : ISpatialStructureFactory
   {
      private readonly IObjectBaseFactory _objectBaseFactory;

      public SpatialStructureFactory(IObjectBaseFactory objectBaseFactory)
      {
         _objectBaseFactory = objectBaseFactory;
      }

      public virtual ISpatialStructure Create()
      {
         ISpatialStructure spatialStructure = CreateSpatialStructure();
         var neighborhoods = CreateNeighborhoods()
            .WithName(Constants.NEIGHBORHOODS)
            .WithMode(ContainerMode.Logical);
         spatialStructure.NeighborhoodsContainer = neighborhoods;

         var moleculeProperties = CreateGlobalMoleculeDependentProperties()
            .WithName(Constants.MOLECULE_PROPERTIES)
            .WithMode(ContainerMode.Logical)
            .WithContainerType(ContainerType.Molecule);

         spatialStructure.GlobalMoleculeDependentProperties = moleculeProperties;

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

      protected virtual ISpatialStructure CreateSpatialStructure()
      {
         return _objectBaseFactory.Create<ISpatialStructure>();
      }
   }
}