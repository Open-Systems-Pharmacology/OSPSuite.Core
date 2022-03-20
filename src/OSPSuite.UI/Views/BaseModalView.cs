using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout;
using DevExpress.XtraLayout.Utils;
using OSPSuite.Assets;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Extensions;
using Padding = System.Windows.Forms.Padding;

namespace OSPSuite.UI.Views
{
   public partial class BaseModalView : BaseView, IModalView
   {
      private readonly Form _owner;
      private readonly BarManager _shortcutsManager;

      //only for design time
      public BaseModalView() : this(null)
      {
      }

      public BaseModalView(IView owner)
      {
         _owner = owner as Form;
         InitializeComponent();
         _shortcutsManager = new BarManager {Form = this};
         ShowInTaskbar = false;
         btnExtra.Click += (o, e) => OnEvent(ExtraClicked);
         btnCancel.Click += (o, e) => OnEvent(CancelClicked);
         btnOk.Manager = _shortcutsManager;
         btnCancel.Manager = _shortcutsManager;
         btnExtra.Manager = _shortcutsManager;
      }

      protected virtual void CancelClicked()
      {
         /*nothing to do here*/
      }


      protected virtual void ExtraClicked()
      {
         /*nothing to do here*/
      }

      public virtual void Display()
      {
         SetActiveControl();
         ShowDialog(_owner);
      }

      public virtual void CloseView()
      {
         DialogResult = DialogResult.OK;
      }

      protected override void OnFormClosing(FormClosingEventArgs e)
      {
         if (Canceled)
            e.Cancel = !ShouldClose;

         base.OnFormClosing(e);
      }

      /// <summary>
      ///    should be implemented in derived method if logic to cancel closing is available
      ///    Returns true if the form should close otherwise false
      /// </summary>
      protected virtual bool ShouldClose => true;

      public bool Canceled => DialogResult == DialogResult.Cancel;

      public bool OkEnabled
      {
         set => btnOk.Enabled = value;
         get => btnOk.Enabled;
      }

      public bool ExtraEnabled
      {
         set => btnExtra.Enabled = value;
         get => btnExtra.Enabled;
      }

      public string OkCaption
      {
         get => btnOk.Text;
         set => btnOk.Text = value;
      }

      public string ExtraCaption
      {
         get => btnExtra.Text;
         set => btnExtra.Text = value;
      }

      protected virtual void SetOkButtonEnable()
      {
         OkEnabled = IsOkButtonEnable;
      }

      protected virtual bool IsOkButtonEnable => !HasError;

      protected override void OnValidationError(Control control, string error)
      {
         base.OnValidationError(control, error);
         SetOkButtonEnable();
      }

      protected override void OnClearError(Control control)
      {
         base.OnClearError(control);
         SetOkButtonEnable();
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         btnOk.InitWithImage(ApplicationIcons.OK, Captions.OKButton, ImageLocation.MiddleRight);
         btnCancel.InitWithImage(ApplicationIcons.Cancel, Captions.CancelButton, ImageLocation.MiddleRight);
         btnCancel.Text = Captions.CancelButton;
         tablePanel.AdjustButton(btnOk);
         tablePanel.AdjustButton(btnCancel);
         tablePanel.AdjustButton(btnExtra);
         tablePanel.AutoScroll = false;
         MaximizeBox = false; 
         MinimizeBox = false;
         btnOk.Shortcut = Keys.Control | Keys.Enter;
         tablePanel.ColumnFor(btnExtra).Visible = false;
         //Double margin right for cancel 
         btnCancel.Margin = new Padding(btnCancel.Margin.Left, btnCancel.Margin.Top, btnCancel.Margin.Right * 2, btnCancel.Margin.Bottom);
         //Double margin right for extra 
         btnExtra.Margin = new Padding(btnExtra.Margin.Left * 2, btnExtra.Margin.Top, btnExtra.Margin.Right, btnExtra.Margin.Bottom);
      }

      public bool CancelVisible
      {
         set => SetItemVisibility(btnCancel, value);
         get => tablePanel.ColumnFor(btnCancel).Visible;
      }

      public bool ExtraVisible
      {
         set => SetItemVisibility(btnExtra, value);
         get => tablePanel.ColumnFor(btnExtra).Visible;
      }

      protected void SetItemVisibility(LayoutControlItem itemForButton, bool visible)
      {
         itemForButton.Visibility = LayoutVisibilityConvertor.FromBoolean(visible);
      }

      protected void SetItemVisibility(SimpleButton button, bool visible)
      {
         tablePanel.ColumnFor(button).Visible  =visible;
      }

      /// <summary>
      ///    Defines if the ok button should be activated when clicking enter. Default is true
      /// </summary>
      protected bool OKOnEnter
      {
         set => AcceptButton = value ? btnOk : null;
      }
   }
};