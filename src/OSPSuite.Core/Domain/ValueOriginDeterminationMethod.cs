using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Core.Domain
{
   public enum ValueOriginDeterminationMethodId
   {
      Assumption,
      ManualFit,
      ParameterIdentification,
      Other,
      Undefined,
      Unknown,
      InVitroAssay,
      Measurement
   }

   public static class ValueOriginDeterminationMethods
   {
      private static readonly Cache<ValueOriginDeterminationMethodId, ValueOriginDeterminationMethod> _allValueOriginDeterminationMethods = new Cache<ValueOriginDeterminationMethodId, ValueOriginDeterminationMethod>(x => x.Id);
      public static IReadOnlyCollection<ValueOriginDeterminationMethod> All => _allValueOriginDeterminationMethods;

      public static ValueOriginDeterminationMethod Assumption = create(ValueOriginDeterminationMethodId.Assumption, Captions.ValueOrigins.Methods.Assumption, ApplicationIcons.AssumptionValueOrigin);
      public static ValueOriginDeterminationMethod ManualFit = create(ValueOriginDeterminationMethodId.ManualFit, Captions.ValueOrigins.Methods.ManualFit, ApplicationIcons.ManualFitValueOrigin);
      public static ValueOriginDeterminationMethod ParameterIdentification = create(ValueOriginDeterminationMethodId.ParameterIdentification, Captions.ValueOrigins.Methods.ParameterIdentification, ApplicationIcons.ParameterIdentificationValueOrigin);
      public static ValueOriginDeterminationMethod Undefined = create(ValueOriginDeterminationMethodId.Undefined, Captions.ValueOrigins.Undefined, ApplicationIcons.EmptyIcon);
      public static ValueOriginDeterminationMethod Unknown = create(ValueOriginDeterminationMethodId.Unknown, Captions.ValueOrigins.Unknown, ApplicationIcons.UnknownValueOrigin);
      public static ValueOriginDeterminationMethod Other = create(ValueOriginDeterminationMethodId.Other, Captions.ValueOrigins.Other, ApplicationIcons.OtherValueOrigin);
      public static ValueOriginDeterminationMethod InVitroAssay = create(ValueOriginDeterminationMethodId.InVitroAssay, Captions.ValueOrigins.Methods.InVitroAssay, ApplicationIcons.InVitroAssayValueOrigin);
      public static ValueOriginDeterminationMethod Measurement = create(ValueOriginDeterminationMethodId.Measurement, Captions.ValueOrigins.Methods.Measurement, ApplicationIcons.MeasurementValueOrigin);

      public static ValueOriginDeterminationMethod ById(ValueOriginDeterminationMethodId valueOriginDeterminationMethodId)
      {
         return _allValueOriginDeterminationMethods[valueOriginDeterminationMethodId];
      }

      private static ValueOriginDeterminationMethod create(ValueOriginDeterminationMethodId valueOriginDeterminationMethodId, string display, ApplicationIcon icon)
      {
         var valueOriginType = new ValueOriginDeterminationMethod(valueOriginDeterminationMethodId, display, icon);
         _allValueOriginDeterminationMethods.Add(valueOriginType);
         return valueOriginType;
      }
   }

   public class ValueOriginDeterminationMethod
   {
      public ValueOriginDeterminationMethodId Id { get; }
      public string Display { get; }
      public ApplicationIcon Icon { get; }

      internal ValueOriginDeterminationMethod(ValueOriginDeterminationMethodId id, string display, ApplicationIcon icon)
      {
         Id = id;
         Display = display;
         Icon = icon;
      }

      public override string ToString()
      {
         return Display;
      }
   }
}