using System;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain
{
   public class ValueOrigin : IComparable<ValueOrigin>, IComparable
   {
      /// <summary>
      /// Default Value origin for all parameters
      /// </summary>
      public static ValueOrigin Undefined = new ValueOrigin();

      /// <summary>
      /// Unknown value origin that will be used as soon as a default value is being overwritten in PK-Sim or MoBi
      /// </summary>
      public static ValueOrigin Unknown = new ValueOrigin{Source = ValueOriginSources.Unknown, Method = ValueOriginDeterminationMethods.Undefined};

      
      /// <summary>
      ///    Key computed based on properties to assess if values origin are equals
      /// </summary>
      private string _key;

      private ValueOriginSource _source = ValueOriginSources.Undefined;
      private ValueOriginDeterminationMethod _method = ValueOriginDeterminationMethods.Undefined;
      private string _description;

      /// <summary>
      ///    Id of the given ValueOrigin. This is the database entry for predefined parameters and is set to null if no entry is
      ///    available
      ///    <remarks>Id is not part of equakity check</remarks>
      /// </summary>
      public int? Id { get; set; }

      /// <summary>
      ///    Source of the value
      /// </summary>
      public ValueOriginSource Source
      {
         get => _source;
         set
         {
            _source = value;
            resetKey();
         }
      }

      /// <summary>
      ///    Determination method of the value
      /// </summary>
      public ValueOriginDeterminationMethod Method
      {
         get => _method;
         set
         {
            _method = value;
            resetKey();
         }
      }

      /// <summary>
      ///    Optional description explaining the quantity value
      /// </summary>
      public string Description
      {
         get => _description;
         set
         {
            _description = value;
            resetKey();
         }
      }

      private void resetKey()
      {
         _key = null;
      }

      public ValueOrigin Clone()
      {
         var clone = new ValueOrigin();
         clone.UpdateAllFrom(this);
         return clone;
      }

      /// <summary>
      ///    Updates all properties from the value origin including Id. Typically used in the context of cloning
      /// </summary>
      public void UpdateAllFrom(ValueOrigin valueOrigin)
      {
         UpdateFrom(valueOrigin, updateId: true);
      }

      public void UpdateFrom(ValueOrigin valueOrigin, bool updateId = false)
      {
         if (valueOrigin == null)
            return;

         //Id is only updated when creating value origin from database. Otherwise it should never change.
         if (updateId)
            Id = valueOrigin.Id;

         Source = valueOrigin.Source;
         Method = valueOrigin.Method;
         Description = valueOrigin.Description;
      }

      public string Display => defaultDisplay(this);

      public int CompareTo(ValueOrigin other)
      {
         return string.Compare(key, other.key, StringComparison.Ordinal);
      }

      public int CompareTo(object obj)
      {
         return CompareTo(obj.DowncastTo<ValueOrigin>());
      }

      public override bool Equals(object obj)
      {
         return Equals(obj as ValueOrigin);
      }

      public bool Equals(ValueOrigin other)
      {
         if (ReferenceEquals(null, other)) return false;
         if (ReferenceEquals(this, other)) return true;
         return Equals(other.key, key);
      }

      public override int GetHashCode() => key.GetHashCode();

      public override string ToString() => Display;

      //TEMP to ensure that we can test the best display text from the app
      public static Func<ValueOrigin, string> DisplayFunc = defaultDisplay;

      private static string defaultDisplay(ValueOrigin valueOrigin)
      {
         if (valueOrigin.IsUndefined)
            return Captions.ValueOrigins.Undefined;

         return new[]
         {
            valueOrigin.Source.Display, valueOrigin.Method.Display, valueOrigin.Description
         }.Where(x => !string.IsNullOrWhiteSpace(x)).ToString("-");
      }

      public bool IsUndefined => Equals(Undefined);

      private string key
      {
         get
         {
            if (!string.IsNullOrEmpty(_key))
               return _key;

            _key = new[]
            {
               Source.Id.ToString(), Method.Id.ToString(), Description
            }.Where(x => !string.IsNullOrWhiteSpace(x)).ToString("-");

            return _key;
         }
      }
   }
}