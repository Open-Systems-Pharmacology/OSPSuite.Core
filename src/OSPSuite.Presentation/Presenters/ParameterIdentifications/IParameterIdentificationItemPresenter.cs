using OSPSuite.Core.Domain.ParameterIdentifications;

namespace OSPSuite.Presentation.Presenters.ParameterIdentifications
{
   public interface IParameterIdentificationPresenter : IPresenter
   {
      void EditParameterIdentification(ParameterIdentification parameterIdentification);
   }

   public interface IParameterIdentificationItemPresenter : IParameterIdentificationPresenter, ISubPresenter
   {
   }
}