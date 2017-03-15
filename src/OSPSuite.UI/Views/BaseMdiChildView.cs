using System.Windows.Forms;
using OSPSuite.Assets;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;

namespace OSPSuite.UI.Views
{
   public partial class BaseMdiChildView : BaseView, IMdiChildView
   {
      protected ISingleStartPresenter _presenter;

      public BaseMdiChildView()
      {
         InitializeComponent();
      }

      public BaseMdiChildView(IView owner)
      {
         InitializeComponent();
         ApplicationIcon = ApplicationIcons.DefaultIcon;

         //this is call whenever the form closes, either after 'x' or when CloseView is called from 
         //the presenter
         FormClosed += (o, e) =>
         {
            Hide();
            _presenter.OnFormClosed();
            Release();
         };

         Icon = ApplicationIcon.WithSize(IconSizes.Size16x16);
         MdiParent = owner as Form;
      }

      public virtual void Display()
      {
         Focus();
         Show();
      }

      public virtual void CloseView()
      {
         //this calls the form Closed event
         Close();
      }

      public virtual void SaveChanges()
      {
         Focus();
      }

      protected virtual void Release()
      {
         MdiParent = null;
         _presenter = null;
      }

      public virtual ISingleStartPresenter Presenter
      {
         get { return _presenter; }
      }

      protected override bool GetAllowSkin()
      {
         return !DesignMode;
      }

      public virtual void SetControlEnabled(ISubPresenterItem subPresenterItem, bool enabled)
      {
         //nothing to do
      }

      public virtual void SetControlIcon(ISubPresenterItem subPresenterItem, ApplicationIcon icon)
      {
         //nothing to do
      }

      public virtual void SetControlToolTip(ISubPresenterItem subPresenterItem, string toolTip)
      {
         //nothing to do
      }

      public virtual void SetControlVisible(ISubPresenterItem subPresenterItem, bool visible)
      {
         //nothing to do
      }

      public virtual bool IsControlVisible(ISubPresenterItem subPresenterItem)
      {
         return true;
      }

      public virtual void ActivateControl(ISubPresenterItem subPresenterItem)
      {
         //nothing to do
      }

      public virtual void AddSubItemView(ISubPresenterItem subPresenterItem, IView viewToAdd)
      {
         //nothing to do
      }
   }
}