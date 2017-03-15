using DevExpress.XtraBars;
using OSPSuite.Assets;
using OSPSuite.Presentation.Presenters.Journal;
using OSPSuite.Presentation.Views.Journal;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Services;

namespace OSPSuite.UI.Views.Journal
{
   public partial class JournalDiagramMainView : BaseUserControl, IJournalDiagramMainView
   {
      private IJournalDiagramMainPresenter _presenter;

      public JournalDiagramMainView(IImageListRetriever imageListRetriever)
      {
         InitializeComponent();
         barManager.Images = imageListRetriever.AllImages16x16;
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         initializeButton(btnSaveDiagram, ApplicationIcons.Save, Captions.Journal.SaveDiagram);
         initializeButton(btnRestoreOrder, ApplicationIcons.Reset, Captions.Journal.RestoreLayout);
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         btnSaveDiagram.ItemClick += (o, e) => OnEvent(saveDiagram);
         btnRestoreOrder.ItemClick += (o, e) => OnEvent(restoreOrder);
      }

      private void restoreOrder()
      {
         _presenter.RestoreChronologicalOrder();
      }

      private void saveDiagram()
      {
         _presenter.SaveDiagram();
      }

      public void AttachPresenter(IJournalDiagramMainPresenter presenter)
      {
         _presenter = presenter;
      }

      public void InsertDiagram(IJournalDiagramView view)
      {
         diagramPanel.FillWith(view);
      }

      private void initializeButton(BarButtonItem button, ApplicationIcon icon, string caption)
      {
         button.Caption = caption;
         button.ImageIndex = ApplicationIcons.IconIndex(icon);
         button.PaintStyle = BarItemPaintStyle.CaptionGlyph;
      }
   }
}
