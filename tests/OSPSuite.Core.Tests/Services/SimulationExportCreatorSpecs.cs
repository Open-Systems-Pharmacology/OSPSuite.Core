using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Serialization.SimModel.DTO;
using OSPSuite.Core.Serialization.SimModel.Services;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Services
{
   internal class concern_for_SimulationExportCreator : ContextSpecification<SimulationExportCreator>
   {
      private IObjectPathFactory _objectPathFactory;
      private ITableFormulaToTableFormulaExportMapper _tableFormulaExportMapper;
      private IConcentrationBasedFormulaUpdater _concentrationBasedFormulaUpdater;
      private IEntityPathResolver _entityPathResolver;
      private IModel _model;

      protected override void Context()
      {
         base.Context();

         _model = new Model
         {
            Root = new ARootContainer()
         };
         _objectPathFactory = A.Fake<IObjectPathFactory>();
         _tableFormulaExportMapper = A.Fake<ITableFormulaToTableFormulaExportMapper>();
         _concentrationBasedFormulaUpdater = A.Fake<IConcentrationBasedFormulaUpdater>();
         _entityPathResolver = A.Fake<IEntityPathResolver>();

         sut = new SimulationExportCreator(_objectPathFactory, _tableFormulaExportMapper, _concentrationBasedFormulaUpdater, _entityPathResolver);
      }

      public class When_visiting_parameter_with_explicit_formula_and_value : concern_for_SimulationExportCreator
      {
         private IParameter _parameter;
         private SimulationExport _modelExport;

         protected override void Context()
         {
            base.Context();
            _parameter = new Parameter();
            _parameter.Formula = A.Fake<IFormula>();
            A.CallTo(() => _parameter.Formula.Calculate(A<IUsingFormula>._)).Returns(0.11704000000000003);
            _model.Root.Add(_parameter);
         }

         protected override void Because()
         {
            _modelExport = sut.CreateExportFor(_model);
         }

         [Observation]
         public void the_serialized_value_should_drop_precision()
         {
            var formulaExport = _modelExport.FormulaList.First() as ExplicitFormulaExport;
            // we are testing that precision beyond 15 significant digits is not preserved during serialization
            formulaExport.Equation.ShouldBeEqualTo("0.11704");
         }
      }
   }
}