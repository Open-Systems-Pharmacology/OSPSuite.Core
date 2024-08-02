using System;
using System.Linq;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Views.Parameters;

namespace OSPSuite.Presentation.Presenters.Parameters
{
   public interface ITableParameterPresenter : ITableFormulaPresenter
   {
      void Edit(IParameter tableParameter);
      void Save();
   }

   public abstract class TableParameterPresenter<TView> : TableFormulaPresenter<TView>, ITableParameterPresenter
      where TView : ITableFormulaView
   {
      private IParameter _tableParameter;

      protected TableParameterPresenter(TView view, Func<TableFormula> importTableFormula) : base(view, importTableFormula)
      {
      }

      public override void SetXValue(ValuePointDTO valuePointDTO, double newValue) => valuePointDTO.X = newValue;

      public override void SetYValue(ValuePointDTO valuePointDTO, double newValue) => valuePointDTO.Y = newValue;

      protected override IParameter Owner => _tableParameter;

      public void Edit(IParameter tableParameter)
      {
         _tableParameter = tableParameter;
         _view.Editable = _tableParameter.Editable;
         //do not edit the parameter formula itself as the user might cancel the edit
         var tableFormula = tableParameter.Formula as TableFormula;

         if (tableFormula != null)
            tableFormula = CreateClone(tableFormula);

         tableFormula = tableFormula ?? CreateTableFormula();
         Edit(tableFormula ?? CreateTableFormula());
      }

      protected abstract TableFormula CreateClone(TableFormula tableFormula);
      protected abstract TableFormula NewTableFormula();

      protected virtual TableFormula CreateTableFormula()
      {
         var formula = NewTableFormula().WithName(OwnerName);
         //use whatever default were created in the factory
         formula.InitializedWith(formula.XName, OwnerName, formula.XDimension, _tableParameter.Dimension);

         ConfigureCreatedTableAction(formula);

         return formula;
      }

      public void Save()
      {
         //do not use AddCommand here as we only want to save the table without notifying any change events
         //since all changed were performed already
         CommandCollector.AddCommand(SetParameterFormula(_tableParameter, EditedFormula));
      }

      protected abstract ICommand SetParameterFormula(IParameter tableParameter, TableFormula tableFormula);

      public override bool CanClose
      {
         get
         {
            if (_editedFormula == null)
               return base.CanClose;

            return base.CanClose && _editedFormula.AllPoints.Any();
         }
      }

      public override void AddPoint()
      {
         var newPoint = new ParameterValuePointDTO(Owner, _editedFormula, new ValuePoint(double.NaN, double.NaN));
         try
         {
            _tableFormulaDTO.AllPoints.Add(newPoint);
         }
         catch (ValuePointAlreadyExistsForPointException)
         {
            _tableFormulaDTO.AllPoints.Remove(newPoint);
            throw;
         }

         _view.EditPoint(newPoint);
      }

      public override void RemovePoint(ValuePointDTO pointToRemove) => _tableFormulaDTO.AllPoints.Remove(pointToRemove);
   }
}