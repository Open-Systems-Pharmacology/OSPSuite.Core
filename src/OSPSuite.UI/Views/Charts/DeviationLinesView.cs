using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OSPSuite.Assets;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Views.Charts;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.UI.Views.Charts
{
   public partial class DeviationLinesView : BaseModalView, IDeviationLinesView
   {
      private IDeviationLinesPresenter _presenter;
      public DeviationLinesView()
      {
         InitializeComponent();
         layoutControlItem1.TextVisible = false;
         labelControl1.Text = Captions.Chart.DeviationLines.SpecifyFoldValue.FormatForLabel();
      }

      public void AttachPresenter(IDeviationLinesPresenter presenter)
      {
         _presenter = presenter;
      }

      public float GetFoldValue()
      {
         return foldValueTextEdit.EditValue.ConvertedTo<float>();
      }
   }
}