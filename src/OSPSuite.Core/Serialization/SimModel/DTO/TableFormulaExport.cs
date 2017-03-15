using System.Collections.Generic;
using OSPSuite.Core.Domain.Formulas;

namespace OSPSuite.Core.Serialization.SimModel.DTO
{
   public class TableFormulaExport:FormulaExport
   {
      public TableFormulaExport()
      {
         _pointList = new List<ValuePoint>();
      }
      private IList<ValuePoint> _pointList;

      public void AddPoint(ValuePoint newPoint)
      {
         _pointList.Add(newPoint);
      }

      public IEnumerable<ValuePoint> PointList
      {
         get { return _pointList; }
      }

      public bool UseDerivedValues { get; set; }
   }
}