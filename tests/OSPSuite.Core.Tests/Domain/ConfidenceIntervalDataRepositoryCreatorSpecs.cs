using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Core.Extensions;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_ConfidenceIntervalDataRepositoryCreator : ContextSpecification<IConfidenceIntervalDataRepositoryCreator>
   {
      protected override void Context()
      {
         sut = new ConfidenceIntervalDataRepositoryCreator();
      }
   }

   public class When_creating_the_confidence_interval_data_repository_for_a_given_linear_output_mapping : concern_for_ConfidenceIntervalDataRepositoryCreator
   {
      private OptimizationRunResult _runResult;
      private OutputMapping _outputMapping;
      private DataRepository _result;
      private double[] _confidenceInterval;
      private DataColumn _outputValues;
      private DataColumn _intervalCol;

      protected override void Context()
      {
         base.Context();
         _runResult = A.Fake<OptimizationRunResult>();   
         _outputMapping= A.Fake<OutputMapping>();
         A.CallTo(() => _outputMapping.FullOutputPath).Returns("A|B|C");
         _outputMapping.Scaling = Scalings.Linear;
         var outputTime = new BaseGrid("Time", DomainHelperForSpecs.TimeDimensionForSpecs()) {Values = new [] {1f,2f, 3f}};
         _outputValues = new DataColumn("Output", DomainHelperForSpecs.ConcentrationDimensionForSpecs(), outputTime)
         {
            Values = new[] {15f, 16f, 17f},
            DataInfo = {MolWeight = 150}
         };

         A.CallTo(() => _runResult.SimulationResultFor(_outputMapping.FullOutputPath)).Returns(_outputValues);
         _confidenceInterval = new[] { 10d, 20d, 30d };
      }

      protected override void Because()
      {
         _result = sut.CreateFor("Test", _confidenceInterval, _outputMapping, _runResult);
         _intervalCol = _result.AllButBaseGrid().First(x => x.Name == "Test");
      }

      [Observation]
      public void should_return_a_repository_containing_one_column_for_the_interval_and_one_related_column_with_the_output_values()
      {
         _result.Columns.Count().ShouldBeEqualTo(3);
         _intervalCol.Values.ShouldBeEqualTo(_confidenceInterval.ToFloatArray());
         _intervalCol.IsInternal.ShouldBeFalse();

         var relatedColumn = _intervalCol.GetRelatedColumn(AuxiliaryType.ArithmeticMeanPop);
         relatedColumn.ShouldNotBeNull();
         relatedColumn.Values.ShouldBeEqualTo(_outputValues.Values);
         relatedColumn.IsInternal.ShouldBeTrue();
      }

      [Observation]
      public void should_have_updated_the_molweight_of_the_interval_column_to_the_molweight_of_the_output_column()
      {
         _intervalCol.DataInfo.MolWeight.ShouldBeEqualTo(_outputValues.DataInfo.MolWeight);
      }

      [Observation]
      public void should_have_set_the_origin_of_the_interval_column_to_calculation_auxiliary()
      {
         _intervalCol.DataInfo.Origin.ShouldBeEqualTo(ColumnOrigins.CalculationAuxiliary);
      }

      [Observation]
      public void should_have_set_the_time_for_the_interval_column_equal_to_the_output_time()
      {
         _intervalCol.BaseGrid.Values.ShouldBeEqualTo(_outputValues.BaseGrid.Values);
      }
   }
}	
