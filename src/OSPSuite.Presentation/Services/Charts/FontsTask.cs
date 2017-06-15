using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OSPSuite.Core.Domain;

namespace OSPSuite.Presentation.Services.Charts
{
   public interface IFontsTask
   {
      IReadOnlyList<string> SystemFontFamilyNames { get; }
      string DefaultSansSerifFontName { get; }
      IReadOnlyList<string> ChartFontFamilyNames { get; }
   }

   public class FontsTask : IFontsTask
   {
      private readonly Func<IEnumerable<string>> _systemFontFamilyFunc;
      private readonly Func<string> _defaultSansSerifFontNameFunc;

      public FontsTask() : this(() => FontFamily.Families.Select(x => x.Name), () => FontFamily.GenericSansSerif.Name)
      {
      }

      internal FontsTask(Func<IEnumerable<string>> systemFontFamilyFunc, Func<string> defaultSansSerifFontNameFunc)
      {
         _systemFontFamilyFunc = systemFontFamilyFunc;
         _defaultSansSerifFontNameFunc = defaultSansSerifFontNameFunc;
      }

      public IReadOnlyList<string> SystemFontFamilyNames => _systemFontFamilyFunc().ToList();

      public string DefaultSansSerifFontName => _defaultSansSerifFontNameFunc();

      public IReadOnlyList<string> ChartFontFamilyNames => new HashSet<string>(SystemFontFamilyNames.Intersect(Constants.ChartFontOptions.AllFontFamilies)) {DefaultSansSerifFontName}.ToList();
   }
}