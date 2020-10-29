using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Assets
{
   public static class ApplicationIcons
   {
      private static readonly ICache<string, ApplicationIcon> _allIcons = new Cache<string, ApplicationIcon>(icon => icon.IconName);
      private static IList<ApplicationIcon> _allIconsList;

      public static readonly ApplicationIcon Absorption = AddNamedIcon("Absorption", "Absorption");
      public static readonly ApplicationIcon ActiveEfflux = AddNamedIcon("Efflux", "ActiveEfflux");
      public static readonly ApplicationIcon ActiveInflux = AddNamedIcon("Influx", "ActiveInflux");
      public static readonly ApplicationIcon Add = AddNamedIcon("AddAction", "Add");
      public static readonly ApplicationIcon AddEnzyme = AddNamedIcon("Enzyme", "AddEnzyme");
      public static readonly ApplicationIcon AddProtein = AddNamedIcon("Protein", "AddProtein");
      public static readonly ApplicationIcon AddToJournal = AddNamedIcon("AddAction", "AddToJournal");
      public static readonly ApplicationIcon AgingPopulationSimulation = AddNamedIcon("AgingPopulationSimulation", "AgingPopulationSimulation");
      public static readonly ApplicationIcon AgingPopulationSimulationGreen = AddNamedIcon("AgingPopulationSimulationGreen", "AgingPopulationSimulationGreen");
      public static readonly ApplicationIcon AgingPopulationSimulationRed = AddNamedIcon("AgingPopulationSimulationRed", "AgingPopulationSimulationRed");
      public static readonly ApplicationIcon AgingSimulation = AddNamedIcon("AgingSimulation", "AgingSimulation");
      public static readonly ApplicationIcon AgingSimulationGreen = AddNamedIcon("AgingSimulationGreen", "AgingSimulationGreen");
      public static readonly ApplicationIcon AgingSimulationRed = AddNamedIcon("AgingSimulationRed", "AgingSimulationRed");
      public static readonly ApplicationIcon Application = AddNamedIcon("Application", "Application");
      public static readonly ApplicationIcon Applications = AddNamedIcon("Application", "Applications");
      public static readonly ApplicationIcon ApplicationsError = AddNamedIcon("ApplicationError", "ApplicationsError");
      public static readonly ApplicationIcon ApplicationSettings = AddNamedIcon("Settings", "ApplicationSettings");
      public static readonly ApplicationIcon ApplyAll = AddNamedIcon("ApplyAll", "ApplyAll");
      public static readonly ApplicationIcon ArterialBlood = AddNamedIcon("ArterialBlood", "ArterialBlood");
      public static readonly ApplicationIcon Back = AddNamedIcon("Back", "Back");
      public static readonly ApplicationIcon BasicPharmacochemistry = AddNamedIcon("BasicPharmacochemistry", "BasicPharmacochemistry");
      public static readonly ApplicationIcon BasicPharmacochemistryError = AddNamedIcon("BasicPharmacochemistryError", "BasicPharmacochemistryError");
      public static readonly ApplicationIcon Beagle = AddNamedIcon("Beagle", "Beagle");
      public static readonly ApplicationIcon BeagleGreen = AddNamedIcon("BeagleGreen", "BeagleGreen");
      public static readonly ApplicationIcon BeagleRed = AddNamedIcon("BeagleRed", "BeagleRed");
      public static readonly ApplicationIcon BiologicalProperties = AddNamedIcon("BiologicalProperties", "BiologicalProperties");
      public static readonly ApplicationIcon BiologicalPropertiesError = AddNamedIcon("BiologicalPropertiesError", "BiologicalPropertiesError");
      public static readonly ApplicationIcon Blood = AddNamedIcon("Blood", "Blood");
      public static readonly ApplicationIcon BloodCells = AddNamedIcon("BloodCells", "BloodCells");
      public static readonly ApplicationIcon Bone = AddNamedIcon("Bone", "Bone");
      public static readonly ApplicationIcon BoxWhiskerAnalysis = AddNamedIcon("BoxWhiskerAnalysis", "BoxWhiskerAnalysis");
      public static readonly ApplicationIcon BoxWhiskerAnalysisGreen = AddNamedIcon("BoxWhiskerAnalysisGreen", "BoxWhiskerAnalysisGreen");
      public static readonly ApplicationIcon BoxWhiskerAnalysisRed = AddNamedIcon("BoxWhiskerAnalysisRed", "BoxWhiskerAnalysisRed");
      public static readonly ApplicationIcon Brain = AddNamedIcon("Brain", "Brain");
      public static readonly ApplicationIcon BuildingBlockExplorer = AddNamedIcon("BuildingBlockExplorer", "BuildingBlockExplorer");
      public static readonly ApplicationIcon Caecum = AddNamedIcon("Caecum", "Caecum");
      public static readonly ApplicationIcon Cancel = AddNamedIcon("Cancel", "Cancel");
      public static readonly ApplicationIcon Cat = AddNamedIcon("Cat", "Cat");
      public static readonly ApplicationIcon CatGreen = AddNamedIcon("CatGreen", "CatGreen");
      public static readonly ApplicationIcon CatRed = AddNamedIcon("CatRed", "CatRed");
      public static readonly ApplicationIcon Cattle = AddNamedIcon("Cattle", "Cattle");
      public static readonly ApplicationIcon CattleGreen = AddNamedIcon("CattleGreen", "CattleGreen");
      public static readonly ApplicationIcon CattleRed = AddNamedIcon("CattleRed", "CattleRed");
      public static readonly ApplicationIcon Cecum = AddNamedIcon("Caecum", "Cecum");
      public static readonly ApplicationIcon CheckAll = AddNamedIcon("CheckAll", "CheckAll");
      public static readonly ApplicationIcon CheckSelected = AddNamedIcon("CheckSelected", "CheckSelected");
      public static readonly ApplicationIcon Clone = AddNamedIcon("SimulationClone", "Clone");
      public static readonly ApplicationIcon Close = AddNamedIcon("ProjectClose", "Close");
      public static readonly ApplicationIcon CloseProject = AddNamedIcon("ProjectClose", "CloseProject");
      public static readonly ApplicationIcon ClusterExport = AddNamedIcon("ClusterExport", "ClusterExport");
      public static readonly ApplicationIcon ColonAscendens = AddNamedIcon("ColonAscendens", "ColonAscendens");
      public static readonly ApplicationIcon ColonDescendens = AddNamedIcon("ColonDescendens", "ColonDescendens");
      public static readonly ApplicationIcon ColonSigmoid = AddNamedIcon("ColonSigmoid", "ColonSigmoid");
      public static readonly ApplicationIcon ColonTransversum = AddNamedIcon("ColonTransversum", "ColonTransversum");
      public static readonly ApplicationIcon Commit = AddNamedIcon("Commit", "Commit");
      public static readonly ApplicationIcon CommitRed = AddNamedIcon("CommitRed", "CommitRed");
      public static readonly ApplicationIcon SimulationComparisonFolder = AddNamedIcon("ComparisonFolder", "SimulationComparisonFolder");
      public static readonly ApplicationIcon CompetitiveInhibition = AddNamedIcon("CompetitiveInhibition", "CompetitiveInhibition");
      public static readonly ApplicationIcon Complex = AddNamedIcon("Complex", "Complex");
      public static readonly ApplicationIcon Compound = AddNamedIcon("Molecule", "Compound");
      public static readonly ApplicationIcon CompoundError = AddNamedIcon("MoleculeError", "CompoundError");
      public static readonly ApplicationIcon CompoundFolder = AddNamedIcon("MoleculeFolder", "CompoundFolder");
      public static readonly ApplicationIcon CompoundGreen = AddNamedIcon("MoleculeGreen", "CompoundGreen");
      public static readonly ApplicationIcon CompoundRed = AddNamedIcon("MoleculeRed", "CompoundRed");
      public static readonly ApplicationIcon SimulationConfigure = AddNamedIcon("SimulationConfigure", "SimulationConfigure");
      public static readonly ApplicationIcon Container = AddNamedIcon("Container", "Container");
      public static readonly ApplicationIcon Copy = AddNamedIcon("Copy", "Copy");
      public static readonly ApplicationIcon Create = AddNamedIcon("AddAction", "Create");
      public static readonly ApplicationIcon Debug = AddNamedIcon("Debug", "Debug");
      public static readonly ApplicationIcon ConfigureAndRun = AddNamedIcon("ConfigureAndRun", "ConfigureAndRun");
      public static readonly ApplicationIcon Delete = AddNamedIcon("Delete", "Delete");
      public static readonly ApplicationIcon Dermal = AddNamedIcon("Dermal", "Dermal");
      public static readonly ApplicationIcon Description = AddNamedIcon("Description", "Description");
      public static readonly ApplicationIcon Comparison = AddNamedIcon("Comparison", "Comparison");
      public static readonly ApplicationIcon Distribution = AddNamedIcon("Distribution", "Distribution");
      public static readonly ApplicationIcon DistributionCalculation = AddNamedIcon("DistributionCalculation", "DistributionCalculation");
      public static readonly ApplicationIcon Dog = AddNamedIcon("Dog", "Dog");
      public static readonly ApplicationIcon DogGreen = AddNamedIcon("DogGreen", "DogGreen");
      public static readonly ApplicationIcon DogRed = AddNamedIcon("DogRed", "DogRed");
      public static readonly ApplicationIcon Down = AddNamedIcon("Down", "Down");
      public static readonly ApplicationIcon Drug = AddNamedIcon("Molecule", "Drug");
      public static readonly ApplicationIcon Duodenum = AddNamedIcon("Duodenum", "Duodenum");
      public static readonly ApplicationIcon DxError = AddNamedIcon("ErrorProvider", "ErrorProvider");
      public static readonly ApplicationIcon Edit = AddNamedIcon("Edit", "Edit");
      public static readonly ApplicationIcon PageEdit = AddNamedIcon("PageEdit", "PageEdit");
      public static readonly ApplicationIcon Efflux = AddNamedIcon("Efflux", "Efflux");
      public static readonly ApplicationIcon Endothelium = AddNamedIcon("Endothelium", "Endothelium");
      public static readonly ApplicationIcon Enzyme = AddNamedIcon("Enzyme", "Enzyme");
      public static readonly ApplicationIcon Error = AddNamedIcon("Error", "Error");
      public static readonly ApplicationIcon ErrorHint = AddNamedIcon("ErrorHint", "ErrorHint");
      public static readonly ApplicationIcon Event = AddNamedIcon("Event", IconNames.EVENT);
      public static readonly ApplicationIcon EventFolder = AddNamedIcon("EventFolder", "EventFolder");
      public static readonly ApplicationIcon EventGreen = AddNamedIcon("EventGreen", "EventGreen");
      public static readonly ApplicationIcon EventGroup = AddNamedIcon("Event", IconNames.EVENT_GROUP);
      public static readonly ApplicationIcon EventRed = AddNamedIcon("EventRed", "EventRed");
      public static readonly ApplicationIcon Excel = AddNamedIcon("ObservedData", IconNames.EXCEL);
      public static readonly ApplicationIcon Excretion = AddNamedIcon("Excretion", "Excretion");
      public static readonly ApplicationIcon Exit = AddNamedIcon("Exit", "Exit");
      public static readonly ApplicationIcon ExpertParameters = AddNamedIcon("Parameters", "ExpertParameters");
      public static readonly ApplicationIcon PopulationExportToCSV = AddNamedIcon("PopulationExportToCSV", "PopulationExportToCSV");
      public static readonly ApplicationIcon ExportToPDF = AddNamedIcon("PDF", "ExportToPDF");
      public static readonly ApplicationIcon ExtendParameterStartValues = AddNamedIcon("ExtendParameterStartValues", "ExtendParameterStartValues");
      public static readonly ApplicationIcon ExtracellularMembrane = AddNamedIcon("ExtracellularMembrane", "ExtracellularMembrane");
      public static readonly ApplicationIcon Fat = AddNamedIcon("Fat", "Fat");
      public static readonly ApplicationIcon Favorites = AddNamedIcon("Favorites", "Favorites");
      public static readonly ApplicationIcon FitToPage = AddNamedIcon("FitToPage", "FitToPage");
      public static readonly ApplicationIcon Folder = AddNamedIcon("Folder", "Folder");
      public static readonly ApplicationIcon Formulation = AddNamedIcon("Formulation", "Formulation");
      public static readonly ApplicationIcon FormulationFolder = AddNamedIcon("FormulationFolder", "FormulationFolder");
      public static readonly ApplicationIcon FormulationGreen = AddNamedIcon("FormulationGreen", "FormulationGreen");
      public static readonly ApplicationIcon FormulationRed = AddNamedIcon("FormulationRed", "FormulationRed");
      public static readonly ApplicationIcon Forward = AddNamedIcon("Forward", "Forward");
      public static readonly ApplicationIcon Gallbladder = AddNamedIcon("GallBladder", "Gallbladder");
      public static readonly ApplicationIcon GITract = AddNamedIcon("GITract", "GI-Tract");
      public static readonly ApplicationIcon GlomerularFiltration = AddNamedIcon("GlomerularFiltration", "GlomerularFiltration");
      public static readonly ApplicationIcon Gonads = AddNamedIcon("Gonads", "Gonads");
      public static readonly ApplicationIcon GoTo = AddNamedIcon("GoTo", "GoTo");
      public static readonly ApplicationIcon GroupBy = AddNamedIcon("GroupBy", "GroupBy");
      public static readonly ApplicationIcon Heart = AddNamedIcon("Heart", "Heart");
      public static readonly ApplicationIcon Help = AddNamedIcon("Help", "Help");
      public static readonly ApplicationIcon Histogram = AddNamedIcon("Histogram", "Histogram");
      public static readonly ApplicationIcon History = AddNamedIcon("History", "History");
      public static readonly ApplicationIcon Human = AddNamedIcon("Human", "Human");
      public static readonly ApplicationIcon HumanGreen = AddNamedIcon("HumanGreen", "HumanGreen");
      public static readonly ApplicationIcon HumanRed = AddNamedIcon("HumanRed", "HumanRed");
      public static readonly ApplicationIcon Import = AddNamedIcon("ObservedData", "LoadCurrentSheet");
      public static readonly ApplicationIcon MoleculeStartValuesImport = AddNamedIcon("MoleculeStartValuesImport", "MoleculeStartValuesImport");
      public static readonly ApplicationIcon ParameterStartValuesImport = AddNamedIcon("ParameterStartValuesImport", "ParameterStartValuesImport");
      public static readonly ApplicationIcon ImportPopulation = AddNamedIcon("ImportPopulation", "ImportPopulation");
      public static readonly ApplicationIcon PopulationSimulationLoad = AddNamedIcon("PopulationSimulationLoad", "PopulationSimulationLoad");
      public static readonly ApplicationIcon ResultsImportFromCSV = AddNamedIcon("ResultsImportFromCSV", IconNames.RESULTS_IMPORT_FROM_CSV);
      public static readonly ApplicationIcon IndividualSimulationLoad = AddNamedIcon("IndividualSimulationLoad", "IndividualSimulationLoad");
      public static readonly ApplicationIcon Individual = AddNamedIcon("Individual", "Individual");
      public static readonly ApplicationIcon IndividualError = AddNamedIcon("IndividualError", "IndividualError");
      public static readonly ApplicationIcon IndividualFolder = AddNamedIcon("IndividualFolder", "IndividualFolder");
      public static readonly ApplicationIcon IndividualGreen = AddNamedIcon("IndividualGreen", "IndividualGreen");
      public static readonly ApplicationIcon IndividualRed = AddNamedIcon("IndividualRed", "IndividualRed");
      public static readonly ApplicationIcon ScaleIndividual = AddNamedIcon("ScaleIndividual", "ScaleIndividual");
      public static readonly ApplicationIcon Induction = AddNamedIcon("Induction", "Induction");
      public static readonly ApplicationIcon Influx = AddNamedIcon("Influx", "Influx");
      public static readonly ApplicationIcon Info = AddNamedIcon("About", "Info");
      public static readonly ApplicationIcon Inhibition = AddNamedIcon("Inhibition", "Inhibition");
      public static readonly ApplicationIcon Interstitial = AddNamedIcon("Interstitial", "Interstitial");
      public static readonly ApplicationIcon Intracellular = AddNamedIcon("Intracellular", "Intracellular");
      public static readonly ApplicationIcon Intravenous = AddNamedIcon("Intravenous", "Intravenous");
      public static readonly ApplicationIcon IntravenousBolus = AddNamedIcon("IntravenousBolus", "IntravenousBolus");
      public static readonly ApplicationIcon IrreversibleInhibition = AddNamedIcon("IrreversibleInhibition", "IrreversibleInhibition");
      public static readonly ApplicationIcon Journal = AddNamedIcon("Journal", "Journal");
      public static readonly ApplicationIcon Page = AddNamedIcon("Page", "Page");
      public static readonly ApplicationIcon JournalDiagram = AddNamedIcon("JournalDiagram", "JournalDiagram");
      public static readonly ApplicationIcon Kidney = AddNamedIcon("Kidney", "Kidney");
      public static readonly ApplicationIcon LabelAdd = AddNamedIcon("LabelAdd", "LabelAdd");
      public static readonly ApplicationIcon LargeIntestine = AddNamedIcon("LargeIntestine", "LargeIntestine");
      public static readonly ApplicationIcon License = AddNamedIcon("LicenseRegister", "License");
      public static readonly ApplicationIcon Liver = AddNamedIcon("Liver", "Liver");
      public static readonly ApplicationIcon Load = AddNamedIcon("LoadAction", "Load");
      public static readonly ApplicationIcon ContainerLoad = AddNamedIcon("ContainerLoad", "ContainerLoad");
      public static readonly ApplicationIcon EventLoad = AddNamedIcon("EventLoad", "EventLoad");
      public static readonly ApplicationIcon FavoritesLoad = AddNamedIcon("FavoritesLoad", "FavoritesLoad");
      public static readonly ApplicationIcon LoadFromTemplate = AddNamedIcon("LoadAction", "LoadFromTemplate");
      public static readonly ApplicationIcon MoleculeLoad = AddNamedIcon("MoleculeLoad", "MoleculeLoad");
      public static readonly ApplicationIcon ObserverLoad = AddNamedIcon("ObserverLoad", "ObserverLoad");
      public static readonly ApplicationIcon ReactionLoad = AddNamedIcon("ReactionLoad", "ReactionLoad");
      public static readonly ApplicationIcon SpatialStructureLoad = AddNamedIcon("SpatialStructureLoad", "SpatialStructureLoad");
      public static readonly ApplicationIcon LowerIleum = AddNamedIcon("LowerIleum", "LowerIleum");
      public static readonly ApplicationIcon LowerJejunum = AddNamedIcon("LowerJejunum", "LowerJejunum");
      public static readonly ApplicationIcon Lumen = AddNamedIcon("Lumen", "Lumen");
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
      public static readonly ApplicationIcon Lung = AddNamedIcon("Lung", "Lung");
      public static readonly ApplicationIcon ProjectDisplayUnitsConfigure = AddNamedIcon("ProjectDisplayUnitsConfigure", "ProjectDisplayUnitsConfigure");
      public static readonly ApplicationIcon UserDisplayUnitsConfigure = AddNamedIcon("UserDisplayUnitsConfigure", "UserDisplayUnitsConfigure");
      public static readonly ApplicationIcon Matlab = AddNamedIcon("Matlab", IconNames.MATLAB);
      public static readonly ApplicationIcon MetaData = AddNamedIcon("MetaData", "MetaData");
      public static readonly ApplicationIcon MetaDataAndUnitInformation = AddNamedIcon("MetaData", "MetaDataAndUnitInformation");
      public static readonly ApplicationIcon Merge = AddNamedIcon("Merge", "Merge");
      public static readonly ApplicationIcon MergePopulation = AddNamedIcon("MergePopulation", "MergePopulation");
      public static readonly ApplicationIcon Metabolism = AddNamedIcon("Metabolism", "Metabolism");
      public static readonly ApplicationIcon Metabolite = AddNamedIcon("Metabolite", "Metabolite");
      public static readonly ApplicationIcon Minipig = AddNamedIcon("Minipig", "Minipig");
      public static readonly ApplicationIcon MinipigGreen = AddNamedIcon("MinipigGreen", "MinipigGreen");
      public static readonly ApplicationIcon MinipigRed = AddNamedIcon("MinipigRed", "MinipigRed");
      public static readonly ApplicationIcon MissingData = AddNamedIcon("MissingData", "MissingData");
      public static readonly ApplicationIcon MissingMetaData = AddNamedIcon("MissingMetaData", "MissingMetaData");
      public static readonly ApplicationIcon MissingUnitInformation = AddNamedIcon("MissingUnitInformation", "MissingUnitInformation");
      public static readonly ApplicationIcon MixedInhibition = AddNamedIcon("MixedInhibition", "MixedInhibition");
      public static readonly ApplicationIcon MoBi = AddNamedIcon("MoBi", "MoBi");
      public static readonly ApplicationIcon MoBiSimulation = AddNamedIcon("PKML", "MoBiSimulation");
      public static readonly ApplicationIcon ModelStructure = AddNamedIcon("ModelStructure", "ModelStructure");
      public static readonly ApplicationIcon ModelStructureError = AddNamedIcon("ModelStructureError", "ModelStructureError");
      public static readonly ApplicationIcon Molecule = AddNamedIcon("Molecule", IconNames.MOLECULE);
      public static readonly ApplicationIcon MoleculeFolder = AddNamedIcon("MoleculeFolder", "MoleculeFolder");
      public static readonly ApplicationIcon MoleculeGreen = AddNamedIcon("MoleculeGreen", "MoleculeGreen");
      public static readonly ApplicationIcon MoleculeRed = AddNamedIcon("MoleculeRed", "MoleculeRed");
      public static readonly ApplicationIcon MoleculeStartValues = AddNamedIcon("MoleculeStartValues", IconNames.MOLECULE_START_VALUES);
      public static readonly ApplicationIcon MoleculeStartValuesFolder = AddNamedIcon("MoleculeStartValuesFolder", "MoleculeStartValuesFolder");
      public static readonly ApplicationIcon MoleculeStartValuesGreen = AddNamedIcon("MoleculeStartValuesGreen", "MoleculeStartValuesGreen");
      public static readonly ApplicationIcon MoleculeStartValuesRed = AddNamedIcon("MoleculeStartValuesRed", "MoleculeStartValuesRed");
      public static readonly ApplicationIcon Monkey = AddNamedIcon("Monkey", "Monkey");
      public static readonly ApplicationIcon MonkeyGreen = AddNamedIcon("MonkeyGreen", "MonkeyGreen");
      public static readonly ApplicationIcon MonkeyRed = AddNamedIcon("MonkeyRed", "MonkeyRed");
      public static readonly ApplicationIcon Mouse = AddNamedIcon("Mouse", "Mouse");
      public static readonly ApplicationIcon MouseGreen = AddNamedIcon("MouseGreen", "MouseGreen");
      public static readonly ApplicationIcon MouseRed = AddNamedIcon("MouseRed", "MouseRed");
      public static readonly ApplicationIcon Muscle = AddNamedIcon("Muscle", "Muscle");
      public static readonly ApplicationIcon AmountProjectNew = AddNamedIcon("ProjectNewAmount", "AmountProjectNew");
      public static readonly ApplicationIcon ConcentrationProjectNew = AddNamedIcon("ProjectNewConcentration", "ConcentrationProjectNew");
      public static readonly ApplicationIcon ContainerAdd = AddNamedIcon("ContainerAdd", "ContainerAdd");
      public static readonly ApplicationIcon EventAdd = AddNamedIcon("EventAdd", "EventAdd");
      public static readonly ApplicationIcon MoleculeAdd = AddNamedIcon("MoleculeAdd", "MoleculeAdd");
      public static readonly ApplicationIcon PKSimMoleculeAdd = AddNamedIcon("MoleculeAdd", "PKSimMoleculeAdd");
      public static readonly ApplicationIcon ProjectNew = AddNamedIcon("ProjectNew", "ProjectNew");
      public static readonly ApplicationIcon ReactionAdd = AddNamedIcon("ReactionAdd", "ReactionAdd");
      public static readonly ApplicationIcon SpatialStructureAdd = AddNamedIcon("SpatialStructureAdd", "SpatialStructureAdd");
      public static readonly ApplicationIcon Next = AddNamedIcon("Next", "Next");
      public static readonly ApplicationIcon NonCompetitiveInhibition = AddNamedIcon("NonCompetitiveInhibition", "NonCompetitiveInhibition");
      public static readonly ApplicationIcon ObservedData = AddNamedIcon("ObservedData", IconNames.OBSERVED_DATA);
      public static readonly ApplicationIcon ObservedDataCompound = AddNamedIcon("ObservedDataForMolecule", "ObservedDataCompound");
      public static readonly ApplicationIcon ObservedDataFolder = AddNamedIcon("ObservedDataFolder", "ObservedDataFolder");
      public static readonly ApplicationIcon Observer = AddNamedIcon("Observer", IconNames.OBSERVER);
      public static readonly ApplicationIcon ObserverFolder = AddNamedIcon("ObserverFolder", "ObserverFolder");
      public static readonly ApplicationIcon ObserverGreen = AddNamedIcon("ObserverGreen", "ObserverGreen");
      public static readonly ApplicationIcon ObserverRed = AddNamedIcon("ObserverRed", "ObserverRed");
      public static readonly ApplicationIcon OK = AddNamedIcon("OK", "OK");
      public static readonly ApplicationIcon ProjectOpen = AddNamedIcon("ProjectOpen", "ProjectOpen");
      public static readonly ApplicationIcon Oral = AddNamedIcon("Oral", "Oral");
      public static readonly ApplicationIcon Organism = AddNamedIcon("Organism", "Organism");
      public static readonly ApplicationIcon Other = AddNamedIcon("Help", IconNames.OTHER);
      public static readonly ApplicationIcon OtherProtein = AddNamedIcon("Protein", "OtherProtein");
      public static readonly ApplicationIcon OutputInterval = AddNamedIcon("OutputInterval", "OutputInterval");
      public static readonly ApplicationIcon Pancreas = AddNamedIcon("Pancreas", "Pancreas");
      public static readonly ApplicationIcon Parameter = AddNamedIcon("Parameters", IconNames.PARAMETER);
      public static readonly ApplicationIcon ParameterDistribution = AddNamedIcon("Histogram", "ParameterDistribution");
      public static readonly ApplicationIcon Parameters = AddNamedIcon("Parameters", "Parameters");
      public static readonly ApplicationIcon ParametersError = AddNamedIcon("ParametersError", "ParametersError");
      public static readonly ApplicationIcon ParameterStartValueGreen = AddNamedIcon("ParameterStartValuesGreen", "ParameterStartValuesGreen");
      public static readonly ApplicationIcon ParameterStartValues = AddNamedIcon("ParameterStartValues", IconNames.PARAMETER_START_VALUES);
      public static readonly ApplicationIcon ParameterStartValuesFolder = AddNamedIcon("ParameterStartValuesFolder", "ParameterStartValuesFolder");
      public static readonly ApplicationIcon ParameterStartValuesRed = AddNamedIcon("ParameterStartValuesRed", "ParameterStartValuesRed");
      public static readonly ApplicationIcon PassiveTransport = AddNamedIcon("PassiveTransport", IconNames.PASSIVE_TRANSPORT);
      public static readonly ApplicationIcon PassiveTransportFolder = AddNamedIcon("PassiveTransportFolder", "PassiveTransportFolder");
      public static readonly ApplicationIcon PassiveTransportGreen = AddNamedIcon("PassiveTransportGreen", "PassiveTransportGreen");
      public static readonly ApplicationIcon PassiveTransportRed = AddNamedIcon("PassiveTransportRed", "PassiveTransportRed");
      public static readonly ApplicationIcon Paste = AddNamedIcon("Paste", "Paste");
      public static readonly ApplicationIcon PDF = AddNamedIcon("PDF", "PDF");
      public static readonly ApplicationIcon Pericentral = AddNamedIcon("Pericentral", "Pericentral");
      public static readonly ApplicationIcon Periportal = AddNamedIcon("Periportal", "Periportal");
      public static readonly ApplicationIcon Permeability = AddNamedIcon("Permeability", "Permeability");
      public static readonly ApplicationIcon Pgp = AddNamedIcon("Pgp", "Pgp");
      public static readonly ApplicationIcon PKAnalysis = AddNamedIcon("PKAnalysis", "PKAnalysis");
      public static readonly ApplicationIcon PKML = AddNamedIcon("PKML", "PKML");
      public static readonly ApplicationIcon PKSim = AddNamedIcon("PKSim", "PKSim");
      public static readonly ApplicationIcon Plasma = AddNamedIcon("Plasma", "Plasma");
      public static readonly ApplicationIcon Population = AddNamedIcon("Population", "Population");
      public static readonly ApplicationIcon PopulationError = AddNamedIcon("PopulationError", "PopulationError");
      public static readonly ApplicationIcon PopulationFolder = AddNamedIcon("PopulationFolder", "PopulationFolder");
      public static readonly ApplicationIcon PopulationGreen = AddNamedIcon("PopulationGreen", "PopulationGreen");
      public static readonly ApplicationIcon PopulationRed = AddNamedIcon("PopulationRed", "PopulationRed");
      public static readonly ApplicationIcon PopulationSimulation = AddNamedIcon("PopulationSimulation", "PopulationSimulation");
      public static readonly ApplicationIcon PopulationSimulationGreen = AddNamedIcon("PopulationSimulationGreen", "PopulationSimulationGreen");
      public static readonly ApplicationIcon PopulationSimulationRed = AddNamedIcon("PopulationSimulationRed", "PopulationSimulationRed");
      public static readonly ApplicationIcon PopulationSimulationSettings = AddNamedIcon("SimulationSettings", "PopulationSimulationSettings");
      public static readonly ApplicationIcon PortalVein = AddNamedIcon("PortalVein", "PortalVein");
      public static readonly ApplicationIcon Previous = AddNamedIcon("Previous", "Previous");
      public static readonly ApplicationIcon ProjectDescription = AddNamedIcon("Description", "ProjectDescription");
      public static readonly ApplicationIcon Protein = AddNamedIcon("Protein", "Protein");
      public static readonly ApplicationIcon ProteinExpression = AddNamedIcon("ProteinExpression", "ProteinExpression");
      public static readonly ApplicationIcon ProteinExpressionError = AddNamedIcon("ProteinExpressionError", "ProteinExpressionError");
      public static readonly ApplicationIcon Protocol = AddNamedIcon("Protocol", "Protocol");
      public static readonly ApplicationIcon ProtocolFolder = AddNamedIcon("ProtocolFolder", "ProtocolFolder");
      public static readonly ApplicationIcon ProtocolGreen = AddNamedIcon("ProtocolGreen", "ProtocolGreen");
      public static readonly ApplicationIcon ProtocolRed = AddNamedIcon("ProtocolRed", "ProtocolRed");
      public static readonly ApplicationIcon R = AddNamedIcon("R", "R");
      public static readonly ApplicationIcon Rabbit = AddNamedIcon("Rabbit", "Rabbit");
      public static readonly ApplicationIcon RabbitGreen = AddNamedIcon("RabbitGreen", "RabbitGreen");
      public static readonly ApplicationIcon RabbitRed = AddNamedIcon("RabbitRed", "RabbitRed");
      public static readonly ApplicationIcon RangeAnalysis = AddNamedIcon("RangeAnalysis", "RangeAnalysis");
      public static readonly ApplicationIcon RangeAnalysisGreen = AddNamedIcon("RangeAnalysisGreen", "RangeAnalysisGreen");
      public static readonly ApplicationIcon RangeAnalysisRed = AddNamedIcon("RangeAnalysisRed", "RangeAnalysisRed");
      public static readonly ApplicationIcon Rat = AddNamedIcon("Rat", "Rat");
      public static readonly ApplicationIcon RatGreen = AddNamedIcon("RatGreen", "RatGreen");
      public static readonly ApplicationIcon RatRed = AddNamedIcon("RatRed", "RatRed");
      public static readonly ApplicationIcon Reaction = AddNamedIcon("Reaction", IconNames.REACTION);
      public static readonly ApplicationIcon ReactionFolder = AddNamedIcon("ReactionFolder", "ReactionFolder");
      public static readonly ApplicationIcon ReactionGreen = AddNamedIcon("ReactionGreen", "ReactionGreen");
      public static readonly ApplicationIcon ReactionRed = AddNamedIcon("ReactionRed", "ReactionRed");
      public static readonly ApplicationIcon Rectum = AddNamedIcon("Rectum", "Rectum");
      public static readonly ApplicationIcon Refresh = AddNamedIcon("Refresh", "Refresh");
      public static readonly ApplicationIcon RefreshAll = AddNamedIcon("RefreshAll", "RefreshAll");
      public static readonly ApplicationIcon RefreshSelected = AddNamedIcon("RefreshSelected", "RefreshSelected");
      public static readonly ApplicationIcon Remove = AddNamedIcon("Delete", "Remove");
      public static readonly ApplicationIcon Rename = AddNamedIcon("Rename", "Rename");
      public static readonly ApplicationIcon Reset = AddNamedIcon("Refresh", "Reset");
      public static readonly ApplicationIcon Run = AddNamedIcon("Run", "Run");
      public static readonly ApplicationIcon Saliva = AddNamedIcon("Saliva", "Saliva");
      public static readonly ApplicationIcon Save = AddNamedIcon("Save", "Save");
      public static readonly ApplicationIcon SaveAs = AddNamedIcon("SaveAs", "SaveAs");
      public static readonly ApplicationIcon SaveAsTemplate = AddNamedIcon("SaveAction", "SaveAsTemplate");
      public static readonly ApplicationIcon FavoritesSave = AddNamedIcon("FavoritesSave", "FavoritesSave");
      public static readonly ApplicationIcon ScaleFactor = AddNamedIcon("ScaleFactor", "ScaleFactor");
      public static readonly ApplicationIcon ScatterAnalysis = AddNamedIcon("ScatterAnalysis", "ScatterAnalysis");
      public static readonly ApplicationIcon ScatterAnalysisGreen = AddNamedIcon("ScatterAnalysisGreen", "ScatterAnalysisGreen");
      public static readonly ApplicationIcon ScatterAnalysisRed = AddNamedIcon("ScatterAnalysisRed", "ScatterAnalysisRed");
      public static readonly ApplicationIcon Search = AddNamedIcon("Search", "Search");
      public static readonly ApplicationIcon Settings = AddNamedIcon("Settings", "Settings");
      public static readonly ApplicationIcon Simulation = AddNamedIcon("Simulation", IconNames.SIMULATION);
      public static readonly ApplicationIcon SimulationExplorer = AddNamedIcon("SimulationExplorer", "SimulationExplorer");
      public static readonly ApplicationIcon SimulationFolder = AddNamedIcon("SimulationFolder", "SimulationFolder");
      public static readonly ApplicationIcon SimulationGreen = AddNamedIcon("SimulationGreen", "SimulationGreen");
      public static readonly ApplicationIcon SimulationRed = AddNamedIcon("SimulationRed", "SimulationRed");
      public static readonly ApplicationIcon SimulationSettings = AddNamedIcon("SimulationSettings", IconNames.SIMULATION_SETTINGS);
      public static readonly ApplicationIcon SimulationSettingsFolder = AddNamedIcon("SimulationSettingsFolder", "SimulationSettingsFolder");
      public static readonly ApplicationIcon SimulationSettingsGreen = AddNamedIcon("SimulationSettingsGreen", "SimulationSettingsGreen");
      public static readonly ApplicationIcon SimulationSettingsRed = AddNamedIcon("SimulationSettingsRed", "SimulationSettingsRed");
      public static readonly ApplicationIcon Skin = AddNamedIcon("Skin", "Skin");
      public static readonly ApplicationIcon SmallIntestine = AddNamedIcon("SmallIntestine", "SmallIntestine");
      public static readonly ApplicationIcon Solver = AddNamedIcon("Solver", "Solver");
      public static readonly ApplicationIcon SpatialStructure = AddNamedIcon("SpatialStructure", IconNames.SPATIAL_STRUCTURE);
      public static readonly ApplicationIcon SpatialStructureFolder = AddNamedIcon("SpatialStructureFolder", "SpatialStructureFolder");
      public static readonly ApplicationIcon SpatialStructureGreen = AddNamedIcon("SpatialStructureGreen", "SpatialStructureGreen");
      public static readonly ApplicationIcon SpatialStructureRed = AddNamedIcon("SpatialStructureRed", "SpatialStructureRed");
      public static readonly ApplicationIcon SpecificBinding = AddNamedIcon("SpecificBinding", "SpecificBinding");
      public static readonly ApplicationIcon Spleen = AddNamedIcon("Spleen", "Spleen");
      public static readonly ApplicationIcon Stomach = AddNamedIcon("Stomach", "Stomach");
      public static readonly ApplicationIcon Stop = AddNamedIcon("Stop", "Stop");
      public static readonly ApplicationIcon Subcutaneous = AddNamedIcon("Subcutaneous", "Subcutaneous");
      public static readonly ApplicationIcon SimulationComparison = AddNamedIcon("IndividualSimulationComparison", "SimulationComparison");
      public static readonly ApplicationIcon SytemSettings = AddNamedIcon("Population", "SytemSettings");
      public static readonly ApplicationIcon Time = AddNamedIcon("Time", "Time");
      public static readonly ApplicationIcon TimeProfileAnalysis = AddNamedIcon("TimeProfileAnalysis", "TimeProfileAnalysis");
      public static readonly ApplicationIcon TimeProfileAnalysisGreen = AddNamedIcon("TimeProfileAnalysisGreen", "TimeProfileAnalysisGreen");
      public static readonly ApplicationIcon TimeProfileAnalysisRed = AddNamedIcon("TimeProfileAnalysisRed", "TimeProfileAnalysisRed");
      public static readonly ApplicationIcon Transport = AddNamedIcon("Influx", "Transport");
      public static readonly ApplicationIcon Transporter = AddNamedIcon("Transporter", "Transporter");
      public static readonly ApplicationIcon TubularSecretion = AddNamedIcon("TubularSecretion", "TubularSecretion");
      public static readonly ApplicationIcon UncheckAll = AddNamedIcon("UncheckAll", "UncheckAll");
      public static readonly ApplicationIcon UncheckSelected = AddNamedIcon("UncheckSelected", "UncheckSelected");
      public static readonly ApplicationIcon UncompetitiveInhibition = AddNamedIcon("UncompetitiveInhibition", "UncompetitiveInhibition");
      public static readonly ApplicationIcon Undo = AddNamedIcon("Undo", "Undo");
      public static readonly ApplicationIcon UnitInformation = AddNamedIcon("UnitInformation", "UnitInformation");
      public static readonly ApplicationIcon Up = AddNamedIcon("Up", "Up");
      public static readonly ApplicationIcon Update = AddNamedIcon("Update", "Update");
      public static readonly ApplicationIcon UpperIleum = AddNamedIcon("UpperIleum", "UpperIleum");
      public static readonly ApplicationIcon UpperJejunum = AddNamedIcon("UpperJejunum", "UpperJejunum");
      public static readonly ApplicationIcon UserDefined = AddNamedIcon("Individual", "UserDefined");
      public static readonly ApplicationIcon UserSettings = AddNamedIcon("Settings", "UserSettings");
      public static readonly ApplicationIcon VascularEndothelium = AddNamedIcon("Endothelium", "VascularEndothelium");
      public static readonly ApplicationIcon VenousBlood = AddNamedIcon("VenousBlood", "VenousBlood");
      public static readonly ApplicationIcon Warning = AddNamedIcon("Notifications", "Warning");
      public static readonly ApplicationIcon ZoomIn = AddNamedIcon("ZoomIn", "ZoomIn");
      public static readonly ApplicationIcon ZoomOut = AddNamedIcon("ZoomOut", "ZoomOut");
      public static readonly ApplicationIcon CopySelection = AddNamedIcon("CopySelection", "Copy Selection");
      public static readonly ApplicationIcon JournalExportToWord = AddNamedIcon("JournalExportToWord", IconNames.JOURNAL_EXPORT_TO_WORD);
      public static readonly ApplicationIcon SBML = AddNamedIcon("SBML", "SBML");
      public static readonly ApplicationIcon PageAdd = AddNamedIcon("PageAdd", "PageAdd");
      public static readonly ApplicationIcon IndividualSimulationComparison = AddNamedIcon("IndividualSimulationComparison", "IndividualSimulationComparison");
      public static readonly ApplicationIcon PopulationSimulationComparison = AddNamedIcon("PopulationSimulationComparison", "PopulationSimulationComparison");
      public static readonly ApplicationIcon JournalSelect = AddNamedIcon("JournalSelect", "JournalSelect");
      public static readonly ApplicationIcon HistoryExport = AddNamedIcon("HistoryExport", "HistoryExport");
      public static readonly ApplicationIcon SimulationClone = AddNamedIcon("SimulationClone", "SimulationClone");
      public static readonly ApplicationIcon AnalysesLoad = AddNamedIcon("AnalysesLoad", "AnalysesLoad");
      public static readonly ApplicationIcon AnalysesSave = AddNamedIcon("AnalysesSave", "AnalysesSave");
      public static readonly ApplicationIcon ObserverAdd = AddNamedIcon("ObserverAdd", "ObserverAdd");
      public static readonly ApplicationIcon Report = AddNamedIcon("Report", IconNames.REPORT);
      public static readonly ApplicationIcon ExportToCSV = AddNamedIcon("ExportToCSV", "ExportToCSV");
      public static readonly ApplicationIcon ExportToExcel = AddNamedIcon("ExportToExcel", "ExportToExcel");
      public static readonly ApplicationIcon PKAnalysesImportFromCSV = AddNamedIcon("PKAnalysesImportFromCSV", "PKAnalysesImportFromCSV");
      public static readonly ApplicationIcon Notifications = AddNamedIcon("Notifications", "Notifications");
      public static readonly ApplicationIcon SimulationLoad = AddNamedIcon("SimulationLoad", "SimulationLoad");
      public static readonly ApplicationIcon PKMLLoad = AddNamedIcon("PKMLLoad", "PKMLLoad");
      public static readonly ApplicationIcon PKMLSave = AddNamedIcon("PKMLSave", "PKMLSave");
      public static readonly ApplicationIcon Administration = AddNamedIcon("Administration", "Administration");
      public static readonly ApplicationIcon ComparisonFolder = AddNamedIcon("ComparisonFolder", "ComparisonFolder");
      public static readonly ApplicationIcon About = AddNamedIcon("About", "About");
      public static readonly ApplicationIcon ContainerSave = AddNamedIcon("ContainerSave", "ContainerSave");
      public static readonly ApplicationIcon EventSave = AddNamedIcon("EventSave", "EventSave");
      public static readonly ApplicationIcon AddFormulation = AddNamedIcon("FormulationAdd", "FormulationAdd");
      public static readonly ApplicationIcon FormulationLoad = AddNamedIcon("FormulationLoad", "FormulationLoad");
      public static readonly ApplicationIcon SaveFormulation = AddNamedIcon("FormulationSave", "FormulationSave");
      public static readonly ApplicationIcon MoleculeError = AddNamedIcon("MoleculeError", "MoleculeError");
      public static readonly ApplicationIcon SaveMolecule = AddNamedIcon("MoleculeSave", "MoleculeSave");
      public static readonly ApplicationIcon AddMoleculeStartValues = AddNamedIcon("MoleculeStartValuesAdd", "MoleculeStartVAluesAdd");
      public static readonly ApplicationIcon MoleculeStartValuesLoad = AddNamedIcon("MoleculeStartValuesLoad", "MoleculeStartValuesLoad");
      public static readonly ApplicationIcon SaveMoleculeStartValues = AddNamedIcon("MoleculeStartValuesSave", "MoleculeStartValuesSave");
      public static readonly ApplicationIcon SaveObserver = AddNamedIcon("ObserverSave", "ObserverSave");
      public static readonly ApplicationIcon AddParameterStartValues = AddNamedIcon("ParameterStartValuesAdd", "ParameterStartValuesAdd");
      public static readonly ApplicationIcon ParameterStartValuesLoad = AddNamedIcon("ParameterStartValuesLoad", "ParameterStartValuesLoad");
      public static readonly ApplicationIcon SaveParameterStartValues = AddNamedIcon("ParameterStartValuesSave", "ParameterStartValuesSave");
      public static readonly ApplicationIcon SaveReaction = AddNamedIcon("ReactionSave", "ReactionSave");
      public static readonly ApplicationIcon SaveSpatialStructure = AddNamedIcon("SpatialStructureSave", "SpatialStructureSave");
      public static readonly ApplicationIcon ParameterIdentificationFolder = AddNamedIcon("ParameterIdentificationFolder", "ParameterIdentificationFolder");
      public static readonly ApplicationIcon SensitivityAnalysisFolder = AddNamedIcon("SensitivityAnalysisFolder", "SensitivityAnalysisFolder");
      public static readonly ApplicationIcon SensitivityAnalysis = AddNamedIcon("SensitivityAnalysis", "SensitivityAnalysis");
      public static readonly ApplicationIcon ParameterIdentification = AddNamedIcon("ParameterIdentification", "ParameterIdentification");
      public static readonly ApplicationIcon ResidualHistogramAnalysis = AddNamedIcon("ResidualHistogramAnalysis", "ResidualHistogramAnalysis");
      public static readonly ApplicationIcon ResidualHistogramAnalysisGreen = AddNamedIcon("ResidualHistogramAnalysisGreen", "ResidualHistogramAnalysisGreen");
      public static readonly ApplicationIcon ResidualHistogramAnalysisRed = AddNamedIcon("ResidualHistogramAnalysisRed", "ResidualHistogramAnalysisRed");
      public static readonly ApplicationIcon ResidualVsTimeAnalysis = AddNamedIcon("ResidualVsTimeAnalysis", "ResidualVsTimeAnalysis");
      public static readonly ApplicationIcon ResidualVsTimeAnalysisGreen = AddNamedIcon("ResidualVsTimeAnalysisGreen", "ResidualVsTimeAnalysisGreen");
      public static readonly ApplicationIcon ResidualVsTimeAnalysisRed = AddNamedIcon("ResidualVsTimeAnalysisRed", "ResidualVsTimeAnalysisRed");
      public static readonly ApplicationIcon PredictedVsObservedAnalysis = AddNamedIcon("PredictedVsObservedAnalysis", "PredictedVsObservedAnalysis");
      public static readonly ApplicationIcon PredictedVsObservedAnalysisGreen = AddNamedIcon("PredictedVsObservedAnalysisGreen", "PredictedVsObservedAnalysisGreen");
      public static readonly ApplicationIcon PredictedVsObservedAnalysisRed = AddNamedIcon("PredictedVsObservedAnalysisRed", "PredictedVsObservedAnalysisRed");
      public static readonly ApplicationIcon DeleteFolderOnly = AddNamedIcon("DeleteFolderOnly", "DeleteFolderOnly");
      public static readonly ApplicationIcon CorrelationAnalysis = AddNamedIcon("CorrelationAnalysis", "CorrelationAnalysis");
      public static readonly ApplicationIcon CorrelationAnalysisGreen = AddNamedIcon("CorrelationAnalysisGreen", "CorrelationAnalysisGreen");
      public static readonly ApplicationIcon CorrelationAnalysisRed = AddNamedIcon("CorrelationAnalysisRed", "CorrelationAnalysisRed");
      public static readonly ApplicationIcon CovarianceAnalysis = AddNamedIcon("CovarianceAnalysis", "CovarianceAnalysis");
      public static readonly ApplicationIcon CovarianceAnalysisGreen = AddNamedIcon("CovarianceAnalysisGreen", "CovarianceAnalysisGreen");
      public static readonly ApplicationIcon CovarianceAnalysisRed = AddNamedIcon("CovarianceAnalysisRed", "CovarianceAnalysisRed");
      public static readonly ApplicationIcon DeleteSelected = AddNamedIcon("DeleteSelected", "DeleteSelected");
      public static readonly ApplicationIcon DeleteSourceNotDefined = AddNamedIcon("DeleteSourceNotDefined", "DeleteSourceNotDefined");
      public static readonly ApplicationIcon ExtendMoleculeStartValues = AddNamedIcon("ExtendMoleculeStartValues", "ExtendMoleculeStartValues");
      public static readonly ApplicationIcon MoleculeObserver = AddNamedIcon("MoleculeObserver", "MoleculeObserver");
      public static readonly ApplicationIcon OutputSelection = AddNamedIcon("OutputSelection", "OutputSelection");
      public static readonly ApplicationIcon PreviewOriginData = AddNamedIcon("PreviewOriginData", "PreviewOriginData");
      public static readonly ApplicationIcon Tree = AddNamedIcon("Tree", "Tree");
      public static readonly ApplicationIcon Diagram = AddNamedIcon("Diagram", "Diagram");
      public static readonly ApplicationIcon ContainerObserver = AddNamedIcon("ContainerObserver", "ContainerObserver");
      public static readonly ApplicationIcon ImportAll = AddNamedIcon("LoadAllSheets", "LoadAllSheets");
      public static readonly ApplicationIcon ReactionList = AddNamedIcon("ReactionList", "ReactionList");
      public static readonly ApplicationIcon SolverSettings = AddNamedIcon("SolverSettings", "SolverSettings");
      public static readonly ApplicationIcon Swap = AddNamedIcon("Swap", "Swap");
      public static readonly ApplicationIcon ImportAction = AddNamedIcon("ImportAction", "ImportAction");
      public static readonly ApplicationIcon FractionData = AddNamedIcon("FractionData", "FractionData");
      public static readonly ApplicationIcon ObservedDataForMolecule = AddNamedIcon("ObservedDataForMolecule", "ObservedDataForMolecule");
      public static readonly ApplicationIcon ParameterIdentificationVisualFeedback = AddNamedIcon("ParameterIdentificationVisualFeedback", "ParameterIdentificationVisualFeedback");
      public static readonly ApplicationIcon Results = AddNamedIcon("Results", "Results");
      public static readonly ApplicationIcon Formula = AddNamedIcon("Formula", "Formula");
      public static readonly ApplicationIcon UserDefinedVariability = AddNamedIcon("UserDefinedVariability", "UserDefinedVariability");
      public static readonly ApplicationIcon TimeProfileConfidenceInterval = AddNamedIcon("TimeProfileConfidenceInterval", "TimeProfileConfidenceInterval");
      public static readonly ApplicationIcon TimeProfilePredictionInterval = AddNamedIcon("TimeProfilePredictionInterval", "TimeProfilePredictionInterval");
      public static readonly ApplicationIcon TimeProfileVPCInterval = AddNamedIcon("TimeProfileVPCInterval", "TimeProfileVPCInterval");
      public static readonly ApplicationIcon PKAnalysesExportToCSV = AddNamedIcon("PKAnalysesExportToCSV", "PKAnalysesExportToCSV");
      public static readonly ApplicationIcon PKParameterSensitivityAnalysis = AddNamedIcon("PKParameterSensitivityAnalysis", "PKParameterSensitivityAnalysis");
      public static readonly ApplicationIcon SensitivityAnalysisVisualFeedback = AddNamedIcon("SensitivityAnalysisVisualFeedback", "SensitivityAnalysisVisualFeedback");
      public static readonly ApplicationIcon ValueOriginMethodAssumption = AddNamedIcon("ValueOriginMethodAssumption", "ValueOriginMethodAssumption");
      public static readonly ApplicationIcon ValueOriginMethodInVitro = AddNamedIcon("ValueOriginMethodInVitro", "ValueOriginMethodInVitro");
      public static readonly ApplicationIcon ValueOriginMethodInVivo = AddNamedIcon("ValueOriginMethodInVivo", "ValueOriginMethodInVivo");
      public static readonly ApplicationIcon ValueOriginMethodOther = AddNamedIcon("ValueOriginMethodOther", "ValueOriginMethodOther");
      public static readonly ApplicationIcon ValueOriginMethodManualFit = AddNamedIcon("ValueOriginMethodManualFit", "ValueOriginMethodManualFit");
      public static readonly ApplicationIcon ValueOriginMethodParameterIdentification = AddNamedIcon("ValueOriginMethodParameterIdentification", "ValueOriginMethodParameterIdentification");
      public static readonly ApplicationIcon ValueOriginMethodUnknown = AddNamedIcon("ValueOriginMethodUnknown", "ValueOriginMethodUnknown");
      public static readonly ApplicationIcon ValueOriginSourceDatabase = AddNamedIcon("ValueOriginSourceDatabase", "ValueOriginSourceDatabase");
      public static readonly ApplicationIcon ValueOriginSourceInternet = AddNamedIcon("ValueOriginSourceInternet", "ValueOriginSourceInternet");
      public static readonly ApplicationIcon ValueOriginSourceParameterIdentification = AddNamedIcon("ValueOriginSourceParameterIdentification", "ValueOriginSourceParameterIdentification");
      public static readonly ApplicationIcon ValueOriginSourcePublication = AddNamedIcon("ValueOriginSourcePublication", "ValueOriginSourcePublication");
      public static readonly ApplicationIcon ValueOriginSourceUnknown = AddNamedIcon("ValueOriginSourceUnknown", "ValueOriginSourceUnknown");
      public static readonly ApplicationIcon ValueOriginSourceOther = AddNamedIcon("ValueOriginSourceOther", "ValueOriginSourceOther");
      public static readonly ApplicationIcon Tag = AddNamedIcon("Tag", "Tag");
      public static readonly ApplicationIcon UserDefinedSpecies = AddNamedIcon("UserDefinedSpecies", "UserDefinedSpecies");
      public static readonly ApplicationIcon Properties = AddNamedIcon("Properties", "Properties");
      public static readonly ApplicationIcon Snapshot = AddNamedIcon("Snapshot", "Snapshot");
      public static readonly ApplicationIcon SnapshotExport = AddNamedIcon("SnapshotExport", "SnapshotExport");
      public static readonly ApplicationIcon SnapshotImport = AddNamedIcon("SnapshotImport", "SnapshotImport");
      public static readonly ApplicationIcon File = AddNamedIcon("ProjectNew", "File");
      public static readonly ApplicationIcon Redo = AddNamedIcon("Redo", "Redo");
      public static readonly ApplicationIcon ClearHistory = AddNamedIcon("Delete", "ClearHistory");
      public static readonly ApplicationIcon AmountObservedData = AddNamedIcon("AmountObservedData", "AmountObservedData");
      public static readonly ApplicationIcon AmountObservedDataForMolecule = AddNamedIcon("AmountObservedDataForMolecule", "AmountObservedDataForMolecule");

      // All icons should go at the end of the preceding list, before this delimiting icon - EmptyIcon
      private static ApplicationIcon createEmptyIcon() => new ApplicationIcon((Icon) null);

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

      public static ApplicationIcon AddNamedIcon(string resName, string iconName)
      {
         var name = iconName.ToUpperInvariant();
         var iconAsBytes = getIcon(resName);
         if (iconAsBytes == null)
            return createEmptyIcon();

         var appIcon = new ApplicationIcon(iconAsBytes)
         {
            IconName = name,
            Index = _allIcons.Count
         };

         _allIcons.Add(appIcon);
         return appIcon;
      }

//
//      public static ApplicationIcon AddNamedIcon(byte[] icon, string iconName)
//      {
//         var name = iconName.ToUpperInvariant();
//         var appIcon = new ApplicationIcon(icon)
//         {
//            IconName = name,
//            Index = _allIcons.Count
//         };
//
//         _allIcons.Add(appIcon);
//         return appIcon;
//      }

      public static byte[] getIcon(string iconName)
      {
         var assembly = Assembly.GetExecutingAssembly();
         var resourceName = typeof(ApplicationIcon).Namespace + ".Icons." + iconName + ".ico";
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