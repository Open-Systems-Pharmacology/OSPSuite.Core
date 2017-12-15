namespace OSPSuite.Core.Domain.Builder
{
   public interface IWithValueOrigin
   {
      /// <summary>
      ///    Origin of underlying value
      /// </summary>
      ValueOrigin ValueOrigin { get; }
   }
}