using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Helpers;
using OSPSuite.Presentation.DTO;
using OSPSuite.Utility.Validation;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_OutputIntervalDTO : ContextSpecification<OutputIntervalDTO>
   {
      private IDimensionFactory _dimensionFactory;

      protected override void Context()
      {

         _dimensionFactory = DimensionFactoryForSpecs.Factory;
         var timeDimension = _dimensionFactory.Dimension(Constants.Dimension.TIME);
         sut = new OutputIntervalDTO {StartTimeParameter = A.Fake<IParameterDTO>(), EndTimeParameter = A.Fake<IParameterDTO>()};
         sut.StartTimeParameter.Parameter.Dimension = timeDimension;
         sut.EndTimeParameter.Parameter.Dimension = timeDimension;
         sut.StartTimeParameter.Parameter.DisplayUnit = timeDimension.BaseUnit;
         sut.EndTimeParameter.Parameter.DisplayUnit = timeDimension.BaseUnit;

         A.CallTo(() => sut.StartTimeParameter.KernelValue).ReturnsLazily(() => sut.StartTimeParameter.Value);
         A.CallTo(() => sut.EndTimeParameter.KernelValue).ReturnsLazily(() => sut.EndTimeParameter.Value);
      }
   }

   public class When_setting_the_value_for_minimum_smaller_than_maximum : concern_for_OutputIntervalDTO
   {

      [Observation]
      public void the_output_interval_should_be_valid()
      {
         sut.StartTimeParameter.Value = 5;
         sut.EndTimeParameter.Value = 10;
         sut.IsValid().ShouldBeTrue();
      }
   }

   public class When_setting_the_value_for_minimum_larrge_than_maximum : concern_for_OutputIntervalDTO
   {
      [Observation]
      public void the_output_interval_should_be_invalid()
      {
         sut.StartTimeParameter.Value = 3;
         sut.EndTimeParameter.Value = 1;
         sut.IsValid().ShouldBeFalse();
      }
   }
}
