using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraCharts.Wizard;

namespace PkModelCore.DataChart.Tests
{
   public partial class ChartControlForm : Form
   {
      public ChartControlForm()
      {
         InitializeComponent();
         // Create a ChartWizard for a new ChartControl
         ChartWizard wiz = new ChartWizard(chartControl1);

         // Invoke the chart's wizard.
         wiz.ShowDialog();

      }
   }
}
