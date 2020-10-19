namespace OSPSuite.Infrastructure.Import.Services
{
   public class SimulationResultsImportFile : ImportLogger
   {
      public virtual int NumberOfIndividuals { get; set; }
      public virtual int NumberOfQuantities { get; set; }
   }
}