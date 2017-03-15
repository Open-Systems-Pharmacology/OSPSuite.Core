using OSPSuite.Serializer.Xml;

namespace OSPSuite.Core.Serialization.SimModel.Serializer
{
   public abstract class SimModelSerializerBase<TObject> : XmlSerializer<TObject, SimModelSerializationContext>, ISimModelSerializer
   {
   }
}