using OSPSuite.BDDHelper;
using FakeItEasy;
using OSPSuite.Presentation.DTO.Commands;
using OSPSuite.Presentation.Presenters.Commands;
using OSPSuite.Presentation.Views.Commands;

namespace OSPSuite.Presentation
{
    public abstract class concern_for_label_presenter : ContextSpecification<ILabelPresenter>
    {
        protected ILabelView _view;

        protected override void Context()
        {
            _view = A.Fake<ILabelView>();
            sut = new LabelPresenter(_view);
        }
    }

    public class When_creating_a_label_item : concern_for_label_presenter
    {
        protected override void Because()
        {
            sut.CreateLabel();
        }

        [Observation]
        public void should_tell_the_view_to_bind_to_the_label_dto()
        {
           A.CallTo(()=>_view.BindTo(A<LabelDTO>.Ignored)).MustHaveHappened();
        }
    }
}