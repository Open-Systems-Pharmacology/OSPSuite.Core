using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Core.Domain
{
   public enum ValueOriginSourceId
   {
      Database,
      Internet,
      ParameterIdentification,
      Publication,
      Other,
      Undefined,
      Unknown,
   }

   public static class ValueOriginSources
   {
      private static readonly Cache<ValueOriginSourceId, ValueOriginSource> _allValueOriginSources = new Cache<ValueOriginSourceId, ValueOriginSource>(x => x.Id);

      public static IReadOnlyCollection<ValueOriginSource> All => _allValueOriginSources;

      public static ValueOriginSource Database = create(ValueOriginSourceId.Database, Captions.ValueOrigins.Sources.Database, IconNames.VALUE_ORIGIN_SOURCE_DATABASE);
      public static ValueOriginSource Internet = create(ValueOriginSourceId.Internet, Captions.ValueOrigins.Sources.Internet, IconNames.VALUE_ORIGIN_SOURCE_INTERNET);
      public static ValueOriginSource ParameterIdentification = create(ValueOriginSourceId.ParameterIdentification, Captions.ValueOrigins.Sources.ParameterIdentification, IconNames.VALUE_ORIGIN_SOURCE_PARAMETER_IDENTIFICATION);
      public static ValueOriginSource Publication = create(ValueOriginSourceId.Publication, Captions.ValueOrigins.Sources.Publication, IconNames.VALUE_ORIGIN_SOURCE_PUBLICATION);
      public static ValueOriginSource Other = create(ValueOriginSourceId.Other, Captions.ValueOrigins.Other, IconNames.VALUE_ORIGIN_SOURCE_OTHER);
      public static ValueOriginSource Unknown = create(ValueOriginSourceId.Unknown, Captions.ValueOrigins.Unknown, IconNames.VALUE_ORIGIN_SOURCE_UNKNOWN);
      public static ValueOriginSource Undefined = create(ValueOriginSourceId.Undefined, Captions.ValueOrigins.Undefined);

      public static ValueOriginSource ById(ValueOriginSourceId valueOriginSourceId)
      {
         return _allValueOriginSources[valueOriginSourceId];
      }

      private static ValueOriginSource create(ValueOriginSourceId valueOriginSourceId, string display, string iconName = null)
      {
         var valueOriginType = new ValueOriginSource(valueOriginSourceId, display, iconName);
         _allValueOriginSources.Add(valueOriginType);
         return valueOriginType;
      }
   }

   public class ValueOriginSource : IWithIcon
   {
      public ValueOriginSourceId Id { get; }
      public string Display { get; }
      public string IconName { get; }

      internal ValueOriginSource(ValueOriginSourceId id, string display, string iconName)
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