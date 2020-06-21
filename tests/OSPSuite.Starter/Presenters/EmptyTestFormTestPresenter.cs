using OSPSuite.Presentation.Presenters;
using OSPSuite.Starter.Views;

namespace OSPSuite.Starter.Presenters
{
    public interface IEmptyTestFormTestPresenter : IPresenter<IEmptyTestFormTestView>
    {
    }

    public class EmptyTestFormTestPresenter :AbstractPresenter<IEmptyTestFormTestView, IEmptyTestFormTestPresenter>, IEmptyTestFormTestPresenter
    {
        public EmptyTestFormTestPresenter(IEmptyTestFormTestView view) : base(view)
        {
        }
    }
}