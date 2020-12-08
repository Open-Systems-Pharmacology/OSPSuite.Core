using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Presentation.Presenters.Importer;
using OSPSuite.Presentation.Views.Importer;

namespace OSPSuite.UI.Views.Importer
{
   public partial class LloqEditorView : BaseModalView, ILloqEditorView
   {
      public LloqEditorView()
      {
         InitializeComponent();
         ColumnsComboBox.EditValueChanged += (s, e) => OnEvent(() => _presenter.SetLloqColumn(ColumnsComboBox.EditValue.ToString()));

      }

      private ILloqEditorPresenter _presenter;
      public void AttachPresenter(ILloqEditorPresenter presenter)
      {
         _presenter = presenter;
      }

      public void FillComboBox(IEnumerable<string> columns, string defaultValue)
      {
         ColumnsComboBox.Properties.Items.Clear();
         ColumnsComboBox.Properties.Items.AddRange(columns.ToArray());
         ColumnsComboBox.EditValue = defaultValue;
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         Text = Captions.Importer.LloqColumnEditorTitle;
      }
   }
}