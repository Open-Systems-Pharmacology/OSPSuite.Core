using System;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Utility.Reflection;

namespace OSPSuite.Presentation.Presenters.Charts
{
   public enum BrowserColumns
   {
      RepositoryName,
      Simulation,
      TopContainer,
      Container,
      BottomCompartment,
      Molecule,
      Name,
      ColumnId,
      DimensionName,
      BaseGridName,
      HasRelatedColumns,
      Origin,
      Date,
      Category,
      Source,
      QuantityName,
      QuantityType,
      QuantityPath,
      OrderIndex,
      Used
   }

   public class DataColumnDTO : Notifier
   {
      private readonly PathElements _pathElements;
      private bool _used;
      public DataColumn DataColumn { get; }

      public string RepositoryName => DataColumn.Repository?.Name;
      public string Simulation => displayNameFor(PathElementId.Simulation);
      public string TopContainer => displayNameFor(PathElementId.TopContainer);
      public string Container => displayNameFor(PathElementId.Container);
      public string BottomCompartment => displayNameFor(PathElementId.BottomCompartment);
      public string Molecule => displayNameFor(PathElementId.Molecule);
      public string Name => displayNameFor(PathElementId.Name);
      public string BaseGridName => DataColumn.BaseGrid.Name;
      public int OrderIndex => DataColumn.QuantityInfo.OrderIndex;
      public string QuantityName => DataColumn.Name;
      public string DimensionName => DataColumn.Dimension.Name;
      public string QuantityType => DataColumn.QuantityInfo.Type.ToString();
      public bool HasRelatedColumns => DataColumn.RelatedColumns.Any();
      public string Origin => DataColumn.DataInfo.Origin.ToString();
      public string Category => categoryFromOrigin(DataColumn.DataInfo.Origin);

      private string categoryFromOrigin(ColumnOrigins dataInfoOrigin)
      {
         switch (dataInfoOrigin)
         {
            case ColumnOrigins.BaseGrid:
               return Captions.Chart.GroupRowFormat.Time;
            case ColumnOrigins.Calculation:
            case ColumnOrigins.CalculationAuxiliary:
               return Captions.Chart.GroupRowFormat.Simulation;
            case ColumnOrigins.Observation:
            case ColumnOrigins.ObservationAuxiliary:
               return Captions.Chart.GroupRowFormat.Observation;
            case ColumnOrigins.DeviationLine:
               return Captions.Chart.GroupRowFormat.DeviationLine;
            case ColumnOrigins.Undefined:
               return Captions.Chart.GroupRowFormat.Undefined;
            default:
               throw new ArgumentOutOfRangeException(nameof(dataInfoOrigin), dataInfoOrigin, null);
         }
      }

      public bool Used
      {
         get => _used;
         set => SetProperty(ref _used, value);
      }

      public DataColumnDTO(DataColumn dataColumn, Func<DataColumn, PathElements> displayQuantityPathFunc)
      {
         DataColumn = dataColumn;
         Used = false;
         _pathElements = displayQuantityPathFunc(dataColumn);
         DataColumn.BottomCompartment = BottomCompartment;
      }

      private string displayNameFor(PathElementId pathElementId) => _pathElements[pathElementId].DisplayName;
   }
}