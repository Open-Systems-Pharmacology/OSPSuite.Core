using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;

namespace OSPSuite.Presentation.Extensions
{
   public abstract class concern_for_DataColumnDisplayExtensions : StaticContextSpecification
   {
      protected BaseGrid _baseGrid;
      protected DataColumn _column;

      protected override void Context()
      {
         _baseGrid = new BaseGrid("Time", Constants.Dimension.NO_DIMENSION);
         _column = new DataColumn("Error", Constants.Dimension.NO_DIMENSION, _baseGrid);
      }
   }

   public class When_retrieving_the_name_of_an_arithmetic_error_column : concern_for_DataColumnDisplayExtensions
   {
      protected override void Context()
      {
         base.Context();
         _column.DataInfo = new DataInfo(ColumnOrigins.ObservationAuxiliary) { AuxiliaryType = AuxiliaryType.ArithmeticStdDev };
      }

      [Observation]
      public void should_return_the_name_followed_by_the_arithmetic_error_type()
      {
         _column.NameWithErrorType().ShouldBeEqualTo("Error (Arithmetic Standard Deviation)");
      }
   }

   public class When_retrieving_the_name_of_a_geometric_error_column : concern_for_DataColumnDisplayExtensions
   {
      protected override void Context()
      {
         base.Context();
         _column.DataInfo = new DataInfo(ColumnOrigins.ObservationAuxiliary) { AuxiliaryType = AuxiliaryType.GeometricStdDev };
      }

      [Observation]
      public void should_return_the_name_followed_by_the_geometric_error_type()
      {
         _column.NameWithErrorType().ShouldBeEqualTo("Error (Geometric Standard Deviation)");
      }
   }

   public class When_retrieving_the_name_of_a_column_that_is_not_an_error : concern_for_DataColumnDisplayExtensions
   {
      protected override void Context()
      {
         base.Context();
         _column.DataInfo = new DataInfo(ColumnOrigins.Observation);
      }

      [Observation]
      public void should_return_the_name_of_an_observation_column()
      {
         _column.NameWithErrorType().ShouldBeEqualTo("Error");
      }

      [Observation]
      public void should_return_the_name_of_the_base_grid()
      {
         _baseGrid.NameWithErrorType().ShouldBeEqualTo("Time");
      }
   }

   public class When_retrieving_the_name_of_a_population_mean_column : concern_for_DataColumnDisplayExtensions
   {
      protected override void Context()
      {
         base.Context();
         _column.DataInfo = new DataInfo(ColumnOrigins.CalculationAuxiliary) { AuxiliaryType = AuxiliaryType.ArithmeticMeanPop };
      }

      [Observation]
      public void should_return_the_name_only()
      {
         _column.NameWithErrorType().ShouldBeEqualTo("Error");
      }
   }
}
