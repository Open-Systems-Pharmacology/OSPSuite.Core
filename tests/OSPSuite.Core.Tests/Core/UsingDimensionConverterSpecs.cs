using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Core.Converter.v5_2;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core
{
   internal abstract class concern_for_UsingDimensionConverter : ContextSpecification<IUsingDimensionConverter>
   {
      protected IDimensionFactory _dimensionFactory;
      protected IWithDimension _withDimension;

      protected override void Context()
      {
         _dimensionFactory= A.Fake<IDimensionFactory>();
         _withDimension= A.Fake<IWithDimension>();
         sut = new UsingDimensionConverter(_dimensionFactory);
      }

      protected override void Because()
      {
         sut.Convert(_withDimension);
      }
   }

   internal class When_converting_an_object_with_dimension_that_still_references_an_old_dimension : concern_for_UsingDimensionConverter
   {
      private IDimension _oldDimension;
      private IDimension _newDimension;

      protected override void Context()
      {
         base.Context();
         _newDimension= A.Fake<IDimension>();
         _oldDimension = A.Fake<IDimension>();
         _withDimension.Dimension = _oldDimension;
         A.CallTo(() => _oldDimension.Name).Returns("DosePerBodyWeight");
         A.CallTo(() => _dimensionFactory.Dimension(ConverterConstants.DummyDimensions[_oldDimension.Name])).Returns(_newDimension);
      }

      [Observation]
      public void should_change_the_dimension_to_the_new_one()
      {
         _withDimension.Dimension.ShouldBeEqualTo(_newDimension);
      }
   }

   internal class When_converting_an_object_with_dimension_that_should_not_be_converted : concern_for_UsingDimensionConverter
   {
      private IDimension _dimension;

      protected override void Context()
      {
         base.Context();
         _dimension = A.Fake<IDimension>();
         _withDimension.Dimension = _dimension;
         A.CallTo(() => _dimension.Name).Returns("tralala");
      }

      [Observation]
      public void should_change_the_dimension_to_the_new_one()
      {
         _withDimension.Dimension.ShouldBeEqualTo(_dimension);
      }
   }
}	