using System.Xml.Linq;
using OSPSuite.Core.Serialization.SimModel.DTO;
using OSPSuite.Serializer.Xml.Extensions;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Serialization.SimModel.Serializer
{
   public class TableFormulaWithXArgumentExportSerializer : FormulaExportXmlSerializerBase<TableFormulaWithXArgumentExport>
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         ElementName = SimModelSchemaConstants.TableFormulaWithXArgument;
      }

      protected override XElement TypedSerialize(TableFormulaWithXArgumentExport tableFormulaWithXArgumentExport, SimModelSerializationContext serializationContext)
      {
         var formulaNode = base.TypedSerialize(tableFormulaWithXArgumentExport, serializationContext);

         var tableObjectNode = SerializerRepository.CreateElement(SimModelSchemaConstants.TableObject);
         tableObjectNode.AddAttribute(SimModelSchemaConstants.Id, tableFormulaWithXArgumentExport.TableObjectId);
         formulaNode.Add(tableObjectNode);

         var xArgumentObject = SerializerRepository.CreateElement(SimModelSchemaConstants.XArgumentObject);
         xArgumentObject.AddAttribute(SimModelSchemaConstants.Id, tableFormulaWithXArgumentExport.XArgumentObjectId);
         formulaNode.Add(xArgumentObject);

         return formulaNode;
      }
   }
}