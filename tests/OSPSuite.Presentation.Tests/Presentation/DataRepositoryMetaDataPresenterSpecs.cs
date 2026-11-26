using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Events;
using OSPSuite.Helpers;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Presenters.ObservedData;
using OSPSuite.Presentation.Views.ObservedData;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Validation;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_DataRepositoryMetaDataPresenter : ContextSpecification<DataRepositoryMetaDataPresenter>
   {
      protected IObservedDataMetaDataTask _observedDataMetaDataTask;
      protected IDataRepositoryMetaDataView _view;
      protected DataRepository _dataRepository;
      private ICommandCollector _commandCollector;
      protected IObservedDataConfiguration _observedDataConfiguration;
      private IDimensionFactory _dimensionFactory;
      protected IParameterFactory _parameterFactory;
      protected DataColumn _dataColumn1;
      protected DataColumn _dataColumn2;

      protected override void Context()
      {
         _commandCollector = A.Fake<ICommandCollector>();
         _observedDataMetaDataTask = A.Fake<IObservedDataMetaDataTask>();
         _view = A.Fake<IDataRepositoryMetaDataView>();
         _observedDataConfiguration = A.Fake<IObservedDataConfiguration>();
         _dataRepository = new DataRepository();
         _dimensionFactory = A.Fake<IDimensionFactory>();
         _parameterFactory = A.Fake<IParameterFactory>();
         sut = new DataRepositoryMetaDataPresenter(_view, _observedDataMetaDataTask, _observedDataConfiguration, _parameterFactory, _dimensionFactory);
         sut.InitializeWith(_commandCollector);
         sut.EditObservedData(_dataRepository);

         var baseGrid = new BaseGrid("time", DomainHelperForSpecs.TimeDimensionForSpecs());
         _dataColumn1 = new DataColumn("Col1", "Col1", DomainHelperForSpecs.ConcentrationDimensionForSpecs(), baseGrid);
         _dataColumn2 = new DataColumn("Col2", "Col2", DomainHelperForSpecs.ConcentrationDimensionForSpecs(), baseGrid);
         _dataRepository.Add(_dataColumn1);
         _dataRepository.Add(_dataColumn2);
      }

      protected void SetExtendedProperty(string key, string value)
      {
         _dataRepository.ExtendedProperties.Add(new ExtendedProperty<string> {Name = key, Value = value});
      }
   }

   public abstract class When_handling_meta_data_events_for_nonbound_meta_data : concern_for_DataRepositoryMetaDataPresenter
   {
      [Observation]
      public void rebinds_to_view_with_new_list()
      {
         A.CallTo(() => _view.BindToMetaData(A<NotifyList<MetaDataDTO>>.Ignored)).MustHaveHappenedOnceExactly();
      }
   }

   public abstract class When_handling_meta_data_events_for_bound_meta_data : concern_for_DataRepositoryMetaDataPresenter
   {
      [Observation]
      public void rebinds_to_view_with_new_list()
      {
         A.CallTo(() => _view.BindToMetaData(A<NotifyList<MetaDataDTO>>.Ignored)).MustHaveHappenedTwiceExactly();
      }
   }

   public class When_handling_event_for_bound_meta_data_for_meta_data_changed : When_handling_meta_data_events_for_bound_meta_data
   {
      protected override void Because()
      {
         sut.Handle(new ObservedDataMetaDataChangedEvent(_dataRepository));
      }
   }

   public class When_handling_event_for_nonbound_meta_data_for_meta_data_changed : When_handling_meta_data_events_for_nonbound_meta_data
   {
      protected override void Because()
      {
         sut.Handle(new ObservedDataMetaDataChangedEvent(new DataRepository()));
      }
   }

   public class When_handling_event_for_bound_meta_data_for_meta_data_removed : When_handling_meta_data_events_for_bound_meta_data
   {
      protected override void Because()
      {
         sut.Handle(new ObservedDataMetaDataRemovedEvent(_dataRepository));
      }
   }

   public class When_handling_event_for_nonbound_meta_data_for_meta_data_removed : When_handling_meta_data_events_for_nonbound_meta_data
   {
      protected override void Because()
      {
         sut.Handle(new ObservedDataMetaDataRemovedEvent(new DataRepository()));
      }
   }

   public class When_handling_event_for_bound_meta_data_for_meta_data_added : When_handling_meta_data_events_for_bound_meta_data
   {
      protected override void Because()
      {
         sut.Handle(new ObservedDataMetaDataAddedEvent(_dataRepository));
      }
   }

   public class When_handling_event_for_nonbound_meta_data_for_meta_data_added : When_handling_meta_data_events_for_nonbound_meta_data
   {
      protected override void Because()
      {
         sut.Handle(new ObservedDataMetaDataAddedEvent(new DataRepository()));
      }
   }

   public abstract class When_missing_data_in_dto : concern_for_DataRepositoryMetaDataPresenter
   {
      [Observation]
      public void command_to_add_is_not_issued()
      {
         A.CallTo(() => _observedDataMetaDataTask.AddMetaData(A<IEnumerable<DataRepository>>.Ignored, A<MetaDataKeyValue>.Ignored)).MustNotHaveHappened();
      }
   }

   public class When_asked_to_change_metadata_name_with_empty_value : When_missing_data_in_dto
   {
      protected override void Context()
      {
         base.Context();
         SetExtendedProperty("oldName", "oldValue");
      }

      protected override void Because()
      {
         sut.MetaDataNameChanged(new MetaDataDTO {Name = "oldName", Value = string.Empty}, "oldName");
      }
   }

   public class When_asked_to_add_metadata_with_empty_value : When_missing_data_in_dto
   {
      protected override void Because()
      {
         sut.MetaDataValueChanged(new MetaDataDTO {Name = "oldName", Value = string.Empty}, "newValue");
      }
   }

   public class When_asked_to_add_metadata_with_empty_name : When_missing_data_in_dto
   {
      protected override void Because()
      {
         sut.MetaDataValueChanged(new MetaDataDTO {Name = string.Empty, Value = "newValue"}, "oldValue");
      }
   }

   public class When_changing_metadata_value_where_metadata_doesnt_exist : concern_for_DataRepositoryMetaDataPresenter
   {
      [Observation]
      public void command_for_add_is_issued()
      {
         A.CallTo(() => _observedDataMetaDataTask.AddMetaData(A<IEnumerable<DataRepository>>.That.Contains(_dataRepository), A<MetaDataKeyValue>.That.Matches(x => x.Value.Equals("onlyValue") && x.Key.Equals("onlyName")))).MustHaveHappened();
      }

      protected override void Context()
      {
         base.Context();
         SetExtendedProperty("wrongName", "onlyValue");
      }

      protected override void Because()
      {
         sut.MetaDataValueChanged(new MetaDataDTO {Name = "onlyName", Value = "onlyValue"}, string.Empty);
      }
   }

   public class When_changing_metadata_value_where_metadata_exists : concern_for_DataRepositoryMetaDataPresenter
   {
      [Observation]
      public void command_for_change_is_issued()
      {
         A.CallTo(() => _observedDataMetaDataTask.ChangeMetaData(A<IEnumerable<DataRepository>>.That.Contains(_dataRepository), A<MetaDataChanged>.Ignored)).MustHaveHappened();
      }

      protected override void Context()
      {
         base.Context();
         SetExtendedProperty("onlyName", "oldValue");
      }

      protected override void Because()
      {
         sut.MetaDataValueChanged(new MetaDataDTO {Name = "onlyName", Value = "newValue"}, "oldValue");
      }
   }

   public class When_changing_metadata_name_where_metadata_doesnt_exist : concern_for_DataRepositoryMetaDataPresenter
   {
      [Observation]
      public void command_for_add_is_issued()
      {
         A.CallTo(() => _observedDataMetaDataTask.AddMetaData(A<IEnumerable<DataRepository>>.That.Contains(_dataRepository), A<MetaDataKeyValue>.Ignored)).MustHaveHappened();
      }

      protected override void Context()
      {
         base.Context();
         SetExtendedProperty("what", "that");
      }

      protected override void Because()
      {
         sut.MetaDataNameChanged(new MetaDataDTO {Name = "this", Value = "that"}, "that");
      }
   }

   public class When_changing_metadata_name_where_metadata_exists : concern_for_DataRepositoryMetaDataPresenter
   {
      protected override void Context()
      {
         base.Context();
         SetExtendedProperty("that", "that");
      }

      protected override void Because()
      {
         sut.MetaDataNameChanged(new MetaDataDTO {Name = "this", Value = "that"}, "that");
      }

      [Observation]
      public void command_for_change_is_issued()
      {
         A.CallTo(() => _observedDataMetaDataTask.ChangeMetaData(A<IEnumerable<DataRepository>>.That.Contains(_dataRepository), A<MetaDataChanged>.Ignored)).MustHaveHappened();
      }
   }

   public abstract class When_removing_a_metadata_row_that_was_never_saved : concern_for_DataRepositoryMetaDataPresenter
   {
      protected NotifyList<MetaDataDTO> _boundMetaData;
      protected MetaDataDTO _newMetaDataDTO;

      protected override void Context()
      {
         base.Context();
         SetExtendedProperty("Species", "Human");
         A.CallTo(() => _view.BindToMetaData(A<IEnumerable<MetaDataDTO>>._))
            .Invokes(x => _boundMetaData = x.GetArgument<NotifyList<MetaDataDTO>>(0));
         sut.EditObservedData(_dataRepository);
         sut.NewMetaDataAdded();
         _newMetaDataDTO = _boundMetaData.Last();
      }

      protected override void Because()
      {
         sut.RemoveMetaData(_newMetaDataDTO);
      }

      [Observation]
      public void command_to_remove_is_not_issued()
      {
         A.CallTo(() => _observedDataMetaDataTask.RemoveMetaData(A<IEnumerable<DataRepository>>._, A<MetaDataKeyValue>._)).MustNotHaveHappened();
      }

      [Observation]
      public void the_row_is_removed_from_the_view()
      {
         _boundMetaData.ShouldNotContain(_newMetaDataDTO);
      }

      [Observation]
      public void the_saved_rows_are_kept()
      {
         _boundMetaData.Select(x => x.Name).ShouldOnlyContain("Species");
      }

      [Observation]
      public void the_repository_metadata_is_unchanged()
      {
         _dataRepository.ExtendedProperties.Keys.ShouldOnlyContain("Species");
      }
   }

   public class When_removing_a_new_metadata_row_without_name : When_removing_a_metadata_row_that_was_never_saved
   {
   }

   public class When_removing_a_new_metadata_row_with_a_name_but_without_value : When_removing_a_metadata_row_that_was_never_saved
   {
      protected override void Context()
      {
         base.Context();
         _newMetaDataDTO.Name = "Route";
         sut.MetaDataNameChanged(_newMetaDataDTO, string.Empty);
      }

      [Observation]
      public void command_to_add_is_not_issued()
      {
         A.CallTo(() => _observedDataMetaDataTask.AddMetaData(A<IEnumerable<DataRepository>>._, A<MetaDataKeyValue>._)).MustNotHaveHappened();
      }
   }

   public class When_removing_a_new_metadata_row_with_the_name_of_an_existing_metadata : When_removing_a_metadata_row_that_was_never_saved
   {
      protected override void Context()
      {
         base.Context();
         _newMetaDataDTO.Name = "Species";
      }

      [Observation]
      public void the_existing_metadata_keeps_its_value()
      {
         _dataRepository.ExtendedPropertyValueFor("Species").ShouldBeEqualTo("Human");
      }
   }

   public class When_removing_a_metadata_row_that_exists_in_the_repository : concern_for_DataRepositoryMetaDataPresenter
   {
      private MetaDataDTO _metaDataDTO;

      protected override void Context()
      {
         base.Context();
         SetExtendedProperty("Species", "Human");
         A.CallTo(() => _view.BindToMetaData(A<IEnumerable<MetaDataDTO>>._))
            .Invokes(x => _metaDataDTO = x.GetArgument<IEnumerable<MetaDataDTO>>(0).Single());
         sut.EditObservedData(_dataRepository);
      }

      protected override void Because()
      {
         sut.RemoveMetaData(_metaDataDTO);
      }

      [Observation]
      public void command_to_remove_is_issued()
      {
         A.CallTo(() => _observedDataMetaDataTask.RemoveMetaData(A<IEnumerable<DataRepository>>.That.Contains(_dataRepository), A<MetaDataKeyValue>.That.Matches(x => x.Key.Equals("Species") && x.Value.Equals("Human")))).MustHaveHappened();
      }
   }

   public class When_removing_a_metadata_row_that_exists_in_the_repository_after_editing_its_name_without_saving : concern_for_DataRepositoryMetaDataPresenter
   {
      private MetaDataDTO _metaDataDTO;

      protected override void Context()
      {
         base.Context();
         SetExtendedProperty("Sheet", string.Empty);
         A.CallTo(() => _view.BindToMetaData(A<IEnumerable<MetaDataDTO>>._))
            .Invokes(x => _metaDataDTO = x.GetArgument<IEnumerable<MetaDataDTO>>(0).Single());
         sut.EditObservedData(_dataRepository);

         //not saved since the value is empty
         _metaDataDTO.Name = "Tab";
         sut.MetaDataNameChanged(_metaDataDTO, "Sheet");
      }

      protected override void Because()
      {
         sut.RemoveMetaData(_metaDataDTO);
      }

      [Observation]
      public void command_to_change_is_not_issued()
      {
         A.CallTo(() => _observedDataMetaDataTask.ChangeMetaData(A<IEnumerable<DataRepository>>._, A<MetaDataChanged>._)).MustNotHaveHappened();
      }

      [Observation]
      public void command_to_remove_the_saved_metadata_is_issued()
      {
         A.CallTo(() => _observedDataMetaDataTask.RemoveMetaData(A<IEnumerable<DataRepository>>.That.Contains(_dataRepository), A<MetaDataKeyValue>.That.Matches(x => x.Key.Equals("Sheet") && x.Value.Equals(string.Empty)))).MustHaveHappened();
      }
   }

   public class When_binding_saved_metadata_with_predefined_and_custom_names : concern_for_DataRepositoryMetaDataPresenter
   {
      private List<MetaDataDTO> _boundMetaData;

      protected override void Context()
      {
         base.Context();
         SetExtendedProperty("Species", "Human");
         SetExtendedProperty("Custom", "Value");
         A.CallTo(() => _observedDataConfiguration.DefaultMetaDataCategories).Returns(new[] { "Species" });
         A.CallTo(() => _view.BindToMetaData(A<IEnumerable<MetaDataDTO>>._))
            .Invokes(x => _boundMetaData = x.GetArgument<IEnumerable<MetaDataDTO>>(0).ToList());
      }

      protected override void Because()
      {
         sut.EditObservedData(_dataRepository);
      }

      [Observation]
      public void the_predefined_metadata_cannot_be_removed()
      {
         _boundMetaData.Single(x => x.Name == "Species").Removable.ShouldBeFalse();
      }

      [Observation]
      public void the_custom_metadata_can_be_removed()
      {
         _boundMetaData.Single(x => x.Name == "Custom").Removable.ShouldBeTrue();
      }
   }

   public abstract class When_the_output_path_metadata_row_was_added_and_never_saved : concern_for_DataRepositoryMetaDataPresenter
   {
      protected NotifyList<MetaDataDTO> _boundMetaData;
      protected MetaDataDTO _outputPathDTO;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _view.BindToMetaData(A<IEnumerable<MetaDataDTO>>._))
            .Invokes(x => _boundMetaData = x.GetArgument<NotifyList<MetaDataDTO>>(0));
         sut.EditObservedData(_dataRepository);
         sut.AddOutputPathMetadata();
         _outputPathDTO = _boundMetaData.Single(x => x.Name == Constants.OUTPUT_PATH);
      }
   }

   public class When_adding_the_output_path_metadata_row : When_the_output_path_metadata_row_was_added_and_never_saved
   {
      [Observation]
      public void the_row_name_cannot_be_edited()
      {
         _outputPathDTO.NameEditable.ShouldBeFalse();
      }

      [Observation]
      public void the_row_can_be_removed_before_a_value_is_entered()
      {
         _outputPathDTO.Removable.ShouldBeTrue();
      }
   }

   public class When_removing_the_output_path_metadata_row_before_a_value_is_entered : When_the_output_path_metadata_row_was_added_and_never_saved
   {
      protected override void Because()
      {
         sut.RemoveMetaData(_outputPathDTO);
      }

      [Observation]
      public void command_to_remove_is_not_issued()
      {
         A.CallTo(() => _observedDataMetaDataTask.RemoveMetaData(A<IEnumerable<DataRepository>>._, A<MetaDataKeyValue>._)).MustNotHaveHappened();
      }

      [Observation]
      public void the_row_is_removed_from_the_view()
      {
         _boundMetaData.ShouldNotContain(_outputPathDTO);
      }

      [Observation]
      public void the_output_path_row_can_be_added_again()
      {
         sut.AddOutputPathMetadata();
         _boundMetaData.Count(x => x.Name == Constants.OUTPUT_PATH).ShouldBeEqualTo(1);
      }
   }

   public class When_binding_to_a_set_of_observed_data_containing_different_molweights : concern_for_DataRepositoryMetaDataPresenter
   {
      protected override void Context()
      {
         base.Context();
         _dataColumn1.DataInfo.MolWeight = 50;
         _dataColumn2.DataInfo.MolWeight = 60;
         A.CallTo(() => _observedDataConfiguration.MolWeightAlwaysEditable).Returns(true);
         A.CallTo(() => _observedDataConfiguration.MolWeightVisible).Returns(true);
         sut.EditObservedData(_dataRepository);
      }

      [Observation]
      public void should_hide_the_molweight()
      {
         _view.MolWeightVisible.ShouldBeFalse();
      }
   }

   public class When_binding_to_single_observed_data_with_no_molweight_set : concern_for_DataRepositoryMetaDataPresenter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _observedDataConfiguration.MolWeightAlwaysEditable).Returns(true);
         A.CallTo(() => _observedDataConfiguration.MolWeightVisible).Returns(true);
         sut.EditObservedData(_dataRepository);
      }

      [Observation]
      public void should_show_the_molweight()
      {
         _view.MolWeightVisible.ShouldBeTrue();
      }
   }

   public class When_binding_to_a_set_of_observed_data_containing_same_molweight_and_molweight_can_be_edited : concern_for_DataRepositoryMetaDataPresenter
   {
      private IParameter _molWeightParameter;

      protected override void Context()
      {
         base.Context();
         _molWeightParameter = A.Fake<IParameter>();
         _dataColumn1.DataInfo.MolWeight = 50;
         _dataColumn2.DataInfo.MolWeight = 50;
         A.CallTo(() => _observedDataConfiguration.MolWeightAlwaysEditable).Returns(false);
         A.CallTo(() => _observedDataConfiguration.MolWeightVisible).Returns(true);
         A.CallTo(_parameterFactory).WithReturnType<IParameter>().Returns(_molWeightParameter);
         sut.EditObservedData(_dataRepository);
      }

      [Observation]
      public void should_allow_molweight_edit()
      {
         _view.MolWeightEditable.ShouldBeTrue();
      }

      [Observation]
      public void should_bind_to_molweight_parameter()
      {
         A.CallTo(() => _view.BindToMolWeight(_molWeightParameter)).MustHaveHappened();
      }
   }

   public class When_binding_to_a_set_of_observed_data_containing_same_molweight_and_molweight_cannot_be_edited : concern_for_DataRepositoryMetaDataPresenter
   {
      private IParameter _molWeightParameter;

      protected override void Context()
      {
         base.Context();
         _molWeightParameter = A.Fake<IParameter>();
         _dataColumn1.DataInfo.MolWeight = 50;
         _dataColumn2.DataInfo.MolWeight = 50;
         _dataRepository.ExtendedProperties.Add(new ExtendedProperty<string> { Name = "Molecule", Value = "DummyMolecule" });
         A.CallTo(() => _observedDataConfiguration.MolWeightAlwaysEditable).Returns(false);
         A.CallTo(() => _observedDataConfiguration.MolWeightVisible).Returns(true);
         A.CallTo(_parameterFactory).WithReturnType<IParameter>().Returns(_molWeightParameter);
         sut.EditObservedData(_dataRepository);
      }

      [Observation]
      public void should_show_the_molweight()
      {
         _view.MolWeightVisible.ShouldBeTrue();
      }

      [Observation]
      public void should_not_allow_molweight_edit()
      {
         _view.MolWeightEditable.ShouldBeFalse();
      }

      [Observation]
      public void should_bind_to_molweight_parameter()
      {
         A.CallTo(() => _view.BindToMolWeight(_molWeightParameter)).MustHaveHappened();
      }
   }

   public class When_binding_to_a_set_of_observed_data_containing_same_molweight_and_molweight_is_not_visible : concern_for_DataRepositoryMetaDataPresenter
   {
      private IParameter _molWeightParameter;

      protected override void Context()
      {
         base.Context();
         _molWeightParameter = A.Fake<IParameter>();
         _dataColumn1.DataInfo.MolWeight = 50;
         _dataColumn2.DataInfo.MolWeight = 50;
         A.CallTo(() => _observedDataConfiguration.MolWeightAlwaysEditable).Returns(true);
         A.CallTo(() => _observedDataConfiguration.MolWeightVisible).Returns(false);
         A.CallTo(_parameterFactory).WithReturnType<IParameter>().Returns(_molWeightParameter);
         sut.EditObservedData(_dataRepository);
      }

      [Observation]
      public void should_hide_the_molweight()
      {
         _view.MolWeightVisible.ShouldBeFalse();
      }

      [Observation]
      public void should_not_bind_to_molweight_parameter()
      {
         A.CallTo(() => _view.BindToMolWeight(_molWeightParameter)).MustNotHaveHappened();
      }
   }
   public abstract class When_binding_observed_data_with_a_subject_count : concern_for_DataRepositoryMetaDataPresenter
   {
      protected MetaDataDTO _subjectCountDTO;
      protected IEnumerable<MetaDataDTO> _metaDataDTOs;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _view.BindToSubjectCount(A<MetaDataDTO>._)).Invokes(x => _subjectCountDTO = x.GetArgument<MetaDataDTO>(0));
         A.CallTo(() => _view.BindToMetaData(A<IEnumerable<MetaDataDTO>>._)).Invokes(x => _metaDataDTOs = x.GetArgument<IEnumerable<MetaDataDTO>>(0));
         SetExtendedProperty(Constants.ObservedData.STUDY_ID, "Study");
         _dataRepository.SetSubjectCount(12);
      }
   }

   public class When_binding_a_single_observed_data_with_a_subject_count : When_binding_observed_data_with_a_subject_count
   {
      protected override void Because()
      {
         sut.EditObservedData(_dataRepository);
      }

      [Observation]
      public void should_bind_the_subject_count_to_the_view()
      {
         _subjectCountDTO.Name.ShouldBeEqualTo(Constants.ObservedData.SUBJECT_COUNT);
         _subjectCountDTO.Value.ShouldBeEqualTo("12");
      }

      [Observation]
      public void should_not_show_the_subject_count_in_the_meta_data_list()
      {
         _metaDataDTOs.Select(x => x.Name).ShouldOnlyContain(Constants.ObservedData.STUDY_ID);
      }

      [Observation]
      public void should_accept_an_empty_subject_count_to_clear_it()
      {
         _subjectCountDTO.Validate(x => x.Value, string.Empty).IsEmpty.ShouldBeTrue();
      }

      [Observation]
      public void should_reject_an_invalid_subject_count()
      {
         _subjectCountDTO.Validate(x => x.Value, "0").IsEmpty.ShouldBeFalse();
         _subjectCountDTO.Validate(x => x.Value, "2.5").IsEmpty.ShouldBeFalse();
      }
   }

   public class When_binding_multiple_observed_data_with_different_subject_counts : When_binding_observed_data_with_a_subject_count
   {
      private DataRepository _otherDataRepository;

      protected override void Context()
      {
         base.Context();
         _otherDataRepository = new DataRepository();
         _otherDataRepository.SetSubjectCount(5);
      }

      protected override void Because()
      {
         sut.EditObservedDataMetaData(new[] {_dataRepository, _otherDataRepository});
      }

      [Observation]
      public void should_bind_an_empty_subject_count_to_the_view()
      {
         _subjectCountDTO.Value.ShouldBeEqualTo(string.Empty);
      }
   }

   public class When_binding_multiple_observed_data_with_the_same_subject_count : When_binding_observed_data_with_a_subject_count
   {
      private DataRepository _otherDataRepository;

      protected override void Context()
      {
         base.Context();
         _otherDataRepository = new DataRepository();
         _otherDataRepository.SetSubjectCount(12);
      }

      protected override void Because()
      {
         sut.EditObservedDataMetaData(new[] {_dataRepository, _otherDataRepository});
      }

      [Observation]
      public void should_bind_the_shared_subject_count_to_the_view()
      {
         _subjectCountDTO.Value.ShouldBeEqualTo("12");
      }
   }

   public class When_binding_multiple_observed_data_with_the_same_subject_count_written_differently : When_binding_observed_data_with_a_subject_count
   {
      private DataRepository _otherDataRepository;

      protected override void Context()
      {
         base.Context();
         _otherDataRepository = new DataRepository();
         _otherDataRepository.ExtendedProperties.Add(new ExtendedProperty<string> {Name = Constants.ObservedData.SUBJECT_COUNT, Value = "012"});
      }

      protected override void Because()
      {
         sut.EditObservedDataMetaData(new[] {_dataRepository, _otherDataRepository});
      }

      [Observation]
      public void should_bind_the_normalized_shared_subject_count_to_the_view()
      {
         _subjectCountDTO.Value.ShouldBeEqualTo("12");
      }
   }

   public class When_binding_multiple_observed_data_where_only_some_have_a_subject_count : When_binding_observed_data_with_a_subject_count
   {
      protected override void Because()
      {
         sut.EditObservedDataMetaData(new[] {_dataRepository, new DataRepository()});
      }

      [Observation]
      public void should_bind_an_empty_subject_count_to_the_view()
      {
         _subjectCountDTO.Value.ShouldBeEqualTo(string.Empty);
      }
   }

   public class When_setting_the_subject_count : concern_for_DataRepositoryMetaDataPresenter
   {
      protected override void Because()
      {
         sut.SetSubjectCount("12");
      }

      [Observation]
      public void should_update_the_subject_count_of_the_edited_observed_data()
      {
         A.CallTo(() => _observedDataMetaDataTask.UpdateSubjectCount(A<IEnumerable<DataRepository>>.That.Contains(_dataRepository), "12")).MustHaveHappened();
      }
   }
}