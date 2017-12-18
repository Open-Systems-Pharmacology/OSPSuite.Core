using System;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation.Presenters
{
   public abstract class AbstractDisposablePresenter<TView, TPresenter> : AbstractPresenter<TView, TPresenter>,
                                                                          IDisposablePresenter
      where TView : IView<TPresenter>, IDisposable
      where TPresenter : IDisposablePresenter
   {
      protected AbstractDisposablePresenter(TView view)
         : base(view)
      {
      }

      public virtual bool ShouldClose => true;

      protected virtual void Cleanup()
      {
         _view.Dispose();
      }

      #region Disposable properties

      private bool _disposed;

      public void Dispose()
      {
         if (_disposed) return;

         Cleanup();
         GC.SuppressFinalize(this);
         _disposed = true;
      }

      ~AbstractDisposablePresenter()
      {
         Cleanup();
      }

      #endregion
   }
}