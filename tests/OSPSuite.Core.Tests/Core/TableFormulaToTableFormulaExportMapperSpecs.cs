using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Serialization.SimModel.DTO;
using OSPSuite.Core.Serialization.SimModel.Services;

namespace OSPSuite.Core
{
   public abstract class concern_for_TableFormulaToTableFormulaExportMapper : ContextSpecification<ITableFormulaToTableFormulaExportMapper>
   {
      protected override void Context()
      {
         sut = new TableFormulaToTableFormulaExportMapper();
      }
   }
   class When_mapping_a_TableFormula : concern_for_TableFormulaToTableFormulaExportMapper
   {
      
      private TableFormula _input;
      private ValuePoint _p1;
      private ValuePoint _p2;
      private ValuePoint _p3;
      private TableFormulaExport _result;

      protected override void Context()
      {
         base.Context();
         _input = new TableFormula();
         _p1 = new ValuePoint(0, 0);
         _p2 = new ValuePoint(1, 1);
         _p3 = new ValuePoint(2, 2);

         _input.UseDerivedValues = false;

         _input.AddPoint(_p1);
         _input.AddPoint(_p2);
         _input.AddPoint(_p3);
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_input);
      }
      [Observation]
      public void should_map_all_points_in_order()
      {
         _result.PointList.ShouldOnlyContainInOrder(_p1,_p2,_p3);
      }

      [Observation]
      public void should_map_use_derived_values_property()
      {
         _result.UseDerivedValues.ShouldBeEqualTo(_input.UseDerivedValues);
      }
   }
}	