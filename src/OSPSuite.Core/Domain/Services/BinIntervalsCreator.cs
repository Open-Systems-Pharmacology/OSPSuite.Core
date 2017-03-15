using System;
using System.Collections.Generic;
using System.Linq;

namespace OSPSuite.Core.Domain.Services
{
   public interface IBinIntervalsCreator
   {
      /// <summary>
      ///    Returns an enumeration of <see cref="BinInterval" /> each containing the same number of element
      ///    <paramref name="numberOfElementsPerBin" /> except maybe for the last one
      /// </summary>
      /// <param name="allValues">All values from which the interval will be dynamically created</param>
      /// <param name="numberOfElementsPerBin">Number of elements to create per bin</param>
      /// <param name="numberOfBins">Number of bins</param>
      IReadOnlyList<BinInterval> CreateIntervalsFor(IReadOnlyCollection<double> allValues, int numberOfElementsPerBin, int numberOfBins);

      /// <summary>
      ///    Returns an enumeration of <see cref="BinInterval" /> using the user defined number of individuals per bin and number
      ///    of bins
      /// </summary>
      /// <param name="allValues">All values from which the interval will be dynamically created</param>
      IReadOnlyList<BinInterval> CreateIntervalsFor(IReadOnlyCollection<double> allValues);

      /// <summary>
      ///    Returns an enumeration of <see cref="BinInterval" /> uniformely distributed using the number of bins defined in the
      ///    user settings of bins
      /// </summary>
      /// <param name="allValues">All values used to create the uniform interval distribution</param>
      IReadOnlyList<BinInterval> CreateUniformIntervalsFor(IReadOnlyCollection<double> allValues);

      /// <summary>
      ///    Returns an enumeration of <see cref="BinInterval" /> uniformly distributed using the number of bins defined in the
      ///    user settings of bins
      /// </summary>
      /// <param name="allValues">All values used to create the uniform interval distribution</param>
      /// <param name="numberOfBins">Number of bins to create</param>
      IReadOnlyList<BinInterval> CreateUniformIntervalsFor(IReadOnlyCollection<double> allValues, int numberOfBins);
   }

   public class BinIntervalsCreator : IBinIntervalsCreator
   {
      private readonly ICoreUserSettings _userSettings;

      public BinIntervalsCreator(ICoreUserSettings userSettings)
      {
         _userSettings = userSettings;
      }

      public IReadOnlyList<BinInterval> CreateIntervalsFor(IReadOnlyCollection<double> allValues, int numberOfElementsPerBin, int numberOfBins)
      {
         return createIntervals(allValues, numberOfBins, () => createIntervalsFor(allValues, numberOfElementsPerBin, numberOfBins));
      }

      public IReadOnlyList<BinInterval> CreateUniformIntervalsFor(IReadOnlyCollection<double> allValues, int numberOfBins)
      {
         return createIntervals(allValues, numberOfBins, () => createUniform(allValues, numberOfBins));
      }

      public IReadOnlyList<BinInterval> CreateIntervalsFor(IReadOnlyCollection<double> allValues)
      {
         return CreateIntervalsFor(allValues, _userSettings.NumberOfIndividualsPerBin, _userSettings.NumberOfBins);
      }

      public IReadOnlyList<BinInterval> CreateUniformIntervalsFor(IReadOnlyCollection<double> allValues)
      {
         return CreateUniformIntervalsFor(allValues, _userSettings.NumberOfBins);
      }

      private IReadOnlyList<BinInterval> createIntervals(IReadOnlyCollection<double> allValues, int numberOfBins, Func<IEnumerable<BinInterval>> createBinIntervals)
      {
         var constantIntervals = constantIntervalsFor(allValues, numberOfBins);
         if (constantIntervals.Any())
            return constantIntervals;

         return createBinIntervals().ToList();
      }

      private IEnumerable<BinInterval> createIntervalsFor(IEnumerable<double> allValues, int numberOfElementsPerBin, int numberOfBins)
      {
         var sortedValues = allValues.OrderBy(x => x).ToList();

         //we have enough elements to fill each bin with at least numberOfElementsPerBin per bin
         if (sortedValues.Count >= numberOfElementsPerBin * numberOfBins)
            return createExactly(numberOfBins, sortedValues);

         //at least two bins
         if (sortedValues.Count >= 2 * numberOfElementsPerBin)
            return createExactly(ratio(sortedValues.Count, numberOfElementsPerBin), sortedValues);

         //we cannot create two bins with the specified numberOfElements per bin. Return undefined bin intervals
         return undefinedBinIntervals;
      }

      private IEnumerable<BinInterval> createExactly(int numberOfBins, List<double> sortedValues)
      {
         var numberOfElementsPerBin = ratio(sortedValues.Count, numberOfBins);
         var min = sortedValues.Min();
         var max = sortedValues.Max();

         double currentMin = min;
         for (int i = 0; i < numberOfBins - 1; i++)
         {
            var currentMax = sortedValues[(i + 1) * numberOfElementsPerBin - 1];
            yield return new BinInterval(currentMin, currentMax, minAllowed: (i == 0), maxAllowed: true);
            currentMin = currentMax;
         }

         yield return new BinInterval(currentMin, max, minAllowed: false, maxAllowed: true);
      }

      private IEnumerable<BinInterval> createUniform(IReadOnlyCollection<double> allValues, int numberOfBins)
      {
         var min = allValues.Min();
         var max = allValues.Max();

         var width = (max - min) / numberOfBins;

         double currentMin = min;

         for (int i = 0; i < numberOfBins - 1; i++)
         {
            yield return new BinInterval(currentMin, currentMin + width);
            currentMin += width;
         }
         yield return new BinInterval(currentMin, max, maxAllowed: true);
      }

      private IReadOnlyList<BinInterval> constantIntervalsFor(IReadOnlyCollection<double> allValues, int numberOfBins)
      {
         if (allValues.Count == 0)
            return undefinedBinIntervals;

         var min = allValues.Min();
         var max = allValues.Max();

         var binInterval = new BinInterval(min, max);
         if (binInterval.IsConstant || numberOfBins <= 1)
            return new[] {binInterval};

         return new List<BinInterval>();
      }

      private int ratio(int numerator, int denominator)
      {
         return (int) Math.Floor((float) numerator / denominator);
      }

      private static IReadOnlyList<BinInterval> undefinedBinIntervals => new[] {new BinInterval(double.NaN, double.NaN)};
   }
}