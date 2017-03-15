using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation.Presenters.ParameterIdentifications
{
   public interface IParameterIdentificationRunModePresenter : IPresenter
   {
      void Edit(ParameterIdentification parameterIdentification);
      bool CanEdit(ParameterIdentification identification);
      ParameterIdentificationRunMode RunMode { get; }
      string RunModeDisplayName { get; }
      void Refresh();
   }

   public abstract class ParameterIdentificationRunModePresenter<TView, TPresenter, TRunMode> : AbstractPresenter<TView, TPresenter>, IParameterIdentificationRunModePresenter
      where TPresenter : IParameterIdentificationRunModePresenter where TView : IView<TPresenter>
      where TRunMode : ParameterIdentificationRunMode, new()
   {
      protected TRunMode _runMode;
      protected ParameterIdentification _parameterIdentification;

      protected ParameterIdentificationRunModePresenter(TView view) : base(view)
      {
         _runMode = new TRunMode();
      }

      public virtual void Edit(ParameterIdentification parameterIdentification)
      {
         _parameterIdentification = parameterIdentification;
         _runMode = parameterIdentification.Configuration.RunMode.DowncastTo<TRunMode>();
      }

      public bool CanEdit(ParameterIdentification identification)
      {
         return identification.Configuration.RunMode.IsAnImplementationOf<TRunMode>();
      }

      public virtual void Refresh()
      {
         Edit(_parameterIdentification);
      }

      public ParameterIdentificationRunMode RunMode => _runMode;
      public string RunModeDisplayName => _runMode?.DisplayName;
   }
}