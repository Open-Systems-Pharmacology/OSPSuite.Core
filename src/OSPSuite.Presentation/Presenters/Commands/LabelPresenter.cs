using OSPSuite.Presentation.DTO.Commands;
using OSPSuite.Presentation.Views.Commands;

namespace OSPSuite.Presentation.Presenters.Commands
{
   public interface ILabelPresenter : IDisposablePresenter, IPresenter<ILabelView>
   {
      LabelDTO LabelDTO { get; }
      bool CreateLabel();
   }

   public class LabelPresenter : AbstractDisposablePresenter<ILabelView, ILabelPresenter>, ILabelPresenter
   {
      public LabelDTO LabelDTO { get; } = new LabelDTO();

      public LabelPresenter(ILabelView view) : base(view)
      {
      }

      public bool CreateLabel()
      {
         _view.BindTo(LabelDTO);
         _view.Display();
         return !_view.Canceled;
      }
   }
}