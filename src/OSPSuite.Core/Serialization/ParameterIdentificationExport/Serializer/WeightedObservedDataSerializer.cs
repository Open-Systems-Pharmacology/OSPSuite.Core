using System.Globalization;
using System.Xml.Linq;
using OSPSuite.Serializer.Xml.Extensions;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Extensions;

namespace OSPSuite.Core.Serialization.ParameterIdentificationExport.Serializer
{
   public class WeightedObservedDataSerializer : ParameterIdentificationExportSerializerBase<WeightedObservedData>
   {
      public override void PerformMapping()
      {
         Map(x => x.Name);
      }

      protected override XElement TypedSerialize(WeightedObservedData weightedObservedData, ParameterIdentificationExportSerializationContext context)
      {
         var observedDataElement = base.TypedSerialize(weightedObservedData, context);
         var dataColumn = weightedObservedData.ObservedData.FirstDataColumn();
         observedDataElement.AddAttribute(ParameterIdentificationExportSchemaConstants.Attributes.Dimension, dataColumn.Dimension.Name);
         observedDataElement.AddAttribute(ParameterIdentificationExportSchemaConstants.Attributes.MolWeight, dataColumn.DataInfo.MolWeight.GetValueOrDefault(double.NaN));
         observedDataElement.AddAttribute(ParameterIdentificationExportSchemaConstants.Attributes.lloq, dataColumn.DataInfo.LLOQ.GetValueOrDefault(float.NaN));
         var pointListNode = observedDataElement.AddElement(SerializerRepository.CreateElement(ParameterIdentificationExportSchemaConstants.PointList));

         for (int i = 0; i < weightedObservedData.Count; i++)
         {
            pointListNode.Add(createPointElement(weightedObservedData, i));
         }

         return observedDataElement;
      }

      private XElement createPointElement(WeightedObservedData weightedObservedData, int i)
      {
         var element = SerializerRepository.CreateElement(ParameterIdentificationExportSchemaConstants.Point);
         var dataColumn = weightedObservedData.ObservedData.FirstDataColumn();
         element.AddAttribute(ParameterIdentificationExportSchemaConstants.Attributes.Time, dataColumn.BaseGrid[i]);
         element.AddAttribute(ParameterIdentificationExportSchemaConstants.Attributes.Value, dataColumn[i]);
         element.AddAttribute(ParameterIdentificationExportSchemaConstants.Attributes.Weight, weightedObservedData.Weights[i]);
         return element;
      }

      private string stringValueFor(double? value)
      {
         return value.GetValueOrDefault(double.NaN).ToString(NumberFormatInfo.InvariantInfo);
      }
   }
}