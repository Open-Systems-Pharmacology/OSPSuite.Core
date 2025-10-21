using System.Xml.Linq;
using OSPSuite.Core.Domain.PKAnalyses;
using OSPSuite.Core.Domain.UnitSystem;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace OSPSuite.Core.Serialization.Xml
{
   public interface IPKParameterRepositoryLoader
   {
      void Load(IPKParameterRepository pkParameterMetaDataRepository, string fileName);
   }

   public class PKParameterRepositoryLoader : IPKParameterRepositoryLoader
   {
      private readonly IDimensionFactory _dimensionFactory;
      private readonly IOSPSuiteXmlSerializerRepository _modelingXmlSerializerRepository;
      private readonly IContainer _container;

      public PKParameterRepositoryLoader(
         IDimensionFactory dimensionFactory, 
         IOSPSuiteXmlSerializerRepository modelingXmlSerializerRepository, 
         IContainer container)
      {
         _dimensionFactory = dimensionFactory;
         _modelingXmlSerializerRepository = modelingXmlSerializerRepository;
         _container = container;
      }

      public void Load(IPKParameterRepository pkParameterMetaDataRepository, string fileName)
      {

         using (var serializationContext = SerializationTransaction.Create(_container, _dimensionFactory))
         {
            var serializer = _modelingXmlSerializerRepository.SerializerFor(pkParameterMetaDataRepository);
            var xel = XElementSerializer.PermissiveLoad(fileName);
            serializer.Deserialize(pkParameterMetaDataRepository, xel, serializationContext);
         }
      }
   }
}