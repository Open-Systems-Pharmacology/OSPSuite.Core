using System;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_DataRowDataSpecs : ContextSpecification<DataRowData>
   {
      protected override void Context()
      {
         base.Context();
         sut = new DataRowData();
      }
   }

   public class When_filling_from_data_repository : concern_for_DataRowDataSpecs
   {
      private DataRepository _repository;
      private BaseGrid _baseGrid;

      protected override void Context()
      {
         _repository = new DataRepository();
         _baseGrid = new BaseGrid("baseGridId", new Dimension(new BaseDimensionRepresentation(), "Time", "min")) { Values = new ArraySegment<float>() };
         var column = new DataColumn("columnId", "name1", new Dimension(new BaseDimensionRepresentation(), "Conc", "mg/l"), _baseGrid) { Values = new ArraySegment<float>() };
         _repository.Add(_baseGrid);
         _repository.Add(column);

         column = new DataColumn("column2Id", "name2", new Dimension(new BaseDimensionRepresentation(), "Conc", "mg/l"), _baseGrid) { Values = new ArraySegment<float>() };
         _repository.Add(column);

         _repository.InsertValues(0.0f, new Cache<string, float> { { "baseGridId", 0.0f }, { "columnId", 0.0f }, { "column2Id", 0.0f } });
         _repository.InsertValues(0.1f, new Cache<string, float> { { "baseGridId", 1f }, { "columnId", 1f }, { "column2Id", 1f } });
         _repository.InsertValues(0.2f, new Cache<string, float> { { "baseGridId", 2f }, { "columnId", 2f }, { "column2Id", 2f } });
         _repository.InsertValues(0.3f, new Cache<string, float> { { "baseGridId", 3f }, { "columnId", 3f }, { "column2Id", 3f } });


         base.Context();
      }

      [Observation]
      public void should_fill_with_appropriate_values()
      {
         sut.BaseGridValue.ShouldBeEqualTo(0.3f);
         sut.Data.Each(x => x.ShouldBeEqualTo(3));
      }

      protected override void Because()
      {
         sut.FillFromRepository(3, _repository);
      }
   }
}
