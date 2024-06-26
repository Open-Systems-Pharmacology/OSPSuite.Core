﻿using OSPSuite.Starter.Presenters;
using OSPSuite.UI.Views;

namespace OSPSuite.Starter.Views
{
   public partial class JournalRichEditFormView : BaseModalView, IJournalRichEditFormView
   {
      private IJournalRichEditFormPresenter _presenter;

      public JournalRichEditFormView()
      {
         InitializeComponent();
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         ExtraEnabled = true;
         ExtraVisible = true;
         ExtraCaption = "Export";
      }

      protected override void ExtraClicked()
      {
         base.ExtraClicked();
         exportToWord(uxRichEditControl.Document.OpenXmlBytes);
      }

      private void exportToWord(byte[] openXmlBytes)
      {
         _presenter.ExportToWord(openXmlBytes);
      }

      public void AttachPresenter(IJournalRichEditFormPresenter presenter)
      {
         _presenter = presenter;
      }

      public void SetPageContent(byte[] data)
      {
         uxRichEditControl.OpenXmlBytes = data;
      }
   }
}
