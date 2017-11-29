using OSPSuite.Presentation.DTO.Commands;
using OSPSuite.Presentation.Presenters.Commands;

namespace OSPSuite.Presentation.Views.Commands
{
    public interface ILabelView : IModalView<ILabelPresenter>
    {
        void BindTo(LabelDTO labelDTO);
    }
}