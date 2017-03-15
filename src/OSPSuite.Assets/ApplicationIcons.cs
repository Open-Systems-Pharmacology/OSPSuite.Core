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

      public static readonly ApplicationIcon Absorption = createIconFrom(Icons.Absorption, "Absorption");
      public static readonly ApplicationIcon ActiveEfflux = createIconFrom(Icons.Efflux, "ActiveEfflux");
      public static readonly ApplicationIcon ActiveInflux = createIconFrom(Icons.Influx, "ActiveInflux");
      public static readonly ApplicationIcon Add = createIconFrom(Icons.AddAction, "Add");
      public static readonly ApplicationIcon AddEnzyme = createIconFrom(Icons.Enzyme, "AddEnzyme");
      public static readonly ApplicationIcon AddProtein = createIconFrom(Icons.Protein, "AddProtein");
      public static readonly ApplicationIcon AddToJournal = createIconFrom(Icons.AddAction, "AddToJournal");
      public static readonly ApplicationIcon AgingPopulationSimulation = createIconFrom(Icons.AgingPopulationSimulation, "AgingPopulationSimulation");
      public static readonly ApplicationIcon AgingPopulationSimulationGreen = createIconFrom(Icons.AgingPopulationSimulationGreen, "AgingPopulationSimulationGreen");
      public static readonly ApplicationIcon AgingPopulationSimulationRed = createIconFrom(Icons.AgingPopulationSimulationRed, "AgingPopulationSimulationRed");
      public static readonly ApplicationIcon AgingSimulation = createIconFrom(Icons.AgingSimulation, "AgingSimulation");
      public static readonly ApplicationIcon AgingSimulationGreen = createIconFrom(Icons.AgingSimulationGreen, "AgingSimulationGreen");
      public static readonly ApplicationIcon AgingSimulationRed = createIconFrom(Icons.AgingSimulationRed, "AgingSimulationRed");
      public static readonly ApplicationIcon Application = createIconFrom(Icons.Application, "Application");
      public static readonly ApplicationIcon Applications = createIconFrom(Icons.Application, "Applications");
      public static readonly ApplicationIcon ApplicationsError = createIconFrom(Icons.ApplicationError, "ApplicationsError");
      public static readonly ApplicationIcon ApplicationSettings = createIconFrom(Icons.Settings, "ApplicationSettings");
      public static readonly ApplicationIcon ApplyAll = createIconFrom(Icons.ApplyAll, "ApplyAll");
      public static readonly ApplicationIcon ArterialBlood = createIconFrom(Icons.ArterialBlood, "ArterialBlood");
      public static readonly ApplicationIcon Back = createIconFrom(Icons.Back, "Back");
      public static readonly ApplicationIcon BasicPharmacochemistry = createIconFrom(Icons.BasicPharmacochemistry, "BasicPharmacochemistry");
      public static readonly ApplicationIcon BasicPharmacochemistryError = createIconFrom(Icons.BasicPharmacochemistryError, "BasicPharmacochemistryError");
      public static readonly ApplicationIcon Beagle = createIconFrom(Icons.Beagle, "Beagle");
      public static readonly ApplicationIcon BeagleGreen = createIconFrom(Icons.BeagleGreen, "BeagleGreen");
      public static readonly ApplicationIcon BeagleRed = createIconFrom(Icons.BeagleRed, "BeagleRed");
      public static readonly ApplicationIcon BiologicalProperties = createIconFrom(Icons.BiologicalProperties, "BiologicalProperties");
      public static readonly ApplicationIcon BiologicalPropertiesError = createIconFrom(Icons.BiologicalPropertiesError, "BiologicalPropertiesError");
      public static readonly ApplicationIcon Blood = createIconFrom(Icons.Blood, "Blood");
      public static readonly ApplicationIcon BloodCells = createIconFrom(Icons.BloodCells, "BloodCells");
      public static readonly ApplicationIcon Bone = createIconFrom(Icons.Bone, "Bone");
      public static readonly ApplicationIcon BoxWhiskerAnalysis = createIconFrom(Icons.BoxWhiskerAnalysis, "BoxWhiskerAnalysis");
      public static readonly ApplicationIcon BoxWhiskerAnalysisGreen = createIconFrom(Icons.BoxWhiskerAnalysisGreen, "BoxWhiskerAnalysisGreen");
      public static readonly ApplicationIcon BoxWhiskerAnalysisRed = createIconFrom(Icons.BoxWhiskerAnalysisRed, "BoxWhiskerAnalysisRed");
      public static readonly ApplicationIcon Brain = createIconFrom(Icons.Brain, "Brain");
      public static readonly ApplicationIcon BuildingBlockExplorer = createIconFrom(Icons.BuildingBlockExplorer, "BuildingBlockExplorer");
      public static readonly ApplicationIcon Caecum = createIconFrom(Icons.Caecum, "Caecum");
      public static readonly ApplicationIcon Cancel = createIconFrom(Icons.Cancel, "Cancel");
      public static readonly ApplicationIcon Cat = createIconFrom(Icons.Cat, "Cat");
      public static readonly ApplicationIcon CatGreen = createIconFrom(Icons.CatGreen, "CatGreen");
      public static readonly ApplicationIcon CatRed = createIconFrom(Icons.CatRed, "CatRed");
      public static readonly ApplicationIcon Cattle = createIconFrom(Icons.Cattle, "Cattle");
      public static readonly ApplicationIcon CattleGreen = createIconFrom(Icons.CattleGreen, "CattleGreen");
      public static readonly ApplicationIcon CattleRed = createIconFrom(Icons.CattleRed, "CattleRed");
      public static readonly ApplicationIcon Cecum = createIconFrom(Icons.Caecum, "Cecum");
      public static readonly ApplicationIcon CheckAll = createIconFrom(Icons.CheckAll, "CheckAll");
      public static readonly ApplicationIcon CheckSelected = createIconFrom(Icons.CheckSelected, "CheckSelected");
      public static readonly ApplicationIcon Clone = createIconFrom(Icons.SimulationClone, "Clone");
      public static readonly ApplicationIcon Close = createIconFrom(Icons.ProjectClose, "Close");
      public static readonly ApplicationIcon CloseProject = createIconFrom(Icons.ProjectClose, "CloseProject");
      public static readonly ApplicationIcon ClusterExport = createIconFrom(Icons.ClusterExport, "ClusterExport");
      public static readonly ApplicationIcon ColonAscendens = createIconFrom(Icons.ColonAscendens, "ColonAscendens");
      public static readonly ApplicationIcon ColonDescendens = createIconFrom(Icons.ColonDescendens, "ColonDescendens");
      public static readonly ApplicationIcon ColonSigmoid = createIconFrom(Icons.ColonSigmoid, "ColonSigmoid");
      public static readonly ApplicationIcon ColonTransversum = createIconFrom(Icons.ColonTransversum, "ColonTransversum");
      public static readonly ApplicationIcon Commit = createIconFrom(Icons.Commit, "Commit");
      public static readonly ApplicationIcon CommitRed = createIconFrom(Icons.CommitRed, "CommitRed");
      public static readonly ApplicationIcon SimulationComparisonFolder = createIconFrom(Icons.ComparisonFolder, "SimulationComparisonFolder");
      public static readonly ApplicationIcon CompetitiveInhibition = createIconFrom(Icons.CompetitiveInhibition, "CompetitiveInhibition");
      public static readonly ApplicationIcon Complex = createIconFrom(Icons.Complex, "Complex");
      public static readonly ApplicationIcon Compound = createIconFrom(Icons.Molecule, "Compound");
      public static readonly ApplicationIcon CompoundError = createIconFrom(Icons.MoleculeError, "CompoundError");
      public static readonly ApplicationIcon CompoundFolder = createIconFrom(Icons.MoleculeFolder, "CompoundFolder");
      public static readonly ApplicationIcon CompoundGreen = createIconFrom(Icons.MoleculeGreen, "CompoundGreen");
      public static readonly ApplicationIcon CompoundRed = createIconFrom(Icons.MoleculeRed, "CompoundRed");
      public static readonly ApplicationIcon SimulationConfigure = createIconFrom(Icons.SimulationConfigure, "SimulationConfigure");
      public static readonly ApplicationIcon Container = createIconFrom(Icons.Container, "Container");
      public static readonly ApplicationIcon Copy = createIconFrom(Icons.Copy, "Copy");
      public static readonly ApplicationIcon Create = createIconFrom(Icons.AddAction, "Create");
      public static readonly ApplicationIcon Debug = createIconFrom(Icons.Debug, "Debug");
      public static readonly ApplicationIcon ConfigureAndRun = createIconFrom(Icons.ConfigureAndRun, "ConfigureAndRun");
      public static readonly ApplicationIcon Delete = createIconFrom(Icons.Delete, "Delete");
      public static readonly ApplicationIcon Dermal = createIconFrom(Icons.Dermal, "Dermal");
      public static readonly ApplicationIcon Description = createIconFrom(Icons.Description, "Description");
      public static readonly ApplicationIcon Comparison = createIconFrom(Icons.Comparison, "Comparison");
      public static readonly ApplicationIcon Distribution = createIconFrom(Icons.Distribution, "Distribution");
      public static readonly ApplicationIcon DistributionCalculation = createIconFrom(Icons.DistributionCalculation, "DistributionCalculation");
      public static readonly ApplicationIcon Dog = createIconFrom(Icons.Dog, "Dog");
      public static readonly ApplicationIcon DogGreen = createIconFrom(Icons.DogGreen, "DogGreen");
      public static readonly ApplicationIcon DogRed = createIconFrom(Icons.DogRed, "DogRed");
      public static readonly ApplicationIcon Down = createIconFrom(Icons.Down, "Down");
      public static readonly ApplicationIcon Drug = createIconFrom(Icons.Molecule, "Drug");
      public static readonly ApplicationIcon Duodenum = createIconFrom(Icons.Duodenum, "Duodenum");
      public static readonly ApplicationIcon DxError = createIconFrom(Icons.ErrorProvider, "ErrorProvider");
      public static readonly ApplicationIcon Edit = createIconFrom(Icons.Edit, "Edit");
      public static readonly ApplicationIcon PageEdit = createIconFrom(Icons.PageEdit, "PageEdit");
      public static readonly ApplicationIcon Efflux = createIconFrom(Icons.Efflux, "Efflux");
      public static readonly ApplicationIcon Endothelium = createIconFrom(Icons.Endothelium, "Endothelium");
      public static readonly ApplicationIcon Enzyme = createIconFrom(Icons.Enzyme, "Enzyme");
      public static readonly ApplicationIcon Error = createIconFrom(Icons.Error, "Error");
      public static readonly ApplicationIcon ErrorHint = createIconFrom(Icons.ErrorHint, "ErrorHint");
      public static readonly ApplicationIcon Event = createIconFrom(Icons.Event, IconNames.EVENT);
      public static readonly ApplicationIcon EventFolder = createIconFrom(Icons.EventFolder, "EventFolder");
      public static readonly ApplicationIcon EventGreen = createIconFrom(Icons.EventGreen, "EventGreen");
      public static readonly ApplicationIcon EventGroup = createIconFrom(Icons.Event, IconNames.EVENT_GROUP);
      public static readonly ApplicationIcon EventRed = createIconFrom(Icons.EventRed, "EventRed");
      public static readonly ApplicationIcon Excel = createIconFrom(Icons.ObservedData, "Excel");
      public static readonly ApplicationIcon Excretion = createIconFrom(Icons.Excretion, "Excretion");
      public static readonly ApplicationIcon Exit = createIconFrom(Icons.Exit, "Exit");
      public static readonly ApplicationIcon ExpertParameters = createIconFrom(Icons.Parameters, "ExpertParameters");
      public static readonly ApplicationIcon PopulationExportToCSV = createIconFrom(Icons.PopulationExportToCSV, "PopulationExportToCSV");
      public static readonly ApplicationIcon ExportToPDF = createIconFrom(Icons.PDF, "ExportToPDF");
      public static readonly ApplicationIcon ExtendParameterStartValues = createIconFrom(Icons.ExtendParameterStartValues, "ExtendParameterStartValues");
      public static readonly ApplicationIcon ExtracellularMembrane = createIconFrom(Icons.ExtracellularMembrane, "ExtracellularMembrane");
      public static readonly ApplicationIcon Fat = createIconFrom(Icons.Fat, "Fat");
      public static readonly ApplicationIcon Favorites = createIconFrom(Icons.Favorites, "Favorites");
      public static readonly ApplicationIcon FitToPage = createIconFrom(Icons.FitToPage, "FitToPage");
      public static readonly ApplicationIcon Folder = createIconFrom(Icons.Folder, "Folder");
      public static readonly ApplicationIcon Formulation = createIconFrom(Icons.Formulation, "Formulation");
      public static readonly ApplicationIcon FormulationFolder = createIconFrom(Icons.FormulationFolder, "FormulationFolder");
      public static readonly ApplicationIcon FormulationGreen = createIconFrom(Icons.FormulationGreen, "FormulationGreen");
      public static readonly ApplicationIcon FormulationRed = createIconFrom(Icons.FormulationRed, "FormulationRed");
      public static readonly ApplicationIcon Forward = createIconFrom(Icons.Forward, "Forward");
      public static readonly ApplicationIcon Gallbladder = createIconFrom(Icons.GallBladder, "Gallbladder");
      public static readonly ApplicationIcon GITract = createIconFrom(Icons.GITract, "GI-Tract");
      public static readonly ApplicationIcon GlomerularFiltration = createIconFrom(Icons.GlomerularFiltration, "GlomerularFiltration");
      public static readonly ApplicationIcon Gonads = createIconFrom(Icons.Gonads, "Gonads");
      public static readonly ApplicationIcon GoTo = createIconFrom(Icons.GoTo, "GoTo");
      public static readonly ApplicationIcon GroupBy = createIconFrom(Icons.GroupBy, "GroupBy");
      public static readonly ApplicationIcon Heart = createIconFrom(Icons.Heart, "Heart");
      public static readonly ApplicationIcon Help = createIconFrom(Icons.Help, "Help");
      public static readonly ApplicationIcon Histogram = createIconFrom(Icons.Histogram, "Histogram");
      public static readonly ApplicationIcon History = createIconFrom(Icons.History, "History");
      public static readonly ApplicationIcon Human = createIconFrom(Icons.Human, "Human");
      public static readonly ApplicationIcon HumanGreen = createIconFrom(Icons.HumanGreen, "HumanGreen");
      public static readonly ApplicationIcon HumanRed = createIconFrom(Icons.HumanRed, "HumanRed");
      public static readonly ApplicationIcon Import = createIconFrom(Icons.ObservedData, "Import");
      public static readonly ApplicationIcon MoleculeStartValuesImport = createIconFrom(Icons.MoleculeStartValuesImport, "MoleculeStartValuesImport");
      public static readonly ApplicationIcon ParameterStartValuesImport = createIconFrom(Icons.ParameterStartValuesImport, "ParameterStartValuesImport");
      public static readonly ApplicationIcon ImportPopulation = createIconFrom(Icons.ImportPopulation, "ImportPopulation");
      public static readonly ApplicationIcon PopulationSimulationLoad = createIconFrom(Icons.PopulationSimulationLoad, "PopulationSimulationLoad");
      public static readonly ApplicationIcon ResultsImportFromCSV = createIconFrom(Icons.ResultsImportFromCSV, "ResultsImportFromCSV");
      public static readonly ApplicationIcon IndividualSimulationLoad = createIconFrom(Icons.IndividualSimulationLoad, "IndividualSimulationLoad");
      public static readonly ApplicationIcon Individual = createIconFrom(Icons.Individual, "Individual");
      public static readonly ApplicationIcon IndividualError = createIconFrom(Icons.IndividualError, "IndividualError");
      public static readonly ApplicationIcon IndividualFolder = createIconFrom(Icons.IndividualFolder, "IndividualFolder");
      public static readonly ApplicationIcon IndividualGreen = createIconFrom(Icons.IndividualGreen, "IndividualGreen");
      public static readonly ApplicationIcon IndividualRed = createIconFrom(Icons.IndividualRed, "IndividualRed");
      public static readonly ApplicationIcon ScaleIndividual = createIconFrom(Icons.ScaleIndividual, "ScaleIndividual");
      public static readonly ApplicationIcon Induction = createIconFrom(Icons.Induction, "Induction");
      public static readonly ApplicationIcon Influx = createIconFrom(Icons.Influx, "Influx");
      public static readonly ApplicationIcon Info = createIconFrom(Icons.Info, "Info");
      public static readonly ApplicationIcon Inhibition = createIconFrom(Icons.Inhibition, "Inhibition");
      public static readonly ApplicationIcon Interstitial = createIconFrom(Icons.Interstitial, "Interstitial");
      public static readonly ApplicationIcon Intracellular = createIconFrom(Icons.Intracellular, "Intracellular");
      public static readonly ApplicationIcon Intravenous = createIconFrom(Icons.Intravenous, "Intravenous");
      public static readonly ApplicationIcon IntravenousBolus = createIconFrom(Icons.IntravenousBolus, "IntravenousBolus");
      public static readonly ApplicationIcon IrreversibleInhibition = createIconFrom(Icons.IrreversibleInhibition, "IrreversibleInhibition");
      public static readonly ApplicationIcon Journal = createIconFrom(Icons.Journal, "Journal");
      public static readonly ApplicationIcon Page = createIconFrom(Icons.Page, "Page");
      public static readonly ApplicationIcon JournalDiagram = createIconFrom(Icons.JournalDiagram, "JournalDiagram");
      public static readonly ApplicationIcon Kidney = createIconFrom(Icons.Kidney, "Kidney");
      public static readonly ApplicationIcon LabelAdd = createIconFrom(Icons.LabelAdd, "LabelAdd");
      public static readonly ApplicationIcon LargeIntestine = createIconFrom(Icons.LargeIntestine, "LargeIntestine");
      public static readonly ApplicationIcon License = createIconFrom(Icons.LicenseRegister, "License");
      public static readonly ApplicationIcon Liver = createIconFrom(Icons.Liver, "Liver");
      public static readonly ApplicationIcon Load = createIconFrom(Icons.LoadAction, "Load");
      public static readonly ApplicationIcon ContainerLoad = createIconFrom(Icons.ContainerLoad, "ContainerLoad");
      public static readonly ApplicationIcon EventLoad = createIconFrom(Icons.EventLoad, "EventLoad");
      public static readonly ApplicationIcon FavoritesLoad = createIconFrom(Icons.FavoritesLoad, "FavoritesLoad");
      public static readonly ApplicationIcon LoadFromTemplate = createIconFrom(Icons.LoadAction, "LoadFromTemplate");
      public static readonly ApplicationIcon MoleculeLoad = createIconFrom(Icons.MoleculeLoad, "MoleculeLoad");
      public static readonly ApplicationIcon ObserverLoad = createIconFrom(Icons.ObserverLoad, "ObserverLoad");
      public static readonly ApplicationIcon ReactionLoad = createIconFrom(Icons.ReactionLoad, "ReactionLoad");
      public static readonly ApplicationIcon SpatialStructureLoad = createIconFrom(Icons.SpatialStructureLoad, "SpatialStructureLoad");
      public static readonly ApplicationIcon LowerIleum = createIconFrom(Icons.LowerIleum, "LowerIleum");
      public static readonly ApplicationIcon LowerJejunum = createIconFrom(Icons.LowerJejunum, "LowerJejunum");
      public static readonly ApplicationIcon Lumen = createIconFrom(Icons.Lumen, "Lumen");
      public static readonly ApplicationIcon LumenCaecum = createIconFrom(Icons.Caecum, "Lumen-Caecum");
      public static readonly ApplicationIcon LumenColonAscendens = createIconFrom(Icons.ColonAscendens, "Lumen-ColonAscendens");
      public static readonly ApplicationIcon LumenColonDescendens = createIconFrom(Icons.ColonDescendens, "Lumen-ColonDescendens");
      public static readonly ApplicationIcon LumenColonSigmoid = createIconFrom(Icons.ColonSigmoid, "Lumen-ColonSigmoid");
      public static readonly ApplicationIcon LumenColonTransversum = createIconFrom(Icons.ColonTransversum, "Lumen-ColonTransversum");
      public static readonly ApplicationIcon LumenDuodenum = createIconFrom(Icons.Duodenum, "Lumen-Duodenum");
      public static readonly ApplicationIcon LumenLowerIleum = createIconFrom(Icons.LowerIleum, "Lumen-LowerIleum");
      public static readonly ApplicationIcon LumenLowerJejunum = createIconFrom(Icons.LowerJejunum, "Lumen-LowerJejunum");
      public static readonly ApplicationIcon LumenRectum = createIconFrom(Icons.Rectum, "Lumen-Rectum");
      public static readonly ApplicationIcon LumenStomach = createIconFrom(Icons.Stomach, "Lumen-Stomach");
      public static readonly ApplicationIcon LumenUpperIleum = createIconFrom(Icons.UpperIleum, "Lumen-UpperIleum");
      public static readonly ApplicationIcon LumenUpperJejunum = createIconFrom(Icons.UpperJejunum, "Lumen-UpperJejunum");
      public static readonly ApplicationIcon Lung = createIconFrom(Icons.Lung, "Lung");
      public static readonly ApplicationIcon ProjectDisplayUnitsConfigure = createIconFrom(Icons.ProjectDisplayUnitsConfigure, "ProjectDisplayUnitsConfigure");
      public static readonly ApplicationIcon UserDisplayUnitsConfigure = createIconFrom(Icons.UserDisplayUnitsConfigure, "UserDisplayUnitsConfigure");
      public static readonly ApplicationIcon Matlab = createIconFrom(Icons.Matlab, "Matlab");
      public static readonly ApplicationIcon MetaData = createIconFrom(Icons.MetaData, "MetaData");
      public static readonly ApplicationIcon MetaDataAndUnitInformation = createIconFrom(Icons.MetaData, "MetaDataAndUnitInformation");
      public static readonly ApplicationIcon Merge = createIconFrom(Icons.Merge, "Merge");
      public static readonly ApplicationIcon MergePopulation = createIconFrom(Icons.MergePopulation, "MergePopulation");
      public static readonly ApplicationIcon Metabolism = createIconFrom(Icons.Metabolism, "Metabolism");
      public static readonly ApplicationIcon Metabolite = createIconFrom(Icons.Metabolite, "Metabolite");
      public static readonly ApplicationIcon Minipig = createIconFrom(Icons.Minipig, "Minipig");
      public static readonly ApplicationIcon MinipigGreen = createIconFrom(Icons.MinipigGreen, "MinipigGreen");
      public static readonly ApplicationIcon MinipigRed = createIconFrom(Icons.MinipigRed, "MinipigRed");
      public static readonly ApplicationIcon MissingData = createIconFrom(Icons.MissingData, "MissingData");
      public static readonly ApplicationIcon MissingMetaData = createIconFrom(Icons.MissingMetaData, "MissingMetaData");
      public static readonly ApplicationIcon MissingUnitInformation = createIconFrom(Icons.MissingUnitInformation, "MissingUnitInformation");
      public static readonly ApplicationIcon MixedInhibition = createIconFrom(Icons.MixedInhibition, "MixedInhibition");
      public static readonly ApplicationIcon MoBi = createIconFrom(Icons.MoBi, "MoBi");
      public static readonly ApplicationIcon MoBiSimulation = createIconFrom(Icons.PKML, "MoBiSimulation");
      public static readonly ApplicationIcon ModelStructure = createIconFrom(Icons.ModelStructure, "ModelStructure");
      public static readonly ApplicationIcon ModelStructureError = createIconFrom(Icons.ModelStructureError, "ModelStructureError");
      public static readonly ApplicationIcon Molecule = createIconFrom(Icons.Molecule, IconNames.MOLECULE);
      public static readonly ApplicationIcon MoleculeFolder = createIconFrom(Icons.MoleculeFolder, "MoleculeFolder");
      public static readonly ApplicationIcon MoleculeGreen = createIconFrom(Icons.MoleculeGreen, "MoleculeGreen");
      public static readonly ApplicationIcon MoleculeRed = createIconFrom(Icons.MoleculeRed, "MoleculeRed");
      public static readonly ApplicationIcon MoleculeStartValues = createIconFrom(Icons.MoleculeStartValues, IconNames.MOLECULE_START_VALUES);
      public static readonly ApplicationIcon MoleculeStartValuesFolder = createIconFrom(Icons.MoleculeStartValuesFolder, "MoleculeStartValuesFolder");
      public static readonly ApplicationIcon MoleculeStartValuesGreen = createIconFrom(Icons.MoleculeStartValuesGreen, "MoleculeStartValuesGreen");
      public static readonly ApplicationIcon MoleculeStartValuesRed = createIconFrom(Icons.MoleculeStartValuesRed, "MoleculeStartValuesRed");
      public static readonly ApplicationIcon Monkey = createIconFrom(Icons.Monkey, "Monkey");
      public static readonly ApplicationIcon MonkeyGreen = createIconFrom(Icons.MonkeyGreen, "MonkeyGreen");
      public static readonly ApplicationIcon MonkeyRed = createIconFrom(Icons.MonkeyRed, "MonkeyRed");
      public static readonly ApplicationIcon Mouse = createIconFrom(Icons.Mouse, "Mouse");
      public static readonly ApplicationIcon MouseGreen = createIconFrom(Icons.MouseGreen, "MouseGreen");
      public static readonly ApplicationIcon MouseRed = createIconFrom(Icons.MouseRed, "MouseRed");
      public static readonly ApplicationIcon Muscle = createIconFrom(Icons.Muscle, "Muscle");
      public static readonly ApplicationIcon AmountProjectNew = createIconFrom(Icons.ProjectNewAmount, "AmountProjectNew");
      public static readonly ApplicationIcon ConcentrationProjectNew = createIconFrom(Icons.ProjectNewConcentration, "ConcentrationProjectNew");
      public static readonly ApplicationIcon ContainerAdd = createIconFrom(Icons.ContainerAdd, "ContainerAdd");
      public static readonly ApplicationIcon EventAdd = createIconFrom(Icons.EventAdd, "EventAdd");
      public static readonly ApplicationIcon MoleculeAdd = createIconFrom(Icons.MoleculeAdd, "MoleculeAdd");
      public static readonly ApplicationIcon PKSimMoleculeAdd = createIconFrom(Icons.MoleculeAdd, "PKSimMoleculeAdd");
      public static readonly ApplicationIcon ProjectNew = createIconFrom(Icons.ProjectNew, "ProjectNew");
      public static readonly ApplicationIcon ReactionAdd = createIconFrom(Icons.ReactionAdd, "ReactionAdd");
      public static readonly ApplicationIcon SpatialStructureAdd = createIconFrom(Icons.SpatialStructureAdd, "SpatialStructureAdd");
      public static readonly ApplicationIcon Next = createIconFrom(Icons.Next, "Next");
      public static readonly ApplicationIcon NonCompetitiveInhibition = createIconFrom(Icons.NonCompetitiveInhibition, "NonCompetitiveInhibition");
      public static readonly ApplicationIcon ObservedData = createIconFrom(Icons.ObservedData, IconNames.OBSERVED_DATA);
      public static readonly ApplicationIcon ObservedDataCompound = createIconFrom(Icons.ObservedDataCompound, "ObservedDataCompound");
      public static readonly ApplicationIcon ObservedDataFolder = createIconFrom(Icons.ObservedDataFolder, "ObservedDataFolder");
      public static readonly ApplicationIcon Observer = createIconFrom(Icons.Observer, IconNames.OBSERVER);
      public static readonly ApplicationIcon ObserverFolder = createIconFrom(Icons.ObserverFolder, "ObserverFolder");
      public static readonly ApplicationIcon ObserverGreen = createIconFrom(Icons.ObserverGreen, "ObserverGreen");
      public static readonly ApplicationIcon ObserverRed = createIconFrom(Icons.ObserverRed, "ObserverRed");
      public static readonly ApplicationIcon OK = createIconFrom(Icons.OK, "OK");
      public static readonly ApplicationIcon ProjectOpen = createIconFrom(Icons.ProjectOpen, "ProjectOpen");
      public static readonly ApplicationIcon Oral = createIconFrom(Icons.Oral, "Oral");
      public static readonly ApplicationIcon Organism = createIconFrom(Icons.Organism, "Organism");
      public static readonly ApplicationIcon Other = createIconFrom(Icons.Help, "Other");
      public static readonly ApplicationIcon OtherProtein = createIconFrom(Icons.Protein, "OtherProtein");
      public static readonly ApplicationIcon OutputInterval = createIconFrom(Icons.OutputInterval, "OutputInterval");
      public static readonly ApplicationIcon Pancreas = createIconFrom(Icons.Pancreas, "Pancreas");
      public static readonly ApplicationIcon Parameter = createIconFrom(Icons.Parameters, IconNames.PARAMETER);
      public static readonly ApplicationIcon ParameterDistribution = createIconFrom(Icons.Histogram, "ParameterDistribution");
      public static readonly ApplicationIcon Parameters = createIconFrom(Icons.Parameters, "Parameters");
      public static readonly ApplicationIcon ParametersError = createIconFrom(Icons.ParametersError, "ParametersError");
      public static readonly ApplicationIcon ParameterStartValueGreen = createIconFrom(Icons.ParameterStartValuesGreen, "ParameterStartValuesGreen");
      public static readonly ApplicationIcon ParameterStartValues = createIconFrom(Icons.ParameterStartValues, IconNames.PARAMETER_START_VALUES);
      public static readonly ApplicationIcon ParameterStartValuesFolder = createIconFrom(Icons.ParameterStartValuesFolder, "ParameterStartValuesFolder");
      public static readonly ApplicationIcon ParameterStartValuesRed = createIconFrom(Icons.ParameterStartValuesRed, "ParameterStartValuesRed");
      public static readonly ApplicationIcon PassiveTransport = createIconFrom(Icons.PassiveTransport, IconNames.PASSIVE_TRANSPORT);
      public static readonly ApplicationIcon PassiveTransportFolder = createIconFrom(Icons.PassiveTransportFolder, "PassiveTransportFolder");
      public static readonly ApplicationIcon PassiveTransportGreen = createIconFrom(Icons.PassiveTransportGreen, "PassiveTransportGreen");
      public static readonly ApplicationIcon PassiveTransportRed = createIconFrom(Icons.PassiveTransportRed, "PassiveTransportRed");
      public static readonly ApplicationIcon Paste = createIconFrom(Icons.Paste, "Paste");
      public static readonly ApplicationIcon PDF = createIconFrom(Icons.PDF, "PDF");
      public static readonly ApplicationIcon Pericentral = createIconFrom(Icons.Pericentral, "Pericentral");
      public static readonly ApplicationIcon Periportal = createIconFrom(Icons.Periportal, "Periportal");
      public static readonly ApplicationIcon Permeability = createIconFrom(Icons.Permeability, "Permeability");
      public static readonly ApplicationIcon Pgp = createIconFrom(Icons.Pgp, "Pgp");
      public static readonly ApplicationIcon PKAnalysis = createIconFrom(Icons.PKAnalysis, "PKAnalysis");
      public static readonly ApplicationIcon PKML = createIconFrom(Icons.PKML, "PKML");
      public static readonly ApplicationIcon PKSim = createIconFrom(Icons.PKSim, "PKSim");
      public static readonly ApplicationIcon Plasma = createIconFrom(Icons.Plasma, "Plasma");
      public static readonly ApplicationIcon Population = createIconFrom(Icons.Population, "Population");
      public static readonly ApplicationIcon PopulationError = createIconFrom(Icons.PopulationError, "PopulationError");
      public static readonly ApplicationIcon PopulationFolder = createIconFrom(Icons.PopulationFolder, "PopulationFolder");
      public static readonly ApplicationIcon PopulationGreen = createIconFrom(Icons.PopulationGreen, "PopulationGreen");
      public static readonly ApplicationIcon PopulationRed = createIconFrom(Icons.PopulationRed, "PopulationRed");
      public static readonly ApplicationIcon PopulationSimulation = createIconFrom(Icons.PopulationSimulation, "PopulationSimulation");
      public static readonly ApplicationIcon PopulationSimulationGreen = createIconFrom(Icons.PopulationSimulationGreen, "PopulationSimulationGreen");
      public static readonly ApplicationIcon PopulationSimulationRed = createIconFrom(Icons.PopulationSimulationRed, "PopulationSimulationRed");
      public static readonly ApplicationIcon PopulationSimulationSettings = createIconFrom(Icons.SimulationSettings, "PopulationSimulationSettings");
      public static readonly ApplicationIcon PortalVein = createIconFrom(Icons.PortalVein, "PortalVein");
      public static readonly ApplicationIcon Previous = createIconFrom(Icons.Previous, "Previous");
      public static readonly ApplicationIcon ProjectDescription = createIconFrom(Icons.ObservedData, "ProjectDescription");
      public static readonly ApplicationIcon Protein = createIconFrom(Icons.Protein, "Protein");
      public static readonly ApplicationIcon ProteinExpression = createIconFrom(Icons.ProteinExpression, "ProteinExpression");
      public static readonly ApplicationIcon ProteinExpressionError = createIconFrom(Icons.ProteinExpressionError, "ProteinExpressionError");
      public static readonly ApplicationIcon Protocol = createIconFrom(Icons.Protocol, "Protocol");
      public static readonly ApplicationIcon ProtocolFolder = createIconFrom(Icons.ProtocolFolder, "ProtocolFolder");
      public static readonly ApplicationIcon ProtocolGreen = createIconFrom(Icons.ProtocolGreen, "ProtocolGreen");
      public static readonly ApplicationIcon ProtocolRed = createIconFrom(Icons.ProtocolRed, "ProtocolRed");
      public static readonly ApplicationIcon R = createIconFrom(Icons.R, "R");
      public static readonly ApplicationIcon Rabbit = createIconFrom(Icons.Rabbit, "Rabbit");
      public static readonly ApplicationIcon RabbitGreen = createIconFrom(Icons.RabbitGreen, "RabbitGreen");
      public static readonly ApplicationIcon RabbitRed = createIconFrom(Icons.RabbitRed, "RabbitRed");
      public static readonly ApplicationIcon RangeAnalysis = createIconFrom(Icons.RangeAnalysis, "RangeAnalysis");
      public static readonly ApplicationIcon RangeAnalysisGreen = createIconFrom(Icons.RangeAnalysisGreen, "RangeAnalysisGreen");
      public static readonly ApplicationIcon RangeAnalysisRed = createIconFrom(Icons.RangeAnalysisRed, "RangeAnalysisRed");
      public static readonly ApplicationIcon Rat = createIconFrom(Icons.Rat, "Rat");
      public static readonly ApplicationIcon RatGreen = createIconFrom(Icons.RatGreen, "RatGreen");
      public static readonly ApplicationIcon RatRed = createIconFrom(Icons.RatRed, "RatRed");
      public static readonly ApplicationIcon Reaction = createIconFrom(Icons.Reaction, IconNames.REACTION);
      public static readonly ApplicationIcon ReactionFolder = createIconFrom(Icons.ReactionFolder, "ReactionFolder");
      public static readonly ApplicationIcon ReactionGreen = createIconFrom(Icons.ReactionGreen, "ReactionGreen");
      public static readonly ApplicationIcon ReactionRed = createIconFrom(Icons.ReactionRed, "ReactionRed");
      public static readonly ApplicationIcon Rectum = createIconFrom(Icons.Rectum, "Rectum");
      public static readonly ApplicationIcon Refresh = createIconFrom(Icons.Refresh, "Refresh");
      public static readonly ApplicationIcon RefreshAll = createIconFrom(Icons.RefreshAll, "RefreshAll");
      public static readonly ApplicationIcon RefreshSelected = createIconFrom(Icons.RefreshSelected, "RefreshSelected");
      public static readonly ApplicationIcon Remove = createIconFrom(Icons.Delete, "Remove");
      public static readonly ApplicationIcon Rename = createIconFrom(Icons.Rename, "Rename");
      public static readonly ApplicationIcon Reset = createIconFrom(Icons.Refresh, "Reset");
      public static readonly ApplicationIcon Run = createIconFrom(Icons.Run, "Run");
      public static readonly ApplicationIcon Saliva = createIconFrom(Icons.Saliva, "Saliva");
      public static readonly ApplicationIcon Save = createIconFrom(Icons.Save, "Save");
      public static readonly ApplicationIcon SaveAs = createIconFrom(Icons.SaveAs, "SaveAs");
      public static readonly ApplicationIcon SaveAsTemplate = createIconFrom(Icons.SaveAction, "SaveAsTemplate");
      public static readonly ApplicationIcon FavoritesSave = createIconFrom(Icons.FavoritesSave, "FavoritesSave");
      public static readonly ApplicationIcon ScaleFactor = createIconFrom(Icons.ScaleFactor, "ScaleFactor");
      public static readonly ApplicationIcon ScatterAnalysis = createIconFrom(Icons.ScatterAnalysis, "ScatterAnalysis");
      public static readonly ApplicationIcon ScatterAnalysisGreen = createIconFrom(Icons.ScatterAnalysisGreen, "ScatterAnalysisGreen");
      public static readonly ApplicationIcon ScatterAnalysisRed = createIconFrom(Icons.ScatterAnalysisRed, "ScatterAnalysisRed");
      public static readonly ApplicationIcon Search = createIconFrom(Icons.Search, "Search");
      public static readonly ApplicationIcon Settings = createIconFrom(Icons.Settings, "Settings");
      public static readonly ApplicationIcon Simulation = createIconFrom(Icons.Simulation, IconNames.SIMULATION);
      public static readonly ApplicationIcon SimulationExplorer = createIconFrom(Icons.SimulationExplorer, "SimulationExplorer");
      public static readonly ApplicationIcon SimulationFolder = createIconFrom(Icons.SimulationFolder, "SimulationFolder");
      public static readonly ApplicationIcon SimulationGreen = createIconFrom(Icons.SimulationGreen, "SimulationGreen");
      public static readonly ApplicationIcon SimulationRed = createIconFrom(Icons.SimulationRed, "SimulationRed");
      public static readonly ApplicationIcon SimulationSettings = createIconFrom(Icons.SimulationSettings, IconNames.SIMULATION_SETTINGS);
      public static readonly ApplicationIcon SimulationSettingsFolder = createIconFrom(Icons.SimulationSettingsFolder, "SimulationSettingsFolder");
      public static readonly ApplicationIcon SimulationSettingsGreen = createIconFrom(Icons.SimulationSettingsGreen, "SimulationSettingsGreen");
      public static readonly ApplicationIcon SimulationSettingsRed = createIconFrom(Icons.SimulationSettingsRed, "SimulationSettingsRed");
      public static readonly ApplicationIcon Skin = createIconFrom(Icons.Skin, "Skin");
      public static readonly ApplicationIcon SmallIntestine = createIconFrom(Icons.SmallIntestine, "SmallIntestine");
      public static readonly ApplicationIcon Solver = createIconFrom(Icons.Solver, "Solver");
      public static readonly ApplicationIcon SpatialStructure = createIconFrom(Icons.SpatialStructure, IconNames.SPATIAL_STRUCTURE);
      public static readonly ApplicationIcon SpatialStructureFolder = createIconFrom(Icons.SpatialStructureFolder, "SpatialStructureFolder");
      public static readonly ApplicationIcon SpatialStructureGreen = createIconFrom(Icons.SpatialStructureGreen, "SpatialStructureGreen");
      public static readonly ApplicationIcon SpatialStructureRed = createIconFrom(Icons.SpatialStructureRed, "SpatialStructureRed");
      public static readonly ApplicationIcon SpecificBinding = createIconFrom(Icons.SpecificBinding, "SpecificBinding");
      public static readonly ApplicationIcon Spleen = createIconFrom(Icons.Spleen, "Spleen");
      public static readonly ApplicationIcon Stomach = createIconFrom(Icons.Stomach, "Stomach");
      public static readonly ApplicationIcon Stop = createIconFrom(Icons.Stop, "Stop");
      public static readonly ApplicationIcon Subcutaneous = createIconFrom(Icons.Subcutaneous, "Subcutaneous");
      public static readonly ApplicationIcon SimulationComparison = createIconFrom(Icons.IndividualSimulationComparison, "SimulationComparison");
      public static readonly ApplicationIcon SytemSettings = createIconFrom(Icons.Population, "SytemSettings");
      public static readonly ApplicationIcon Time = createIconFrom(Icons.Time, "Time");
      public static readonly ApplicationIcon TimeProfileAnalysis = createIconFrom(Icons.TimeProfileAnalysis, "TimeProfileAnalysis");
      public static readonly ApplicationIcon TimeProfileAnalysisGreen = createIconFrom(Icons.TimeProfileAnalysisGreen, "TimeProfileAnalysisGreen");
      public static readonly ApplicationIcon TimeProfileAnalysisRed = createIconFrom(Icons.TimeProfileAnalysisRed, "TimeProfileAnalysisRed");
      public static readonly ApplicationIcon Transport = createIconFrom(Icons.Influx, "Transport");
      public static readonly ApplicationIcon Transporter = createIconFrom(Icons.Transporter, "Transporter");
      public static readonly ApplicationIcon TubularSecretion = createIconFrom(Icons.TubularSecretion, "TubularSecretion");
      public static readonly ApplicationIcon UncheckAll = createIconFrom(Icons.UncheckAll, "UncheckAll");
      public static readonly ApplicationIcon UncheckSelected = createIconFrom(Icons.UncheckSelected, "UncheckSelected");
      public static readonly ApplicationIcon UncompetitiveInhibition = createIconFrom(Icons.UncompetitiveInhibition, "UncompetitiveInhibition");
      public static readonly ApplicationIcon Undo = createIconFrom(Icons.Undo, "Undo");
      public static readonly ApplicationIcon UnitInformation = createIconFrom(Icons.UnitInformation, "UnitInformation");
      public static readonly ApplicationIcon Up = createIconFrom(Icons.Up, "Up");
      public static readonly ApplicationIcon Update = createIconFrom(Icons.Update, "Update");
      public static readonly ApplicationIcon UpperIleum = createIconFrom(Icons.UpperIleum, "UpperIleum");
      public static readonly ApplicationIcon UpperJejunum = createIconFrom(Icons.UpperJejunum, "UpperJejunum");
      public static readonly ApplicationIcon UserDefined = createIconFrom(Icons.Individual, "UserDefined");
      public static readonly ApplicationIcon UserSettings = createIconFrom(Icons.Settings, "UserSettings");
      public static readonly ApplicationIcon VascularEndothelium = createIconFrom(Icons.Endothelium, "VascularEndothelium");
      public static readonly ApplicationIcon VenousBlood = createIconFrom(Icons.VenousBlood, "VenousBlood");
      public static readonly ApplicationIcon Warning = createIconFrom(Icons.Notifications, "Warning");
      public static readonly ApplicationIcon ZoomIn = createIconFrom(Icons.ZoomIn, "ZoomIn");
      public static readonly ApplicationIcon ZoomOut = createIconFrom(Icons.ZoomOut, "ZoomOut");
      public static readonly ApplicationIcon CopySelection = createIconFrom(Icons.CopySelection, "Copy Selection");
      public static readonly ApplicationIcon JournalExportToWord = createIconFrom(Icons.JournalExportToWord, "JournalExportToWord");
      public static readonly ApplicationIcon SBML = createIconFrom(Icons.SBML, "SBML");
      public static readonly ApplicationIcon PageAdd = createIconFrom(Icons.PageAdd, "PageAdd");
      public static readonly ApplicationIcon IndividualSimulationComparison = createIconFrom(Icons.IndividualSimulationComparison, "IndividualSimulationComparison");
      public static readonly ApplicationIcon PopulationSimulationComparison = createIconFrom(Icons.PopulationSimulationComparison, "PopulationSimulationComparison");
      public static readonly ApplicationIcon JournalSelect = createIconFrom(Icons.JournalSelect, "JournalSelect");
      public static readonly ApplicationIcon HistoryExport = createIconFrom(Icons.HistoryExport, "HistoryExport");
      public static readonly ApplicationIcon SimulationClone = createIconFrom(Icons.SimulationClone, "SimulationClone");
      public static readonly ApplicationIcon AnalysesLoad = createIconFrom(Icons.AnalysesLoad, "AnalysesLoad");
      public static readonly ApplicationIcon AnalysesSave = createIconFrom(Icons.AnalysesSave, "AnalysesSave");
      public static readonly ApplicationIcon ObserverAdd = createIconFrom(Icons.ObserverAdd, "ObserverAdd");
      public static readonly ApplicationIcon Report = createIconFrom(Icons.Report, "Report");
      public static readonly ApplicationIcon ExportToCSV = createIconFrom(Icons.ExportToCSV, "ExportToCSV");
      public static readonly ApplicationIcon ExportToExcel = createIconFrom(Icons.ExportToExcel, "ExportToExcel");
      public static readonly ApplicationIcon PKAnalysesImportFromCSV = createIconFrom(Icons.PKAnalysesImportFromCSV, "PKAnalysesImportFromCSV");
      public static readonly ApplicationIcon Notifications = createIconFrom(Icons.Notifications, "Notifications");
      public static readonly ApplicationIcon SimulationLoad = createIconFrom(Icons.SimulationLoad, "SimulationLoad");
      public static readonly ApplicationIcon PKMLLoad = createIconFrom(Icons.PKMLLoad, "PKMLLoad");
      public static readonly ApplicationIcon PKMLSave = createIconFrom(Icons.PKMLSave, "PKMLSave");
      public static readonly ApplicationIcon Administration = createIconFrom(Icons.Administration, "Administration");
      public static readonly ApplicationIcon ComparisonFolder = createIconFrom(Icons.ComparisonFolder, "ComparisonFolder");
      public static readonly ApplicationIcon About = createIconFrom(Icons.Info, "About");
      public static readonly ApplicationIcon ContainerSave = createIconFrom(Icons.ContainerSave, "ContainerSave");
      public static readonly ApplicationIcon EventSave = createIconFrom(Icons.EventSave, "EventSave");
      public static readonly ApplicationIcon AddFormulation = createIconFrom(Icons.FormulationAdd, "FormulationAdd");
      public static readonly ApplicationIcon FormulationLoad = createIconFrom(Icons.FormulationLoad, "FormulationLoad");
      public static readonly ApplicationIcon SaveFormulation = createIconFrom(Icons.FormulationSave, "FormulationSave");
      public static readonly ApplicationIcon MoleculeError = createIconFrom(Icons.MoleculeError, "MoleculeError");
      public static readonly ApplicationIcon SaveMolecule = createIconFrom(Icons.MoleculeSave, "MoleculeSave");
      public static readonly ApplicationIcon AddMoleculeStartValues = createIconFrom(Icons.MoleculeStartValuesAdd, "MoleculeStartVAluesAdd");
      public static readonly ApplicationIcon MoleculeStartValuesLoad = createIconFrom(Icons.MoleculeStartValuesLoad, "MoleculeStartValuesLoad");
      public static readonly ApplicationIcon SaveMoleculeStartValues = createIconFrom(Icons.MoleculeStartValuesSave, "MoleculeStartValuesSave");
      public static readonly ApplicationIcon SaveObserver = createIconFrom(Icons.ObserverSave, "ObserverSave");
      public static readonly ApplicationIcon AddParameterStartValues = createIconFrom(Icons.ParameterStartValuesAdd, "ParameterStartValuesAdd");
      public static readonly ApplicationIcon ParameterStartValuesLoad = createIconFrom(Icons.ParameterStartValuesLoad, "ParameterStartValuesLoad");
      public static readonly ApplicationIcon SaveParameterStartValues = createIconFrom(Icons.ParameterStartValuesSave, "ParameterStartValuesSave");
      public static readonly ApplicationIcon SaveReaction = createIconFrom(Icons.ReactionSave, "ReactionSave");
      public static readonly ApplicationIcon SaveSpatialStructure = createIconFrom(Icons.SpatialStructureSave, "SpatialStructureSave");
      public static readonly ApplicationIcon ParameterIdentificationFolder = createIconFrom(Icons.ParameterIdentificationFolder, "ParameterIdentificationFolder");
      public static readonly ApplicationIcon SensitivityAnalysisFolder = createIconFrom(Icons.SensitivityAnalysisFolder, "SensitivityAnalysisFolder");
      public static readonly ApplicationIcon SensitivityAnalysis = createIconFrom(Icons.SensitivityAnalysis, "SensitivityAnalysis");
      public static readonly ApplicationIcon ParameterIdentification = createIconFrom(Icons.ParameterIdentification, "ParameterIdentification");
      public static readonly ApplicationIcon ResidualHistogramAnalysis = createIconFrom(Icons.ResidualHistogramAnalysis, "ResidualHistogramAnalysis");
      public static readonly ApplicationIcon ResidualHistogramAnalysisGreen = createIconFrom(Icons.ResidualHistogramAnalysisGreen, "ResidualHistogramAnalysisGreen");
      public static readonly ApplicationIcon ResidualHistogramAnalysisRed = createIconFrom(Icons.ResidualHistogramAnalysisRed, "ResidualHistogramAnalysisRed");
      public static readonly ApplicationIcon ResidualVsTimeAnalysis = createIconFrom(Icons.ResidualVsTimeAnalysis, "ResidualVsTimeAnalysis");
      public static readonly ApplicationIcon ResidualVsTimeAnalysisGreen = createIconFrom(Icons.ResidualVsTimeAnalysisGreen, "ResidualVsTimeAnalysisGreen");
      public static readonly ApplicationIcon ResidualVsTimeAnalysisRed = createIconFrom(Icons.ResidualVsTimeAnalysisRed, "ResidualVsTimeAnalysisRed");
      public static readonly ApplicationIcon PredictedVsObservedAnalysis = createIconFrom(Icons.PredictedVsObservedAnalysis, "PredictedVsObservedAnalysis");
      public static readonly ApplicationIcon PredictedVsObservedAnalysisGreen = createIconFrom(Icons.PredictedVsObservedAnalysisGreen, "PredictedVsObservedAnalysisGreen");
      public static readonly ApplicationIcon PredictedVsObservedAnalysisRed = createIconFrom(Icons.PredictedVsObservedAnalysisRed, "PredictedVsObservedAnalysisRed");
      public static readonly ApplicationIcon DeleteFolderOnly = createIconFrom(Icons.DeleteFolderOnly, "DeleteFolderOnly");
      public static readonly ApplicationIcon CorrelationAnalysis = createIconFrom(Icons.CorrelationAnalysis, "CorrelationAnalysis");
      public static readonly ApplicationIcon CorrelationAnalysisGreen = createIconFrom(Icons.CorrelationAnalysisGreen, "CorrelationAnalysisGreen");
      public static readonly ApplicationIcon CorrelationAnalysisRed = createIconFrom(Icons.CorrelationAnalysisRed, "CorrelationAnalysisRed");
      public static readonly ApplicationIcon CovarianceAnalysis = createIconFrom(Icons.CovarianceAnalysis, "CovarianceAnalysis");
      public static readonly ApplicationIcon CovarianceAnalysisGreen = createIconFrom(Icons.CovarianceAnalysisGreen, "CovarianceAnalysisGreen");
      public static readonly ApplicationIcon CovarianceAnalysisRed = createIconFrom(Icons.CovarianceAnalysisRed, "CovarianceAnalysisRed");
      public static readonly ApplicationIcon DeleteSelected = createIconFrom(Icons.DeleteSelected, "DeleteSelected");
      public static readonly ApplicationIcon DeleteSourceNotDefined = createIconFrom(Icons.DeleteSourceNotDefined, "DeleteSourceNotDefined");
      public static readonly ApplicationIcon ExtendMoleculeStartValues = createIconFrom(Icons.ExtendMoleculeStartValues, "ExtendMoleculeStartValues");
      public static readonly ApplicationIcon MoleculeObserver = createIconFrom(Icons.MoleculeObserver, "MoleculeObserver");
      public static readonly ApplicationIcon OutputSelection = createIconFrom(Icons.OutputSelection, "OutputSelection");
      public static readonly ApplicationIcon PreviewOriginData = createIconFrom(Icons.PreviewOriginData, "PreviewOriginData");
      public static readonly ApplicationIcon Tree = createIconFrom(Icons.Tree, "Tree");
      public static readonly ApplicationIcon Diagram = createIconFrom(Icons.Diagram, "Diagram");
      public static readonly ApplicationIcon ContainerObserver = createIconFrom(Icons.ContainerObserver, "ContainerObserver");
      public static readonly ApplicationIcon ImportAll = createIconFrom(Icons.ImportAll, "ImportAll");
      public static readonly ApplicationIcon ReactionList = createIconFrom(Icons.ReactionList, "ReactionList");
      public static readonly ApplicationIcon SolverSettings = createIconFrom(Icons.SolverSettings, "SolverSettings");
      public static readonly ApplicationIcon Swap = createIconFrom(Icons.Swap, "Swap");
      public static readonly ApplicationIcon ImportAction = createIconFrom(Icons.ImportAction, "ImportAction");
      public static readonly ApplicationIcon FractionData = createIconFrom(Icons.FractionData, "FractionData");
      public static readonly ApplicationIcon ObservedDataForMolecule = createIconFrom(Icons.ObservedDataForMolecule, "ObservedDataForMolecule");
      public static readonly ApplicationIcon ParameterIdentificationVisualFeedback = createIconFrom(Icons.ParameterIdentificationVisualFeedback, "ParameterIdentificationVisualFeedback");
      public static readonly ApplicationIcon Results = createIconFrom(Icons.Results, "Results");
      public static readonly ApplicationIcon Formula = createIconFrom(Icons.Formula, "Formula");
      public static readonly ApplicationIcon UserDefinedVariability = createIconFrom(Icons.UserDefinedVariability, "UserDefinedVariability");
      public static readonly ApplicationIcon TimeProfileConfidenceInterval = createIconFrom(Icons.TimeProfileConfidenceInterval, "TimeProfileConfidenceInterval");
      public static readonly ApplicationIcon TimeProfilePredictionInterval = createIconFrom(Icons.TimeProfilePredictionInterval, "TimeProfilePredictionInterval");
      public static readonly ApplicationIcon TimeProfileVPCInterval = createIconFrom(Icons.TimeProfileVPCInterval, "TimeProfileVPCInterval");
      public static readonly ApplicationIcon PKAnalysesExportToCSV = createIconFrom(Icons.PKAnalysesExportToCSV, "PKAnalysesExportToCSV");
      public static readonly ApplicationIcon PKParameterSensitivityAnalysis = createIconFrom(Icons.PKParameterSensitivityAnalysis, "PKParameterSensitivityAnalysis");
      public static readonly ApplicationIcon SensitivityAnalysisVisualFeedback = createIconFrom(Icons.SensitivityAnalysisVisualFeedback, "SensitivityAnalysisVisualFeedback");

      // All icons should go at the end of the preceding list, before this delimiting icon - EmptyIcon
      public static readonly ApplicationIcon EmptyIcon = new ApplicationIcon(null);

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

      private static ApplicationIcon createIconFrom(Icon icon, string iconName)
      {
         var appIcon = new ApplicationIcon(icon)
         {
            IconName = iconName.ToUpperInvariant(),
            Index = _allIcons.Count()
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

         var iconName = string.Format("{0}{1}", applicationIcon.IconName, type);
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