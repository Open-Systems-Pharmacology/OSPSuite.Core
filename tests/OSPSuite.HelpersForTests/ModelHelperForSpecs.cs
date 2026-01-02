using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Utility.Extensions;
using static OSPSuite.Core.Domain.ObjectPath;
using static OSPSuite.Core.Domain.ObjectPathKeywords;
using static OSPSuite.Helpers.ConstantsForSpecs;

namespace OSPSuite.Helpers
{
   public class ModelHelperForSpecs
   {
      private readonly IInitialConditionsCreator _initialConditionsCreator;
      private readonly IObjectBaseFactory _objectBaseFactory;
      private readonly IObjectPathFactory _objectPathFactory;
      private readonly IDimensionFactory _dimensionFactory;
      private readonly IParameterValuesCreator _parameterValuesCreator;
      private readonly ISpatialStructureFactory _spatialStructureFactory;
      private readonly INeighborhoodBuilderFactory _neighborhoodFactory;
      private readonly IOutputSchemaFactory _outputSchemaFactory;
      private readonly IMoleculeBuilderFactory _moleculeBuilderFactory;
      private readonly ISolverSettingsFactory _solverSettingsFactory;
      private readonly IParameterFactory _parameterFactory;

      private IDimension amountPerTimeDimension => _dimensionFactory.Dimension(Constants.Dimension.AMOUNT_PER_TIME);

      private IDimension amountDimension => _dimensionFactory.Dimension(Constants.Dimension.MOLAR_AMOUNT);

      public ModelHelperForSpecs(
         IObjectBaseFactory objectBaseFactory,
         IParameterValuesCreator parameterValuesCreator,
         IInitialConditionsCreator initialConditionsCreator,
         IObjectPathFactory objectPathFactory,
         IDimensionFactory dimensionFactory,
         ISpatialStructureFactory spatialStructureFactory,
         INeighborhoodBuilderFactory neighborhoodFactory,
         IOutputSchemaFactory outputSchemaFactory,
         IMoleculeBuilderFactory moleculeBuilderFactory,
         ISolverSettingsFactory solverSettingsFactory,
         IParameterFactory parameterFactory
      )
      {
         _objectBaseFactory = objectBaseFactory;
         _neighborhoodFactory = neighborhoodFactory;
         _outputSchemaFactory = outputSchemaFactory;
         _moleculeBuilderFactory = moleculeBuilderFactory;
         _solverSettingsFactory = solverSettingsFactory;
         _parameterFactory = parameterFactory;
         _spatialStructureFactory = spatialStructureFactory;
         _parameterValuesCreator = parameterValuesCreator;
         _initialConditionsCreator = initialConditionsCreator;
         _objectPathFactory = objectPathFactory;
         _dimensionFactory = dimensionFactory;
      }

      public SimulationConfiguration CreateSimulationConfiguration()
      {
         var module = _objectBaseFactory.Create<Module>().WithName("Module");
         module.Add(getMolecules());
         module.Add(getReactions());
         module.Add(getPassiveTransports());
         module.Add(getSpatialStructure());
         module.Add(getObservers());
         module.Add(getEventGroups());

         var simulationConfiguration = new SimulationConfiguration()
         {
            SimulationSettings = createSimulationConfiguration(),
         };

         allCalculationMethods().Each(simulationConfiguration.AddCalculationMethod);
         var initialConditions = _initialConditionsCreator.CreateFrom(module.SpatialStructure, module.Molecules.ToList());

         //add one start values that does not exist in Molecules@"
         var initialCondition = _initialConditionsCreator.CreateInitialCondition(Constants.ORGANISM.ToObjectPath(), "MoleculeThatDoesNotExist", amountDimension);
         initialCondition.IsPresent = true;
         initialConditions.Add(initialCondition);
         var parameterValues = _objectBaseFactory.Create<ParameterValuesBuildingBlock>();
         setInitialConditions(initialConditions);
         setParameterValues(parameterValues);

         module.Add(initialConditions);
         module.Add(parameterValues);

         simulationConfiguration.Individual = getIndividual();

         var moduleConfiguration = new ModuleConfiguration(module, initialConditions, parameterValues);
         simulationConfiguration.AddModuleConfiguration(moduleConfiguration);

         return simulationConfiguration;
      }

      private IndividualBuildingBlock getIndividual()
      {
         //create a new individual building block with one parameter that will override a value that already exists
         //and one parameter that does not exist in the spatial structure and will be added to the simulation
         var individual = _objectBaseFactory.Create<IndividualBuildingBlock>();
         individual.Add(new IndividualParameter
         {
            //original value is 10
            Path = new ObjectPath(Constants.ORGANISM, ArterialBlood, P),
            Value = 20
         });

         individual.Add(new IndividualParameter
         {
            //new parameter that does not exist
            Path = new ObjectPath(Constants.ORGANISM, ArterialBlood, "NEW_PARAM"),
            Value = 10,
            Dimension = amountPerTimeDimension
         });

         //putting this one first to show that order does not matter
         individual.Add(new IndividualParameter
         {
            //new parameter that does not exist
            Path = new ObjectPath(Constants.ORGANISM, ArterialBlood, "NEW_PARAM_DISTRIBUTED", Constants.Distribution.MEAN),
            Value = 10,
            Dimension = amountPerTimeDimension
         });

         individual.Add(new IndividualParameter
         {
            //new parameter that does not exist
            Path = new ObjectPath(Constants.ORGANISM, ArterialBlood, "NEW_PARAM_DISTRIBUTED"),
            DistributionType = DistributionType.Normal,
            Dimension = amountPerTimeDimension
         });

         individual.Add(new IndividualParameter
         {
            //new parameter that does not exist
            Path = new ObjectPath(Constants.ORGANISM, ArterialBlood, "NEW_PARAM_DISTRIBUTED", Constants.Distribution.DEVIATION),
            Value = 2,
            Dimension = amountPerTimeDimension
         });

         individual.Add(new IndividualParameter
         {
            //new parameter that does not exist
            Path = new ObjectPath(Constants.ORGANISM, ArterialBlood, "NEW_PARAM_DISTRIBUTED", Constants.Distribution.PERCENTILE),
            Value = 0.5
         });


         //dummy parameter that does not fit in the structure
         individual.Add(new IndividualParameter
         {
            //new parameter that does not exist
            Path = new ObjectPath(Constants.ORGANISM, "DOES_NOT_EXIST", "NOPE"),
            Value = 10,
         });
         return individual;
      }

      private SimulationSettings createSimulationConfiguration()
      {
         return new SimulationSettings {Solver = _solverSettingsFactory.CreateCVODE(), OutputSchema = createDefaultOutputSchema(), OutputSelections = new OutputSelections()};
      }

      private OutputSchema createDefaultOutputSchema()
      {
         return _outputSchemaFactory.Create(0, 1440, 240);
      }

      private IEnumerable<CoreCalculationMethod> allCalculationMethods()
      {
         var cm1 = _objectBaseFactory.Create<CoreCalculationMethod>().WithName("CM1");
         cm1.Category = "PartitionCoeff";
         cm1.AddOutputFormula(PartitionCoeff_1(), new ParameterDescriptor("K", Create.Criteria(x => x.With("Cell2Plasma"))));
         var helpMeLungPlasma = NewConstantParameter("HelpMe", 10);
         cm1.AddHelpParameter(helpMeLungPlasma, Create.Criteria(x => x.With(Lung).And.With(Plasma)));
         var helpMeBonePlasma = NewConstantParameter("HelpMe", 20);
         cm1.AddHelpParameter(helpMeBonePlasma, Create.Criteria(x => x.With(Bone).And.With(Plasma)));

         var cm2 = _objectBaseFactory.Create<CoreCalculationMethod>().WithName("CM2");
         cm2.Category = "PartitionCoeff";
         cm2.AddOutputFormula(PartitionCoeff_2(), new ParameterDescriptor("K", Create.Criteria(x => x.With("Cell2Plasma"))));
         return new[] {cm1, cm2};
      }

      private EventGroupBuildingBlock getEventGroups()
      {
         var eventGroupBuilderCollection = _objectBaseFactory.Create<EventGroupBuildingBlock>();
         eventGroupBuilderCollection.Add(createBolusApplication(eventGroupBuilderCollection.FormulaCache));
         return eventGroupBuilderCollection;
      }

