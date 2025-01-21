using OSPSuite.Assets;
using OSPSuite.Utility.Collections;
using OSPSuite.Core.Extensions;

namespace OSPSuite.Core.Domain.ParameterIdentifications
{
   public enum RunStatusId
   {
      Created,
      WaitingToRun,
      Running,
      RanToCompletion,
      Canceled,
      Faulted,
      CalculatingSensitivity,
      SensitivityCalculationFailed
   }

   public class RunStatus: IWithIcon
   {
      public RunStatusId Id { get; }
      public string IconName { get; }
      public string DisplayName { get; }

      private RunStatus(RunStatusId id, string iconName, string displayName)
      {
         Id = id;
         IconName = iconName;
         DisplayName = displayName;
      }

      public override string ToString()
      {
         return DisplayName;
      }

      private static readonly Cache<RunStatusId, RunStatus> _allRunStatus = new Cache<RunStatusId, RunStatus>(x => x.Id);

      public static RunStatus Created = create(RunStatusId.Created, IconNames.ADD);
      public static RunStatus WaitingToRun = create(RunStatusId.WaitingToRun, IconNames.WARNING);
      public static RunStatus Running = create(RunStatusId.Running, IconNames.RUN);
      public static RunStatus RanToCompletion = create(RunStatusId.RanToCompletion, IconNames.OK);
      public static RunStatus Canceled = create(RunStatusId.Canceled, IconNames.STOP);
      public static RunStatus Faulted = create(RunStatusId.Faulted, IconNames.ERROR);
      public static RunStatus CalculatingSensitivity = create(RunStatusId.CalculatingSensitivity, IconNames.SOLVER);
      public static RunStatus SensitivityCalculationFailed = create(RunStatusId.SensitivityCalculationFailed, IconNames.WARNING);

      private static RunStatus create(RunStatusId id, string icon, string displayName = null)
      {
         var runStatus = new RunStatus(id, icon, displayName ?? id.ToString().SplitToUpperCase());
         _allRunStatus.Add(runStatus);
         return runStatus;
      }

      public static RunStatus FindById(RunStatusId runStatusId)
      {
         return _allRunStatus[runStatusId];
      }
   }
}