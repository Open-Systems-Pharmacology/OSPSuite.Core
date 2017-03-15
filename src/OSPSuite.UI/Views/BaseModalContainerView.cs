using OSPSuite.Assets;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Views;

namespace OSPSuite.UI.Views
{
   public partial class BaseModalContainerView : BaseModalView, IContainerView
   {
      //just for designer
      public BaseModalContainerView()
      {
         InitializeComponent();
      }

      public BaseModalContainerView(IView owner): base(owner)
      {
         InitializeComponent();
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         OKOnEnter = false;
      }

      public virtual void SetControlEnabled(ISubPresenterItem subPresenterItem, bool enabled)
      {
         //nothing to do
      }

      public void SetControlIcon(ISubPresenterItem subPresenterItem, ApplicationIcon icon)
      {
         //nothing to do
      }

      public void SetControlToolTip(ISubPresenterItem subPresenterItem, string toolTip)
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