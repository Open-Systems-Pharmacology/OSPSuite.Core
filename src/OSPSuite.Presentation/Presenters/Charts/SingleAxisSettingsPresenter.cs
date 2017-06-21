using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.Views.Charts;

namespace OSPSuite.Presentation.Presenters.Charts
{
   public interface ISingleAxisSettingsPresenter : IPresenter<ISingleAxisSettingsView>, IDisposablePresenter
   {
      /// <summary>
      ///    returns all the valid dimensions for inclusion into selection editors by the view
      /// </summary>
      /// <returns>A list of all valid dimensions</returns>
      IEnumerable<IDimension> AllDimensionsForEditor(IDimension dimension);

      /// <summary>
      ///    Gets the valid units for the current dimension of the axis
      /// </summary>
      /// <returns>A list of all the valid units</returns>
      IEnumerable<string> AllUnitsForDimension();

      void Edit(IChart chart, Axis axis);
   }

   public class SingleAxisSettingsPresenter : AbstractDisposablePresenter<ISingleAxisSettingsView, ISingleAxisSettingsPresenter>, ISingleAxisSettingsPresenter
   {
      private readonly IDimensionFactory _dimensionFactory;
      private readonly IChartUpdater _chartUpdater;
      private Axis _axisClone;

      public SingleAxisSettingsPresenter(ISingleAxisSettingsView view, IDimensionFactory dimensionFactory, IChartUpdater chartUpdater)
         : base(view)
      {
         _dimensionFactory = dimensionFactory;
         _chartUpdater = chartUpdater;
      }

      public void Edit(IChart chart, Axis axis)
      {
         _axisClone = axis.Clone();
         _view.BindTo(_axisClone);

         if (axis.IsXAxis)
            _view.HideDefaultStyles();

         _view.Display();
         if (_view.Canceled)
            return;

         axis.UpdateFrom(_axisClone);
         _chartUpdater.Update(chart);
      }

      public IEnumerable<IDimension> AllDimensionsForEditor(IDimension dimension)
      {
         return _dimensionFactory.AllDimensionsForEditors(dimension);
      }

      public IEnumerable<string> AllUnitsForDimension()
      {
         return _axisClone.Dimension?.GetUnitNames() ?? Enumerable.Empty<string>();
      }
   }
}