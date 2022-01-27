using System.Collections.Generic;
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

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_DataRepositoryMetaDataPresenter : ContextSpecification<DataRepositoryMetaDataPresenter>
   {
      protected IEditObservedDataTask _editObservedDataTask;
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
         _editObservedDataTask = A.Fake<IEditObservedDataTask>();
         _view = A.Fake<IDataRepositoryMetaDataView>();
         _observedDataConfiguration = A.Fake<IObservedDataConfiguration>();
         _dataRepository = new DataRepository();
         _dimensionFactory = A.Fake<IDimensionFactory>();
         _parameterFactory = A.Fake<IParameterFactory>();
         sut = new DataRepositoryMetaDataPresenter(_view, _editObservedDataTask, _observedDataConfiguration, _parameterFactory, _dimensionFactory);
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
         A.CallTo(() => _editObservedDataTask.AddMetaData(A<IEnumerable<DataRepository>>.Ignored, A<MetaDataKeyValue>.Ignored)).MustNotHaveHappened();
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
         A.CallTo(() => _editObservedDataTask.AddMetaData(A<IEnumerable<DataRepository>>.That.Contains(_dataRepository), A<MetaDataKeyValue>.That.Matches(x => x.Value.Equals("onlyValue") && x.Key.Equals("onlyName")))).MustHaveHappened();
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
         A.CallTo(() => _editObservedDataTask.ChangeMetaData(A<IEnumerable<DataRepository>>.That.Contains(_dataRepository), A<MetaDataChanged>.Ignored)).MustHaveHappened();
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
         A.CallTo(() => _editObservedDataTask.AddMetaData(A<IEnumerable<DataRepository>>.That.Contains(_dataRepository), A<MetaDataKeyValue>.Ignored)).MustHaveHappened();
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
         A.CallTo(() => _editObservedDataTask.ChangeMetaData(A<IEnumerable<DataRepository>>.That.Contains(_dataRepository), A<MetaDataChanged>.Ignored)).MustHaveHappened();
      }
   }

   public class When_binding_to_a_set_of_observed_data_containing_different_molweights : concern_for_DataRepositoryMetaDataPresenter
   {
      protected override void Context()
      {
         base.Context();
         _dataColumn1.DataInfo.MolWeight = 50;
         _dataColumn2.DataInfo.MolWeight = 60;
         A.CallTo(() => _observedDataConfiguration.MolWeightEditable).Returns(true);
         A.CallTo(() => _observedDataConfiguration.MolWeightVisible).Returns(true);
         sut.EditObservedData(_dataRepository);
      }

      [Observation]
      public void should_hide_the_molweight()
      {
         _view.MolWeightVisible.ShouldBeFalse();
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
         A.CallTo(() => _observedDataConfiguration.MolWeightEditable).Returns(true);
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
         A.CallTo(() => _observedDataConfiguration.MolWeightEditable).Returns(false);
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
         A.CallTo(() => _observedDataConfiguration.MolWeightEditable).Returns(true);
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
}