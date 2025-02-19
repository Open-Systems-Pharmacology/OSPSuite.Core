using System;
using System.Linq;
using NUnit.Framework;
using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Helpers;
using OSPSuite.Utility;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.R.Services
{
   public abstract class concern_for_DataRepositoryTask : ContextForIntegration<IDataRepositoryTask>
   {
      protected double[] _result;
      protected IPKMLPersistor _pkmlPersistor;
      protected IDimensionTask _dimensionTask;

      protected override void Context()
      {
         sut = Api.GetDataRepositoryTask();
         _dimensionTask = Api.GetDimensionTask();
         _pkmlPersistor = Api.Container.Resolve<IPKMLPersistor>();
      }
   }

   public class When_loading_a_data_repository_from_pkml_file : concern_for_DataRepositoryTask
   {
      private DataRepository _dataRepository;
      private string _dataRepositoryFile;
      private DataRepository _loadedRepository;

      protected override void Context()
      {
         base.Context();
         _dataRepository = DomainHelperForSpecs.ObservedData("TOTO", _dimensionTask.DimensionByName(Constants.Dimension.TIME), _dimensionTask.DimensionByName(Constants.Dimension.MOLAR_CONCENTRATION));
         _dataRepositoryFile = FileHelper.GenerateTemporaryFileName();
         sut.SaveDataRepository(_dataRepository, _dataRepositoryFile);
      }

      protected override void Because()
      {
         _loadedRepository = sut.LoadDataRepository(_dataRepositoryFile);
      }

      [Observation]
      public void should_return_the_expected_data_repository()
      {
         AssertForSpecs.AreEqual(_dataRepository, _loadedRepository);
      }

      public override void Cleanup()
      {
         base.Cleanup();
         FileHelper.DeleteFile(_dataRepositoryFile);
      }
   }

   public class When_retrieving_the_measurement_column_in_a_data_repository : concern_for_DataRepositoryTask
   {
      private DataRepository _dataRepository;
      private DataColumn _obsColumn;

      protected override void Context()
      {
         base.Context();
         _dataRepository = DomainHelperForSpecs.ObservedData(
            "TOTO",
            _dimensionTask.DimensionByName(Constants.Dimension.TIME),
            _dimensionTask.DimensionByName(Constants.Dimension.MOLAR_CONCENTRATION),
            "OBS_COL"
         );

         _obsColumn = _dataRepository.Columns.FindByName("OBS_COL");
      }

      [Observation]
      public void should_return_the_expected_column_if_one_is_defined()
      {
         sut.GetMeasurementColumn(_dataRepository).ShouldBeEqualTo(_obsColumn);
      }

      [Observation]
      public void should_throw_an_exception_if_multiple_columns_are_found()
      {
         _dataRepository.Add(DomainHelperForSpecs.ConcentrationColumnForObservedData(_dataRepository.BaseGrid));
         Assert.Catch<OSPSuiteException>(() => sut.GetMeasurementColumn(_dataRepository), Error.MoreThanOneMeasurementColumnFound);
      }

      [Observation]
      public void should_return_null_if_none_is_defined()
      {
         var dataRepository = new DataRepository();
         sut.GetMeasurementColumn(dataRepository).ShouldBeNull();
      }
   }

   public class When_getting_or_adding_related_column : concern_for_DataRepositoryTask
   {
      private DataRepository _dataRepository;
      private string _dataRepositoryFile;
      private DataRepository _loadedRepository;

      protected override void Context()
      {
         base.Context();
         _dataRepository = DomainHelperForSpecs.ObservedData("TOTO", _dimensionTask.DimensionByName(Constants.Dimension.TIME), _dimensionTask.DimensionByName(Constants.Dimension.MOLAR_CONCENTRATION));
         _dataRepositoryFile = FileHelper.GenerateTemporaryFileName();
         sut.SaveDataRepository(_dataRepository, _dataRepositoryFile);
         _loadedRepository = sut.LoadDataRepository(_dataRepositoryFile);
      }

      [Observation]
      public void should_create_error_column()
      {
         var col = _loadedRepository.ObservationColumns().First();
         var errorColumn = sut.AddErrorColumn(col, "yError", AuxiliaryType.ArithmeticStdDev.ToString());
         errorColumn.Name.ShouldBeEqualTo("yError");
         errorColumn.Dimension.ShouldBeEqualTo(col.Dimension);
         errorColumn.BaseGrid.ShouldBeEqualTo(_loadedRepository.BaseGrid);
         errorColumn.DisplayUnit.ShouldBeEqualTo(col.DisplayUnit);
         errorColumn.DataInfo.AuxiliaryType.ShouldBeEqualTo(AuxiliaryType.ArithmeticStdDev);
         errorColumn.DataInfo.Origin.ShouldBeEqualTo(ColumnOrigins.ObservationAuxiliary);
         col.RelatedColumns.ShouldContain(errorColumn);
      }

      [Observation]
      public void should_add_the_column_to_the_underlying_repository()
      {
         var col = _loadedRepository.ObservationColumns().First();
         var errorColumn = sut.AddErrorColumn(col, "yError", AuxiliaryType.ArithmeticStdDev.ToString());
         _loadedRepository.Columns.ShouldContain(errorColumn);
      }

      [Observation]
      public void should_create_error_column_with_geometric_error()
      {
         var col = _loadedRepository.ObservationColumns().First();
         var name = new ShortGuid().ToString();
         var errorColumn = sut.AddErrorColumn(col, name, AuxiliaryType.GeometricStdDev.ToString());
         errorColumn.Name.ShouldBeEqualTo(name);
         errorColumn.Dimension.ShouldBeEqualTo(Api.GetDimensionTask().DimensionByName("Dimensionless"));
         errorColumn.BaseGrid.ShouldBeEqualTo(_loadedRepository.BaseGrid);
         errorColumn.DataInfo.AuxiliaryType.ShouldBeEqualTo(AuxiliaryType.GeometricStdDev);
         errorColumn.DataInfo.Origin.ShouldBeEqualTo(ColumnOrigins.ObservationAuxiliary);
         col.RelatedColumns.ShouldContain(errorColumn);
      }

      [Observation]
      public void should_throw_on_bad_error_type()
      {
         var col = _loadedRepository.ObservationColumns().First();
         var name = new ShortGuid().ToString();
         The.Action(() => sut.AddErrorColumn(col, name, "something else")).ShouldThrowAn<OSPSuiteException>();
      }

      [Observation]
      public void should_get_error_column()
      {
         var col = _loadedRepository.ObservationColumns().First();
         var errorColumn = sut.AddErrorColumn(col, "", AuxiliaryType.ArithmeticStdDev.ToString());
         sut.GetErrorColumn(col).ShouldBeEqualTo(errorColumn);
      }

      public override void Cleanup()
      {
         base.Cleanup();
         FileHelper.DeleteFile(_dataRepositoryFile);
      }
   }

   public class When_adding_metadata : concern_for_DataRepositoryTask
   {
      private DataRepository _dataRepository;

      protected override void Context()
      {
         base.Context();
         _dataRepository = DomainHelperForSpecs.ObservedData("TOTO", _dimensionTask.DimensionByName(Constants.Dimension.TIME), _dimensionTask.DimensionByName(Constants.Dimension.MOLAR_CONCENTRATION));
      }

      [Observation]
      public void should_create_meta_data()
      {
         sut.AddMetaData(_dataRepository, "meta_data", "value");
         _dataRepository.ExtendedPropertyValueFor("meta_data").ShouldBeEqualTo("value");
      }

      [Observation]
      public void should_update_the_meta_data_if_it_already_exists()
      {
         sut.AddMetaData(_dataRepository, "meta_data", "value");
         sut.AddMetaData(_dataRepository, "meta_data", "updated_value");
         _dataRepository.ExtendedPropertyValueFor("meta_data").ShouldBeEqualTo("updated_value");
      }
   }

   public class When_removing_metadata : concern_for_DataRepositoryTask
   {
      private DataRepository _dataRepository;

      protected override void Context()
      {
         base.Context();
         _dataRepository = DomainHelperForSpecs.ObservedData("TOTO");
         sut.AddMetaData(_dataRepository, "meta_data", "value");
      }

      [Observation]
      public void should_not_crash_if_the_meta_data_by_key_does_not_exist()
      {
         sut.RemoveMetaData(_dataRepository, "NOPE");
      }

      [Observation]
      public void should_update_the_meta_data_if_it_already_exists()
      {
         sut.RemoveMetaData(_dataRepository, "meta_data");
         _dataRepository.ExtendedProperties.Contains("meta_data").ShouldBeFalse();
      }
   }

   public class When_removing_a_column_from_a_data_repository : concern_for_DataRepositoryTask
   {
      private DataRepository _dataRepository;

      protected override void Context()
      {
         base.Context();
         _dataRepository = DomainHelperForSpecs.ObservedData();
      }

      [Observation]
      public void should_throw_an_exception_if_removing_a_base_grid_and_the_data_repository_has_other_column()
      {
         var baseGrid = _dataRepository.BaseGrid;
         The.Action(() => sut.RemoveColumn(_dataRepository, baseGrid)).ShouldThrowAn<OSPSuiteException>();
      }

      [Observation]
      public void should_remove_the_base_grid_if_it_is_not_used_anywhere()
      {
         var baseGrid = _dataRepository.BaseGrid;
         _dataRepository.AllButBaseGridAsArray.Each(x => sut.RemoveColumn(_dataRepository, x));
         sut.RemoveColumn(_dataRepository, baseGrid);
         _dataRepository.Columns.ShouldBeEmpty();
      }

      [Observation]
      public void should_remove_the_column_and_related_column_associated_with_the_column()
      {
         var baseGrid = _dataRepository.BaseGrid;
         var column = DomainHelperForSpecs.ConcentrationColumnForObservedData(baseGrid);
         var errorColumn = DomainHelperForSpecs.ConcentrationColumnForObservedData(baseGrid);
         errorColumn.DataInfo.AuxiliaryType = AuxiliaryType.ArithmeticStdDev;
         column.AddRelatedColumn(errorColumn);
         _dataRepository.Add(column);
         //2 from the creation and new columns + error = 4
         _dataRepository.Columns.Count().ShouldBeEqualTo(4);
         sut.RemoveColumn(_dataRepository, column);
         _dataRepository.Columns.Contains(column).ShouldBeFalse();
         _dataRepository.Columns.Contains(errorColumn).ShouldBeFalse();
      }

      [Observation]
      public void should_remove_the_column_from_the_related_column_association_if_it_is_being_used_as_association()
      {
         var baseGrid = _dataRepository.BaseGrid;
         var column = DomainHelperForSpecs.ConcentrationColumnForObservedData(baseGrid);
         var errorColumn = DomainHelperForSpecs.ConcentrationColumnForObservedData(baseGrid);
         errorColumn.DataInfo.AuxiliaryType = AuxiliaryType.ArithmeticStdDev;
         column.AddRelatedColumn(errorColumn);
         _dataRepository.Add(column);
         //2 from the creation and new columns + error = 4
         _dataRepository.Columns.Count().ShouldBeEqualTo(4);
         sut.RemoveColumn(_dataRepository, errorColumn);
         _dataRepository.Columns.Contains(errorColumn).ShouldBeFalse();
         column.RelatedColumns.ShouldBeEmpty();
      }
   }

   public class When_setting_the_value_origin_of_a_predefined_column_to_a_valid_value : concern_for_DataRepositoryTask
   {
      private DataRepository _dataRepository;
      private DataColumn _column;

      protected override void Context()
      {
         base.Context();
         _dataRepository = DomainHelperForSpecs.ObservedData();
         var baseGrid = _dataRepository.BaseGrid;
         _column = DomainHelperForSpecs.ConcentrationColumnForObservedData(baseGrid);
      }

      [TestCase("Calculation", ColumnOrigins.Calculation)]
      [TestCase("CalculationAuxiliary", ColumnOrigins.CalculationAuxiliary)]
      [TestCase("Observation", ColumnOrigins.Observation)]
      public void should_set_the_value_as_expected(string value, ColumnOrigins origin)
      {
         sut.SetColumnOrigin(_column, value);
         _column.DataInfo.Origin.ShouldBeEqualTo(origin);
      }
   }

   public class When_setting_the_value_origin_of_a_predefined_column_to_an_invalid_value : concern_for_DataRepositoryTask
   {
      private DataRepository _dataRepository;
      private DataColumn _column;

      protected override void Context()
      {
         base.Context();
         _dataRepository = DomainHelperForSpecs.ObservedData();
         var baseGrid = _dataRepository.BaseGrid;
         _column = DomainHelperForSpecs.ConcentrationColumnForObservedData(baseGrid);
      }

      [Observation]
      public void should_throw_an_error()
      {
         The.Action(() => sut.SetColumnOrigin(_column, "TOTO")).ShouldThrowAn<Exception>();
      }
   }

   public class When_creating_a_default_observation_repository : concern_for_DataRepositoryTask
   {
      private DataRepository _dataRepository;
      private DataColumn _column;

      protected override void Because()
      {
         _dataRepository = sut.CreateEmptyObservationRepository("xValue", "yValue");
         _column = _dataRepository.FindByName("yValue");
      }

      [Observation]
      public void should_return_a_repository_with_two_columns_having_the_expected_names()
      {
         _dataRepository.FindByName("xValue").ShouldNotBeNull();
         _column.ShouldNotBeNull();
      }

      [Observation]
      public void the_type_of_the_column_should_be_observation()
      {
         _column.DataInfo.Origin.ShouldBeEqualTo(ColumnOrigins.Observation);
      }

      [Observation]
      public void the_dimension_of_the_column_should_be_concentration_mass()
      {
         _column.DimensionName().ShouldBeEqualTo(Constants.Dimension.MASS_CONCENTRATION);
      }
   }
}