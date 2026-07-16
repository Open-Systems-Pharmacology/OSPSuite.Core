using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;
using OSPSuite.Core.Snapshots.Services;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Views;
using OSPSuite.Utility.Events;

namespace OSPSuite.Presentation.Presenters
{
   public abstract class LoadProjectFromSnapshotPresenter<TProject, TSnapshotProject> : LoadFromSnapshotPresenter<TProject, TProject, TSnapshotProject> where TProject : Project
   {
      protected readonly IQualificationPlanRunner _qualificationPlanRunner;

      protected LoadProjectFromSnapshotPresenter(ILoadFromSnapshotView view,
         ILogPresenter logPresenter,
         ISnapshotTask<TProject, TSnapshotProject> snapshotTask,
         IDialogCreator dialogCreator,
         IObjectTypeResolver objectTypeResolver,
         IOSPSuiteLogger logger,
         IEventPublisher eventPublisher,
         IQualificationPlanRunner qualificationPlanRunner) : base(view, logPresenter, snapshotTask, dialogCreator, objectTypeResolver, logger, eventPublisher)
      {
         _qualificationPlanRunner = qualificationPlanRunner;
      }

      public TProject LoadProject()
      {
         var models = LoadModelFromSnapshot();
         return models?.FirstOrDefault();
      }

      protected override async Task<IEnumerable<TProject>> LoadModelAsync(LoadFromSnapshotDTO loadFromSnapshotDTO)
      {
         var project = await _snapshotTask.LoadProjectFromSnapshotFileAsync(loadFromSnapshotDTO.SnapshotFile, loadFromSnapshotDTO.RunSimulations);
         RegisterProject(project);
         await runQualificationPlans(project);
         return new[] { project };
      }

      protected TProject ProjectFrom(IEnumerable<TProject> projects) => projects?.FirstOrDefault();

      private async Task runQualificationPlans(TProject project)
      {
         //needs to be done sequentially
         foreach (var qualificationPlan in AllQualificationPlansFrom(project))
         {
            await _qualificationPlanRunner.RunAsync(qualificationPlan);
         }
      }

      protected override void ClearModel(IEnumerable<TProject> model)
      {
         var projects = model?.ToList();
         base.ClearModel(projects);
         UnRegisterProjects(projects);
      }

      protected abstract void UnRegisterProjects(List<TProject> projects);

      protected abstract IReadOnlyList<QualificationPlan> AllQualificationPlansFrom(TProject project);

      protected abstract void RegisterProject(TProject project);
   }
}