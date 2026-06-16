using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Chart;

public abstract class AnalysisChart : ChartWithObservedData, ISimulationAnalysis
{
   public IAnalysable Analysable { get; set; }

   /// <summary>
   ///    Returns the color another <see cref="AnalysisChart" /> on the same <see cref="Analysable" />
   ///    already assigned to the given output <paramref name="path" />, preferring a peer marked
   ///    <see cref="CurveChartTypes.TimeProfile" /> as the canonical color source.
   ///    Returns <c>null</c> when no peer carries a curve for that path.
   /// </summary>
   public virtual Color? PeerColorForPath(string path)
   {
      if (Analysable == null)
         return null;

      var peers = Analysable.Analyses
         .OfType<AnalysisChart>()
         .Where(c => !ReferenceEquals(c, this))
         .ToList();

      //prefer a Time Profile peer so the canonical color source wins
      var fromTimeProfile = peers.Where(c => c.CurveChartType == CurveChartTypes.TimeProfile)
         .SelectMany(c => c.Curves)
         .FirstOrDefault(c => string.Equals(c.yData?.PathAsString, path));
      if (fromTimeProfile != null)
         return fromTimeProfile.Color;

      //fall back to any peer chart that already chose a color for this path
      var fromAnyPeer = peers.SelectMany(c => c.Curves)
         .FirstOrDefault(c => string.Equals(c.yData?.PathAsString, path));
      return fromAnyPeer?.Color;
   }
}

public abstract class AnalysisChartWithLocalRepositories : AnalysisChart
{
   private readonly List<DataRepository> _dataRepositories;
   public virtual IReadOnlyList<DataRepository> DataRepositories => _dataRepositories;

   protected AnalysisChartWithLocalRepositories()
   {
      _dataRepositories = new List<DataRepository>();
   }

   public virtual void AddRepository(DataRepository dataRepository)
   {
      _dataRepositories.Add(dataRepository);
   }

   public virtual void ClearDataRepositories()
   {
      _dataRepositories.Clear();
   }

   public virtual void AddRepositories(IEnumerable<DataRepository> dataRepositories)
   {
      dataRepositories.Each(AddRepository);
   }
}