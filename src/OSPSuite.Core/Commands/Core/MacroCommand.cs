using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Commands.Core
{
   public interface IMacroCommand : ICommand, ICommandCollector
   {
      int Count { get; }
      bool IsEmtpy { get; }
      void Clear();
      void Add(ICommand command);
      void AddRange(IEnumerable<ICommand> commandsToAdd);
   }

   public interface IMacroCommand<in TExecutionContext> : IReversibleCommand<TExecutionContext>, IMacroCommand
   {
   }

   public class MacroCommand<TExecutionContext> : Command, IMacroCommand<TExecutionContext>
   {
      protected readonly IList<ICommand<TExecutionContext>> _subCommands;

      public MacroCommand() : this(new List<ICommand<TExecutionContext>>())
      {
      }

      public MacroCommand(IEnumerable<ICommand<TExecutionContext>> subCommands) : this(subCommands.ToList())
      {
      }

      public MacroCommand(IList<ICommand<TExecutionContext>> subCommands)
      {
         _subCommands = subCommands;
      }

      public void Add(ICommand commandToAdd)
      {
         if (commandToAdd.IsEmpty()) return;
         if (commandToAdd.IsEmptyMacro()) return;
         _subCommands.Add(commandToAdd.DowncastTo<ICommand<TExecutionContext>>());
      }

      public void AddRange(IEnumerable<ICommand> commandsToAdd)
      {
         commandsToAdd.Each(Add);
      }

      public void AddCommand(ICommand command)
      {
         Add(command);
      }

      public IEnumerable<ICommand> All()
      {
         return _subCommands;
      }

      public void Clear()
      {
         _subCommands.Clear();
      }

      public int Count
      {
         get { return _subCommands.Count; }
      }

      public bool IsEmtpy
      {
         get { return Count <= 0; }
      }

      public virtual void Execute(TExecutionContext context)
      {
         _subCommands.Each(command => command.Execute(context));
      }

      public void RestoreExecutionData(TExecutionContext context)
      {
         foreach (var subCommand in _subCommands)
         {
            var reverseCommand = subCommand as IReversibleCommand<TExecutionContext>;
            if (reverseCommand == null)
               throw new CreateInverseCommandException();

            reverseCommand.RestoreExecutionData(context);
         }
      }

      public IReversibleCommand<TExecutionContext> InverseCommand(TExecutionContext context)
      {
         var undoableCommand = new MacroCommand<TExecutionContext>().AsInverseFor(this);
         for (int i = _subCommands.Count - 1; i >= 0; i--)
         {
            var reverseCommand = _subCommands[i] as IReversibleCommand<TExecutionContext>;
            if (reverseCommand == null)
               throw new CreateInverseCommandException();

            undoableCommand.Add(reverseCommand.InverseCommand(context));
         }

         return undoableCommand;
      }
   }
}