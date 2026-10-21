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
   public abstract class concern_for_RemoveObservedDataMetaDataCommand : ContextSpecification<RemoveObservedDataMetaDataCommand>
   {
      protected MetaDataKeyValue _metaDataKeyValue;
      protected DataRepository _dataRepository;
      protected IOSPSuiteExecutionContext _executionContext = A.Fake<IOSPSuiteExecutionContext>();

      protected override void Context()
      {
         _dataRepository = new DataRepository();
         A.CallTo(() => _executionContext.Get<DataRepository>(_dataRepository.Id)).Returns(_dataRepository);

         sut = new RemoveObservedDataMetaDataCommand(_dataRepository, _metaDataKeyValue);
      }
   }

   public class When_matching_meta_data_is_not_found_to_remove : concern_for_RemoveObservedDataMetaDataCommand
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
         _metaDataKeyValue = new MetaDataKeyValue {Key = "Key", Value = "Value"};
         base.Context();
         _dataRepository.ExtendedProperties.Add(new ExtendedProperty<string> {Name = "Name", Value = "Value"});
      }

      protected override void Because()
      {
         sut.Execute(_executionContext);
      }
   }

   public class When_existing_matching_meta_data_is_found_to_remove : concern_for_RemoveObservedDataMetaDataCommand
   {
      [Observation]
      public void existing_matching_meta_data_is_removed()
      {
         _dataRepository.ExtendedProperties.Where(x => x.Name.Equals("Name")).ShouldBeEmpty();
      }

      protected override void Context()
      {
         _metaDataKeyValue = new MetaDataKeyValue {Key = "Name", Value = "Value"};
         base.Context();
         _dataRepository.ExtendedProperties.Add(new ExtendedProperty<string> {Name = "Name", Value = "Value"});
      }

      protected override void Because()
      {
         sut.Execute(_executionContext);
      }
   }

   public class When_retrieving_inverse_of_remove_command : concern_for_RemoveObservedDataMetaDataCommand
   {
      private ICommand<IOSPSuiteExecutionContext> _result;

      [Observation]
      public void inverse_command_should_be_of_correct_type()
      {
         _result.ShouldBeAnInstanceOf<AddObservedDataMetaDataCommand>();
      }

      protected override void Context()
      {
         _metaDataKeyValue = new MetaDataKeyValue {Key = "Key", Value = "Value"};
         base.Context();
         _dataRepository.ExtendedProperties.Add(new ExtendedProperty<string> {Name = "Key", Value = "Value"});
         sut.Execute(_executionContext);
      }

      protected override void Because()
      {
         sut.RestoreExecutionData(_executionContext);
         _result = sut.InverseCommand(_executionContext);
      }
   }

   public class When_reverting_the_removal_of_meta_data_whose_value_differs_between_two_repositories : concern_for_RemoveObservedDataMetaDataCommand
   {
      private DataRepository _otherDataRepository;
      private RemoveObservedDataMetaDataCommand _otherCommand;

      protected override void Context()
      {
         //Meta data shared by the commands of all repositories. Its value is empty because the values differ
         _metaDataKeyValue = new MetaDataKeyValue {Key = "Species", Value = string.Empty};
         base.Context();
         _dataRepository.ExtendedProperties.Add(new ExtendedProperty<string> {Name = "Species", Value = "Human"});

         _otherDataRepository = new DataRepository();
         _otherDataRepository.ExtendedProperties.Add(new ExtendedProperty<string> {Name = "Species", Value = "Dog"});
         A.CallTo(() => _executionContext.Get<DataRepository>(_otherDataRepository.Id)).Returns(_otherDataRepository);
         _otherCommand = new RemoveObservedDataMetaDataCommand(_otherDataRepository, _metaDataKeyValue);

         sut.Execute(_executionContext);
         _otherCommand.Execute(_executionContext);
      }

      protected override void Because()
      {
         _otherCommand.InvokeInverse(_executionContext);
         sut.InvokeInverse(_executionContext);
      }

      [Observation]
      public void should_restore_the_value_of_each_repository()
      {
         _dataRepository.ExtendedPropertyValueFor("Species").ShouldBeEqualTo("Human");
         _otherDataRepository.ExtendedPropertyValueFor("Species").ShouldBeEqualTo("Dog");
      }

      [Observation]
      public void should_describe_the_value_removed_from_each_repository()
      {
         sut.Description.ShouldBeEqualTo(Command.SetMetaDataRemovedCommandDescription("Species", "Human"));
         _otherCommand.Description.ShouldBeEqualTo(Command.SetMetaDataRemovedCommandDescription("Species", "Dog"));
      }

      [Observation]
      public void should_not_modify_the_shared_meta_data()
      {
         _metaDataKeyValue.Key.ShouldBeEqualTo("Species");
         _metaDataKeyValue.Value.ShouldBeEqualTo(string.Empty);
      }
   }

   public class When_reverting_the_removal_of_meta_data_that_was_not_found : concern_for_RemoveObservedDataMetaDataCommand
   {
      private ICommand<IOSPSuiteExecutionContext> _inverse;

      protected override void Context()
      {
         _metaDataKeyValue = new MetaDataKeyValue {Key = "Key", Value = "Value"};
         base.Context();
         _dataRepository.ExtendedProperties.Add(new ExtendedProperty<string> {Name = "Name", Value = "Value"});
         sut.Execute(_executionContext);
      }

      protected override void Because()
      {
         _inverse = sut.InvokeInverse(_executionContext);
      }

      [Observation]
      public void should_not_add_the_meta_data_to_the_repository()
      {
         _dataRepository.ExtendedProperties.Contains("Key").ShouldBeFalse();
         _dataRepository.ExtendedProperties.Count().ShouldBeEqualTo(1);
      }

      [Observation]
      public void should_return_an_inverse_that_can_be_reverted_again()
      {
         _inverse.ShouldBeAnInstanceOf<IReversibleCommand<IOSPSuiteExecutionContext>>();
      }
   }
}