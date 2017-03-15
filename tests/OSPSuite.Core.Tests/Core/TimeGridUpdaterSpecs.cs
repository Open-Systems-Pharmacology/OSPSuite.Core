using System.Collections.Generic;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Helpers;

namespace OSPSuite.Core
{
   public abstract class concern_for_TimeGridUpdater : ContextSpecification<TimeGridUpdater>
   {
      protected DataRepository _observedData;
      protected IModelCoreSimulation _coreSimulation;
      protected IReadOnlyList<double> _addedPoints;

      protected override void Context()
      {
         sut = new TimeGridUpdater(new TimeGridRestrictor());

         _observedData = DomainHelperForSpecs.ObservedDataWithLLOQ();
         _coreSimulation = A.Fake<IModelCoreSimulation>();

         var outputSchema = _coreSimulation.BuildConfiguration.SimulationSettings.OutputSchema;
         A.CallTo(() => outputSchema.AddTimePoints(A<IReadOnlyList<double>>._))
            .Invokes(x => _addedPoints = x.GetArgument<IReadOnlyList<double>>(0));
      }

      public DataRepository ObservedData2WithLLOQ(string id = "TestData2WithLLOQ")
      {
         var observedData = new DataRepository(id);

         var baseGrid = new BaseGrid("Time", DomainHelperForSpecs.TimeDimensionForSpecs())
         {
            Values = new[] {0f, 1f, 1.5f, 2f, 2.5f, 3f, 4f}
         };

         observedData.Add(baseGrid);

         var data = new DataColumn("Col", DomainHelperForSpecs.ConcentrationDimensionForSpecs(), baseGrid)
         {
            Values = new[] {0.02f, 1f, 11F, 6f, 1f, 0.04f, 0.02f},
            DataInfo = {Origin = ColumnOrigins.Observation}
         };

         data.DataInfo.LLOQ = 0.04F;
         observedData.Add(data);

         return observedData;
      }
   }

   public class When_building_a_timegrid_based_on_two_observed_data_repositories : concern_for_TimeGridUpdater
   {
      private DataRepository _observedData2;

      protected override void Context()
      {
         base.Context();

         _observedData2 = ObservedData2WithLLOQ();
      }

      protected override void Because()
      {
         sut.UpdateSimulationTimeGrid(_coreSimulation, RemoveLLOQModes.Always, new[] {_observedData2, _observedData});
      }

      [Observation]
      public void should_contain_the_time_points_from_observed_data_2_greaterOrEqual_than_LLOQ()
      {
         _addedPoints.ShouldContain(1f, 1.5f, 2f, 2.5f, 3f);
      }

      [Observation]
      public void should_contain_the_time_points_from_observed_data_greaterOrEqual_than_LLOQ()
      {
         _addedPoints.ShouldContain(2f, 3f, 4f, 5f, 7f, 8f);
      }

      [Observation]
      public void should_not_cotain_the_time_points_below_LLOQ()
      {
         _addedPoints.ShouldNotContain(9f, 10f);
      }
   }
}