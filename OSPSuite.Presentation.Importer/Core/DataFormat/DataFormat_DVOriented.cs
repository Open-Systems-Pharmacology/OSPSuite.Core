using OSPSuite.Core.Importer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPSuite.Presentation.Importer.Core.DataFormat
{
   public class DataFormat_DVOriented : IDataFormat
   {
      public string Name => "DV Oriented";

      public string Description => "https://github.com/Open-Systems-Pharmacology/OSPSuite.Core/issues/797";

      public IList<DataFormatParameter> Parameters 
      { 
         get; 
         private set; 
      }

      public IDictionary<IEnumerable<InstanstiatedMetaData>, Dictionary<Column, IList<ValueAndLloq>>> Parse(IUnformattedData data, IReadOnlyList<ColumnInfo> columnInfos)
      {
         throw new NotImplementedException();
      }

      public bool SetParameters(IUnformattedData rawData, IReadOnlyList<ColumnInfo> columnInfos)
      {
         throw new NotImplementedException();
      }
   }
}
