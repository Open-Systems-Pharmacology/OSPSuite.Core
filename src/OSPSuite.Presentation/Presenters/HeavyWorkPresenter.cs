using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation.Presenters
{
   public interface IHeavyWorkPresenter : IDisposablePresenter
   {
      void Start(string actionCaption);
      void Close();
   }

   public class HeavyWorkPresenter : AbstractDisposablePresenter<IHeavyWorkView, IHeavyWorkPresenter>, IHeavyWorkPresenter
   {
      public HeavyWorkPresenter(IHeavyWorkView view)
         : base(view)
      {
      }

      public void Start(string actionCaption)
      {
         _view.Caption = actionCaption;
         _view.Display();
      }

      public void Close()
      {
         _view.Close();
      }
   }
}