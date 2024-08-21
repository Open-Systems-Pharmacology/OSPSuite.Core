using System;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Maths.Interpolations;
using OSPSuite.Helpers;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Presenters.Parameters;
using OSPSuite.Presentation.Views.Parameters;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Presentation
{
   internal class concern_for_TableFormulaPresenter : ContextSpecification<TableFormulaPresenterForSpecs>
   {
      protected ITableFormulaView _view;

      protected override void Context()
      {
         _view = A.Fake<ITableFormulaView>();
         sut = new TableFormulaPresenterForSpecs(_view);
      }


   }

   internal class When_importing_a_table_formula : concern_for_TableFormulaPresenter
   {
      private TableFormula _tableFormula;

      protected override void Context()
      {
         base.Context();
         _tableFormula = new TableFormula();
         sut.Edit(_tableFormula);
      }

      protected override void Because()
      {
         sut.ImportTable();
      }

      [Observation]
      public void the_imported_table_should_be_applied()
      {
         sut.ImportedFormula.ShouldNotBeNull();
      }
   }

   internal class When_canceling_importing_a_table_formula : concern_for_TableFormulaPresenter
   {
      private TableFormula _tableFormula;

      protected override void Context()
      {
         base.Context();
         _tableFormula = new TableFormula();
         sut.Edit(_tableFormula);
         sut.ImportedDataRepository = null;
      }

      protected override void Because()
      {
         sut.ImportTable();
      }

      [Observation]
      public void the_imported_table_should_not_be_applied()
      {
         sut.ImportedFormula.ShouldBeNull();
      }
   }

   internal class When_editing_a_table_formula : concern_for_TableFormulaPresenter
   {
      private TableFormula _tableFormula;

      protected override void Context()
      {
         base.Context();
         _tableFormula = new TableFormula
         {
            Name = "TableFormulaName",
            XDimension = DimensionFactoryForSpecs.TimeDimension,
            YName = "YName"
         };
      }

      protected override void Because()
      {
         sut.Edit(_tableFormula);
      }

      [Observation]
      public void captions_must_have_been_set()
      {
         A.CallTo(_view).Where(call => call.Method.Name.Equals("set_XCaption") && call.Arguments.Get<string>(0).Equals(Constants.NameWithUnitFor(_tableFormula.XName, _tableFormula.XDisplayUnit))).MustHaveHappened();
         A.CallTo(_view).Where(call => call.Method.Name.Equals("set_YCaption") && call.Arguments.Get<string>(0).Equals(Constants.NameWithUnitFor(_tableFormula.YName, _tableFormula.YDisplayUnit))).MustHaveHappened();
      }

      [Observation]
      public void the_table_formula_should_be_bound_to_the_view()
      {
         A.CallTo(() => _view.BindTo(A<TableFormulaDTO>._)).MustHaveHappened();
      }
   }

   internal class TableFormulaPresenterForSpecs : TableFormulaPresenter<ITableFormulaView>
   {
      public DataRepository ImportedDataRepository { set; get; }
      public TableFormulaPresenterForSpecs(ITableFormulaView view) : base(view)
      {
         var baseGrid = new BaseGrid("id", "name", DimensionFactoryForSpecs.TimeDimension);
         var dataColumn = new DataColumn("column", DimensionFactoryForSpecs.ConcentrationDimension, baseGrid)
         {
            InternalValues = new List<float>()
         };

         var dataRepository = new DataRepository
         {
            baseGrid,
            dataColumn
         };

         baseGrid.InternalValues.AddRange(new[] { 1.0f, 2.0f });
         dataColumn.InternalValues.AddRange(new[] { 1.0f, 2.0f });

         ImportedDataRepository = dataRepository;
      }

      protected override DataRepository ImportTablePoints()
      {
         return ImportedDataRepository;
      }

      public override void SetXValue(ValuePointDTO valuePointDTO, double newValue)
      {
         throw new NotImplementedException();
      }

      public override void SetYValue(ValuePointDTO valuePointDTO, double newValue)
      {
         throw new NotImplementedException();
      }

      public override void RemovePoint(ValuePointDTO pointToRemove)
      {
         throw new NotImplementedException();
      }

      public override void AddPoint()
      {
         throw new NotImplementedException();
      }

      protected override void ApplyImportedTablePoints(DataRepository importedTablePoints)
      {
         ImportedFormula = new TableFormula(new LinearInterpolation());
         importedTablePoints.BaseGrid.InternalValues.Each(x => { ImportedFormula.AddPoint(new ValuePoint(x, importedTablePoints.AllButBaseGridAsArray.Single().GetValue(x))); });
      }

      public TableFormula ImportedFormula { get; private set; }
   }
}