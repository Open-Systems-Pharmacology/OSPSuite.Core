using System;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Utility;

namespace OSPSuite.Core.Commands
{
   public abstract class concern_for_history_item_factory : ContextSpecification<IHistoryItemFactory>
   {
      protected override void Context()
      {
         sut = new HistoryItemFactory();
      }
   }

   public class When_creating_a_new_history_item_for_a_command : concern_for_history_item_factory
   {
      private ICommand _command;
      private DateTime _dateTime;
      private string _currentUser;
      private IHistoryItem result;

      protected override void Context()
      {
         base.Context();
         _dateTime = new DateTime(2000, 1, 1);
         _currentUser = "Michael";
         SystemTime.Now = () => _dateTime;
         EnvironmentHelper.UserName = () => _currentUser;
         _command = A.Fake<ICommand>();
      }

      protected override void Because()
      {
         result = sut.CreateFor(_command);
      }

      [Observation]
      public void should_return_a_new_history_item_for_the_current_date_and_current_user()
      {
         result.User.ShouldBeEqualTo(_currentUser);
         result.DateTime.ShouldBeEqualTo(_dateTime);
         result.Command.ShouldBeEqualTo(_command);
      }

      [Observation]
      public void should_return_an_history_item_with_an_id_set_to_a_non_empty_value()
      {
         result.Id.ShouldNotBeNull();
      }
   }
}