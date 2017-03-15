using System.Xml.Linq;
using OSPSuite.Core.Serialization.SimModel.Serializer;

namespace OSPSuite.Core.Serialization.SimModel.Services
{
   public interface IExportSerializer
   {
      string Serialize<TObject>(TObject objectToSerialize);
      XElement SerializeElement<TObject>(TObject objectToSerialize);
   }

   internal class ExportSerializer : IExportSerializer
   {
      private readonly ISimModelSerializerRepository _simModelSerializerRepository;

      public ExportSerializer(ISimModelSerializerRepository simModelSerializerRepository)
      {
         _simModelSerializerRepository = simModelSerializerRepository;
      }

      public string Serialize<TObject>(TObject objectToSerialize)
      {
         return SerializeElement(objectToSerialize).ToString(SaveOptions.None);
      }

      public XElement SerializeElement<TObject>(TObject objectToSerialize)
      {
         var itemSerializer = _simModelSerializerRepository.SerializerFor(objectToSerialize);
         return itemSerializer.Serialize(objectToSerialize, new SimModelSerializationContext());
      }
   }
}