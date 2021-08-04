using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Helpers;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Helpers;
using OSPSuite.Utility;
using OSPSuite.Utility.Exceptions;
using System;
using System.Linq;

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
      public void should_update_the_meta_data_if_it_already_exists ()
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
}