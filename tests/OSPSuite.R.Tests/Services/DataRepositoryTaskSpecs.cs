using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Helpers;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Helpers;
using OSPSuite.Utility;
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
         var errorColumn = sut.AddErrorColumn(col);
         errorColumn.Name.ShouldBeEqualTo("yError");
         errorColumn.Dimension.ShouldBeEqualTo(col.Dimension);
         errorColumn.BaseGrid.ShouldBeEqualTo(_loadedRepository.BaseGrid);
         errorColumn.DisplayUnit.ShouldBeEqualTo(col.DisplayUnit);
         errorColumn.DataInfo.AuxiliaryType.ShouldBeEqualTo(AuxiliaryType.ArithmeticStdDev);
         errorColumn.DataInfo.Origin.ShouldBeEqualTo(ColumnOrigins.ObservationAuxiliary);
         col.RelatedColumns.ShouldContain(errorColumn);
      }

      [Observation]
      public void should_create_error_column_with_name()
      {
         var col = _loadedRepository.ObservationColumns().First();
         var name = new ShortGuid().ToString();
         var errorColumn = sut.AddErrorColumn(col, name);
         errorColumn.Name.ShouldBeEqualTo(name);
         errorColumn.Dimension.ShouldBeEqualTo(col.Dimension);
         errorColumn.BaseGrid.ShouldBeEqualTo(_loadedRepository.BaseGrid);
         errorColumn.DisplayUnit.ShouldBeEqualTo(col.DisplayUnit);
         errorColumn.DataInfo.AuxiliaryType.ShouldBeEqualTo(AuxiliaryType.ArithmeticStdDev);
         errorColumn.DataInfo.Origin.ShouldBeEqualTo(ColumnOrigins.ObservationAuxiliary);
         col.RelatedColumns.ShouldContain(errorColumn);
      }

      [Observation]
      public void should_get_error_column()
      {
         var col = _loadedRepository.ObservationColumns().First();
         var errorColumn = sut.AddErrorColumn(col);
         sut.GetErrorColumn(col).ShouldBeEqualTo(errorColumn);
      }

      public override void Cleanup()
      {
         base.Cleanup();
         FileHelper.DeleteFile(_dataRepositoryFile);
      }
   }
}