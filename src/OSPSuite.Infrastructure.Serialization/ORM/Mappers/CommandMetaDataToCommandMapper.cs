using OSPSuite.Core.Commands;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Infrastructure.Serialization.ORM.MetaData;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Infrastructure.Serialization.ORM.Mappers
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

      private IOSPSuiteCommand<IOSPSuiteExecutionContext> commandFrom(string descriminator)
      {
         //This commands are create from the command manager and needs to be cast to pksim types afterwards
         if (string.Equals(descriminator, Constants.Serialization.MACRO_COMMAND))
            return new OSPSuiteMacroCommand<IOSPSuiteExecutionContext>();

         if (string.Equals(descriminator, Constants.Serialization.LABEL_COMMAND))
            return new OSPSuiteLabelCommand();

         if (string.Equals(descriminator, Constants.Serialization.INFO_COMMAND))
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