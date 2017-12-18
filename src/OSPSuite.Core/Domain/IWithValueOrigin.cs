namespace OSPSuite.Core.Domain
{
   public interface IWithValueOrigin
   {
      /// <summary>
      ///    Origin of underlying value
      /// </summary>
      ValueOrigin ValueOrigin { get; }
   }
}