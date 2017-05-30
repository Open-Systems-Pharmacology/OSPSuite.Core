using System;
using OSPSuite.Serializer;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;

namespace OSPSuite.Core.Serialization.Xml
{
   public class DataInfoXmlSerializer : OSPSuiteXmlSerializer<DataInfo>
   {
      public DataInfoXmlSerializer() : base(Constants.Serialization.DATA_INFO)
      {
      }

      public override void PerformMapping()
      {
         Map(x => x.Origin);
         Map(x => x.AuxiliaryType);
         Map(x => x.DisplayUnitName);
         Map(x => x.Date);
         Map(x => x.Source);
         Map(x => x.Category);
         Map(x => x.MolWeight);
         Map(x => x.LLOQ).WithMappingName(Constants.Serialization.Attribute.LLOQ);
         Map(x => x.ComparisonThreshold);
         MapEnumerable(x => x.ExtendedProperties, addFunction);
      }

      private Action<IExtendedProperty> addFunction(DataInfo dataInfo)
      {
         return dataInfo.ExtendedProperties.Add;
      }
   }
}