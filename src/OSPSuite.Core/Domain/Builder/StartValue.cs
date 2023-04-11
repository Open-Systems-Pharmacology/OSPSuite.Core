namespace OSPSuite.Core.Domain.Builder
{
   public interface IStartValue : IUsingFormula, IWithDisplayUnit, IWithPath, IWithValueOrigin, IWithNullableValue, IBuilder
   {
      /// <summary>
      ///    Obsolete for serialization only
      /// </summary>
      double? StartValue { get; set; }
   }

   public abstract class StartValueBase : PathAndValueEntity, IStartValue
   {
      public double? StartValue
      {
         get => Value;
         set => Value = value;
      }
   }
}