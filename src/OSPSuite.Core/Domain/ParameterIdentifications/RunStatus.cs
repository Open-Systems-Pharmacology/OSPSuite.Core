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
      CalculatingSensitivity
   }

   public class RunStatus
   {
      public virtual RunStatusId Id { get; }
      public virtual ApplicationIcon Icon { get; }
      public virtual string DisplayName { get; }

      private RunStatus(RunStatusId id, ApplicationIcon icon, string displayName)
      {
         Id = id;
         Icon = icon;
         DisplayName = displayName;
      }

      public override string ToString()
      {
         return DisplayName;
      }

      private static readonly Cache<RunStatusId, RunStatus> _allRunStatus = new Cache<RunStatusId, RunStatus>(x => x.Id);

      public static RunStatus Created = create(RunStatusId.Created, ApplicationIcons.Add);
      public static RunStatus WaitingToRun = create(RunStatusId.WaitingToRun, ApplicationIcons.Warning);
      public static RunStatus Running = create(RunStatusId.Running, ApplicationIcons.Run);
      public static RunStatus RanToCompletion = create(RunStatusId.RanToCompletion, ApplicationIcons.OK);
      public static RunStatus Canceled = create(RunStatusId.Canceled, ApplicationIcons.Stop);
      public static RunStatus Faulted = create(RunStatusId.Faulted, ApplicationIcons.Error);
      public static RunStatus CalculatingSensitivity = create(RunStatusId.CalculatingSensitivity, ApplicationIcons.Solver);

      private static RunStatus create(RunStatusId id, ApplicationIcon icon, string displayName = null)
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