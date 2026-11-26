using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Core.Events;
using OSPSuite.Core.Import;
using OSPSuite.Core.Services;
using OSPSuite.Helpers;
using OSPSuite.Infrastructure.Import.Services;

namespace OSPSuite.Presentation.Services
{
   public abstract class concern_for_ReloadObservedDataTask : ContextSpecification<ReloadObservedDataTask>
   {
      protected const string CONFIGURATION_ID = "ConfigurationId";
      protected IOSPSuiteExecutionContext _executionContext;
      protected IObservedDataImporter _observedDataImporter;
      protected IDataImporter _dataImporter;
      protected IObservedDataTask _observedDataTask;
      protected IParameterIdentificationTask _parameterIdentificationTask;
      protected IDialogCreator _dialogCreator;
      protected IProject _project;
      protected ImporterConfiguration _configuration;

      protected override void Context()
      {
         _executionContext = A.Fake<IOSPSuiteExecutionContext>();
         _observedDataImporter = A.Fake<IObservedDataImporter>();
         _dataImporter = A.Fake<IDataImporter>();
         _observedDataTask = A.Fake<IObservedDataTask>();
         _parameterIdentificationTask = A.Fake<IParameterIdentificationTask>();
         _dialogCreator = A.Fake<IDialogCreator>();
         _project = A.Fake<IProject>();
         _configuration = new ImporterConfiguration { Id = CONFIGURATION_ID };

         A.CallTo(() => _executionContext.Project).Returns(_project);
         A.CallTo(() => _project.ImporterConfigurationBy(CONFIGURATION_ID)).Returns(_configuration);

         sut = new ReloadObservedDataTask(_executionContext, _observedDataImporter, _dataImporter, _observedDataTask, _parameterIdentificationTask, _dialogCreator);
      }

      protected override void Because()
      {
         sut.ReloadAllObservedDataImportedWith(CONFIGURATION_ID);
      }

      protected static DataRepository observedData(string id, string configurationId = CONFIGURATION_ID)
      {
         var observedData = DomainHelperForSpecs.ObservedData(id);
         observedData.ConfigurationId = configurationId;
         return observedData;
      }

      protected void reloadWillReturn(IReadOnlyList<DataRepository> importedObservedData, ReloadDataSets reloadDataSets)
      {
         A.CallTo(() => _observedDataImporter.ImportFrom(_configuration)).Returns(importedObservedData);
         A.CallTo(() => _dataImporter.CalculateReloadDataSetsFromConfiguration(importedObservedData, A<IReadOnlyList<DataRepository>>._)).Returns(reloadDataSets);
      }

      protected void reloadedObservedDataMatches(DataRepository existingObservedData, DataRepository reloadedObservedData)
      {
         A.CallTo(() => _dataImporter.AreFromSameMetaDataCombination(existingObservedData, reloadedObservedData)).Returns(true);
      }
   }

