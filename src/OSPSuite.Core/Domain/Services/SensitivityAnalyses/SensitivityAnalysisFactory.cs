using OSPSuite.Assets;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain.SensitivityAnalyses;

namespace OSPSuite.Core.Domain.Services.SensitivityAnalyses
{
   public interface ISensitivityAnalysisFactory
   {
      SensitivityAnalysis Create();
   }

   public class SensitivityAnalysisFactory : ISensitivityAnalysisFactory
   {
      private readonly IObjectBaseFactory _objectBaseFactory;
      private readonly IOSPSuiteExecutionContext _executionContext;
      private readonly IContainerTask _containerTask;

      public SensitivityAnalysisFactory(IObjectBaseFactory objectBaseFactory, IOSPSuiteExecutionContext executionContext, IContainerTask containerTask)
      {
         _objectBaseFactory = objectBaseFactory;
         _executionContext = executionContext;
         _containerTask = containerTask;
      }

      public SensitivityAnalysis Create()
      {
         var sensitivityAnalysis = _objectBaseFactory.Create<SensitivityAnalysis>();
         var project = _executionContext.Project;
         sensitivityAnalysis.Name = _containerTask.CreateUniqueName(project.AllSensitivityAnalyses, Captions.SensitivityAnalysis.SensitivityAnalysisDefaultName);
         sensitivityAnalysis.Icon = ApplicationIcons.SensitivityAnalysis.IconName;
         sensitivityAnalysis.IsLoaded = true;
         return sensitivityAnalysis;
      }
   }
}