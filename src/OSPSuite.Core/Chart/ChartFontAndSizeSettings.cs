using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using OSPSuite.Assets;
using OSPSuite.Utility.Reflection;
using OSPSuite.Utility.Validation;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Chart
{
   public class ChartFontAndSizeSettings : Notifier, IValidatable, IUpdatable
   {
      public readonly static ChartFontAndSizeSettings Default = new ChartFontAndSizeSettings();

      private int? _chartWidth;
      private int? _chartHeight;
      private readonly IBusinessRuleSet _rules;
      private ChartFonts _fonts;

      public int? ChartWidth
      {
         get { return _chartWidth; }
         set
         {
            _chartWidth = value;
            OnPropertyChanged(() => ChartWidth);
         }
      }

      public int? ChartHeight
      {
         get { return _chartHeight; }
         set
         {
            _chartHeight = value;
            OnPropertyChanged(() => ChartHeight);
         }
      }

      public ChartFonts Fonts
      {
         get { return _fonts; }
         private set
         {
            _fonts = value;
            Fonts.PropertyChanged += (o, e) => OnPropertyChanged(() => Fonts);
         }
      }

      public ChartFontAndSizeSettings()
      {
         ChartWidth = null;
         ChartHeight = null;

         Fonts = new ChartFonts();
         _rules = DefaultRules();
      }

      public void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         UpdatePropertiesFrom(source);
      }

      public void UpdatePropertiesFrom(IUpdatable source)
      {
         var sourceChartFontAndSizeSettings = source as ChartFontAndSizeSettings;
         if (sourceChartFontAndSizeSettings == null) return;

         ChartWidth = sourceChartFontAndSizeSettings.ChartWidth;
         ChartHeight = sourceChartFontAndSizeSettings.ChartHeight;
         Fonts.UpdateSettingsFrom(sourceChartFontAndSizeSettings.Fonts);
      }

      protected virtual IBusinessRuleSet DefaultRules()
      {
         return new BusinessRuleSet(ValidationRules.AllRules());
      }

      private static class ValidationRules
      {
         private const int MINIMUM_EXPORT_SIZE = 200;
         private const int MAXIMUM_EXPORT_SIZE = 2000;

         internal static IEnumerable<IBusinessRule> AllRules()
         {
            yield return emptyOrGreaterThanMinSizeAndLessThanMaxSizePixels<ChartFontAndSizeSettings>(x => x.ChartHeight);
            yield return emptyOrGreaterThanMinSizeAndLessThanMaxSizePixels<ChartFontAndSizeSettings>(x => x.ChartWidth);
         }

         private static IBusinessRule emptyOrGreaterThanMinSizeAndLessThanMaxSizePixels<T>(Expression<Func<T, int?>> property)
         {
            return CreateRule.For<T>()
               .Property(property)
               .WithRule((o, v) => v == null || (v >= MINIMUM_EXPORT_SIZE && v <= MAXIMUM_EXPORT_SIZE))
               .WithError(Validation.ValueGreaterThanMinSizeInPixelAndLessThanMaxSizeIsRequiredOrEmpty(MINIMUM_EXPORT_SIZE, MAXIMUM_EXPORT_SIZE));
         }
      }

      public virtual IBusinessRuleSet Rules
      {
         get { return _rules; }
      }

      public void Reset()
      {
         UpdatePropertiesFrom(new ChartFontAndSizeSettings());
      }
   }
}