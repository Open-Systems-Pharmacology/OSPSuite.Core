using System.Collections.Concurrent;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Presentation.DTO.ParameterIdentifications;
using OSPSuite.Presentation.Views.ParameterIdentifications;

namespace OSPSuite.Presentation.Presenters.ParameterIdentifications
{
   public interface IMultipleParameterIdentificationFeedbackPresenter : IPresenter<IMultipleParameterIdentificationFeedbackView>, IParameterIdentificationRunFeedbackPresenter
   {
   }

   public class MultipleParameterIdentificationFeedbackPresenter : AbstractPresenter<IMultipleParameterIdentificationFeedbackView, IMultipleParameterIdentificationFeedbackPresenter>, IMultipleParameterIdentificationFeedbackPresenter
   {
      private readonly ConcurrentDictionary<int, MultiOptimizationRunResultDTO> _allRunResultDTO = new ConcurrentDictionary<int, MultiOptimizationRunResultDTO>();

      public MultipleParameterIdentificationFeedbackPresenter(IMultipleParameterIdentificationFeedbackView view) : base(view)
      {
      }

      public void EditParameterIdentification(ParameterIdentification parameterIdentification)
      {
      }

      public void ClearReferences()
      {
      }

      public void ResetFeedback()
      {
         _allRunResultDTO.Clear();
         _view.DeleteBinding();
      }

      public void UpdateFeedback(ParameterIdentificationRunState runState)
      {
         var runResult = runState.RunResult;
         if (_allRunResultDTO.TryGetValue(runResult.Index, out var runStatusDTO))
         {
            updateErrorsAndStatus(runState, runStatusDTO);
            _view.RefreshData();
            return;
         }

         runStatusDTO = _allRunResultDTO.GetOrAdd(runResult.Index, new MultiOptimizationRunResultDTO());
         runStatusDTO.Index = runResult.Index;
         updateErrorsAndStatus(runState, runStatusDTO);
         _view.BindTo(_allRunResultDTO.Values);
      }

      private static void updateErrorsAndStatus(ParameterIdentificationRunState runState, MultiOptimizationRunResultDTO runStatusDTO)
      {
         runStatusDTO.Status = runState.Status;
         runStatusDTO.BestError = runState.BestResult.TotalError;
         runStatusDTO.CurrentError = runState.CurrentResult.TotalError;
         runStatusDTO.Description = runState.RunResult.Description;
      }
   }
}