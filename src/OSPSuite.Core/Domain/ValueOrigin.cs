namespace OSPSuite.Core.Domain
{
   public class ValueOrigin
   {
      /// <summary>
      ///    Origin of the value
      /// </summary>
      public ValueOriginType Type { get; set; }

      /// <summary>
      ///    Optional description explaining the quantity value
      /// </summary>
      public string Description { get; set; }

      public ValueOrigin Clone()
      {
         var clone = new ValueOrigin();
         clone.UpdateFrom(this);
         return clone;
      }

      public void UpdateFrom(ValueOrigin valueOrigin)
      {
         if (valueOrigin == null)
            return;

         Type = valueOrigin.Type;
         Description = valueOrigin.Description;
      }
   }
}