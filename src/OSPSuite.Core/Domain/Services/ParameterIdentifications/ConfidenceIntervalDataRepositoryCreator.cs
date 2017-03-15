using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Extensions;

namespace OSPSuite.Core.Domain.Services.ParameterIdentifications
{
   public interface IConfidenceIntervalDataRepositoryCreator
   {
      DataRepository CreateFor(string confidenceIntervalName, double[] confidenceInterval, OutputMapping outputMapping, OptimizationRunResult runResult);
   }

   public class ConfidenceIntervalDataRepositoryCreator : IConfidenceIntervalDataRepositoryCreator
   {
      public DataRepository CreateFor(string confidenceIntervalName, double[] confidenceInterval, OutputMapping outputMapping, OptimizationRunResult runResult)
      {
         if (confidenceInterval == null)
            return new NullDataRepository();

         var fullOutputPath = outputMapping.FullOutputPath;
         var dataRepository = new DataRepository().WithName(fullOutputPath);

         var simulationResult = runResult.SimulationResultFor(fullOutputPath);
         var simulationBaseGrid = simulationResult.BaseGrid;
         var timeGrid = new BaseGrid(simulationBaseGrid.Name, simulationBaseGrid.Dimension)
         {
            Values = simulationBaseGrid.Values,
            DataInfo = simulationBaseGrid.DataInfo.Clone(),
            QuantityInfo = simulationBaseGrid.QuantityInfo.Clone(),
         };

         var outputColumn = new DataColumn(simulationResult.Name, simulationResult.Dimension, timeGrid)
         {
            Values = simulationResult.Values,
            DataInfo = simulationResult.DataInfo.Clone(),
            QuantityInfo = simulationResult.QuantityInfo.Clone(),
         };

         outputColumn.DataInfo.AuxiliaryType = outputMapping.Scaling == Scalings.Linear ? AuxiliaryType.ArithmeticMeanPop : AuxiliaryType.GeometricMeanPop;

         var interval = new DataColumn(confidenceIntervalName, simulationResult.Dimension, timeGrid)
         {
            DisplayUnit = simulationResult.DisplayUnit,
            Values = confidenceInterval.ToFloatArray(),
            QuantityInfo = simulationResult.QuantityInfo.Clone(),
            DataInfo = {Origin = ColumnOrigins.CalculationAuxiliary, MolWeight = outputColumn.DataInfo.MolWeight},
         };

         outputColumn.IsInternal = true;
         interval.AddRelatedColumn(outputColumn);
         dataRepository.Add(interval);

         return dataRepository;
      }
   }
}