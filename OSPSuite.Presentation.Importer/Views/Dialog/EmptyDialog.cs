using DevExpress.XtraEditors;
using OSPSuite.Presentation.Presenters;
using OSPSuite.UI.Extensions;

namespace OSPSuite.Presentation.Importer.Views.Dialog
{
   public static class EmptyDialog
   {
      public static TPresenter Show<TPresenter> (int width, int height) where TPresenter : IPresenter
      {
         var presenter = Utility.Container.IoC.Resolve<TPresenter>();
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
         form.FillWith(presenter.BaseView);
         form.Show();
         return presenter;
      }
   }
}
