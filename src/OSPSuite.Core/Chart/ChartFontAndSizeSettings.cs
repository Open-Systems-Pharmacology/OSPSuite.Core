using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq.Expressions;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Reflection;
using OSPSuite.Utility.Validation;

namespace OSPSuite.Core.Chart
{
   public class ChartFontAndSizeSettings : Notifier, IValidatable, IUpdatable
   {
      private int? _chartWidth;
      private int? _chartHeight;
      private ChartFonts _fonts;
      public virtual IBusinessRuleSet Rules { get; }

      public ChartFontAndSizeSettings()
      {
         ChartWidth = null;
         ChartHeight = null;

         Fonts = new ChartFonts();
         Rules = new BusinessRuleSet(ValidationRules.AllRules());
      }

      public int? ChartWidth
      {
         get => _chartWidth;
         set => SetProperty(ref _chartWidth, value);
      }

      public int? ChartHeight
      {
         get => _chartHeight;
         set => SetProperty(ref _chartHeight, value);
      }

      public ChartFonts Fonts
      {
         get => _fonts;
         set => SetProperty(ref _fonts, value);
      }

      public void Reset()
      {
         UpdatePropertiesFrom(new ChartFontAndSizeSettings());
      }

      public bool SizeIsDefined => ChartWidth.GetValueOrDefault() > 0 && ChartHeight.GetValueOrDefault() > 0;

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

      public Font FontFor(Func<ChartFonts, int> fontSizeFunc)
      {
         return FontFor(fontSizeFunc(Fonts));
      }

      public Font FontFor(int fontSize)
      {
         return new Font(Fonts.FontFamilyName, fontSize);
      }

      private static class ValidationRules
      {
         private const int MINIMUM_EXPORT_SIZE = 200;
         private const int MAXIMUM_EXPORT_SIZE = 2000;

         internal static IEnumerable<IBusinessRule> AllRules()
         {
            yield return chartHeightIsValid;
            yield return chartWitdhIsValid;
         }

         private static IBusinessRule chartHeightIsValid { get; } = emptyOrGreaterThanMinSizeAndLessThanMaxSizePixels<ChartFontAndSizeSettings>(x => x.ChartHeight);
         private static IBusinessRule chartWitdhIsValid { get; } = emptyOrGreaterThanMinSizeAndLessThanMaxSizePixels<ChartFontAndSizeSettings>(x => x.ChartWidth);

         private static IBusinessRule emptyOrGreaterThanMinSizeAndLessThanMaxSizePixels<T>(Expression<Func<T, int?>> property)
         {
            return CreateRule.For<T>()
               .Property(property)
               .WithRule((o, v) => v == null || (v >= MINIMUM_EXPORT_SIZE && v <= MAXIMUM_EXPORT_SIZE))
               .WithError(Validation.ValueGreaterThanMinSizeInPixelAndLessThanMaxSizeIsRequiredOrEmpty(MINIMUM_EXPORT_SIZE, MAXIMUM_EXPORT_SIZE));
         }
      }
   }
}