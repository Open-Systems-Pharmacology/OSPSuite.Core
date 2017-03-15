using OSPSuite.Core.Serialization.SimModel.DTO;

namespace OSPSuite.Core.Serialization.SimModel.Serializer
{
   public abstract class FormulaExportXmlSerializerBase<TFormula> : SimModelSerializerBase<TFormula> where TFormula : FormulaExport
   {
      public override void PerformMapping()
      {
         Map(x => x.Id);
      }
   }
}