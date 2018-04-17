using System.Xml.Linq;
using OSPSuite.Core.Serialization.SimModel.DTO;
using OSPSuite.Serializer.Xml.Extensions;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Serialization.SimModel.Serializer
{
   public class TableFormulaWithOffsetExportSerializer : FormulaExportXmlSerializerBase<TableFormulaWithOffsetExport>
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         ElementName = SimModelSchemaConstants.TableFormulaWithOffset;
      }

      protected override XElement TypedSerialize(TableFormulaWithOffsetExport tableFormulaWithOffsetExport, SimModelSerializationContext serializationContext)
      {
         var formulaNode = base.TypedSerialize(tableFormulaWithOffsetExport, serializationContext);

         var tableObjectNode = SerializerRepository.CreateElement(SimModelSchemaConstants.TableObject);
         tableObjectNode.AddAttribute(SimModelSchemaConstants.Id, tableFormulaWithOffsetExport.TableObjectId.ConvertedTo<string>());
         formulaNode.Add(tableObjectNode);

         var offsetObjectNode = SerializerRepository.CreateElement(SimModelSchemaConstants.OffsetObject);
         offsetObjectNode.AddAttribute(SimModelSchemaConstants.Id, tableFormulaWithOffsetExport.OffsetObjectId.ConvertedTo<string>());
         formulaNode.Add(offsetObjectNode);

         return formulaNode;
      }
   }
}