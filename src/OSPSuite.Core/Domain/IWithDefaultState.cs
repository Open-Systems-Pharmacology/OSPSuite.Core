namespace OSPSuite.Core.Domain
{
   public interface IWithDefaultState
   {
      /// <summary>
      ///    Indicates if a entity value is a default value (e.g. coming from the PK-Sim database) or if the value was entered
      ///    or modified by the user. Default value is <c>false</c>
      /// </summary>
      bool IsDefault { get; set; }
   }
}