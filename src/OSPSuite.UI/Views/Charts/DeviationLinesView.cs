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
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Views.Charts;

namespace OSPSuite.UI.Views.Charts
{
   public partial class DeviationLinesView : BaseModalView, IDeviationLinesView
   {
      private IDeviationLinesPresenter _presenter;
      public DeviationLinesView()
      {
         InitializeComponent();
      }

      public void AttachPresenter(IDeviationLinesPresenter presenter)
      {
         _presenter = presenter;
      }
   }
}