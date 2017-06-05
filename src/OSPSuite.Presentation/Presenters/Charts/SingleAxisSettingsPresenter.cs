using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;
using OSPSuite.Presentation.Views.Charts;

namespace OSPSuite.Presentation.Presenters.Charts
{
   public interface ISingleAxisSettingsPresenter : IPresenter<ISingleAxisSettingsView>, IDisposablePresenter
   {
      /// <summary>
      /// returns all the valid dimensions for inclusion into selection editors by the view
      /// </summary>
      /// <returns>A list of all valid dimensions</returns>
      IEnumerable<IDimension> GetDimensionsForEditor(IDimension dimension);

      /// <summary>
      /// Gets the valid units for the current dimension of the axis
      /// </summary>
      /// <returns>A list of all the valid units</returns>
      IEnumerable<string> GetUnitsForDimension();

      void Edit(Axis axis);
   }

   public class SingleAxisSettingsPresenter : AbstractDisposablePresenter<ISingleAxisSettingsView, ISingleAxisSettingsPresenter>, ISingleAxisSettingsPresenter
   {
      private readonly IDimensionFactory _dimensionFactory;
      private Axis _axis;

      public SingleAxisSettingsPresenter(ISingleAxisSettingsView view, IDimensionFactory dimensionFactory)
         : base(view)
      {
         _dimensionFactory = dimensionFactory;
      }

      public void Edit(Axis axis)
      {
         _axis = axis;
         _view.BindToSource(axis);

         if (axis.AxisType.IsXAxis())
            _view.HideDefaultStyles();
      }

      public IEnumerable<IDimension> GetDimensionsForEditor(IDimension dimension)
      {
         return _dimensionFactory.AllDimensionsForEditors(dimension);
      }

      public IEnumerable<string> GetUnitsForDimension()
      {
         return _axis.Dimension != null ? _axis.Dimension.GetUnitNames() : Enumerable.Empty<string>();
      }
   }
}
