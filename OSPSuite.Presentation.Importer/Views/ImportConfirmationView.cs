using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OSPSuite.UI.Views;
using OSPSuite.Presentation.Importer.Presenters;

namespace OSPSuite.Presentation.Importer.Views
{
   public partial class ImportConfirmationView : BaseModalView, IImportConfirmationView
   {
      public ImportConfirmationView()
      {
         InitializeComponent();
      }

      public void AttachPresenter(IImportConfirmationPresenter presenter)
      {
      }

      public void SetDataSetNames(IEnumerable<string> names)
      {
         listBoxControl1.Items.AddRange(names.ToArray());
      }
   }
}
