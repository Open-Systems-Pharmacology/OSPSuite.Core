using DevExpress.XtraEditors;
using OSPSuite.Presentation.Presenters;
using OSPSuite.UI.Extensions;

namespace OSPSuite.Starter.Tasks.Starters
{
   public interface ITestStarter
   {
      void Start();
   }

   public abstract class TestStarter<TPresenter> : ITestStarter where TPresenter : IPresenter
   {
      protected readonly TPresenter _presenter;

      protected TestStarter(TPresenter presenter)
      {
         _presenter = presenter;
      }

      public virtual void Start()
      {
         var form = new XtraForm();

         form.FillWith(_presenter.BaseView);

         form.Show();
      }
   }
}