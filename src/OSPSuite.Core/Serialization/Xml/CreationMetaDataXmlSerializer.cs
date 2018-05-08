using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Serialization.Xml
{
   public class CreationMetaDataXmlSerializer : OSPSuiteXmlSerializer<CreationMetaData>
   {
      public override void PerformMapping()
      {
         Map(x => x.CreatedAt);
         Map(x => x.CreatedBy);
         Map(x => x.CreationMode);
         Map(x => x.Origin);
         Map(x => x.Version);
         Map(x => x.InternalVersion);
         Map(x => x.ClonedFrom);
      }
   }
}