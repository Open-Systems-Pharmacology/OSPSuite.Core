using System.ComponentModel;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Drawing;
using DevExpress.XtraEditors.Popup;
using DevExpress.XtraEditors.Registrator;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.ViewInfo;

namespace OSPSuite.UI.Controls
{
   //adapted from
   //https://supportcenter.devexpress.com/ticket/details/e3666/how-to-display-the-default-ok-button-in-the-popupcontaineredit-s-popup-window#PopupContainerEditOKButton.cs

   public class UxRepositoryItemPopupContainerEditOKButton : RepositoryItemPopupContainerEdit
   {
      // static constructor which calls static registration method
      static UxRepositoryItemPopupContainerEditOKButton()
      {
         RegisterPopupContainerEditOKButton();
      }

      // static register method
      public static void RegisterPopupContainerEditOKButton()
      {
         EditorRegistrationInfo.Default.Editors.Add(new EditorClassInfo(
            PopupContainerEditOKButtonEditorName,
            typeof(PopupContainerEditOKButton),
            typeof(UxRepositoryItemPopupContainerEditOKButton),
            typeof(PopupContainerEditViewInfo),
            new ButtonEditPainter(),
            true,
            null));
      }

      // internal editor name
      internal const string PopupContainerEditOKButtonEditorName = "PopupContainerEditOKButton";

      // public constructor
      public UxRepositoryItemPopupContainerEditOKButton() : base()
      {
         _protShowOkButton = true;
      }

      protected bool _protShowOkButton;

      [Description("Determines whether the Ok button will be displayed in the popup form or not.")]
      [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
      public bool ShowOkButton
      {
         get => _protShowOkButton;
         set => _protShowOkButton = value;
      }

      // ovverride property
      public override string EditorTypeName => PopupContainerEditOKButtonEditorName;

      public override void Assign(RepositoryItem item)
      {
         base.Assign(item);
         UxRepositoryItemPopupContainerEditOKButton currentRepository = (item as UxRepositoryItemPopupContainerEditOKButton);
         ShowOkButton = currentRepository.ShowOkButton;
      }
   }

   class PopupContainerEditOKButton : PopupContainerEdit
   {
      // static constructor
      static PopupContainerEditOKButton()
      {
         UxRepositoryItemPopupContainerEditOKButton.RegisterPopupContainerEditOKButton();
      }


      // ovverride property
      public override string EditorTypeName => UxRepositoryItemPopupContainerEditOKButton.PopupContainerEditOKButtonEditorName;

      // property as corresponded repositoryitem
      [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
      public new UxRepositoryItemPopupContainerEditOKButton Properties => base.Properties as UxRepositoryItemPopupContainerEditOKButton;

      protected override PopupBaseForm CreatePopupForm()
      {
         return new PopupContainerFormOkButton(this);
      }
   }

   class PopupContainerFormOkButton : PopupContainerForm
   {
      // constructor
      public PopupContainerFormOkButton(PopupContainerEditOKButton ownerEdit) : base(ownerEdit)
      {
      }

      // override methods
      protected override void SetupButtons()
      {
         UpdatePopupButtons();
      }

      new UxRepositoryItemPopupContainerEditOKButton Properties
      {
         get
         {
            PopupContainerEditOKButton edit = OwnerEdit as PopupContainerEditOKButton;
            if (edit == null) return null;
            return edit.Properties;
         }
      }

      internal void UpdatePopupButtons()
      {
         if (Properties == null) return;
         fShowOkButton = Properties.ShowOkButton;
         if (Properties.ShowPopupCloseButton)
            fCloseButtonStyle = Properties.ShowOkButton ? BlobCloseButtonStyle.Caption : BlobCloseButtonStyle.Glyph;
         else
            fCloseButtonStyle = BlobCloseButtonStyle.None;
         AllowSizing = Properties.PopupSizeable;
         if (!AllowSizing && !fShowOkButton && fCloseButtonStyle == BlobCloseButtonStyle.None)
            ViewInfo.ShowSizeBar = false;
      }
   }
}