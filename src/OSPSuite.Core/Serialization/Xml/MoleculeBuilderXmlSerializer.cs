using System.Xml.Linq;
using OSPSuite.Serializer;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Serialization.Xml.Extensions;

namespace OSPSuite.Core.Serialization.Xml
{
   public class MoleculeBuilderXmlSerializer<T> : ContainerXmlSerializer<T> where T : class, IMoleculeBuilder
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         Map(x => x.IsFloating);
         Map(x => x.QuantityType);
         Map(x => x.IsXenobiotic);
         Map(x => x.Dimension).WithMappingName(Constants.Serialization.Attribute.Dimension);
         MapReference(x => x.DefaultStartFormula);
         MapEnumerable(x => x.UsedCalculationMethods, x => x.AddUsedCalculationMethod);
      }
   }

   public class MoleculeBuilderXmlSerializer : MoleculeBuilderXmlSerializer<MoleculeBuilder>
   {
      protected override void TypedDeserialize(MoleculeBuilder moleculeBuilder, XElement element, SerializationContext serializationContext)
      {
         base.TypedDeserialize(moleculeBuilder, element,serializationContext);
         element.UpdateDisplayUnit(moleculeBuilder);
      }

      protected override XElement TypedSerialize(MoleculeBuilder moleculeBuilder, SerializationContext serializationContext)
      {
         var element= base.TypedSerialize(moleculeBuilder, serializationContext);
         return element.AddDisplayUnitFor(moleculeBuilder);
      }
   }
}