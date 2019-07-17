using System;
using OSPSuite.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Infrastructure.ORM.MetaData;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Infrastructure.ORM.Mappers
{
   public interface ICommandToCommandMetaDataMapper : IMapper<ICommand, CommandMetaData>
   {
   }

   public class CommandToCommandMetaDataMapper : ICommandToCommandMetaDataMapper
   {
      public CommandMetaData MapFrom(ICommand command)
      {
         var commandMetaData = metaDataCommandFrom(command);

         if (string.IsNullOrEmpty(command.InternalId))
            command.InternalId = Guid.NewGuid().ToString();

         commandMetaData.Id = command.InternalId;
         commandMetaData.CommandId = command.Id.Id;
         commandMetaData.CommandInverseId = command.Id.InverseId;
         commandMetaData.Comment = command.Comment;
         commandMetaData.Description = command.Description;
         commandMetaData.ObjectType = command.ObjectType;
         commandMetaData.CommandType = command.CommandType;
         commandMetaData.ExtendedDescription = command.ExtendedDescription;
         commandMetaData.Visible = command.Visible;

         command.AllExtendedProperties.Each(ep => commandMetaData.AddProperty(new CommandPropertyMetaData { Name = ep, Value = command.ExtendedPropertyValueFor(ep) }));

         return commandMetaData;
      }

      private CommandMetaData metaDataCommandFrom(ICommand command)
      {
         var macroCommand = command as IMacroCommand;
         if (macroCommand != null)
            return metaDataCommandFrom(macroCommand);

         return new CommandMetaData { Discriminator = descriminatorFor(command) };
      }

      private string descriminatorFor(ICommand command)
      {
         if (command.IsAnImplementationOf<ILabelCommand>())
            return SerializationConstants.LabelCommand;
         if (command.IsAnImplementationOf<IInfoCommand>())
            return SerializationConstants.InfoCommand;
         if (command.IsAnImplementationOf<IMacroCommand>())
            return SerializationConstants.MacroCommand;

         return SerializationConstants.SimpleCommand;
      }

      private CommandMetaData metaDataCommandFrom(IMacroCommand macroCommand)
      {
         var commandMetaData = new CommandMetaData { Discriminator = SerializationConstants.MacroCommand };
         macroCommand.All().Each(childCommand => commandMetaData.AddCommand(MapFrom(childCommand)));
         return commandMetaData;
      }
   }
}