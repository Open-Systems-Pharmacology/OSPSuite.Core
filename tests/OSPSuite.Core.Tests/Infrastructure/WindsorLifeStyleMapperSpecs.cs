using Castle.Core;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Container;
using OSPSuite.Infrastructure.Container.Castle;

namespace OSPSuite.Infrastructure
{
   public class When_mapping_a_lifestyle_to_a_castle_windsor_lifestyle : ContextSpecification<IWindsorLifeStyleMapper>
   {
      private LifestyleType _result1;
      private LifestyleType _result2;

      [Observation]
      public void should_return_the_accurate_lifestyle()
      {
         _result1.ShouldBeEqualTo(LifestyleType.Singleton);
         _result2.ShouldBeEqualTo(LifestyleType.Transient);
      }

      protected override void Because()
      {
         _result1 = sut.MapFrom(LifeStyle.Singleton);
         _result2 = sut.MapFrom(LifeStyle.Transient);
      }

      protected override void Context()
      {
         sut = new WindsorLifeStyleMapper();
      }
   }
}