      private EventGroupBuilder createBolusApplication(IFormulaCache cache)
      {
         var eventGroup = _objectBaseFactory.Create<EventGroupBuilder>().WithName("Bolus Application");
         eventGroup.SourceCriteria = Create.Criteria(x => x.With(ArterialBlood).And.With(Plasma));

         var eventBuilder = _objectBaseFactory.Create<EventBuilder>()
            .WithName("application")
            .WithFormula(conditionForBolusApp(cache));
         eventBuilder.OneTime = true;

         var eventAssignment = _objectBaseFactory.Create<EventAssignmentBuilder>()
            .WithName("BLA")
            .WithFormula(createBolusDosisFormula(cache));
         eventAssignment.UseAsValue = true;
         eventAssignment.ObjectPath = new ObjectPath(Constants.ORGANISM, ArterialBlood, Plasma, "A");
         eventBuilder.AddAssignment(eventAssignment);
         eventBuilder.AddParameter(NewConstantParameter("StartTime", 10));
         eventGroup.Add(eventBuilder);
         return eventGroup;
      }

      private IFormula createBolusDosisFormula(IFormulaCache cache)
      {
         var dosisFormula = _objectBaseFactory.Create<ExplicitFormula>()
            .WithFormulaString("A+10")
            .WithName("BolusDosis");
         dosisFormula.AddObjectPath(_objectPathFactory.CreateFormulaUsablePathFrom(
            Constants.ORGANISM, ArterialBlood,
            Plasma, "A").WithAlias("A"));
         cache.Add(dosisFormula);
         return dosisFormula;
      }

      private IFormula conditionForBolusApp(IFormulaCache cache)
      {
         var conditionFormula = _objectBaseFactory.Create<ExplicitFormula>()
            .WithFormulaString("Time = StartTime")
            .WithName("StartCondition");
         conditionFormula.AddObjectPath(_objectPathFactory.CreateTimePath(_dimensionFactory.Dimension(Constants.Dimension.TIME)));
         conditionFormula.AddObjectPath(_objectPathFactory.CreateFormulaUsablePathFrom("StartTime").WithAlias("StartTime"));
         cache.Add(conditionFormula);
         return conditionFormula;
      }

      private void setParameterValues(ParameterValuesBuildingBlock parameterValues)
      {
         //set a parameter value to a formula 
         var moleculeAPath = new ObjectPath(Constants.ORGANISM, Lung, Plasma, "D");
         moleculeAPath.Add("RelExpNorm");
         var formula = _objectBaseFactory.Create<ExplicitFormula>().WithFormulaString("RelExp + RelExpGlobal").WithName("RelExpNormD");
         formula.AddObjectPath(_objectPathFactory.CreateFormulaUsablePathFrom(MOLECULE, "RelExpGlobal"));
         formula.AddObjectPath(_objectPathFactory.CreateFormulaUsablePathFrom(PARENT_CONTAINER, "RelExp").WithAlias("RelExp"));
         parameterValues.AddFormula(formula);

         var parameterValue = _parameterValuesCreator.CreateParameterValue(moleculeAPath, 0, Constants.Dimension.NO_DIMENSION);
         parameterValue.Formula = formula;
         parameterValues.Add(parameterValue);

         var parameterPath = new ObjectPath(Constants.ORGANISM, Bone, Cell, "FormulaParameterOverwritten");
         parameterValues.Add(_parameterValuesCreator.CreateParameterValue(parameterPath, 300, Constants.Dimension.NO_DIMENSION));


         //overwrite to make sure we have a value for a given path
         var nanParameterNotNaN = new ObjectPath(Constants.ORGANISM, Bone, Cell, "E", "OtherNaNParam");

         //NAN parameters are not added to the PV by default
         parameterValues.Add(_parameterValuesCreator.CreateParameterValue(nanParameterNotNaN, 10, Constants.Dimension.NO_DIMENSION));


         //dummy parameter that will be added dynamically to the organism 
         parameterValues.Add(_parameterValuesCreator.CreateParameterValue(
            new ObjectPath(Constants.ORGANISM, "NewParameterAddedFromParameterValues"),
            10, Constants.Dimension.NO_DIMENSION));
      }

      private void setInitialConditions(InitialConditionsBuildingBlock moleculesStartValues)
      {
         var art_plasma_A = moleculesStartValues[new ObjectPath(Constants.ORGANISM, ArterialBlood, Plasma, "A")];
         art_plasma_A.Value = 1;

         var lng_plasma_A = moleculesStartValues[new ObjectPath(Constants.ORGANISM, Lung, Plasma, "A")];
         lng_plasma_A.Value = 6;

         var ven_plasma_A = moleculesStartValues[new ObjectPath(Constants.ORGANISM, VenousBlood, Plasma, "A")];
         ven_plasma_A.Value = 1;

         var lng_plasma_B = moleculesStartValues[new ObjectPath(Constants.ORGANISM, Lung, Plasma, "B")];
         lng_plasma_B.Value = 7;

         var ven_plasma_B = moleculesStartValues[new ObjectPath(Constants.ORGANISM, VenousBlood, Plasma, "B")];
         ven_plasma_B.Value = 0.4;

         setAllIsPresentForMoleculeToFalse(moleculesStartValues, "C", "D", "E", "F", "Enz");
         var lng_cell_C = moleculesStartValues[new ObjectPath(Constants.ORGANISM, Lung, Cell, "C")];
         lng_cell_C.IsPresent = true;
         lng_cell_C.Value = 0;

         var bon_cell_C = moleculesStartValues[new ObjectPath(Constants.ORGANISM, Bone, Cell, "C")];
         bon_cell_C.IsPresent = true;
         bon_cell_C.Value = 0;

         var lng_plasma_D = moleculesStartValues[new ObjectPath(Constants.ORGANISM, Lung, Plasma, "D")];
         lng_plasma_D.Value = 8;
         lng_plasma_D.IsPresent = true;

         var ven_plasma_D = moleculesStartValues[new ObjectPath(Constants.ORGANISM, VenousBlood, Plasma, "D")];
         ven_plasma_D.Value = 0;
         ven_plasma_D.IsPresent = true;

         var bon_cell_E = moleculesStartValues[new ObjectPath(Constants.ORGANISM, Bone, Cell, "E")];
         bon_cell_E.Value = 2;
         bon_cell_E.IsPresent = true;
         var formula = _objectBaseFactory.Create<ExplicitFormula>().WithFormulaString("RelExp").WithName("RelExpNormE");
         formula.AddObjectPath(_objectPathFactory.CreateFormulaUsablePathFrom("RelExp").WithAlias("RelExp"));
         moleculesStartValues.AddFormula(formula);
         bon_cell_E.Formula = formula;
         bon_cell_E.ScaleDivisor = 2.5;

         var ven_plasma_E = moleculesStartValues[new ObjectPath(Constants.ORGANISM, VenousBlood, Plasma, "E")];
         ven_plasma_E.Value = 0;
         ven_plasma_E.IsPresent = true;

         var ven_plasma_F = moleculesStartValues[new ObjectPath(Constants.ORGANISM, Bone, Cell, "F")];
         ven_plasma_F.IsPresent = true;
      }

      private void setAllIsPresentForMoleculeToFalse(InitialConditionsBuildingBlock moleculesStartValues, params string[] moleculeNames)
      {
         foreach (var moleculesStartValue in moleculesStartValues)
         {
            if (moleculeNames.Contains(moleculesStartValue.MoleculeName))
               moleculesStartValue.IsPresent = false;
         }
      }

