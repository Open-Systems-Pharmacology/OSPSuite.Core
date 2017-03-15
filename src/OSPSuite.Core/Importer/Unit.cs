namespace OSPSuite.Core.Importer
{
   public struct Unit
   {
      public string Name { get; set; }

      private string _displayName;

      public string DisplayName
      {
         get { return string.IsNullOrEmpty(_displayName) ? Name : _displayName; }
         set { _displayName = value; }
      }

      public bool IsDefault { get; set; }

      public bool IsEqual(string name)
      {
         //units might have white spaces and must be trimmed before comparing
         name = name.Trim();
         name = name.Replace(" ", "");
         if (name.Length > 1)
            return Name.Replace(" ", "").ToUpper() == name.ToUpper();

         return Name.Replace(" ", "") == name;
      }

      /// <summary>
      ///    This method determines whether two objects are the same.
      /// </summary>
      /// <remarks>
      ///    The bools are not checked. That means two units are treaded as equal objects if their definition is the same
      ///    (Name and display name).
      /// </remarks>
      /// <param name="obj">Object to be checked.</param>
      /// <returns>True, if objects are the same.</returns>
      public override bool Equals(object obj)
      {
         return obj is Unit && this == (Unit) obj;
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
      ///    This operator checks equality of two units.
      /// </summary>
      /// <remarks>
      ///    The bools are not checked. That means two units are treaded as equal objects if their definition is the same
      ///    (Name and display name).
      /// </remarks>
      /// <param name="a">Left side of the operator.</param>
      /// <param name="b">Right side of the operator.</param>
      /// <returns>True, if objects are the same.</returns>
      public static bool operator ==(Unit a, Unit b)
      {
         return a.Name == b.Name && a.DisplayName == b.DisplayName;
      }

      /// <summary>
      ///    This operator checks difference of two units.
      /// </summary>
      /// <remarks>
      ///    The bools are not checked. That means two units are treaded as equal objects if their definition is the same
      ///    (Name and display name).
      /// </remarks>
      /// <param name="a">Left side of the operator.</param>
      /// <param name="b">Right side of the operator.</param>
      /// <returns>True, if objects differ.</returns>
      public static bool operator !=(Unit a, Unit b)
      {
         return !(a == b);
      }
   }
}