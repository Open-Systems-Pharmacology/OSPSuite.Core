using OSPSuite.Assets;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Presenters.Importer;
using OSPSuite.Presentation.Views.Importer;
using OSPSuite.UI.Controls;

namespace OSPSuite.UI.Views.Importer
{
   public partial class MetaDataParameterEditorView : BaseUserControl, IMetaDataParameterEditorView
   {
      private IMetaDataParameterEditorPresenter _presenter;

      public MetaDataParameterEditorView()
      {
         InitializeComponent();
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         manualnputLayoutControlItem.Text = Captions.Importer.ManualInput.FormatForLabel();
      }

      public string Input => manualInput.Text;

      public void AttachPresenter(IMetaDataParameterEditorPresenter presenter)
      {
         _presenter = presenter;
      }
   }
}