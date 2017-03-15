namespace OSPSuite.Core.Domain
{
   /// <summary>
   ///    One possible analysis performed using simulation data.
   ///    This can be for instance a curve chart (individual curves), or a population analysis plot such as BoxWhisher
   /// </summary>
   public interface ISimulationAnalysis : IWithName, IWithId, IWithDescription
   {
      /// <summary>
      ///    Reference to the <see cref="IAnalysable"/> containing this analysis
      /// </summary>
      IAnalysable Analysable { get; set; }
   }
}