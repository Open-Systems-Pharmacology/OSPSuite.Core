using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Chart
{
   public class CurveDataTemplate
   {
      public string Path { get; set; }
      public string RepositoryName { get; set; }
      public QuantityType QuantityType { get; set; }

      public CurveDataTemplate Clone()
      {
         var clone = new CurveDataTemplate();
         clone.UpdateFrom(this);
         return clone;
      }

      public void UpdateFrom(CurveDataTemplate curveDataTemplate)
      {
         Path = curveDataTemplate.Path;
         RepositoryName = curveDataTemplate.RepositoryName;
         QuantityType = curveDataTemplate.QuantityType;
      }
   }
}