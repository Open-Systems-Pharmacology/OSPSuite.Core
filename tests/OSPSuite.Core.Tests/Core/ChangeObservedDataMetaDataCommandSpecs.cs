using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using Command = OSPSuite.Assets.Command;

namespace OSPSuite.Core
{
   public abstract class concern_for_ChangeObservedDataMetaDataCommand : ContextSpecification<ChangeObservedDataMetaDataCommand>
   {
      protected MetaDataChanged _metaDataChanged;
      protected DataRepository _dataRepository;
      protected IOSPSuiteExecutionContext _executionContext = A.Fake<IOSPSuiteExecutionContext>();
      protected override void Context()
      {
         _dataRepository = new DataRepository();
         sut = new ChangeObservedDataMetaDataCommand(_dataRepository, _metaDataChanged);
      }
   }

   public class When_asking_for_inverse_command : concern_for_ChangeObservedDataMetaDataCommand
   {
      private IReversibleCommand<IOSPSuiteExecutionContext> _result;

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
      }

      protected override void Because()
      {
         _result = sut.InverseCommand(_executionContext);
         _result.Execute(_executionContext);
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
