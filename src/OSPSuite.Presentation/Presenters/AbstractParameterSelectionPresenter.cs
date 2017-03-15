using System.Collections.Generic;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation.Presenters
{
   public interface IAbstractParameterSelectionPresenter : IPresenter
   {
      void AddParameters(IReadOnlyList<ParameterSelection> parameters);
      void AddParameter(ParameterSelection parameter);
      void SetParameterUnit(IParameterDTO parameterDTO, Unit displayUnit);
      void SetParameterValue(IParameterDTO parameterDTO, double valueInGuiUnit);
   }

   public abstract class AbstractParameterSelectionPresenter<TView,TPresenter> : AbstractPresenter<TView,TPresenter>, IAbstractParameterSelectionPresenter where TPresenter : IPresenter where TView : IView<TPresenter>
   {
      protected AbstractParameterSelectionPresenter(TView view) : base(view)
      {
      }

      public abstract void AddParameters(IReadOnlyList<ParameterSelection> parameters);

      public virtual void AddParameter(ParameterSelection parameter)
      {
         AddParameters(new[] { parameter });
      }

      public virtual void SetParameterUnit(IParameterDTO parameterDTO, Unit displayUnit)
      {
         var oldDisplayValue = parameterDTO.Value;
         parameterDTO.Parameter.DisplayUnit = displayUnit;
         SetParameterValue(parameterDTO, oldDisplayValue);
      }

      public virtual void SetParameterValue(IParameterDTO parameterDTO, double valueInGuiUnit)
      {
         parameterDTO.Parameter.ValueInDisplayUnit = valueInGuiUnit;
      }
   }
}