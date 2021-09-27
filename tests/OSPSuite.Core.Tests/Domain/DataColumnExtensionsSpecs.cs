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

   public class When_checking_if_a_column_without_data_info_is_of_a_given_origin : StaticContextSpecification
   {
      [Observation]
      public void should_return_false()
      {
         var column = new DataColumn {DataInfo = null};
         column.IsCalculation().ShouldBeFalse();
         column.IsObservation().ShouldBeFalse();
         column.IsCalculationAuxiliary().ShouldBeFalse();
      }
   }

   public class When_checking_if_a_column_is_observed_data : StaticContextSpecification
   {
      private DataColumn _obsDataColumn;
      private DataColumn _otherColumn;

      protected override void Context()
      {
         base.Context();
         _obsDataColumn = new DataColumn {DataInfo = new DataInfo(ColumnOrigins.Observation)};
         _otherColumn = new DataColumn {DataInfo = new DataInfo(ColumnOrigins.Calculation)};
      }

      [Observation]
      public void should_return_true_if_the_column_is_observed_data()
      {
         _obsDataColumn.IsObservation().ShouldBeTrue();
         _obsDataColumn.IsCalculation().ShouldBeFalse();
         _obsDataColumn.IsCalculationAuxiliary().ShouldBeFalse();
      }

      [Observation]
      public void should_return_false_otherwise()
      {
         _otherColumn.IsObservation().ShouldBeFalse();
      }
   }

   public class When_checking_if_a_column_is_calculation : StaticContextSpecification
   {
      private DataColumn _calculationColumn;
      private DataColumn _otherColumn;

      protected override void Context()
      {
         base.Context();
         _calculationColumn = new DataColumn { DataInfo = new DataInfo(ColumnOrigins.Calculation) };
         _otherColumn = new DataColumn { DataInfo = new DataInfo(ColumnOrigins.Observation) };
      }

      [Observation]
      public void should_return_true_if_the_column_is_calculation()
      {
         _calculationColumn.IsCalculation().ShouldBeTrue();
         _calculationColumn.IsObservation().ShouldBeFalse();
         _calculationColumn.IsCalculationAuxiliary().ShouldBeFalse();
      }

      [Observation]
      public void should_return_false_otherwise()
      {
         _otherColumn.IsCalculation().ShouldBeFalse();
      }
   }

   public class When_checking_if_a_column_is_calculation_auxiliary : StaticContextSpecification
   {
      private DataColumn _calculationAuxiliaryColumn;
      private DataColumn _otherColumn;

      protected override void Context()
      {
         base.Context();
         _calculationAuxiliaryColumn = new DataColumn { DataInfo = new DataInfo(ColumnOrigins.CalculationAuxiliary) };
         _otherColumn = new DataColumn { DataInfo = new DataInfo(ColumnOrigins.Observation) };
      }

      [Observation]
      public void should_return_true_if_the_column_is_calculation()
      {
         _calculationAuxiliaryColumn.IsCalculationAuxiliary().ShouldBeTrue();
         _calculationAuxiliaryColumn.IsObservation().ShouldBeFalse();
         _calculationAuxiliaryColumn.IsCalculation().ShouldBeFalse();
      }

      [Observation]
      public void should_return_false_otherwise()
      {
         _otherColumn.IsCalculationAuxiliary().ShouldBeFalse();
      }
   }
}