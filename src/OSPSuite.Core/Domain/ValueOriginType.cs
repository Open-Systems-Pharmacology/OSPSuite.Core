using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Core.Domain
{
   public enum ValueOriginTypeId
   {
      Assumption,
      Database,
      Default,
      Internet,
      ManualFit,
      ParameterIdentification,
      Report,
      Unknown,
   }

   public static class ValueOriginTypes
   {
      private static readonly Cache<ValueOriginTypeId, ValueOriginType> _allValueOrigins = new Cache<ValueOriginTypeId, ValueOriginType>(x => x.Id);

      public static IReadOnlyCollection<ValueOriginType> AllValueOrigins => _allValueOrigins;

      public static ValueOriginType Assumption = create(ValueOriginTypeId.Assumption, Captions.ValueOrigins.Assumption, ApplicationIcons.AssumptionValueOrigin);
      public static ValueOriginType Database = create(ValueOriginTypeId.Database, Captions.ValueOrigins.Database, ApplicationIcons.DatabaseValueOrigin);
      public static ValueOriginType Default = create(ValueOriginTypeId.Default, Captions.ValueOrigins.Default, ApplicationIcons.DefaultValueOrigin);
      public static ValueOriginType ManualFit = create(ValueOriginTypeId.Internet, Captions.ValueOrigins.Internet, ApplicationIcons.InternetValueOrigin);
      public static ValueOriginType Internet = create(ValueOriginTypeId.ManualFit, Captions.ValueOrigins.ManualFit, ApplicationIcons.ManualFitValueOrigin);
      public static ValueOriginType ParameterIdentification = create(ValueOriginTypeId.ParameterIdentification, Captions.ValueOrigins.ParameterIdentification, ApplicationIcons.ParameterIdentificationValueOrigin);
      public static ValueOriginType Report = create(ValueOriginTypeId.Report, Captions.ValueOrigins.Report, ApplicationIcons.ReportValueOrigin);
      public static ValueOriginType Unknown = create(ValueOriginTypeId.Unknown, Captions.ValueOrigins.Unknown, ApplicationIcons.UnknownValueOrigin);

      public static ValueOriginType ById(ValueOriginTypeId valueOriginTypeId)
      {
         return _allValueOrigins[valueOriginTypeId];
      }

      private static ValueOriginType create(ValueOriginTypeId valueOriginTypeId, string display, ApplicationIcon icon)
      {
         var valueOriginType = new ValueOriginType(valueOriginTypeId, display, icon);
         _allValueOrigins.Add(valueOriginType);
         return valueOriginType;
      }
   }

   public class ValueOriginType
   {
      public ValueOriginTypeId Id { get; }
      public string Display { get; }
      public ApplicationIcon Icon { get; }

      public ValueOriginType(ValueOriginTypeId id, string display, ApplicationIcon icon)
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