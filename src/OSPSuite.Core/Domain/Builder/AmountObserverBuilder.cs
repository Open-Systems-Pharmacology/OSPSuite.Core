namespace OSPSuite.Core.Domain.Builder
{
   /// <summary>
   /// Configuration for molecule-specific observer which references only one molecule amount at specific location.<para></para>
   /// Corresponding observer will be stored under molecule amount
   /// in the spatial structure of the model.<para></para>
   /// Typical example: "Concentration"-Observer (M/V)
   /// </summary>
   public interface IAmountObserverBuilder : IObserverBuilder
   {
   }

   /// <summary>
   /// Configuration for molecule-specific observer which references only one molecule amount at specific location.<para></para>
   /// Corresponding observer will be stored under molecule amount
   /// in the spatial structure of the model.<para></para>
   /// Typical example: "Concentration"-Observer (M/V)
   /// </summary>
   public class AmountObserverBuilder : ObserverBuilder, IAmountObserverBuilder
   {
      
   }
}