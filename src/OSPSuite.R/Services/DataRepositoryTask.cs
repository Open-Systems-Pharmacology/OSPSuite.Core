using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Serialization.Xml;

namespace OSPSuite.R.Services
{
   public interface IDataRepositoryTask
   {
      DataRepository LoadDataRepository(string fileName);

      void SaveDataRepository(DataRepository dataRepository, string fileName);
   }

   public class DataRepositoryTask : IDataRepositoryTask
   {
      private readonly IPKMLPersistor _pkmlPersistor;

      public DataRepositoryTask(IPKMLPersistor pkmlPersistor)
      {
         _pkmlPersistor = pkmlPersistor;
      }

      public DataRepository LoadDataRepository(string fileName)
      {
         return _pkmlPersistor.Load<DataRepository>(fileName);
      }

      public void SaveDataRepository(DataRepository dataRepository, string fileName)
      {
         _pkmlPersistor.SaveToPKML(dataRepository, fileName);
      }
   }
}