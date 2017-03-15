using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;

namespace OSPSuite.Core
{
   public abstract class concern_for_RemoveObservedDataMetaDataCommand : ContextSpecification<RemoveObservedDataMetaDataCommand>
   {
      protected MetaDataKeyValue _metaDataKeyValue;
      protected DataRepository _dataRepository;
      protected IOSPSuiteExecutionContext _executionContext = A.Fake<IOSPSuiteExecutionContext>();

      protected override void Context()
      {
         _dataRepository = new DataRepository();

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
      private IReversibleCommand<IOSPSuiteExecutionContext> _result;

      [Observation]
      public void inverse_command_should_be_of_correct_type()
      {
         _result.ShouldBeAnInstanceOf<AddObservedDataMetaDataCommand>();
      }

      protected override void Context()
      {
         _metaDataKeyValue = new MetaDataKeyValue {Key = "Key", Value = "Value"};
         base.Context();
      }

      protected override void Because()
      {
         _result = sut.InverseCommand(_executionContext);
      }
   }
}