using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DevExpress.Utils.Svg;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Assets
{
   public static class ApplicationIcons
   {
      private static readonly ICache<string, ApplicationIcon> _allIcons = new Cache<string, ApplicationIcon>(icon => icon.IconName);
      private static IList<ApplicationIcon> _allIconsList;

      public static readonly ApplicationIcon Absorption = AddNamedIcon("Absorption");
      public static readonly ApplicationIcon ActiveEfflux = AddNamedIcon("Efflux", "ActiveEfflux");
      public static readonly ApplicationIcon ActiveInflux = AddNamedIcon("Influx", "ActiveInflux");
      public static readonly ApplicationIcon Add = AddNamedIcon("AddAction", "Add");
      public static readonly ApplicationIcon AddEnzyme = AddNamedIcon("Enzyme", "AddEnzyme");
      public static readonly ApplicationIcon AddProtein = AddNamedIcon("Protein", "AddProtein");
      public static readonly ApplicationIcon AddToJournal = AddNamedIcon("AddAction", "AddToJournal");
      public static readonly ApplicationIcon AgingPopulationSimulation = AddNamedIcon("AgingPopulationSimulation");
      public static readonly ApplicationIcon AgingPopulationSimulationGreen = AddNamedIcon("AgingPopulationSimulationGreen");
      public static readonly ApplicationIcon AgingPopulationSimulationRed = AddNamedIcon("AgingPopulationSimulationRed");
      public static readonly ApplicationIcon AgingSimulation = AddNamedIcon("AgingSimulation");
      public static readonly ApplicationIcon AgingSimulationGreen = AddNamedIcon("AgingSimulationGreen");
      public static readonly ApplicationIcon AgingSimulationRed = AddNamedIcon("AgingSimulationRed");
      public static readonly ApplicationIcon Application = AddNamedIcon("Application");
      public static readonly ApplicationIcon Applications = AddNamedIcon("Application", "Applications");
      public static readonly ApplicationIcon ApplicationsError = AddNamedIcon("ApplicationError", "ApplicationsError");
      public static readonly ApplicationIcon ApplicationSettings = AddNamedIcon("Settings", "ApplicationSettings");
      public static readonly ApplicationIcon ApplyAll = AddNamedIcon("ApplyAll");
      public static readonly ApplicationIcon ArterialBlood = AddNamedIcon("ArterialBlood");
      public static readonly ApplicationIcon Back = AddNamedIcon("Back");
      public static readonly ApplicationIcon BasicPharmacochemistry = AddNamedIcon("BasicPharmacochemistry");
      public static readonly ApplicationIcon BasicPharmacochemistryError = AddNamedIcon("BasicPharmacochemistryError");
      public static readonly ApplicationIcon Beagle = AddNamedIcon("Beagle");
      public static readonly ApplicationIcon BeagleGreen = AddNamedIcon("BeagleGreen");
      public static readonly ApplicationIcon BeagleRed = AddNamedIcon("BeagleRed");
      public static readonly ApplicationIcon BiologicalProperties = AddNamedIcon("BiologicalProperties");
      public static readonly ApplicationIcon BiologicalPropertiesError = AddNamedIcon("BiologicalPropertiesError");
      public static readonly ApplicationIcon Blood = AddNamedIcon("Blood");
      public static readonly ApplicationIcon BloodCells = AddNamedIcon("BloodCells");
      public static readonly ApplicationIcon Bone = AddNamedIcon("Bone");
      public static readonly ApplicationIcon BoxWhiskerAnalysis = AddNamedIcon("BoxWhiskerAnalysis");
      public static readonly ApplicationIcon BoxWhiskerAnalysisGreen = AddNamedIcon("BoxWhiskerAnalysisGreen");
      public static readonly ApplicationIcon BoxWhiskerAnalysisRed = AddNamedIcon("BoxWhiskerAnalysisRed");
      public static readonly ApplicationIcon Brain = AddNamedIcon("Brain");
      public static readonly ApplicationIcon BuildingBlockExplorer = AddNamedIcon("BuildingBlockExplorer");
      public static readonly ApplicationIcon Caecum = AddNamedIcon("Caecum");
      public static readonly ApplicationIcon Cancel = AddNamedIcon("Cancel");
      public static readonly ApplicationIcon Cat = AddNamedIcon("Cat");
      public static readonly ApplicationIcon CatGreen = AddNamedIcon("CatGreen");
      public static readonly ApplicationIcon CatRed = AddNamedIcon("CatRed");
      public static readonly ApplicationIcon Cattle = AddNamedIcon("Cattle");
      public static readonly ApplicationIcon CattleGreen = AddNamedIcon("CattleGreen");
      public static readonly ApplicationIcon CattleRed = AddNamedIcon("CattleRed");
      public static readonly ApplicationIcon Cecum = AddNamedIcon("Caecum", "Cecum");
      public static readonly ApplicationIcon CheckAll = AddNamedIcon("CheckAll");
      public static readonly ApplicationIcon CheckSelected = AddNamedIcon("CheckSelected");
      public static readonly ApplicationIcon Clone = AddNamedIcon("SimulationClone", "Clone");
      public static readonly ApplicationIcon Close = AddNamedIcon("ProjectClose", "Close");
      public static readonly ApplicationIcon CloseProject = AddNamedIcon("ProjectClose", "CloseProject");
      public static readonly ApplicationIcon ClusterExport = AddNamedIcon("ClusterExport");
      public static readonly ApplicationIcon ColonAscendens = AddNamedIcon("ColonAscendens");
      public static readonly ApplicationIcon ColonDescendens = AddNamedIcon("ColonDescendens");
      public static readonly ApplicationIcon ColonSigmoid = AddNamedIcon("ColonSigmoid");
      public static readonly ApplicationIcon ColonTransversum = AddNamedIcon("ColonTransversum");
      public static readonly ApplicationIcon Commit = AddNamedIcon("Commit");
      public static readonly ApplicationIcon CommitRed = AddNamedIcon("CommitRed");
      public static readonly ApplicationIcon SimulationComparisonFolder = AddNamedIcon("ComparisonFolder", "SimulationComparisonFolder");
      public static readonly ApplicationIcon CompetitiveInhibition = AddNamedIcon("CompetitiveInhibition");
      public static readonly ApplicationIcon Complex = AddNamedIcon("Complex");
      public static readonly ApplicationIcon Compound = AddNamedIcon("Molecule", "Compound");
      public static readonly ApplicationIcon CompoundError = AddNamedIcon("MoleculeError", "CompoundError");
      public static readonly ApplicationIcon CompoundFolder = AddNamedIcon("MoleculeFolder", "CompoundFolder");
      public static readonly ApplicationIcon CompoundGreen = AddNamedIcon("MoleculeGreen", "CompoundGreen");
      public static readonly ApplicationIcon CompoundRed = AddNamedIcon("MoleculeRed", "CompoundRed");
      public static readonly ApplicationIcon SimulationConfigure = AddNamedIcon("SimulationConfigure");
      public static readonly ApplicationIcon Container = AddNamedIcon("Container");
      public static readonly ApplicationIcon Copy = AddNamedIcon("Copy");
      public static readonly ApplicationIcon Create = AddNamedIcon("AddAction", "Create");
      public static readonly ApplicationIcon Debug = AddNamedIcon("Debug");
      public static readonly ApplicationIcon ConfigureAndRun = AddNamedIcon("ConfigureAndRun");
      public static readonly ApplicationIcon Delete = AddNamedIcon("Delete");
      public static readonly ApplicationIcon Dermal = AddNamedIcon("Dermal");
      public static readonly ApplicationIcon Description = AddNamedIcon("Description");
      public static readonly ApplicationIcon Comparison = AddNamedIcon("Comparison");
      public static readonly ApplicationIcon Distribution = AddNamedIcon("Distribution");
      public static readonly ApplicationIcon DistributionCalculation = AddNamedIcon("DistributionCalculation");
      public static readonly ApplicationIcon Dog = AddNamedIcon("Dog");
      public static readonly ApplicationIcon DogGreen = AddNamedIcon("DogGreen");
      public static readonly ApplicationIcon DogRed = AddNamedIcon("DogRed");
      public static readonly ApplicationIcon Down = AddNamedIcon("Down");
      public static readonly ApplicationIcon Drug = AddNamedIcon("Molecule", "Drug");
      public static readonly ApplicationIcon Duodenum = AddNamedIcon("Duodenum");
      public static readonly ApplicationIcon DxError = AddNamedIcon("ErrorProvider");
      public static readonly ApplicationIcon Edit = AddNamedIcon("Edit");
      public static readonly ApplicationIcon PageEdit = AddNamedIcon("PageEdit");
      public static readonly ApplicationIcon Efflux = AddNamedIcon("Efflux");
      public static readonly ApplicationIcon Endothelium = AddNamedIcon("Endothelium");
      public static readonly ApplicationIcon Enzyme = AddNamedIcon("Enzyme");
      public static readonly ApplicationIcon Error = AddNamedIcon("Error");
      public static readonly ApplicationIcon ErrorHint = AddNamedIcon("ErrorHint");
      public static readonly ApplicationIcon Event = AddNamedIcon("Event", IconNames.EVENT);
      public static readonly ApplicationIcon EventFolder = AddNamedIcon("EventFolder");
      public static readonly ApplicationIcon EventGreen = AddNamedIcon("EventGreen");
      public static readonly ApplicationIcon EventGroup = AddNamedIcon("Event", IconNames.EVENT_GROUP);
      public static readonly ApplicationIcon EventRed = AddNamedIcon("EventRed");
      public static readonly ApplicationIcon Excel = AddNamedIcon("ObservedData", IconNames.EXCEL);
      public static readonly ApplicationIcon Excretion = AddNamedIcon("Excretion");
      public static readonly ApplicationIcon Exit = AddNamedIcon("Exit");
      public static readonly ApplicationIcon ExpertParameters = AddNamedIcon("Parameters", "ExpertParameters");
      public static readonly ApplicationIcon PopulationExportToCSV = AddNamedIcon("PopulationExportToCSV");
      public static readonly ApplicationIcon ExportToPDF = AddNamedIcon("PDF", "ExportToPDF");
      public static readonly ApplicationIcon ExtendParameterValues = AddNamedIcon("ExtendParameterValues");
      public static readonly ApplicationIcon ExtracellularMembrane = AddNamedIcon("ExtracellularMembrane");
      public static readonly ApplicationIcon Fat = AddNamedIcon("Fat");
      public static readonly ApplicationIcon Favorites = AddNamedIcon("Favorites");
      public static readonly ApplicationIcon FitToPage = AddNamedIcon("FitToPage");
      public static readonly ApplicationIcon Folder = AddNamedIcon("Folder");
      public static readonly ApplicationIcon Formulation = AddNamedIcon("Formulation");
      public static readonly ApplicationIcon FormulationFolder = AddNamedIcon("FormulationFolder");
      public static readonly ApplicationIcon FormulationGreen = AddNamedIcon("FormulationGreen");
      public static readonly ApplicationIcon FormulationRed = AddNamedIcon("FormulationRed");
      public static readonly ApplicationIcon Forward = AddNamedIcon("Forward");
      public static readonly ApplicationIcon Gallbladder = AddNamedIcon("GallBladder");
      public static readonly ApplicationIcon GITract = AddNamedIcon("GITract", "GI-Tract");
      public static readonly ApplicationIcon GlomerularFiltration = AddNamedIcon("GlomerularFiltration");
      public static readonly ApplicationIcon Gonads = AddNamedIcon("Gonads");
      public static readonly ApplicationIcon GoTo = AddNamedIcon("GoTo");
      public static readonly ApplicationIcon GroupBy = AddNamedIcon("GroupBy");
      public static readonly ApplicationIcon Heart = AddNamedIcon("Heart");
      public static readonly ApplicationIcon Help = AddNamedIcon("Help");
      public static readonly ApplicationIcon Histogram = AddNamedIcon("Histogram");
      public static readonly ApplicationIcon History = AddNamedIcon("History");
      public static readonly ApplicationIcon Human = AddNamedIcon("Human");
      public static readonly ApplicationIcon HumanGreen = AddNamedIcon("HumanGreen");
      public static readonly ApplicationIcon HumanRed = AddNamedIcon("HumanRed");
      public static readonly ApplicationIcon Import = AddNamedIcon("ObservedData", "Import");
      public static readonly ApplicationIcon InitialConditionsImport = AddNamedIcon("InitialConditionsImport");
      public static readonly ApplicationIcon ParameterValuesImport = AddNamedIcon("ParameterValuesImport");
      public static readonly ApplicationIcon ImportPopulation = AddNamedIcon("ImportPopulation");
      public static readonly ApplicationIcon PopulationSimulationLoad = AddNamedIcon("PopulationSimulationLoad");
      public static readonly ApplicationIcon ResultsImportFromCSV = AddNamedIcon("ResultsImportFromCSV", IconNames.RESULTS_IMPORT_FROM_CSV);
      public static readonly ApplicationIcon IndividualSimulationLoad = AddNamedIcon("IndividualSimulationLoad");
      public static readonly ApplicationIcon Individual = AddNamedIcon("Individual");
      public static readonly ApplicationIcon IndividualError = AddNamedIcon("IndividualError");
      public static readonly ApplicationIcon IndividualFolder = AddNamedIcon("IndividualFolder");
      public static readonly ApplicationIcon IndividualGreen = AddNamedIcon("IndividualGreen");
      public static readonly ApplicationIcon IndividualRed = AddNamedIcon("IndividualRed");
      public static readonly ApplicationIcon ScaleIndividual = AddNamedIcon("ScaleIndividual");
      public static readonly ApplicationIcon Induction = AddNamedIcon("Induction");
      public static readonly ApplicationIcon Influx = AddNamedIcon("Influx");
      public static readonly ApplicationIcon Info = AddNamedIcon("About", "Info");
      public static readonly ApplicationIcon Inhibition = AddNamedIcon("Inhibition");
      public static readonly ApplicationIcon Interstitial = AddNamedIcon("Interstitial");
      public static readonly ApplicationIcon Intracellular = AddNamedIcon("Intracellular");
      public static readonly ApplicationIcon Intravenous = AddNamedIcon("Intravenous");
      public static readonly ApplicationIcon IntravenousBolus = AddNamedIcon("IntravenousBolus");
      public static readonly ApplicationIcon IrreversibleInhibition = AddNamedIcon("IrreversibleInhibition");
      public static readonly ApplicationIcon Journal = AddNamedIcon("Journal");
      public static readonly ApplicationIcon Page = AddNamedIcon("Page");
      public static readonly ApplicationIcon JournalDiagram = AddNamedIcon("JournalDiagram");
      public static readonly ApplicationIcon Kidney = AddNamedIcon("Kidney");
      public static readonly ApplicationIcon LabelAdd = AddNamedIcon("LabelAdd");
      public static readonly ApplicationIcon LargeIntestine = AddNamedIcon("LargeIntestine");
      public static readonly ApplicationIcon License = AddNamedIcon("LicenseRegister", "License");
      public static readonly ApplicationIcon Liver = AddNamedIcon("Liver");
      public static readonly ApplicationIcon Load = AddNamedIcon("LoadAction", "Load");
      public static readonly ApplicationIcon ContainerLoad = AddNamedIcon("ContainerLoad");
      public static readonly ApplicationIcon EventLoad = AddNamedIcon("EventLoad");
      public static readonly ApplicationIcon FavoritesLoad = AddNamedIcon("FavoritesLoad");
      public static readonly ApplicationIcon LoadFromTemplate = AddNamedIcon("LoadAction", "LoadFromTemplate");
      public static readonly ApplicationIcon MoleculeLoad = AddNamedIcon("MoleculeLoad");
      public static readonly ApplicationIcon ObserverLoad = AddNamedIcon("ObserverLoad");
      public static readonly ApplicationIcon ReactionLoad = AddNamedIcon("ReactionLoad");
      public static readonly ApplicationIcon SpatialStructureLoad = AddNamedIcon("SpatialStructureLoad");
      public static readonly ApplicationIcon LowerIleum = AddNamedIcon("LowerIleum");
      public static readonly ApplicationIcon LowerJejunum = AddNamedIcon("LowerJejunum");
      public static readonly ApplicationIcon Lumen = AddNamedIcon("Lumen");
      public static readonly ApplicationIcon LumenCaecum = AddNamedIcon("Caecum", "Lumen-Caecum");
      public static readonly ApplicationIcon LumenColonAscendens = AddNamedIcon("ColonAscendens", "Lumen-ColonAscendens");
      public static readonly ApplicationIcon LumenColonDescendens = AddNamedIcon("ColonDescendens", "Lumen-ColonDescendens");
      public static readonly ApplicationIcon LumenColonSigmoid = AddNamedIcon("ColonSigmoid", "Lumen-ColonSigmoid");
      public static readonly ApplicationIcon LumenColonTransversum = AddNamedIcon("ColonTransversum", "Lumen-ColonTransversum");
      public static readonly ApplicationIcon LumenDuodenum = AddNamedIcon("Duodenum", "Lumen-Duodenum");
      public static readonly ApplicationIcon LumenLowerIleum = AddNamedIcon("LowerIleum", "Lumen-LowerIleum");
      public static readonly ApplicationIcon LumenLowerJejunum = AddNamedIcon("LowerJejunum", "Lumen-LowerJejunum");
      public static readonly ApplicationIcon LumenRectum = AddNamedIcon("Rectum", "Lumen-Rectum");
      public static readonly ApplicationIcon LumenStomach = AddNamedIcon("Stomach", "Lumen-Stomach");
      public static readonly ApplicationIcon LumenUpperIleum = AddNamedIcon("UpperIleum", "Lumen-UpperIleum");
      public static readonly ApplicationIcon LumenUpperJejunum = AddNamedIcon("UpperJejunum", "Lumen-UpperJejunum");
      public static readonly ApplicationIcon Lung = AddNamedIcon("Lung");
      public static readonly ApplicationIcon ProjectDisplayUnitsConfigure = AddNamedIcon("ProjectDisplayUnitsConfigure");
      public static readonly ApplicationIcon UserDisplayUnitsConfigure = AddNamedIcon("UserDisplayUnitsConfigure");
      public static readonly ApplicationIcon Matlab = AddNamedIcon("Matlab", IconNames.MATLAB);
      public static readonly ApplicationIcon MetaData = AddNamedIcon("MetaData");
      public static readonly ApplicationIcon MetaDataAndUnitInformation = AddNamedIcon("MetaData", "MetaDataAndUnitInformation");
      public static readonly ApplicationIcon Merge = AddNamedIcon("Merge");
      public static readonly ApplicationIcon MergePopulation = AddNamedIcon("MergePopulation");
      public static readonly ApplicationIcon Metabolism = AddNamedIcon("Metabolism");
      public static readonly ApplicationIcon Metabolite = AddNamedIcon("Metabolite");
      public static readonly ApplicationIcon Minipig = AddNamedIcon("Minipig");
      public static readonly ApplicationIcon MinipigGreen = AddNamedIcon("MinipigGreen");
      public static readonly ApplicationIcon MinipigRed = AddNamedIcon("MinipigRed");
      public static readonly ApplicationIcon MissingData = AddNamedIcon("MissingData");
      public static readonly ApplicationIcon MissingMetaData = AddNamedIcon("MissingMetaData");
      public static readonly ApplicationIcon MissingUnitInformation = AddNamedIcon("MissingUnitInformation");
      public static readonly ApplicationIcon MixedInhibition = AddNamedIcon("MixedInhibition");
      public static readonly ApplicationIcon MoBi = AddNamedIcon("MoBi");
      public static readonly ApplicationIcon MoBiSimulation = AddNamedIcon("PKML", "MoBiSimulation");
      public static readonly ApplicationIcon ModelStructure = AddNamedIcon("ModelStructure");
      public static readonly ApplicationIcon ModelStructureError = AddNamedIcon("ModelStructureError");
      public static readonly ApplicationIcon Molecule = AddNamedIcon("Molecule", IconNames.MOLECULE);
      public static readonly ApplicationIcon MoleculeFolder = AddNamedIcon("MoleculeFolder");
      public static readonly ApplicationIcon MoleculeGreen = AddNamedIcon("MoleculeGreen");
      public static readonly ApplicationIcon MoleculeRed = AddNamedIcon("MoleculeRed");
      public static readonly ApplicationIcon InitialConditions = AddNamedIcon("InitialConditions", IconNames.INITIAL_CONDITIONS);
      public static readonly ApplicationIcon InitialConditionsFolder = AddNamedIcon("InitialConditionsFolder");
      public static readonly ApplicationIcon InitialConditionsGreen = AddNamedIcon("InitialConditionsGreen");
      public static readonly ApplicationIcon InitialConditionsRed = AddNamedIcon("InitialConditionsRed");
      public static readonly ApplicationIcon Monkey = AddNamedIcon("Monkey");
      public static readonly ApplicationIcon MonkeyGreen = AddNamedIcon("MonkeyGreen");
      public static readonly ApplicationIcon MonkeyRed = AddNamedIcon("MonkeyRed");
      public static readonly ApplicationIcon Mouse = AddNamedIcon("Mouse");
      public static readonly ApplicationIcon MouseGreen = AddNamedIcon("MouseGreen");
      public static readonly ApplicationIcon MouseRed = AddNamedIcon("MouseRed");
      public static readonly ApplicationIcon Muscle = AddNamedIcon("Muscle");
      public static readonly ApplicationIcon AmountProjectNew = AddNamedIcon("ProjectNewAmount", "AmountProjectNew");
      public static readonly ApplicationIcon ConcentrationProjectNew = AddNamedIcon("ProjectNewConcentration", "ConcentrationProjectNew");
      public static readonly ApplicationIcon ContainerAdd = AddNamedIcon("ContainerAdd");
      public static readonly ApplicationIcon EventAdd = AddNamedIcon("EventAdd");
      public static readonly ApplicationIcon MoleculeAdd = AddNamedIcon("MoleculeAdd");
      public static readonly ApplicationIcon PKSimMoleculeAdd = AddNamedIcon("MoleculeAdd", "PKSimMoleculeAdd");
      public static readonly ApplicationIcon ProjectNew = AddNamedIcon("ProjectNew");
      public static readonly ApplicationIcon ReactionAdd = AddNamedIcon("ReactionAdd");
      public static readonly ApplicationIcon SpatialStructureAdd = AddNamedIcon("SpatialStructureAdd");
      public static readonly ApplicationIcon Next = AddNamedIcon("Next");
      public static readonly ApplicationIcon NonCompetitiveInhibition = AddNamedIcon("NonCompetitiveInhibition");
      public static readonly ApplicationIcon ObservedData = AddNamedIcon("ObservedData", IconNames.OBSERVED_DATA);
      public static readonly ApplicationIcon ObservedDataCompound = AddNamedIcon("ObservedDataForMolecule", "ObservedDataCompound");
      public static readonly ApplicationIcon ObservedDataFolder = AddNamedIcon("ObservedDataFolder");
      public static readonly ApplicationIcon Observer = AddNamedIcon("Observer", IconNames.OBSERVER);
      public static readonly ApplicationIcon ObserverFolder = AddNamedIcon("ObserverFolder");
      public static readonly ApplicationIcon ObserverGreen = AddNamedIcon("ObserverGreen");
      public static readonly ApplicationIcon ObserverRed = AddNamedIcon("ObserverRed");
      public static readonly ApplicationIcon OK = AddNamedIcon("OK");
      public static readonly ApplicationIcon ProjectOpen = AddNamedIcon("ProjectOpen");
      public static readonly ApplicationIcon Oral = AddNamedIcon("Oral");
      public static readonly ApplicationIcon Organism = AddNamedIcon("Organism");
      public static readonly ApplicationIcon Other = AddNamedIcon("Help", IconNames.OTHER);
      public static readonly ApplicationIcon OtherProtein = AddNamedIcon("Protein", "OtherProtein");
      public static readonly ApplicationIcon OutputInterval = AddNamedIcon("OutputInterval");
      public static readonly ApplicationIcon Pancreas = AddNamedIcon("Pancreas");
      public static readonly ApplicationIcon Parameter = AddNamedIcon("Parameters", IconNames.PARAMETER);
      public static readonly ApplicationIcon ParameterDistribution = AddNamedIcon("Histogram", "ParameterDistribution");
      public static readonly ApplicationIcon Parameters = AddNamedIcon("Parameters");
      public static readonly ApplicationIcon ParametersError = AddNamedIcon("ParametersError");
      public static readonly ApplicationIcon ParameterValueGreen = AddNamedIcon("ParameterValuesGreen");
      public static readonly ApplicationIcon ParameterValues = AddNamedIcon("ParameterValues", IconNames.PARAMETER_VALUES);
      public static readonly ApplicationIcon ParameterValuesFolder = AddNamedIcon("ParameterValuesFolder");
      public static readonly ApplicationIcon ParameterValuesRed = AddNamedIcon("ParameterValuesRed");
      public static readonly ApplicationIcon PassiveTransport = AddNamedIcon("PassiveTransport", IconNames.PASSIVE_TRANSPORT);
      public static readonly ApplicationIcon PassiveTransportFolder = AddNamedIcon("PassiveTransportFolder");
      public static readonly ApplicationIcon PassiveTransportGreen = AddNamedIcon("PassiveTransportGreen");
      public static readonly ApplicationIcon PassiveTransportRed = AddNamedIcon("PassiveTransportRed");
      public static readonly ApplicationIcon Paste = AddNamedIcon("Paste");
      public static readonly ApplicationIcon PDF = AddNamedIcon("PDF");
      public static readonly ApplicationIcon Pericentral = AddNamedIcon("Pericentral");
      public static readonly ApplicationIcon Periportal = AddNamedIcon("Periportal");
      public static readonly ApplicationIcon Permeability = AddNamedIcon("Permeability");
      public static readonly ApplicationIcon Pgp = AddNamedIcon("Pgp");
      public static readonly ApplicationIcon PKAnalysis = AddNamedIcon("PKAnalysis");
      public static readonly ApplicationIcon PKML = AddNamedIcon("PKML");
      public static readonly ApplicationIcon PKSim = AddNamedIcon("PKSim");
      public static readonly ApplicationIcon Plasma = AddNamedIcon("Plasma");
      public static readonly ApplicationIcon Population = AddNamedIcon("Population");
      public static readonly ApplicationIcon PopulationError = AddNamedIcon("PopulationError");
      public static readonly ApplicationIcon PopulationFolder = AddNamedIcon("PopulationFolder");
      public static readonly ApplicationIcon PopulationGreen = AddNamedIcon("PopulationGreen");
      public static readonly ApplicationIcon PopulationRed = AddNamedIcon("PopulationRed");
      public static readonly ApplicationIcon PopulationSimulation = AddNamedIcon("PopulationSimulation");
      public static readonly ApplicationIcon PopulationSimulationGreen = AddNamedIcon("PopulationSimulationGreen");
      public static readonly ApplicationIcon PopulationSimulationRed = AddNamedIcon("PopulationSimulationRed");
      public static readonly ApplicationIcon PopulationSimulationSettings = AddNamedIcon("SimulationSettings", "PopulationSimulationSettings");
      public static readonly ApplicationIcon PortalVein = AddNamedIcon("PortalVein");
      public static readonly ApplicationIcon Previous = AddNamedIcon("Previous");
      public static readonly ApplicationIcon ProjectDescription = AddNamedIcon("Description", "ProjectDescription");
      public static readonly ApplicationIcon Protein = AddNamedIcon("Protein");
      public static readonly ApplicationIcon ProteinExpression = AddNamedIcon("ProteinExpression");
      public static readonly ApplicationIcon ProteinExpressionError = AddNamedIcon("ProteinExpressionError");
      public static readonly ApplicationIcon Protocol = AddNamedIcon("Protocol");
      public static readonly ApplicationIcon ProtocolFolder = AddNamedIcon("ProtocolFolder");
      public static readonly ApplicationIcon ProtocolGreen = AddNamedIcon("ProtocolGreen");
      public static readonly ApplicationIcon ProtocolRed = AddNamedIcon("ProtocolRed");
      public static readonly ApplicationIcon R = AddNamedIcon("R");
      public static readonly ApplicationIcon Rabbit = AddNamedIcon("Rabbit");
      public static readonly ApplicationIcon RabbitGreen = AddNamedIcon("RabbitGreen");
      public static readonly ApplicationIcon RabbitRed = AddNamedIcon("RabbitRed");
      public static readonly ApplicationIcon RangeAnalysis = AddNamedIcon("RangeAnalysis");
      public static readonly ApplicationIcon RangeAnalysisGreen = AddNamedIcon("RangeAnalysisGreen");
      public static readonly ApplicationIcon RangeAnalysisRed = AddNamedIcon("RangeAnalysisRed");
      public static readonly ApplicationIcon Rat = AddNamedIcon("Rat");
      public static readonly ApplicationIcon RatGreen = AddNamedIcon("RatGreen");
      public static readonly ApplicationIcon RatRed = AddNamedIcon("RatRed");
      public static readonly ApplicationIcon Reaction = AddNamedIcon("Reaction", IconNames.REACTION);
      public static readonly ApplicationIcon ReactionFolder = AddNamedIcon("ReactionFolder");
      public static readonly ApplicationIcon ReactionGreen = AddNamedIcon("ReactionGreen");
      public static readonly ApplicationIcon ReactionRed = AddNamedIcon("ReactionRed");
      public static readonly ApplicationIcon Rectum = AddNamedIcon("Rectum");
      public static readonly ApplicationIcon Refresh = AddNamedIcon("Refresh");
      public static readonly ApplicationIcon RefreshAll = AddNamedIcon("RefreshAll");
      public static readonly ApplicationIcon RefreshSelected = AddNamedIcon("RefreshSelected");
      public static readonly ApplicationIcon Remove = AddNamedIcon("Delete", "Remove");
      public static readonly ApplicationIcon Rename = AddNamedIcon("Rename");
      public static readonly ApplicationIcon Reset = AddNamedIcon("Refresh", "Reset");
      public static readonly ApplicationIcon Run = AddNamedIcon("Run");
      public static readonly ApplicationIcon Saliva = AddNamedIcon("Saliva");
      public static readonly ApplicationIcon Save = AddNamedIcon("Save");
      public static readonly ApplicationIcon SaveAs = AddNamedIcon("SaveAs");
      public static readonly ApplicationIcon SaveAsTemplate = AddNamedIcon("SaveAction", "SaveAsTemplate");
      public static readonly ApplicationIcon FavoritesSave = AddNamedIcon("FavoritesSave");
      public static readonly ApplicationIcon ScaleFactor = AddNamedIcon("ScaleFactor");
      public static readonly ApplicationIcon ScatterAnalysis = AddNamedIcon("ScatterAnalysis");
      public static readonly ApplicationIcon ScatterAnalysisGreen = AddNamedIcon("ScatterAnalysisGreen");
      public static readonly ApplicationIcon ScatterAnalysisRed = AddNamedIcon("ScatterAnalysisRed");
      public static readonly ApplicationIcon Search = AddNamedIcon("Search");
      public static readonly ApplicationIcon Settings = AddNamedIcon("Settings");
      public static readonly ApplicationIcon Simulation = AddNamedIcon("Simulation", IconNames.SIMULATION);
      public static readonly ApplicationIcon SimulationExplorer = AddNamedIcon("SimulationExplorer");
      public static readonly ApplicationIcon SimulationFolder = AddNamedIcon("SimulationFolder");
      public static readonly ApplicationIcon SimulationGreen = AddNamedIcon("SimulationGreen");
      public static readonly ApplicationIcon SimulationRed = AddNamedIcon("SimulationRed");
      public static readonly ApplicationIcon SimulationSettings = AddNamedIcon("SimulationSettings", IconNames.SIMULATION_SETTINGS);
      public static readonly ApplicationIcon SimulationSettingsFolder = AddNamedIcon("SimulationSettingsFolder");
      public static readonly ApplicationIcon SimulationSettingsGreen = AddNamedIcon("SimulationSettingsGreen");
      public static readonly ApplicationIcon SimulationSettingsRed = AddNamedIcon("SimulationSettingsRed");
      public static readonly ApplicationIcon Skin = AddNamedIcon("Skin");
      public static readonly ApplicationIcon SmallIntestine = AddNamedIcon("SmallIntestine");
      public static readonly ApplicationIcon Solver = AddNamedIcon("Solver");
      public static readonly ApplicationIcon SpatialStructure = AddNamedIcon("SpatialStructure", IconNames.SPATIAL_STRUCTURE);
      public static readonly ApplicationIcon SpatialStructureFolder = AddNamedIcon("SpatialStructureFolder");
      public static readonly ApplicationIcon SpatialStructureGreen = AddNamedIcon("SpatialStructureGreen");
      public static readonly ApplicationIcon SpatialStructureRed = AddNamedIcon("SpatialStructureRed");
      public static readonly ApplicationIcon SpecificBinding = AddNamedIcon("SpecificBinding");
      public static readonly ApplicationIcon Spleen = AddNamedIcon("Spleen");
      public static readonly ApplicationIcon Stomach = AddNamedIcon("Stomach");
      public static readonly ApplicationIcon Stop = AddNamedIcon("Stop");
      public static readonly ApplicationIcon Subcutaneous = AddNamedIcon("Subcutaneous");
      public static readonly ApplicationIcon SimulationComparison = AddNamedIcon("IndividualSimulationComparison", "SimulationComparison");
      public static readonly ApplicationIcon SystemSettings = AddNamedIcon("Population", "SystemSettings");
      public static readonly ApplicationIcon Time = AddNamedIcon("Time");
      public static readonly ApplicationIcon TimeProfileAnalysis = AddNamedIcon("TimeProfileAnalysis");
      public static readonly ApplicationIcon TimeProfileAnalysisGreen = AddNamedIcon("TimeProfileAnalysisGreen");
      public static readonly ApplicationIcon TimeProfileAnalysisRed = AddNamedIcon("TimeProfileAnalysisRed");
      public static readonly ApplicationIcon Transport = AddNamedIcon("Influx", "Transport");
      public static readonly ApplicationIcon Transporter = AddNamedIcon("Transporter");
      public static readonly ApplicationIcon TubularSecretion = AddNamedIcon("TubularSecretion");
      public static readonly ApplicationIcon UncheckAll = AddNamedIcon("UncheckAll");
      public static readonly ApplicationIcon UncheckSelected = AddNamedIcon("UncheckSelected");
      public static readonly ApplicationIcon UncompetitiveInhibition = AddNamedIcon("UncompetitiveInhibition");
      public static readonly ApplicationIcon Undo = AddNamedIcon("Undo");
      public static readonly ApplicationIcon UnitInformation = AddNamedIcon("UnitInformation");
      public static readonly ApplicationIcon Up = AddNamedIcon("Up");
      public static readonly ApplicationIcon Update = AddNamedIcon("Update");
      public static readonly ApplicationIcon UpperIleum = AddNamedIcon("UpperIleum");
      public static readonly ApplicationIcon UpperJejunum = AddNamedIcon("UpperJejunum");
      public static readonly ApplicationIcon UserDefined = AddNamedIcon("Individual", "UserDefined");
      public static readonly ApplicationIcon UserSettings = AddNamedIcon("Settings", "UserSettings");
      public static readonly ApplicationIcon VascularEndothelium = AddNamedIcon("Endothelium", "VascularEndothelium");
      public static readonly ApplicationIcon VenousBlood = AddNamedIcon("VenousBlood");
      public static readonly ApplicationIcon Warning = AddNamedIcon("Notifications", "Warning");
      public static readonly ApplicationIcon ZoomIn = AddNamedIcon("ZoomIn");
      public static readonly ApplicationIcon ZoomOut = AddNamedIcon("ZoomOut");
      public static readonly ApplicationIcon CopySelection = AddNamedIcon("CopySelection");
      public static readonly ApplicationIcon JournalExportToWord = AddNamedIcon("JournalExportToWord", IconNames.JOURNAL_EXPORT_TO_WORD);
      public static readonly ApplicationIcon SBML = AddNamedIcon("SBML");
      public static readonly ApplicationIcon PageAdd = AddNamedIcon("PageAdd");
      public static readonly ApplicationIcon IndividualSimulationComparison = AddNamedIcon("IndividualSimulationComparison");
      public static readonly ApplicationIcon PopulationSimulationComparison = AddNamedIcon("PopulationSimulationComparison");
      public static readonly ApplicationIcon JournalSelect = AddNamedIcon("JournalSelect");
      public static readonly ApplicationIcon HistoryExport = AddNamedIcon("HistoryExport");
      public static readonly ApplicationIcon SimulationClone = AddNamedIcon("SimulationClone");
      public static readonly ApplicationIcon AnalysesLoad = AddNamedIcon("AnalysesLoad");
      public static readonly ApplicationIcon AnalysesSave = AddNamedIcon("AnalysesSave");
      public static readonly ApplicationIcon ObserverAdd = AddNamedIcon("ObserverAdd");
      public static readonly ApplicationIcon Report = AddNamedIcon("Report", IconNames.REPORT);
      public static readonly ApplicationIcon ExportToCSV = AddNamedIcon("ExportToCSV");
      public static readonly ApplicationIcon ExportToExcel = AddNamedIcon("ExportToExcel");
      public static readonly ApplicationIcon PKAnalysesImportFromCSV = AddNamedIcon("PKAnalysesImportFromCSV");
      public static readonly ApplicationIcon Notifications = AddNamedIcon("Notifications");
      public static readonly ApplicationIcon SimulationLoad = AddNamedIcon("SimulationLoad");
      public static readonly ApplicationIcon PKMLLoad = AddNamedIcon("PKMLLoad");
      public static readonly ApplicationIcon PKMLSave = AddNamedIcon("PKMLSave");
      public static readonly ApplicationIcon Administration = AddNamedIcon("Administration");
      public static readonly ApplicationIcon ComparisonFolder = AddNamedIcon("ComparisonFolder");
      public static readonly ApplicationIcon About = AddNamedIcon("About");
      public static readonly ApplicationIcon ContainerSave = AddNamedIcon("ContainerSave");
      public static readonly ApplicationIcon EventSave = AddNamedIcon("EventSave");
      public static readonly ApplicationIcon AddFormulation = AddNamedIcon("FormulationAdd");
      public static readonly ApplicationIcon FormulationLoad = AddNamedIcon("FormulationLoad");
      public static readonly ApplicationIcon SaveFormulation = AddNamedIcon("FormulationSave");
      public static readonly ApplicationIcon MoleculeError = AddNamedIcon("MoleculeError");
      public static readonly ApplicationIcon SaveMolecule = AddNamedIcon("MoleculeSave");
      public static readonly ApplicationIcon AddInitialConditions = AddNamedIcon("InitialConditionsAdd");
      public static readonly ApplicationIcon InitialConditionsLoad = AddNamedIcon("InitialConditionsLoad");
      public static readonly ApplicationIcon SaveInitialConditions = AddNamedIcon("InitialConditionsSave");
      public static readonly ApplicationIcon SaveObserver = AddNamedIcon("ObserverSave");
      public static readonly ApplicationIcon AddParameterValues = AddNamedIcon("ParameterValuesAdd");
      public static readonly ApplicationIcon ParameterValuesLoad = AddNamedIcon("ParameterValuesLoad");
      public static readonly ApplicationIcon SaveParameterValues = AddNamedIcon("ParameterValuesSave");
      public static readonly ApplicationIcon SaveReaction = AddNamedIcon("ReactionSave");
      public static readonly ApplicationIcon SaveSpatialStructure = AddNamedIcon("SpatialStructureSave");
      public static readonly ApplicationIcon ParameterIdentificationFolder = AddNamedIcon("ParameterIdentificationFolder");
      public static readonly ApplicationIcon SensitivityAnalysisFolder = AddNamedIcon("SensitivityAnalysisFolder");
      public static readonly ApplicationIcon SensitivityAnalysis = AddNamedIcon("SensitivityAnalysis");
      public static readonly ApplicationIcon ParameterIdentification = AddNamedIcon("ParameterIdentification");
      public static readonly ApplicationIcon ResidualHistogramAnalysis = AddNamedIcon("ResidualHistogramAnalysis");
      public static readonly ApplicationIcon ResidualHistogramAnalysisGreen = AddNamedIcon("ResidualHistogramAnalysisGreen");
      public static readonly ApplicationIcon ResidualHistogramAnalysisRed = AddNamedIcon("ResidualHistogramAnalysisRed");
      public static readonly ApplicationIcon ResidualVsTimeAnalysis = AddNamedIcon("ResidualVsTimeAnalysis");
      public static readonly ApplicationIcon ResidualVsTimeAnalysisGreen = AddNamedIcon("ResidualVsTimeAnalysisGreen");
      public static readonly ApplicationIcon ResidualVsTimeAnalysisRed = AddNamedIcon("ResidualVsTimeAnalysisRed");
      public static readonly ApplicationIcon PredictedVsObservedAnalysis = AddNamedIcon("PredictedVsObservedAnalysis");
      public static readonly ApplicationIcon PredictedVsObservedAnalysisGreen = AddNamedIcon("PredictedVsObservedAnalysisGreen");
      public static readonly ApplicationIcon PredictedVsObservedAnalysisRed = AddNamedIcon("PredictedVsObservedAnalysisRed");
      public static readonly ApplicationIcon DeleteFolderOnly = AddNamedIcon("DeleteFolderOnly");
      public static readonly ApplicationIcon CorrelationAnalysis = AddNamedIcon("CorrelationAnalysis");
      public static readonly ApplicationIcon CorrelationAnalysisGreen = AddNamedIcon("CorrelationAnalysisGreen");
      public static readonly ApplicationIcon CorrelationAnalysisRed = AddNamedIcon("CorrelationAnalysisRed");
      public static readonly ApplicationIcon CovarianceAnalysis = AddNamedIcon("CovarianceAnalysis");
      public static readonly ApplicationIcon CovarianceAnalysisGreen = AddNamedIcon("CovarianceAnalysisGreen");
      public static readonly ApplicationIcon CovarianceAnalysisRed = AddNamedIcon("CovarianceAnalysisRed");
      public static readonly ApplicationIcon DeleteSelected = AddNamedIcon("DeleteSelected");
      public static readonly ApplicationIcon DeleteSourceNotDefined = AddNamedIcon("DeleteSourceNotDefined");
      public static readonly ApplicationIcon ExtendInitialConditions = AddNamedIcon("ExtendInitialConditions");
      public static readonly ApplicationIcon MoleculeObserver = AddNamedIcon("MoleculeObserver");
      public static readonly ApplicationIcon OutputSelection = AddNamedIcon("OutputSelection");
      public static readonly ApplicationIcon PreviewOriginData = AddNamedIcon("PreviewOriginData");
      public static readonly ApplicationIcon Tree = AddNamedIcon("Tree");
      public static readonly ApplicationIcon Diagram = AddNamedIcon("Diagram");
      public static readonly ApplicationIcon ContainerObserver = AddNamedIcon("ContainerObserver");
      public static readonly ApplicationIcon ImportAll = AddNamedIcon("ImportAll");
      public static readonly ApplicationIcon ReactionList = AddNamedIcon("ReactionList");
      public static readonly ApplicationIcon SolverSettings = AddNamedIcon("SolverSettings");
      public static readonly ApplicationIcon Swap = AddNamedIcon("Swap");
      public static readonly ApplicationIcon ImportAction = AddNamedIcon("ImportAction");
      public static readonly ApplicationIcon FractionData = AddNamedIcon("FractionData");
      public static readonly ApplicationIcon ObservedDataForMolecule = AddNamedIcon("ObservedDataForMolecule");
      public static readonly ApplicationIcon ParameterIdentificationVisualFeedback = AddNamedIcon("ParameterIdentificationVisualFeedback");
      public static readonly ApplicationIcon Results = AddNamedIcon("Results");
      public static readonly ApplicationIcon Formula = AddNamedIcon("Formula");
      public static readonly ApplicationIcon UserDefinedVariability = AddNamedIcon("UserDefinedVariability");
      public static readonly ApplicationIcon TimeProfileConfidenceInterval = AddNamedIcon("TimeProfileConfidenceInterval");
      public static readonly ApplicationIcon TimeProfilePredictionInterval = AddNamedIcon("TimeProfilePredictionInterval");
      public static readonly ApplicationIcon TimeProfileVPCInterval = AddNamedIcon("TimeProfileVPCInterval");
      public static readonly ApplicationIcon PKAnalysesExportToCSV = AddNamedIcon("PKAnalysesExportToCSV");
      public static readonly ApplicationIcon PKParameterSensitivityAnalysis = AddNamedIcon("PKParameterSensitivityAnalysis");
      public static readonly ApplicationIcon SensitivityAnalysisVisualFeedback = AddNamedIcon("SensitivityAnalysisVisualFeedback");
      public static readonly ApplicationIcon ValueOriginMethodAssumption = AddNamedIcon("ValueOriginMethodAssumption");
      public static readonly ApplicationIcon ValueOriginMethodInVitro = AddNamedIcon("ValueOriginMethodInVitro");
      public static readonly ApplicationIcon ValueOriginMethodInVivo = AddNamedIcon("ValueOriginMethodInVivo");
      public static readonly ApplicationIcon ValueOriginMethodOther = AddNamedIcon("ValueOriginMethodOther");
      public static readonly ApplicationIcon ValueOriginMethodManualFit = AddNamedIcon("ValueOriginMethodManualFit");
      public static readonly ApplicationIcon ValueOriginMethodParameterIdentification = AddNamedIcon("ValueOriginMethodParameterIdentification");
      public static readonly ApplicationIcon ValueOriginMethodUnknown = AddNamedIcon("ValueOriginMethodUnknown");
      public static readonly ApplicationIcon ValueOriginSourceDatabase = AddNamedIcon("ValueOriginSourceDatabase");
      public static readonly ApplicationIcon ValueOriginSourceInternet = AddNamedIcon("ValueOriginSourceInternet");
      public static readonly ApplicationIcon ValueOriginSourceParameterIdentification = AddNamedIcon("ValueOriginSourceParameterIdentification");
      public static readonly ApplicationIcon ValueOriginSourcePublication = AddNamedIcon("ValueOriginSourcePublication");
      public static readonly ApplicationIcon ValueOriginSourceUnknown = AddNamedIcon("ValueOriginSourceUnknown");
      public static readonly ApplicationIcon ValueOriginSourceOther = AddNamedIcon("ValueOriginSourceOther");
      public static readonly ApplicationIcon Tag = AddNamedIcon("Tag");
      public static readonly ApplicationIcon UserDefinedSpecies = AddNamedIcon("UserDefinedSpecies");
      public static readonly ApplicationIcon Properties = AddNamedIcon("Properties");
      public static readonly ApplicationIcon Snapshot = AddNamedIcon("Snapshot");
      public static readonly ApplicationIcon SnapshotExport = AddNamedIcon("SnapshotExport");
      public static readonly ApplicationIcon SnapshotImport = AddNamedIcon("SnapshotImport");
      public static readonly ApplicationIcon File = AddNamedIcon("ProjectNew", "File");
      public static readonly ApplicationIcon Redo = AddNamedIcon("Redo");
      public static readonly ApplicationIcon ClearHistory = AddNamedIcon("Delete", "ClearHistory");
      public static readonly ApplicationIcon AmountObservedData = AddNamedIcon("AmountObservedData");
      public static readonly ApplicationIcon AmountObservedDataForMolecule = AddNamedIcon("AmountObservedDataForMolecule");
      public static readonly ApplicationIcon BiDirectional = AddNamedIcon("BiDirectional");
      public static readonly ApplicationIcon Breasts = AddNamedIcon("Breasts");
      public static readonly ApplicationIcon Endometrium = AddNamedIcon("Endometrium");
      public static readonly ApplicationIcon Endosome = AddNamedIcon("Endosome");
      public static readonly ApplicationIcon Fetus = AddNamedIcon("Fetus");
      public static readonly ApplicationIcon Placenta = AddNamedIcon("Placenta");
      public static readonly ApplicationIcon Myometrium = AddNamedIcon("Endometrium", "Myometrium");
      public static readonly ApplicationIcon ExpressionProfile = AddNamedIcon("ProteinExpression", "ExpressionProfile");
      public static readonly ApplicationIcon ExpressionProfileFolder = AddNamedIcon("ExpressionProfileFolder");
      public static readonly ApplicationIcon OSPSuite = AddNamedIcon("OSPSuite");
      public static readonly ApplicationIcon RedCross = AddNamedIcon("RedCross");
      public static readonly ApplicationIcon ModuleExplorer = AddNamedIcon("ModuleExplorer");
      public static readonly ApplicationIcon ModulesFolder = AddNamedIcon("ModulesFolder");
      public static readonly ApplicationIcon Module = AddNamedIcon("Module");
      public static readonly ApplicationIcon Neighborhood = AddNamedIcon( "Neighborhood");
      public static readonly ApplicationIcon Neighbor = AddNamedIcon("Neighbor");
      public static readonly ApplicationIcon TransporterRed = AddNamedIcon("TransporterRed", "TransporterRed");
      public static readonly ApplicationIcon TransporterGreen = AddNamedIcon("TransporterGreen", "TransporterGreen");
      public static readonly ApplicationIcon EnzymeRed = AddNamedIcon("EnzymeRed", "EnzymeRed");
      public static readonly ApplicationIcon EnzymeGreen = AddNamedIcon("EnzymeGreen", "EnzymeGreen");
      public static readonly ApplicationIcon ProteinRed = AddNamedIcon("ProteinRed", "ProteinRed");
      public static readonly ApplicationIcon ProteinGreen = AddNamedIcon("ProteinGreen", "ProteinGreen");
      public static readonly ApplicationIcon ModuleGreen = AddNamedIcon("ModuleGreen");
      public static readonly ApplicationIcon ModuleRed = AddNamedIcon("ModuleRed");
      public static readonly ApplicationIcon PKSimModule = AddNamedIcon("PKSimModule");
      public static readonly ApplicationIcon PKSimModuleGreen = AddNamedIcon("PKSimModuleGreen");
      public static readonly ApplicationIcon PKSimModuleRed = AddNamedIcon("PKSimModuleRed");


      // All icons should go at the end of the preceding list, before this delimiting icon - EmptyIcon
      private static ApplicationIcon createEmptyIcon() => new ApplicationIcon((SvgImage) null);

      public static readonly ApplicationIcon EmptyIcon = createEmptyIcon();

      //This icon should be set in the application
      public static ApplicationIcon DefaultIcon = EmptyIcon;

      private static ApplicationIcon iconByNameInternal(string iconName)
      {
         return _allIcons[iconName.ToUpperInvariant()];
      }

      public static bool HasIconNamed(string iconName)
      {
         if (string.IsNullOrEmpty(iconName))
            return false;

         return _allIcons.Contains(iconName.ToUpperInvariant());
      }

      public static ApplicationIcon IconByNameOrDefault(string iconName, ApplicationIcon defaultIcon)
      {
         var defaultIconToUse = defaultIcon ?? EmptyIcon;
         if (string.IsNullOrEmpty(iconName))
            return defaultIconToUse;

         return HasIconNamed(iconName)
            ? iconByNameInternal(iconName)
            : defaultIconToUse;
      }

      public static ApplicationIcon AddNamedIcon(string resName, string iconName = null)
      {
         var name = (iconName ?? resName).ToUpperInvariant();
         var iconAsBytes = getIcon(resName);
         
         var appIcon = new ApplicationIcon(iconAsBytes)
         {
            IconName = name,
            Index = _allIcons.Count
         };

         _allIcons.Add(appIcon);
         return appIcon;
      }

      private static byte[] getIcon(string iconName)
      {
         var assembly = Assembly.GetExecutingAssembly();
         var resourceName = typeof(ApplicationIcon).Namespace + ".Icons." + iconName + ".svg";
         
         using (var stream = assembly.GetManifestResourceStream(resourceName))
         {
            if (stream == null)
               return null;

            var buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            return buffer;
         }
      }

      public static IEnumerable<ApplicationIcon> All()
      {
         return _allIcons.All();
      }

      public static int IconIndex(ApplicationIcon icon)
      {
         if (_allIconsList == null)
            _allIconsList = _allIcons.ToList();

         return _allIconsList.IndexOf(icon);
      }

      public static int IconIndex(string iconName)
      {
         return IconIndex(IconByName(iconName));
      }

      public static ApplicationIcon IconByName(string iconName)
      {
         return IconByNameOrDefault(iconName, EmptyIcon);
      }

      public static ApplicationIcon IconFor(IWithIcon withIcon)
      {
         return IconByNameOrDefault(withIcon?.IconName, EmptyIcon);
      }

      private static ApplicationIcon dynamicIconFor(string template, string entity, ApplicationIcon defaultIcon)
      {
         var iconName = string.Format(template, entity);
         return IconByNameOrDefault(iconName, defaultIcon);
      }

      private static ApplicationIcon overlayFor(ApplicationIcon applicationIcon, string type)
      {
         if (applicationIcon == null)
            return EmptyIcon;

         var iconName = $"{applicationIcon.IconName}{type}";
         return IconByNameOrDefault(iconName, applicationIcon);
      }

      public static ApplicationIcon SaveIconFor(string entityType)
      {
         return dynamicIconFor("{0}Save", entityType, PKMLSave);
      }

      public static ApplicationIcon LoadIconFor(string entityType)
      {
         return dynamicIconFor("{0}Load", entityType, PKMLLoad);
      }

      public static ApplicationIcon AddIconFor(string entityType)
      {
         return dynamicIconFor("{0}Add", entityType, Add);
      }

      public static ApplicationIcon LoadTemplateIconFor(string entityType)
      {
         return dynamicIconFor("Template{0}Load", entityType, LoadFromTemplate);
      }

      public static ApplicationIcon GreenOverlayFor(ApplicationIcon applicationIcon)
      {
         return overlayFor(applicationIcon, "Green");
      }

      public static ApplicationIcon ErrorOverlayFor(ApplicationIcon applicationIcon)
      {
         return overlayFor(applicationIcon, "Error");
      }

      public static ApplicationIcon RedOverlayFor(string iconName)
      {
         return RedOverlayFor(IconByName(iconName));
      }

      public static ApplicationIcon GreenOverlayFor(string iconName)
      {
         return GreenOverlayFor(IconByName(iconName));
      }

      public static ApplicationIcon RedOverlayFor(ApplicationIcon applicationIcon)
      {
         return overlayFor(applicationIcon, "Red");
      }
   }
}