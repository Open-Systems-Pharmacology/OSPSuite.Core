using System;
using OSPSuite.Core.Chart.ParameterIdentifications;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Serialization.Chart;

namespace OSPSuite.Core.Serialization.Xml
{
   public class ParameterIdentificationResidualHistogramXmlSerializer : ObjectBaseXmlSerializer<ParameterIdentificationResidualHistogram>
   {
   }

   public class ParameterIdentificationTimeProfileChartXmlSerializer : CurveChartXmlSerializer<ParameterIdentificationTimeProfileChart>
   {
   }

   public class ParameterIdentificationPredictedVsObservedChartXmlSerializer : CurveChartXmlSerializer<ParameterIdentificationPredictedVsObservedChart>
   {
   }

   public class ParameterIdentificationCorrelationMatrixXmlSerializer : ObjectBaseXmlSerializer<ParameterIdentificationCorrelationMatrix>
   {
   }

   public class ParameterIdentificationCovarianceMatrixXmlSerializer : ObjectBaseXmlSerializer<ParameterIdentificationCovarianceMatrix>
   {
   }

   public class ParameterIdentificationTimeProfileConfidenceIntervalChartXmlSerializer : ParameterIdentificationAnalysisChartWithLocalRepositoriesXmlSerializer<ParameterIdentificationTimeProfileConfidenceIntervalChart>
   {
   }

   public class ParameterIdentificationTimeProfilePredictionIntervalChartXmlSerializer : ParameterIdentificationAnalysisChartWithLocalRepositoriesXmlSerializer<ParameterIdentificationTimeProfilePredictionIntervalChart>
   {
   }

   public class ParameterIdentificationTimeProfileVPCIntervalChartXmlSerializer : ParameterIdentificationAnalysisChartWithLocalRepositoriesXmlSerializer<ParameterIdentificationTimeProfileVPCIntervalChart>
   {
   }

   public abstract class ParameterIdentificationAnalysisChartWithLocalRepositoriesXmlSerializer<T> : CurveChartXmlSerializer<T> where T : ParameterIdentificationAnalysisChartWithLocalRepositories
   {
      public override void PerformMapping()
      {
         //This needs to be done before base mapping to ensure that local repo are available when deserializing chart
         MapEnumerable(x => x.DataRepositories, addRepositoryToChart);
         base.PerformMapping();
      }

      private Action<DataRepository> addRepositoryToChart(T chart, SerializationContext context)
      {
         return repo =>
         {
            chart.AddRepository(repo);
            context.AddRepository(repo);
         };
      }
   }

   public class ParameterIdentificationResidualVsTimeChartXmlSerializer : ParameterIdentificationAnalysisChartWithLocalRepositoriesXmlSerializer<ParameterIdentificationResidualVsTimeChart>
   {
   }
}