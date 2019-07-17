using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Assets
{
   public static class ApplicationIcons
   {
      private static readonly ICache<string, ApplicationIcon> _allIcons = new Cache<string, ApplicationIcon>(icon => icon.IconName);
      private static IList<ApplicationIcon> _allIconsList;

      public static readonly ApplicationIcon Absorption = AddNamedIcon(Icons.Absorption, "Absorption");
      public static readonly ApplicationIcon ActiveEfflux = AddNamedIcon(Icons.Efflux, "ActiveEfflux");
      public static readonly ApplicationIcon ActiveInflux = AddNamedIcon(Icons.Influx, "ActiveInflux");
      public static readonly ApplicationIcon Add = AddNamedIcon(Icons.AddAction, "Add");
      public static readonly ApplicationIcon AddEnzyme = AddNamedIcon(Icons.Enzyme, "AddEnzyme");
      public static readonly ApplicationIcon AddProtein = AddNamedIcon(Icons.Protein, "AddProtein");
      public static readonly ApplicationIcon AddToJournal = AddNamedIcon(Icons.AddAction, "AddToJournal");
      public static readonly ApplicationIcon AgingPopulationSimulation = AddNamedIcon(Icons.AgingPopulationSimulation, "AgingPopulationSimulation");
      public static readonly ApplicationIcon AgingPopulationSimulationGreen = AddNamedIcon(Icons.AgingPopulationSimulationGreen, "AgingPopulationSimulationGreen");
      public static readonly ApplicationIcon AgingPopulationSimulationRed = AddNamedIcon(Icons.AgingPopulationSimulationRed, "AgingPopulationSimulationRed");
      public static readonly ApplicationIcon AgingSimulation = AddNamedIcon(Icons.AgingSimulation, "AgingSimulation");
      public static readonly ApplicationIcon AgingSimulationGreen = AddNamedIcon(Icons.AgingSimulationGreen, "AgingSimulationGreen");
      public static readonly ApplicationIcon AgingSimulationRed = AddNamedIcon(Icons.AgingSimulationRed, "AgingSimulationRed");
      public static readonly ApplicationIcon Application = AddNamedIcon(Icons.Application, "Application");
      public static readonly ApplicationIcon Applications = AddNamedIcon(Icons.Application, "Applications");
      public static readonly ApplicationIcon ApplicationsError = AddNamedIcon(Icons.ApplicationError, "ApplicationsError");
      public static readonly ApplicationIcon ApplicationSettings = AddNamedIcon(Icons.Settings, "ApplicationSettings");
      public static readonly ApplicationIcon ApplyAll = AddNamedIcon(Icons.ApplyAll, "ApplyAll");
      public static readonly ApplicationIcon ArterialBlood = AddNamedIcon(Icons.ArterialBlood, "ArterialBlood");
      public static readonly ApplicationIcon Back = AddNamedIcon(Icons.Back, "Back");
      public static readonly ApplicationIcon BasicPharmacochemistry = AddNamedIcon(Icons.BasicPharmacochemistry, "BasicPharmacochemistry");
      public static readonly ApplicationIcon BasicPharmacochemistryError = AddNamedIcon(Icons.BasicPharmacochemistryError, "BasicPharmacochemistryError");
      public static readonly ApplicationIcon Beagle = AddNamedIcon(Icons.Beagle, "Beagle");
      public static readonly ApplicationIcon BeagleGreen = AddNamedIcon(Icons.BeagleGreen, "BeagleGreen");
      public static readonly ApplicationIcon BeagleRed = AddNamedIcon(Icons.BeagleRed, "BeagleRed");
      public static readonly ApplicationIcon BiologicalProperties = AddNamedIcon(Icons.BiologicalProperties, "BiologicalProperties");
      public static readonly ApplicationIcon BiologicalPropertiesError = AddNamedIcon(Icons.BiologicalPropertiesError, "BiologicalPropertiesError");
      public static readonly ApplicationIcon Blood = AddNamedIcon(Icons.Blood, "Blood");
      public static readonly ApplicationIcon BloodCells = AddNamedIcon(Icons.BloodCells, "BloodCells");
      public static readonly ApplicationIcon Bone = AddNamedIcon(Icons.Bone, "Bone");
      public static readonly ApplicationIcon BoxWhiskerAnalysis = AddNamedIcon(Icons.BoxWhiskerAnalysis, "BoxWhiskerAnalysis");
      public static readonly ApplicationIcon BoxWhiskerAnalysisGreen = AddNamedIcon(Icons.BoxWhiskerAnalysisGreen, "BoxWhiskerAnalysisGreen");
      public static readonly ApplicationIcon BoxWhiskerAnalysisRed = AddNamedIcon(Icons.BoxWhiskerAnalysisRed, "BoxWhiskerAnalysisRed");
      public static readonly ApplicationIcon Brain = AddNamedIcon(Icons.Brain, "Brain");
      public static readonly ApplicationIcon BuildingBlockExplorer = AddNamedIcon(Icons.BuildingBlockExplorer, "BuildingBlockExplorer");
      public static readonly ApplicationIcon Caecum = AddNamedIcon(Icons.Caecum, "Caecum");
      public static readonly ApplicationIcon Cancel = AddNamedIcon(Icons.Cancel, "Cancel");
      public static readonly ApplicationIcon Cat = AddNamedIcon(Icons.Cat, "Cat");
      public static readonly ApplicationIcon CatGreen = AddNamedIcon(Icons.CatGreen, "CatGreen");
      public static readonly ApplicationIcon CatRed = AddNamedIcon(Icons.CatRed, "CatRed");
      public static readonly ApplicationIcon Cattle = AddNamedIcon(Icons.Cattle, "Cattle");
      public static readonly ApplicationIcon CattleGreen = AddNamedIcon(Icons.CattleGreen, "CattleGreen");
      public static readonly ApplicationIcon CattleRed = AddNamedIcon(Icons.CattleRed, "CattleRed");
      public static readonly ApplicationIcon Cecum = AddNamedIcon(Icons.Caecum, "Cecum");
      public static readonly ApplicationIcon CheckAll = AddNamedIcon(Icons.CheckAll, "CheckAll");
      public static readonly ApplicationIcon CheckSelected = AddNamedIcon(Icons.CheckSelected, "CheckSelected");
      public static readonly ApplicationIcon Clone = AddNamedIcon(Icons.SimulationClone, "Clone");
      public static readonly ApplicationIcon Close = AddNamedIcon(Icons.ProjectClose, "Close");
      public static readonly ApplicationIcon CloseProject = AddNamedIcon(Icons.ProjectClose, "CloseProject");
      public static readonly ApplicationIcon ClusterExport = AddNamedIcon(Icons.ClusterExport, "ClusterExport");
      public static readonly ApplicationIcon ColonAscendens = AddNamedIcon(Icons.ColonAscendens, "ColonAscendens");
      public static readonly ApplicationIcon ColonDescendens = AddNamedIcon(Icons.ColonDescendens, "ColonDescendens");
      public static readonly ApplicationIcon ColonSigmoid = AddNamedIcon(Icons.ColonSigmoid, "ColonSigmoid");
      public static readonly ApplicationIcon ColonTransversum = AddNamedIcon(Icons.ColonTransversum, "ColonTransversum");
      public static readonly ApplicationIcon Commit = AddNamedIcon(Icons.Commit, "Commit");
      public static readonly ApplicationIcon CommitRed = AddNamedIcon(Icons.CommitRed, "CommitRed");
      public static readonly ApplicationIcon SimulationComparisonFolder = AddNamedIcon(Icons.ComparisonFolder, "SimulationComparisonFolder");
      public static readonly ApplicationIcon CompetitiveInhibition = AddNamedIcon(Icons.CompetitiveInhibition, "CompetitiveInhibition");
      public static readonly ApplicationIcon Complex = AddNamedIcon(Icons.Complex, "Complex");
      public static readonly ApplicationIcon Compound = AddNamedIcon(Icons.Molecule, "Compound");
      public static readonly ApplicationIcon CompoundError = AddNamedIcon(Icons.MoleculeError, "CompoundError");
      public static readonly ApplicationIcon CompoundFolder = AddNamedIcon(Icons.MoleculeFolder, "CompoundFolder");
      public static readonly ApplicationIcon CompoundGreen = AddNamedIcon(Icons.MoleculeGreen, "CompoundGreen");
      public static readonly ApplicationIcon CompoundRed = AddNamedIcon(Icons.MoleculeRed, "CompoundRed");
      public static readonly ApplicationIcon SimulationConfigure = AddNamedIcon(Icons.SimulationConfigure, "SimulationConfigure");
      public static readonly ApplicationIcon Container = AddNamedIcon(Icons.Container, "Container");
      public static readonly ApplicationIcon Copy = AddNamedIcon(Icons.Copy, "Copy");
      public static readonly ApplicationIcon Create = AddNamedIcon(Icons.AddAction, "Create");
      public static readonly ApplicationIcon Debug = AddNamedIcon(Icons.Debug, "Debug");
      public static readonly ApplicationIcon ConfigureAndRun = AddNamedIcon(Icons.ConfigureAndRun, "ConfigureAndRun");
      public static readonly ApplicationIcon Delete = AddNamedIcon(Icons.Delete, "Delete");
      public static readonly ApplicationIcon Dermal = AddNamedIcon(Icons.Dermal, "Dermal");
      public static readonly ApplicationIcon Description = AddNamedIcon(Icons.Description, "Description");
      public static readonly ApplicationIcon Comparison = AddNamedIcon(Icons.Comparison, "Comparison");
      public static readonly ApplicationIcon Distribution = AddNamedIcon(Icons.Distribution, "Distribution");
      public static readonly ApplicationIcon DistributionCalculation = AddNamedIcon(Icons.DistributionCalculation, "DistributionCalculation");
      public static readonly ApplicationIcon Dog = AddNamedIcon(Icons.Dog, "Dog");
      public static readonly ApplicationIcon DogGreen = AddNamedIcon(Icons.DogGreen, "DogGreen");
      public static readonly ApplicationIcon DogRed = AddNamedIcon(Icons.DogRed, "DogRed");
      public static readonly ApplicationIcon Down = AddNamedIcon(Icons.Down, "Down");
      public static readonly ApplicationIcon Drug = AddNamedIcon(Icons.Molecule, "Drug");
      public static readonly ApplicationIcon Duodenum = AddNamedIcon(Icons.Duodenum, "Duodenum");
      public static readonly ApplicationIcon DxError = AddNamedIcon(Icons.ErrorProvider, "ErrorProvider");
      public static readonly ApplicationIcon Edit = AddNamedIcon(Icons.Edit, "Edit");
      public static readonly ApplicationIcon PageEdit = AddNamedIcon(Icons.PageEdit, "PageEdit");
      public static readonly ApplicationIcon Efflux = AddNamedIcon(Icons.Efflux, "Efflux");
      public static readonly ApplicationIcon Endothelium = AddNamedIcon(Icons.Endothelium, "Endothelium");
      public static readonly ApplicationIcon Enzyme = AddNamedIcon(Icons.Enzyme, "Enzyme");
      public static readonly ApplicationIcon Error = AddNamedIcon(Icons.Error, "Error");
      public static readonly ApplicationIcon ErrorHint = AddNamedIcon(Icons.ErrorHint, "ErrorHint");
      public static readonly ApplicationIcon Event = AddNamedIcon(Icons.Event, IconNames.EVENT);
      public static readonly ApplicationIcon EventFolder = AddNamedIcon(Icons.EventFolder, "EventFolder");
      public static readonly ApplicationIcon EventGreen = AddNamedIcon(Icons.EventGreen, "EventGreen");
      public static readonly ApplicationIcon EventGroup = AddNamedIcon(Icons.Event, IconNames.EVENT_GROUP);
      public static readonly ApplicationIcon EventRed = AddNamedIcon(Icons.EventRed, "EventRed");
      public static readonly ApplicationIcon Excel = AddNamedIcon(Icons.ObservedData, "Excel");
      public static readonly ApplicationIcon Excretion = AddNamedIcon(Icons.Excretion, "Excretion");
      public static readonly ApplicationIcon Exit = AddNamedIcon(Icons.Exit, "Exit");
      public static readonly ApplicationIcon ExpertParameters = AddNamedIcon(Icons.Parameters, "ExpertParameters");
      public static readonly ApplicationIcon PopulationExportToCSV = AddNamedIcon(Icons.PopulationExportToCSV, "PopulationExportToCSV");
      public static readonly ApplicationIcon ExportToPDF = AddNamedIcon(Icons.PDF, "ExportToPDF");
      public static readonly ApplicationIcon ExtendParameterStartValues = AddNamedIcon(Icons.ExtendParameterStartValues, "ExtendParameterStartValues");
      public static readonly ApplicationIcon ExtracellularMembrane = AddNamedIcon(Icons.ExtracellularMembrane, "ExtracellularMembrane");
      public static readonly ApplicationIcon Fat = AddNamedIcon(Icons.Fat, "Fat");
      public static readonly ApplicationIcon Favorites = AddNamedIcon(Icons.Favorites, "Favorites");
      public static readonly ApplicationIcon FitToPage = AddNamedIcon(Icons.FitToPage, "FitToPage");
      public static readonly ApplicationIcon Folder = AddNamedIcon(Icons.Folder, "Folder");
      public static readonly ApplicationIcon Formulation = AddNamedIcon(Icons.Formulation, "Formulation");
      public static readonly ApplicationIcon FormulationFolder = AddNamedIcon(Icons.FormulationFolder, "FormulationFolder");
      public static readonly ApplicationIcon FormulationGreen = AddNamedIcon(Icons.FormulationGreen, "FormulationGreen");
      public static readonly ApplicationIcon FormulationRed = AddNamedIcon(Icons.FormulationRed, "FormulationRed");
      public static readonly ApplicationIcon Forward = AddNamedIcon(Icons.Forward, "Forward");
      public static readonly ApplicationIcon Gallbladder = AddNamedIcon(Icons.GallBladder, "Gallbladder");
      public static readonly ApplicationIcon GITract = AddNamedIcon(Icons.GITract, "GI-Tract");
      public static readonly ApplicationIcon GlomerularFiltration = AddNamedIcon(Icons.GlomerularFiltration, "GlomerularFiltration");
      public static readonly ApplicationIcon Gonads = AddNamedIcon(Icons.Gonads, "Gonads");
      public static readonly ApplicationIcon GoTo = AddNamedIcon(Icons.GoTo, "GoTo");
      public static readonly ApplicationIcon GroupBy = AddNamedIcon(Icons.GroupBy, "GroupBy");
      public static readonly ApplicationIcon Heart = AddNamedIcon(Icons.Heart, "Heart");
      public static readonly ApplicationIcon Help = AddNamedIcon(Icons.Help, "Help");
      public static readonly ApplicationIcon Histogram = AddNamedIcon(Icons.Histogram, "Histogram");
      public static readonly ApplicationIcon History = AddNamedIcon(Icons.History, "History");
      public static readonly ApplicationIcon Human = AddNamedIcon(Icons.Human, "Human");
      public static readonly ApplicationIcon HumanGreen = AddNamedIcon(Icons.HumanGreen, "HumanGreen");
      public static readonly ApplicationIcon HumanRed = AddNamedIcon(Icons.HumanRed, "HumanRed");
      public static readonly ApplicationIcon Import = AddNamedIcon(Icons.ObservedData, "Import");
      public static readonly ApplicationIcon MoleculeStartValuesImport = AddNamedIcon(Icons.MoleculeStartValuesImport, "MoleculeStartValuesImport");
      public static readonly ApplicationIcon ParameterStartValuesImport = AddNamedIcon(Icons.ParameterStartValuesImport, "ParameterStartValuesImport");
      public static readonly ApplicationIcon ImportPopulation = AddNamedIcon(Icons.ImportPopulation, "ImportPopulation");
      public static readonly ApplicationIcon PopulationSimulationLoad = AddNamedIcon(Icons.PopulationSimulationLoad, "PopulationSimulationLoad");
      public static readonly ApplicationIcon ResultsImportFromCSV = AddNamedIcon(Icons.ResultsImportFromCSV, "ResultsImportFromCSV");
      public static readonly ApplicationIcon IndividualSimulationLoad = AddNamedIcon(Icons.IndividualSimulationLoad, "IndividualSimulationLoad");
      public static readonly ApplicationIcon Individual = AddNamedIcon(Icons.Individual, "Individual");
      public static readonly ApplicationIcon IndividualError = AddNamedIcon(Icons.IndividualError, "IndividualError");
      public static readonly ApplicationIcon IndividualFolder = AddNamedIcon(Icons.IndividualFolder, "IndividualFolder");
      public static readonly ApplicationIcon IndividualGreen = AddNamedIcon(Icons.IndividualGreen, "IndividualGreen");
      public static readonly ApplicationIcon IndividualRed = AddNamedIcon(Icons.IndividualRed, "IndividualRed");
      public static readonly ApplicationIcon ScaleIndividual = AddNamedIcon(Icons.ScaleIndividual, "ScaleIndividual");
      public static readonly ApplicationIcon Induction = AddNamedIcon(Icons.Induction, "Induction");
      public static readonly ApplicationIcon Influx = AddNamedIcon(Icons.Influx, "Influx");
      public static readonly ApplicationIcon Info = AddNamedIcon(Icons.About, "Info");
      public static readonly ApplicationIcon Inhibition = AddNamedIcon(Icons.Inhibition, "Inhibition");
      public static readonly ApplicationIcon Interstitial = AddNamedIcon(Icons.Interstitial, "Interstitial");
      public static readonly ApplicationIcon Intracellular = AddNamedIcon(Icons.Intracellular, "Intracellular");
      public static readonly ApplicationIcon Intravenous = AddNamedIcon(Icons.Intravenous, "Intravenous");
      public static readonly ApplicationIcon IntravenousBolus = AddNamedIcon(Icons.IntravenousBolus, "IntravenousBolus");
      public static readonly ApplicationIcon IrreversibleInhibition = AddNamedIcon(Icons.IrreversibleInhibition, "IrreversibleInhibition");
      public static readonly ApplicationIcon Journal = AddNamedIcon(Icons.Journal, "Journal");
      public static readonly ApplicationIcon Page = AddNamedIcon(Icons.Page, "Page");
      public static readonly ApplicationIcon JournalDiagram = AddNamedIcon(Icons.JournalDiagram, "JournalDiagram");
      public static readonly ApplicationIcon Kidney = AddNamedIcon(Icons.Kidney, "Kidney");
      public static readonly ApplicationIcon LabelAdd = AddNamedIcon(Icons.LabelAdd, "LabelAdd");
      public static readonly ApplicationIcon LargeIntestine = AddNamedIcon(Icons.LargeIntestine, "LargeIntestine");
      public static readonly ApplicationIcon License = AddNamedIcon(Icons.LicenseRegister, "License");
      public static readonly ApplicationIcon Liver = AddNamedIcon(Icons.Liver, "Liver");
      public static readonly ApplicationIcon Load = AddNamedIcon(Icons.LoadAction, "Load");
      public static readonly ApplicationIcon ContainerLoad = AddNamedIcon(Icons.ContainerLoad, "ContainerLoad");
      public static readonly ApplicationIcon EventLoad = AddNamedIcon(Icons.EventLoad, "EventLoad");
      public static readonly ApplicationIcon FavoritesLoad = AddNamedIcon(Icons.FavoritesLoad, "FavoritesLoad");
      public static readonly ApplicationIcon LoadFromTemplate = AddNamedIcon(Icons.LoadAction, "LoadFromTemplate");
      public static readonly ApplicationIcon MoleculeLoad = AddNamedIcon(Icons.MoleculeLoad, "MoleculeLoad");
      public static readonly ApplicationIcon ObserverLoad = AddNamedIcon(Icons.ObserverLoad, "ObserverLoad");
      public static readonly ApplicationIcon ReactionLoad = AddNamedIcon(Icons.ReactionLoad, "ReactionLoad");
      public static readonly ApplicationIcon SpatialStructureLoad = AddNamedIcon(Icons.SpatialStructureLoad, "SpatialStructureLoad");
      public static readonly ApplicationIcon LowerIleum = AddNamedIcon(Icons.LowerIleum, "LowerIleum");
      public static readonly ApplicationIcon LowerJejunum = AddNamedIcon(Icons.LowerJejunum, "LowerJejunum");
      public static readonly ApplicationIcon Lumen = AddNamedIcon(Icons.Lumen, "Lumen");
      public static readonly ApplicationIcon LumenCaecum = AddNamedIcon(Icons.Caecum, "Lumen-Caecum");
      public static readonly ApplicationIcon LumenColonAscendens = AddNamedIcon(Icons.ColonAscendens, "Lumen-ColonAscendens");
      public static readonly ApplicationIcon LumenColonDescendens = AddNamedIcon(Icons.ColonDescendens, "Lumen-ColonDescendens");
      public static readonly ApplicationIcon LumenColonSigmoid = AddNamedIcon(Icons.ColonSigmoid, "Lumen-ColonSigmoid");
      public static readonly ApplicationIcon LumenColonTransversum = AddNamedIcon(Icons.ColonTransversum, "Lumen-ColonTransversum");
      public static readonly ApplicationIcon LumenDuodenum = AddNamedIcon(Icons.Duodenum, "Lumen-Duodenum");
      public static readonly ApplicationIcon LumenLowerIleum = AddNamedIcon(Icons.LowerIleum, "Lumen-LowerIleum");
      public static readonly ApplicationIcon LumenLowerJejunum = AddNamedIcon(Icons.LowerJejunum, "Lumen-LowerJejunum");
      public static readonly ApplicationIcon LumenRectum = AddNamedIcon(Icons.Rectum, "Lumen-Rectum");
      public static readonly ApplicationIcon LumenStomach = AddNamedIcon(Icons.Stomach, "Lumen-Stomach");
      public static readonly ApplicationIcon LumenUpperIleum = AddNamedIcon(Icons.UpperIleum, "Lumen-UpperIleum");
      public static readonly ApplicationIcon LumenUpperJejunum = AddNamedIcon(Icons.UpperJejunum, "Lumen-UpperJejunum");
      public static readonly ApplicationIcon Lung = AddNamedIcon(Icons.Lung, "Lung");
      public static readonly ApplicationIcon ProjectDisplayUnitsConfigure = AddNamedIcon(Icons.ProjectDisplayUnitsConfigure, "ProjectDisplayUnitsConfigure");
      public static readonly ApplicationIcon UserDisplayUnitsConfigure = AddNamedIcon(Icons.UserDisplayUnitsConfigure, "UserDisplayUnitsConfigure");
      public static readonly ApplicationIcon Matlab = AddNamedIcon(Icons.Matlab, "Matlab");
      public static readonly ApplicationIcon MetaData = AddNamedIcon(Icons.MetaData, "MetaData");
      public static readonly ApplicationIcon MetaDataAndUnitInformation = AddNamedIcon(Icons.MetaData, "MetaDataAndUnitInformation");
      public static readonly ApplicationIcon Merge = AddNamedIcon(Icons.Merge, "Merge");
      public static readonly ApplicationIcon MergePopulation = AddNamedIcon(Icons.MergePopulation, "MergePopulation");
      public static readonly ApplicationIcon Metabolism = AddNamedIcon(Icons.Metabolism, "Metabolism");
      public static readonly ApplicationIcon Metabolite = AddNamedIcon(Icons.Metabolite, "Metabolite");
      public static readonly ApplicationIcon Minipig = AddNamedIcon(Icons.Minipig, "Minipig");
      public static readonly ApplicationIcon MinipigGreen = AddNamedIcon(Icons.MinipigGreen, "MinipigGreen");
      public static readonly ApplicationIcon MinipigRed = AddNamedIcon(Icons.MinipigRed, "MinipigRed");
      public static readonly ApplicationIcon MissingData = AddNamedIcon(Icons.MissingData, "MissingData");
      public static readonly ApplicationIcon MissingMetaData = AddNamedIcon(Icons.MissingMetaData, "MissingMetaData");
      public static readonly ApplicationIcon MissingUnitInformation = AddNamedIcon(Icons.MissingUnitInformation, "MissingUnitInformation");
      public static readonly ApplicationIcon MixedInhibition = AddNamedIcon(Icons.MixedInhibition, "MixedInhibition");
      public static readonly ApplicationIcon MoBi = AddNamedIcon(Icons.MoBi, "MoBi");
      public static readonly ApplicationIcon MoBiSimulation = AddNamedIcon(Icons.PKML, "MoBiSimulation");
      public static readonly ApplicationIcon ModelStructure = AddNamedIcon(Icons.ModelStructure, "ModelStructure");
      public static readonly ApplicationIcon ModelStructureError = AddNamedIcon(Icons.ModelStructureError, "ModelStructureError");
      public static readonly ApplicationIcon Molecule = AddNamedIcon(Icons.Molecule, IconNames.MOLECULE);
      public static readonly ApplicationIcon MoleculeFolder = AddNamedIcon(Icons.MoleculeFolder, "MoleculeFolder");
      public static readonly ApplicationIcon MoleculeGreen = AddNamedIcon(Icons.MoleculeGreen, "MoleculeGreen");
      public static readonly ApplicationIcon MoleculeRed = AddNamedIcon(Icons.MoleculeRed, "MoleculeRed");
      public static readonly ApplicationIcon MoleculeStartValues = AddNamedIcon(Icons.MoleculeStartValues, IconNames.MOLECULE_START_VALUES);
      public static readonly ApplicationIcon MoleculeStartValuesFolder = AddNamedIcon(Icons.MoleculeStartValuesFolder, "MoleculeStartValuesFolder");
      public static readonly ApplicationIcon MoleculeStartValuesGreen = AddNamedIcon(Icons.MoleculeStartValuesGreen, "MoleculeStartValuesGreen");
      public static readonly ApplicationIcon MoleculeStartValuesRed = AddNamedIcon(Icons.MoleculeStartValuesRed, "MoleculeStartValuesRed");
      public static readonly ApplicationIcon Monkey = AddNamedIcon(Icons.Monkey, "Monkey");
      public static readonly ApplicationIcon MonkeyGreen = AddNamedIcon(Icons.MonkeyGreen, "MonkeyGreen");
      public static readonly ApplicationIcon MonkeyRed = AddNamedIcon(Icons.MonkeyRed, "MonkeyRed");
      public static readonly ApplicationIcon Mouse = AddNamedIcon(Icons.Mouse, "Mouse");
      public static readonly ApplicationIcon MouseGreen = AddNamedIcon(Icons.MouseGreen, "MouseGreen");
      public static readonly ApplicationIcon MouseRed = AddNamedIcon(Icons.MouseRed, "MouseRed");
      public static readonly ApplicationIcon Muscle = AddNamedIcon(Icons.Muscle, "Muscle");
      public static readonly ApplicationIcon AmountProjectNew = AddNamedIcon(Icons.ProjectNewAmount, "AmountProjectNew");
      public static readonly ApplicationIcon ConcentrationProjectNew = AddNamedIcon(Icons.ProjectNewConcentration, "ConcentrationProjectNew");
      public static readonly ApplicationIcon ContainerAdd = AddNamedIcon(Icons.ContainerAdd, "ContainerAdd");
      public static readonly ApplicationIcon EventAdd = AddNamedIcon(Icons.EventAdd, "EventAdd");
      public static readonly ApplicationIcon MoleculeAdd = AddNamedIcon(Icons.MoleculeAdd, "MoleculeAdd");
      public static readonly ApplicationIcon PKSimMoleculeAdd = AddNamedIcon(Icons.MoleculeAdd, "PKSimMoleculeAdd");
      public static readonly ApplicationIcon ProjectNew = AddNamedIcon(Icons.ProjectNew, "ProjectNew");
      public static readonly ApplicationIcon ReactionAdd = AddNamedIcon(Icons.ReactionAdd, "ReactionAdd");
      public static readonly ApplicationIcon SpatialStructureAdd = AddNamedIcon(Icons.SpatialStructureAdd, "SpatialStructureAdd");
      public static readonly ApplicationIcon Next = AddNamedIcon(Icons.Next, "Next");
      public static readonly ApplicationIcon NonCompetitiveInhibition = AddNamedIcon(Icons.NonCompetitiveInhibition, "NonCompetitiveInhibition");
      public static readonly ApplicationIcon ObservedData = AddNamedIcon(Icons.ObservedData, IconNames.OBSERVED_DATA);
      public static readonly ApplicationIcon ObservedDataCompound = AddNamedIcon(Icons.ObservedDataForMolecule, "ObservedDataCompound");
      public static readonly ApplicationIcon ObservedDataFolder = AddNamedIcon(Icons.ObservedDataFolder, "ObservedDataFolder");
      public static readonly ApplicationIcon Observer = AddNamedIcon(Icons.Observer, IconNames.OBSERVER);
      public static readonly ApplicationIcon ObserverFolder = AddNamedIcon(Icons.ObserverFolder, "ObserverFolder");
      public static readonly ApplicationIcon ObserverGreen = AddNamedIcon(Icons.ObserverGreen, "ObserverGreen");
      public static readonly ApplicationIcon ObserverRed = AddNamedIcon(Icons.ObserverRed, "ObserverRed");
      public static readonly ApplicationIcon OK = AddNamedIcon(Icons.OK, "OK");
      public static readonly ApplicationIcon ProjectOpen = AddNamedIcon(Icons.ProjectOpen, "ProjectOpen");
      public static readonly ApplicationIcon Oral = AddNamedIcon(Icons.Oral, "Oral");
      public static readonly ApplicationIcon Organism = AddNamedIcon(Icons.Organism, "Organism");
      public static readonly ApplicationIcon Other = AddNamedIcon(Icons.Help, "Other");
      public static readonly ApplicationIcon OtherProtein = AddNamedIcon(Icons.Protein, "OtherProtein");
      public static readonly ApplicationIcon OutputInterval = AddNamedIcon(Icons.OutputInterval, "OutputInterval");
      public static readonly ApplicationIcon Pancreas = AddNamedIcon(Icons.Pancreas, "Pancreas");
      public static readonly ApplicationIcon Parameter = AddNamedIcon(Icons.Parameters, IconNames.PARAMETER);
      public static readonly ApplicationIcon ParameterDistribution = AddNamedIcon(Icons.Histogram, "ParameterDistribution");
      public static readonly ApplicationIcon Parameters = AddNamedIcon(Icons.Parameters, "Parameters");
      public static readonly ApplicationIcon ParametersError = AddNamedIcon(Icons.ParametersError, "ParametersError");
      public static readonly ApplicationIcon ParameterStartValueGreen = AddNamedIcon(Icons.ParameterStartValuesGreen, "ParameterStartValuesGreen");
      public static readonly ApplicationIcon ParameterStartValues = AddNamedIcon(Icons.ParameterStartValues, IconNames.PARAMETER_START_VALUES);
      public static readonly ApplicationIcon ParameterStartValuesFolder = AddNamedIcon(Icons.ParameterStartValuesFolder, "ParameterStartValuesFolder");
      public static readonly ApplicationIcon ParameterStartValuesRed = AddNamedIcon(Icons.ParameterStartValuesRed, "ParameterStartValuesRed");
      public static readonly ApplicationIcon PassiveTransport = AddNamedIcon(Icons.PassiveTransport, IconNames.PASSIVE_TRANSPORT);
      public static readonly ApplicationIcon PassiveTransportFolder = AddNamedIcon(Icons.PassiveTransportFolder, "PassiveTransportFolder");
      public static readonly ApplicationIcon PassiveTransportGreen = AddNamedIcon(Icons.PassiveTransportGreen, "PassiveTransportGreen");
      public static readonly ApplicationIcon PassiveTransportRed = AddNamedIcon(Icons.PassiveTransportRed, "PassiveTransportRed");
      public static readonly ApplicationIcon Paste = AddNamedIcon(Icons.Paste, "Paste");
      public static readonly ApplicationIcon PDF = AddNamedIcon(Icons.PDF, "PDF");
      public static readonly ApplicationIcon Pericentral = AddNamedIcon(Icons.Pericentral, "Pericentral");
      public static readonly ApplicationIcon Periportal = AddNamedIcon(Icons.Periportal, "Periportal");
      public static readonly ApplicationIcon Permeability = AddNamedIcon(Icons.Permeability, "Permeability");
      public static readonly ApplicationIcon Pgp = AddNamedIcon(Icons.Pgp, "Pgp");
      public static readonly ApplicationIcon PKAnalysis = AddNamedIcon(Icons.PKAnalysis, "PKAnalysis");
      public static readonly ApplicationIcon PKML = AddNamedIcon(Icons.PKML, "PKML");
      public static readonly ApplicationIcon PKSim = AddNamedIcon(Icons.PKSim, "PKSim");
      public static readonly ApplicationIcon Plasma = AddNamedIcon(Icons.Plasma, "Plasma");
      public static readonly ApplicationIcon Population = AddNamedIcon(Icons.Population, "Population");
      public static readonly ApplicationIcon PopulationError = AddNamedIcon(Icons.PopulationError, "PopulationError");
      public static readonly ApplicationIcon PopulationFolder = AddNamedIcon(Icons.PopulationFolder, "PopulationFolder");
      public static readonly ApplicationIcon PopulationGreen = AddNamedIcon(Icons.PopulationGreen, "PopulationGreen");
      public static readonly ApplicationIcon PopulationRed = AddNamedIcon(Icons.PopulationRed, "PopulationRed");
      public static readonly ApplicationIcon PopulationSimulation = AddNamedIcon(Icons.PopulationSimulation, "PopulationSimulation");
      public static readonly ApplicationIcon PopulationSimulationGreen = AddNamedIcon(Icons.PopulationSimulationGreen, "PopulationSimulationGreen");
      public static readonly ApplicationIcon PopulationSimulationRed = AddNamedIcon(Icons.PopulationSimulationRed, "PopulationSimulationRed");
      public static readonly ApplicationIcon PopulationSimulationSettings = AddNamedIcon(Icons.SimulationSettings, "PopulationSimulationSettings");
      public static readonly ApplicationIcon PortalVein = AddNamedIcon(Icons.PortalVein, "PortalVein");
      public static readonly ApplicationIcon Previous = AddNamedIcon(Icons.Previous, "Previous");
      public static readonly ApplicationIcon ProjectDescription = AddNamedIcon(Icons.Description, "ProjectDescription");
      public static readonly ApplicationIcon Protein = AddNamedIcon(Icons.Protein, "Protein");
      public static readonly ApplicationIcon ProteinExpression = AddNamedIcon(Icons.ProteinExpression, "ProteinExpression");
      public static readonly ApplicationIcon ProteinExpressionError = AddNamedIcon(Icons.ProteinExpressionError, "ProteinExpressionError");
      public static readonly ApplicationIcon Protocol = AddNamedIcon(Icons.Protocol, "Protocol");
      public static readonly ApplicationIcon ProtocolFolder = AddNamedIcon(Icons.ProtocolFolder, "ProtocolFolder");
      public static readonly ApplicationIcon ProtocolGreen = AddNamedIcon(Icons.ProtocolGreen, "ProtocolGreen");
      public static readonly ApplicationIcon ProtocolRed = AddNamedIcon(Icons.ProtocolRed, "ProtocolRed");
      public static readonly ApplicationIcon R = AddNamedIcon(Icons.R, "R");
      public static readonly ApplicationIcon Rabbit = AddNamedIcon(Icons.Rabbit, "Rabbit");
      public static readonly ApplicationIcon RabbitGreen = AddNamedIcon(Icons.RabbitGreen, "RabbitGreen");
      public static readonly ApplicationIcon RabbitRed = AddNamedIcon(Icons.RabbitRed, "RabbitRed");
      public static readonly ApplicationIcon RangeAnalysis = AddNamedIcon(Icons.RangeAnalysis, "RangeAnalysis");
      public static readonly ApplicationIcon RangeAnalysisGreen = AddNamedIcon(Icons.RangeAnalysisGreen, "RangeAnalysisGreen");
      public static readonly ApplicationIcon RangeAnalysisRed = AddNamedIcon(Icons.RangeAnalysisRed, "RangeAnalysisRed");
      public static readonly ApplicationIcon Rat = AddNamedIcon(Icons.Rat, "Rat");
      public static readonly ApplicationIcon RatGreen = AddNamedIcon(Icons.RatGreen, "RatGreen");
      public static readonly ApplicationIcon RatRed = AddNamedIcon(Icons.RatRed, "RatRed");
      public static readonly ApplicationIcon Reaction = AddNamedIcon(Icons.Reaction, IconNames.REACTION);
      public static readonly ApplicationIcon ReactionFolder = AddNamedIcon(Icons.ReactionFolder, "ReactionFolder");
      public static readonly ApplicationIcon ReactionGreen = AddNamedIcon(Icons.ReactionGreen, "ReactionGreen");
      public static readonly ApplicationIcon ReactionRed = AddNamedIcon(Icons.ReactionRed, "ReactionRed");
      public static readonly ApplicationIcon Rectum = AddNamedIcon(Icons.Rectum, "Rectum");
      public static readonly ApplicationIcon Refresh = AddNamedIcon(Icons.Refresh, "Refresh");
      public static readonly ApplicationIcon RefreshAll = AddNamedIcon(Icons.RefreshAll, "RefreshAll");
      public static readonly ApplicationIcon RefreshSelected = AddNamedIcon(Icons.RefreshSelected, "RefreshSelected");
      public static readonly ApplicationIcon Remove = AddNamedIcon(Icons.Delete, "Remove");
      public static readonly ApplicationIcon Rename = AddNamedIcon(Icons.Rename, "Rename");
      public static readonly ApplicationIcon Reset = AddNamedIcon(Icons.Refresh, "Reset");
      public static readonly ApplicationIcon Run = AddNamedIcon(Icons.Run, "Run");
      public static readonly ApplicationIcon Saliva = AddNamedIcon(Icons.Saliva, "Saliva");
      public static readonly ApplicationIcon Save = AddNamedIcon(Icons.Save, "Save");
      public static readonly ApplicationIcon SaveAs = AddNamedIcon(Icons.SaveAs, "SaveAs");
      public static readonly ApplicationIcon SaveAsTemplate = AddNamedIcon(Icons.SaveAction, "SaveAsTemplate");
      public static readonly ApplicationIcon FavoritesSave = AddNamedIcon(Icons.FavoritesSave, "FavoritesSave");
      public static readonly ApplicationIcon ScaleFactor = AddNamedIcon(Icons.ScaleFactor, "ScaleFactor");
      public static readonly ApplicationIcon ScatterAnalysis = AddNamedIcon(Icons.ScatterAnalysis, "ScatterAnalysis");
      public static readonly ApplicationIcon ScatterAnalysisGreen = AddNamedIcon(Icons.ScatterAnalysisGreen, "ScatterAnalysisGreen");
      public static readonly ApplicationIcon ScatterAnalysisRed = AddNamedIcon(Icons.ScatterAnalysisRed, "ScatterAnalysisRed");
      public static readonly ApplicationIcon Search = AddNamedIcon(Icons.Search, "Search");
      public static readonly ApplicationIcon Settings = AddNamedIcon(Icons.Settings, "Settings");
      public static readonly ApplicationIcon Simulation = AddNamedIcon(Icons.Simulation, IconNames.SIMULATION);
      public static readonly ApplicationIcon SimulationExplorer = AddNamedIcon(Icons.SimulationExplorer, "SimulationExplorer");
      public static readonly ApplicationIcon SimulationFolder = AddNamedIcon(Icons.SimulationFolder, "SimulationFolder");
      public static readonly ApplicationIcon SimulationGreen = AddNamedIcon(Icons.SimulationGreen, "SimulationGreen");
      public static readonly ApplicationIcon SimulationRed = AddNamedIcon(Icons.SimulationRed, "SimulationRed");
      public static readonly ApplicationIcon SimulationSettings = AddNamedIcon(Icons.SimulationSettings, IconNames.SIMULATION_SETTINGS);
      public static readonly ApplicationIcon SimulationSettingsFolder = AddNamedIcon(Icons.SimulationSettingsFolder, "SimulationSettingsFolder");
      public static readonly ApplicationIcon SimulationSettingsGreen = AddNamedIcon(Icons.SimulationSettingsGreen, "SimulationSettingsGreen");
      public static readonly ApplicationIcon SimulationSettingsRed = AddNamedIcon(Icons.SimulationSettingsRed, "SimulationSettingsRed");
      public static readonly ApplicationIcon Skin = AddNamedIcon(Icons.Skin, "Skin");
      public static readonly ApplicationIcon SmallIntestine = AddNamedIcon(Icons.SmallIntestine, "SmallIntestine");
      public static readonly ApplicationIcon Solver = AddNamedIcon(Icons.Solver, "Solver");
      public static readonly ApplicationIcon SpatialStructure = AddNamedIcon(Icons.SpatialStructure, IconNames.SPATIAL_STRUCTURE);
      public static readonly ApplicationIcon SpatialStructureFolder = AddNamedIcon(Icons.SpatialStructureFolder, "SpatialStructureFolder");
      public static readonly ApplicationIcon SpatialStructureGreen = AddNamedIcon(Icons.SpatialStructureGreen, "SpatialStructureGreen");
      public static readonly ApplicationIcon SpatialStructureRed = AddNamedIcon(Icons.SpatialStructureRed, "SpatialStructureRed");
      public static readonly ApplicationIcon SpecificBinding = AddNamedIcon(Icons.SpecificBinding, "SpecificBinding");
      public static readonly ApplicationIcon Spleen = AddNamedIcon(Icons.Spleen, "Spleen");
      public static readonly ApplicationIcon Stomach = AddNamedIcon(Icons.Stomach, "Stomach");
      public static readonly ApplicationIcon Stop = AddNamedIcon(Icons.Stop, "Stop");
      public static readonly ApplicationIcon Subcutaneous = AddNamedIcon(Icons.Subcutaneous, "Subcutaneous");
      public static readonly ApplicationIcon SimulationComparison = AddNamedIcon(Icons.IndividualSimulationComparison, "SimulationComparison");
      public static readonly ApplicationIcon SytemSettings = AddNamedIcon(Icons.Population, "SytemSettings");
      public static readonly ApplicationIcon Time = AddNamedIcon(Icons.Time, "Time");
      public static readonly ApplicationIcon TimeProfileAnalysis = AddNamedIcon(Icons.TimeProfileAnalysis, "TimeProfileAnalysis");
      public static readonly ApplicationIcon TimeProfileAnalysisGreen = AddNamedIcon(Icons.TimeProfileAnalysisGreen, "TimeProfileAnalysisGreen");
      public static readonly ApplicationIcon TimeProfileAnalysisRed = AddNamedIcon(Icons.TimeProfileAnalysisRed, "TimeProfileAnalysisRed");
      public static readonly ApplicationIcon Transport = AddNamedIcon(Icons.Influx, "Transport");
      public static readonly ApplicationIcon Transporter = AddNamedIcon(Icons.Transporter, "Transporter");
      public static readonly ApplicationIcon TubularSecretion = AddNamedIcon(Icons.TubularSecretion, "TubularSecretion");
      public static readonly ApplicationIcon UncheckAll = AddNamedIcon(Icons.UncheckAll, "UncheckAll");
      public static readonly ApplicationIcon UncheckSelected = AddNamedIcon(Icons.UncheckSelected, "UncheckSelected");
      public static readonly ApplicationIcon UncompetitiveInhibition = AddNamedIcon(Icons.UncompetitiveInhibition, "UncompetitiveInhibition");
      public static readonly ApplicationIcon Undo = AddNamedIcon(Icons.Undo, "Undo");
      public static readonly ApplicationIcon UnitInformation = AddNamedIcon(Icons.UnitInformation, "UnitInformation");
      public static readonly ApplicationIcon Up = AddNamedIcon(Icons.Up, "Up");
      public static readonly ApplicationIcon Update = AddNamedIcon(Icons.Update, "Update");
      public static readonly ApplicationIcon UpperIleum = AddNamedIcon(Icons.UpperIleum, "UpperIleum");
      public static readonly ApplicationIcon UpperJejunum = AddNamedIcon(Icons.UpperJejunum, "UpperJejunum");
      public static readonly ApplicationIcon UserDefined = AddNamedIcon(Icons.Individual, "UserDefined");
      public static readonly ApplicationIcon UserSettings = AddNamedIcon(Icons.Settings, "UserSettings");
      public static readonly ApplicationIcon VascularEndothelium = AddNamedIcon(Icons.Endothelium, "VascularEndothelium");
      public static readonly ApplicationIcon VenousBlood = AddNamedIcon(Icons.VenousBlood, "VenousBlood");
      public static readonly ApplicationIcon Warning = AddNamedIcon(Icons.Notifications, "Warning");
      public static readonly ApplicationIcon ZoomIn = AddNamedIcon(Icons.ZoomIn, "ZoomIn");
      public static readonly ApplicationIcon ZoomOut = AddNamedIcon(Icons.ZoomOut, "ZoomOut");
      public static readonly ApplicationIcon CopySelection = AddNamedIcon(Icons.CopySelection, "Copy Selection");
      public static readonly ApplicationIcon JournalExportToWord = AddNamedIcon(Icons.JournalExportToWord, "JournalExportToWord");
      public static readonly ApplicationIcon SBML = AddNamedIcon(Icons.SBML, "SBML");
      public static readonly ApplicationIcon PageAdd = AddNamedIcon(Icons.PageAdd, "PageAdd");
      public static readonly ApplicationIcon IndividualSimulationComparison = AddNamedIcon(Icons.IndividualSimulationComparison, "IndividualSimulationComparison");
      public static readonly ApplicationIcon PopulationSimulationComparison = AddNamedIcon(Icons.PopulationSimulationComparison, "PopulationSimulationComparison");
      public static readonly ApplicationIcon JournalSelect = AddNamedIcon(Icons.JournalSelect, "JournalSelect");
      public static readonly ApplicationIcon HistoryExport = AddNamedIcon(Icons.HistoryExport, "HistoryExport");
      public static readonly ApplicationIcon SimulationClone = AddNamedIcon(Icons.SimulationClone, "SimulationClone");
      public static readonly ApplicationIcon AnalysesLoad = AddNamedIcon(Icons.AnalysesLoad, "AnalysesLoad");
      public static readonly ApplicationIcon AnalysesSave = AddNamedIcon(Icons.AnalysesSave, "AnalysesSave");
      public static readonly ApplicationIcon ObserverAdd = AddNamedIcon(Icons.ObserverAdd, "ObserverAdd");
      public static readonly ApplicationIcon Report = AddNamedIcon(Icons.Report, "Report");
      public static readonly ApplicationIcon ExportToCSV = AddNamedIcon(Icons.ExportToCSV, "ExportToCSV");
      public static readonly ApplicationIcon ExportToExcel = AddNamedIcon(Icons.ExportToExcel, "ExportToExcel");
      public static readonly ApplicationIcon PKAnalysesImportFromCSV = AddNamedIcon(Icons.PKAnalysesImportFromCSV, "PKAnalysesImportFromCSV");
      public static readonly ApplicationIcon Notifications = AddNamedIcon(Icons.Notifications, "Notifications");
      public static readonly ApplicationIcon SimulationLoad = AddNamedIcon(Icons.SimulationLoad, "SimulationLoad");
      public static readonly ApplicationIcon PKMLLoad = AddNamedIcon(Icons.PKMLLoad, "PKMLLoad");
      public static readonly ApplicationIcon PKMLSave = AddNamedIcon(Icons.PKMLSave, "PKMLSave");
      public static readonly ApplicationIcon Administration = AddNamedIcon(Icons.Administration, "Administration");
      public static readonly ApplicationIcon ComparisonFolder = AddNamedIcon(Icons.ComparisonFolder, "ComparisonFolder");
      public static readonly ApplicationIcon About = AddNamedIcon(Icons.About, "About");
      public static readonly ApplicationIcon ContainerSave = AddNamedIcon(Icons.ContainerSave, "ContainerSave");
      public static readonly ApplicationIcon EventSave = AddNamedIcon(Icons.EventSave, "EventSave");
      public static readonly ApplicationIcon AddFormulation = AddNamedIcon(Icons.FormulationAdd, "FormulationAdd");
      public static readonly ApplicationIcon FormulationLoad = AddNamedIcon(Icons.FormulationLoad, "FormulationLoad");
      public static readonly ApplicationIcon SaveFormulation = AddNamedIcon(Icons.FormulationSave, "FormulationSave");
      public static readonly ApplicationIcon MoleculeError = AddNamedIcon(Icons.MoleculeError, "MoleculeError");
      public static readonly ApplicationIcon SaveMolecule = AddNamedIcon(Icons.MoleculeSave, "MoleculeSave");
      public static readonly ApplicationIcon AddMoleculeStartValues = AddNamedIcon(Icons.MoleculeStartValuesAdd, "MoleculeStartVAluesAdd");
      public static readonly ApplicationIcon MoleculeStartValuesLoad = AddNamedIcon(Icons.MoleculeStartValuesLoad, "MoleculeStartValuesLoad");
      public static readonly ApplicationIcon SaveMoleculeStartValues = AddNamedIcon(Icons.MoleculeStartValuesSave, "MoleculeStartValuesSave");
      public static readonly ApplicationIcon SaveObserver = AddNamedIcon(Icons.ObserverSave, "ObserverSave");
      public static readonly ApplicationIcon AddParameterStartValues = AddNamedIcon(Icons.ParameterStartValuesAdd, "ParameterStartValuesAdd");
      public static readonly ApplicationIcon ParameterStartValuesLoad = AddNamedIcon(Icons.ParameterStartValuesLoad, "ParameterStartValuesLoad");
      public static readonly ApplicationIcon SaveParameterStartValues = AddNamedIcon(Icons.ParameterStartValuesSave, "ParameterStartValuesSave");
      public static readonly ApplicationIcon SaveReaction = AddNamedIcon(Icons.ReactionSave, "ReactionSave");
      public static readonly ApplicationIcon SaveSpatialStructure = AddNamedIcon(Icons.SpatialStructureSave, "SpatialStructureSave");
      public static readonly ApplicationIcon ParameterIdentificationFolder = AddNamedIcon(Icons.ParameterIdentificationFolder, "ParameterIdentificationFolder");
      public static readonly ApplicationIcon SensitivityAnalysisFolder = AddNamedIcon(Icons.SensitivityAnalysisFolder, "SensitivityAnalysisFolder");
      public static readonly ApplicationIcon SensitivityAnalysis = AddNamedIcon(Icons.SensitivityAnalysis, "SensitivityAnalysis");
      public static readonly ApplicationIcon ParameterIdentification = AddNamedIcon(Icons.ParameterIdentification, "ParameterIdentification");
      public static readonly ApplicationIcon ResidualHistogramAnalysis = AddNamedIcon(Icons.ResidualHistogramAnalysis, "ResidualHistogramAnalysis");
      public static readonly ApplicationIcon ResidualHistogramAnalysisGreen = AddNamedIcon(Icons.ResidualHistogramAnalysisGreen, "ResidualHistogramAnalysisGreen");
      public static readonly ApplicationIcon ResidualHistogramAnalysisRed = AddNamedIcon(Icons.ResidualHistogramAnalysisRed, "ResidualHistogramAnalysisRed");
      public static readonly ApplicationIcon ResidualVsTimeAnalysis = AddNamedIcon(Icons.ResidualVsTimeAnalysis, "ResidualVsTimeAnalysis");
      public static readonly ApplicationIcon ResidualVsTimeAnalysisGreen = AddNamedIcon(Icons.ResidualVsTimeAnalysisGreen, "ResidualVsTimeAnalysisGreen");
      public static readonly ApplicationIcon ResidualVsTimeAnalysisRed = AddNamedIcon(Icons.ResidualVsTimeAnalysisRed, "ResidualVsTimeAnalysisRed");
      public static readonly ApplicationIcon PredictedVsObservedAnalysis = AddNamedIcon(Icons.PredictedVsObservedAnalysis, "PredictedVsObservedAnalysis");
      public static readonly ApplicationIcon PredictedVsObservedAnalysisGreen = AddNamedIcon(Icons.PredictedVsObservedAnalysisGreen, "PredictedVsObservedAnalysisGreen");
      public static readonly ApplicationIcon PredictedVsObservedAnalysisRed = AddNamedIcon(Icons.PredictedVsObservedAnalysisRed, "PredictedVsObservedAnalysisRed");
      public static readonly ApplicationIcon DeleteFolderOnly = AddNamedIcon(Icons.DeleteFolderOnly, "DeleteFolderOnly");
      public static readonly ApplicationIcon CorrelationAnalysis = AddNamedIcon(Icons.CorrelationAnalysis, "CorrelationAnalysis");
      public static readonly ApplicationIcon CorrelationAnalysisGreen = AddNamedIcon(Icons.CorrelationAnalysisGreen, "CorrelationAnalysisGreen");
      public static readonly ApplicationIcon CorrelationAnalysisRed = AddNamedIcon(Icons.CorrelationAnalysisRed, "CorrelationAnalysisRed");
      public static readonly ApplicationIcon CovarianceAnalysis = AddNamedIcon(Icons.CovarianceAnalysis, "CovarianceAnalysis");
      public static readonly ApplicationIcon CovarianceAnalysisGreen = AddNamedIcon(Icons.CovarianceAnalysisGreen, "CovarianceAnalysisGreen");
      public static readonly ApplicationIcon CovarianceAnalysisRed = AddNamedIcon(Icons.CovarianceAnalysisRed, "CovarianceAnalysisRed");
      public static readonly ApplicationIcon DeleteSelected = AddNamedIcon(Icons.DeleteSelected, "DeleteSelected");
      public static readonly ApplicationIcon DeleteSourceNotDefined = AddNamedIcon(Icons.DeleteSourceNotDefined, "DeleteSourceNotDefined");
      public static readonly ApplicationIcon ExtendMoleculeStartValues = AddNamedIcon(Icons.ExtendMoleculeStartValues, "ExtendMoleculeStartValues");
      public static readonly ApplicationIcon MoleculeObserver = AddNamedIcon(Icons.MoleculeObserver, "MoleculeObserver");
      public static readonly ApplicationIcon OutputSelection = AddNamedIcon(Icons.OutputSelection, "OutputSelection");
      public static readonly ApplicationIcon PreviewOriginData = AddNamedIcon(Icons.PreviewOriginData, "PreviewOriginData");
      public static readonly ApplicationIcon Tree = AddNamedIcon(Icons.Tree, "Tree");
      public static readonly ApplicationIcon Diagram = AddNamedIcon(Icons.Diagram, "Diagram");
      public static readonly ApplicationIcon ContainerObserver = AddNamedIcon(Icons.ContainerObserver, "ContainerObserver");
      public static readonly ApplicationIcon ImportAll = AddNamedIcon(Icons.ImportAll, "ImportAll");
      public static readonly ApplicationIcon ReactionList = AddNamedIcon(Icons.ReactionList, "ReactionList");
      public static readonly ApplicationIcon SolverSettings = AddNamedIcon(Icons.SolverSettings, "SolverSettings");
      public static readonly ApplicationIcon Swap = AddNamedIcon(Icons.Swap, "Swap");
      public static readonly ApplicationIcon ImportAction = AddNamedIcon(Icons.ImportAction, "ImportAction");
      public static readonly ApplicationIcon FractionData = AddNamedIcon(Icons.FractionData, "FractionData");
      public static readonly ApplicationIcon ObservedDataForMolecule = AddNamedIcon(Icons.ObservedDataForMolecule, "ObservedDataForMolecule");
      public static readonly ApplicationIcon ParameterIdentificationVisualFeedback = AddNamedIcon(Icons.ParameterIdentificationVisualFeedback, "ParameterIdentificationVisualFeedback");
      public static readonly ApplicationIcon Results = AddNamedIcon(Icons.Results, "Results");
      public static readonly ApplicationIcon Formula = AddNamedIcon(Icons.Formula, "Formula");
      public static readonly ApplicationIcon UserDefinedVariability = AddNamedIcon(Icons.UserDefinedVariability, "UserDefinedVariability");
      public static readonly ApplicationIcon TimeProfileConfidenceInterval = AddNamedIcon(Icons.TimeProfileConfidenceInterval, "TimeProfileConfidenceInterval");
      public static readonly ApplicationIcon TimeProfilePredictionInterval = AddNamedIcon(Icons.TimeProfilePredictionInterval, "TimeProfilePredictionInterval");
      public static readonly ApplicationIcon TimeProfileVPCInterval = AddNamedIcon(Icons.TimeProfileVPCInterval, "TimeProfileVPCInterval");
      public static readonly ApplicationIcon PKAnalysesExportToCSV = AddNamedIcon(Icons.PKAnalysesExportToCSV, "PKAnalysesExportToCSV");
      public static readonly ApplicationIcon PKParameterSensitivityAnalysis = AddNamedIcon(Icons.PKParameterSensitivityAnalysis, "PKParameterSensitivityAnalysis");
      public static readonly ApplicationIcon SensitivityAnalysisVisualFeedback = AddNamedIcon(Icons.SensitivityAnalysisVisualFeedback, "SensitivityAnalysisVisualFeedback");
      public static readonly ApplicationIcon ValueOriginMethodAssumption = AddNamedIcon(Icons.ValueOriginMethodAssumption, "ValueOriginMethodAssumption");
      public static readonly ApplicationIcon ValueOriginMethodInVitro = AddNamedIcon(Icons.ValueOriginMethodInVitro, "ValueOriginMethodInVitro");
      public static readonly ApplicationIcon ValueOriginMethodInVivo = AddNamedIcon(Icons.ValueOriginMethodInVivo, "ValueOriginMethodInVivo");
      public static readonly ApplicationIcon ValueOriginMethodOther = AddNamedIcon(Icons.ValueOriginMethodOther, "ValueOriginMethodOther");
      public static readonly ApplicationIcon ValueOriginMethodManualFit = AddNamedIcon(Icons.ValueOriginMethodManualFit, "ValueOriginMethodManualFit");
      public static readonly ApplicationIcon ValueOriginMethodParameterIdentification = AddNamedIcon(Icons.ValueOriginMethodParameterIdentification, "ValueOriginMethodParameterIdentification");
      public static readonly ApplicationIcon ValueOriginMethodUnknown = AddNamedIcon(Icons.ValueOriginMethodUnknown, "ValueOriginMethodUnknown");
      public static readonly ApplicationIcon ValueOriginSourceDatabase = AddNamedIcon(Icons.ValueOriginSourceDatabase, "ValueOriginSourceDatabase");
      public static readonly ApplicationIcon ValueOriginSourceInternet = AddNamedIcon(Icons.ValueOriginSourceInternet, "ValueOriginSourceInternet");
      public static readonly ApplicationIcon ValueOriginSourceParameterIdentification = AddNamedIcon(Icons.ValueOriginSourceParameterIdentification, "ValueOriginSourceParameterIdentification");
      public static readonly ApplicationIcon ValueOriginSourcePublication = AddNamedIcon(Icons.ValueOriginSourcePublication, "ValueOriginSourcePublication");
      public static readonly ApplicationIcon ValueOriginSourceUnknown = AddNamedIcon(Icons.ValueOriginSourceUnknown, "ValueOriginSourceUnknown");
      public static readonly ApplicationIcon ValueOriginSourceOther = AddNamedIcon(Icons.ValueOriginSourceOther, "ValueOriginSourceOther");
      public static readonly ApplicationIcon Tag = AddNamedIcon(Icons.Tag, "Tag");
      public static readonly ApplicationIcon UserDefinedSpecies = AddNamedIcon(Icons.UserDefinedSpecies, "UserDefinedSpecies");
      public static readonly ApplicationIcon Properties = AddNamedIcon(Icons.Properties, "Properties");
      public static readonly ApplicationIcon Snapshot = AddNamedIcon(Icons.Snapshot, "Snapshot");
      public static readonly ApplicationIcon SnapshotExport = AddNamedIcon(Icons.SnapshotExport, "SnapshotExport");
      public static readonly ApplicationIcon SnapshotImport = AddNamedIcon(Icons.SnapshotImport, "SnapshotImport");
      public static readonly ApplicationIcon File = AddNamedIcon(Icons.ProjectNew, "File");
      public static readonly ApplicationIcon Redo = AddNamedIcon(Icons.Redo, "Redo");
      public static readonly ApplicationIcon ClearHistory = AddNamedIcon(Icons.Delete, "ClearHistory");
      public static readonly ApplicationIcon AmountObservedData = AddNamedIcon(Icons.AmountObservedData, "AmountObservedData");
      public static readonly ApplicationIcon AmountObservedDataForMolecule = AddNamedIcon(Icons.AmountObservedDataForMolecule, "AmountObservedDataForMolecule");


      // All icons should go at the end of the preceding list, before this delimiting icon - EmptyIcon
      public static readonly ApplicationIcon EmptyIcon = new ApplicationIcon((Icon)null);

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

      public static ApplicationIcon AddNamedIcon(byte[] icon, string iconName)
      {
         var name = iconName.ToUpperInvariant();
         var appIcon = new ApplicationIcon(icon)
         {
            IconName = name,
            Index = _allIcons.Count
         };

         _allIcons.Add(appIcon);
         return appIcon;
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