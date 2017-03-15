using OSPSuite.Core.Serialization.SimModel.DTO;

namespace OSPSuite.Core.Serialization.SimModel.Serializer
{
   public class TableFormulaExportXmlSerializer: FormulaExportXmlSerializerBase<TableFormulaExport> 
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         ElementName = SimModelSchemaConstants.TableFormula;
         MapEnumerable(x => x.PointList, x => x.AddPoint);
         Map(x => x.UseDerivedValues);
      }
   }
}