using System.Drawing;
using OSPSuite.Assets;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Utility.Validation;

namespace OSPSuite.Presentation.DTO.Charts;

public class SelectedCurveValues : IValidatable
{
   public Color? Color { get; set; }
   public LineStyles? Style { get; set; }
   public Symbols? Symbol { get; set; }
   public int? LineThickness { get; set; }
   public bool? Visible { get; set; }
   public bool? VisibleInLegend { get; set; }
   public IBusinessRuleSet Rules { get; } = new BusinessRuleSet(lineThicknessUnsetOrWithinLimits);

   private static IBusinessRule lineThicknessUnsetOrWithinLimits { get; } = CreateRule.For<SelectedCurveValues>()
      .Property(x => x.LineThickness)
      .WithRule((selectedValues, lineThickness) => lineThickness == null || (lineThickness >= Constants.MIN_LINE_THICKNESS && lineThickness <= Constants.MAX_LINE_THICKNESS))
      .WithError(Validation.LineThicknessShouldBeBetween(Constants.MIN_LINE_THICKNESS, Constants.MAX_LINE_THICKNESS));
}