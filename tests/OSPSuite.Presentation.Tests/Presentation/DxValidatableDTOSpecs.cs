using DevExpress.XtraEditors.DXErrorProvider;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Presentation.DTO;
using OSPSuite.Utility.Validation;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_DxValidatableDTO : ContextSpecification<DummyDxValidatableDTO>
   {
      protected override void Context()
      {
         sut = new DummyDxValidatableDTO();
      }

      public class When_validating_an_invalid_dto_implementing_the_devexpress_error_interface : concern_for_DxValidatableDTO
      {
         private ErrorInfo _errorInfo;

         protected override void Context()
         {
            base.Context();
            _errorInfo = new ErrorInfo();
         }

         protected override void Because()
         {
            sut.GetError(_errorInfo);
         }

         [Observation]
         public void should_fill_up_the_error_info_with_the_error_message_of_the_validation()
         {
            _errorInfo.ErrorText.ShouldBeEqualTo("Name is required");
         }

         [Observation]
         public void should_fill_up_the_error_type_with_standard_critical_error_icon()
         {
            _errorInfo.ErrorType.ShouldBeEqualTo(ErrorType.Critical);
         }
      }

      public class When_validating_the_property_of_an_invalid_dto_implementing_the_devexpress_error_interface : concern_for_DxValidatableDTO
      {
         private ErrorInfo _errorInfo;

         protected override void Context()
         {
            base.Context();
            _errorInfo = new ErrorInfo();
         }

         protected override void Because()
         {
            sut.GetPropertyError("Name", _errorInfo);
         }

         [Observation]
         public void should_fill_up_the_error_info_with_the_error_message_of_the_validation()
         {
            _errorInfo.ErrorText.ShouldBeEqualTo("Name is required");
         }

         [Observation]
         public void should_fill_up_the_error_type_with_standard_critical_error_icon()
         {
            _errorInfo.ErrorType.ShouldBeEqualTo(ErrorType.Critical);
         }
      }
   }

   public class When_validating_a_valid_dto_implementing_the_devexpress_error_interface : concern_for_DxValidatableDTO
   {
      private ErrorInfo _errorInfo;

      protected override void Context()
      {
         base.Context();
         sut.Name = "toto";
         _errorInfo = new ErrorInfo();
      }

      protected override void Because()
      {
         sut.GetError(_errorInfo);
      }

      [Observation]
      public void should_return_an_error_info_without_any_error()
      {
         _errorInfo.ErrorType.ShouldBeEqualTo(ErrorType.Default);
      }
   }

   public class When_validating_the_property_of_a_valid_dto_implementing_the_devexpress_error_interface : concern_for_DxValidatableDTO
   {
      private ErrorInfo _errorInfo;

      protected override void Context()
      {
         base.Context();
         sut.Name = "toto";
         _errorInfo = new ErrorInfo();
      }

      protected override void Because()
      {
         sut.GetPropertyError("Name", _errorInfo);
      }

      [Observation]
      public void should_return_an_error_info_without_any_error()
      {
         _errorInfo.ErrorType.ShouldBeEqualTo(ErrorType.Default);
      }
   }

   public class When_validating_a_property_that_does_not_exist_of_a_dto_implementing_the_devexpress_error_interface : concern_for_DxValidatableDTO
   {
      private ErrorInfo _errorInfo;

      protected override void Context()
      {
         base.Context();
         sut.Name = "toto";
         _errorInfo = new ErrorInfo();
      }

      protected override void Because()
      {
         sut.GetPropertyError("asdasd", _errorInfo);
      }

      [Observation]
      public void should_not_crash()
      {
         _errorInfo.ErrorType.ShouldBeEqualTo(ErrorType.Default);
      }
   }

   public class DummyDxValidatableDTO : DxValidatableDTO
   {
      public string Name { get; set; }

      public DummyDxValidatableDTO()
      {
         Rules.Add(NameNotEmpty);
      }

      public static IBusinessRule NameNotEmpty
      {
         get
         {
            return CreateRule.For<DummyDxValidatableDTO>()
               .Property(item => item.Name)
               .WithRule((e, v) => !string.IsNullOrEmpty(v))
               .WithError("Name is required");
         }
      }
   }
}