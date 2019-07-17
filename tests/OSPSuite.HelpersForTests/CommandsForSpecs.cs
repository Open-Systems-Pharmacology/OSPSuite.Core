using OSPSuite.Core.Commands.Core;

namespace OSPSuite.Helpers
{
   public class MyContext
   {
   }

   public class MyReversibleCommand : Command, IReversibleCommand<MyContext>
   {
      public MyReversibleCommand()
      {
      }

      public MyReversibleCommand(CommandId id) : base(id)
      {
      }

      public void Execute(MyContext context)
      {
      }

      public void RestoreExecutionData(MyContext context)
      {
      }

      public IReversibleCommand<MyContext> InverseCommand(MyContext context)
      {
         return new MyReversibleCommand().AsInverseFor(this);
      }
   }

   public class MySimpleCommand : Command, ICommand<MyContext>
   {
      public void Execute(MyContext context)
      {
      }

      public void RestoreExecutionData(MyContext context)
      {
      }
   }
}