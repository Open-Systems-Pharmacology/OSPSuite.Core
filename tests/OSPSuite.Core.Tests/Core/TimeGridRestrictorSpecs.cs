using System.Collections.Generic;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Core.Extensions;
using OSPSuite.Helpers;

namespace OSPSuite.Core
{
   public abstract class concern_for_TimeGridRestrictor : ContextSpecification<ITimeGridRestrictor>
   {
      protected DataRepository _observedData;
      protected ITimeGridRestrictor _timeGridRestrictor;

      protected override void Context()
      {
         sut = new TimeGridRestrictor();
         _observedData = DomainHelperForSpecs.ObservedDataWithLLOQ();
      }
   }

   public class When_restricting_timegrid_indices_with_RemoveLLOQModes_never : concern_for_TimeGridRestrictor
   {
      private IReadOnlyList<int> _indices;

      protected override void Because()
      {
         _indices = sut.GetRelevantIndices(_observedData, RemoveLLOQModes.Never);
      }

      [Observation]
      public void should_return_all_indices_in_original_order()
      {
         _observedData.BaseGrid.Count.ShouldBeEqualTo(_indices.Count);
         for (int i = 0; i < _observedData.BaseGrid.Count; i++)
         {
            _indices[i].ShouldBeEqualTo(i);
         }
      }
   }


   public class When_restricting_timegrid_indices_with_RemoveLLOQModes_never_using_log_scale_and_some_values_are_zero : concern_for_TimeGridRestrictor
   {
      private IReadOnlyList<int> _indices;

      protected override void Context()
      {
         base.Context();
         _observedData = DomainHelperForSpecs.ObservedData();
         var col = _observedData.FirstDataColumn();
         col.Values = new[] { 0f, 10f, 1e-30f };
      }

      protected override void Because()
      {
         _indices = sut.GetRelevantIndices(_observedData, RemoveLLOQModes.Never, Scalings.Log);
      }

      [Observation]
      public void should_remove_indices_corresponding_to_zero_values()
      {
         _indices.Count.ShouldBeEqualTo(1);
         _indices[0].ShouldBeEqualTo(1);
      }
   }

   public class When_restricting_timegrid_indices_with_RemoveLLOQModes_Always : concern_for_TimeGridRestrictor
   {
      private IReadOnlyList<int> _indices;

      protected override void Because()
      {
         _indices = sut.GetRelevantIndices(_observedData, RemoveLLOQModes.Always);
      }

      [Observation]
      public void should_return_indices_for_values_greaterOrEqual_LLOQ_in_original_order()
      {
         _indices.Count.ShouldBeEqualTo(6);
         _indices[0].ShouldBeEqualTo(2);
         _indices[1].ShouldBeEqualTo(3);
         _indices[2].ShouldBeEqualTo(4);
         _indices[3].ShouldBeEqualTo(5);
         _indices[4].ShouldBeEqualTo(7);
         _indices[5].ShouldBeEqualTo(8);
      }
   }

   public class When_restricting_timegrid_indices_with_RemoveLLOQModes_Always_and_the_scaling_is_log_and_no_LLOQ_is_defined : concern_for_TimeGridRestrictor
   {
      private IReadOnlyList<int> _indices;

      protected override void Context()
      {
         base.Context();
         _observedData = DomainHelperForSpecs.ObservedData();
         var col = _observedData.FirstDataColumn();
         col.Values = new[] {0f, 10f, 1e-30f};
      }

      protected override void Because()
      {
         _indices = sut.GetRelevantIndices(_observedData, RemoveLLOQModes.Always, Scalings.Log);
      }

      [Observation]
      public void should_return_indices_for_values_greaterOrEqual_LLOQ_in_original_order()
      {
         _indices.Count.ShouldBeEqualTo(1);
         _indices[0].ShouldBeEqualTo(1);
      }
   }

   public class When_restricting_timegrid_indices_with_LLOQUsages_NoTrailing : concern_for_TimeGridRestrictor
   {
      private IReadOnlyList<int> _indices;

      protected override void Because()
      {
         _indices = sut.GetRelevantIndices(_observedData, RemoveLLOQModes.NoTrailing);
      }

