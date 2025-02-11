using System;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.PKAnalyses;

namespace OSPSuite.R.Services
{
   public abstract class concern_for_PKParameterTask : ContextForIntegration<IPKParameterTask>
   {
      public override void GlobalContext()
      {
         base.GlobalContext();
         sut = Api.GetPKParameterTask();
      }
   }

   public class When_returning_a_pk_parameter_by_name : concern_for_PKParameterTask
   {
      [Observation]
      public void should_return_the_pk_parameter_if_defined()
      {
         sut.PKParameterByName("t_max").ShouldNotBeNull();
      }

      [Observation]
      public void should_return_null_otherwise()
      {
         sut.PKParameterByName("does_not_exist").ShouldBeNull();
      }
   }

   public class When_adding_a_user_defined_pk_parameter_that_is_named_like_a_predefined_pk_parameter : concern_for_PKParameterTask
   {
      [Observation]
      public void should_throw_an_exception()
      {
         The.Action(() => sut.AddUserDefinedPKParameter(new UserDefinedPKParameter {Name = "t_max"})).ShouldThrowAn<ArgumentException>();
      }
   }

   public class When_adding_a_user_defined_pk_parameter_that_already_exists_as_user_defined : concern_for_PKParameterTask
   {
      private UserDefinedPKParameter _userDefinedPKParameter;

      protected override void Context()
      {
         base.Context();
         _userDefinedPKParameter = new UserDefinedPKParameter {Name = "MyParam"};
         sut.AddUserDefinedPKParameter(_userDefinedPKParameter);
      }

      [Observation]
      public void should_be_able_to_add_another_instance_with_the_same_name()
      {
         sut.AddUserDefinedPKParameter(_userDefinedPKParameter);
      }

      [Observation]
      public void the_user_defined_pk_parameters_should_have_the_mode_set_to_always()
      {
         _userDefinedPKParameter.Mode.ShouldBeEqualTo(PKParameterMode.Always);
      }
   }

   public class When_removing_a_user_defined_pk_parameter : concern_for_PKParameterTask
   {
      private UserDefinedPKParameter _userDefinedPKParameter;

      protected override void Context()
      {
         base.Context();
         _userDefinedPKParameter = new UserDefinedPKParameter {Name = "MyParam"};
         sut.AddUserDefinedPKParameter(_userDefinedPKParameter);
      }

      protected override void Because()
      {
         sut.RemoveUserDefinedPKParameter(_userDefinedPKParameter);
      }

      [Observation]
      public void should_have_removed_the_user_defined_pk_parameter()
      {
         sut.UserDefinedPKParameterByName(_userDefinedPKParameter.Name).ShouldBeNull();
      }
   }

   public class
      When_creating_a_user_defined_pk_parameter_using_a_unit_that_is_not_a_unit_of_the_base_parameter_yet_exists_in_our_system :
         concern_for_PKParameterTask
   {
      private UserDefinedPKParameter _userDefinedParameter;

      protected override void Because()
      {
         _userDefinedParameter = sut.CreateUserDefinedPKParameter("Test", StandardPKParameter.C_trough, "Test", "%");
      }

      [Observation]
      public void should_return_the_dimension_of_the_unit()
      {
         _userDefinedParameter.Dimension.Name.ShouldBeEqualTo(Constants.Dimension.FRACTION);
      }

      [Observation]
      public void should_return_the_expected_base_unit()
      {
         _userDefinedParameter.BaseUnit.ShouldBeEqualTo("");
      }

      public override void GlobalCleanup()
      {
         base.GlobalCleanup();
         sut.RemoveAllUserDefinedPKParameters();
      }
   }

   public class
      When_creating_a_user_defined_pk_parameter_using_a_unit_that_is_not_a_unit_of_the_base_parameter_and_does_not_exist_in_our_system :
         concern_for_PKParameterTask
   {
      private UserDefinedPKParameter _userDefinedParameter;

      protected override void Because()
      {
         _userDefinedParameter = sut.CreateUserDefinedPKParameter("Test", StandardPKParameter.C_trough, "Test", "not_there");
      }

      [Observation]
      public void should_return_a_user_defined_dimension_with_the_name_of_the_parameter()
      {
         _userDefinedParameter.Dimension.Name.ShouldBeEqualTo(_userDefinedParameter.Name);
      }

      [Observation]
      public void should_return_the_expected_base_unit_equal_to_the_display_unit()
      {
         _userDefinedParameter.BaseUnit.ShouldBeEqualTo("not_there");
      }

      public override void GlobalCleanup()
      {
         base.GlobalCleanup();
         sut.RemoveAllUserDefinedPKParameters();
      }
   }

   public class When_removing_all_user_defined_pk_parameters : concern_for_PKParameterTask
   {
      protected override void Context()
      {
         base.Context();
         sut.AddUserDefinedPKParameter(new UserDefinedPKParameter {Name = "MyParam"});
         sut.AddUserDefinedPKParameter(new UserDefinedPKParameter {Name = "MyParam2"});
      }

      protected override void Because()
      {
         sut.RemoveAllUserDefinedPKParameters();
      }

      [Observation]
      public void should_have_removed_the_user_defined_pk_parameter()
      {
         sut.UserDefinedPKParameterByName("MyParam").ShouldBeNull();
         sut.UserDefinedPKParameterByName("MyParam2").ShouldBeNull();
      }
   }

   public class When_returning_all_pk_parameter_names_defined_in_the_system : concern_for_PKParameterTask
   {
      private UserDefinedPKParameter _userDefinedPKParameter;

      protected override void Context()
      {
         base.Context();
         _userDefinedPKParameter = new UserDefinedPKParameter {Name = "AnotherParam"};
      }

      [Observation]
      public void should_return_all_predefined_pk_and_all_user_defined_pk_parameters()
      {
         var predefinedPKParameters = sut.AllPKParameterNames().Length;
         sut.AddUserDefinedPKParameter(_userDefinedPKParameter);

         sut.AllPKParameterNames().Length.ShouldBeEqualTo(predefinedPKParameters + 1);
      }

      public override void GlobalCleanup()
      {
         base.GlobalCleanup();
         sut.RemoveAllUserDefinedPKParameters();
      }
   }
}