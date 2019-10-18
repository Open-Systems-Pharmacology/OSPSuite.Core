using DevExpress.XtraEditors.DXErrorProvider;
using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.DTO.ParameterIdentifications;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_OptimizedParameterDTO : ContextSpecification<OptimizedParameterDTO>
   {
      protected override void Context()
      {
         sut = new OptimizedParameterDTO
         {
            MinValue = new ValueDTO {Value = 10},
            MaxValue = new ValueDTO {Value = 20},
            OptimalValue = new ValueDTO()
         };
      }
   }

   public class When_retrieving_the_image_status_defined_for_an_optimized_parameter_dto : concern_for_OptimizedParameterDTO
   {
      [Observation]
      public void should_return_the_ok_image_if_the_value_is_not_close_to_the_boundaries()
      {
         sut.OptimalValue.Value = 15;
         sut.BoundaryCheckIcon.ShouldBeEqualTo(ApplicationIcons.OK);
      }

      [Observation]
      public void should_return_the_warning_image_if_the_value_is_not_close_to_the_boundaries()
      {
         sut.OptimalValue.Value = 10.05;
         sut.BoundaryCheckIcon.ShouldBeEqualTo(ApplicationIcons.Warning);
      }
   }

   public class When_retrieving_the_error_info_defined_for_an_optimized_parameter_dto : concern_for_OptimizedParameterDTO
   {
      private ErrorInfo _errorInfo;

      protected override void Context()
      {
         base.Context();
         _errorInfo = new ErrorInfo();
      }

      [Observation]
      public void should_return_the_ok_image_if_the_value_is_not_close_to_the_boundaries()
      {
         sut.OptimalValue.Value = 15;
         sut.GetError(_errorInfo);
         _errorInfo.ErrorText.ShouldBeEmpty();
      }

      [Observation]
      public void should_return_the_warning_image_if_the_value_is_not_close_to_the_boundaries()
      {
         sut.OptimalValue.Value = 10.05;
         sut.GetError(_errorInfo);
         _errorInfo.ErrorText.ShouldBeEqualTo(Warning.OptimizedValueIsCloseToBoundary);
      }
   }
}