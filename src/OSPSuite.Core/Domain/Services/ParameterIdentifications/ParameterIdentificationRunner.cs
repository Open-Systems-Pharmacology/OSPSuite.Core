using System;
using System.Threading.Tasks;
using OSPSuite.Assets;
using OSPSuite.Utility;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Core.Domain.Services.ParameterIdentifications
{
   public interface IParameterIdentificationRunner
   {
      Task Run(ParameterIdentification parameterIdentification);
      void Stop(ParameterIdentification parameterIdentification);
      bool IsRunning { get; }
   }

   public class ParameterIdentificationRunner : IParameterIdentificationRunner
   {
      private readonly IParameterIdentificationEngineFactory _parameterIdentificationEngineFactory;
      private readonly IDialogCreator _dialogCreator;
      private readonly IEntityValidationTask _entityValidationTask;
      private readonly IOSPSuiteExecutionContext _executionContext;
      private readonly Cache<string, IParameterIdentificationEngine> _parameterIdentificationEngines = new Cache<string, IParameterIdentificationEngine>(onMissingKey: x => null);

      public bool IsRunning => _parameterIdentificationEngines.Count > 0;

      public ParameterIdentificationRunner(IParameterIdentificationEngineFactory parameterIdentificationEngineFactory,  IDialogCreator dialogCreator, 
         IEntityValidationTask entityValidationTask, IOSPSuiteExecutionContext executionContext)
      {
         _parameterIdentificationEngineFactory = parameterIdentificationEngineFactory;
         _dialogCreator = dialogCreator;
         _entityValidationTask = entityValidationTask;
         _executionContext = executionContext;
      }

      public async Task Run(ParameterIdentification parameterIdentification)
      {
         if (!_entityValidationTask.Validate(parameterIdentification))
            return;

         try
         {
            using (var parameterIdentificationEngine = _parameterIdentificationEngineFactory.Create())
            {
               _parameterIdentificationEngines.Add(parameterIdentification.Id, parameterIdentificationEngine);
               var begin = SystemTime.UtcNow();
               await parameterIdentificationEngine.StartAsync(parameterIdentification);
               var end = SystemTime.UtcNow();
               var timeSpent = end - begin;
               _dialogCreator.MessageBoxInfo(Captions.ParameterIdentification.ParameterIdentificationFinished(parameterIdentification.Name, timeSpent.ToDisplay()));
            }
         }
         catch (OperationCanceledException)
         {
            _dialogCreator.MessageBoxInfo(Captions.ParameterIdentification.ParameterIdentificationCanceled(parameterIdentification.Name));
         }
         finally
         {
            _executionContext.ProjectChanged();
            _parameterIdentificationEngines.Remove(parameterIdentification.Id);
         }
      }

      public void Stop(ParameterIdentification parameterIdentification)
      {
         _parameterIdentificationEngines[parameterIdentification.Id]?.Stop();
      }
   }
}