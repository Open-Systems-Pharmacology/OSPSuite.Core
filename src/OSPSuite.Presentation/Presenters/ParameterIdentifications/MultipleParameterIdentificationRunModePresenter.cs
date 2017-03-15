using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Presentation.Views.ParameterIdentifications;

namespace OSPSuite.Presentation.Presenters.ParameterIdentifications
{
   public interface IMultipleParameterIdentificationRunModePresenter : IPresenter<IMultipleParameterIdentificationRunModeView>, IParameterIdentificationRunModePresenter
   {
   }

   public class MultipleParameterIdentificationRunModePresenter : ParameterIdentificationRunModePresenter<IMultipleParameterIdentificationRunModeView, IMultipleParameterIdentificationRunModePresenter, MultipleParameterIdentificationRunMode>, IMultipleParameterIdentificationRunModePresenter
   {
      public MultipleParameterIdentificationRunModePresenter(IMultipleParameterIdentificationRunModeView parameterIdentificationRunModeView) : base(parameterIdentificationRunModeView)
      {
      }

      public override void Edit(ParameterIdentification parameterIdentification)
      {
         base.Edit(parameterIdentification);
         _view.BindTo(_runMode);
      }
   }
}