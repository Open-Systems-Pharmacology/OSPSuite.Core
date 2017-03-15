namespace OSPSuite.Core.Serialization
{
   public interface ISerializationTask
   {
      void SaveModelPart<T>(T objectToSerialize, string filename);
      T Load<T>(string fileName, bool resetIds = false); 
   }
}