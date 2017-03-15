using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Serialization.Xml
{
   public interface IDimensionFactoryPersistor : IFilePersistor<IDimensionFactory>
   {
   }

   public class DimensionFactoryPersistor : AbstractFilePersistor<IDimensionFactory>, IDimensionFactoryPersistor
   {
      public DimensionFactoryPersistor(IUnitSystemXmlSerializerRepository serializerRepository) : base(serializerRepository)
      {
      }
   }
}