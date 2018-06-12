using System.Data;using OSPSuite.Core.Maths.Random;
using OSPSuite.Core.Maths.Statistics;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Services
{
   public interface INormalDistributionDataCreator
   {
      DataTable CreateNormalData(double mean, double std, double? min=null, double? max = null, int numberOfData = 300);
   }

   public class NormalDistributionDataCreator : INormalDistributionDataCreator
   {
      public DataTable CreateNormalData(double mean, double std, double? min = null, double? max = null, int numberOfData = 300)
      {
         var gaussTable = new DataTable();
         var normalDistribution = new NormalDistribution(mean, std);
         var randomGenerator = new RandomGenerator();

         gaussTable.AddColumn<double>(Constants.X);
         gaussTable.AddColumn<double>(Constants.Y);

         var minToUse = min ?? mean - 4 * std;
         var maxToUse = max ?? mean + 4 * std;
         for (int i = 0; i < numberOfData; i++)
         {
            var x = randomGenerator.UniformDeviate(minToUse, maxToUse);
            var y = normalDistribution.ProbabilityDensityFor(x);
            var row = gaussTable.NewRow();
            row[Constants.X] = x;
            row[Constants.Y] = y;
            gaussTable.Rows.Add(row);
         }

         return gaussTable;
      }
   }
}