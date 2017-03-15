using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using NUnit.Framework;
using OSPSuite.Assets;
using OSPSuite.Core.Commands;
using OSPSuite.Infrastructure.Serialization.ORM.Mappers;
using OSPSuite.Infrastructure.Serialization.ORM.MetaData;

namespace OSPSuite.Infrastructure
{
   public abstract class concern_for_CommandMetaDataToCommandMapper : ContextSpecification<CommandMetaDataToCommandMapper>
   {
      protected ICommandMetaDataRepository _commandMetaDataRepository;

      protected override void Context()
      {
         _commandMetaDataRepository = A.Fake<ICommandMetaDataRepository>();
         sut = new CommandMetaDataToCommandMapper(_commandMetaDataRepository);
      }
   }

   public class When_mapping_a_macro_command_with_sub_commands : concern_for_CommandMetaDataToCommandMapper
   {
      private CommandMetaData _macroCommand;
      private OSPSuiteMacroCommand<IOSPSuiteExecutionContext> _result;
      private IEnumerable<CommandMetaData> _subCommands;

      protected override void Context()
      {
         base.Context();
         _macroCommand = new CommandMetaData {Discriminator = SerializationConstants.MacroCommand};
         _subCommands = new List<CommandMetaData>
         {
            new CommandMetaData(),
            new CommandMetaData(),
            new CommandMetaData()
         };
         A.CallTo(() => _commandMetaDataRepository.AllChildrenOf(_macroCommand)).Returns(_subCommands);
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_macroCommand) as OSPSuiteMacroCommand<IOSPSuiteExecutionContext>;
      }

      [Observation]
      public void should_return_a_macro_command_with_sub_commands()
      {
         _result.Count.ShouldBeEqualTo(_subCommands.Count());
      }
   }
   
   public class When_mapping_meta_data_to_a_command : concern_for_CommandMetaDataToCommandMapper
   {
      [TestCase("MacroCommand", typeof(OSPSuiteMacroCommand<IOSPSuiteExecutionContext>))]
      [TestCase("SimpleCommand", typeof(ReadOnlyCommand))]
      [TestCase("LabelCommand", typeof(OSPSuiteLabelCommand))]
      [TestCase("InfoCommand", typeof(OSPSuiteInfoCommand))]
      public void should_return_a_the_correct_command_type(string discriminator, Type t)
      {
         sut.MapFrom(new CommandMetaData { Discriminator = discriminator}).ShouldBeAnInstanceOf(t);
      }
   }
}
