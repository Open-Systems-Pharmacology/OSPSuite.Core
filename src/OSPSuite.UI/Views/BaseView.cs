﻿using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using OSPSuite.Assets;
using OSPSuite.DataBinding;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Extensions;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.UI.Views
{
   public partial class BaseView : XtraForm, IView
   {
      public event EventHandler CaptionChanged = delegate { };

      private IPresenter _basePresenter;
      private ApplicationIcon _applicationIcon;

      public BaseView()
      {
         InitializeComponent();
         initializeHelp();
         //Set default icons for all views. Specific icons should be overwritten
         ApplicationIcon = ApplicationIcons.DefaultIcon;
      }

      private void initializeHelp()
      {
         _helpProvider.Initialize(this);
      }

      public virtual void InitializeResources()
      {
      }

      public virtual ApplicationIcon ApplicationIcon
      {
         get => _applicationIcon;
         set
         {
            _applicationIcon = value;
            IconOptions.SvgImage = value;
            IconOptions.SvgImageSize = IconSizes.Size16x16;
         }
      }

      public virtual void InitializeBinding()
      {
      }

      protected virtual void SetActiveControl()
      {
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

      public virtual string ErrorMessage => string.Empty;

      protected virtual void OnValidationError(Control control, string error)
      {
         _errorProvider.SetError(control, error);
      }

      protected virtual void OnClearError(Control control)
      {
         _errorProvider.SetError(control, string.Empty);
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

      public void AttachPresenter(IPresenter presenter)
      {
         _basePresenter = presenter;
      }

      /// <summary>
      ///    This method should be called whenever an action was performed on the UI which requires the presenter to be notified.
      ///    This is typically done by calling the ViewChanged method of the presenter
      /// </summary>
      protected virtual void NotifyViewChanged()
      {
         _basePresenter.ViewChanged();
      }

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
               NotifyViewChanged();
         });
      }

      /// <summary>
      ///    Encapsulate the call to <c>DoWithinExceptionHandler</c> for events to ease readability
      ///    and ensures that the <see cref="NotifyViewChanged" /> methods is being called if the
      ///    <paramref name="notifyViewChanged" /> is set to true
      /// </summary>
      protected virtual void OnEvent(Func<Task> action, bool notifyViewChanged = false)
      {
         this.DoWithinExceptionHandler(async () =>
         {
            await action();
            if (notifyViewChanged)
               NotifyViewChanged();
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