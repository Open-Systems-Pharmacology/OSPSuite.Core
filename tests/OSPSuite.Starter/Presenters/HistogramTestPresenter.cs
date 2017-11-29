using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Starter.Views;

namespace OSPSuite.Starter.Presenters
{
   public interface IHistogramTestPresenter : IPresenter<IHistogramTestView>
   {
      double Minimum { get; set; }
      double Maximum { get; set; }
      int Bins { get; set; }
      int ValueCount { get; set; }
      void Plot();
   }

   public class HistogramTestPresenter : AbstractPresenter<IHistogramTestView, IHistogramTestPresenter>, IHistogramTestPresenter
   {
      private readonly IBinIntervalsCreator _binIntervalsCreator;

      public HistogramTestPresenter(IHistogramTestView view, IBinIntervalsCreator binIntervalsCreator) : base(view)
      {
         _binIntervalsCreator = binIntervalsCreator;
      }

      public double Minimum { get; set; } = 0;
      public double Maximum { get; set; } = 1;
      public int Bins { get; set; } = 5;
      public int ValueCount { get; set; } = 100;

      public void Plot()
      {
         var distributionSettings = new DistributionSettings();
         _view.PlotPopulationData(generateContinuousDistributionData(Minimum, Maximum, Bins, ValueCount, distributionSettings), distributionSettings);
      }

      private ContinuousDistributionData generateContinuousDistributionData(double minimum, double maximum, int bins, int valueCount, DistributionSettings settings)
      {
         List<double> values = createValues(valueCount, minimum, maximum).ToList();

         var allIntervals = _binIntervalsCreator.CreateUniformIntervalsFor(values, bins);
         var data = new ContinuousDistributionData(settings.AxisCountMode, allIntervals.Count);

         foreach (var interval in allIntervals)
         {
            var currentInterval = interval;

            double count = values.Count(x => currentInterval.Contains(x));

            data.AddData(currentInterval.MeanValue, count);
         }

         data.XMinData = values.Min();
         data.XMaxData = values.Max();

         return data;
      }

      private IEnumerable<double> createValues(int valueCount, double min, double max)
      {
         var list = new List<double>();
         var random = new Random();
         for (int i = 0; i < valueCount; i++)
         {
            list.Add(random.NextDouble() * (max - min) + min);
         }

         return list;
      }
   }
}