using System;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain
{
   public class ValueOrigin
   {
      //Id of the given ValueOrigin. This is the database entry for predefined parameters and is set to null if no entry is available
      public int? Id { get; set; }

      /// <summary>
      ///    Indicates if a parameter value is a default value (e.g. coming from the PK-Sim database) or if the value was entered
      ///    or modified by the user.
      ///    Default value is <c>false</c>
      /// </summary>
      public bool Default { get; set; }

      /// <summary>
      ///    Source of the value
      /// </summary>
      public ValueOriginSource Source { get; set; } = ValueOriginSources.Undefined;

      /// <summary>
      ///    Determination method of the value
      /// </summary>
      public ValueOriginDeterminationMethod Method { get; set; } = ValueOriginDeterminationMethods.Undefined;

      /// <summary>
      ///    Optional description explaining the quantity value
      /// </summary>
      public string Description { get; set; }

      public ValueOrigin Clone()
      {
         var clone = new ValueOrigin {Id = Id};
         clone.UpdateFrom(this);
         return clone;
      }

      public void UpdateFrom(ValueOrigin valueOrigin)
      {
         if (valueOrigin == null)
            return;

         //do not updated Id in this method
         Source = valueOrigin.Source;
         Method = valueOrigin.Method;
         Description = valueOrigin.Description;
         Default = valueOrigin.Default;
      }

      public string Display => defaultDisplay(this);

      public override string ToString() => Display;

      //TEMP to ensure that we can test the best display text from the app
      public static Func<ValueOrigin, string> DisplayFunc = defaultDisplay;

      private static string defaultDisplay(ValueOrigin valueOrigin)
      {
         if (isUndefined(valueOrigin))
            return Captions.ValueOrigins.Undefined;

         return new[]
         {
            valueOrigin.Source.Display, valueOrigin.Method.Display, valueOrigin.Description
         }.Where(x => !string.IsNullOrWhiteSpace(x)).ToString("-");
      }

      private static bool isUndefined(ValueOrigin valueOrigin)
      {
         if (valueOrigin.Source == null || valueOrigin.Method == null)
            return true;

         return valueOrigin.Source == ValueOriginSources.Undefined &&
                valueOrigin.Method == ValueOriginDeterminationMethods.Undefined &&
                string.IsNullOrEmpty(valueOrigin.Description);
      }
   }
}