using System.Collections.Generic;
using System.Xml.Linq;
using OSPSuite.Serializer.Xml.Extensions;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Serialization.SimModel.DTO;

namespace OSPSuite.Core.Serialization.SimModel.Serializer
{
   public class ExplicitFormulaExportSerializer : FormulaExportXmlSerializerBase<ExplicitFormulaExport>
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         ElementName = SimModelSchemaConstants.ExplicitFormula;
      }

      protected override XElement TypedSerialize(ExplicitFormulaExport explicitFormulaExport, SimModelSerializationContext serializationContext)
      {
         var formulaNode = base.TypedSerialize(explicitFormulaExport,serializationContext);
         var equationNode = SerializerRepository.CreateElement(SimModelSchemaConstants.Equation);
         equationNode.Value = explicitFormulaExport.Equation;
         formulaNode.Add(equationNode);
         addReferenceNodeFor(formulaNode, explicitFormulaExport.ReferenceList, SimModelSchemaConstants.ReferenceList, SimModelSchemaConstants.Reference);

         return formulaNode;
      }

      private void addReferenceNodeFor(XElement formulaNode, IDictionary<string, int> referenceDictionary, string enumerationNodeName, string referenceNodeName)
      {
         if (referenceDictionary.Count == 0)
            return;

         var enumerationNode = SerializerRepository.CreateElement(enumerationNodeName);
         foreach (var aliasIdPair in referenceDictionary)
         {
            var referenceNode = SerializerRepository.CreateElement(referenceNodeName);
            referenceNode.AddAttribute(SimModelSchemaConstants.Alias, aliasIdPair.Key);
            referenceNode.AddAttribute(SimModelSchemaConstants.Id, aliasIdPair.Value.ConvertedTo<string>());
            enumerationNode.Add(referenceNode);
         }
         formulaNode.Add(enumerationNode);
      }
   }
}