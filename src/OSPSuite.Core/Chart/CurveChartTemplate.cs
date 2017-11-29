using System.Collections.Generic;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Reflection;

namespace OSPSuite.Core.Chart
{
   public class CurveChartTemplate : Notifier, IChartManagement, IUpdatable, IWithName, IWithAxes
   {
      private bool _isDefault;
      private string _name;
      private bool _previewSettings;
      private readonly Cache<AxisTypes, Axis> _axes = new Cache<AxisTypes, Axis>(x => x.AxisType, x => null);
      public IList<CurveTemplate> Curves { get; }
      public ChartSettings ChartSettings { set; get; }
      public ChartFontAndSizeSettings FontAndSize { get; set; }
      public bool IncludeOriginData { get; set; }
      public IReadOnlyCollection<Axis> Axes => _axes;

      public Axis AxisBy(AxisTypes axisTypes) => _axes[axisTypes];

      public bool HasAxis(AxisTypes axisTypes) => _axes.Contains(axisTypes);

      public CurveChartTemplate()
      {
         Curves = new List<CurveTemplate>();
         FontAndSize = new ChartFontAndSizeSettings();
         ChartSettings = new ChartSettings();
         _isDefault = false;
         _name = string.Empty;
         _previewSettings = false;
      }

      public void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         var sourceChartTemplate = source as CurveChartTemplate;
         if (sourceChartTemplate == null) return;

         FontAndSize.UpdatePropertiesFrom(sourceChartTemplate.FontAndSize, cloneManager);
         ChartSettings.UpdatePropertiesFrom(sourceChartTemplate.ChartSettings, cloneManager);
         Name = sourceChartTemplate.Name;
         _axes.Clear();
         Curves.Clear();
         sourceChartTemplate.Axes.Each(axis => AddAxis(axis.Clone()));
         sourceChartTemplate.Curves.Each(curve => Curves.Add(cloneManager.Clone(curve)));
         IsDefault = sourceChartTemplate.IsDefault;
         PreviewSettings = sourceChartTemplate.PreviewSettings;
      }

      public void AddAxis(Axis axis)
      {
         _axes.Add(axis);
      }

      public void RemoveAxis(Axis axis)
      {
         _axes.Remove(axis.AxisType);
      }

      public Axis AddNewAxisFor(AxisTypes axisType)
      {
         var axis = new Axis(axisType);
         AddAxis(axis);
         return axis;
      }

      public string Name
      {
         get => _name;
         set => SetProperty(ref _name, value);
      }

      public bool IsDefault
      {
         get => _isDefault;
         set => SetProperty(ref _isDefault, value);
      }

      public bool PreviewSettings
      {
         get => _previewSettings;
         set => SetProperty(ref _previewSettings, value);
      }
   }
}