      private ObserverBuildingBlock getObservers()
      {
         var observers = _objectBaseFactory.Create<ObserverBuildingBlock>();

         var amountObserver1 = _objectBaseFactory.Create<AmountObserverBuilder>().WithName("AmountObs_1");
         amountObserver1.Dimension = _dimensionFactory.Dimension(Constants.Dimension.MOLAR_AMOUNT);
         amountObserver1.MoleculeList.ForAll = true;
         amountObserver1.Formula = AmountObs(observers.FormulaCache);
         amountObserver1.ContainerCriteria = Create.Criteria(x => x.With(Plasma));
         observers.Add(amountObserver1);

         var amountObserver2 = _objectBaseFactory.Create<AmountObserverBuilder>().WithName("AmountObs_2");
         amountObserver2.MoleculeList.ForAll = false;
         amountObserver2.Dimension = _dimensionFactory.Dimension(Constants.Dimension.MOLAR_AMOUNT);
         amountObserver2.Formula = AmountObs(observers.FormulaCache);
         amountObserver2.AddMoleculeName("C");
         amountObserver2.AddMoleculeName("E");
         amountObserver2.ContainerCriteria = Create.Criteria(x => x.With(Cell));
         observers.Add(amountObserver2);

         var amountObserver3 = _objectBaseFactory.Create<AmountObserverBuilder>().WithName("AmountObs_3");
         amountObserver3.MoleculeList.ForAll = true;
         amountObserver3.Dimension = _dimensionFactory.Dimension(Constants.Dimension.MOLAR_AMOUNT);
         amountObserver3.Formula = AmountObs(observers.FormulaCache);
         amountObserver3.AddMoleculeNameToExclude("C");
         amountObserver3.AddMoleculeNameToExclude("E");
         amountObserver3.ContainerCriteria = Create.Criteria(x => x.With(Cell));
         observers.Add(amountObserver3);

         var containerObserverBuilder = _objectBaseFactory.Create<ContainerObserverBuilder>().WithName("ContainerObs_1");
         containerObserverBuilder.Dimension = _dimensionFactory.Dimension(Constants.Dimension.MOLAR_AMOUNT);
         containerObserverBuilder.MoleculeList.ForAll = false;
         containerObserverBuilder.AddMoleculeName("C");
         containerObserverBuilder.Formula = ContainerObs(observers.FormulaCache);
         containerObserverBuilder.ContainerCriteria = Create.Criteria(x => x.With(Constants.ORGANISM));
         observers.Add(containerObserverBuilder);


         var fractionObserver = _objectBaseFactory.Create<AmountObserverBuilder>().WithName("FractionObserver_1");
         fractionObserver.Dimension = _dimensionFactory.Dimension(Constants.Dimension.FRACTION);
         fractionObserver.MoleculeList.ForAll = true;
         fractionObserver.Formula = FractionObs(observers.FormulaCache);
         fractionObserver.ContainerCriteria = Create.Criteria(x => x.With(Plasma));
         observers.Add(fractionObserver);


         var observedInOrganismLungPlasma = _objectBaseFactory.Create<ContainerObserverBuilder>().WithName("InContainerObserver");
         observedInOrganismLungPlasma.Dimension = _dimensionFactory.Dimension(Constants.Dimension.MOLAR_AMOUNT);
         observedInOrganismLungPlasma.MoleculeList.ForAll = false;
         observedInOrganismLungPlasma.AddMoleculeName("C");
         observedInOrganismLungPlasma.Formula = InContainerObs(observers.FormulaCache);
         observedInOrganismLungPlasma.ContainerCriteria = Create.Criteria(x => x.With(Plasma).And.InContainer(Lung));
         observers.Add(observedInOrganismLungPlasma);

         var observedInOrganismNotLungPlasma = _objectBaseFactory.Create<ContainerObserverBuilder>().WithName("NotInContainerObserver");
         observedInOrganismNotLungPlasma.Dimension = _dimensionFactory.Dimension(Constants.Dimension.MOLAR_AMOUNT);
         observedInOrganismNotLungPlasma.MoleculeList.ForAll = false;
         observedInOrganismNotLungPlasma.AddMoleculeName("C");
         observedInOrganismNotLungPlasma.Formula = InContainerObs(observers.FormulaCache);
         observedInOrganismNotLungPlasma.ContainerCriteria = Create.Criteria(x => x.With(Plasma).And.NotInContainer(Lung));
         observers.Add(observedInOrganismNotLungPlasma);

         return observers;
      }

      private MoleculeBuildingBlock getMolecules()
      {
         var molecules = _objectBaseFactory.Create<MoleculeBuildingBlock>();
         molecules.Add(createMoleculeA(molecules.FormulaCache));
         molecules.Add(createMoleculeB(molecules.FormulaCache));
         molecules.Add(createMoleculeC(molecules.FormulaCache));
         molecules.Add(createMoleculeD(molecules.FormulaCache));
         molecules.Add(createMoleculeE(molecules.FormulaCache));
         molecules.Add(createMoleculeF(molecules.FormulaCache));

         return molecules;
      }

      private MoleculeBuilder createMoleculeA(IFormulaCache formulaCache)
      {
         var moleculeA = DefaultMolecule("A", 1, 2, QuantityType.Drug, formulaCache);
         var oneGlobalParameter = NewConstantParameter("oneGlobalParameter", 33).WithMode(ParameterBuildMode.Global);
         moleculeA.AddParameter(oneGlobalParameter);
         moleculeA.AddParameter(NewConstantParameter(Constants.Parameters.MOL_WEIGHT, 250).WithMode(ParameterBuildMode.Global));

         var localParameterWithCriteria = NewConstantParameter("LocalWithCriteria", 0).WithMode(ParameterBuildMode.Local);
         //Create a local parameter that should only be defined in Plasma
         localParameterWithCriteria.ContainerCriteria = Create.Criteria(x => x.With(Plasma));
         moleculeA.AddParameter(localParameterWithCriteria);


         moleculeA.AddUsedCalculationMethod(new UsedCalculationMethod("PartitionCoeff", "CM1"));
         var transporter1 = _objectBaseFactory.Create<TransporterMoleculeContainer>().WithName("D");
         transporter1.TransportName = "My Transport1";

         transporter1.AddParameter(NewConstantParameter("GlobalTransportParameter", 250, ParameterBuildMode.Global));
         transporter1.AddParameter(NewConstantParameter("LocalTransportParameter", 250));

         var realization1 =
            _objectBaseFactory.Create<TransportBuilder>().WithName("Transporter #1")
               .WithKinetic(MM1FormulaFrom(formulaCache))
               .WithDimension(amountPerTimeDimension);

         realization1.CreateProcessRateParameter = true;
         realization1.AddParameter(NewConstantParameter("Km", 33));
         realization1.AddParameter(NewConstantParameter("VmaxAbs", 1));
         realization1.AddParameter(NewConstantParameter("p1", 2));
         realization1.AddParameter(NewConstantParameter("GlobalRealizationParameter", 2, ParameterBuildMode.Global));

         realization1.TransportType = TransportType.Influx;
         realization1.SourceCriteria = Create.Criteria(x => x.With(Plasma));
         realization1.TargetCriteria = Create.Criteria(x => x.With(Cell));
         transporter1.AddActiveTransportRealization(realization1);
         var transporter2 = _objectBaseFactory.Create<TransporterMoleculeContainer>().WithName("E");
         transporter2.TransportName = "My Transport2";
         var realization2 = _objectBaseFactory.Create<TransportBuilder>()
            .WithName("Transporter #2")
            .WithKinetic(MM1FormulaFrom(formulaCache))
            .WithDimension(amountPerTimeDimension);


         realization2.AddParameter(NewConstantParameter("Km", 66));
         realization2.AddParameter(NewConstantParameter("VmaxAbs", 2));
         realization2.TransportType = TransportType.Efflux;
         realization2.SourceCriteria = Create.Criteria(x => x.With(Cell));
         realization2.TargetCriteria = Create.Criteria(x => x.With(Plasma));
         realization2.CreateProcessRateParameter = true;
         transporter2.AddActiveTransportRealization(realization2);
         moleculeA.AddTransporterMoleculeContainer(transporter1);
         moleculeA.AddTransporterMoleculeContainer(transporter2);
         return moleculeA;
      }

      private MoleculeBuilder createMoleculeB(IFormulaCache formulaCache)
      {
         var moleculeB = DefaultMolecule("B", 2, 2, QuantityType.Drug, formulaCache);
         moleculeB.DefaultStartFormula = B_StartF(formulaCache);
         moleculeB.AddUsedCalculationMethod(new UsedCalculationMethod("PartitionCoeff", "CM2"));

         var transporter1 = _objectBaseFactory.Create<TransporterMoleculeContainer>().WithName("D");
         transporter1.TransportName = "My Transport1";
         var realization1 = _objectBaseFactory.Create<TransportBuilder>()
            .WithName("Transporter #1")
            .WithKinetic(MM2FormulaFrom(formulaCache))
            .WithDimension(amountPerTimeDimension);

         realization1.AddParameter(NewConstantParameter("VmaxAbs", 1));

         realization1.TransportType = TransportType.Influx;
         realization1.SourceCriteria = Create.Criteria(x => x.With(Plasma));
         realization1.TargetCriteria = Create.Criteria(x => x.With(Cell));
         transporter1.AddActiveTransportRealization(realization1);
         var transporter2 = _objectBaseFactory.Create<TransporterMoleculeContainer>().WithName("XXX");
         transporter2.TransportName = "My Transport2";
         var realization2 =
            _objectBaseFactory.Create<TransportBuilder>()
               .WithName("Transporter #2")
               .WithKinetic(MM2FormulaFrom(formulaCache))
               .WithDimension(amountPerTimeDimension);


         realization2.AddParameter(NewConstantParameter("VmaxAbs", 2));
         realization2.TransportType = TransportType.Efflux;
         realization2.SourceCriteria = Create.Criteria(x => x.With(Cell));
         realization2.TargetCriteria = Create.Criteria(x => x.With(Plasma));
         transporter2.AddActiveTransportRealization(realization2);
         moleculeB.AddTransporterMoleculeContainer(transporter1);
         moleculeB.AddTransporterMoleculeContainer(transporter2);
         return moleculeB;
      }

      private MoleculeBuilder createMoleculeC(IFormulaCache formulaCache)
      {
         var moleculeC = DefaultMolecule("C", 3, 3, QuantityType.Drug, formulaCache);
         var globalParameter = NewConstantParameter("C_Global", 5, ParameterBuildMode.Global);
         var formula = _objectBaseFactory.Create<ExplicitFormula>().WithFormulaString("C_Global_Formula").WithFormulaString("2+2");

         //Add reference to global reaction parameter
         formula.AddObjectPath(_objectPathFactory.CreateFormulaUsablePathFrom("R1", "k2").WithAlias("k2"));
         globalParameter.Formula = formula;
         formulaCache.Add(formula);
         moleculeC.Add(globalParameter);

         moleculeC.IsFloating = false;
         return moleculeC;
      }

