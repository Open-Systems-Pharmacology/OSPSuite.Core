using OSPSuite.Core.Commands.Core;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters.Commands;

namespace OSPSuite.Presentation.Services.Commands
{
   public interface ILabelTask
   {
      void AddLabelTo(IHistoryManager historyManager);
   }

   public class LabelTask : ILabelTask
   {
      private readonly IApplicationController _applicationController;

      public LabelTask(IApplicationController applicationController)
      {
         _applicationController = applicationController;
      }

      public void AddLabelTo(IHistoryManager historyManager)
      {
         using (var labelPresenter = _applicationController.Start<ILabelPresenter>())
         {
            if (!labelPresenter.CreateLabel())
               return;

            var labelDTO = labelPresenter.LabelDTO;

            historyManager.AddLabel(new LabelCommand {Description = labelDTO.Label, Comment = labelDTO.Comment});
         }
      }
   }
}