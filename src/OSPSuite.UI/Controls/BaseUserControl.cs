using System;
using System.Windows.Forms;
using OSPSuite.DataBinding;
using OSPSuite.Utility.Extensions;
using DevExpress.XtraEditors;
using OSPSuite.Assets;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Controls
{
   public partial class BaseUserControl : XtraUserControl, IView
   {
      private IPresenter _basePresenter;
      public event EventHandler CaptionChanged = delegate { };
      public virtual ApplicationIcon ApplicationIcon { get; set; }

      public BaseUserControl()
      {
         InitializeComponent();
         initializeHelp();
         ApplicationIcon = ApplicationIcons.DefaultIcon;
      }

      public virtual void InitializeBinding()
      {
      }

      public virtual void InitializeResources()
      {
      }

      private void initializeHelp()
      {
         _helpProvider.Initialize(this);
      }


      public virtual string Caption
      {
         get => Text;
         set
         {
            Text = value;
            CaptionChanged(this, EventArgs.Empty);
         }
      }

      public virtual bool HasError => false;

      public void AttachPresenter(IPresenter presenter)
      {
         _basePresenter = presenter;
      }

      protected virtual void OnValidationError(Control control, string error)
      {
         errorProvider.SetError(control, error);
      }

      protected virtual void OnClearError(Control control)
      {
         errorProvider.SetError(control, string.Empty);
      }

      /// <summary>
      ///    Registers error validation and bind the changing event of the binder to the action given as parameter
      /// </summary>
      /// <param name="screenBinder">Binder</param>
      /// <param name="statusChangingNotify">Action called when the changing event of the binder is being raised</param>
      /// <param name="statusChangedNotify">Action called when the changed event of the binder is being raised</param>
      protected virtual void RegisterValidationFor<T>(ScreenBinder<T> screenBinder, Action statusChangingNotify = null, Action statusChangedNotify = null)
      {
         screenBinder.OnValidationError += OnValidationError;
         screenBinder.OnValidated += OnClearError;

         if (statusChangingNotify != null)
            screenBinder.Changing += statusChangingNotify;

         if (statusChangedNotify != null)
            screenBinder.Changed += statusChangedNotify;
      }

      /// <summary>
      ///    This methods should be called whenever a item in the view has changed and the presenter should be notified
      /// </summary>
      protected virtual void NotifyViewChanged()
      {
         OnEvent(performViewChangedNotification);
      }

      private void performViewChangedNotification() => _basePresenter.ViewChanged();

      /// <summary>
      ///    Encapsulate the call to <c>DoWithinExceptionHandler</c> for events to ease readability
      ///    and ensures that the <see cref="NotifyViewChanged" /> methods is being called if the
      ///    <paramref name="notifyViewChanged" /> is set to true
      /// </summary>
      protected virtual void OnEvent(Action action, bool notifyViewChanged = false)
      {
         this.DoWithinExceptionHandler(() =>
         {
            action();
            if (notifyViewChanged)
               performViewChangedNotification();
         });
      }

      /// <summary>
      ///    Encapsulate the call to <c>DoWithinExceptionHandler</c> for events to ease readability
      ///    and ensures that the <see cref="NotifyViewChanged" /> methods is being called if the
      ///    <paramref name="notifyViewChanged" /> is set to true
      /// </summary>
      protected virtual void OnEvent<TEventArgs>(Action<TEventArgs> action, TEventArgs e, bool notifyViewChanged = false)
      {
         OnEvent(() => action(e), notifyViewChanged);
      }

      /// <summary>
      ///    Encapsulate the call to <c>DoWithinExceptionHandler</c> for events to ease readability
      ///    and ensures that the <see cref="NotifyViewChanged" /> methods is being called if the
      ///    <paramref name="notifyViewChanged" /> is set to true
      /// </summary>
      protected virtual void OnEvent<TEventArgs>(Action<object, TEventArgs> action, object sender, TEventArgs e, bool notifyViewChanged = false)
      {
         OnEvent(() => action(sender, e), notifyViewChanged);
      }
   }
}