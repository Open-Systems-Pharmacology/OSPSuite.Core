using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Helpers;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Services;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Utility.Extensions;
using Command = OSPSuite.Assets.Command;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_EditObservedDataTask : ContextSpecification<IObservedDataMetaDataTask>
   {
      private IApplicationController _applicationController;
      private IOSPSuiteExecutionContext _context;
      private IDimensionFactory _dimensionFactory;
      protected IParameterIdentificationTask _parameterIdentificationTask;

      protected override void Context()
      {
         _applicationController = A.Fake<IApplicationController>();
         _context = A.Fake<IOSPSuiteExecutionContext>();
         _dimensionFactory = A.Fake<IDimensionFactory>();
         _parameterIdentificationTask = A.Fake<IParameterIdentificationTask>();

         sut = new ObservedDataMetaDataTask(_context, _applicationController, _dimensionFactory, _parameterIdentificationTask);
      }
   }


   public abstract class When_executing_metadata_commands_on_repository : concern_for_EditObservedDataTask
   {
      protected ICommand _result;
      protected List<DataRepository> _list;
      protected string _expectedCommandType;
      protected string _expectedDescription;

      protected override void Context()
      {
         base.Context();
         _list = new List<DataRepository> {new DataRepository(), new DataRepository()};
      }

      [Observation]
      public void returned_command_should_be_pksimmacrocommand()
      {
         _result.ShouldBeAnInstanceOf<IMacroCommand>();
      }

      [Observation]
      public void returned_command_should_contain_a_command_per_repository()
      {
         var command = _result as IMacroCommand;

         // ReSharper disable once PossibleNullReferenceException - allow this to fail if returned command is wrong type
         command.Count.ShouldBeEqualTo(_list.Count);
      }

      [Observation]
      public void description_has_appropriate_information()
      {
         _result.Description.ShouldBeEqualTo(_expectedDescription);
      }

      [Observation]
      public void should_have_set_the_expected_type()
      {
         _result.CommandType.ShouldBeEqualTo(_expectedCommandType);
      }

      [Observation]
      public void object_type_should_be_observed_data()
      {
         _result.ObjectType.ShouldBeEqualTo(ObjectTypes.ObservedData);
      }
   }

   public class When_executing_metadata_remove_command_on_repsoitory : When_executing_metadata_commands_on_repository
   {
      protected override void Context()
      {
         base.Context();
         _expectedCommandType = Command.CommandTypeDelete;
         _expectedDescription = Command.MetaDataRemovedFromDataRepositories;
      }

      protected override void Because()
      {
         _result = sut.RemoveMetaData(_list, new MetaDataKeyValue {Key = "Key", Value = "Value"});
      }
   }

   public class When_executing_metadata_change_command_on_repository : When_executing_metadata_commands_on_repository
   {
      protected override void Context()
      {
         base.Context();
         _expectedCommandType = Command.CommandTypeEdit;
         _expectedDescription = Command.MetaDataModifiedInDataRepositories;
      }

      protected override void Because()
      {
         _result = sut.ChangeMetaData(_list, new MetaDataChanged());
      }
   }

   public class When_executing_metadata_add_command_on_repository : When_executing_metadata_commands_on_repository
   {
      protected override void Context()
      {
         base.Context();
         _expectedCommandType = Command.CommandTypeAdd;
         _expectedDescription = Command.MetaDataAddedToDataRepositories;
      }

      protected override void Because()
      {
         _result = sut.AddMetaData(_list, new MetaDataKeyValue {Key = "Key", Value = "Value"});
      }
   }

   public class When_executing_the_update_molecular_weight_on_repositories : When_executing_metadata_commands_on_repository
   {
      private DataRepository _observedData;
      private DataColumn _column;

      protected override void Context()
      {
         base.Context();
         _expectedCommandType = Command.CommandTypeEdit;
         _expectedDescription = Command.MolecularWeightModifiedInDataRepositories;
         var baseGrid = new BaseGrid("Time", "Time", DomainHelperForSpecs.TimeDimensionForSpecs());
         _column = new DataColumn("Col", "Col", DomainHelperForSpecs.ConcentrationDimensionForSpecs(), baseGrid) ;

         _observedData = new DataRepository { _column };

         _list = new List<DataRepository>{_observedData};
      }

      protected override void Because()
      {
         _result = sut.UpdateMolWeight(_list, 20, 50);
      }

      [Observation]
      public void should_have_set_the_mol_weight_as_expected()
      {
         _column.DataInfo.MolWeight.ShouldBeEqualTo(50);
      }
   }
   public abstract class When_updating_the_subject_count_on_repositories : concern_for_EditObservedDataTask
   {
      protected ICommand _result;
      protected DataRepository _withoutSubjectCount;
      protected DataRepository _withSameSubjectCount;
      protected DataRepository _withOtherSubjectCount;
      protected List<DataRepository> _list;

      protected override void Context()
      {
         base.Context();
         _withoutSubjectCount = new DataRepository();
         _withSameSubjectCount = new DataRepository();
         _withSameSubjectCount.SetSubjectCount(12);
         _withOtherSubjectCount = new DataRepository();
         _withOtherSubjectCount.SetSubjectCount(5);
         _list = new List<DataRepository> {_withoutSubjectCount, _withSameSubjectCount, _withOtherSubjectCount};
      }
   }

   public class When_setting_a_subject_count_on_repositories : When_updating_the_subject_count_on_repositories
   {
      protected override void Because()
      {
         _result = sut.UpdateSubjectCount(_list, " 012 ");
      }

      [Observation]
      public void should_return_a_subject_count_edit_command_for_observed_data()
      {
         _result.ShouldBeAnInstanceOf<IMacroCommand>();
         _result.CommandType.ShouldBeEqualTo(Command.CommandTypeEdit);
         _result.ObjectType.ShouldBeEqualTo(ObjectTypes.ObservedData);
         _result.Description.ShouldBeEqualTo(Command.SubjectCountModifiedInDataRepositories);
      }

      [Observation]
      public void should_set_the_normalized_subject_count_on_all_repositories()
      {
         _list.Each(x => x.ExtendedPropertyValueFor(Constants.ObservedData.SUBJECT_COUNT).ShouldBeEqualTo("12"));
      }

      [Observation]
      public void should_only_create_commands_for_the_repositories_with_a_different_subject_count()
      {
         var macroCommand = _result as IMacroCommand;
         macroCommand.All().Count().ShouldBeEqualTo(2);
         macroCommand.All().OfType<AddObservedDataMetaDataCommand>().Count().ShouldBeEqualTo(1);
         macroCommand.All().OfType<ChangeObservedDataMetaDataCommand>().Count().ShouldBeEqualTo(1);
      }
   }

   public class When_clearing_the_subject_count_on_repositories : When_updating_the_subject_count_on_repositories
   {
      protected override void Because()
      {
         _result = sut.UpdateSubjectCount(_list, string.Empty);
      }

      [Observation]
      public void should_remove_the_subject_count_from_all_repositories()
      {
         _list.Each(x => x.ExtendedProperties.Contains(Constants.ObservedData.SUBJECT_COUNT).ShouldBeFalse());
      }

      [Observation]
      public void should_only_create_remove_commands_for_the_repositories_having_a_subject_count()
      {
         var macroCommand = _result as IMacroCommand;
         macroCommand.All().Count().ShouldBeEqualTo(2);
         macroCommand.All().Each(x => x.ShouldBeAnInstanceOf<RemoveObservedDataMetaDataCommand>());
      }
   }

   public class When_setting_an_invalid_subject_count_on_repositories : When_updating_the_subject_count_on_repositories
   {
      [Observation]
      public void should_throw_an_exception_and_leave_the_repositories_unchanged()
      {
         The.Action(() => sut.UpdateSubjectCount(_list, "0")).ShouldThrowAn<OSPSuiteException>();
         _withoutSubjectCount.SubjectCount().ShouldBeNull();
         _withSameSubjectCount.SubjectCount().ShouldBeEqualTo(12);
         _withOtherSubjectCount.SubjectCount().ShouldBeEqualTo(5);
      }
   }
}