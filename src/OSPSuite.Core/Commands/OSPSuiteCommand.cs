using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Commands
{
   public interface IOSPSuiteCommand : ICommand
   {
      /// <summary>
      ///    Type of building block for which the command was performed (or type of parent building block)
      /// </summary>
      string BuildingBlockType { get; set; }

      /// <summary>
      ///    Name of building block for which the command was performed (or name of parent building block)
      /// </summary>
      string BuildingBlockName { get; set; }
   }

   public interface IOSPSuiteCommmand<in TContext> : IOSPSuiteCommand, ICommand<TContext> where TContext : IOSPSuiteExecutionContext
   {

   }

   public abstract class OSPSuiteCommand<TContext> : Command, IOSPSuiteCommmand<TContext> where TContext : IOSPSuiteExecutionContext
   {
      public void Execute(TContext context)
      {
         ExecuteWith(context);
         ClearReferences();
         context.ProjectChanged();
      }

      public string BuildingBlockType
      {
         get { return ExtendedPropertyValueFor(Constants.Command.BUILDING_BLOCK_TYPE); }
         set { AddExtendedProperty(Constants.Command.BUILDING_BLOCK_TYPE, value); }
      }

      public string BuildingBlockName
      {
         get { return ExtendedPropertyValueFor(Constants.Command.BUILDING_BLOCK_NAME); }
         set { AddExtendedProperty(Constants.Command.BUILDING_BLOCK_NAME, value); }
      }

      protected abstract void ExecuteWith(TContext context);
      protected abstract void ClearReferences();
   }

   public abstract class OSPSuiteReversibleCommand<TContext> : OSPSuiteCommand<TContext>, IReversibleCommand<TContext> where TContext : IOSPSuiteExecutionContext
   {
      public IReversibleCommand<TContext> InverseCommand(TContext context)
      {
         var inverse = GetInverseCommand(context);
         inverse.Visible = Visible;
         ClearReferences();
         return inverse;
      }

      protected abstract IReversibleCommand<TContext> GetInverseCommand(TContext context);
      public abstract void RestoreExecutionData(TContext context);
   }

   public class OSPSuiteMacroCommand<TContext> : MacroCommand<TContext>, IOSPSuiteCommmand<TContext> where TContext : IOSPSuiteExecutionContext
   {
      public OSPSuiteMacroCommand()
      {
      }

      public OSPSuiteMacroCommand(IEnumerable<ICommand<TContext>> subCommands)
         : this()
      {
         subCommands.Each(Add);
      }

      public string BuildingBlockType
      {
         get { return ExtendedPropertyValueFor(Constants.Command.BUILDING_BLOCK_TYPE); }
         set { AddExtendedProperty(Constants.Command.BUILDING_BLOCK_TYPE, value); }
      }

      public string BuildingBlockName
      {
         get { return ExtendedPropertyValueFor(Constants.Command.BUILDING_BLOCK_NAME); }
         set { AddExtendedProperty(Constants.Command.BUILDING_BLOCK_NAME, value); }
      }

      public new IEnumerable<IOSPSuiteCommmand<TContext>> All()
      {
         return base.All().Select(command => command.DowncastTo<IOSPSuiteCommmand<TContext>>());
      }
   }

   public class OSPSuiteEmptyCommand<TContext> : EmptyCommand<TContext>, IOSPSuiteCommmand<TContext> where TContext : IOSPSuiteExecutionContext
   {
      public string BuildingBlockType { get; set; }
      public string BuildingBlockName { get; set; }
   }
}