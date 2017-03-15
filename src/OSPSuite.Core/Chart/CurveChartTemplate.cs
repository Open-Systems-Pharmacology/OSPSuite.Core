using System.Collections.Generic;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Reflection;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Chart
{
   public class CurveChartTemplate : Notifier, IChartManagement, IUpdatable, IWithName
   {
      private bool _isDefault;
      private string _name;
      private bool _previewSettings;

      public ICache<AxisTypes, IAxis> Axes { get; private set; }
      public IList<CurveTemplate> Curves { get; private set; }
      public ChartSettings ChartSettings { set; get; }
      public ChartFontAndSizeSettings FontAndSize { get; set; }
      public bool IncludeOriginData { get; set; }

      public CurveChartTemplate()
      {
         Axes = new Cache<AxisTypes, IAxis>(x => x.AxisType);
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
         Axes.Clear();
         Curves.Clear();
         sourceChartTemplate.Axes.Each(axis => Axes.Add(axis.Clone()));
         sourceChartTemplate.Curves.Each(curve => Curves.Add(cloneManager.Clone(curve)));
         IsDefault = sourceChartTemplate.IsDefault;
         PreviewSettings = sourceChartTemplate.PreviewSettings;
      }

      public string Name
      {
         get { return _name; }
         set
         {
            _name = value;
            OnPropertyChanged(() => Name);
         }
      }

      public bool IsDefault
      {
         get { return _isDefault; }
         set
         {
            _isDefault = value;
            OnPropertyChanged(() => IsDefault);
         }
      }

      public bool PreviewSettings
      {
         get { return _previewSettings; }
         set
         {
            _previewSettings = value;
            OnPropertyChanged(() => PreviewSettings);
         }
      }
   }
}