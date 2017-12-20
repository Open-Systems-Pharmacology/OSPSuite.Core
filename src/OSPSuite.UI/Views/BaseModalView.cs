using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout;
using DevExpress.XtraLayout.Utils;
using OSPSuite.Assets;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Extensions;

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
         btnOk.Manager = _shortcutsManager;
         btnCancel.Manager = _shortcutsManager;
         btnExtra.Manager = _shortcutsManager;
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
         btnOk.Text = Captions.OKButton;
         btnCancel.Text = Captions.CancelButton;
         layoutItemOK.AdjustButtonSize();
         layoutItemCancel.AdjustButtonSize();
         layoutItemExtra.AdjustButtonSize();
         layoutControlBase.AutoScroll = false;
         btnOk.Image = ApplicationIcons.OK.ToImage(IconSizes.Size16x16);
         btnCancel.Image = ApplicationIcons.Cancel.ToImage(IconSizes.Size16x16);
         btnOk.ImageLocation = ImageLocation.MiddleRight;
         btnCancel.ImageLocation = ImageLocation.MiddleRight;
         MaximizeBox = false; 
         MinimizeBox = false;
         btnOk.Shortcut = Keys.Control | Keys.Enter;
         //hide the extra button per default
         layoutItemExtra.Visibility = LayoutVisibilityConvertor.FromBoolean(false);

         layoutControlGroupBase.HideBorderIfRequired();
      }

      public bool CancelVisible
      {
         set => SetItemVisibility(layoutItemCancel, value);
         get => LayoutVisibilityConvertor.ToBoolean(layoutItemCancel.Visibility);
      }

      public bool ExtraVisible
      {
         get => LayoutVisibilityConvertor.ToBoolean(layoutItemExtra.Visibility);
         set => SetItemVisibility(layoutItemExtra, value);
      }

      protected void SetItemVisibility(LayoutControlItem itemForButton, bool visible)
      {
         itemForButton.Visibility = LayoutVisibilityConvertor.FromBoolean(visible);
      }

      /// <summary>
      ///    Defines if the ok button should be activated when clicking enter. Default is true
      /// </summary>
      protected bool OKOnEnter
      {
         set => AcceptButton = value ? btnOk : null;
      }
   }
}