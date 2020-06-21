using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.UICommands;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_ObjectUICommand : ContextSpecification<ObjectUICommand<IParameter>>
   {
      protected override void Context()
      {
         sut = new ParameterUICommand();
      }
   }

   public class When_retrieving_the_subject_of_a_UI_command_that_has_not_been_set : concern_for_ObjectUICommand
   {
      [Observation]
      public void should_return_null()
      {
         sut.Subject.ShouldBeNull();
      }
   }

   public class When_retrieving_the_subject_of_a_UI_command_that_was_set : concern_for_ObjectUICommand
   {
      private IParameter _parameter;

      protected override void Context()
      {
         base.Context();
         _parameter= A.Fake<IParameter>();   
         sut.Subject = _parameter;
      }
      [Observation]
      public void should_return_the_subject()
      {
         sut.Subject.ShouldBeEqualTo(_parameter);
      }
   }
   class ParameterUICommand : ObjectUICommand<IParameter>
   {
      protected override void PerformExecute()
      {
         //do nothing
      }
   }
}	  