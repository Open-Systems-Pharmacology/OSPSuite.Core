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
         Start(0, 0);
      }

      public virtual void Start(int width, int height)
      {
         XtraForm form;
         if (height == 0 || width == 0)
            form = new XtraForm();
         else
         {
            form = new XtraForm
            {
               Height = height,
               Width = width
            };
         }
         form.FillWith(_presenter.BaseView);
         form.Show();
      }
   }
}