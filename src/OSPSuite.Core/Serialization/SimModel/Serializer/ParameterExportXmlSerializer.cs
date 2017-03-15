using System.Xml.Linq;
using OSPSuite.Serializer.Xml.Extensions;
using OSPSuite.Core.Serialization.SimModel.DTO;

namespace OSPSuite.Core.Serialization.SimModel.Serializer
{
   internal class ParameterExportXmlSerializer : QuantityExportSerializer<ParameterExport>
   {
      public ParameterExportXmlSerializer(): base(SimModelSchemaConstants.FormulaId)
      {
      }

      public override void PerformMapping()
      {
         base.PerformMapping();
         ElementName = SimModelSchemaConstants.Parameter;
      }

      protected override XElement TypedSerialize(ParameterExport parameterExport,SimModelSerializationContext serializationContext)
      {
         var parameterNode = base.TypedSerialize(parameterExport,serializationContext);
         //do not write default value for paramter.CanBeVaried which is almost always true
         
         if (!parameterExport.CanBeVaried)
            parameterNode.AddAttribute(SimModelSchemaConstants.CanBeVaried, "0");

         return parameterNode;

      }
   }
}