using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Utility;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
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

   public interface IAxisSettingsPresenter : IPresenterWithColumnSettings, IPresenter<IAxisSettingsView>
   {
      void SetDataSource(ICache<AxisTypes, IAxis> value);

      void RemoveAxis(AxisTypes axisType);
      void AddYAxis();

      IEnumerable<IDimension> GetDimensions(IDimension dimension);
      IEnumerable<string> GetUnitNamesFor(IDimension dimension);
   }

   /// <summary>
   ///    The AxisSettings component displays a list of chart axes and allows for editing their properties and inserting or
   ///    deleting curves from the list.
   /// </summary>
   internal class AxisSettingsPresenter : PresenterWithColumnSettings<IAxisSettingsView, IAxisSettingsPresenter>, IAxisSettingsPresenter
   {
      private readonly IDimensionFactory _dimensionFactory;
      private readonly IPresentationUserSettings _commonUserSettings;
      private ICache<AxisTypes, IAxis> _axes;

      public AxisSettingsPresenter(IAxisSettingsView view, IDimensionFactory dimensionFactory, IPresentationUserSettings commonUserSettings)
         : base(view)
      {
         _dimensionFactory = dimensionFactory;
         _commonUserSettings = commonUserSettings;
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

      public void SetDataSource(ICache<AxisTypes, IAxis> value)
      {
         _axes = value;
         if (_axes == null)
            _view.DeleteBinding();
         else
            _view.BindToSource(_axes);
      }

      public void RemoveAxis(AxisTypes axisType)
      {
         if (axisType >= AxisTypes.Y2 && _axes.Contains(axisType + 1))
            throw new OSPSuiteException("Remove higher AxisType first!");
         _axes.Remove(axisType);
      }

      public void AddYAxis()
      {
         foreach (var axisType in EnumHelper.AllValuesFor<AxisTypes>())
         {
            if (_axes.Contains(axisType))
               continue;

            var axis = new Axis(axisType);
            if (axis.IsYAxis())
               axis.Scaling = _commonUserSettings.DefaultChartYScaling;

            _axes.Add(axis);
            return;
         }
      }

      public IEnumerable<IDimension> GetDimensions(IDimension dimension)
      {
         return _dimensionFactory.GetAllDimensionsForEditors(dimension);
      }

      public IEnumerable<string> GetUnitNamesFor(IDimension dimension)
      {
         if(dimension==null)
            return Enumerable.Empty<string>();

         return dimension.GetUnitNames();
      }
   }
}