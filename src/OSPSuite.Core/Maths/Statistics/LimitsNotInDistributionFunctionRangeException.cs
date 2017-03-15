using System;

namespace OSPSuite.Core.Maths.Statistics
{
   public class DistributionException : Exception
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
         : base(string.Format("Minimum:{0}\nMaximum:{1}\nMean:{2}\nStandard Deviation:{3}", min, max, mean, sigma))
      {
      }
   }

   public class LimitsNotInLogNormalDistributionFunctionRangeException : DistributionException
   {
      public LimitsNotInLogNormalDistributionFunctionRangeException(double min, double max, double mean, double sigma)
         : base(string.Format("Minimum:{0}\nMaximum:{1}\nMean:{2}\nGeometric Standard Deviation:{3}", min, max, mean, sigma))
      {
      }
   }

   public class LimitsNotInUniformDistributionFunctionRangeException : DistributionException
   {
      public LimitsNotInUniformDistributionFunctionRangeException(double min, double max, double distMin, double distMax)
         : base(string.Format("Minimum:{0}\nMaximum:{1}\nDistribution Minimum:{2}\nDistribution Maximum:{3}", min, max, distMin, distMax))
      {
      }
   }
}