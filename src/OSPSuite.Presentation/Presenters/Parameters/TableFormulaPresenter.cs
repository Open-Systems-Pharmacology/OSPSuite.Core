using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Views.Parameters;

namespace OSPSuite.Presentation.Presenters.Parameters
{
   public interface ITableFormulaPresenter : ICommandCollectorPresenter
   {
      void ImportTable();
      void RemovePoint(ValuePointDTO pointToRemove);
      void Edit(TableFormula tableFormula);
      void AddPoint();
      IEnumerable<ValuePointDTO> AllPoints { get; }
      void SetXValue(ValuePointDTO valuePointDTO, double newValue);
      void SetYValue(ValuePointDTO valuePointDTO, double newValue);

      /// <summary>
      ///    Action that can be called to configure the created <see cref="TableFormula" />
      /// </summary>
      Action<TableFormula> ConfigureCreatedTableAction { get; set; }

      string Description { get; set; }

      string ImportToolTip { get; set; }
      void SetUseDerivedValues(bool useDerivedValues);
      void SetRestartSolver(ValuePointDTO valuePointDTO, bool restart);
   }

   public abstract class TableFormulaPresenter<TView> : AbstractCommandCollectorPresenter<TView, ITableFormulaPresenter>, ITableFormulaPresenter where TView : ITableFormulaView
   {
      protected TableFormula _editedFormula;
      protected TableFormulaDTO _tableFormulaDTO;
      public Action<TableFormula> ConfigureCreatedTableAction { get; set; }

      protected TableFormulaPresenter(TView view) : base(view)
      {
         view.ShowUseDerivedValues(show:false);
         view.ShowRestartSolver(show:false);
      }

      public abstract void SetXValue(ValuePointDTO valuePointDTO, double newValue);

      public abstract void SetYValue(ValuePointDTO valuePointDTO, double newValue);

      public abstract void RemovePoint(ValuePointDTO pointToRemove);

      public abstract void AddPoint();

      public void Edit(TableFormula tableFormula)
      {
         _editedFormula = tableFormula;
         _tableFormulaDTO = new TableFormulaDTO(_editedFormula);

         if (_tableFormulaDTO.AllPoints != null)
            _tableFormulaDTO.AllPoints.CollectionChanged -= notifyChange;

         var yName = string.IsNullOrEmpty(_editedFormula.YName) ? OwnerName : _editedFormula.YName;
         _view.XCaption = Constants.NameWithUnitFor(_editedFormula.XName, _editedFormula.XDisplayUnit);
         _view.YCaption = Constants.NameWithUnitFor(yName, _editedFormula.YDisplayUnit);

         _view.BindTo(_tableFormulaDTO);

         _tableFormulaDTO.AllPoints.CollectionChanged += notifyChange;
      }

      protected virtual IParameter Owner => null;
      protected virtual string OwnerName => Owner?.Name;

      private void notifyChange(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs) => ViewChanged();

      public string Description
      {
         set => View.Description = value;
         get => View.Description;
      }

      public string ImportToolTip
      {
         set => View.ImportToolTip = value;
         get => View.ImportToolTip;
      }

      public virtual void SetUseDerivedValues(bool useDerivedValues)
      {
         
      }

      public virtual void SetRestartSolver(ValuePointDTO valuePointDTO, bool restart)
      {
         
      }

      public void ImportTable()
      {
         var importedData = ImportTablePoints();
         if (importedData == null)
            return;

         ApplyImportedTablePoints(importedData);
         ViewChanged();
      }

      protected abstract DataRepository ImportTablePoints();

      protected abstract void ApplyImportedTablePoints(DataRepository importedTablePoints);

      public IEnumerable<ValuePointDTO> AllPoints => _tableFormulaDTO.AllPoints;
   }
}