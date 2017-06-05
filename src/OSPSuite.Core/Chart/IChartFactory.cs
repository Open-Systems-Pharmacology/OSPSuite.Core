using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Formulas;

namespace OSPSuite.Core.Chart
{
   public interface IChartFactory
   {
      TChartType Create<TChartType>() where TChartType : CurveChart;

      /// <summary>
      /// Creates a chart for the <paramref name="dataRepository"/> and adds the columns. Also sets the default
      /// scaling to Log or Lin before adding columns.
      /// </summary>
      CurveChart CreateChartFor(DataRepository dataRepository, Scalings defaultYScale);

      CurveChart CreateChartFor(TableFormula tableFormula);

      /// <summary>
      /// Creates a chart for the <paramref name="dataRepository"/> and adds the columns. 
      /// Sets the default scaling to user preference.
      /// </summary>
      CurveChart CreateChartFor(DataRepository dataRepository);
   }
}
