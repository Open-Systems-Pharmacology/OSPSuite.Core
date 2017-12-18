using System.Windows.Forms;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout.Utils;
using DevExpress.XtraTab;
using OSPSuite.Assets;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Extensions;

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
         set { btnPrevious.Enabled = value; }
         get { return btnPrevious.Enabled ; }
      }

      public void InitializeWizard()
      {
         btnNext.Click += (o, e) => this.DoWithinWaitCursor(() => WizardPresenter.WizardNext(SelectedPageIndex));
         btnPrevious.Click += (o, e) => this.DoWithinWaitCursor(() => WizardPresenter.WizardPrevious(SelectedPageIndex));
         TabControl.SelectedPageChanging += (o, e) => onSelectedPageChanging(e.PrevPage.TabIndex, e.Page.TabIndex);
      }

      public int SelectedPageIndex
      {
         get { return TabControl.SelectedTabPageIndex; }
      }

      public bool CancelEnabled
      {
         set { btnCancel.Enabled = value; }
         get { return btnCancel.Enabled; }
      }

      protected override void OnFormClosing(FormClosingEventArgs e)
      {
         if (Canceled)
         {
            //if cancel button is deactivated, and the user triggered cancel should not close the form
            if (e.CloseReason == CloseReason.UserClosing && btnCancel.Enabled == false)
               e.Cancel = true;
            else
               e.Cancel = !WizardPresenter.ShouldClose;
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
         set { btnOk.Enabled = value; }
         get { return btnOk.Enabled; }
      }

      public bool NextEnabled
      {
         set { btnNext.Enabled = value; }
         get { return btnNext.Enabled; }
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

      public virtual XtraTabControl TabControl
      {
         get
         {
            //this should be implemented in all derived views
            return null;
         }
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         btnNext.Text = Captions.NextButton;
         btnPrevious.Text = Captions.PreviousButton;
         btnOk.Text = Captions.OKButton;
         btnCancel.Text = Captions.CancelButton;
         layoutItemNext.AdjustButtonSize();
         layoutItemPrevious.AdjustButtonSize();
         layoutItemOK.AdjustButtonSize();
         layoutItemCancel.AdjustButtonSize();
         btnPrevious.Image = ApplicationIcons.Previous.ToImage(IconSizes.Size16x16);
         btnNext.Image = ApplicationIcons.Next.ToImage(IconSizes.Size16x16);
         btnPrevious.ImageLocation = ImageLocation.MiddleLeft;
         btnNext.ImageLocation = ImageLocation.MiddleRight;
         btnOk.Image = ApplicationIcons.OK.ToImage(IconSizes.Size16x16);
         btnCancel.Image = ApplicationIcons.Cancel.ToImage(IconSizes.Size16x16);
         btnOk.ImageLocation = ImageLocation.MiddleRight;
         btnCancel.ImageLocation = ImageLocation.MiddleRight;
         MinimizeBox = false;
         MaximizeBox = false;
         layoutControlGroup.HideBorderIfRequired();
         btnOk.Shortcut = Keys.Control | Keys.Enter;
      }

      public bool Canceled
      {
         get { return DialogResult == DialogResult.Cancel; }
      }

      public virtual void CloseView()
      {
         DialogResult = DialogResult.OK;
      }
   }
}