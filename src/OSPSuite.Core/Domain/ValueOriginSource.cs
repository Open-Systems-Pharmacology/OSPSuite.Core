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

      public static ValueOriginSource Database = create(ValueOriginSourceId.Database, Captions.ValueOrigins.Sources.Database, ApplicationIcons.ValueOriginSourceDatabase);
      public static ValueOriginSource Internet = create(ValueOriginSourceId.Internet, Captions.ValueOrigins.Sources.Internet, ApplicationIcons.ValueOriginSourceInternet);
      public static ValueOriginSource ParameterIdentification = create(ValueOriginSourceId.ParameterIdentification, Captions.ValueOrigins.Sources.ParameterIdentification, ApplicationIcons.ValueOriginSourceParameterIdentification);
      public static ValueOriginSource Publication = create(ValueOriginSourceId.Publication, Captions.ValueOrigins.Sources.Publication, ApplicationIcons.ValueOriginSourcePublication);
      public static ValueOriginSource Other = create(ValueOriginSourceId.Other, Captions.ValueOrigins.Other, ApplicationIcons.ValueOriginSourceOther);
      public static ValueOriginSource Unknown = create(ValueOriginSourceId.Unknown, Captions.ValueOrigins.Unknown, ApplicationIcons.ValueOriginSourceUnknown);
      public static ValueOriginSource Undefined = create(ValueOriginSourceId.Undefined, Captions.ValueOrigins.Undefined, ApplicationIcons.EmptyIcon);

      public static ValueOriginSource ById(ValueOriginSourceId valueOriginSourceId)
      {
         return _allValueOriginSources[valueOriginSourceId];
      }

      private static ValueOriginSource create(ValueOriginSourceId valueOriginSourceId, string display, ApplicationIcon icon)
      {
         var valueOriginType = new ValueOriginSource(valueOriginSourceId, display, icon);
         _allValueOriginSources.Add(valueOriginType);
         return valueOriginType;
      }
   }

   public class ValueOriginSource
   {
      public ValueOriginSourceId Id { get; }
      public string Display { get; }
      public ApplicationIcon Icon { get; }

      internal ValueOriginSource(ValueOriginSourceId id, string display, ApplicationIcon icon)
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