using System;
using System.Drawing;
using DevExpress.XtraEditors.DXErrorProvider;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;

namespace OSPSuite.Presentation.DTO.ParameterIdentifications
{
   public abstract class ConstraintParameterDTO : ValidatableDTO, IDXDataErrorInfo, IWithName
   {
      public string Name { get; set; }
      public ValueDTO MinValue { get; set; }
      public ValueDTO MaxValue { get; set; }
      public bool NeedBoundaryCheck { get; set; } = true;

      public bool ValueIsCloseToBoundary => valueIsCloseToBoundary(ValueForBoundaryCheck, MinValue.Value, MaxValue.Value);
      public abstract double ValueForBoundaryCheck { get; }

      private bool valueIsCloseToBoundary(double value, double min, double max)
      {
         if (!NeedBoundaryCheck)
            return false;

         return valueIsOutOfBounds(value, min, max) || valueIsCloseTo(value, min) || valueIsCloseTo(value, max);
      }

      private bool valueIsOutOfBounds(double value, double min, double max)
      {
         return value < min || value > max;
      }

      private bool valueIsCloseTo(double value, double boundary)
      {
         if (double.IsInfinity(boundary))
            return false;

         return Math.Abs(value - boundary) <= boundary * Constants.TOO_CLOSE_TO_BOUNDARY_FACTOR;
      }

      public void GetPropertyError(string propertyName, ErrorInfo info)
      {
         return;
      }

      public ApplicationIcon BoundaryCheckIcon
      {
         get
         {
            if (!NeedBoundaryCheck)
               return ApplicationIcons.EmptyIcon;

            return ValueIsCloseToBoundary ? ApplicationIcons.Warning : ApplicationIcons.OK;
         }
      }

      public void GetError(ErrorInfo info)
      {
         if (!ValueIsCloseToBoundary) return;

         info.ErrorText = Warning.OptimizedValueIsCloseToBoundary;
         info.ErrorType = ErrorType.Warning;
      }

      public Image RangeImage { get; set; }
   }
}