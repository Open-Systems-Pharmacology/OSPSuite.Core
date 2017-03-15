using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Presentation.Views.ParameterIdentifications;

namespace OSPSuite.Presentation.Presenters.ParameterIdentifications
{
   public interface IStandardParameterIdentificationRunModePresenter : IPresenter<IStandardParameterIdentificationRunModeView>, IParameterIdentificationRunModePresenter
   {
   }

   public class StandardParameterIdentificationRunModePresenter : ParameterIdentificationRunModePresenter<IStandardParameterIdentificationRunModeView, IStandardParameterIdentificationRunModePresenter, StandardParameterIdentificationRunMode>, IStandardParameterIdentificationRunModePresenter
   {
      public StandardParameterIdentificationRunModePresenter(IStandardParameterIdentificationRunModeView view) : base(view)
      {
      }
   }
}