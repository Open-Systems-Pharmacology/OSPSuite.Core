using OSPSuite.Assets;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Infrastructure.ORM.MetaData;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Infrastructure.ORM.Mappers
{
   public interface ICommandMetaDataToCommandMapper : IMapper<CommandMetaData, ICommand>
   {
   }

   public class CommandMetaDataToCommandMapper : ICommandMetaDataToCommandMapper
   {
      private readonly ICommandMetaDataRepository _commandMetaDataRepository;

      public CommandMetaDataToCommandMapper(ICommandMetaDataRepository commandMetaDataRepository)
      {
         _commandMetaDataRepository = commandMetaDataRepository;
      }

      public ICommand MapFrom(CommandMetaData commandMetaData)
      {
         var command = commandFrom(commandMetaData);
         command.CommandType = commandMetaData.CommandType;
         command.Id = new CommandId(commandMetaData.CommandId, commandMetaData.CommandInverseId);
         command.InternalId = commandMetaData.Id;
         command.Comment = commandMetaData.Comment;
         command.Description = commandMetaData.Description;
         command.ObjectType = commandMetaData.ObjectType;
         command.CommandType = commandMetaData.CommandType;
         command.ExtendedDescription = commandMetaData.ExtendedDescription;
         command.Visible = commandMetaData.Visible;
         command.Loaded = false;
         commandMetaData.Properties.Values.Each(p => command.AddExtendedProperty(p.Name, p.Value));
         return command;
      }

      private IOSPSuiteCommmand<IOSPSuiteExecutionContext> commandFrom(string descrimiator)
      {
         //This commands are create from the command manager and needs to be casted to pksim types afterwards
         if (string.Equals(descrimiator, SerializationConstants.MacroCommand))
            return new OSPSuiteMacroCommand<IOSPSuiteExecutionContext>();

         if (string.Equals(descrimiator, SerializationConstants.LabelCommand))
            return new OSPSuiteLabelCommand();

         if (string.Equals(descrimiator, SerializationConstants.InfoCommand))
            return new OSPSuiteInfoCommand();

         return new ReadOnlyCommand();
      }

      private ICommand commandFrom(CommandMetaData commandMetaData)
      {
         var command = commandFrom(commandMetaData.Discriminator);

         var macroCommand = command as OSPSuiteMacroCommand<IOSPSuiteExecutionContext>;
         if (macroCommand == null) return command;

         foreach (var childMetaData in _commandMetaDataRepository.AllChildrenOf(commandMetaData))
         {
            macroCommand.Add(MapFrom(childMetaData));
         }

         return macroCommand;
      }
   }
}