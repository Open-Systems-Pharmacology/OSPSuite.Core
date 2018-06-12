using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Services;
using OSPSuite.Utility;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Utility.Extensions;
using Command = OSPSuite.Assets.Command;

namespace OSPSuite.Core.Domain.Services.ParameterIdentifications
{
   public interface ITransferOptimizedParametersToSimulationsTask
   {
      ICommand TransferParametersFrom(ParameterIdentification parameterIdentification, ParameterIdentificationRunResult runResult);
   }

   public class TransferOptimizedParametersToSimulationsTask<TExecutionContext> : ITransferOptimizedParametersToSimulationsTask where TExecutionContext : IOSPSuiteExecutionContext
   {
      private readonly ISetParameterTask _parameterTask;
      private readonly IDialogCreator _dialogCreator;
      private readonly IOSPSuiteExecutionContext _executionContext;

      public TransferOptimizedParametersToSimulationsTask(ISetParameterTask parameterTask, IDialogCreator dialogCreator, TExecutionContext executionContext)
      {
         _parameterTask = parameterTask;
         _dialogCreator = dialogCreator;
         _executionContext = executionContext;
      }

      public ICommand TransferParametersFrom(ParameterIdentification parameterIdentification, ParameterIdentificationRunResult runResult)
      {
         if (runResult.Status == RunStatus.Canceled)
         {
            var res = _dialogCreator.MessageBoxYesNo(Warning.ImportingParameterIdentificationValuesFromCancelledRun);
            if (res == ViewResult.No)
               return new OSPSuiteEmptyCommand<TExecutionContext>();
         }

         if (parameterIdentification.IsCategorialRunMode())
            _dialogCreator.MessageBoxInfo(Warning.ImportingParameterIdentificationValuesFromCategorialRun);

         var macroCommand = new OSPSuiteMacroCommand<TExecutionContext>
         {
            BuildingBlockType = ObjectTypes.Simulation,
            CommandType = Command.CommandTypeEdit,
            ObjectType = ObjectTypes.Simulation,
            Description = Captions.ParameterIdentification.ParameterIdentificationTransferredToSimulations(parameterIdentification.Name)
         };

         foreach (var optimizedParameter in runResult.BestResult.Values)
         {
            var identificationParameter = parameterIdentification.IdentificationParameterByName(optimizedParameter.Name);
            if (identificationParameter == null)
               throw new OSPSuiteException(Error.IdentificationParameterCannotBeFound(optimizedParameter.Name));

            macroCommand.AddRange(setOptimalParameterValueIn(identificationParameter, optimizedParameter.Value));
         }

         macroCommand.AddRange(parameterIdentification.AllFixedIdentificationParameters.SelectMany(x => setOptimalParameterValueIn(x, x.StartValue)));

         return macroCommand;
      }

      private IEnumerable<ICommand> setOptimalParameterValueIn(IdentificationParameter identificationParameter, double optimalValue)
      {
         return identificationParameter.AllLinkedParameters.SelectMany(linkedParameter => updateParameterValue(identificationParameter, optimalValue, linkedParameter));
      }

      private IEnumerable<ICommand> updateParameterValue(IdentificationParameter identificationParameter, double optimialValue, ParameterSelection linkedParameter)
      {
         var value = identificationParameter.OptimizedParameterValueFor(optimialValue, linkedParameter);
         var parameter = linkedParameter.Parameter;

         var setValueCommand = _parameterTask.SetParameterValue(parameter, value, linkedParameter.Simulation);
         return new[] {setValueCommand, updateValueOriginCommand(identificationParameter, parameter, linkedParameter.Simulation).AsHidden()};
      }

      private ICommand updateValueOriginCommand(IdentificationParameter identificationParameter, IParameter parameter, ISimulation simulation)
      {
         var isoDate = SystemTime.Now().ToIsoFormat();
         var valueOrigin = new ValueOrigin
         {
            Description = Captions.ParameterIdentification.ValueUpdatedFrom(identificationParameter.ParameterIdentification.Name, isoDate),
            Source = ValueOriginSources.ParameterIdentification,
            Method = ValueOriginDeterminationMethods.ParameterIdentification
         };

         return _parameterTask.UpdateParameterValueOrigin(parameter, valueOrigin, simulation);
      }
   }
}