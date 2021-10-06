using System.Collections.Generic;
using System.Linq;
using OSPSuite.Presentation.Presenters.Importer;
using OSPSuite.Presentation.Views.Importer;



namespace OSPSuite.UI.Views.Importer
{
   public partial class ImporterReloadView : BaseModalView, IImporterReloadView
   {
      private IImporterReloadPresenter _presenter;

      public ImporterReloadView()
      {
         InitializeComponent();
      }

      public void AttachPresenter(IImporterReloadPresenter presenter)
      {
         _presenter = presenter;
      }

      public void AddDeletedDataSets(IEnumerable<string> names)
      {
         deletedListBoxControl.Items.Clear();
         deletedListBoxControl.Items.AddRange(names.ToArray());
      }

      public void AddNewDataSets(IEnumerable<string> names)
      {
         newListBoxControl.Items.Clear();
         newListBoxControl.Items.AddRange(names.ToArray());
      }

      public void AddOverwrittenDataSets(IEnumerable<string> names)
      {
         overwrittenListBoxControl.Items.Clear();
         overwrittenListBoxControl.Items.AddRange(names.ToArray());
      }
   }
}