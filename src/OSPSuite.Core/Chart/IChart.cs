using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Chart
{
   public interface IChart : IChartManagement, IWithName, IWithDescription
   {
      string Title { set; get; }
      string OriginText { get; set; }
   }
}