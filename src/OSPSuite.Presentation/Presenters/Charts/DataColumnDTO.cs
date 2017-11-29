using System;
using System.Linq;
using OSPSuite.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Presentation.DTO;
using OSPSuite.Utility.Extensions;
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
      public string Simulation => displayNameFor(PathElement.Simulation);
      public string TopContainer => displayNameFor(PathElement.TopContainer);
      public string Container => displayNameFor(PathElement.Container);
      public string BottomCompartment => displayNameFor(PathElement.BottomCompartment);
      public string Molecule => displayNameFor(PathElement.Molecule);
      public string Name => displayNameFor(PathElement.Name);
      public string BaseGridName => DataColumn.BaseGrid.Name;
      public int OrderIndex => DataColumn.QuantityInfo.OrderIndex;
      public string QuantityName => DataColumn.QuantityInfo.Name;
      public string DimensionName => DataColumn.Dimension.Name;
      public string QuantityType => DataColumn.QuantityInfo.Type.ToString();
      public bool HasRelatedColumns => DataColumn.RelatedColumns.Any();
      public string Origin => DataColumn.DataInfo.Origin.ToString();
      public string Date => DataColumn.DataInfo.Date.ToIsoFormat();
      public string Category => DataColumn.DataInfo.Category;
      public string Source => DataColumn.DataInfo.Source;

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
      }

      private string displayNameFor(PathElement pathElement) => _pathElements[pathElement].DisplayName;
   }
}