using System.Xml.Linq;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Serialization.Xml
{
   public abstract class ObjectBaseXmlSerializer<T> : OSPSuiteXmlSerializer<T> where T : IObjectBase //, new()
   {
      protected ObjectBaseXmlSerializer()
      {
      }

      protected ObjectBaseXmlSerializer(string name) : base(name)
      {
      }

      public override void PerformMapping()
      {
         Map(x => x.Id);
         Map(x => x.Name);
         Map(x => x.Description);
         Map(x => x.Icon);
      }

      //method used in Deserialization by our framework
      public override T CreateObject(XElement outputToDeserialize, SerializationContext serializationContext)
      {
         return serializationContext.ObjectFactory.CreateObjectBaseFrom<T>(typeof(T));
      }

      protected override void TypedDeserialize(T objectToDeserialize, XElement outputToDeserialize, SerializationContext serializationContext)
      {
         base.TypedDeserialize(objectToDeserialize, outputToDeserialize, serializationContext);
         serializationContext.Register(objectToDeserialize);
      }
   }
}