using System.Xml.Linq;
using OSPSuite.Serializer.Xml.Extensions;
using OSPSuite.Core.Serialization.SimModel.DTO;

namespace OSPSuite.Core.Serialization.SimModel.Serializer
{
   public abstract class QuantityExportSerializer<TQuantity> : SimModelSerializerBase<TQuantity> where TQuantity : QuantityExport
   {
      private readonly string _formulaIdAttribute;

      protected QuantityExportSerializer(string formulaIdAttribute)
      {
         _formulaIdAttribute = formulaIdAttribute;
      }

      public override void PerformMapping()
      {
         Map(x => x.Id);
         Map(x => x.EntityId);
         Map(x => x.Name);
         Map(x => x.Path);
         Map(x => x.Unit);
         Map(x => x.Persistable);
         Map(x => x.Value);
      }

      protected override XElement TypedSerialize(TQuantity quantityExport, SimModelSerializationContext serializationContext)
      {
         var quantityExportNode = base.TypedSerialize(quantityExport,serializationContext);
         if (quantityExport.Value == null)
         {
            quantityExportNode.AddAttribute(_formulaIdAttribute, quantityExport.FormulaId.ToString());
         }

         return quantityExportNode;
      }
   }
}