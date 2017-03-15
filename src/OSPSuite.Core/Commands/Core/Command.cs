using System.Collections.Generic;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Core.Commands.Core
{
   public interface ICommand
   {
      /// <summary>
      ///   Internal id used to idenfity a command uniquely. This is not the id of the command, since two commands could have the same Id (inverse of inverse)
      /// </summary>
      string InternalId { get; set; }

      /// <summary>
      ///   Id of the command identifing the action being performed
      /// </summary>
      CommandId Id { get; set; }

      /// <summary>
      ///   Type of command
      /// </summary>
      string CommandType { get; set; }

      /// <summary>
      ///   Type of object on which the command is being applied
      /// </summary>
      string ObjectType { get; set; }

      /// <summary>
      ///   Short description of the command identifying the action (visible in the browser)
      /// </summary>
      string Description { get; set; }

      /// <summary>
      ///   Comment linked to the command (editable)
      /// </summary>
      string Comment { get; set; }

      /// <summary>
      ///   Return true if the current command is an inverse command for the command given parameter, otherwise false
      /// </summary>
      bool IsInverseFor(ICommand command);

      /// <summary>
      ///   Return true if the command should be diplayed in the browser, otherwise false
      /// </summary>
      bool Visible { get; set; }

      /// <summary>
      ///   Longer description of the command, that will be only displayed when requested.
      /// </summary>
      string ExtendedDescription { get; set; }

      /// <summary>
      ///   Returns a the value defined for the extend properties named <paramref name="propertyName" /> if defined otherwise an empty string
      /// </summary>
      string ExtendedPropertyValueFor(string propertyName);

      /// <summary>
      ///   Adds or replace the property defined by <paramref name="propertyName" /> and the value <paramref name="propertyValue" />
      /// </summary>
      /// <param name="propertyName"> Name of the property to add or replace </param>
      /// <param name="propertyValue"> Value of the property </param>
      void AddExtendedProperty(string propertyName, string propertyValue);

      /// <summary>
      ///   Returns the list of all extended properties available in the given command
      /// </summary>
      IEnumerable<string> AllExtendedProperties { get; }

      /// <summary>
      ///  Returns true if the command content is loaded. If the value is false, the command cannot be used in a rollback
      ///  Default is true.
      /// </summary>
      bool Loaded { get; set; }
   }

   public interface ICommand<in TExecutionContext> : ICommand
   {
      /// <summary>
      ///   Execute the command with the given <typeparamref name="TExecutionContext" />
      /// </summary>
      /// <param name="context"> context with which the command should be executed </param>
      void Execute(TExecutionContext context);
   }

   public interface IReversibleCommand<in TExecutionContext> : ICommand<TExecutionContext>
   {
      /// <summary>
      ///   Restore the data from the context used to run the command (this is typically called before creating an inverse command)
      /// </summary>
      void RestoreExecutionData(TExecutionContext context);

      /// <summary>
      ///   Create an inverse for the current command
      /// </summary>
      IReversibleCommand<TExecutionContext> InverseCommand(TExecutionContext context);
   }

   public abstract class Command : ICommand
   {
      private readonly ICache<string, string> _extendedProperties;
      public virtual bool Loaded { get; set; }
      public CommandId Id { get; set; }
      public string InternalId { get; set; }
      public string Description { get; set; }
      public string Comment { get; set; }
      public string ObjectType { get; set; }
      public string CommandType { get; set; }
      public bool Visible { get; set; }
      public string ExtendedDescription { get; set; }

      protected Command() : this(new CommandId())
      {
      }

      protected Command(CommandId id)
      {
         Id = id;
         //default: All commands should be visible
         Visible = true;
         _extendedProperties = new Cache<string, string>();

         //default: Commands are loaded
         Loaded = true;
      }

      public string ExtendedPropertyValueFor(string propertyName)
      {
         if (_extendedProperties.Contains(propertyName))
            return _extendedProperties[propertyName];
         return string.Empty;
      }

      public void AddExtendedProperty(string propertyName, string propertyValue)
      {
         _extendedProperties[propertyName] = propertyValue;
      }

      public IEnumerable<string> AllExtendedProperties
      {
         get { return _extendedProperties.Keys; }
      }

      public bool IsInverseFor(ICommand command)
      {
         return command != null && (Id.IsInverseFor(command.Id));
      }
   }


}