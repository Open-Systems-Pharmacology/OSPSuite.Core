using System.Threading.Tasks;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Snapshots.Mappers
{
   public class AxisMapper : SnapshotMapperBase<Core.Chart.Axis, Axis, SnapshotContext>
   {
      private readonly IDimensionFactory _dimensionFactory;

      public AxisMapper(IDimensionFactory dimensionFactory)
      {
         _dimensionFactory = dimensionFactory;
      }

      public override Task<Axis> MapToSnapshot(Core.Chart.Axis axis)
      {
         return SnapshotFrom(axis, x =>
         {
            x.Dimension = axis.Dimension?.Name;
            x.Unit = ModelValueFor(axis.UnitName);
            x.Caption = SnapshotValueFor(axis.Caption);
            x.Type = axis.AxisType;
            x.GridLines = axis.GridLines;
            x.Visible = axis.Visible;
            x.Scaling = axis.Scaling;
            x.NumberMode = axis.NumberMode;
            x.DefaultColor = axis.DefaultColor;
            x.DefaultLineStyle = axis.DefaultLineStyle;
            x.Min = axis.Min;
            x.Max = axis.Max;
         });
      }

      public override Task<Core.Chart.Axis> MapToModel(Axis snapshot, SnapshotContext snapshotContext)
      {
         var axis = new Core.Chart.Axis(snapshot.Type)
         {
            Dimension = dimensionByName(snapshot),
            Caption = snapshot.Caption,
            GridLines = snapshot.GridLines,
            Visible = snapshot.Visible,
            DefaultColor = snapshot.DefaultColor,
            DefaultLineStyle = snapshot.DefaultLineStyle,
            Scaling = snapshot.Scaling,
            NumberMode = snapshot.NumberMode
         };

         axis.Dimension = _dimensionFactory.OptimalDimension(axis.Dimension);
         axis.UnitName = ModelValueFor(snapshot.Unit);
         axis.Min = snapshot.Min;
         axis.Max = snapshot.Max;

         return Task.FromResult(axis);
      }

      private IDimension dimensionByName(Axis snapshot)
      {
         if (!_dimensionFactory.Has(snapshot.Dimension))
            return _dimensionFactory.NoDimension;

         return _dimensionFactory.Dimension(snapshot.Dimension);
      }
   }
}