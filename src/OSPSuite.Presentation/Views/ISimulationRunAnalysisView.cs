using System;
using System.Collections.Generic;
using System.Text;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views.ParameterIdentifications;

namespace OSPSuite.Presentation.Views
{ 
   public interface ISimulationRunAnalysisView : IView<ISimulationRunAnalysisPresenter>, ISimulationAnalysisView
   {
      void SetTotalError(double totalError);
   }
}
