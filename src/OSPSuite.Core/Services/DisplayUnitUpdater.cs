using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Visitor;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Formulas;

namespace OSPSuite.Core.Services
{
   public interface IDisplayUnitUpdater
   {
      void UpdateDisplayUnitsIn(IObjectBase objectToUpdate);
      void UpdateDisplayUnitsIn(DataRepository dataRepository);
      void UpdateDisplayUnitsIn(ICurveChart curveChart);
   }

   public class DisplayUnitUpdater : IDisplayUnitUpdater,
      IVisitor<IWithDisplayUnit>,
      IVisitor<DataRepository>,
      IVisitor<ICurveChart>,
      IVisitor<IParameter>,
      IVisitor<TableFormula>
   {
      private readonly IDisplayUnitRetriever _displayUnitRetriever;

      public DisplayUnitUpdater(IDisplayUnitRetriever displayUnitRetriever)
      {
         _displayUnitRetriever = displayUnitRetriever;
      }

      public virtual void UpdateDisplayUnitsIn(IObjectBase objectToUpdate)
      {
         objectToUpdate?.AcceptVisitor(this);
      }

      public virtual void UpdateDisplayUnitsIn(DataRepository dataRepository)
      {
         if (dataRepository.IsNull()) return;
         dataRepository.Each(updateDisplayUnit);
      }

      public void UpdateDisplayUnitsIn(ICurveChart curveChart)
      {
         curveChart?.Axes.Each(updateAxisUnit);
      }

      private void updateDisplayUnit(IWithDisplayUnit withDisplayUnit)
      {
         withDisplayUnit.DisplayUnit = _displayUnitRetriever.PreferredUnitFor(withDisplayUnit);
      }

      private void updateAxisUnit(IAxis axis)
      {
         var preferredUnit = _displayUnitRetriever.PreferredUnitFor(axis, axis.Unit);
         if (preferredUnit == null) return;
         axis.UnitName = preferredUnit.Name;
      }

      public void Visit(IWithDisplayUnit withDisplayUnit)
      {
         updateDisplayUnit(withDisplayUnit);
      }

      public void Visit(ICurveChart curveChart)
      {
         UpdateDisplayUnitsIn(curveChart);
      }

      public void Visit(DataRepository dataRepository)
      {
         UpdateDisplayUnitsIn(dataRepository);
      }

      public void Visit(IParameter parameter)
      {
         updateDisplayUnit(parameter);
         this.Visit(parameter.Formula);
         this.Visit(parameter.RHSFormula);
      }

      public void Visit(TableFormula tableFormula)
      {
         tableFormula.XDisplayUnit = _displayUnitRetriever.PreferredUnitFor(tableFormula.XDimension);
         tableFormula.YDisplayUnit = _displayUnitRetriever.PreferredUnitFor(tableFormula.Dimension);
      }
   }
}