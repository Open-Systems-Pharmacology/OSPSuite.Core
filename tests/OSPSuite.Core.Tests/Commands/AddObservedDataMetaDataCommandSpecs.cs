using System;
using FakeItEasy;
using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;

namespace OSPSuite.Core.Commands
{
   public abstract class concern_for_AddObservedDataMetaDataCommand : ContextSpecification<AddObservedDataMetaDataCommand>
   {
      protected MetaDataKeyValue _metaDataKeyValue;
      protected DataRepository _dataRepository;
      protected IOSPSuiteExecutionContext _executionContext = A.Fake<IOSPSuiteExecutionContext>();
      protected override void Context()
      {
         _dataRepository = new DataRepository();

         sut = new AddObservedDataMetaDataCommand(_dataRepository, _metaDataKeyValue);
      }
   }

   public class When_retrieving_inverse_command : concern_for_AddObservedDataMetaDataCommand
   {
      private IReversibleCommand<IOSPSuiteExecutionContext> _result;

      [Observation]
      public void inverse_command_should_be_of_correct_type()
      {
         _result.ShouldBeAnInstanceOf<RemoveObservedDataMetaDataCommand>();
      }

      protected override void Context()
      {
         _metaDataKeyValue = new MetaDataKeyValue { Key = "Key", Value = "Value" };
         base.Context();
      }

      protected override void Because()
      {
         _result = sut.InverseCommand(_executionContext);
      }
   }

   public class When_existing_matching_meta_data_is_found : concern_for_AddObservedDataMetaDataCommand
   {
      private ArgumentException _result;

      [Observation]
      public void argument_exception_thrown()
      {
         _result.ShouldNotBeNull();
      }

      [Observation]
      public void should_contain_appropriate_message()
      {
         _result.Message.ShouldBeEqualTo(Error.CannotAddMetaDataDuplicateKey("Key", "Value"));
      }

      protected override void Context()
      {
         _metaDataKeyValue = new MetaDataKeyValue { Key = "Key", Value = "Value" };
         base.Context();
         _dataRepository.ExtendedProperties.Add(new ExtendedProperty<string> { Name = "Key", Value = "Value" });
      }

      protected override void Because()
      {
         try
         {
            sut.Execute(_executionContext);
         }
         catch (ArgumentException e)
         {
            _result = e;
         }
      }
   }

   public class When_adding_new_meta_data_to_repository : concern_for_AddObservedDataMetaDataCommand
   {
      [Observation]
      public void new_meta_data_must_be_added_to_repository()
      {
         _dataRepository.ExtendedProperties.Contains("Key").ShouldBeTrue();
         _dataRepository.ExtendedProperties["Key"].ValueAsObject.ShouldBeEqualTo("Value");
      }

      protected override void Context()
      {
         _metaDataKeyValue = new MetaDataKeyValue { Key = "Key", Value = "Value" };
         base.Context();
      }

      protected override void Because()
      {
         sut.Execute(_executionContext);
      }
   }
}
