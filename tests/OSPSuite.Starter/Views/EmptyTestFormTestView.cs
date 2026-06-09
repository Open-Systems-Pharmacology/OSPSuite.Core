using OSPSuite.Starter.Presenters;
using OSPSuite.UI.Controls;

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