      private MoleculeBuilder createMoleculeD(IFormulaCache formulaCache)
      {
         var moleculeD = DefaultMolecule("D", 3, 3, QuantityType.Transporter, formulaCache);

         moleculeD.IsFloating = false;
         var relExp = NewConstantParameter("RelExp", 0).WithMode(ParameterBuildMode.Local);

         var relExpNorm = NewConstantParameter("RelExpNorm", 0).WithMode(ParameterBuildMode.Local);


         var relExpGlobal = NewConstantParameter("RelExpGlobal", 0).WithMode(ParameterBuildMode.Global);
         IFormula moleculeReferenceFormula = _objectBaseFactory.Create<ExplicitFormula>().WithFormulaString("RelExpGlobal");
         formulaCache.Add(moleculeReferenceFormula);
         moleculeReferenceFormula.AddObjectPath(_objectPathFactory.CreateFormulaUsablePathFrom(MOLECULE, "RelExpGlobal").WithAlias("RelExpGlobal"));
         var relExpLocalFromGlobal =
            _objectBaseFactory.Create<IParameter>()
               .WithName("RelExpLocal")
               .WithFormula(moleculeReferenceFormula)
               .WithMode(ParameterBuildMode.Local);
         moleculeD.AddParameter(relExp);
         moleculeD.AddParameter(relExpNorm);
         moleculeD.AddParameter(relExpGlobal);
         moleculeD.AddParameter(relExpLocalFromGlobal);

         return moleculeD;
      }

      private MoleculeBuilder createMoleculeE(IFormulaCache formulaCache)
      {
         var moleculeE = DefaultMolecule("E", 4, 5, QuantityType.Transporter, formulaCache);
         moleculeE.IsFloating = false;

         moleculeE.AddParameter(NewConstantParameter("RelExp", 0).WithMode(ParameterBuildMode.Local));

         //create a couple of NaN Parameters. Only a few should be created in the simulation
         moleculeE.AddParameter(NewConstantParameter("NaNParam", double.NaN).WithMode(ParameterBuildMode.Local));
         moleculeE.AddParameter(NewConstantParameter("OtherNaNParam", double.NaN).WithMode(ParameterBuildMode.Local));
         moleculeE.AddParameter(NewConstantParameter("GlobalNaNParam", double.NaN).WithMode(ParameterBuildMode.Global));
         return moleculeE;
      }

      private MoleculeBuilder createMoleculeF(IFormulaCache formulaCache)
      {
         var moleculeF = DefaultMolecule("F", 4, 5, QuantityType.Enzyme, formulaCache);
         moleculeF.IsXenobiotic = false;
         var pb1 = NewConstantParameter("RelExp", 0).WithMode(ParameterBuildMode.Local);

         var pb2 = NewConstantParameter("ProteinContent", 1).WithMode(ParameterBuildMode.Global);
         moleculeF.AddParameter(pb1);
         moleculeF.AddParameter(pb2);

         moleculeF.DefaultStartFormula = enzymeStartFormula(formulaCache);
         moleculeF.IsFloating = false;

         return moleculeF;
      }

      public MoleculeBuilder DefaultMolecule(string name, double MW, double logMA, QuantityType quantityType, IFormulaCache formulaCache)
      {
         var molecule = _moleculeBuilderFactory.Create(formulaCache).WithName(name);
         molecule.QuantityType = quantityType;
         molecule.DefaultStartFormula = _objectBaseFactory.Create<ConstantFormula>().WithValue(0).WithDimension(_dimensionFactory.Dimension(Constants.Dimension.MOLAR_AMOUNT));
         molecule.IsFloating = true;
         molecule.Dimension = _dimensionFactory.Dimension(Constants.Dimension.MOLAR_AMOUNT);
         molecule.DisplayUnit = molecule.Dimension.DefaultUnit;

         var molweight = NewConstantParameter("MW", MW);
         molweight.BuildMode = ParameterBuildMode.Global;
         molecule.AddParameter(molweight);

         if (!double.IsNaN(logMA))
         {
            var logMAParam = NewConstantParameter("logMA", logMA);
            logMAParam.BuildMode = ParameterBuildMode.Global;
            molecule.AddParameter(logMAParam);
         }

         var C = _objectBaseFactory.Create<IParameter>().WithName("C").WithFormula(ConcentrationFormulaFrom(formulaCache));
         C.BuildMode = ParameterBuildMode.Local;
         molecule.AddParameter(C);

         return molecule;
      }

      private IFormula B_StartF(IFormulaCache formulaCache)
      {
         var formula = formulaCache.FirstOrDefault(x => string.Equals(x.Name, "B_StartF"));
         if (formula != null)
            return formula;

         formula = _objectBaseFactory.Create<ExplicitFormula>().WithFormulaString("A/2").WithName("B_StartF")
            .WithDimension(amountDimension);

         formula.AddObjectPath(_objectPathFactory.CreateFormulaUsablePathFrom(PARENT_CONTAINER, "A").WithAlias("A"));

         //Add reference to global parameters in reaction
         formula.AddObjectPath(_objectPathFactory.CreateFormulaUsablePathFrom("R1", "k2").WithAlias("k2"));

         formulaCache.Add(formula);

         return formula;
      }

      private IFormula enzymeStartFormula(IFormulaCache formulaCache)
      {
         if (formulaCache.Contains("EnzymeStartFormula"))
            return formulaCache["EnzymeStartFormula"];

         var formula = _objectBaseFactory.Create<ExplicitFormula>().WithId("EnzymeStartFormula")
            .WithName("EnzymeStartFormula")
            .WithFormulaString("RelExp * ProteinContent");
         formula.AddObjectPath(_objectPathFactory.CreateFormulaUsablePathFrom("RelExp"));
         formula.AddObjectPath(_objectPathFactory.CreateFormulaUsablePathFrom(MOLECULE, "ProteinContent"));
         formulaCache.Add(formula);
         return formula;
      }

      private IFormula R1Formula(IFormulaCache formulaCache)
      {
         var formula = formulaCache.FirstOrDefault(x => string.Equals(x.Name, "R1"));
         if (formula != null)
            return formula;

         formula = _objectBaseFactory.Create<ExplicitFormula>().WithFormulaString("k1*k2*fu*A*B*C").WithName("R1");
         formula.AddObjectPath(_objectPathFactory.CreateFormulaUsablePathFrom("k1").WithAlias("k1"));
         formula.AddObjectPath(_objectPathFactory.CreateFormulaUsablePathFrom("R1", "k2"));
         formula.AddObjectPath(_objectPathFactory.CreateFormulaUsablePathFrom(Constants.ORGANISM, "fu").WithAlias("fu"));
         formula.AddObjectPath(_objectPathFactory.CreateFormulaUsablePathFrom(PARENT_CONTAINER, "A").WithAlias("A"));
         formula.AddObjectPath(_objectPathFactory.CreateFormulaUsablePathFrom(PARENT_CONTAINER, "B").WithAlias("B"));
         formula.AddObjectPath(_objectPathFactory.CreateFormulaUsablePathFrom(PARENT_CONTAINER, "C").WithAlias("C"));

         formulaCache.Add(formula);

         return formula;
      }

      private IFormula R1_K3Formula(IFormulaCache formulaCache)
      {
         var formula = formulaCache.FirstOrDefault(x => string.Equals(x.Name, "R1_K3"));
         if (formula != null)
            return formula;

         formula = _objectBaseFactory.Create<ExplicitFormula>().WithFormulaString("p").WithName("R1_K3");
         formula.AddObjectPath(_objectPathFactory.CreateFormulaUsablePathFrom("A", "oneGlobalParameter").WithAlias("p"));
         formulaCache.Add(formula);

         return formula;
      }

      private IFormula MM1FormulaFrom(IFormulaCache formulaCache)
      {
         var formula = formulaCache.FirstOrDefault(x => string.Equals(x.Name, "MM1"));
         if (formula != null)
            return formula;

         formula = _objectBaseFactory.Create<ExplicitFormula>().WithFormulaString("VmaxAbs*RelExp*C/(Km+C)").WithName("MM1");
         formula.AddObjectPath(
            _objectPathFactory.CreateFormulaUsablePathFrom(NEIGHBORHOOD, MOLECULE, REALIZATION, "VmaxAbs").WithAlias("VmaxAbs"));
         formula.AddObjectPath(_objectPathFactory.CreateFormulaUsablePathFrom(NEIGHBORHOOD, MOLECULE, REALIZATION, "Km").WithAlias("Km"));
         formula.AddObjectPath(_objectPathFactory.CreateFormulaUsablePathFrom(SOURCE, MOLECULE, "C").WithAlias("C"));
         formula.AddObjectPath(_objectPathFactory.CreateFormulaUsablePathFrom(SOURCE, TRANSPORTER, "RelExp").WithAlias("RelExp"));

         formulaCache.Add(formula);

         return formula;
      }

