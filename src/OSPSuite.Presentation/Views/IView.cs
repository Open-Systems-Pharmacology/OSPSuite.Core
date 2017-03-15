using System;
using OSPSuite.Assets;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;

namespace OSPSuite.Presentation.Views
{
   public interface IView : IDisposable
   {
      /// <summary>
      ///    Initialize the default binding behavior
      /// </summary>
      void InitializeBinding();

      /// <summary>
      ///    Initialize the resources of the current view
      /// </summary>
      void InitializeResources();

      /// <summary>
      ///    Get or set the caption displayed in the view
      /// </summary>
      string Caption { get; set; }

      /// <summary>
      ///    is raised whenever the caption is being set
      /// </summary>
      event EventHandler CaptionChanged;

      /// <summary>
      ///    Returns true if the view contains an error
      /// </summary>
      bool HasError { get; }

      /// <summary>
      ///    Attach the presenter to the view
      /// </summary>
      void AttachPresenter(IPresenter presenter);

      /// <summary>
      ///    Returns the icon representing the sub view
      /// </summary>
      ApplicationIcon ApplicationIcon { get; set; }
   }

   public class ViewResizedEventArgs : EventArgs
   {
      public int Height { get; private set; }

      public ViewResizedEventArgs(int height)
      {
         Height = height;
      }
   }

   public interface IResizableView : IView
   {
      /// <summary>
      ///    Event is fired  whenever the height of the view has changed
      ///    The parameter is the new height of the view
      /// </summary>
      event EventHandler<ViewResizedEventArgs> HeightChanged;

      /// <summary>
      ///    triggers the height changed event (typically called from the presenter)
      /// </summary>
      void AdjustHeight();

      /// <summary>
      ///    After resizing, a refresh might be required
      /// </summary>
      void Repaint();

      /// <summary>
      ///    Optimal Height of the view
      /// </summary>
      int OptimalHeight { get; }
   }

   public interface IView<TPresenter> : IView where TPresenter : IPresenter
   {
      /// <summary>
      ///    Attach the presenter of type <typeparamref name="TPresenter" /> to the view
      /// </summary>
      void AttachPresenter(TPresenter presenter);
   }

   public interface IModalView : IView
   {
      /// <summary>
      ///    Show the view
      /// </summary>
      void Display();

      /// <summary>
      ///    Close the view
      /// </summary>
      void CloseView();

      /// <summary>
      ///    Returns true if the view was closed or canceled, otherwise false
      /// </summary>
      bool Canceled { get; }

      /// <summary>
      ///    Sets the OK button enabled or disabled
      /// </summary>
      bool OkEnabled { get; set; }

      /// <summary>
      ///    Sets the Extra button enabled or disabled
      /// </summary>
      bool ExtraEnabled { get; set; }

      /// <summary>
      ///    Sets the Extra button visible or hidden
      /// </summary>
      bool ExtraVisible { get; set; }

      /// <summary>
      ///    Sets the Cancel button visible or hidden
      /// </summary>
      bool CancelVisible { get; set; }
   }

   public interface IModalView<TPresenter> : IView<TPresenter>, IModalView where TPresenter : IDisposablePresenter
   {
   }

   public interface IContainerView : IView
   {
      void SetControlEnabled(ISubPresenterItem subPresenterItem, bool enabled);
      void SetControlIcon(ISubPresenterItem subPresenterItem, ApplicationIcon icon);
      void SetControlVisible(ISubPresenterItem subPresenterItem, bool visible);
      bool IsControlVisible(ISubPresenterItem subPresenterItem);
      void ActivateControl(ISubPresenterItem subPresenterItem);
      void AddSubItemView(ISubPresenterItem subPresenterItem, IView viewToAdd);
   }
}