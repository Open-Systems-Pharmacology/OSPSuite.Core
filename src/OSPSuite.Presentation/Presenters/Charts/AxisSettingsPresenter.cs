using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.Views.Charts;

namespace OSPSuite.Presentation.Presenters.Charts
{
   public enum AxisOptionsColumns
   {
      AxisType,
      Scaling,
      NumberMode,
      Dimension,
      UnitName,
      GridLines,
      Min,
      Max,
      Caption,
      DefaultLineStyle,
      DefaultColor
   }

   public class AxisEventArgs : EventArgs
   {
      public Axis Axis { get; }
      public AxisEventArgs(Axis axis) => Axis = axis;
   }

   public interface IAxisSettingsPresenter : IPresenterWithColumnSettings, IPresenter<IAxisSettingsView>
   {
      void Edit(IEnumerable<Axis> axes);
      void Clear();
      void RemoveAxis(Axis axis);
      void AddYAxis();
      IEnumerable<IDimension> AllDimensions(IDimension defaultDimension);
      IEnumerable<string> AllUnitNamesFor(IDimension dimension);
      void Refresh();
      event EventHandler AxisAdded;
      event EventHandler<AxisEventArgs> AxisRemoved;
      void NotifyAxisPropertyChanged(Axis axis);
      event EventHandler<AxisEventArgs> AxisPropertyChanged;
      void UnitChanged(Axis axis);
   }

   /// <summary>
   ///    The AxisSettings component displays a list of chart axes and allows for editing their properties and inserting or
   ///    deleting curves from the list.
   /// </summary>
   internal class AxisSettingsPresenter : PresenterWithColumnSettings<IAxisSettingsView, IAxisSettingsPresenter>, IAxisSettingsPresenter
   {
      private readonly IDimensionFactory _dimensionFactory;
      private IEnumerable<Axis> _axes;
      public event EventHandler AxisAdded = delegate { };
      public event EventHandler<AxisEventArgs> AxisRemoved = delegate { };
      public event EventHandler<AxisEventArgs> AxisPropertyChanged = delegate { };

      public AxisSettingsPresenter(IAxisSettingsView view, IDimensionFactory dimensionFactory)
         : base(view)
      {
         _dimensionFactory = dimensionFactory;
      }

      protected override void SetDefaultColumnSettings()
      {
         AddColumnSettings(AxisOptionsColumns.AxisType).WithCaption(Captions.Chart.AxisOptions.AxisType);
         AddColumnSettings(AxisOptionsColumns.Caption).WithCaption(Captions.Chart.AxisOptions.Caption);
         AddColumnSettings(AxisOptionsColumns.Dimension).WithCaption(Captions.Chart.AxisOptions.Dimension);
         AddColumnSettings(AxisOptionsColumns.UnitName).WithCaption(Captions.Chart.AxisOptions.UnitName);
         AddColumnSettings(AxisOptionsColumns.Scaling).WithCaption(Captions.Chart.AxisOptions.Scaling);
         AddColumnSettings(AxisOptionsColumns.NumberMode).WithCaption(Captions.Chart.AxisOptions.NumberMode);
         AddColumnSettings(AxisOptionsColumns.Min).WithCaption(Captions.Chart.AxisOptions.Min);
         AddColumnSettings(AxisOptionsColumns.Max).WithCaption(Captions.Chart.AxisOptions.Max);
         AddColumnSettings(AxisOptionsColumns.GridLines).WithCaption(Captions.Chart.AxisOptions.GridLines);
         AddColumnSettings(AxisOptionsColumns.DefaultLineStyle).WithCaption(Captions.Chart.AxisOptions.DefaultLinestyle);
         AddColumnSettings(AxisOptionsColumns.DefaultColor).WithCaption(Captions.Chart.AxisOptions.DefaultColor);
      }

      public void UnitChanged(Axis axis)
      {
         axis.ResetRange();
      }

      public void NotifyAxisPropertyChanged(Axis axis) => AxisPropertyChanged(this, new AxisEventArgs(axis));

      public void Refresh()
      {
         Edit(_axes);
      }

      public void Edit(IEnumerable<Axis> axes)
      {
         _axes = axes;
         if (_axes == null)
            _view.DeleteBinding();
         else
            _view.BindTo(_axes);
      }

      public void Clear()
      {
         Edit(null);
      }

      public void RemoveAxis(Axis axis)
      {
         AxisRemoved(this, new AxisEventArgs(axis));
      }

      public void AddYAxis()
      {
         AxisAdded(this, EventArgs.Empty);
      }

      public IEnumerable<IDimension> AllDimensions(IDimension defaultDimension)
      {
         return _dimensionFactory.AllDimensionsForEditors(defaultDimension);
      }

      public IEnumerable<string> AllUnitNamesFor(IDimension dimension)
      {
         if (dimension == null)
            return Enumerable.Empty<string>();

         return dimension.GetUnitNames();
      }
   }
}