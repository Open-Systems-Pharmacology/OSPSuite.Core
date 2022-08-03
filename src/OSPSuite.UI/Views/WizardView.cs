using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout.Utils;
using DevExpress.XtraTab;
using OSPSuite.Assets;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Extensions;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;
using Keys = System.Windows.Forms.Keys;

namespace OSPSuite.UI.Views
{
   public partial class WizardView : BaseView, ITabbedView, ILatchable, IWizardView
   {
      private readonly Form _owner;
      protected IWizardPresenter WizardPresenter { get; set; }
      public bool IsLatched { get; set; }
      public bool ExtraEnabled { get; set; }
      public bool ExtraVisible { get; set; }
      public bool CancelVisible { get; set; }

      //just for designer
      public WizardView() : this(null)
      {
      }

      public WizardView(Form owner)
      {
         _owner = owner;
         InitializeComponent();
         var shortcutManager = new BarManager {Form = this};
         btnOk.Manager = shortcutManager;
         btnCancel.Manager = shortcutManager;
         btnOk.Manager = shortcutManager;
         btnPrevious.Manager = shortcutManager;
      }

      public void Display()
      {
         SetActiveControl();
         ShowDialog(_owner);
      }

      public bool PreviousEnabled
      {
         set => btnPrevious.Enabled = value;
         get => btnPrevious.Enabled;
      }

      public void InitializeWizard()
      {
         btnNext.Click += (o, e) => this.DoWithinWaitCursor(() => WizardPresenter.WizardNext(SelectedPageIndex));
         btnPrevious.Click += (o, e) => this.DoWithinWaitCursor(() => WizardPresenter.WizardPrevious(SelectedPageIndex));
         TabControl.SelectedPageChanging += (o, e) => onSelectedPageChanging(e.PrevPage.TabIndex, e.Page.TabIndex);
      }

      public int SelectedPageIndex => TabControl.SelectedTabPageIndex;

      public bool CancelEnabled
      {
         set => btnCancel.Enabled = value;
         get => btnCancel.Enabled;
      }

      protected override void OnFormClosing(FormClosingEventArgs e)
      {
         if (Canceled)
         {
            //if cancel button is deactivated, and the user triggered cancel should not close the form
            if (e.CloseReason == CloseReason.UserClosing && btnCancel.Enabled == false)
               e.Cancel = true;
            else
               e.Cancel = !(WizardPresenter?.ShouldClose ?? true);
         }

         base.OnFormClosing(e);
      }

      private void onSelectedPageChanging(int previousIndex, int index)
      {
         this.DoWithinLatch(() =>
            this.DoWithinWaitCursor(() => WizardPresenter.WizardCurrent(previousIndex, index)));
      }

      public bool OkEnabled
      {
         set => btnOk.Enabled = value;
         get => btnOk.Enabled;
      }

      public bool NextEnabled
      {
         set => btnNext.Enabled = value;
         get => btnNext.Enabled;
      }

      public void SetButtonsVisible(bool nextVisible, bool okVisible)
      {
         try
         {
            layoutControlBase.BeginUpdate();
            layoutItemNext.Visibility = LayoutVisibilityConvertor.FromBoolean(nextVisible);
            layoutItemOK.Visibility = LayoutVisibilityConvertor.FromBoolean(okVisible);
         }
         finally
         {
            layoutControlBase.EndUpdate();
         }
      }

      public void AddSubItemView(ISubPresenterItem simulationItem, IView viewToAdd)
      {
         this.AddTabbedView(simulationItem, viewToAdd);
      }

      public void SetControlEnabled(ISubPresenterItem simulationItem, bool enabled)
      {
         this.SetTabEnabled(simulationItem, enabled);
      }

      public void SetControlIcon(ISubPresenterItem subPresenterItem, ApplicationIcon icon)
      {
         this.SetTabIcon(subPresenterItem, icon);
      }

      public void SetControlVisible(ISubPresenterItem subPresenterItem, bool visible)
      {
         this.SetTabVisibility(subPresenterItem, visible);
      }

      public bool IsControlVisible(ISubPresenterItem subPresenterItem)
      {
         return this.IsTabVisible(subPresenterItem);
      }

      public void ActivateControl(ISubPresenterItem subPresenterItem)
      {
         this.DoWithinLatch(() => this.ActivateTab(subPresenterItem));
      }

      public virtual XtraTabControl TabControl =>
         //this should be implemented in all derived views
         null;

      public override void InitializeResources()
      {
         base.InitializeResources();
         btnNext.InitWithImage(ApplicationIcons.Next, Captions.NextButton, ImageLocation.MiddleRight);
         btnPrevious.InitWithImage(ApplicationIcons.Previous, Captions.PreviousButton, ImageLocation.MiddleLeft);
         btnOk.InitWithImage(ApplicationIcons.OK, Captions.OKButton, ImageLocation.MiddleRight);
         btnCancel.InitWithImage(ApplicationIcons.Cancel, Captions.CancelButton, ImageLocation.MiddleRight);
         layoutItemNext.AdjustButtonSize(layoutControlBase);
         layoutItemPrevious.AdjustButtonSize(layoutControlBase);
         layoutItemOK.AdjustButtonSize(layoutControlBase);
         layoutItemCancel.AdjustButtonSize(layoutControlBase);
         MinimizeBox = false;
         MaximizeBox = false;
         layoutControlGroup.HideBorderIfRequired();
         btnOk.Shortcut = Keys.Control | Keys.Enter;
      }

      public bool Canceled => DialogResult == DialogResult.Cancel;

      public virtual void CloseView()
      {
         DialogResult = DialogResult.OK;
      }
   }
}