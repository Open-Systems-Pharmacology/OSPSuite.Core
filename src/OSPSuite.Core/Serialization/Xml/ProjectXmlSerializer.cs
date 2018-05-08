using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Serialization.Xml
{
   public abstract class ProjectXmlSerializer<TProject> : OSPSuiteXmlSerializer<TProject> where TProject : IProject
   {
      public override void PerformMapping()
      {
         Map(x => x.Name);
         Map(x => x.Description);
         Map(x => x.Creation);
         Map(x => x.JournalPath);
         Map(x => x.DisplayUnits);
         Map(x => x.Favorites);
         MapEnumerable(x => x.AllClassifications, x => x.AddClassification);
         MapEnumerable(x => x.AllClassifiables, x => x.AddClassifiable);
         MapEnumerable(x => x.ChartTemplates, x => x.AddChartTemplate);
      }
   }
}