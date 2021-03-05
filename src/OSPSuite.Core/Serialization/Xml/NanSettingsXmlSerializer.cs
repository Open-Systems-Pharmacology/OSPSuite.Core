using OSPSuite.Core.Import;
using System;
using System.Xml.Linq;
using static OSPSuite.Core.Import.NanSettings;

namespace OSPSuite.Core.Serialization.Xml
{
   public class NanSettingsXmlSerializer : OSPSuiteXmlSerializer<NanSettings>
   {
      public override void PerformMapping()
      {
         Map(x => x.Indicator);
         Map(x => x.Action);
      }
   }

   public class ActionTypeXmlSerializer : OSPSuiteXmlSerializer<ActionType>
   {
      protected ActionTypeXmlSerializer() : base($"{typeof(ActionType).Name}") {}

      public override void PerformMapping()
      {
      }

      protected override XElement TypedSerialize(ActionType action, SerializationContext serializationContext)
      {
         var element = SerializerRepository.CreateElement(ElementName);
         element.SetValue((int)action);
         return element;
      }

      protected virtual string StringValueFor(ActionType action)
      {
         return action.ToString();
      }

      public override ActionType CreateObject(XElement element, SerializationContext serializationContext)
      {
         return (ActionType)int.Parse(element.Value);
      }
   }
}
