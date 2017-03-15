using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Core.Services;

namespace OSPSuite.Presentation.UICommands
{
   public abstract class StartParameterIdentificationAnalysisUICommand : ActiveObjectUICommand<ParameterIdentification>
   {
      private readonly IParameterIdentificationAnalysisCreator _parameterIdentificationAnalysisCreator;
      private readonly ParameterIdentificationAnalysisType _parameterIdentificationAnalysisType;

      protected StartParameterIdentificationAnalysisUICommand(IParameterIdentificationAnalysisCreator parameterIdentificationAnalysisCreator, IActiveSubjectRetriever activeSubjectRetriever,
         ParameterIdentificationAnalysisType parameterIdentificationAnalysisType)
         : base(activeSubjectRetriever)
      {
         _parameterIdentificationAnalysisCreator = parameterIdentificationAnalysisCreator;
         _parameterIdentificationAnalysisType = parameterIdentificationAnalysisType;
      }

      protected override void PerformExecute()
      {
         _parameterIdentificationAnalysisCreator.CreateAnalysisFor(Subject, _parameterIdentificationAnalysisType);
      }
   }

   public class StartCovarianceMatrixAnalysisUICommand : StartParameterIdentificationAnalysisUICommand
   {
      public StartCovarianceMatrixAnalysisUICommand(IParameterIdentificationAnalysisCreator parameterIdentificationAnalysisCreator, IActiveSubjectRetriever activeSubjectRetriever)
         : base(parameterIdentificationAnalysisCreator, activeSubjectRetriever, ParameterIdentificationAnalysisType.CovarianceMatrix)
      {
      }
   }

   public class StartCorrelationMatrixAnalysisUICommand : StartParameterIdentificationAnalysisUICommand
   {
      public StartCorrelationMatrixAnalysisUICommand(IParameterIdentificationAnalysisCreator parameterIdentificationAnalysisCreator, IActiveSubjectRetriever activeSubjectRetriever) 
         : base(parameterIdentificationAnalysisCreator, activeSubjectRetriever, ParameterIdentificationAnalysisType.CorrelationMatrix)
      {
      }
   }

   public class StartTimeProfileParameterIdentificationAnalysisUICommand : StartParameterIdentificationAnalysisUICommand
   {
      public StartTimeProfileParameterIdentificationAnalysisUICommand(IParameterIdentificationAnalysisCreator simulationAnalysisCreator, IActiveSubjectRetriever activeSubjectRetriever)
         : base(simulationAnalysisCreator, activeSubjectRetriever, ParameterIdentificationAnalysisType.TimeProfile)
      {
      }
   }

   public class StartPredictedVsObservedParameterIdentificationAnalysisUICommand : StartParameterIdentificationAnalysisUICommand
   {
      public StartPredictedVsObservedParameterIdentificationAnalysisUICommand(IParameterIdentificationAnalysisCreator simulationAnalysisCreator, IActiveSubjectRetriever activeSubjectRetriever)
         : base(simulationAnalysisCreator, activeSubjectRetriever, ParameterIdentificationAnalysisType.PredictedVsObserved)
      {
      }
   }

   public class StartResidualsVsTimeParameterIdentificationAnalysisUICommand : StartParameterIdentificationAnalysisUICommand
   {
      public StartResidualsVsTimeParameterIdentificationAnalysisUICommand(IParameterIdentificationAnalysisCreator simulationAnalysisCreator, IActiveSubjectRetriever activeSubjectRetriever)
         : base(simulationAnalysisCreator, activeSubjectRetriever, ParameterIdentificationAnalysisType.ResidualsVsTime)
      {
      }
   }

   public class StartResidualHistogramParameterIdentificationAnalysisUICommand : StartParameterIdentificationAnalysisUICommand
   {
      public StartResidualHistogramParameterIdentificationAnalysisUICommand(IParameterIdentificationAnalysisCreator simulationAnalysisCreator, IActiveSubjectRetriever activeSubjectRetriever)
         : base(simulationAnalysisCreator, activeSubjectRetriever, ParameterIdentificationAnalysisType.ResidualHistogram)
      {
      }
   }

   public class StartTimeProfilePredictionIntervalAnalysisUICommand : StartParameterIdentificationAnalysisUICommand
   {
      public StartTimeProfilePredictionIntervalAnalysisUICommand(IParameterIdentificationAnalysisCreator parameterIdentificationAnalysisCreator, IActiveSubjectRetriever activeSubjectRetriever) : 
         base(parameterIdentificationAnalysisCreator, activeSubjectRetriever, ParameterIdentificationAnalysisType.TimeProfilePredictionInterval)
      {
      }
   }
   public class StartTimeProfileConfidenceIntervalAnalysisUICommand : StartParameterIdentificationAnalysisUICommand
   {
      public StartTimeProfileConfidenceIntervalAnalysisUICommand(IParameterIdentificationAnalysisCreator parameterIdentificationAnalysisCreator, IActiveSubjectRetriever activeSubjectRetriever) :
         base(parameterIdentificationAnalysisCreator, activeSubjectRetriever, ParameterIdentificationAnalysisType.TimeProfileConfidenceInterval)
      {
      }
   }

   public class StartTimeProfileVPCIntervalAnalysisUICommand : StartParameterIdentificationAnalysisUICommand
   {
      public StartTimeProfileVPCIntervalAnalysisUICommand(IParameterIdentificationAnalysisCreator parameterIdentificationAnalysisCreator, IActiveSubjectRetriever activeSubjectRetriever) :
         base(parameterIdentificationAnalysisCreator, activeSubjectRetriever, ParameterIdentificationAnalysisType.TimeProfileVPCInterval)
      {
      }
   }
}