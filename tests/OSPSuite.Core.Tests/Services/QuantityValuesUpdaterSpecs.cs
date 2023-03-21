using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Services
{
   public abstract class concern_for_QuantityValuesUpdater : ContextSpecification<IQuantityValuesUpdater>
   {
      protected IConcentrationBasedFormulaUpdater _concentrationBasedFormulaUpdater;
      protected IKeywordReplacerTask _keywordReplacerTask;
      protected ICloneManagerForModel _cloneManagerForModel;
      protected IFormulaFactory _formulaFactory;
      protected IModel _model;
      protected SimulationConfiguration _simulationConfiguration;

      protected override void Context()
      {
         _concentrationBasedFormulaUpdater = A.Fake<IConcentrationBasedFormulaUpdater>();
         _keywordReplacerTask = A.Fake<IKeywordReplacerTask>();
         _cloneManagerForModel = A.Fake<ICloneManagerForModel>();
         _formulaFactory = A.Fake<IFormulaFactory>();

         sut = new QuantityValuesUpdater(_keywordReplacerTask, _cloneManagerForModel, _formulaFactory, _concentrationBasedFormulaUpdater);

         _model = A.Fake<IModel>();
         _simulationConfiguration = new SimulationConfigurationForSpecs();
      }
   }
 }