      private IFormula MM2FormulaFrom(IFormulaCache formulaCache)
      {
         var formula = formulaCache.FirstOrDefault(x => string.Equals(x.Name, "MM2"));
         if (formula != null)
            return formula;

         formula = _objectBaseFactory.Create<ExplicitFormula>().WithFormulaString("VMaxAbs*C/(1+C)").WithName("MM2");
         formula.AddObjectPath(
            _objectPathFactory.CreateFormulaUsablePathFrom(NEIGHBORHOOD, MOLECULE, REALIZATION, "VmaxAbs").WithAlias("VMaxAbs"));
         formula.AddObjectPath(_objectPathFactory.CreateFormulaUsablePathFrom(SOURCE, MOLECULE, "C").WithAlias("C"));

         formulaCache.Add(formula);

         return formula;
      }

      private IFormula ConcentrationFormulaFrom(IFormulaCache formulaCache)
      {
         var formula = formulaCache.FirstOrDefault(x => string.Equals(x.Name, "ConcFormula"));
         if (formula != null)
            return formula;

         formula = _objectBaseFactory.Create<ExplicitFormula>().WithFormulaString("M/V").WithName("ConcFormula");
         formula.AddObjectPath(_objectPathFactory.CreateFormulaUsablePathFrom(PARENT_CONTAINER).WithAlias("M"));
         formula.AddObjectPath(_objectPathFactory.CreateFormulaUsablePathFrom(PARENT_CONTAINER, PARENT_CONTAINER, Volume).WithAlias("V"));

         formulaCache.Add(formula);

         return formula;
      }

      public IParameter NewConstantParameter(string name, double value, ParameterBuildMode parameterBuildMode = ParameterBuildMode.Local)
      {
         return _parameterFactory.CreateParameter(name, value)
            .WithMode(parameterBuildMode);
      }

      private ReactionBuildingBlock getReactions()
      {
         var reactions = _objectBaseFactory.Create<ReactionBuildingBlock>();
         var R1 = _objectBaseFactory.Create<ReactionBuilder>()
            .WithName("R1")
            .WithKinetic(R1Formula(reactions.FormulaCache))
            .WithDimension(amountPerTimeDimension);

         var k1 = NewConstantParameter("k1", 22);
         k1.BuildMode = ParameterBuildMode.Local;
         R1.AddParameter(k1);

         var k2 = NewConstantParameter("k2", 1);
         k2.BuildMode = ParameterBuildMode.Global;
         R1.AddParameter(k2);

         //Parameter k3 is a formula parameter referencing a global paramter in A
         var k3 = NewConstantParameter("k3", 1).WithFormula(R1_K3Formula(reactions.FormulaCache));
         k3.BuildMode = ParameterBuildMode.Global;
         R1.AddParameter(k3);

         R1.AddEduct(new ReactionPartnerBuilder("A", 1));
         R1.AddProduct(new ReactionPartnerBuilder("C", 1));

         var R3 = _objectBaseFactory.Create<ReactionBuilder>()
            .WithName("R2")
            .WithKinetic(R1Formula(reactions.FormulaCache))
            .WithDimension(amountPerTimeDimension);

         k1 = NewConstantParameter("k1", 22);
         k1.BuildMode = ParameterBuildMode.Local;
         R3.AddParameter(k1);

         k2 = NewConstantParameter("k2", 1);
         k2.BuildMode = ParameterBuildMode.Global;
         R3.AddParameter(k2);

         //Parameter k3 is a formula parameter referencing a global paramter in A
         k3 = NewConstantParameter("k3", 1).WithFormula(R1_K3Formula(reactions.FormulaCache));
         k3.BuildMode = ParameterBuildMode.Global;
         R3.AddParameter(k3);

         R3.AddEduct(new ReactionPartnerBuilder("A", 1));
         R3.AddProduct(new ReactionPartnerBuilder("C", 1));


         var R2 = _objectBaseFactory.Create<ReactionBuilder>()
            .WithName("R2")
            .WithKinetic(R1Formula(reactions.FormulaCache))
            .WithDimension(amountPerTimeDimension);
         k1 = NewConstantParameter("k1", 22, ParameterBuildMode.Local);
         R2.AddParameter(k1);

         //global reaction parameter refernecing another global parameter
         k2 = NewConstantParameter("k2", 10, ParameterBuildMode.Global);
         var explicitFormula = new ExplicitFormula("k2").WithId("R2_k2_Formula").WithName("R2_k2_Formula");
         explicitFormula.AddObjectPath(new FormulaUsablePath("R1", "k2").WithAlias("k2"));
         k2.Formula = explicitFormula;

         reactions.AddFormula(explicitFormula);
         R2.AddParameter(k2);
         R2.AddEduct(new ReactionPartnerBuilder("A", 1));
         R2.AddProduct(new ReactionPartnerBuilder("C", 1));
         reactions.Add(R3);
         reactions.Add(R2);
         reactions.Add(R1);
         return reactions;
      }

      private PassiveTransportBuildingBlock getPassiveTransports()
      {
         var passiveTransportBuilderCollection = _objectBaseFactory.Create<PassiveTransportBuildingBlock>();
         //T1_1 "Blutfluss Arterial Blood=> Tissue Organ"
         var T1_1 = _objectBaseFactory.Create<TransportBuilder>()
            .WithName("T1_1")
            .WithDimension(amountPerTimeDimension);

         T1_1.TransportType = TransportType.Convection;
         T1_1.SourceCriteria = Create.Criteria(x => x.With(Plasma)
            .And.With(ArterialBlood));

         T1_1.TargetCriteria = Create.Criteria(x => x.With(Plasma)
            .And.Not(ArterialBlood)
            .And.Not(Lung)
            .And.Not(VenousBlood));

         T1_1.Formula = Convection(passiveTransportBuilderCollection.FormulaCache);

         T1_1.AddParameter(NewConstantParameter("p1", 1));

         passiveTransportBuilderCollection.Add(T1_1);

         //T1_2 "Blutfluss Tissue Organ => VenousBlood"
         var T1_2 = _objectBaseFactory.Create<TransportBuilder>()
            .WithName("T1_2")
            .WithDimension(amountPerTimeDimension);

         T1_2.TransportType = TransportType.Convection;
         T1_2.SourceCriteria = Create.Criteria(x => x.With(Plasma)
            .And.Not(ArterialBlood)
            .And.Not(Lung)
            .And.Not(VenousBlood));

         T1_2.TargetCriteria = Create.Criteria(x => x.With(Plasma)
            .And.With(VenousBlood));

         T1_2.Formula = Convection(passiveTransportBuilderCollection.FormulaCache);
         passiveTransportBuilderCollection.Add(T1_2);

         //T1_3 "Blutfluss VenousBlood => Lung"
         var T1_3 = _objectBaseFactory.Create<TransportBuilder>()
            .WithName("T1_3")
            .WithDimension(amountPerTimeDimension);

         T1_3.TransportType = TransportType.Convection;
         T1_3.SourceCriteria = Create.Criteria(x => x.With(Plasma)
            .And.With(VenousBlood));

         T1_3.TargetCriteria = Create.Criteria(x => x.With(Plasma)
            .And.With(Lung));

         T1_3.Formula = Convection(passiveTransportBuilderCollection.FormulaCache);
         passiveTransportBuilderCollection.Add(T1_3);

         //T1_4 "Blutfluss  Lung => ArterialBlood"
         var T1_4 = _objectBaseFactory.Create<TransportBuilder>()
            .WithName("T1_4")
            .WithDimension(amountPerTimeDimension);

         T1_4.TransportType = TransportType.Convection;
         T1_4.SourceCriteria = Create.Criteria(x => x.With(Plasma)
            .And.With(Lung));

         T1_4.TargetCriteria = Create.Criteria(x => x.With(Plasma)
            .And.With(ArterialBlood));

         T1_4.Formula = Convection(passiveTransportBuilderCollection.FormulaCache);
         passiveTransportBuilderCollection.Add(T1_4);

         //T2
         var T2 = _objectBaseFactory.Create<TransportBuilder>()
            .WithName("T2")
            .WithDimension(amountPerTimeDimension);

         T2.TransportType = TransportType.Diffusion;
         T2.SourceCriteria = Create.Criteria(x => x.With(Plasma));
         T2.TargetCriteria = Create.Criteria(x => x.With(Cell));

         T2.Formula = Diffusion(passiveTransportBuilderCollection.FormulaCache);
         passiveTransportBuilderCollection.Add(T2);

         return passiveTransportBuilderCollection;
      }

