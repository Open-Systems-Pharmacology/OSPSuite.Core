using System;
using System.Xml.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;

namespace OSPSuite.Core.Serialization.Xml
{
   public class DataRepositoryXmlSerializer : OSPSuiteXmlSerializer<DataRepository>
   {
      public DataRepositoryXmlSerializer() : base(Constants.Serialization.DATA_REPOSITORY)
      {
      }

      public override void PerformMapping()
      {
         Map(x => x.Id);
         Map(x => x.Name);
         Map(x => x.Icon);
         Map(x => x.Description);
         Map(x => x.ConfigurationId);
         MapEnumerable(x => x.Columns, x => x.Add);
         MapEnumerable(x => x.ExtendedProperties, addFunction);
      }

      private Action<IExtendedProperty> addFunction(DataRepository dataRepository)
      {
         return x =>
         {
            if (string.IsNullOrEmpty(x.Name))
               x.Name = $"Unnamed Property {dataRepository.ExtendedProperties.Count}";
            dataRepository.ExtendedProperties.Add(x);
         };
      }

      protected override void TypedDeserialize(DataRepository dataRepository, XElement outputToDeserialize, SerializationContext serializationContext)
      {
         base.TypedDeserialize(dataRepository, outputToDeserialize, serializationContext);
         serializationContext.Register(dataRepository);
      }
   }
}