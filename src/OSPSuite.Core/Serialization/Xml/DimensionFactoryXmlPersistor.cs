using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Utility.Container;

namespace OSPSuite.Core.Serialization.Xml
{
   public interface IDimensionFactoryPersistor : IFilePersistor<IDimensionFactory>
   {
   }

   public class DimensionFactoryPersistor : AbstractFilePersistor<IDimensionFactory>, IDimensionFactoryPersistor
   {
      public DimensionFactoryPersistor(IUnitSystemXmlSerializerRepository serializerRepository, IContainer container) : base(serializerRepository, container)
      {
      }
   }
}