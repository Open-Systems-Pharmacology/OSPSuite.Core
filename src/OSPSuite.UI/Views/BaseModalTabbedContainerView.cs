using DevExpress.XtraTab;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Views
{
   public partial class BaseModalTabbedContainerView : BaseModalContainerView, ITabbedView
   {
      public BaseModalTabbedContainerView(IView owner): base(owner)
      {
         InitializeComponent();
      }

      public override void AddSubItemView(ISubPresenterItem subPresenterItem, IView viewToAdd)
      {
         this.AddTabbedView(subPresenterItem, viewToAdd);
      }

      public override void SetControlEnabled(ISubPresenterItem subPresenterItem, bool enabled)
      {
         this.SetTabEnabled(subPresenterItem, enabled);
      }

      public override void SetControlVisible(ISubPresenterItem subPresenterItem, bool visible)
      {
         this.SetTabVisibility(subPresenterItem, visible);
      }

      public override void ActivateControl(ISubPresenterItem subPresenterItem)
      {
         this.ActivateTab(subPresenterItem);
      }

      public override bool IsControlVisible(ISubPresenterItem subPresenterItem)
      {
         return this.IsTabVisible(subPresenterItem);
      }

      public virtual XtraTabControl TabControl
      {
         get { return null; }
      }
   }
}