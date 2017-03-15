namespace OSPSuite.Core.Importer
{
   public struct InputParameter
   {
      public string Name { get; set; }

      private string _displayName;

      public string DisplayName
      {
         get { return string.IsNullOrEmpty(_displayName) ? Name : _displayName; }
         set { _displayName = value; }
      }

      public double? Value { get; set; }

      public Unit Unit { get; set; }

      public double? MinValue { get; set; }

      public bool MinValueAllowed { get; set; }

      public double? MaxValue { get; set; }

      public bool MaxValueAllowed { get; set; }

      public bool IsValueValid(double valueToCheck)
      {
         if (MinValue == null && MaxValue == null) return true;

         var retVal = true;
         if (MinValue != null && MinValueAllowed) retVal &= (valueToCheck >= MinValue);
         if (MinValue != null && !MinValueAllowed) retVal &= (valueToCheck > MinValue);
         if (MaxValue != null && MaxValueAllowed) retVal &= (valueToCheck <= MaxValue);
         if (MaxValue != null && !MaxValueAllowed) retVal &= (valueToCheck < MaxValue);
         return retVal;
      }

      #region some method for comparing input parameters

      /// <summary>
      ///    This method determines whether two objects are the same.
      /// </summary>
      /// <remarks>
      ///    The value is not checked. That means two input parameters are treaded as equal objects if their definition is
      ///    the same. The value can differ.
      /// </remarks>
      /// <param name="obj">Object to be checked.</param>
      /// <returns>True, if objects are the same.</returns>
      public override bool Equals(object obj)
      {
         return obj is InputParameter && this == (InputParameter) obj;
      }

      /// <summary>
      ///    Gets a hash code for the struct.
      /// </summary>
      /// <remarks>Only name and display name are used for new hash value.</remarks>
      /// <returns>Hash value.</returns>
      public override int GetHashCode()
      {
         return Name.GetHashCode() ^ DisplayName.GetHashCode();
      }

      /// <summary>
      ///    This operator checks equality of two input parameters.
      /// </summary>
      /// <remarks>
      ///    The value is not checked. That means two input parameters are treaded as equal objects if their definition is
      ///    the same. The value can differ.
      /// </remarks>
      /// <param name="a">Left side of the operator.</param>
      /// <param name="b">Right side of the operator.</param>
      /// <returns>True, if objects are the same.</returns>
      public static bool operator ==(InputParameter a, InputParameter b)
      {
         return a.Name == b.Name &&
                a.DisplayName == b.DisplayName &&
                a.Unit == b.Unit &&
                a.MinValue == b.MinValue &&
                a.MinValueAllowed == b.MinValueAllowed &&
                a.MaxValue == b.MaxValue &&
                a.MaxValueAllowed == b.MaxValueAllowed;
      }

      /// <summary>
      ///    This operator checks difference of two input parameters.
      /// </summary>
      /// <remarks>
      ///    The value is not checked. That means two input parameters are treaded as equal objects if their definition is
      ///    the same. The value does not matter.
      /// </remarks>
      /// <param name="a">Left side of the operator.</param>
      /// <param name="b">Right side of the operator.</param>
      /// <returns>True, if objects differ.</returns>
      public static bool operator !=(InputParameter a, InputParameter b)
      {
         return !(a == b);
      }

      #endregion
   }
}