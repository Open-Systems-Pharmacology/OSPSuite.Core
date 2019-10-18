using System.Linq;
using System.Xml.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Serialization.SimModel;
using OSPSuite.Core.Serialization.SimModel.DTO;
using OSPSuite.Core.Serialization.SimModel.Serializer;

namespace OSPSuite.Core.Serializers
{
   public abstract class concern_for_TableFormulaExportXmlSerializer : ContextSpecification<TableFormulaExportXmlSerializer>
   {
      private SimModelSerializerRepository _repository;

      protected override void Context()
      {
         _repository = new SimModelSerializerRepository();
         sut = (TableFormulaExportXmlSerializer) _repository.SerializerFor<TableFormulaExport>();
      }
   }
   
   class When_serializing_a_TableFormulaExport : concern_for_TableFormulaExportXmlSerializer
   {
      private TableFormulaExport _tableFormulaExport;
      private XElement _result;
      private ValuePoint _p1;
      private ValuePoint _p2;
      private ValuePoint _p3;

      protected override void Context()
      {
         base.Context();
         _tableFormulaExport = new TableFormulaExport {Id = 1};
         _p1 = new ValuePoint(0, 0);
         _p2 = new ValuePoint(1, 1) {RestartSolver = true};
         _p3 = new ValuePoint(2, 2);
         _tableFormulaExport.AddPoint(_p1);
         _tableFormulaExport.AddPoint(_p2);
         _tableFormulaExport.AddPoint(_p3);
         _tableFormulaExport.UseDerivedValues = false;
      }

      protected override void Because()
      {
         _result = sut.Serialize(_tableFormulaExport, new SimModelSerializationContext());
      }
      [Observation]
      public void should_create_right_xml()
      {
         _result.Name.LocalName.ShouldBeEqualTo(SimModelSchemaConstants.TableFormula);
         _result.Element(XName.Get( "PointList", SimModelSchemaConstants.Namespace)).ShouldNotBeNull();
         _result.Descendants(XName.Get(SimModelSchemaConstants.ValuePoint,SimModelSchemaConstants.Namespace)).Count().ShouldBeEqualTo(3);
         _result.Attribute(SimModelSchemaConstants.UseDerivedValues).Value.ShouldBeEqualTo(_tableFormulaExport.UseDerivedValues ? "1" : "0");
      }
   }

   
}	