using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_TimePath : ContextSpecification<TimePath>
   {
      protected IDimension _time;

      protected override void Context()
      {
         _time = A.Fake<IDimension>();
         sut = new TimePath { TimeDimension = _time };
      }
   }

   class When_resolving_a_time_path : concern_for_TimePath
   {  
      private IContainer _depObject;
      private IUsingFormula _res;

      protected override void Context()
      {
         base.Context();
         _depObject = A.Fake<IContainer>();
      }

      protected override void Because()
      {
         _res = sut.Resolve<IUsingFormula>(_depObject);
      }

      [Observation]
      public void should_create_a_time_parameter()
      {
         _res.GetType().ShouldBeEqualTo(typeof(TimeParameter));
      }

      [Observation]
      public void returned_object_should_have_dimension_time()
      {
         _res.Dimension.ShouldBeEqualTo(_time);
      }

      [Observation]
      public void the_time_path_has_an_appropriate_path()
      {
         sut.First().ShouldBeEqualTo(Constants.TIME);
      }
   }
}