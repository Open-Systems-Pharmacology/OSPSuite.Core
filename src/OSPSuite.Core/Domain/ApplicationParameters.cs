namespace OSPSuite.Core.Domain
{
   public class ApplicationParameters
   {
      public IParameter StartTime { get; }
      public IParameter InfusionTime { get; }
      public IParameter DrugMass { get; }

      public ApplicationParameters(IParameter startTime)
      {
         StartTime = startTime;
         InfusionTime = startTime.ParentContainer?.Parameter(Constants.Parameters.INFUSION_TIME);
         DrugMass = startTime.ParentContainer?.Parameter(Constants.Parameters.DRUG_MASS);
      }

      public ApplicationParameters(IParameter startTime, IParameter drugMass, IParameter infusionTime)
      {
         StartTime = startTime;
         InfusionTime = infusionTime;
         DrugMass = drugMass;
      }
   }
}