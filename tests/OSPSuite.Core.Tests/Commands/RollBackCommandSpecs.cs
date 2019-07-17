using System.Collections.Generic;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Commands
{
   public abstract class concern_for_roll_back_command : ContextSpecification<IMacroCommand<MyContext>>
   {
      protected IList<IReversibleCommand<MyContext>> _commandList;
      protected int _state;
      protected IReversibleCommand<MyContext> _command1;
      protected IReversibleCommand<MyContext> _command2;
      protected MyContext _context;
      protected IReversibleCommand<MyContext> _inverseCommand1;
      protected IReversibleCommand<MyContext> _inverseCommand2;

      protected override void Context()
      {
         _state = 2;
         _inverseCommand1 = A.Fake<IReversibleCommand<MyContext>>();
         _inverseCommand2 = A.Fake<IReversibleCommand<MyContext>>();
         _command1 = A.Fake<IReversibleCommand<MyContext>>();
         _command2 = A.Fake<IReversibleCommand<MyContext>>();
         _commandList = new List<IReversibleCommand<MyContext>> {_command1, _command2};
         sut = new RollBackCommand<MyContext>(_state, _commandList);
         _context = new MyContext();
         A.CallTo(() => _command1.InverseCommand(_context)).Returns(_inverseCommand1);
         A.CallTo(() => _command2.InverseCommand(_context)).Returns(_inverseCommand2);
      }
   }

   public class When_executing_a_roll_back_command : concern_for_roll_back_command
   {
      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_restore_the_execution_context_for_each_command()
      {
         A.CallTo(() => _command1.RestoreExecutionData(_context)).MustHaveHappened();
         A.CallTo(() => _inverseCommand1.Execute(_context)).MustHaveHappened();
         A.CallTo(() => _command2.RestoreExecutionData(_context)).MustHaveHappened();
         A.CallTo(() => _command2.RestoreExecutionData(_context)).MustHaveHappened();
         A.CallTo(() => _inverseCommand2.Execute(_context)).MustHaveHappened();
      }
   }
}