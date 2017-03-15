using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Chart
{
   public class CurveTemplate : IWithName, IUpdatable
   {
      public CurveDataTemplate xData { get; }
      public CurveDataTemplate yData { get; }

      public string Name { get; set; }
      public CurveOptions CurveOptions { get; private set; }

      public CurveTemplate()
      {
         CurveOptions = new CurveOptions();
         xData = new CurveDataTemplate();
         yData = new CurveDataTemplate();
      }

      public bool IsBaseGrid => xData.QuantityType.Is(QuantityType.BaseGrid);

      public void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         var sourceCurveTemplate = source as CurveTemplate;
         if (sourceCurveTemplate == null) return;
         xData.UpdateFrom(sourceCurveTemplate.xData);
         yData.UpdateFrom(sourceCurveTemplate.yData);
         Name = sourceCurveTemplate.Name;
         CurveOptions = sourceCurveTemplate.CurveOptions.Clone();
      }
   }
}