namespace OSPSuite.Core.Domain
{
   public interface IWithValueOrigin
   {
      /// <summary>
      ///    Origin of underlying value
      /// </summary>
      ValueOrigin ValueOrigin { get; }

      /// <summary>
      ///    Updates the value origin in the underlying object. The object is responsible to decide HOW the update should be
      ///    performed
      /// </summary>
      /// <param name="sourceValueOrigin">
      ///    Value origin containing the properties used to update own <see cref="ValueOrigin" />
      /// </param>
      void UpdateValueOriginFrom(ValueOrigin sourceValueOrigin);
   }
}