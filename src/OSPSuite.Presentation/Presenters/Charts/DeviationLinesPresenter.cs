using OSPSuite.Presentation.DTO.Charts;
using OSPSuite.Presentation.Views.Charts;

namespace OSPSuite.Presentation.Presenters.Charts
{
   public interface IDeviationLinesPresenter : IDisposablePresenter, IPresenter<IDeviationLinesView>
   {
      /// <summary>
      ///    A returned value of null means that the action was cancelled by the user
      /// </summary>
      float? GetFoldValue();
   }

   public class DeviationLinesPresenter : AbstractDisposablePresenter<IDeviationLinesView, IDeviationLinesPresenter>,
      IDeviationLinesPresenter
   {
      public FoldValueDTO FoldValueDTO { get; } = new FoldValueDTO();

      public DeviationLinesPresenter(IDeviationLinesView view) : base(view)
      {
      }

      public float? GetFoldValue()
      {
         _view.BindTo(FoldValueDTO);
         _view.Display();

         return _view.Canceled ? null : FoldValueDTO.FoldValue;
      }
   }
}