      private SpatialStructure getSpatialStructure()
      {
         var spatialStructure = _spatialStructureFactory.Create().WithName("SPATIAL STRUCTURE");

         var organism = _objectBaseFactory.Create<IContainer>()
            .WithName(Constants.ORGANISM)
            .WithMode(ContainerMode.Logical);
         organism.AddTag(new Tag(Constants.ORGANISM));

         //global molecule dependent param (a la K_rbc)
         var someGlobalMoleculeDepParam = _objectBaseFactory.Create<IParameter>().WithName("XYZ").WithFormula(GlobalMoleculeDepParamFormula_1(spatialStructure.FormulaCache));
         spatialStructure.GlobalMoleculeDependentProperties.Add(someGlobalMoleculeDepParam);

         //Create a parameter with formula in Organism with absolute path
         var bw = NewConstantParameter(BW, 20);
         organism.Add(bw);

         var tableParameter =
            _objectBaseFactory.Create<IParameter>()
               .WithName(TableParameter1)
               .WithFormula(TableFormula1(spatialStructure.FormulaCache));
         tableParameter.Persistable = true;
         organism.Add(tableParameter);

         var tableParameter2 =
            _objectBaseFactory.Create<IParameter>()
               .WithName(TableParameter2)
               .WithFormula(TableFormula2(spatialStructure.FormulaCache));
         organism.Add(tableParameter2);

         var organismMoleculeProperties = _objectBaseFactory.Create<IContainer>().WithName(Constants.MOLECULE_PROPERTIES)
            .WithMode(ContainerMode.Logical)
            .WithContainerType(ContainerType.Molecule);
         organism.Add(organismMoleculeProperties);
         organismMoleculeProperties.Add(_objectBaseFactory.Create<IParameter>().WithName("Fabs").WithFormula(Fabs(spatialStructure.FormulaCache)));

         organism.Add(_objectBaseFactory.Create<IParameter>().WithName("BMI").WithFormula(BMI(spatialStructure.FormulaCache)));

         var distributedParameter = _objectBaseFactory.Create<IDistributedParameter>().WithName("Distributed");
         organism.Add(distributedParameter);
         var mean = NewConstantParameter(Constants.Distribution.MEAN, 2);
         var percentile = NewConstantParameter(Constants.Distribution.PERCENTILE, 0.5);
         distributedParameter.Add(mean);
         distributedParameter.Add(percentile);
         distributedParameter.Formula =
            new DistributionFormulaFactory(_objectPathFactory, _objectBaseFactory).CreateDiscreteDistributionFormulaFor(
               distributedParameter, mean);

         //ART
         var art = CreateContainerWithName(ArterialBlood)
            .WithMode(ContainerMode.Logical);

         var artPlasma = CreateContainerWithName(Plasma).WithMode(ContainerMode.Physical);
         artPlasma.Add(NewConstantParameter(Volume, 2));
         art.Add(artPlasma);
         artPlasma.AddTag(new Tag(art.Name));
         art.Add(NewConstantParameter(Q, 2));
         art.Add(NewConstantParameter(P, 10));
         organism.Add(art);

         var parameterReferencingNeighborhood = NewConstantParameter("RefParam", 10);
         var refFormula = _objectBaseFactory.Create<ExplicitFormula>()
            .WithFormulaString("K*10")
            .WithName("FormulaReferencingNBH");
         parameterReferencingNeighborhood.Formula = refFormula;
         //referencing parameter K between art_pls and bone_pls

         var objectPath = _objectPathFactory.CreateFormulaUsablePathFrom(
            PARENT_CONTAINER,
            NBH,
            PARENT_CONTAINER, PARENT_CONTAINER, PARENT_CONTAINER, Bone, Plasma,
            NBH, "K");

         objectPath.Alias = "K";
         parameterReferencingNeighborhood.Formula.AddObjectPath(objectPath);
         artPlasma.Add(parameterReferencingNeighborhood);
         spatialStructure.FormulaCache.Add(refFormula);


         //LUNG
         var lung = CreateContainerWithName(Lung)
            .WithMode(ContainerMode.Logical);

         var lngPlasma = CreateContainerWithName(Plasma).WithMode(ContainerMode.Physical);
         lngPlasma.Add(NewConstantParameter(Volume, 2));
         lngPlasma.Add(NewConstantParameter(pH, 7.5));
         lngPlasma.AddTag(new Tag(lung.Name));
         lung.Add(lngPlasma);

         var lngCell = CreateContainerWithName(Cell).WithMode(ContainerMode.Physical);
         lngCell.Add(NewConstantParameter(Volume, 1));
         lngCell.Add(NewConstantParameter(pH, 7));
         lngCell.AddTag(new Tag(lung.Name));
         lung.Add(lngCell);

         lung.Add(NewConstantParameter(Q, 3));
         lung.Add(NewConstantParameter(P, 2));
         organism.Add(lung);

         //BONE
         var bone = CreateContainerWithName(Bone)
            .WithMode(ContainerMode.Logical);

         var bonePlasma = CreateContainerWithName(Plasma).WithMode(ContainerMode.Physical);
         bonePlasma.Add(NewConstantParameter(Volume, 2));
         bonePlasma.Add(NewConstantParameter(pH, 7.5));
         bonePlasma.AddTag(new Tag(bone.Name));
         bone.Add(bonePlasma);


         var boneCell = CreateContainerWithName(Cell).WithMode(ContainerMode.Physical);
         boneCell.Add(NewConstantParameter(Volume, 1));
         boneCell.Add(NewConstantParameter(pH, 7));
         var moleculeProperties = CreateContainerWithName(Constants.MOLECULE_PROPERTIES).WithMode(ContainerMode.Logical);
         boneCell.Add(moleculeProperties);
         moleculeProperties.Add(NewConstantParameter(Constants.ONTOGENY_FACTOR, 5));
         moleculeProperties.Add(NewConstantParameter(Constants.HALF_LIFE, 11));
         moleculeProperties.Add(NewConstantParameter(Constants.DEGRADATION_COEFF, 1));
         boneCell.AddTag(new Tag(bone.Name));
         bone.Add(boneCell);
         var formulaParameter = NewConstantParameter("FormulaParameterOverwritten", 10);
         formulaParameter.Formula = new ExplicitFormula("1+5").WithId("FormulaParameterOverwritten").WithName("FormulaParameterOverwritten");
         spatialStructure.AddFormula(formulaParameter.Formula);
         boneCell.Add(formulaParameter);

         bone.Add(NewConstantParameter(Q, 3));
         bone.Add(NewConstantParameter(P, 2));
         organism.Add(bone);

         //VEN
         var ven = CreateContainerWithName(VenousBlood)
            .WithMode(ContainerMode.Logical);
         var venPlasma = CreateContainerWithName(Plasma).WithMode(ContainerMode.Physical);
         venPlasma.Add(NewConstantParameter(Volume, 2));
         ven.Add(venPlasma);
         ven.Add(NewConstantParameter(Q, 2));
         venPlasma.AddTag(new Tag(ven.Name));
         organism.Add(ven);

         organism.Add(NewConstantParameter(fu, 1));
         spatialStructure.AddTopContainer(organism);

         var neighborhood1 = _neighborhoodFactory.CreateBetween(artPlasma, bonePlasma).WithName("art_pls_to_bon_pls");
         //this is a constant parmaeter that will be referenced from arterial plasma compartment
         neighborhood1.AddParameter(NewConstantParameter("K", 10));
         spatialStructure.AddNeighborhood(neighborhood1);
         var neighborhood2 = _neighborhoodFactory.CreateBetween(lngPlasma, artPlasma).WithName("lng_pls_to_art_pls");
         spatialStructure.AddNeighborhood(neighborhood2);
         var neighborhood3 = _neighborhoodFactory.CreateBetween(bonePlasma, venPlasma).WithName("bon_pls_to_ven_pls");
         spatialStructure.AddNeighborhood(neighborhood3);
         var neighborhood4 = _neighborhoodFactory.CreateBetween(venPlasma, lngPlasma).WithName("ven_pls_to_lng_pls");
         spatialStructure.AddNeighborhood(neighborhood4);

         var neighborhood5 = _neighborhoodFactory.CreateBetween(lngPlasma, lngCell).WithName("lng_pls_to_lng_cell");
         neighborhood5.AddTag("Cell2Plasma");
         var K = _objectBaseFactory.Create<IParameter>().WithName("K").WithFormula(BlackBoxPartitionCoeffFormula(spatialStructure.FormulaCache));
         neighborhood5.MoleculeProperties.Add(K);
         neighborhood5.AddParameter(NewConstantParameter("SA", 22));
         spatialStructure.AddNeighborhood(neighborhood5);
         var sumProcessRate = _objectBaseFactory.Create<IParameter>().WithName(SumProcessRate)
            .WithFormula(SumFormula(spatialStructure.FormulaCache));
         neighborhood5.MoleculeProperties.Add(sumProcessRate);

         var neighborhood6 = _neighborhoodFactory.CreateBetween(bonePlasma, boneCell).WithName("bon_pls_to_bon_cell");
         neighborhood6.AddTag("Cell2Plasma");
         K = _objectBaseFactory.Create<IParameter>().WithName("K").WithFormula(BlackBoxPartitionCoeffFormula(spatialStructure.FormulaCache));
         neighborhood6.MoleculeProperties.Add(K);
         neighborhood6.AddParameter(NewConstantParameter("SA", 22));
         sumProcessRate = _objectBaseFactory.Create<IParameter>().WithName(SumProcessRate)
            .WithFormula(SumFormula(spatialStructure.FormulaCache));
         neighborhood6.MoleculeProperties.Add(sumProcessRate);

         spatialStructure.AddNeighborhood(neighborhood6);

         spatialStructure.ResolveReferencesInNeighborhoods();
         return spatialStructure;
      }

