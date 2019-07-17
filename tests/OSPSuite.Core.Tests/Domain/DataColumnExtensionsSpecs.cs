using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Data;

namespace OSPSuite.Core.Domain
{
   public class When_checking_if_a_column_is_a_base_grid_column : StaticContextSpecification
   {
      private DataColumn _oneBaseGridColumn;
      private DataColumn _oneNotBaseGridColumn;

      protected override void Context()
      {
         _oneBaseGridColumn = A.Fake<BaseGrid>();
         _oneNotBaseGridColumn = A.Fake<DataColumn>();
      }

      [Observation]
      public void should_return_true_for_a_base_grid_column()
      {
         _oneBaseGridColumn.IsBaseGrid().ShouldBeTrue();
      }

      [Observation]
      public void should_return_false_if_the_column_is_not_a_base_grid_column()
      {
         _oneNotBaseGridColumn.IsBaseGrid().ShouldBeFalse();
      }
   }
}