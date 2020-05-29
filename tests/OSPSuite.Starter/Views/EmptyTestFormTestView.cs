using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using OSPSuite.Starter.Presenters;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Views;

namespace OSPSuite.Starter.Views
{
    public partial class EmptyTestFormTestView : BaseUserControl, IEmptyTestFormTestView
    {
        public EmptyTestFormTestView()
        {
            InitializeComponent();
        }

        public void AttachPresenter(IEmptyTestFormTestPresenter presenter)
        {
            //do not need it
        }
    }
}