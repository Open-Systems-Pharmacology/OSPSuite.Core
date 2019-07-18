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
      InVitro,
      InVivo
   }

   public static class ValueOriginDeterminationMethods
   {
      private static readonly Cache<ValueOriginDeterminationMethodId, ValueOriginDeterminationMethod> _allValueOriginDeterminationMethods = new Cache<ValueOriginDeterminationMethodId, ValueOriginDeterminationMethod>(x => x.Id);

      public static IReadOnlyCollection<ValueOriginDeterminationMethod> All => _allValueOriginDeterminationMethods;

      public static ValueOriginDeterminationMethod Assumption = create(ValueOriginDeterminationMethodId.Assumption, Captions.ValueOrigins.Methods.Assumption, IconNames.VALUE_ORIGIN_METHOD_ASSUMPTION);
      public static ValueOriginDeterminationMethod InVitro = create(ValueOriginDeterminationMethodId.InVitro, Captions.ValueOrigins.Methods.InVitro, IconNames.VALUE_ORIGIN_METHOD_IN_VITRO);
      public static ValueOriginDeterminationMethod InVivo = create(ValueOriginDeterminationMethodId.InVivo, Captions.ValueOrigins.Methods.InVivo, IconNames.VALUE_ORIGIN_METHOD_IN_VIVO);
      public static ValueOriginDeterminationMethod ManualFit = create(ValueOriginDeterminationMethodId.ManualFit, Captions.ValueOrigins.Methods.ManualFit, IconNames.VALUE_ORIGIN_METHOD_MANUAL_FIT);
      public static ValueOriginDeterminationMethod ParameterIdentification = create(ValueOriginDeterminationMethodId.ParameterIdentification, Captions.ValueOrigins.Methods.ParameterIdentification, IconNames.VALUE_ORIGIN_METHOD_PARAMETER_IDENTIFICATION);
      public static ValueOriginDeterminationMethod Other = create(ValueOriginDeterminationMethodId.Other, Captions.ValueOrigins.Other, IconNames.VALUE_ORIGIN_METHOD_OTHER);
      public static ValueOriginDeterminationMethod Unknown = create(ValueOriginDeterminationMethodId.Unknown, Captions.ValueOrigins.Unknown, IconNames.VALUE_ORIGIN_METHOD_UNKNOWN);
      public static ValueOriginDeterminationMethod Undefined = create(ValueOriginDeterminationMethodId.Undefined, Captions.ValueOrigins.Undefined);

      public static ValueOriginDeterminationMethod ById(ValueOriginDeterminationMethodId valueOriginDeterminationMethodId)
      {
         return _allValueOriginDeterminationMethods[valueOriginDeterminationMethodId];
      }

      private static ValueOriginDeterminationMethod create(ValueOriginDeterminationMethodId valueOriginDeterminationMethodId, string display, string iconName = null )
      {
         var valueOriginType = new ValueOriginDeterminationMethod(valueOriginDeterminationMethodId, display, iconName);
         _allValueOriginDeterminationMethods.Add(valueOriginType);
         return valueOriginType;
      }
   }

   public class ValueOriginDeterminationMethod : IWithIcon
   {
      public ValueOriginDeterminationMethodId Id { get; }
      public string Display { get; }
      public string IconName { get; }

      internal ValueOriginDeterminationMethod(ValueOriginDeterminationMethodId id, string display, string iconName)
      {
         Id = id;
         Display = display;
         IconName = iconName;
      }

      public override string ToString()
      {
         return Display;
      }
   }
}