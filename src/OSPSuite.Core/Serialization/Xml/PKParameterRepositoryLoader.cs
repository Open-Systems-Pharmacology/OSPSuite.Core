using System.Xml.Linq;
using OSPSuite.Core.Domain.PKAnalyses;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Serialization.Xml
{
   public interface IPKParameterRepositoryLoader
   {
      void Load(IPKParameterRepository pkParameterMetaDataRepository, string fileName);
   }

   public class PKParameterRepositoryLoader : IPKParameterRepositoryLoader
   {
      private readonly IDimensionFactory _dimensionFactory;
      private readonly IOSPSuiteXmlSerializerRepository _modellingXmlSerializerRepository;

      public PKParameterRepositoryLoader(IDimensionFactory dimensionFactory, IOSPSuiteXmlSerializerRepository modellingXmlSerializerRepository)
      {
         _dimensionFactory = dimensionFactory;
         _modellingXmlSerializerRepository = modellingXmlSerializerRepository;
      }

      public void Load(IPKParameterRepository pkParameterMetaDataRepository, string fileName)
      {

         using (var serializationContext = SerializationTransaction.Create(_dimensionFactory))
         {
            var serializer = _modellingXmlSerializerRepository.SerializerFor(pkParameterMetaDataRepository);
            var xel = XElement.Load(fileName);
            serializer.Deserialize(pkParameterMetaDataRepository, xel, serializationContext);
         }
      }
   }
}