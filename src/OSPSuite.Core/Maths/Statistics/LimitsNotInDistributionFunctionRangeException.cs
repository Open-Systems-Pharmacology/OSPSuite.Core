using OSPSuite.Utility.Exceptions;

namespace OSPSuite.Core.Maths.Statistics
{
   public class DistributionException : OSPSuiteException
   {
      private const string _errorMessage = "Unable to create a random deviate with the given constraints:\n{0}";

      public DistributionException(string message)
         : base(string.Format(_errorMessage, message))
      {
      }
   }

   public class LimitsNotInNormalDistributionFunctionRangeException : DistributionException
   {
      public LimitsNotInNormalDistributionFunctionRangeException(double min, double max, double mean, double sigma)
         : base($"Minimum:{min}\nMaximum:{max}\nMean:{mean}\nStandard Deviation:{sigma}")
      {
      }
   }

   public class LimitsNotInLogNormalDistributionFunctionRangeException : DistributionException
   {
      public LimitsNotInLogNormalDistributionFunctionRangeException(double min, double max, double mean, double sigma)
         : base($"Minimum:{min}\nMaximum:{max}\nMean:{mean}\nGeometric Standard Deviation:{sigma}")
      {
      }
   }

   public class LimitsNotInUniformDistributionFunctionRangeException : DistributionException
   {
      public LimitsNotInUniformDistributionFunctionRangeException(double min, double max, double distMin, double distMax)
         : base($"Minimum:{min}\nMaximum:{max}\nDistribution Minimum:{distMin}\nDistribution Maximum:{distMax}")
      {
      }
   }
}