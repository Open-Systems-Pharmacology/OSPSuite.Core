using System;
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
         MapEnumerable(x => x.Columns, x => x.Add);
         MapEnumerable(x => x.ExtendedProperties, addFunction);
      }

      private Action<IExtendedProperty> addFunction(DataRepository dataRepository)
      {
         return dataRepository.ExtendedProperties.Add;
      }
   }
}