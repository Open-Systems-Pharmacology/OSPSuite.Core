using OSPSuite.Serializer.Xml;

namespace OSPSuite.Core.Serialization.SimModel.Serializer
{
   public interface ISimModelSerializer : IXmlSerializer
   {
   }

   public interface ISimModelXmlSerializer<TObject> : IXmlSerializer<TObject>, ISimModelSerializer
   {
   }
}