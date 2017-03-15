using System.Xml.Linq;
using OSPSuite.Serializer.Xml;
using OSPSuite.Serializer.Xml.Extensions;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Serialization.SimModel.DTO;

namespace OSPSuite.Core.Serialization.SimModel.Serializer
{
   public class VariableExportSerializer : QuantityExportSerializer<VariableExport>
   {
      public VariableExportSerializer()
         : base(SimModelSchemaConstants.InitialValueFormulaId)
      {
      }

      public override void PerformMapping()
      {
         base.PerformMapping();
         Map(x => x.ScaleFactor).AsNode();
         ElementName = SimModelSchemaConstants.Variable;
      }

      protected override XElement TypedSerialize(VariableExport variableExport,SimModelSerializationContext serializationContext)
      {
         var speciesNode = base.TypedSerialize(variableExport,serializationContext);

         //Export only if NOT allowed (SimModel default for missing attribute is: ALLOWED)
         if (!variableExport.NegativeValuesAllowed)
            speciesNode.AddAttribute(SimModelSchemaConstants.NegativeValuesAllowed, "0");

         if (variableExport.RHSIds.Count == 0)
            return speciesNode;

         var rhsListNode = speciesNode.AddElement(SerializerRepository.CreateElement(SimModelSchemaConstants.RhsFormulaList));
         foreach (var rhsId in variableExport.RHSIds)
         {
            var rhsNode = rhsListNode.AddElement(SerializerRepository.CreateElement(SimModelSchemaConstants.RhsFormula));
            rhsNode.AddAttribute(SimModelSchemaConstants.Id, rhsId.ConvertedTo<string>());
         }

         return speciesNode;
      }
   }
}