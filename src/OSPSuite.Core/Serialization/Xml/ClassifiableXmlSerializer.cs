using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;

namespace OSPSuite.Core.Serialization.Xml
{
   public class ClassifiableXmlSerializer<T, U> : OSPSuiteXmlSerializer<T>
      where T : Classifiable<U>
      where U : IWithId, IWithName
   {
      public override void PerformMapping()
      {
         Map(x => x.Id);
         MapReference(x => x.Parent);
      }
   }

   public class ClassifiableDataRepositoryXmlSerializer : ClassifiableXmlSerializer<ClassifiableObservedData, DataRepository>
   {
   }
}