using System;
using System.Drawing;
using OSPSuite.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation.Presenters
{
   public interface IPresenter : IReleasable
   {
      /// <summary>
      ///    Return true if the presenter can be closed (i.e. no error in any of the controls or sub presenters) otherwise false
      /// </summary>
      bool CanClose { get; }

      /// <summary>
      ///    event is fired whenever something changed in the underlying view(s) managed by the presented
      /// </summary>
      event EventHandler StatusChanged;

      /// <summary>
      ///    should be called whenever the view has changed (e.g. is being called by the view itself)
      /// </summary>
      void ViewChanged();

      /// <summary>
      ///    Returns the view of the current presenter
      /// </summary>
      IView BaseView { get; }

      /// <summary>
      ///    initialize the presenter (everything that should not be performed in constructor but executed before using the
      ///    presenter)
      /// </summary>
      void Initialize();
   }

   public interface IPresenter<TView> : IPresenter where TView : IView
   {
      /// <summary>
      ///    The TView  associated to the presenter,
      /// </summary>
      TView View { get; }
   }

   public interface IInitializablePresenter<in T> : IPresenter
   {
      void InitializeWith(T initializer);
   }

   /// <summary>
   ///    Presenter is disposable (typically presenter for modal views)
   /// </summary>
   public interface IDisposablePresenter : IPresenter, IDisposable
   {
      /// <summary>
      ///    Returns true if the form should really be closed otherwise false (default is true)
      /// </summary>
      bool ShouldClose { get; }
   }

   /// <summary>
   ///    A presenter that has a command register that will be used to collect all commands
   /// </summary>
   public interface ICommandCollectorPresenter : IInitializablePresenter<ICommandCollector>, ICommandCollector
   {
      /// <summary>
      ///    Return the command register
      /// </summary>
      ICommandCollector CommandCollector { get; }
   }

   /// <summary>
   ///    A container presenter is a presenter that has some sub presenters to manage
   /// </summary>
   public interface IContainerPresenter : ICommandCollectorPresenter, IDisposablePresenter
   {
      /// <summary>
      ///    Add the sub view managed by a sub presenter to the presenter view
      /// </summary>
      /// <param name="subPresenterItem">sub presenter item for which the view should be added</param>
      /// <param name="view">sub view to add</param>
      void AddSubItemView(ISubPresenterItem subPresenterItem, IView view);
   }

   public interface ISubPresenter : ICommandCollectorPresenter
   {
      /// <summary>
      ///    Icon to display for the presenter
      /// </summary>
      ApplicationIcon Icon { get; }
   }

   public interface IPresenterWithContextMenu<in TObject> : IPresenter
   {
      void ShowContextMenu(TObject objectRequestingPopup, Point popupLocation);
   }

   public interface ISubjectPresenter : IPresenter
   {
      /// <summary>
      ///    Subject being edited in the presenter
      /// </summary>
      object Subject { get; }
   }

   public interface IEditPresenter : ISubjectPresenter, ICommandCollectorPresenter
   {
      /// <summary>
      ///    Edit the subject given as parameter
      /// </summary>
      void Edit(object objectToEdit);
   }

   public interface IEditPresenter<T> : IEditPresenter
   {
      /// <summary>
      ///    Edit the object of type <typeparamref name="T" /> given as parameter
      /// </summary>
      void Edit(T objectToEdit);
   }
}