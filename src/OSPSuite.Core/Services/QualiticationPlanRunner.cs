using System.Threading.Tasks;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Services
{
   public interface IQualificationPlanRunner
   {
      Task RunAsync(QualificationPlan qualificationPlan);
   }

   public class QualificationPlanRunner : IQualificationPlanRunner
   {
      private readonly IQualificationStepRunnerFactory _qualificationStepRunnerFactory;
      private readonly IOSPSuiteLogger _logger;

      public QualificationPlanRunner(IQualificationStepRunnerFactory qualificationStepRunnerFactory, IOSPSuiteLogger logger)
      {
         _qualificationStepRunnerFactory = qualificationStepRunnerFactory;
         _logger = logger;
      }

      public async Task RunAsync(QualificationPlan qualificationPlan)
      {
         _logger.AddDebug(Captions.StartingQualificationPlan(qualificationPlan.Name));

         //this needs to be run in order. Await EACH run
         foreach (var qualificationStep in qualificationPlan.Steps)
         {
            using (var runner = _qualificationStepRunnerFactory.CreateFor(qualificationStep))
            {
               await runner.RunAsync(qualificationStep);
            }
         }
      }
   }
}