using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Container;
using FakeItEasy;
using OSPSuite.Presentation.Regions;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_RegionResolver : ContextSpecification<IRegionResolver>
   {
      protected IContainer _container;

      protected override void Context()
      {
         _container = A.Fake<IContainer>();
         sut = new RegionResolver(_container);
      }
   }

   public class When_retrieving_a_region_by_name : concern_for_RegionResolver
   {
      private RegionName _regionName;
      private IRegion _result;
      private IRegion _region;

      protected override void Context()
      {
         base.Context();
         _regionName = new RegionName("toto", "tata", null);
         _region = A.Fake<IRegion>();
         A.CallTo(() => _container.Resolve<IRegion>(_regionName)).Returns(_region);
      }
      protected override void Because()
      {
         _result = sut.RegionWithName(_regionName);
      }

      [Observation]
      public void should_leverage_the_container_to_retrieve_a_region_registered_with_this_name()
      {
         _result.ShouldBeEqualTo(_region);
      }
   }
}