      private IFormula BMI(IFormulaCache formulaCache)
      {
         var formula = _objectBaseFactory.Create<ExplicitFormula>().WithFormulaString("BW * 2").WithName("BMI");
         formula.AddObjectPath(_objectPathFactory.CreateFormulaUsablePathFrom(Constants.ORGANISM, BW).WithAlias("BW"));
         formulaCache.Add(formula);
         return formula;
      }

      private IFormula Fabs(IFormulaCache formulaCache)
      {
         var formula = _objectBaseFactory.Create<ExplicitFormula>()
            .WithFormulaString("SumProcessRateLung + SumProcessRateBone").WithName("FabsFormula");
         formula.AddObjectPath(_objectPathFactory.CreateFormulaUsablePathFrom(Constants.NEIGHBORHOODS, "lng_pls_to_lng_cell", MOLECULE, SumProcessRate).WithAlias("SumProcessRateLung"));
         formula.AddObjectPath(_objectPathFactory.CreateFormulaUsablePathFrom(Constants.NEIGHBORHOODS, "bon_pls_to_bon_cell", MOLECULE, SumProcessRate).WithAlias("SumProcessRateBone"));
         formulaCache.Add(formula);
         return formula;
      }

      private IFormula TableFormula1(IFormulaCache formulaCache)
      {
         var tableFormula = _objectBaseFactory.Create<TableFormula>().WithName("TableFormula1");
         tableFormula.AddPoint(0, -1);
         tableFormula.AddPoint(10, -2);
         tableFormula.AddPoint(100, -3);
         formulaCache.Add(tableFormula);
         return tableFormula;
      }

      private IFormula TableFormula2(IFormulaCache formulaCache)
      {
         var tableFormula = _objectBaseFactory.Create<TableFormula>().WithName("TableFormula2");
         tableFormula.AddPoint(0, 0);
         tableFormula.AddPoint(100, 1);
         formulaCache.Add(tableFormula);
         return tableFormula;
      }

      private IFormula SumFormula(IFormulaCache formulaCache)
      {
         var formula = formulaCache.FirstOrDefault(x => string.Equals(x.Name, "SumFormula"));
         if (formula != null)
            return formula;


         var dynamicFormula = _objectBaseFactory.Create<SumFormula>().WithName("SumFormula");
         dynamicFormula.Criteria = Create.Criteria(x => x.With(Constants.Parameters.PROCESS_RATE).And.With(NEIGHBORHOOD).And.With(MOLECULE));
         formulaCache.Add(dynamicFormula);
         return dynamicFormula;
      }

      private IContainer CreateContainerWithName(string containerName)
      {
         //create container with name and set the default tag==name
         var container = _objectBaseFactory.Create<IContainer>().WithName(containerName);
         container.AddTag(new Tag(containerName));
         return container;
      }

      private IFormula Convection(IFormulaCache formulaCache)
      {
         var formula = formulaCache.FirstOrDefault(x => string.Equals(x.Name, "Convection"));
         if (formula != null)
            return formula;

         formula = _objectBaseFactory.Create<ExplicitFormula>().WithFormulaString("Q*C").WithName("Convection");
         formula.AddObjectPath(_objectPathFactory.CreateFormulaUsablePathFrom(SOURCE, PARENT_CONTAINER, "Q").WithAlias("Q"));
         formula.AddObjectPath(_objectPathFactory.CreateFormulaUsablePathFrom(SOURCE, MOLECULE, "C").WithAlias("C"));

         formulaCache.Add(formula);

         return formula;
      }

      private IFormula Diffusion(IFormulaCache formulaCache)
      {
         var formula = formulaCache.FirstOrDefault(x => string.Equals(x.Name, "Diffusion"));
         if (formula != null)
            return formula;

         formula = _objectBaseFactory.Create<ExplicitFormula>().WithFormulaString("fu*P*SA*(C1-C2/K)*logMA").WithName("Diffusion");
         formula.AddObjectPath(_objectPathFactory.CreateFormulaUsablePathFrom(Constants.ORGANISM, "fu").WithAlias("fu"));
         formula.AddObjectPath(_objectPathFactory.CreateFormulaUsablePathFrom(SOURCE, MOLECULE, "C").WithAlias("C1"));
         formula.AddObjectPath(_objectPathFactory.CreateFormulaUsablePathFrom(TARGET, MOLECULE, "C").WithAlias("C2"));
         formula.AddObjectPath(_objectPathFactory.CreateFormulaUsablePathFrom(NEIGHBORHOOD, MOLECULE, "K").WithAlias("K"));
         formula.AddObjectPath(_objectPathFactory.CreateFormulaUsablePathFrom(MOLECULE, "logMA").WithAlias("logMA"));
         formula.AddObjectPath(_objectPathFactory.CreateFormulaUsablePathFrom(SOURCE, PARENT_CONTAINER, "P").WithAlias("P"));
         formula.AddObjectPath(_objectPathFactory.CreateFormulaUsablePathFrom(NEIGHBORHOOD, "SA").WithAlias("SA"));

         formulaCache.Add(formula);

         return formula;
      }

      private IFormula GlobalMoleculeDepParamFormula_1(IFormulaCache formulaCache)
      {
         var formula = _objectBaseFactory.Create<ExplicitFormula>().WithFormulaString("MW").WithName("GlobalMoleculeDepParamFormula_1");
         formula.AddObjectPath(_objectPathFactory.CreateFormulaUsablePathFrom(MOLECULE, "MW").WithAlias("MW"));

         formulaCache.Add(formula);

         return formula;
      }

      private IFormula BlackBoxPartitionCoeffFormula(IFormulaCache formulaCache)
      {
         var formula = formulaCache.FirstOrDefault(x => string.Equals(x.Name, "BlackBoxPartitionCoeff"));
         if (formula != null)
            return formula;

         formula = _objectBaseFactory.Create<BlackBoxFormula>().WithName("BlackBoxPartitionCoeff");
         formulaCache.Add(formula);
         return formula;
      }

      private IFormula PartitionCoeff_1()
      {
         var formula = _objectBaseFactory.Create<ExplicitFormula>().WithFormulaString("pH1/pH2*logMA*HelpMe").WithName("PartitionCoeff_1");
         formula.AddObjectPath(_objectPathFactory.CreateFormulaUsablePathFrom(MOLECULE, "logMA").WithAlias("logMA"));
         formula.AddObjectPath(_objectPathFactory.CreateFormulaUsablePathFrom(FIRST_NEIGHBOR, "pH").WithAlias("pH1"));
         formula.AddObjectPath(_objectPathFactory.CreateFormulaUsablePathFrom(FIRST_NEIGHBOR, MOLECULE, "HelpMe").WithAlias("HelpMe"));
         formula.AddObjectPath(_objectPathFactory.CreateFormulaUsablePathFrom(SECOND_NEIGHBOR, "pH").WithAlias("pH2"));

         return formula;
      }

      private IFormula PartitionCoeff_2()
      {
         var formula = _objectBaseFactory.Create<ExplicitFormula>().WithFormulaString("pH1/pH2*logMA").WithName("PartitionCoeff_2");
         formula.AddObjectPath(_objectPathFactory.CreateFormulaUsablePathFrom(MOLECULE, "logMA").WithAlias("logMA"));
         formula.AddObjectPath(_objectPathFactory.CreateFormulaUsablePathFrom(FIRST_NEIGHBOR, "pH").WithAlias("pH1"));
         formula.AddObjectPath(_objectPathFactory.CreateFormulaUsablePathFrom(SECOND_NEIGHBOR, "pH").WithAlias("pH2"));

         return formula;
      }

