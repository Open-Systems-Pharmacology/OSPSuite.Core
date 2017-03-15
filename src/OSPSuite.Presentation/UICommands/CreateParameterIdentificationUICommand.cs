using System.Collections.Generic;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Services;

namespace OSPSuite.Presentation.UICommands
{
   public class CreateParameterIdentificationUICommand : IUICommand
   {
      private readonly IParameterIdentificationTask _parameterIdentificationTask;
      private readonly ISingleStartPresenterTask _singleStartPresenterTask;

      public CreateParameterIdentificationUICommand(IParameterIdentificationTask parameterIdentificationTask, ISingleStartPresenterTask singleStartPresenterTask)
      {
         _parameterIdentificationTask = parameterIdentificationTask;
         _singleStartPresenterTask = singleStartPresenterTask;
      }

      public void Execute()
      {
         _singleStartPresenterTask.StartForSubject(_parameterIdentificationTask.CreateParameterIdentification());
      }
   }

   public class CreateParameterIdentificationBasedOnParametersUICommand : ObjectUICommand<IEnumerable<IParameter>>
   {
      private readonly IParameterIdentificationTask _parameterIdentificationTask;
      private readonly ISingleStartPresenterTask _singleStartPresenterTask;

      public CreateParameterIdentificationBasedOnParametersUICommand(IParameterIdentificationTask parameterIdentificationTask, ISingleStartPresenterTask singleStartPresenterTask)
      {
         _parameterIdentificationTask = parameterIdentificationTask;
         _singleStartPresenterTask = singleStartPresenterTask;
      }
      protected override void PerformExecute()
      {
         _singleStartPresenterTask.StartForSubject(_parameterIdentificationTask.CreateParameterIdentificationBasedOn(Subject));
      }
   }

   public class CreateParameterIdentificationBasedOnSimulationsUICommand : ObjectUICommand<IEnumerable<ISimulation>>
   {
      private readonly IParameterIdentificationTask _parameterIdentificationTask;
      private readonly ISingleStartPresenterTask _singleStartPresenterTask;

      public CreateParameterIdentificationBasedOnSimulationsUICommand(IParameterIdentificationTask parameterIdentificationTask, ISingleStartPresenterTask singleStartPresenterTask)
      {
         _parameterIdentificationTask = parameterIdentificationTask;
         _singleStartPresenterTask = singleStartPresenterTask;
      }
      protected override void PerformExecute()
      {
         _singleStartPresenterTask.StartForSubject(_parameterIdentificationTask.CreateParameterIdentificationBasedOn(Subject));
      }
   }
}