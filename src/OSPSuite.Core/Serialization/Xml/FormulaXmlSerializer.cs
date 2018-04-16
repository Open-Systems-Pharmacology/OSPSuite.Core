using System;
using System.Xml.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Serializer;
using OSPSuite.Serializer.Xml.Extensions;

namespace OSPSuite.Core.Serialization.Xml
{
   public abstract class FormulaXmlSerializer<T> : ObjectBaseXmlSerializer<T> where T : class, IFormula //, new() 
   {
      protected FormulaXmlSerializer()
      {
      }

      protected FormulaXmlSerializer(string name) : base(name)
      {
      }

      public override void PerformMapping()
      {
         base.PerformMapping();
         Map(x => x.Dimension).WithMappingName(Constants.Serialization.Attribute.Dimension);
         MapEnumerable(x => x.ObjectPaths, x => x.AddObjectPath).WithMappingName(Constants.Serialization.OBJECT_PATHS);
      }
   }

   public class ConstantFormulaXmlSerializer : FormulaXmlSerializer<ConstantFormula>
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         Map(x => x.Value);
      }
   }

   public class ExplicitFormulaXmlSerializer : FormulaXmlSerializer<ExplicitFormula>
   {
      public ExplicitFormulaXmlSerializer() : base(Constants.Serialization.FORMULA)
      {
      }

      public override void PerformMapping()
      {
         base.PerformMapping();
         Map(x => x.FormulaString).WithMappingName(Constants.Serialization.Attribute.FORMULA);
         Map(x => x.OriginId).WithMappingName(Constants.Serialization.Attribute.ORIGIN);
      }
   }

   public class BlackBoxFormulaXmlSerializer : FormulaXmlSerializer<BlackBoxFormula>
   {
   }

   public class TableFormulaXmlSerializerBase<TFormula> : FormulaXmlSerializer<TFormula> where TFormula : TableFormula
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         Map(x => x.XDimension).WithMappingName(Constants.Serialization.Attribute.X_DIMENSION);
         Map(x => x.XName);
         Map(x => x.YName);
         MapEnumerable(x => x.AllPoints(), addPoint);
         Map(x => x.UseDerivedValues);
      }

      private Action<ValuePoint> addPoint(TFormula tableFormula)
      {
         return p => tableFormula.AddPoint(p);
      }

      protected override XElement TypedSerialize(TFormula tableFormula, SerializationContext serializationContext)
      {
         var formulaElenent = base.TypedSerialize(tableFormula, serializationContext);
         if (tableFormula.XDisplayUnit != null)
            formulaElenent.AddAttribute(Constants.Serialization.Attribute.X_DISPLAY_UNIT, tableFormula.XDisplayUnit.Name);

         if (tableFormula.YDisplayUnit != null)
            formulaElenent.AddAttribute(Constants.Serialization.Attribute.Y_DISPLAY_UNIT, tableFormula.YDisplayUnit.Name);

         return formulaElenent;
      }

      protected override void TypedDeserialize(TFormula tableFormula, XElement formulaElement, SerializationContext serializationContext)
      {
         base.TypedDeserialize(tableFormula, formulaElement, serializationContext);
         string xDisplayUnit = formulaElement.GetAttribute(Constants.Serialization.Attribute.X_DISPLAY_UNIT);
         string yDisplayUnit = formulaElement.GetAttribute(Constants.Serialization.Attribute.Y_DISPLAY_UNIT);

         if (tableFormula.XDimension != null)
            tableFormula.XDisplayUnit = tableFormula.XDimension.UnitOrDefault(xDisplayUnit);

         if (tableFormula.Dimension != null)
            tableFormula.YDisplayUnit = tableFormula.Dimension.UnitOrDefault(yDisplayUnit);
      }
   }

   public class TableFormulaXmlSerializer : TableFormulaXmlSerializerBase<TableFormula>
   {
   }

   public class TableFormulaWithOffsetXmlSerializer : FormulaXmlSerializer<TableFormulaWithOffset>
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         Map(x => x.TableObjectAlias);
         Map(x => x.OffsetObjectAlias);
      }
   }

   public class TableFormulaWithXArgumentXmlSerializer : FormulaXmlSerializer<TableFormulaWithXArgument>
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         Map(x => x.TableObjectAlias);
         Map(x => x.XArgumentAlias);
      }
   }

   public abstract class DynamicFormulaXmlSerializer<TFormula> : FormulaXmlSerializer<TFormula> where TFormula : DynamicFormula
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         Map(x => x.Criteria);
         Map(x => x.Variable);
         Map(x => x.FormulaString);
      }
   }

   public class SumFormulaXmlSerializer : DynamicFormulaXmlSerializer<SumFormula>
   {
   }

   //DistributionFormulas have no additional properties but certain restrictions and calculation methods
   public class LogNormalDistributionFormulaXmlSerializer : FormulaXmlSerializer<LogNormalDistributionFormula>
   {
   }

   public class NormalDistributionFormulaXmlSerializer : FormulaXmlSerializer<NormalDistributionFormula>
   {
   }

   public class UniformDistributionFormulaXmlSerializer : FormulaXmlSerializer<UniformDistributionFormula>
   {
   }

   public class DiscreteDistributionFormulaXmlSerializer : FormulaXmlSerializer<DiscreteDistributionFormula>
   {
   }

   public class ValuePointXmlSerializer : OSPSuiteXmlSerializer<ValuePoint>
   {
      public override void PerformMapping()
      {
         Map(x => x.X);
         Map(x => x.Y);
         Map(x => x.RestartSolver);
      }
   }
}