   public class When_reloading_observed_data_whose_importer_configuration_is_not_in_the_project : concern_for_ReloadObservedDataTask
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _project.ImporterConfigurationBy(CONFIGURATION_ID)).Returns(null);
      }

      [Observation]
      public void should_tell_the_user_that_the_data_cannot_be_reloaded()
      {
         A.CallTo(() => _dialogCreator.MessageBoxError(Captions.Importer.ImporterConfigurationNotFoundInProject)).MustHaveHappened();
      }

      [Observation]
      public void should_not_import_any_data()
      {
         A.CallTo(() => _observedDataImporter.ImportFrom(A<ImporterConfiguration>._)).MustNotHaveHappened();
      }
   }

   public class When_reloading_observed_data : concern_for_ReloadObservedDataTask
   {
      private DataRepository _overwrittenObservedData;
      private DataRepository _deletedObservedData;
      private DataRepository _reloadedObservedData;
      private DataRepository _newObservedData;
      private IReadOnlyList<DataRepository> _comparedObservedData;
      private IReadOnlyList<DataRepository> _observedDataUpdatedInParameterIdentifications;
      private List<ObservedDataValueChangedEvent> _events;

      protected override void Context()
      {
         base.Context();
         _events = new List<ObservedDataValueChangedEvent>();
         _overwrittenObservedData = observedData("Overwritten");
         _deletedObservedData = observedData("Deleted");
         A.CallTo(() => _project.AllObservedData).Returns(new[] { _overwrittenObservedData, _deletedObservedData, observedData("Other", "OtherConfigurationId") });

         _reloadedObservedData = observedData("Reloaded");
         _reloadedObservedData.BaseGrid.Values = new[] { 4f, 5f, 6f };
         _reloadedObservedData.AllButBaseGrid().First().Values = new[] { 40f, 50f, 60f };
         _newObservedData = observedData("New");

         var importedObservedData = new[] { _reloadedObservedData, _newObservedData };
         A.CallTo(() => _observedDataImporter.ImportFrom(_configuration)).Returns(importedObservedData);
         A.CallTo(() => _dataImporter.CalculateReloadDataSetsFromConfiguration(importedObservedData, A<IReadOnlyList<DataRepository>>._))
            .Invokes(x => _comparedObservedData = x.GetArgument<IReadOnlyList<DataRepository>>(1))
            .Returns(new ReloadDataSets(new[] { _newObservedData }, new[] { _reloadedObservedData }, new[] { _deletedObservedData }));
         reloadedObservedDataMatches(_overwrittenObservedData, _reloadedObservedData);

         A.CallTo(() => _parameterIdentificationTask.UpdateParameterIdentificationsUsing(A<IReadOnlyList<DataRepository>>._))
            .Invokes(x => _observedDataUpdatedInParameterIdentifications = x.GetArgument<IReadOnlyList<DataRepository>>(0));
         A.CallTo(() => _executionContext.PublishEvent(A<ObservedDataValueChangedEvent>._))
            .Invokes(x => _events.Add(x.GetArgument<ObservedDataValueChangedEvent>(0)));
      }

      [Observation]
      public void should_compare_the_reloaded_data_with_the_observed_data_imported_with_the_same_configuration()
      {
         _comparedObservedData.ShouldOnlyContain(_overwrittenObservedData, _deletedObservedData);
      }

      [Observation]
      public void should_let_the_application_add_the_new_observed_data_to_the_project()
      {
         A.CallTo(() => _observedDataImporter.AddImportedObservedDataToProject(_newObservedData)).MustHaveHappened();
      }

      [Observation]
      public void should_delete_the_observed_data_that_was_not_reloaded()
      {
         A.CallTo(() => _observedDataTask.Delete(_deletedObservedData)).MustHaveHappened();
      }

      [Observation]
      public void should_overwrite_the_values_of_the_matching_observed_data()
      {
         _overwrittenObservedData.BaseGrid.Values.ShouldOnlyContainInOrder(4f, 5f, 6f);
         _overwrittenObservedData.AllButBaseGrid().First().Values.ShouldOnlyContainInOrder(40f, 50f, 60f);
      }

      [Observation]
      public void should_mark_the_project_as_changed()
      {
         A.CallTo(() => _executionContext.ProjectChanged()).MustHaveHappenedOnceExactly();
      }

      [Observation]
      public void should_update_the_parameter_identifications_using_the_overwritten_observed_data()
      {
         _observedDataUpdatedInParameterIdentifications.ShouldOnlyContain(_overwrittenObservedData);
      }

      [Observation]
      public void should_notify_that_the_values_of_the_overwritten_observed_data_changed()
      {
         _events.Select(x => x.ObservedData).ShouldOnlyContain(_overwrittenObservedData);
      }
   }

   public abstract class concern_for_reloading_observed_data_whose_error_type_changed : concern_for_ReloadObservedDataTask
   {
      protected const string MEASUREMENT = "Measurement";
      protected const string ERROR = "Error";
      protected const string CONCENTRATION_UNIT = "mol/l";
      protected const float RELOADED_ERROR_VALUE = 2.0f;
      protected const double MOL_WEIGHT = 300;
      protected DataRepository _existingObservedData;
      protected DataColumn _existingErrorColumn;

      protected abstract AuxiliaryType existingErrorType { get; }
      protected abstract AuxiliaryType reloadedErrorType { get; }

      protected override void Context()
      {
         base.Context();
         _existingObservedData = observedDataWithError("Existing", existingErrorType, 1.0f);
         _existingErrorColumn = _existingObservedData.AllButBaseGrid().Single(x => x.Name == ERROR);
         _existingErrorColumn.DataInfo.MolWeight = MOL_WEIGHT;
         A.CallTo(() => _project.AllObservedData).Returns(new[] { _existingObservedData });

         var reloadedObservedData = observedDataWithError("Reloaded", reloadedErrorType, RELOADED_ERROR_VALUE);
         reloadWillReturn(new[] { reloadedObservedData }, new ReloadDataSets(Enumerable.Empty<DataRepository>(), new[] { reloadedObservedData }, Enumerable.Empty<DataRepository>()));
         reloadedObservedDataMatches(_existingObservedData, reloadedObservedData);
      }

      private static DataRepository observedDataWithError(string id, AuxiliaryType errorType, float errorValue)
      {
         var observedData = new DataRepository(id) { ConfigurationId = CONFIGURATION_ID };
         var baseGrid = new BaseGrid("Time", DomainHelperForSpecs.TimeDimensionForSpecs()) { Values = new[] { 0.0f } };
         var measurement = new DataColumn(MEASUREMENT, DomainHelperForSpecs.ConcentrationDimensionForSpecs(), baseGrid) { Values = new[] { 10.0f } };
         measurement.DataInfo.DisplayUnitName = CONCENTRATION_UNIT;

         var isGeometric = errorType == AuxiliaryType.GeometricStdDev;
         var errorDimension = isGeometric ? DomainHelperForSpecs.NoDimension() : DomainHelperForSpecs.ConcentrationDimensionForSpecs();
         var error = new DataColumn(ERROR, errorDimension, baseGrid) { Values = new[] { errorValue } };
         error.DataInfo.AuxiliaryType = errorType;
         error.DataInfo.DisplayUnitName = isGeometric ? errorDimension.DefaultUnitName : CONCENTRATION_UNIT;
         measurement.AddRelatedColumn(error);

         observedData.Add(baseGrid);
         observedData.Add(measurement);
         observedData.Add(error);
         return observedData;
      }

      [Observation]
      public void should_keep_the_existing_error_column()
      {
         _existingObservedData.AllButBaseGrid().Single(x => x.Name == ERROR).ShouldBeEqualTo(_existingErrorColumn);
      }

      [Observation]
      public void should_update_the_error_values()
      {
         _existingErrorColumn.Values.ShouldOnlyContain(RELOADED_ERROR_VALUE);
      }

      [Observation]
      public void should_keep_the_molecular_weight_of_the_error_column()
      {
         _existingErrorColumn.DataInfo.MolWeight.ShouldBeEqualTo(MOL_WEIGHT);
      }

      [Observation]
      public void the_measurement_should_find_the_error_column_by_its_new_error_type()
      {
         var measurement = _existingObservedData.AllButBaseGrid().Single(x => x.Name == MEASUREMENT);
         measurement.GetRelatedColumn(reloadedErrorType).ShouldBeEqualTo(_existingErrorColumn);
         measurement.ContainsRelatedColumn(existingErrorType).ShouldBeFalse();
      }
   }

   public class When_reloading_observed_data_whose_error_type_changed_from_arithmetic_to_geometric : concern_for_reloading_observed_data_whose_error_type_changed
   {
      protected override AuxiliaryType existingErrorType => AuxiliaryType.ArithmeticStdDev;
      protected override AuxiliaryType reloadedErrorType => AuxiliaryType.GeometricStdDev;

      [Observation]
      public void the_error_column_should_be_a_dimensionless_geometric_standard_deviation()
      {
         _existingErrorColumn.DataInfo.AuxiliaryType.ShouldBeEqualTo(AuxiliaryType.GeometricStdDev);
         _existingErrorColumn.Dimension.ShouldBeEqualTo(DomainHelperForSpecs.NoDimension());
         _existingErrorColumn.DataInfo.DisplayUnitName.ShouldBeEqualTo(DomainHelperForSpecs.NoDimension().DefaultUnitName);
      }
   }

   public class When_reloading_observed_data_whose_error_type_changed_from_geometric_to_arithmetic : concern_for_reloading_observed_data_whose_error_type_changed
   {
      protected override AuxiliaryType existingErrorType => AuxiliaryType.GeometricStdDev;
      protected override AuxiliaryType reloadedErrorType => AuxiliaryType.ArithmeticStdDev;

      [Observation]
      public void the_error_column_should_be_an_arithmetic_standard_deviation_in_the_dimension_of_the_measurement()
      {
         _existingErrorColumn.DataInfo.AuxiliaryType.ShouldBeEqualTo(AuxiliaryType.ArithmeticStdDev);
         _existingErrorColumn.Dimension.ShouldBeEqualTo(DomainHelperForSpecs.ConcentrationDimensionForSpecs());
         _existingErrorColumn.DataInfo.DisplayUnitName.ShouldBeEqualTo(CONCENTRATION_UNIT);
      }
   }

   public class When_reloading_observed_data_without_overwriting_any : concern_for_ReloadObservedDataTask
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _project.AllObservedData).Returns(new DataRepository[0]);

         var newObservedData = observedData("New");
         reloadWillReturn(new[] { newObservedData }, new ReloadDataSets(new[] { newObservedData }, Enumerable.Empty<DataRepository>(), Enumerable.Empty<DataRepository>()));
      }

      [Observation]
      public void should_not_mark_the_project_as_changed()
      {
         A.CallTo(() => _executionContext.ProjectChanged()).MustNotHaveHappened();
      }

      [Observation]
      public void should_not_update_the_parameter_identifications()
      {
         A.CallTo(() => _parameterIdentificationTask.UpdateParameterIdentificationsUsing(A<IReadOnlyList<DataRepository>>._)).MustNotHaveHappened();
      }
   }
}
