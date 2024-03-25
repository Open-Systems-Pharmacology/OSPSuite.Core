using OSPSuite.Core;

namespace OSPSuite.Starter.Services
{
   public class CoreUserSettings : ICoreUserSettings
   {
      public int MaximumNumberOfCoresToUse { get; set; } = 2;
      public int NumberOfBins { get; set; } = 20;
      public int NumberOfIndividualsPerBin { get; set; } = 100;
   }
}