      [Observation]
      public void should_return_all_indices_except_for_trailing_Nminus1_LLOQ_values_in_original_order()
      {
         _indices.Count.ShouldBeEqualTo(10);
         for (int i = 0; i < 10; i++)
         {
            _indices[i].ShouldBeEqualTo(i);
         }
      }
   }

   public class When_restricting_timegrid_indices_with_LLOQUsages_NoTrailing_in_log_scaling_with_values_below_lloq : concern_for_TimeGridRestrictor
   {
      private IReadOnlyList<int> _indices;

      protected override void Context()
      {
         base.Context();
         var col = _observedData.FirstDataColumn();
         col.BaseGrid.Values = new[] { 10f, 20f, 30f, 40f, 50f };
         col.Values = new[] { 10f, 10f, 0.005f, 0.005f, 0.005f };
      }

      protected override void Because()
      {
         _indices = sut.GetRelevantIndices(_observedData, RemoveLLOQModes.NoTrailing, Scalings.Log);
      }

      [Observation]
      public void should_return_all_indices_except_for_trailing_Nminus1_LLOQ_values_in_original_order()
      {
         _indices.Count.ShouldBeEqualTo(3);
         _indices[0].ShouldBeEqualTo(0);
         _indices[1].ShouldBeEqualTo(1);
         _indices[2].ShouldBeEqualTo(2);
      }
   }

   public class When_restricting_timegrid_indices_with_LLOQUsages_NoTrailing_in_lin_scaling_with_zero_values : concern_for_TimeGridRestrictor
   {
      private IReadOnlyList<int> _indices;

      protected override void Context()
      {
         base.Context();
         _observedData = DomainHelperForSpecs.ObservedData();
         var col = _observedData.FirstDataColumn();
         col.DataInfo.LLOQ = 0.001f;
         col.BaseGrid.Values = new[] { 10f, 20f, 30f, 40f, 50f };
         col.Values = new[] { 0f, 10f, 0f, 1e-30f, 0f };
      }

      protected override void Because()
      {
         _indices = sut.GetRelevantIndices(_observedData, RemoveLLOQModes.NoTrailing, Scalings.Linear);
      }

      [Observation]
      public void should_return_all_indices_except_for_trailing_Nminus1_LLOQ_values_in_original_order_and_without_the_zeros()
      {
         _indices.Count.ShouldBeEqualTo(3);
         _indices[0].ShouldBeEqualTo(0);
         _indices[1].ShouldBeEqualTo(1);
         _indices[2].ShouldBeEqualTo(2);
      }
   }


   public class When_restricting_timegrid_indices_with_LLOQUsages_NoTrailing_in_log_scaling_with_zero_values : concern_for_TimeGridRestrictor
   {
      private IReadOnlyList<int> _indices;

      protected override void Context()
      {
         base.Context();
         _observedData = DomainHelperForSpecs.ObservedData();
         var col = _observedData.FirstDataColumn();
         col.BaseGrid.Values = new[] { 10f, 20f, 30f, 40f, 50f };
         col.Values = new[] { 0f, 10f, 0f, 1e-30f, 0f };
      }

      protected override void Because()
      {
         _indices = sut.GetRelevantIndices(_observedData, RemoveLLOQModes.NoTrailing, Scalings.Log);
      }

      [Observation]
      public void should_return_all_indices_except_for_trailing_Nminus1_LLOQ_values_in_original_order_and_without_the_zeros()
      {
         _indices.Count.ShouldBeEqualTo(1);
         _indices[0].ShouldBeEqualTo(1);
      }
   }

   public class When_restricting_a_timegrid_with_LLOQUsages_Always : concern_for_TimeGridRestrictor
   {
      private IReadOnlyList<float> _times;

      protected override void Because()
      {
         _times = sut.GetRelevantTimes(_observedData, RemoveLLOQModes.Always);
      }

      [Observation]
      public void should_return_times_for_values_greaterOrEqual_LLOQ_in_original_order()
      {
         _times.Count.ShouldBeEqualTo(6);
         _times[0].ShouldBeEqualTo(2f);
         _times[1].ShouldBeEqualTo(3f);
         _times[2].ShouldBeEqualTo(4f);
         _times[3].ShouldBeEqualTo(5f);
         _times[4].ShouldBeEqualTo(7f);
         _times[5].ShouldBeEqualTo(8f);
      }
   }
}