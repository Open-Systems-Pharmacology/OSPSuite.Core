using System;
using System.Linq.Expressions;
using System.Xml.Linq;
using OSPSuite.Serializer;
using OSPSuite.Serializer.Xml.Extensions;
using OSPSuite.Core.Serialization.SimModel.DTO;

namespace OSPSuite.Core.Serialization.SimModel.Serializer
{
   public class SolverSettingsExportSerializer : SimModelSerializerBase<SolverSettingsExport>
   {
      public override void PerformMapping()
      {
         ElementName =  SimModelSchemaConstants.Solver;
         Map(x => x.Name);
         MapEnumerable(x => x.SolverOptions).WithMappingName(SimModelSchemaConstants.SolverOptionList);
      }

      protected override XElement TypedSerialize(SolverSettingsExport objectToSerialize, SimModelSerializationContext serializationContext)
      {
         var node = base.TypedSerialize(objectToSerialize,serializationContext);
         addAsElement(node, objectToSerialize, x => x.H0);
         addAsElement(node, objectToSerialize, x => x.HMax);
         addAsElement(node, objectToSerialize, x => x.HMin);
         addAsElement(node, objectToSerialize, x => x.AbsTol);
         addAsElement(node, objectToSerialize, x => x.MxStep);
         addAsElement(node, objectToSerialize, x => x.RelTol);
         addAsElement(node, objectToSerialize, x => x.UseJacobian);
         return node;
      }

      private XElement addAsElement<TObject, TProperty>(XElement element, TObject objectToSerialize, Expression<Func<TObject, TProperty>> exp)
      {
         var memberAccessor = _propertyMapFactory.CreateFor(exp);
         var elemNode = element.AddElement(SerializerRepository.CreateElement(memberAccessor.Name));
         elemNode.AddAttribute(SimModelSchemaConstants.Id, memberAccessor.ResolveValue(objectToSerialize).ToString());
         return element;
      }
   }

   internal class SolverOptionsExportSerializer : SimModelSerializerBase<SolverOptionExport>
   {
      public override void PerformMapping()
      {
         ElementName = SimModelSchemaConstants.SolverOption;
         Map(x=>x.Name);
         Map(x => x.ParameterId);
      }
   }
}