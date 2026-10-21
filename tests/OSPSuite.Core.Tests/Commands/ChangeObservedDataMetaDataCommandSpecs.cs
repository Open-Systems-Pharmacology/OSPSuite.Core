using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using Command = OSPSuite.Assets.Command;

namespace OSPSuite.Core.Commands
{
   public abstract class concern_for_ChangeObservedDataMetaDataCommand : ContextSpecification<ChangeObservedDataMetaDataCommand>
   {
      protected MetaDataChanged _metaDataChanged;
      protected DataRepository _dataRepository;
      protected IOSPSuiteExecutionContext _executionContext = A.Fake<IOSPSuiteExecutionContext>();
      protected override void Context()
      {
         _dataRepository = new DataRepository();
         A.CallTo(() => _executionContext.Get<DataRepository>(_dataRepository.Id)).Returns(_dataRepository);
         sut = new ChangeObservedDataMetaDataCommand(_dataRepository, _metaDataChanged);
      }
   }

   public abstract class concern_for_ChangeObservedDataMetaDataCommand_with_values_differing_between_two_repositories : concern_for_ChangeObservedDataMetaDataCommand
   {
      protected DataRepository _otherDataRepository;
      protected ChangeObservedDataMetaDataCommand _otherCommand;

      protected override void Context()
      {
         base.Context();
         _dataRepository.ExtendedProperties.Add(new ExtendedProperty<string> { Name = "Species", Value = "Human" });

         _otherDataRepository = new DataRepository();
         _otherDataRepository.ExtendedProperties.Add(new ExtendedProperty<string> { Name = "Species", Value = "Dog" });
         A.CallTo(() => _executionContext.Get<DataRepository>(_otherDataRepository.Id)).Returns(_otherDataRepository);
         _otherCommand = new ChangeObservedDataMetaDataCommand(_otherDataRepository, _metaDataChanged);
      }

      protected void executeBothCommands()
      {
         sut.Execute(_executionContext);
         _otherCommand.Execute(_executionContext);
      }

      protected void revertBothCommands()
      {
         _otherCommand.InvokeInverse(_executionContext);
         sut.InvokeInverse(_executionContext);
      }
   }

   public class When_renaming_meta_data_whose_value_differs_between_two_repositories : concern_for_ChangeObservedDataMetaDataCommand_with_values_differing_between_two_repositories
   {
      protected override void Context()
      {
         //Meta data shared by the commands of all repositories. Its value is empty because the values differ
         _metaDataChanged = new MetaDataChanged { OldName = "Species", NewName = "Organism", OldValue = string.Empty, NewValue = string.Empty };
         base.Context();
      }

      protected override void Because()
      {
         executeBothCommands();
      }

      [Observation]
      public void should_keep_the_value_of_each_repository_under_the_new_name()
      {
         _dataRepository.ExtendedPropertyValueFor("Organism").ShouldBeEqualTo("Human");
         _otherDataRepository.ExtendedPropertyValueFor("Organism").ShouldBeEqualTo("Dog");
         _dataRepository.ExtendedProperties.Contains("Species").ShouldBeFalse();
         _otherDataRepository.ExtendedProperties.Contains("Species").ShouldBeFalse();
      }

      [Observation]
      public void should_describe_the_value_of_each_repository()
      {
         sut.Description.ShouldBeEqualTo(Command.SetMetaDataChangedCommandDescription("Species", "Human", "Organism", "Human"));
         _otherCommand.Description.ShouldBeEqualTo(Command.SetMetaDataChangedCommandDescription("Species", "Dog", "Organism", "Dog"));
      }

      [Observation]
      public void should_not_modify_the_shared_meta_data()
      {
         _metaDataChanged.OldValue.ShouldBeEqualTo(string.Empty);
         _metaDataChanged.NewValue.ShouldBeEqualTo(string.Empty);
      }
   }

   public class When_reverting_the_renaming_of_meta_data_whose_value_differs_between_two_repositories : concern_for_ChangeObservedDataMetaDataCommand_with_values_differing_between_two_repositories
   {
      protected override void Context()
      {
         _metaDataChanged = new MetaDataChanged { OldName = "Species", NewName = "Organism", OldValue = string.Empty, NewValue = string.Empty };
         base.Context();
         executeBothCommands();
      }

      protected override void Because()
      {
         revertBothCommands();
      }

      [Observation]
      public void should_restore_the_value_of_each_repository_under_the_old_name()
      {
         _dataRepository.ExtendedPropertyValueFor("Species").ShouldBeEqualTo("Human");
         _otherDataRepository.ExtendedPropertyValueFor("Species").ShouldBeEqualTo("Dog");
         _dataRepository.ExtendedProperties.Contains("Organism").ShouldBeFalse();
         _otherDataRepository.ExtendedProperties.Contains("Organism").ShouldBeFalse();
      }
   }

   public class When_changing_the_value_of_meta_data_whose_value_differs_between_two_repositories : concern_for_ChangeObservedDataMetaDataCommand_with_values_differing_between_two_repositories
   {
      protected override void Context()
      {
         _metaDataChanged = new MetaDataChanged { OldName = "Species", NewName = "Species", OldValue = string.Empty, NewValue = "Rat" };
         base.Context();
      }

      protected override void Because()
      {
         executeBothCommands();
      }

      [Observation]
      public void should_set_the_new_value_in_each_repository()
      {
         _dataRepository.ExtendedPropertyValueFor("Species").ShouldBeEqualTo("Rat");
         _otherDataRepository.ExtendedPropertyValueFor("Species").ShouldBeEqualTo("Rat");
      }