      private IFormula AmountObs(IFormulaCache formulaCache)
      {
         var formula = formulaCache.FirstOrDefault(x => string.Equals(x.Name, "AmountObs"));
         if (formula != null)
            return formula;

         formula = _objectBaseFactory.Create<ExplicitFormula>().WithFormulaString("M/2").WithName("AmountObs");
         formula.AddObjectPath(_objectPathFactory.CreateFormulaUsablePathFrom(PARENT_CONTAINER).WithAlias("M"));
         formula.Dimension = _dimensionFactory.Dimension(Constants.Dimension.MOLAR_AMOUNT);
         formulaCache.Add(formula);
         return formula;
      }

      private IFormula FractionObs(IFormulaCache formulaCache)
      {
         var formula = formulaCache.FirstOrDefault(x => string.Equals(x.Name, "FractionObs"));
         if (formula != null)
            return formula;

         formula = _objectBaseFactory.Create<ExplicitFormula>().WithFormulaString("M/4").WithName("FractionObs");
         formula.AddObjectPath(_objectPathFactory.CreateFormulaUsablePathFrom(PARENT_CONTAINER).WithAlias("M"));
         formula.Dimension = _dimensionFactory.Dimension(Constants.Dimension.FRACTION);
         formulaCache.Add(formula);
         return formula;
      }

      private IFormula ContainerObs(IFormulaCache formulaCache)
      {
         var formula = formulaCache.FirstOrDefault(x => string.Equals(x.Name, "ContainerObs"));
         if (formula != null)
            return formula;

         formula = _objectBaseFactory.Create<ExplicitFormula>().WithFormulaString("M1+M2").WithName("ContainerObs");
         formula.AddObjectPath(_objectPathFactory.CreateFormulaUsablePathFrom(Constants.ORGANISM, Lung, Cell, MOLECULE).WithAlias("M1"));
         formula.AddObjectPath(_objectPathFactory.CreateFormulaUsablePathFrom(Constants.ORGANISM, Bone, Cell, MOLECULE).WithAlias("M2"));
         formula.Dimension = _dimensionFactory.Dimension(Constants.Dimension.MOLAR_AMOUNT);

         formulaCache.Add(formula);
         return formula;
      }

      private IFormula InContainerObs(IFormulaCache formulaCache)
      {
         var formula = formulaCache.FirstOrDefault(x => string.Equals(x.Name, "InContainerObs"));
         if (formula != null)
            return formula;

         formula = _objectBaseFactory.Create<ExplicitFormula>().WithFormulaString("M1+M2").WithName("InContainerObs");
         formula.AddObjectPath(_objectPathFactory.CreateFormulaUsablePathFrom(Constants.ORGANISM, Lung, Cell, MOLECULE).WithAlias("M1"));
         formula.AddObjectPath(_objectPathFactory.CreateFormulaUsablePathFrom(Constants.ORGANISM, Bone, Cell, MOLECULE).WithAlias("M2"));
         formula.Dimension = _dimensionFactory.Dimension(Constants.Dimension.MOLAR_AMOUNT);

         formulaCache.Add(formula);
         return formula;
      }
   }

   public class EntityPathResolverForSpecs : EntityPathResolver
   {
      public EntityPathResolverForSpecs() : base(new ObjectPathFactoryForSpecs())
      {
      }
   }

   public class ObjectPathFactoryForSpecs : ObjectPathFactory
   {
      public ObjectPathFactoryForSpecs()
         : base(new AliasCreator())
      {
      }
   }

   public class ObjectBaseFactoryForSpecs : IObjectBaseFactory
   {
      private readonly IDimensionFactory _dimensionFactory;

      public ObjectBaseFactoryForSpecs(IDimensionFactory dimensionFactory)
      {
         _dimensionFactory = dimensionFactory;
      }

      public T Create<T>() where T : class, IObjectBase
      {
         throw new NotSupportedException();
      }

      public T CreateObjectBaseFrom<T>(Type objectType)
      {
         throw new NotSupportedException();
      }

      public T CreateObjectBaseFrom<T>(Type objectType, string id)
      {
         throw new NotSupportedException();
      }

      public T CreateObjectBaseFrom<T>(T sourceObject)
      {
         string id = Guid.NewGuid().ToString();
         if (sourceObject.IsAnImplementationOf<IDistributedParameter>())
            return new DistributedParameter().WithId(id).WithDimension(_dimensionFactory.NoDimension).DowncastTo<T>();

         if (sourceObject.IsAnImplementationOf<IContainer>())
            return new Container().WithId(id).DowncastTo<T>();

         if (sourceObject.IsAnImplementationOf<IParameter>())
            return new Parameter().WithId(id).WithDimension(_dimensionFactory.NoDimension).DowncastTo<T>();

         if (sourceObject.IsAnImplementationOf<BuildingBlock>())
            return newBuildingBlockWithId(sourceObject as BuildingBlock, id).DowncastTo<T>();

         if (sourceObject.IsAnImplementationOf<ConstantFormula>())
            return new ConstantFormula().WithDimension(_dimensionFactory.NoDimension).WithId(id).DowncastTo<T>();

         if (sourceObject.IsAnImplementationOf<BlackBoxFormula>())
            return new BlackBoxFormula().WithDimension(_dimensionFactory.NoDimension).WithId(id).DowncastTo<T>();

         if (sourceObject.IsAnImplementationOf<ExplicitFormula>())
            return new ExplicitFormula().WithDimension(_dimensionFactory.NoDimension).WithId(id).DowncastTo<T>();

         if (sourceObject.IsAnImplementationOf<NormalDistributionFormula>())
            return new NormalDistributionFormula().WithDimension(_dimensionFactory.NoDimension).WithId(id).DowncastTo<T>();

         if (sourceObject.IsAnImplementationOf<ParameterIdentification>())
            return new ParameterIdentification().WithId(id).DowncastTo<T>();

         if (sourceObject.IsAnImplementationOf<ExpressionParameter>())
            return new ExpressionParameter().WithDimension(_dimensionFactory.NoDimension).WithId(id).DowncastTo<T>();

         if (sourceObject.IsAnImplementationOf<IndividualParameter>())
            return new IndividualParameter().WithDimension(_dimensionFactory.NoDimension).WithId(id).DowncastTo<T>();

         if (sourceObject.IsAnImplementationOf<Module>())
            return new Module().WithId(id).DowncastTo<T>();

         if (sourceObject.IsAnImplementationOf<ModuleConfiguration>())
            return new ModuleConfiguration(new Module()).DowncastTo<T>();

         if (sourceObject.IsAnImplementationOf<SimulationConfiguration>())
            return new SimulationConfiguration().DowncastTo<T>();

         if (sourceObject.IsAnImplementationOf<OutputSelections>())
            return new OutputSelections().DowncastTo<T>();

         if (sourceObject.IsAnImplementationOf<InitialCondition>())
            return new InitialCondition().DowncastTo<T>();

         return default(T);
      }

      private T newBuildingBlockWithId<T>(T sourceObject, string id) where T : BuildingBlock
      {
         BuildingBlock bb = default(T);
         if (sourceObject.IsAnImplementationOf<SpatialStructure>())
            bb = new SpatialStructure();

         if (sourceObject.IsAnImplementationOf<MoleculeBuildingBlock>())
            bb = new MoleculeBuildingBlock();

         if (sourceObject.IsAnImplementationOf<ReactionBuildingBlock>())
            bb = new ReactionBuildingBlock();

         if (sourceObject.IsAnImplementationOf<PassiveTransportBuildingBlock>())
            bb = new PassiveTransportBuildingBlock();

         if (sourceObject.IsAnImplementationOf<ObserverBuildingBlock>())
            bb = new ObserverBuildingBlock();

         if (sourceObject.IsAnImplementationOf<EventGroupBuildingBlock>())
            bb = new EventGroupBuildingBlock();

         if (sourceObject.IsAnImplementationOf<InitialConditionsBuildingBlock>())
            bb = new InitialConditionsBuildingBlock();

         if (sourceObject.IsAnImplementationOf<ParameterValuesBuildingBlock>())
            bb = new ParameterValuesBuildingBlock();

         if (sourceObject.IsAnImplementationOf<SimulationSettings>())
            bb = new SimulationSettings();

         if (bb != null)
            return bb.WithId(id).DowncastTo<T>();

         return null;
      }

      public T Create<T>(string id) where T : class, IObjectBase
      {
         throw new NotSupportedException();
      }

      public IObjectBase GetObjectBase(string id)
      {
         throw new NotSupportedException();
      }

      public IObjectBase Get(string id)
      {
         throw new NotSupportedException();
      }

      public void Unregister(string id)
      {
         throw new NotSupportedException();
      }

      public T Get<T>(string id) where T : IObjectBase
      {
         throw new NotSupportedException();
      }

      public bool ContainsObjectBaseWithId(string id)
      {
         throw new NotSupportedException();
      }

      public void Register(IObjectBase objectBase)
      {
         throw new NotSupportedException();
      }
   }
}