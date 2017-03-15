using DevExpress.XtraTab;
using OSPSuite.Assets;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Views
{
   public partial class BaseMdiChildTabbedView : BaseMdiChildView, ITabbedView
   {
      public BaseMdiChildTabbedView()
      {
         InitializeComponent();
      }

      public BaseMdiChildTabbedView(IView owner) : base(owner)
      {
         InitializeComponent();
      }

      public virtual XtraTabControl TabControl
      {
         get { return null; }
      }

      public override void AddSubItemView(ISubPresenterItem subPresenterItem, IView viewToAdd)
      {
         AddPageFor(subPresenterItem, viewToAdd);
      }

      protected XtraTabPage AddPageFor(ISubPresenterItem subPresenterItem, IView viewToAdd)
      {
         return AddPageFor(subPresenterItem.Index, viewToAdd);
      }

      protected XtraTabPage AddPageFor(int index, IView viewToAdd)
      {
         var page = this.AddTabbedView(index, viewToAdd);
         viewToAdd.CaptionChanged += (o, e) => page.Text = viewToAdd.Caption;
         return page;
      }

      public override void SaveChanges()
      {
         TabControl.Focus();
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

      public override void SetControlIcon(ISubPresenterItem subPresenterItem, ApplicationIcon icon)
      {
         this.SetTabIcon(subPresenterItem, icon);
      }
   }
}