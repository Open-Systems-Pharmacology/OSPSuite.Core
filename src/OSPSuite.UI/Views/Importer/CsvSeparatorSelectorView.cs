using OSPSuite.Assets;
using OSPSuite.Presentation.Presenters.Importer;
using OSPSuite.Presentation.Views.Importer;

namespace OSPSuite.UI.Views.Importer
{
   public partial class CsvSeparatorSelectorView : BaseModalView, ICsvSeparatorSelectorView
   {
      private ICsvSeparatorSelectorPresenter _presenter;
      public CsvSeparatorSelectorView()
      {
         InitializeComponent();
         separatorDescriptionLayoutControlItem.Text = Captions.Importer.CsvSeparatorDescription("");
      }

      public void AttachPresenter(ICsvSeparatorSelectorPresenter presenter)
      {
         _presenter = presenter;
      }
   }
}
