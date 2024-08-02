using System.Collections.Generic;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Utility.Validation;

namespace OSPSuite.Presentation.DTO
{
   public class ValuePointDTO : DxValidatableDTO
   {
      protected readonly TableFormula _tableFormula;

      public ValuePoint ValuePoint { get; }

      public bool RestartSolver { get; }

      private double _x;

      //x value in display unit of the table formula
      public double X
      {
         get => _x;
         set => SetProperty(ref _x, value);
      }

      private double _y;

      //y value in display unit of the table formula
      public double Y
      {
         get => _y;
         set => SetProperty(ref _y, value);
      }


      public ValuePointDTO(TableFormula tableFormula, ValuePoint point)
      {
         _tableFormula = tableFormula;
         X = convertToDisplayUnit(tableFormula.XDimension, tableFormula.XDisplayUnit, point.X);
         Y = convertToDisplayUnit(tableFormula.Dimension, tableFormula.YDisplayUnit, point.Y);
         ValuePoint = point;
         RestartSolver = point.RestartSolver;
         Rules.AddRange(AllRules.All());
      }

      private double convertToDisplayUnit(IDimension dimension, Unit displayUnit, double value)
      {
         return dimension.BaseUnitValueToUnitValue(displayUnit, value);
      }

      private static class AllRules
      {
         private static IBusinessRule xValueShouldBeGreaterOrEqualThanZero { get; } = CreateRule.For<ValuePointDTO>()
            .Property(point => point.X)
            .WithRule((point, value) => value >= 0)
            .WithError((point, value) => Assets.Rules.Parameters.ValueShouldBeGreaterThanOrEqualToZero(point._tableFormula.XName));

         public static IEnumerable<IBusinessRule> All()
         {
            yield return xValueShouldBeGreaterOrEqualThanZero;
         }
      }
   }
}