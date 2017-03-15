using System.Xml.Linq;
using OSPSuite.Serializer.Xml;
using OSPSuite.Serializer.Xml.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Serialization.Xml
{
   public class UnitXmlSerializer : XmlSerializer<Unit, SerializationContext>, IUnitSystemXmlSerializer
   {
      private readonly ExplicitFormula _formula;
      private readonly string _factorAttributeName;

      public UnitXmlSerializer()
      {
         _factorAttributeName = Constants.Serialization.Attribute.FACTOR;
         _formula = new ExplicitFormula();
      }

      public override void PerformMapping()
      {
         Map(x => x.Name);
         //Do not map Factor as double since value might be saved as formula which needs to be evaluated when deserializing
         Map(x => x.Offset);
         Map(x => x.Visible);
      }

      protected override XElement TypedSerialize(Unit unit, SerializationContext serializationContext)
      {
         var unitElement = base.TypedSerialize(unit, serializationContext);

         //Use the "factor" attribute so that the xml schema does not change and can be edited by the user if need be
         unitElement.AddAttribute(_factorAttributeName, unit.FactorFormula);
         return unitElement;
      }

      protected override void TypedDeserialize(Unit unit, XElement unitElement, SerializationContext serializationContext)
      {
         base.TypedDeserialize(unit, unitElement, serializationContext);
         string factorFormula = unitElement.GetAttribute(_factorAttributeName);
         unit.FactorFormula = factorFormula;
         _formula.FormulaString = factorFormula;
         unit.Factor = _formula.Calculate(null);
      }
   }

   public class BaseDimensionRepresentationXmlSerializer : XmlSerializer<BaseDimensionRepresentation, SerializationContext>, IUnitSystemXmlSerializer
   {
      public override void PerformMapping()
      {
         Map(x => x.LengthExponent);
         Map(x => x.MassExponent);
         Map(x => x.TimeExponent);
         Map(x => x.ElectricCurrentExponent);
         Map(x => x.TemperatureExponent);
         Map(x => x.AmountExponent);
         Map(x => x.LuminousIntensityExponent);
      }
   }

   public class DimensionXmlSerializer : XmlSerializer<Dimension, SerializationContext>, IUnitSystemXmlSerializer
   {
      public override void PerformMapping()
      {
         Map(x => x.Name);
         Map(x => x.DisplayName);
         Map(x => x.BaseRepresentation);
         MapEnumerable(x => x.Units, x => x.AddUnit);
      }

      protected override XElement TypedSerialize(Dimension dimension, SerializationContext serializationContext)
      {
         var objectNode = base.TypedSerialize(dimension,serializationContext);

         objectNode.AddAttribute(Constants.Serialization.Attribute.BASE_UNIT, dimension.BaseUnit.Name);
         objectNode.AddAttribute(Constants.Serialization.Attribute.DEFAULT_UNIT, dimension.DefaultUnit.Name);

         return objectNode;
      }

      protected override void TypedDeserialize(Dimension dimension, XElement element, SerializationContext serializationContext)
      {
         base.TypedDeserialize(dimension, element,serializationContext);

         var baseUnitName = element.GetAttribute(Constants.Serialization.Attribute.BASE_UNIT);
         dimension.BaseUnit = dimension.Unit(baseUnitName);

         var defaultUnitName = element.GetAttribute(Constants.Serialization.Attribute.DEFAULT_UNIT);
         dimension.DefaultUnit = dimension.UnitOrDefault(defaultUnitName);
      }
   }

   public class DimensionFactoryXmlSerializer : XmlSerializer<DimensionFactory, SerializationContext>, IUnitSystemXmlSerializer
   {
      public override void PerformMapping()
      {
         MapEnumerable(x => x.Dimensions, x => x.AddDimension);
      }
   }
}