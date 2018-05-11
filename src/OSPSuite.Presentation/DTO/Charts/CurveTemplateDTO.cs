using System.Collections.Generic;
using System.Drawing;
using OSPSuite.Utility.Validation;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;

namespace OSPSuite.Presentation.DTO.Charts
{
   public class CurveTemplateDTO : DxValidatableDTO
   {
      public CurveTemplate CurveTemplate { get; private set; }

      public CurveTemplateDTO(CurveTemplate curveTemplate)
      {
         CurveTemplate = curveTemplate;
         Rules.AddRange(AllRules.All);
      }

      public string Name
      {
         get => CurveTemplate.Name;
         set => CurveTemplate.Name = value;
      }

      public string xDataPath
      {
         get => CurveTemplate.xData.Path;
         set => CurveTemplate.xData.Path = value;
      }

      public QuantityType xQuantityType
      {
         get => CurveTemplate.xData.QuantityType;
         set => CurveTemplate.xData.QuantityType = value;
      }

      public string xRepositoryName
      {
         get => CurveTemplate.xData.RepositoryName;
         set => CurveTemplate.xData.RepositoryName = value;
      }

      public string yDataPath
      {
         get => CurveTemplate.yData.Path;
         set => CurveTemplate.yData.Path = value;
      }

      public QuantityType yQuantityType
      {
         get => CurveTemplate.yData.QuantityType;
         set => CurveTemplate.yData.QuantityType = value;
      }

      public string yRepositoryName
      {
         get => CurveTemplate.yData.RepositoryName;
         set => CurveTemplate.yData.RepositoryName = value;
      }

      public LineStyles LineStyle
      {
         get => CurveTemplate.CurveOptions.LineStyle;
         set => CurveTemplate.CurveOptions.LineStyle = value;
      }

      public Color Color
      {
         get => CurveTemplate.CurveOptions.Color;
         set => CurveTemplate.CurveOptions.Color = value;
      }

      public bool Visible
      {
         get => CurveTemplate.CurveOptions.Visible;
         set => CurveTemplate.CurveOptions.Visible = value;
      }

      public int LineThickness
      {
         get => CurveTemplate.CurveOptions.LineThickness;
         set => CurveTemplate.CurveOptions.LineThickness = value;
      }

      public Symbols Symbol
      {
         get => CurveTemplate.CurveOptions.Symbol;
         set => CurveTemplate.CurveOptions.Symbol = value;
      }

      private static class AllRules
      {
         public static IEnumerable<IBusinessRule> All
         {
            get
            {
               yield return GenericRules.NonEmptyRule<CurveTemplateDTO>(x => x.xDataPath);
               yield return GenericRules.NonEmptyRule<CurveTemplateDTO>(x => x.yDataPath);
               yield return GenericRules.NonEmptyRule<CurveTemplateDTO>(x => x.Name);
            }
         }
      }
   }
}