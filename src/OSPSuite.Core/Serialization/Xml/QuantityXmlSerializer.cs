using System.Globalization;
using System.Xml.Linq;
using OSPSuite.Serializer;
using OSPSuite.Serializer.Xml.Extensions;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Serialization.Xml.Extensions;

namespace OSPSuite.Core.Serialization.Xml
{
   /// <summary>
   ///    Do not inherit from FormulaUsableXmlSerializer, because Value may be serialized only for fixed values
   ///    in case of Parameters in MoleculeBuilder resp. ParameterBuilders or SpatialStructure.
   ///    Then Value cannot be evaluated always since ObjectPaths cannot be resolved to references.
   /// </summary>
   /// <typeparam name="T"></typeparam>
   public abstract class QuantityXmlSerializer<T> : EntityXmlSerializer<T> where T : IQuantity
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         Map(x => x.Persistable);
         Map(x => x.IsFixedValue).WithMappingName(Constants.Serialization.Attribute.IS_FIXED_VALUE);
         Map(x => x.Dimension).WithMappingName(Constants.Serialization.Attribute.Dimension);
         Map(x => x.QuantityType);
         Map(x => x.NegativeValuesAllowed);
         Map(x => x.ValueOrigin);
         MapReference(x => x.Formula).WithMappingName(Constants.Serialization.Attribute.FORMULA);
      }

      protected override XElement TypedSerialize(T quantity, SerializationContext serializationContext)
      {
         var quantityElement = base.TypedSerialize(quantity,serializationContext);

         if (quantity.IsFixedValue)
            addQuantityValue(quantity, quantityElement);
         
            //Constant formula and not fixed! no need to save the formula
         else if (quantity.Formula.IsConstant())
         {
            RemoveConstantFormula(quantityElement, quantity.Formula, Constants.Serialization.Attribute.FORMULA,serializationContext);

            //save value
            addQuantityValue(quantity, quantityElement);
         }

         return quantityElement.AddDisplayUnitFor(quantity);
      }

      protected void RemoveConstantFormula(XElement quantityElement, IFormula formula, string formulaAttributeName, SerializationContext serializationContext)
      {
         if (!formula.IsConstant()) return;

         //we need to save the value and remove the formula attribute that won't be saved
         var attribute = quantityElement.Attribute(formulaAttributeName);
         if (attribute != null)
            attribute.Remove();

         //remove unused formula from cache that was saved automatically in the reference mapping
         serializationContext.RemoveFormulaFromCache(formula);
      }

      private static void addQuantityValue(T quantity, XElement quantityElement)
      {
         quantityElement.AddAttribute(Constants.Serialization.Attribute.VALUE, quantity.Value.ToString(NumberFormatInfo.InvariantInfo));
      }

      protected override void TypedDeserialize(T quantity, XElement quantityElement, SerializationContext serializationContext)
      {
         base.TypedDeserialize(quantity, quantityElement,serializationContext);
         quantityElement.UpdateDisplayUnit(quantity);

         var valueAttribute = valueAttributeFrom(quantityElement);
         if (quantity.IsFixedValue)
         {
            quantity.Value = valueAttribute.Value.ConvertedTo<double>();
            return;
         }

         var formulaId = quantityElement.GetAttribute(Constants.Serialization.Attribute.FORMULA);

         //parameter is a formula=>nothing to do 
         if (!string.IsNullOrEmpty(formulaId) || valueAttribute == null)
            return;

         var formula = serializationContext.ObjectFactory.Create<ConstantFormula>().WithValue(valueAttribute.Value.ConvertedTo<double>()).WithDimension(quantity.Dimension);
         quantity.Formula = formula;
      }

      private static XAttribute valueAttributeFrom(XElement quantityElement)
      {
         //Attribute was renamed from Value to value. We check first for new version and retrieve the old one if required
         return quantityElement.Attribute(Constants.Serialization.Attribute.VALUE) ?? quantityElement.Attribute(Constants.Serialization.VALUE);
      }
   }

   public abstract class ParameterXmlSerializerBase<TParameter> : QuantityXmlSerializer<TParameter> where TParameter : IParameter
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         Map(x => x.BuildMode);
         Map(x => x.Info);
         Map(x => x.IsDefault);
         MapReference(x => x.RHSFormula);

         //no need to save origin, or default value for core parameter are those values are only used in PK-Sim
      }
   }

   public class ParameterXmlSerializer : ParameterXmlSerializerBase<Parameter>
   {
   }

   public class ObserverXmlSerializer : QuantityXmlSerializer<Observer>
   {
   }
}