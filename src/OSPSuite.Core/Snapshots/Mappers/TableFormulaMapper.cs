using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Utility.Extensions;
using System.Linq;
using System.Threading.Tasks;
using ModelTableFormula = OSPSuite.Core.Domain.Formulas.TableFormula;
using SnapshotTableFormula = OSPSuite.Core.Snapshots.TableFormula;
namespace OSPSuite.Core.Snapshots.Mappers
{
   public abstract class TableFormulaMapper : ObjectBaseSnapshotMapperBase<ModelTableFormula, SnapshotTableFormula, SnapshotContext>
   {
      public override Task<ModelTableFormula> MapToModel(SnapshotTableFormula snapshotTableFormula, SnapshotContext snapshotContext)
      {
         var tableFormula = CreateNewTableFormula();
         MapSnapshotPropertiesToModel(snapshotTableFormula, tableFormula);
         UpdateModelProperties(tableFormula, snapshotTableFormula);
         return Task.FromResult(tableFormula);
      }

      protected abstract ModelTableFormula CreateNewTableFormula();
      protected abstract IDimension DimensionByName(string dimensionName);


      public virtual void UpdateModelProperties(ModelTableFormula tableFormula, SnapshotTableFormula snapshotTableFormula)
      {
         tableFormula.XDimension = DimensionByName(snapshotTableFormula.XDimension);
         tableFormula.XDisplayUnit = tableFormula.XDimension.Unit(ModelValueFor(snapshotTableFormula.XUnit));
         tableFormula.XName = snapshotTableFormula.XName;

         tableFormula.Dimension = DimensionByName(snapshotTableFormula.YDimension);
         tableFormula.YDisplayUnit = tableFormula.Dimension.Unit(ModelValueFor(snapshotTableFormula.YUnit));
         tableFormula.YName = snapshotTableFormula.YName;

         tableFormula.UseDerivedValues = snapshotTableFormula.UseDerivedValues;

         snapshotTableFormula.Points.Each(p => tableFormula.AddPoint(valuePointFrom(tableFormula, p)));
      }

      private ValuePoint valuePointFrom(ModelTableFormula tableFormula, Point point)
      {
         var x = tableFormula.XBaseValueFor(point.X);
         var y = tableFormula.YBaseValueFor(point.Y);
         return new ValuePoint(x, y) { RestartSolver = point.RestartSolver };
      }

      private static Point snapshotPointFor(ModelTableFormula tableFormula, ValuePoint p) => new Point
      {
         X = tableFormula.XDisplayValueFor(p.X),
         Y = tableFormula.YDisplayValueFor(p.Y),
         RestartSolver = p.RestartSolver
      };

      public override Task<SnapshotTableFormula> MapToSnapshot(ModelTableFormula tableFormula) => 
         SnapshotFrom(tableFormula, snapshot => { UpdateSnapshotProperties(snapshot, tableFormula); });

      public virtual void UpdateSnapshotProperties(SnapshotTableFormula snapshot, ModelTableFormula tableFormula)
      {
         snapshot.XDimension = tableFormula.XDimension.Name;
         snapshot.XUnit = SnapshotValueFor(tableFormula.XDisplayUnit.Name);
         snapshot.XName = tableFormula.XName;

         snapshot.YDimension = tableFormula.Dimension.Name;
         snapshot.YUnit = SnapshotValueFor(tableFormula.YDisplayUnit.Name);
         snapshot.YName = tableFormula.YName;

         snapshot.UseDerivedValues = tableFormula.UseDerivedValues;

         snapshot.Points = tableFormula.AllPoints.Select(p => snapshotPointFor(tableFormula, p)).ToList();
      }
   }
}
