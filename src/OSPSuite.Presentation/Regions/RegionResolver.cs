using OSPSuite.Utility.Container;

namespace OSPSuite.Presentation.Regions
{
   public interface IRegionResolver
   {
      IRegion RegionWithName(RegionName regionName);
   }

   public class RegionResolver : IRegionResolver
   {
      private readonly IContainer _container;

      public RegionResolver(IContainer container)
      {
         _container = container;
      }

      public IRegion RegionWithName(RegionName regionName)
      {
         return _container.Resolve<IRegion>(regionName);
      }
   }
}