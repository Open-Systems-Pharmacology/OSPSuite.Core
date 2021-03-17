using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Helpers;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Helpers;
using OSPSuite.Utility;

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
         _pkmlPersistor.SaveToPKML(_dataRepository, _dataRepositoryFile);
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
}