using System;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
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
         _userDefinedPKParameter = new UserDefinedPKParameter {Name = "MyParam"};
      }

      [Observation]
      public void should_return_all_predefined_pk_and_all_user_defined_pk_parameters()
      {
         sut.AllPKParameterNames().Length.ShouldBeEqualTo(31);

         sut.AddUserDefinedPKParameter(_userDefinedPKParameter);

         sut.AllPKParameterNames().Length.ShouldBeEqualTo(32);
      }
   }
}