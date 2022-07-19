using System;
using System.Collections.Generic;
using System.Text;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views.ParameterIdentifications;

namespace OSPSuite.Presentation.Views
{
   //temporarily we are not using this. its main use will be to bring together the two presenters
   public interface ISimulationRunAnalysisView : IView<ISimulationRunAnalysisPresenter>, ISimulationAnalysisView
   {
   }
}
