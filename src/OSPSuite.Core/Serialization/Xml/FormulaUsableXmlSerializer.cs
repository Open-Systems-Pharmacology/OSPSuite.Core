using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Serialization.Xml
{
   public class FormulaUsableXmlSerializer<T> : EntityXmlSerializer<T> where T : IFormulaUsable
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         Map(x => x.Value);
         Map(x => x.Dimension);
      }
   }
}