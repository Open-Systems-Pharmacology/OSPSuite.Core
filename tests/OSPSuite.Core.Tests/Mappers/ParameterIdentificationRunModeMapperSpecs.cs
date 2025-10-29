using System.Threading.Tasks;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Snapshots;
using OSPSuite.Core.Snapshots.Mappers;
using OSPSuite.Helpers;
using ParameterIdentificationRunMode = OSPSuite.Core.Snapshots.ParameterIdentificationRunMode;
using ParameterIdentificationRunModeMapper = OSPSuite.Helpers.Snapshots.ParameterIdentificationRunModeMapper;

namespace OSPSuite.Core.Mappers
{
   public abstract class concern_for_ParameterIdentificationRunModeMapper : ContextSpecificationAsync<ParameterIdentificationRunModeMapper>
   {
      protected ParameterIdentificationRunMode _snapshot;

      protected override Task Context()
      {
         sut = new ParameterIdentificationRunModeMapper();

         return _completed;
      }
   }

   public class When_mapping_a_parameter_identification_standard_run_to_snapshot : concern_for_ParameterIdentificationRunModeMapper
   {
      private Domain.ParameterIdentifications.ParameterIdentificationRunMode _standardRunMode;

      protected override async Task Context()
      {
         await base.Context();
         _standardRunMode = new StandardParameterIdentificationRunMode();
      }

      protected override async Task Because()
      {
         _snapshot = await sut.MapToSnapshot(_standardRunMode);
      }

      [Observation]
      public void should_return_null()
      {
         _snapshot.ShouldBeNull();
      }
   }

   public class When_mapping_a_parameter_identification_standard_run_snapshot_to_parameter_identification_run : concern_for_ParameterIdentificationRunModeMapper
   {
      private StandardParameterIdentificationRunMode _standardRunMode;
      private StandardParameterIdentificationRunMode _newStandardRunMode;

      protected override async Task Context()
      {
         await base.Context();
         _standardRunMode = new StandardParameterIdentificationRunMode();
         _snapshot = await sut.MapToSnapshot(_standardRunMode);
      }

      protected override async Task Because()
      {
         _newStandardRunMode = await sut.MapToModel(_snapshot, new SnapshotContext(new TestProject(), SnapshotVersions.Current)) as StandardParameterIdentificationRunMode;
      }

      [Observation]
      public void should_return_a_standard_run_mode()
      {
         _newStandardRunMode.ShouldNotBeNull();
      }
   }

   public class When_mapping_a_parameter_identification_multiple_run_to_snapshot : concern_for_ParameterIdentificationRunModeMapper
   {
      private MultipleParameterIdentificationRunMode _multipleRunMode;

      protected override async Task Context()
      {
         await base.Context();
         _multipleRunMode = new MultipleParameterIdentificationRunMode { NumberOfRuns = 10 };
      }

      protected override async Task Because()
      {
         _snapshot = await sut.MapToSnapshot(_multipleRunMode);
      }

      [Observation]
      public void should_have_set_the_number_of_run()
      {
         _snapshot.NumberOfRuns.ShouldBeEqualTo(_multipleRunMode.NumberOfRuns);
      }

      [Observation]
      public void should_have_set_all_other_parameters_to_null()
      {
         _snapshot.AllTheSameSelection.ShouldBeNull();
         _snapshot.CalculationMethods.ShouldBeNull();
      }
   }

   public class When_mapping_a_parameter_identification_multiple_run_snapshot_to_parameter_identification_run : concern_for_ParameterIdentificationRunModeMapper
   {
      private MultipleParameterIdentificationRunMode _multipleRunMode;
      private MultipleParameterIdentificationRunMode _newMultipleRun;

      protected override async Task Context()
      {
         await base.Context();
         _multipleRunMode = new MultipleParameterIdentificationRunMode { NumberOfRuns = 10 };
         _snapshot = await sut.MapToSnapshot(_multipleRunMode);
      }

      protected override async Task Because()
      {
         _newMultipleRun = await sut.MapToModel(_snapshot, new SnapshotContext(new TestProject(), SnapshotVersions.Current)) as MultipleParameterIdentificationRunMode;
      }

      [Observation]
      public void should_return_a_multiple_run_mode_with_the_expected_properties()
      {
         _newMultipleRun.ShouldNotBeNull();
         _newMultipleRun.NumberOfRuns.ShouldBeEqualTo(_multipleRunMode.NumberOfRuns);
      }
   }
}