      [Observation]
      public void should_describe_the_value_changed_in_each_repository()
      {
         sut.Description.ShouldBeEqualTo(Command.SetMetaDataChangedCommandDescription("Species", "Human", "Species", "Rat"));
         _otherCommand.Description.ShouldBeEqualTo(Command.SetMetaDataChangedCommandDescription("Species", "Dog", "Species", "Rat"));
      }
   }

   public class When_reverting_a_value_change_of_meta_data_whose_value_differed_between_two_repositories : concern_for_ChangeObservedDataMetaDataCommand_with_values_differing_between_two_repositories
   {
      protected override void Context()
      {
         _metaDataChanged = new MetaDataChanged { OldName = "Species", NewName = "Species", OldValue = string.Empty, NewValue = "Rat" };
         base.Context();
         executeBothCommands();
      }

      protected override void Because()
      {
         revertBothCommands();
      }

      [Observation]
      public void should_restore_the_value_of_each_repository()
      {
         _dataRepository.ExtendedPropertyValueFor("Species").ShouldBeEqualTo("Human");
         _otherDataRepository.ExtendedPropertyValueFor("Species").ShouldBeEqualTo("Dog");
      }

      [Observation]
      public void should_not_modify_the_shared_meta_data()
      {
         _metaDataChanged.OldValue.ShouldBeEqualTo(string.Empty);
         _metaDataChanged.NewValue.ShouldBeEqualTo("Rat");
      }
   }

   public class When_reverting_the_change_of_meta_data_that_was_not_found : concern_for_ChangeObservedDataMetaDataCommand
   {
      private ICommand<IOSPSuiteExecutionContext> _inverse;

      protected override void Context()
      {
         _metaDataChanged = new MetaDataChanged { OldName = "Key", OldValue = "Value", NewName = "NewKey", NewValue = "NewValue" };
         base.Context();
         _dataRepository.ExtendedProperties.Add(new ExtendedProperty<string> { Name = "NewKey", Value = "Existing" });
         sut.Execute(_executionContext);
      }

      protected override void Because()
      {
         _inverse = sut.InvokeInverse(_executionContext);
      }

      [Observation]
      public void should_leave_the_repository_unchanged()
      {
         _dataRepository.ExtendedPropertyValueFor("NewKey").ShouldBeEqualTo("Existing");
         _dataRepository.ExtendedProperties.Contains("Key").ShouldBeFalse();
         _dataRepository.ExtendedProperties.Count().ShouldBeEqualTo(1);
      }

      [Observation]
      public void should_return_an_inverse_that_can_be_reverted_again()
      {
         _inverse.ShouldBeAnInstanceOf<IReversibleCommand<IOSPSuiteExecutionContext>>();
      }
   }

   public class When_asking_for_inverse_command : concern_for_ChangeObservedDataMetaDataCommand
   {
      private ICommand<IOSPSuiteExecutionContext> _result;

      [Observation]
      public void command_should_be_appropriate_type()
      {
         _result.ShouldBeAnInstanceOf<ChangeObservedDataMetaDataCommand>();
      }

      [Observation]
      public void returns_command_with_reverse_change_parameters()
      {
         _result.Description.ShouldBeEqualTo(Command.SetMetaDataChangedCommandDescription("NewName", "NewValue", "OldName", "OldValue"));
      }

      protected override void Context()
      {
         _metaDataChanged = new MetaDataChanged { NewName = "NewName", OldName = "OldName", NewValue = "NewValue", OldValue = "OldValue" };
         base.Context();
         _dataRepository.ExtendedProperties.Add(new ExtendedProperty<string> { Name = "OldName", Value = "OldValue" });
         sut.Execute(_executionContext);
      }

      protected override void Because()
      {
         _result = sut.InvokeInverse(_executionContext);
      }
   }

   public class When_matching_meta_data_is_found_to_change : concern_for_ChangeObservedDataMetaDataCommand
   {
      [Observation]
      public void meta_data_is_changed()
      {
         _dataRepository.ExtendedProperties.All(x => x.Name.Equals("NewName") && x.ValueAsObject.Equals("NewValue")).ShouldBeTrue();
         _dataRepository.ExtendedProperties.Count().ShouldBeEqualTo(1);
      }
      [Observation]
      public void returns_command_with_reverse_change_parameters()
      {
         sut.Description.ShouldBeEqualTo(Command.SetMetaDataChangedCommandDescription("OldName", "OldValue", "NewName", "NewValue"));
      }
      protected override void Context()
      {
         _metaDataChanged = new MetaDataChanged { NewName = "NewName", OldName = "OldName", NewValue = "NewValue", OldValue = "OldValue" };
         base.Context();
         _dataRepository.ExtendedProperties.Add(new ExtendedProperty<string> { Name = "OldName", Value = "OldValue" });
      }

      protected override void Because()
      {
         sut.Execute(_executionContext);
      }
   }

   public class When_matching_meta_data_is_not_found_to_change : concern_for_ChangeObservedDataMetaDataCommand
   {
      [Observation]
      public void meta_data_remains()
      {
         _dataRepository.ExtendedProperties.Count(x => x.Name.Equals("Name")).ShouldBeEqualTo(1);
      }

      [Observation]
      public void only_one_metadata()
      {
         _dataRepository.ExtendedProperties.Count().ShouldBeEqualTo(1);
      }

      protected override void Context()
      {
         _metaDataChanged = new MetaDataChanged { OldName = "Key", OldValue = "Value", NewValue = "NewValue", NewName = "NewKey" };
         base.Context();
         _dataRepository.ExtendedProperties.Add(new ExtendedProperty<string> { Name = "Name", Value = "Value" });
      }

      protected override void Because()
      {
         sut.Execute(_executionContext);
      }

   }
}
