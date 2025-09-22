using OSPSuite.Assets.Extensions;
using OSPSuite.Utility.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace OSPSuite.Assets
{
   public static class SizeAndLocation
   {
      public static readonly Point ParameterIdentificationFeedbackEditorLocation = new Point(100, 100);
      public static readonly Size ParameterIdentificationFeedbackEditorSize = new Size(880, 600);
      public static Point SensitivityAnalysisFeedbackEditorLocation = new Point(100, 100);
      public static Size SensitivityFeedbackEditorSize = new Size(350, 120);
   }

   public static class DefaultNames
   {
      public static readonly string MoleculeBuildingBlock = "Molecules";
      public static readonly string Module = "Module";
      public static readonly string ReactionBuildingBlock = "Reactions";
      public static readonly string SpatialStructure = "Organism";
      public static readonly string PassiveTransportBuildingBlock = "Passive Transports";
      public static readonly string EventBuildingBlock = "Events";
      public static readonly string ObserverBuildingBlock = "Observers";
      public static readonly string SimulationSettings = "Simulation Settings";
      public static readonly string ParameterValues = "Parameter Values";
      public static readonly string InitialConditions = "Initial Conditions";
   }

   public static class Captions
   {
      public static readonly string Transporter = "Transporter";
      public static readonly string Protein = "Protein";
      public static readonly string MetabolizingEnzyme = "Metabolizing Enzyme";
      public static readonly string Species = "Species";
      public static readonly string Phenotype = "Phenotype";
      public static readonly string ConfirmationDialog = "Confirmation";
      public static readonly string Excel = "Excel®";
      public static readonly string EmptyColumn = " ";
      public static readonly string EmptyName = "Empty";
      public static readonly string EmptyDescription = "<Empty>";
      public static readonly string ActionAlreadyInProgress = "Action already in progress...Please be patient.";
      public static readonly string AtLeastOneQuantityNeedsToBeSelected = "At least one quantity needs to be selected";
      public static readonly string PleaseWait = "Please wait...";
      public static readonly string ReallyCancel = "Do you really want to cancel?";
      public static readonly string Filter = "Filter";
      public static readonly string Organism = "Organism";
      public static readonly string Organ = "Organ";
      public static readonly string Compartment = "Compartment";
      public static readonly string Simulation = "Simulation";
      public static readonly string Molecule = "Molecule";
      public static readonly string Formulation = "Molecule";
      public static readonly string Application = "Application";
      public static readonly string Name = "Name";
      public static readonly string Value = "Value";
      public static readonly string DeselectAll = "Deselect All";
      public static readonly string Dimension = "Dimension";
      public static readonly string MoleculeProperties = "Molecule Properties";
      public static readonly string SaveChartLayoutToTemplateFile = "Save current layout to template file (Developer only)";
      public static readonly string LinearScale = "Linear Scale";
      public static readonly string LogScale = "Logarithmic Scale";
      public static readonly string ExportChartToExcel = "Export selected curves...";
      public static readonly string ExportChartToPng = "Export chart to Png...";
      public static readonly string ExportComparisonToExcel = "Export comparison to Excel...";
      public static readonly string CloseButton = "&Close";
      public static readonly string Folder = "Folder";
      public static readonly string Rename = "Rename";
      public static readonly string Description = "Description";
      public static readonly string EditDescription = "Edit Description";
      public static string SimulationPath = "Simulation";
      public static string TopContainerPath = "Top Container";
      public static string ContainerPath = "Container";
      public static string BottomCompartmentPath = "Compartment";
      public static string MoleculePath = "Molecule";
      public static string NamePath = "Name";
      public static readonly string Color = "Color";
      public static readonly string XData = "X Data";
      public static readonly string YData = "Y Data";
      public static readonly string YAxisType = "Y Axis Type";
      public static readonly string LineStyle = "Line Style";
      public static readonly string Symbol = "Symbol";
      public static readonly string LineThickness = "Line Thickness";
      public static readonly string Visible = "Visible";
      public static readonly string CurveSettings = "Curve Settings";
      public static readonly string AxisType = "Axis Type";
      public static readonly string NumberRepresentation = "Number Representation";
      public static readonly string Caption = "Caption";
      public static readonly string Unit = "Unit";
      public static readonly string Scaling = "Scaling";
      public static readonly string AxisMinimum = "Axis Minimum";
      public static readonly string AxisMaximum = "Axis Maximum";
      public static readonly string GridLines = "Grid Lines";
      public static readonly string DefaultColor = "Default Color";
      public static readonly string DefaultLineStyle = "Default Line Style";
      public static readonly string AxisSettings = "Axis Settings";
      public static readonly string VisibleInLegend = "Visible In Legend";
      public static readonly string DiagramBackground = "Diagram Background";
      public static readonly string ChartColor = "Chart Color";
      public static readonly string UseSelected = "Use selected";
      public static readonly string LinkDataToSimulations = "Link Data to Simulations";
      public static readonly string MetaData = "Meta Data";
      public static readonly string AddDataPoint = "Add Data Point";
      public static readonly string AddMetaData = "Add Meta Data";
      public static readonly string EditMultipleObservedDataMetaData = "Edit Meta Data";
      public static readonly string DeleteEntry = "Delete entry";
      public static readonly string AddEntry = "Add entry";
      public static readonly string Project = "Project";
      public static readonly string User = "User";
      public static readonly string SaveUnitsToFile = "Save Units";
      public static readonly string SaveImage = "Save Image";
      public static readonly string SaveFavoritesToFile = "Save Favorites";
      public static readonly string LoadUnitsFromFile = "Load Units";
      public static readonly string LoadFavoritesFromFile = "Load Favorites";
      public static readonly string AddUnitMap = "Add Unit";
      public static readonly string SaveUnits = "Save Units";
      public static readonly string LoadUnits = "Load Units";
      public static readonly string DisplayUnit = "Display Unit";
      public static readonly string AutomaticallyAddChartDescription = "Automatically Add Chart Description";
      public static readonly string SideMarginsEnabled = "Side Margins Enabled";
      public static readonly string ShowInLegend = "Show In Legend";
      public static readonly string Edit = "Edit";
      public static readonly string LegendPosition = "Legend Position";
      public static readonly string Title = "Title";
      public static readonly string CurveAndAxisSettings = "Curve and Axis Settings";
      public static readonly string ChartSettings = "Chart Options";
      public static readonly string CurveName = "Curve Name";
      public static readonly string XRepositoryName = "X-Repo";
      public static readonly string XDataPath = "X-Path";
      public static readonly string XQuantityType = "X-Type";
      public static readonly string YDataPath = "Y-Path";
      public static readonly string YRepositoryName = "Y-Repo";
      public static readonly string YQuantityType = "Y-Type";
      public static readonly string SaveChartTemplateToFile = "Save Chart Template...";
      public static readonly string LoadChartTemplateFromFile = "Load Chart Template...";
      public static readonly string OpenLayoutFromFile = "Open Layout Template...";
      public static readonly string LoadTemplate = "Load Template";
      public static readonly string ManageChartTemplates = "Manage Chart Templates";
      public static readonly string OutputSelections = "Output Selections";
      public static readonly string NewName = "New Name";
      public static readonly string RenameTemplate = "Rename Template";
      public static readonly string CloneTemplate = "Clone Template";
      public static readonly string CreateNewTemplate = "Create New Template";
      public static readonly string Default = "Default";
      public static readonly string FilePath = "File Path";
      public static readonly string None = "<None>";
      public static readonly string Favorites = "Favorites";
      public static readonly string Favorite = "Favorite";
      public static readonly string OSPSuite = "Systems Pharmacology Suite";
      public static readonly string InsertPKAnalysis = "Insert PK Analysis";
      public static readonly string CopyTable = "Copy Table";
      public static readonly string CopySelectedRows = "Copy Selected Rows";
      public static readonly string TaggedWith = "Tagged With";
      public static readonly string ChartExportSettings = "Chart Export Settings";
      public static readonly string Word = "Word®";
      public static readonly string ExportJournalToWord = $"Export journal to {Word}";
      public static readonly string ExportSelection = "Export Selection";
      public static readonly string ResetToDefault = "Reset to Default";
      public static readonly string AmountInContainer = "Amount in container";
      public static readonly string ConfirmJournalDiagramRestoreLayout = "Really reset diagram layout? This is irreversible.";
      public static readonly string SimulationFolder = "Simulations";
      public static readonly string ComparisonFolder = "Comparisons";
      public static readonly string ObservedDataFolder = "Observed Data";
      public static readonly string ParameterIdentificationFolder = "Parameter Identifications";
      public static readonly string SensitivityAnalysisFolder = "Sensitivity Analyses";
      public static readonly string QualificationPlanFolder = "Qualification Plans";
      public static readonly string LLOQ = "LLOQ";
      public static readonly string ErrorType = "Error type";
      public static readonly string Delete = "Delete";
      public static readonly string Clear = "Clear";
      public static readonly string SelectSimulations = "Select Simulations";
      public const string AddButtonText = "Add";
      public const string RemoveButtonText = "Remove";
      public static readonly string EnterAValue = "<enter a value>";
      public static readonly string NaN = "<NaN>";
      public static readonly string NA = "<NA>";
      public static readonly string Analysis = "Analysis";
      public static readonly string CopyAsImage = "Copy as image";
      public static readonly string InvalidObject = "Invalid Object";
      public static readonly string InvalidObjectToolTip = "Full path in hierarchy of invalid object";
      public static readonly string GroupingModeHierarchical = "Hierarchy";
      public static readonly string GroupingModeSimple = "Simple";
      public static readonly string GroupingModeAdvanced = "Advanced";
      public static readonly string Cancel = "Cancel";
      public static readonly string CopySelection = "Copy Selection";
      public static readonly string ApplicationMolecule = "Application Molecule";
      public static readonly string NumberOfProcessors = "Max. number of processors to use";
      public static readonly string Boundary = "Boundary";
      public static readonly string OptimalValue = "Optimal Value";
      public static readonly string StartValue = "Start Value";
      public static readonly string Details = "Details";
      public static readonly string ExportObservedDataToExcel = $"Export observed data to {Excel}";
      public static readonly string ExportToExcel = $"Export to {Excel}";
      public static readonly string ReadLicenseAgreement = "Read license agreement";
      public static readonly string CancelButton = "&Cancel";
      public static readonly string OKButton = "&OK";
      public static readonly string NextButton = "&Next";
      public static readonly string PreviousButton = "&Previous";
      public static readonly string DataTable = "DataTable";
      public static readonly string Label = "Label";
      public static readonly string Comments = "Comments";
      public static readonly string CopyToClipboard = "Copy to Clipboard";
      public static readonly string Exception = "Exception";
      public static readonly string StackTrace = "Stack Trace";
      public static readonly string LogLevel = "Log Level";
      public static readonly string ValueOriginDescription = "Value Description";
      public static readonly string ValueOriginSource = "Source";
      public static readonly string ValueOriginDeterminationMethod = "Method";
      public static readonly string ValueOrigin = "Value Origin";
      public static readonly string CalculationMethod = "Calculation Method";
      public static readonly string MoleculeObserver = "Molecule Observer";
      public static readonly string ContainerObserver = "Container Observer";
      public static readonly string UnitsEditorCaption = "Unit Settings";
      public static readonly string EditManually = "Edit manually";
      public static readonly string ShouldColorGroupObservedData = "Color group observed data from same folder when dropping to chart";
      public static readonly string EditAllCurvesProperties = "Edit Options for Selected";
      public static readonly string CurvesAndAxisOptions = "Curves and Axis Options";
      public static readonly string CurvesColorGrouping = "Curves Color Grouping";
      public static readonly string ChartOptions = "Chart Options";
      public static readonly string ChartExportOptions = "Chart Export Options";
      public static readonly string No = "No";
      public static readonly string Yes = "Yes";
      public static readonly string ReallyRemoveObservedDataFromSimulation = $"Really remove {ObjectTypes.ObservedData} from the simulation?\nHint: {ObjectTypes.ObservedData} will not be deleted from the project";
      public static readonly string SimulationWasCanceled = "Simulation was canceled";
      public static readonly string SelectMappingToShowObservedData = "Select mapping to show observed data";
      public static readonly string DoNotShowThisAgainUntilRestart = "Do not show this again until restart";
      public static readonly string AddPoint = "Add Point";
      public static readonly string UseDerivedValues = "Use derivative values";
      public static readonly string NotDistributed = "Not Distributed";
      public static readonly string DeleteSelected = "Delete Selected Records";
      public static readonly string ModulesFolder = "Modules";
      public static readonly string ApplyChangesToUpdateChart = "Apply changes to update chart";
      public static readonly string Apply = "Apply updates";
      public static readonly string AutoUpdateChart = "Auto-update chart";

      public static string EditTableParameter(string parameter, bool editable) => $"{(editable ? "Edit" : "Show")} table parameter '{parameter}'";

      public static string ShouldWatermarkBeUsedForChartExportToClipboard(string applicationName, string optionLocation)
      {
         var sb = new StringBuilder();
         sb.AppendLine("The watermark feature was introduced to clearly identify draft versions of plots.");
         sb.AppendLine();
         sb.AppendLine($"Do you want this installation of {applicationName} to use this feature? If yes, a watermark will be used when copying charts to clipboard.");
         sb.AppendLine($"This setting, as well as the watermark text, can be changed anytime under '{optionLocation}'.");
         return sb.ToString();
      }

      public static string IssueTrackerLinkFor(string application) => $"Go to {application} Issue Tracker";

      public static string ReallyDeleteAllObservedData(IEnumerable<string> anythingNotDeleted)
      {
         var prompt = "Really delete observed data?";
         var notDeleted = anythingNotDeleted.ToList();
         if (notDeleted.Count == 0)
            return prompt;

         return $"{prompt}\n\nWarning: Following observed data are still in use and will not be deleted: \n\n{notDeleted.ToString("\n\n")}";
      }

      public static string CloneObjectBase(string entityType, string name)
      {
         return $"New name for the cloned {entityType} '{name}'";
      }

      public static string ContactSupport(string forumUrl)
      {
         return $"For more information or questions, please visit the Open Systems Pharmacology Forum ({forumUrl}).";
      }

      public static string CreatedOn(string creationDate)
      {
         return $"Created on {creationDate}";
      }

      public static string ClonedOn(string creationDate, string clonedFrom)
      {
         return $"Cloned from '{clonedFrom}' on {creationDate}";
      }

      public static string ConfiguredOn(string creationDate)
      {
         return $"Configured on {creationDate}";
      }

      public static string ChartFingerprintDataFrom(string projectName, string simulationName, string dateString)
      {
         return $"{projectName}\n{simulationName}\n{dateString}";
      }

      public static string ManageDisplayUnits(string type)
      {
         return $"Manage {type} Display Units";
      }

      public static string NumberOfSelectedCurveWouldGoOverThreshold(int numberOfCurves)
      {
         return $"The number of selected curves would exceed {numberOfCurves}. Do you want to continue?";
      }

      public static string PathElement(int index)
      {
         return $"Path Element {index}";
      }

      public static string DoYouWantToDeleteDirectory(string newDirectoryName)
      {
         return $"Do you want to delete the directory '{newDirectoryName}' and continue?";
      }

      public static void AppendOrderedListItem(StringBuilder sb, bool html, string listItem, int index)
      {
         if (html)
            sb.Append($"<li>{listItem}</li>");
         else
            sb.AppendLine($"   {index}: {listItem}");
      }

      public static void AppendLine(StringBuilder sb, bool html, string lineToAppend)
      {
         if (html)
            sb.Append($"<p>{lineToAppend}</p>");
         else
            sb.AppendLine(lineToAppend);
      }

      public static void AddOrderedList(StringBuilder sb, bool html, params string[] list)
      {
         if (html)
            sb.Append("<ol>");

         list.Each((item, i) =>
         {
            //index in list item should start at 1
            AppendOrderedListItem(sb, html, item, i + 1);
         });

         if (html)
            sb.Append("</ol>");
      }

      public static string ExceptionViewDescription(string issueTrackerUrl, bool html = true)
      {
         var sb = new StringBuilder();
         AppendLine(sb, html, "oops...something went terribly wrong.");
         AppendLine(sb, html, string.Empty);
         AppendLine(sb, html, "To best address the error, please enter an issue in our issue tracker:");
         AddOrderedList(sb, html,
            $"Visit <b>{issueTrackerUrl}</b> or click on the link below",
            "Click on the <b>New Issue</b> button",
            "Describe the steps you took prior to the problem emerging",
            $"Copy the information below by using the <b>{CopyToClipboard}</b> button and paste it in the issue description",
            "if possible, attach your project file to the issue (do not attach confidential information)"
         );
         AppendLine(sb, html, string.Empty);
         AppendLine(sb, html, "Note: A GitHub account is required to create an issue");
         return sb.ToString();
      }

      public static string EnterNameEntityCaption(string type)
      {
         return $"Enter name for {type}";
      }

      public static string RenameEntityCaption(string type, string name)
      {
         return $"New name for {type} '{name}'";
      }

      public static string ReallyDeleteObservedData(string observedDataName)
      {
         return $"Really delete observed data '{observedDataName}' from project";
      }

      public static string ReallyClearHistory = "Really clear command history? This action is irreversible even if the project is not saved afterwards.";

      public static class SimulationUI
      {
         public static readonly string ObservedDataSelection = "Observed Data";
         public static readonly string PredictedVsObservedSimulation = "Predicted vs Observed";
         public static readonly string ResidualsVsTimeSimulation = "Residuals vs Time";
         public static readonly string Outputs = "Simulation Outputs";
         public static readonly string ObservedData = "Observed Data";
         public static readonly string NoneEditorNullText = "<None>";
      }

      public static class Importer
      {
         public static readonly string ImportAll = "Import All";
         public static readonly string Import = "Import";
         public static readonly string NoPreview = "No Preview Available";
         public static readonly string LogScale = "Log Scale";
         public static readonly string LinearScale = "Linear Scale";
         public static readonly string NamingPattern = "Naming Pattern";
         public static readonly string ExcelFile = "Excel File";
         public static readonly string OriginalDataPreviewView = "Preview of Original Data";
         public static readonly string SetRange = "Set Range";
         public static readonly string Close = "Close";
         public static readonly string PreviewExcelData = "Preview Original Data";
         public static readonly string MetaData = "Meta Data";
         public static readonly string Data = "Data";
         public static readonly string NoneEditorNullText = "<None>";
         public static readonly string GroupByEditorNullText = "<Group By>";
         public static readonly string NothingSelectableEditorNullText = "<Nothing Selectable>";
         public static readonly string PleaseEnterMetaDataInformation = "Please enter meta data information.";
         public static readonly string PleaseEnterData = "Please enter data";
         public static readonly string ApplyToAll = "Apply to All";
         public static readonly string PleaseEnterDimensionAndUnitInformation = "Please enter the dimension and unit information.";
         public static readonly string PleaseSelectDataFile = "Please select a data file.";
         public static readonly string UnitInformation = "Unit Information";
         public static readonly string TheUnitInformationMustBeEnteredOrConfirmed = "The unit information must be entered or confirmed.";
         public static readonly string TheMetaDataInformationMustBeEnteredOrConfirmed = "The meta data must be entered or confirmed.";
         public static readonly string ResetMapping = "Reset Mapping";
         public static readonly string ResetMappingToolTip = "Automatically recalculates the format based on the sheet currently selected and sets the mapping settings accordingly.";
         public static readonly string ClearMappingToolTip = "Clears all the mappings. Same as clicking all the X buttons to the right of the grid.";
         public static readonly string ClearMapping = "Clear All";
         public static readonly string Format = "Format: ";
         public static readonly string AddKeys = "Add Keys";
         public static readonly string Columns = "Columns";
         public static readonly string Mappings = "Mappings";
         public static readonly string FormatPlain = "Format";
         public static readonly string DataMapping = "Data Mapping";
         public static readonly string ImportPreview = "Import preview";
         public static readonly string ThreeDots = "...";
         public static readonly string File = "File:";
         public static readonly string ManualInput = "Manual input";
         public static readonly string LloqColumnEditorTitle = "Please select the lloq column.";
         public static readonly string ConfirmationImport = "Import";
         public static readonly string NanAction = "Action";
         public static readonly string NanActionThrowsError = "Prevent the import";
         public static readonly string NanActionIgnoreRow = "Ignore the row";

         public static readonly string NanActionHint =
            "Defines what to do when an invalid measurement is found (invalid measurements are NaN or the number indicated in the NaN indicator). \"Ignore the row\" will import the data ignoring the currently invalid row. \"Prevent the import\" will throw an error and halt the import process";

         public static readonly string NanIndicator = "NaN indicator";
         public static readonly string NanIndicatorHint = "Type a number that will be interpreted as NaN(Not a Number). Text in numerical columns is interpreted as NaN anyway.";
         public static readonly string OpenFileConfirmation = "Opening a new file will drop your currently imported data. Are you sure you want to open a new file?";
         public static readonly string ExcelColumn = "Data Column/Value";
         public static readonly string MappingName = "Mapping Name";
         public static readonly string MappingSettings = "Mapping Settings";
         public static readonly string UnitColumn = "Unit";
         public static readonly string ExtraColumn = "Edit Extra Fields";
         public static readonly string ErrorColumn = "Error";
         public static readonly string ErrorType = "Error type";
         public static readonly string LoadAllSheets = "Add All Sheets";
         public static readonly string SourceTab = "Source";
         public static readonly string ConfirmationTab = "Confirmation";
         public static readonly string PreviewLayout = "Preview";
         public static readonly string SourceLayout = "Source";
         public static readonly string Separator = "Separator";
         public static readonly string DataSets = "Data Sets";
         public static readonly string NamingElement = "Naming Element";
         public static readonly string CreateNamingPattern = "Create Naming Pattern";
         public static readonly string Dimension = "Dimension";
         public static readonly string Unit = "Unit";
         public static readonly string ImportLLOQFromColumn = "Import LLOQ from a column";
         public static readonly string ImportUnitFromColumn = "Import unit from a column";
         public static readonly string Column = "Column";
         public static readonly string LoadCurrentSheet = "Add Current Sheet";
         public static readonly string AllSheetsAlreadyImported = "All imported";
         public static readonly string SheetsAlreadyImported = "Imported";
         public static readonly string CloseAllTabsButThis = "Close all tabs but this";
         public static readonly string CloseAllTabsToTheRight = "Close all tabs to the right";
         public static readonly string ResetAllTabs = "Reopen all sheets";
         public static readonly string UseFilterForImport = "Use the filters selected not only for visualization but also for importing the data";
         public static readonly string Title = "Import Observed Data";
         public static readonly string LLOQ = "LLOQ";
         public static readonly string LloqDescription = "LLOQ values will be imported from the measurement column if values are written in the form < xxx (eg <0.001)";
         public static readonly string SaveConfiguration = "Save Configuration";
         public static readonly string ApplyConfiguration = "Load Configuration";
         public static readonly string ActionWillEraseLoadedData = "This action will result in dropping all the loaded sheets. Do you want to continue?";
         public static readonly string OpenFile = "Select the file you would like to apply configuration on";
         public static readonly string GroupByTitle = "Group By";
         public static readonly string SelectToAdd = "Select to add";
         public static readonly string MappingTitle = "Mapping";

         public static readonly string ReloadWillCauseChangeOfDataSets =
            "Reloading will cause the following changes in observed data. Do you really want to reload?";

         public static readonly string UnexpectedExceptionWhenLoading =
            "An unexpected error occurred while loading the file. The file format is probably not supported. Please check the <href =https://docs.open-systems-pharmacology.org/shared-tools-and-example-workflows/import-edit-observed-data#supported-formats >documentation</href> for more details";

         public static readonly string DataSetsWillBeOverwritten = "Datasets that will be overwritten";
         public static readonly string NewDataStetsWillBeImported = "New datasets that will be imported";
         public static readonly string ReloadData = "Reload Data";
         public static readonly string SeparatorSelection = "Separator Selection";
         public static readonly string DecimalSeparator = "Decimal Separator";
         public static readonly string ColumnSeparator = "Column Separator";
         public static readonly string DimensionSelect = "Dimension Select";
         public static readonly string SetDimensionsForAmbiguousUnits = "One or more column dimensions cannot be determined uniquely from the units.\n\nPlease set the dimension from the possible supporting dimensions";

         public static string LLOQInconsistentValuesAt(string dataRepositoryName) => $"There were different LLOQ values detected for the data from a single source. Please check data under name {dataRepositoryName}. Are you sure you want to continue with import?";
         public static string CsvSeparatorInstructions(string fileName) => $"Please select the separators for '{fileName}':";

         public static readonly string SheetFormatNotSupported =
            "The format of the sheet you are trying to use is not supported.You can find a documentation of the supported formats<href =https://docs.open-systems-pharmacology.org/shared-tools-and-example-workflows/import-edit-observed-data#supported-formats > here </href>";

         public static string ConfirmDroppingExcelColumns(string listOfExcelColumns)
         {
            var sb = new StringBuilder();
            sb.AppendLine($"The following excel columns do not exist in the current file. \n \n'{listOfExcelColumns}' ");
            sb.AppendLine();
            sb.AppendLine("The corresponding mappings from the configuration will be lost. Do you want to continue?");
            return sb.ToString();
         }

         public static string SheetsNotFound(List<string> listOfSheetNames)
         {
            var sb = new StringBuilder();
            sb.AppendLine("The following excel sheets were not found in the file and will not be imported ");
            sb.AppendLine();
            listOfSheetNames.ForEach(item => sb.AppendLine(item));
            return sb.ToString();
         }

         public static readonly string UseFiltersForImport = "Use filters for importing data";

         public static readonly string UseFiltersForImportTooltip =
            "When selected, the filter will apply to the data during the import process. When deselected, the filter only affects this view. Check documentation for more information on defining filters: <href=https://docs.open-systems-pharmacology.org/shared-tools-and-example-workflows/features-of-tables#filtering>https://docs.open-systems-pharmacology.org/shared-tools-and-example-workflows/features-of-tables#filtering</href>";

         public static readonly string AddGroupByTitle = "Add Group By";
         public static readonly string MetaDataTitle = "Meta data";
         public static readonly string IgnoredParameterTitle = "Ignored parameter";
         public static readonly string NotConfiguredField = "Field not configured yet";
         public static readonly string AddGroupBy = "Add a new grouping by";
         public static readonly string MissingMandatoryMapping = "Field is mandatory and has not configured yet";
         public static readonly string MissingUnit = "Field must contain a valid unit description";

         public static string MappingHint(string parameter, string target, string unit)
         {
            return $"The column {parameter} will be mapped into {target} with units as {unit}";
         }

         public static string MappingHintUnitColumn(string parameter, string target, string unitColumn)
         {
            return $"The column {parameter} will be mapped into {target} and column {unitColumn} will be mapped into unit";
         }

         public static string MappingHintNoUnit(string parameter, string target)
         {
            return $"The column {parameter} will be mapped into {target}";
         }

         public static string GroupByHint(string parameter)
         {
            return $"The column {parameter} will be used for grouping by";
         }

         public static string MappingHintGeometricError = "Geometric standard deviation";

         public static string AddGroupByHint = "Configure the parameters and click the add button to add a new grouping by field";

         public static string NamingPatternDescription = "Automatically generates names replacing words surrounded by <b>{}</b> with like named meta data values.";

         public static string NamingPatternPanelDescription = "Select one or more names from the list and the separator between them. " +
                                                              "By clicking the <b>Add keys</b> button, " +
                                                              "keys will be added to the naming pattern, separated by the selected separator";

         public static string MetaDataHint(string parameter, string target)
         {
            return $"The column {parameter} will be used as meta data to extract the following data: {target}";
         }

         public static readonly string IgnoredParameterHint = "This parameter will be ignored";

         public static readonly string GroupByDescription = "Group by";

         public static string MetaDataDescription(string metaDataId)
         {
            return $"{metaDataId}";
         }

         public static string MappingDescription(string parameter, string unit)
         {
            return $"{parameter}({unit})";
         }

         public static readonly string UnitInformationCaption = "Unit Information";
         public static readonly string UnitInformationDescription = "Here you can enter unit information which will be used for all created import data table columns";
         public static readonly string AddInformationDescription = "Add a new grouping by field";
         public static readonly string LloqInformationDescription = "Here you can enter lloq information which will be used for all created import data table columns";
         public static readonly string ErrorTypeInformationDescription = "Here you can enter error type information which will be used for all created import data table columns";

         public class ToolTips
         {
            public static readonly string NamingPattern = "Set a pattern for renaming imported data";
            public static readonly string RangeSelect = "Override the default range by selecting a new range and pressing OK.\nTo revert to the default range click OK without a new range selected.";
         }

         public static readonly string ImportFileFilter = "Excel Files (*.xls, *.xlsx)|*.xls;*.xlsx|Comma Separated Value Files (*.csv)|*.csv|NonMem Files (*.NMdat)|*.NMdat|All Files (*.*)|*.*";

         public static string UpdatedMappingsMessage(IEnumerable<(string ParameterName, string OutputPath)> updatedMappingsInfo)
         {
            var sb = new StringBuilder();
            sb.AppendLine("The following parameter identifications and their output mappings were modified:");
            sb.AppendLine();

            var groupedMappings = updatedMappingsInfo
               .GroupBy(mapping => mapping.ParameterName)
               .OrderBy(group => group.Key);

            foreach (var group in groupedMappings)
            {
               sb.AppendLine($"- {group.Key}");
               foreach (var mapping in group)
               {
                  sb.AppendLine($"    - Output Path: {mapping.OutputPath}");
               }
               sb.AppendLine();
            }

            return sb.ToString();
         }

      }

      public static class Diff
      {
         public static readonly string ReactionPartnerName = "Reaction Partner";
         public static readonly string Tag = "Tag";
         public static readonly string ObjectPath = "Path";
         public static readonly string ExcludeMolecule = "Exclude Molecule";
         public static readonly string IncludeMolecule = "Include Molecule";
         public static readonly string Count = "Number of item";
         public static readonly string Assignment = "Assignment";
         public static readonly string OneObjectIsNull = "One object used in the comparison is null";
         public static readonly string Connection = "Connection";
         public static readonly string SourceAmount = "Source Amount";
         public static readonly string TargetAmount = "Target Amount";

         public static string PropertyDiffers(string propertyName, int value1, int value2) => PropertyDiffers(propertyName, $"{value1}", $"{value2}");

         public static string PropertyDiffers(string propertyName, string value1, string value2) => $"{propertyName.Pluralize()} are not equal ({value1} ≠ {value2})";

         public static string DifferentTypes(string type1, string type2)
         {
            return $"Different Types: ({type1} vs {type2})";
         }

         public static string ObjectMissing(string containerType, string containerName, string missingObjectType, string missingObjectName)
         {
            return $"{missingObjectType} '{missingObjectName}' is missing from {containerType} '{containerName}'";
         }

         public static string ConnectionBetween(string firstNeighborPath, string secondNeighborPath)
         {
            return $"Between '{firstNeighborPath}' and '{secondNeighborPath}'";
         }

         public static readonly string NoDifferenceFound = "No difference found.";
         public static readonly string Stationary = "Stationary";
         public static readonly string IsStateVariable = "Is state variable";
         public static readonly string Criteria = "Criteria";
      }

      public static class Commands
      {
         public static readonly string LabelViewCaption = "Add Label...";
         public static readonly string Comment = "Comment";
         public static readonly string Undo = "Undo";
         public static readonly string AddLabel = "Add Label";
         public static readonly string EditComment = "Edit Comment";
         public static readonly string Rollback = "Rollback";
         public static readonly string ExtendedDescription = "Extended Description";
         public static readonly string ClearHistory = "Clear History";

         public static string CommentViewCaption(int historyItemState) => $"Edit comments for state '{historyItemState}' ...";
      }

      public static class Journal
      {
         public static readonly string CreatedBy = "Created By";
         public static readonly string CreatedAt = "Created On";
         public static readonly string UpdatedAt = "Updated On";
         public static readonly string UpdatedBy = "Updated By";
         public static readonly string Title = "Title";
         public static readonly string Source = "Source";
         public static readonly string Description = "Description";
         public static readonly string Tags = "Tags";
         public static readonly string RelatedItem = ObjectTypes.RelatedItem;
         public static readonly string RelatedItems = RelatedItem.Pluralize();
         public static readonly string Project = "Project";
         public static readonly string SearchMatchAny = "Match any";
         public static readonly string SearchMatchWholePhrase = "Match whole phrase";
         public static readonly string SearchMatchCase = "Match case";
         public static readonly string SearchPlaceholder = "Enter search to search...";
         public static readonly string Find = "Find";
         public static readonly string Clear = "Clear";
         public static readonly string RelatedItemType = "Type";
         public static readonly string Name = "Name";
         public static readonly string Version = "Version";
         public static readonly string Origin = "Origin";
         public static readonly string DefaultDiagramName = "Overview";
         public static readonly string JournalEditor = $"{ObjectTypes.Journal} Editor";
         public static readonly string SelectJournal = $"Select {ObjectTypes.Journal}";
         public static readonly string JournalDiagram = $"{ObjectTypes.Journal} Diagram";
         public static readonly string SearchJournal = "Search";
         public static readonly string JournalEditorView = JournalEditor;
         public static readonly string JournalDiagramView = JournalDiagram;
         public static readonly string JournalView = ObjectTypes.Journal;
         public static readonly string AddToJournalMenu = $"Add to {ObjectTypes.Journal}";
         public static readonly string AddToJournal = $"{AddToJournalMenu}...";
         public static readonly string CreateJournalPage = $"Add {ObjectTypes.JournalPage}";
         public static readonly string CreateJournalPageMenu = $"{CreateJournalPage}...";
         public static readonly string JournalEditorViewDescription = $"Show or hide the {JournalEditor.ToLower()}";
         public static readonly string JournalDiagramDescription = $"Show or hide the {JournalDiagram.ToLower()}";
         public static readonly string JournalViewDescription = $"Show or hide the {ObjectTypes.Journal.ToLower()}";
         public static readonly string SearchJournalDescription = "Search journal pages";
         public static readonly string SelectJournalDescription = $"Select the {ObjectTypes.Journal} associated to the project...";
         public static readonly string ExportRelatedItemToFile = $"Export the {ObjectTypes.RelatedItem} to file...";
         public static readonly string SaveDiagram = "Save Diagram";
         public static readonly string RestoreLayout = "Reset Layout to Default";
         public static readonly string CreateJournal = "Create Journal";
         public static readonly string OpenJournal = "Open Journal";
         public static readonly string NoJournalAssociatedWithProject = "Do you want to open an existing journal or create a new one?";
         public static readonly string CreateJournalButton = "Create";
         public static readonly string OpenJournalButton = "Open";
         public static readonly string CancelJournalButton = "Cancel";
         public static readonly string RunComparison = "Compare";
         public static readonly string AddRelatedItem = "Add New";
         public static readonly string ImportAllRelatedItem = "Load All";
         public static readonly string RelatedItemFile = $"{ObjectTypes.RelatedItem} file";
         public static readonly string SelectedFileToLoadAsRelatedItem = "Select file to load as related item";

         public static string ReallyLoadRelatedItemFileExceedingThreshold(string fileSizeInMegaBytes, string thresholdSizeInMegaBytes)
         {
            return $"The selected file size is '{fileSizeInMegaBytes} MB' and exceeds the recommended file size of '{thresholdSizeInMegaBytes} MB'. Do you want to continue?";
         }

         public static string JournalWillBeSharedBetweenProjectInfo(string journalName)
         {
            return $"The journal '{journalName}' will be used by both the old and new project files";
         }

         public static string CompareRelatedItem(string name)
         {
            return $"{RelatedItem} - {name}";
         }

         public static string CompareProjectItem(string name)
         {
            return $"{Project} - {name}";
         }

         public static string ReallyDeleteObjectOfType(string type, string name)
         {
            return $"Really delete {type} '{name}'?\nThis action is irreversible!";
         }

         public static string ExportRelatedItemToFileFilter(string defaultFileExtension)
         {
            return string.Format("Related Item (*{0})|*{0}|(All Files *.*)|**", defaultFileExtension);
         }

         public static string CreatedAtBy(string formattedDate, string by)
         {
            return $"Created on {formattedDate} by {by}";
         }

         public static string UpdatedAtBy(string formattedDate, string by)
         {
            return $"Last updated on {formattedDate} by {by}";
         }

         public static readonly string ReallyDeleteMultipleRelatedItems = "Really delete Related Items?";

         public static readonly string ReallyDeleteMultipleJournalPages = "Really delete Journal Pages?";

         public static string ReallyDeleteJournalPage(string workingJournalTitle)
         {
            return ReallyDeleteObjectOfType(ObjectTypes.JournalPage, workingJournalTitle);
         }

         public static string ReallyDeleteRelatedItem(string relatedItem)
         {
            return ReallyDeleteObjectOfType(ObjectTypes.RelatedItem, relatedItem);
         }

         public static string NoObjectAvailableForComparison(string itemType)
         {
            return $"No {itemType.ToLowerInvariant()} available for comparison";
         }

         public static string AvailbleItemsForComparison(string itemType)
         {
            return $"Available {itemType.Pluralize()}";
         }

         public static string ExportWorkingJournalFileName(string projectName)
         {
            return $"Journal_for_{projectName}.docx";
         }

         public static class ToolTip
         {
            public static string CreateJournalPage = $"Create a new {ObjectTypes.JournalPage.ToLower()}";

            public static string CompareRelatedItemWithProjectItems(string relatedItemName, string relatedItemType)
            {
               var type = relatedItemType.ToLowerInvariant();
               return $"Compare '{relatedItemName}' with {type.Pluralize()} defined in project";
            }

            public static string ReloadRelatedItem(string relatedItemName, string relatedItemType) => $"Reload {relatedItemType.ToLower()} '{relatedItemName}' into project";

            public static string ExportRelatedItemToFile(string relatedItemName) => $"Export '{relatedItemName}' to file";

            public static string DeleteRelatedItem = $"Delete {RelatedItem.ToLower()}";
         }

         public static string UsingOrigin(string originDisplayName)
         {
            return $"Using {originDisplayName}";
         }

         public static string WithParent(int uniqueIndex)
         {
            return $"With parent {uniqueIndex}";
         }
      }

      public static class ValueOrigins
      {
         public static class Sources
         {
            public static string Database = "Database";
            public static string Internet = "Internet";
            public static string ParameterIdentification = "Parameter Identification";
            public static string Publication = "Publication";
         }

         public static class Methods
         {
            public static string Assumption = "Assumption";
            public static string ManualFit = "Manual Fit";
            public static string ParameterIdentification = "Parameter Identification";
            public static string InVitro = "In Vitro";
            public static string InVivo = "In Vivo";
         }

         public static string Other = "Other";
         public static string Unknown = "Unknown";
         public static string Undefined = "";
      }

      public static class Reporting
      {
         public static readonly string DefaultTitle = "Report";
         public static readonly string SelectFile = "Select report file...";
         public static readonly string ObservedData = "Observed Data";
         public static readonly string Font = "Font";
         public static readonly string CreateReport = "Create";
         public static readonly string Verbose = "Extended output (descriptions, images etc...)";
         public static readonly string OpenReportAfterCreation = "Open the report once created";
         public static readonly string Draft = "Draft watermark";
         public static readonly string SaveArtifacts = "Save reporting artifacts (exported in folder <report_name>_Files)";
         public static readonly string FirstPageSettings = "First page settings";
         public static readonly string Options = "Options";
         public static readonly string Output = "Output settings";
         public static readonly string TemplateSelection = "Template selection";
         public static readonly string Author = "Author";
         public static readonly string Title = "Title";
         public static readonly string Type = "Type";
         public static readonly string Subtitle = "Subtitle";
         public static readonly string Template = "Template";
         public static readonly string OutputFile = "File";
         public static readonly string Color = "Color";
         public static readonly string GrayScale = "Gray scale";
         public static readonly string BlackAndWhite = "Black & White";
         public static readonly string ReportToPDFTitle = "Create PDF Report...";
         public static readonly string DeleteWorkingFolder = "Delete working folder (Developer only)";

         public static string ReportFor(string objectType)
         {
            return $"{objectType} Report";
         }
      }

      public static class Comparisons
      {
         public static readonly string RelativeTolerance = "Comparison tolerance (relative) ";
         public static readonly string RelativeToleranceDescription = "Limit where two values are considered equal";
         public static readonly string FormulaComparisonMode = "Compare formulas by";

         public static string FormulaComparisonModeDescription
         {
            get
            {
               var sb = new StringBuilder();
               sb.AppendLine("<b>By values</b>: either the numerical values or the values produced by calculating the formulas are compared.");
               sb.AppendLine();
               sb.AppendLine("<b>By formulas</b>: the formulas are compared, not the values.");
               return sb.ToString();
            }
         }

         public static readonly string OnlyComputeModelRelevantProperties = "Only compare properties relevant to simulation results";
         public static readonly string CompareHiddenEntities = "Compare hidden entities (e.g. parameters)";
         public static readonly string ShowValueOriginForChangedValues = "Show value origin for changed values";
         public static readonly string FormulaComparisonValue = "Values";
         public static readonly string FormulaComparisonFormula = "Formulas";
         public static readonly string RunComparison = "Start";
         public static readonly string Left = "Left";
         public static readonly string Right = "Right";
         public static readonly string ComparisonResults = "Results";
         public static readonly string ComparisonSettings = "Settings";
         public static readonly string ExportToExcel = "Export to Excel";
         public static readonly string ShowSettings = "Settings";
         public static readonly string HideSettings = "Hide";
         public static readonly string Absent = "Absent";
         public static readonly string Present = "Present";
         public static readonly string Description = "Description";
         public static readonly string PathAsString = "Path";
         public static readonly string ObjectName = "Object";
         public static readonly string Property = "Property";
         public static readonly string Comparison = "Comparison";
         public static readonly string ValueDescription = "Value Description";

         public static string ComparisonTitle(string objectType)
         {
            return $"{objectType} {Comparison}";
         }

         public static string ValuePointAt(string tableFormulaName, string formulaOwnerName, double xDisplayValue)
         {
            return $"{tableFormulaName} in {formulaOwnerName}: X={xDisplayValue}";
         }
      }

      public static class ParameterIdentification
      {
         public static readonly string ParameterSelection = "Parameters";
         public static readonly string OutputSelection = "Outputs";
         public static readonly string DataSelection = "Data";
         public static readonly string Configuration = "Configuration";
         public static readonly string ParameterIdentificationDefaultName = "Parameter Identification";
         public static readonly string AddSimulation = "Add Simulation";
         public static readonly string AddOutput = "Add Output";
         public static readonly string Outputs = "Outputs";
         public static readonly string ObservedData = "Observed Data";
         public static readonly string Weight = "Weight";
         public static readonly string StartValue = "Start Value";
         public static readonly string OptimalValue = "Optimal Value";
         public static readonly string MinValue = "Min. Value";
         public static readonly string MaxValue = "Max. Value";
         public static readonly string InitialValue = "Initial Value";
         public static readonly string IdentificationParameters = "Identification Parameters";
         public static readonly string LinkedParameters = "Simulation Parameters";
         public static readonly string NumberOfRuns = "Number of Runs";
         public static readonly string AllTheSame = "All the same";
         public static readonly string All = "All";
         public static readonly string General = "General";
         public static readonly string AlgorithmParameters = "Algorithm Parameters";
         public static readonly string Algorithm = "Algorithm";
         public static readonly string ResidualCalculation = "Residual Calculation";
         public static readonly string Options = "Options";
         public static readonly string Residuals = "Residuals";
         public static readonly string ResidualCount = "# of Residuals";
         public static readonly string Results = "Results";
         public static readonly string NumberOfEvaluations = "Number of Evaluations";
         public static readonly string CompletedDate = "Completed Date";
         public static readonly string TotalError = "Total Error";
         public static readonly string Status = "Status";
         public static readonly string RunMessage = "Message";
         public static readonly string RunIndex = "Run #";
         public static readonly string UseAsFactor = "Use as Factor";
         public static readonly string IsFixed = "Fixed";
         public static readonly string LLOQMode = "Transform data below LLOQ";
         public static readonly string RemoveLLOQMode = "Remove data below LLOQ";
         public static readonly string TimeProfileAnalysis = "Time Profile";
         public static readonly string PredictedVsObservedAnalysis = "Predicted vs. Observed";
         public static readonly string SimulatedChartAxis = "Simulated";
         public static readonly string ObservedChartAxis = "Observed";
         public static readonly string MarkerDeviation = "Marker_Deviation";
         public static readonly string Deviation = "Deviation";
         public static readonly string ResidualsVsTimeAnalysis = "Residuals vs. Time";
         public static readonly string ResidualHistogramAnalysis = "Histogram of Residuals";
         public static readonly string RunResultsProperties = "Parameter Identification Run Properties";
         public static readonly string TransferToSimulation = "Transfer to Simulation";
         public static readonly string FeedbackView = "Parameter Identification Visual Feedback";
         public static readonly string RefreshFeedback = "Refresh";
         public static readonly string MultipleRunsAreBeingCreated = "Multiple parameter identification runs are being created. Runs status will be available shortly";
         public static readonly string UpdateStartValuesFromSimulation = "Update start values from simulation";
         public static readonly string SelectDirectoryForParameterIdentificationExport = "Select directory for parameter identification export";
         public static readonly string SelectFileForParametersHistoryExport = "Export parameters history to file";
         public static readonly string CorrelationMatrix = "Correlation Matrix";
         public static readonly string CovarianceMatrix = "Covariance Matrix";
         public static readonly string CalculateSensitivity = "Calculate Sensitivity";
         public static readonly string Duration = "Elapsed Time";
         public static readonly string ConfidenceInterval = "95% Confidence Interval";
         public static readonly string Parameter = "Parameter";
         public static readonly string ExportParametersHistory = "Parameters History";
         public static readonly string ParametersHistory = "Parameters History";
         public static readonly string CorrelationMatrixNotAvailable = $"Correlation matrix was not calculated. Ensure that the option '{CalculateSensitivity}' is selected";
         public static readonly string CovarianceMatrixNotAvailable = $"Covariance matrix was not calculated. Ensure that the option '{CalculateSensitivity}' is selected";
         public static readonly string ConfidenceIntervalNotAvailable = $"Confidence interval was not calculated. Ensure that the option '{CalculateSensitivity}' is selected";
         public static readonly string ErrorHistory = "Error History";
         public static readonly string Error = "Error";
         public static readonly string TimeProfilePredictionIntervalAnalysis = "Prediction Interval";
         public static readonly string TimeProfileConfidenceIntervalAnalysis = "Confidence Interval";
         public static readonly string TimeProfileVPCIntervalAnalysis = "Visual Predictive Check Interval";

         public static string FeedbackViewFor(string parameterIdentificationName)
         {
            return $"{FeedbackView} for {parameterIdentificationName}";
         }

         public static class RunModes
         {
            public static readonly string Standard = "Standard";
            public static readonly string MultipleRuns = "Multiple optimization (Randomize start values)";
            public static readonly string Category = "Calculation methods variation";
         }

         public static class LLOQModes
         {
            public static readonly string OnlyObservedData = "Observed data below LLOQ set to LLOQ/2";
            public static readonly string SimulationOutputAsObservedDataLLOQ = "Observed data and simulated data below LLOQ set to LLOQ";

            public static readonly string OnlyObservedDataDescription = "Observed data below LLOQ is set to LLOQ/2; simulated data below LLOQ is not changed.";
            public static readonly string SimulationOutputAsObservedDataLLOQDescription = "Observed and simulated data below LLOQ is set to LLOQ";
         }

         public static class RemoveLLOQModes
         {
            public static readonly string Never = "Never";
            public static readonly string NeverDescription = "All observed data will be used";
            public static readonly string NoTrailing = "When part of trailing series of LLOQ observations, reduce to single observation";
            public static readonly string NoTrailingDescription = "Trailing series are reduced. For example: from vector [LLOQ/2, LLOQ/2, 2.4, 1.0, 0.5, LLOQ/2, LLOQ/2, LLOQ/2] use only [LLOQ/2, LLOQ/2, 2.4, 1.0, 0.5, LLOQ/2])";
            public static readonly string Always = "Always";
            public static readonly string AlwaysDescription = "Observed data below LLOQ will not be used. Observed data with a value equal to 0 are not used for log-scaled outputs";
         }

         public static class Algorithms
         {
            public static readonly string NelderMeadPKSim = "Nelder - Mead (unconstrained optimization)";
            public static readonly string LevenberMarquardtMPFit = "Levenberg - Marquardt";
            public static readonly string MonteCarlo = "Monte - Carlo";
         }

         public static readonly string NoResultsAvailable = "No result available. Please start parameter identification";
         public static readonly string NoParameterIdentificationRunning = "No visual feedback available. Please start parameter identification.";

         public static string ParameterIdentificationCanceled(string parameterIdentificationName)
         {
            return $"Parameter identification '{parameterIdentificationName}' canceled";
         }

         public static readonly string Best = "Best";
         public static readonly string Current = "Current";
         public static readonly string Clone = "Clone";

         public static string ParameterIdentificationFinished(string parameterIdentificationName, string duration)
         {
            return $"Parameter identification '{parameterIdentificationName}' finished in {duration}";
         }


         public static string SensitivityCalculationFailed(string parameterIdentificationName, IReadOnlyList<string> errorMessages, string duration = null)
         {
            return string.IsNullOrEmpty(duration) ? 
               $"Parameter identification '{parameterIdentificationName}' finished but sensitivity calculation failed.\n\n {string.Join("\n\n", errorMessages)}" : 
               $"Parameter identification '{parameterIdentificationName}' finished in {duration} but sensitivity calculation failed.\n\n {string.Join("\n\n", errorMessages)}";
         }

         public static string LinkedParametersIn(string name)
         {
            return $"{LinkedParameters} in {name}";
         }

         public static string EditParameterIdentification(string name)
         {
            return $"Parameter Identification: '{name}'";
         }

         public static string CreateCurveDescription(string curveName, int? runIndex, string runDescription)
         {
            var sb = new StringBuilder();
            sb.AppendLine(curveName);

            if (runIndex != null)
               sb.AppendLine($"Run number: {runIndex}");

            if (!string.IsNullOrEmpty(runDescription))
               sb.AppendLine(runDescription);
            return sb.ToString();
         }

         public static string ReallyDeleteSimulationUsedInParameterIdentification(string simulationName)
         {
            return $"Really delete simulation '{simulationName}' currently used in the parameter identification?";
         }

         public static string CannotAddObservedDataPointBeingUsedByParameterIdentification(string observedDataName, IReadOnlyList<string> parameterIdentificationNames)
         {
            return $"Cannot add data point to observed data '{observedDataName}'. It is used by parameter {"identification".PluralizeIf(parameterIdentificationNames)} {parameterIdentificationNames.ToString(", ", "'")}";
         }

         public static string CannotDeleteObservedDataPointBeingUsedByParameterIdentification(string observedDataName, IReadOnlyList<string> parameterIdentificationNames)
         {
            return $"Cannot remove data point from observed data '{observedDataName}'. It is used by parameter {"identification".PluralizeIf(parameterIdentificationNames)} {parameterIdentificationNames.ToString(", ", "'")}";
         }

         public static string CannotRemoveObservedDataBeingUsedByParameterIdentification(string observedDataName, IReadOnlyList<string> parameterIdentificationNames)
         {
            return $"Cannot remove observed data '{observedDataName}'. It is used by parameter {"identification".PluralizeIf(parameterIdentificationNames)} {parameterIdentificationNames.ToString(", ", "'")}";
         }

         public static string CreateCurveNameForPredictedVsObserved(string observationName, string simulationResultName)
         {
            return $"{simulationResultName}  vs. {observationName}";
         }

         public static string OptimizationsCount(int parameterIdentificationCount)
         {
            return $"{parameterIdentificationCount} {"Optimization".PluralizeIf(parameterIdentificationCount)}";
         }

         public static string ParameterIdentificationTransferredToSimulations(string parameterIdentificationName)
         {
            return $"Identified parameters from '{parameterIdentificationName}' transferred to simulations.";
         }

         public static string CategorialDescriptionWithoutCompoundNameOrCategory(string calculationMethodName)
         {
            return $"{calculationMethodName}";
         }

         public static string CategorialDescriptionWithoutCompoundName(string calculationMethodCategory, string calculationMethodName)
         {
            return $"{calculationMethodCategory} - " + CategorialDescriptionWithoutCompoundNameOrCategory(calculationMethodName);
         }

         public static string CategorialDescription(string compoundName, string calculationMethodCategory, string calculationMethodName)
         {
            return compoundString(compoundName) + CategorialDescriptionWithoutCompoundName(calculationMethodCategory, calculationMethodName);
         }

         private static string compoundString(string compoundName)
         {
            return $"{compoundName} - ";
         }

         public static string CategorialDescriptionWithoutCategory(string compoundName, string calculationMethodName)
         {
            return compoundString(compoundName) + CategorialDescriptionWithoutCompoundNameOrCategory(calculationMethodName);
         }

         public static string SimulationResultsForRun(int runIndex)
         {
            return $"Simulation Results for Run {runIndex}";
         }

         public static string RandomStartValueRunNameFor(int runIndex)
         {
            return $"Random Start Values {runIndex}";
         }

         public static class AlgorithmProperties
         {
            public static class Names
            {
               public static readonly string Epsfcn = "Finite derivative step size";
               public static readonly string RelativeChiSquareConvergenceCriteriumFtol = "Relative chi-square convergence criterium (ftol)";
               public static readonly string RelativeParameterConvergenceCriteriumXtol = "Relative parameter convergence criterium (xtol)";
               public static readonly string OrthoganalityConvergenceCriteriumGtol = "Orthogonality convergence criterium (gtol)";
               public static readonly string InitialStepBoundFactor = "Initial step bound factor";
               public static readonly string MaximumNumberOfIterations = "Maximum number of iterations";
               public static readonly string MaximumNumberOfFunctionEvaluations = "Maximum number of function evaluations";
               public static readonly string ConvergenceTolerance = "Convergence tolerance";
               public static readonly string MaxNumberOfEvaluations = "Maximum number of evalutions";
               public static readonly string BreakCondition = "Break condition for relative error improvement";
               public static readonly string InitialAlpha = "Scale of projection degree (alpha)";
            }

            public static class Descriptions
            {
               public static readonly string Epsfcn =
                  "Used in determining a suitable step length for the forward-difference approximation.\nThis approximation assumes that the relative errors in the functions are of the order of epsfcn.\nIf epsfcn is less than the machine precision, it is assumed that the relative errors in the functions are of the order of the machine precision.";

               public static readonly string MaximumNumberOfIterations =
                  "The maximum number of iterations to perform. If the number of calculation iterations exceeds MAXITER, then the algorithm returns.\nIf MAXITER = 0, then the algorithm does not iterate to adjust parameter values;\nhowever, the user function is evaluated and parameter errors/covariance/Jacobian are estimated before returning.";

               public static readonly string RelativeChiSquareConvergenceCriteriumFtol = "Termination occurs when both the actual and predicted relative reductions in the sum of squares are at most ftol. Therefore, ftol measures the relative error desired in the sum of squares.";
               public static readonly string RelativeParameterConvergenceCriteriumXtol = "Termination occurs when the relative error between two consecutive iterates is at most xtol. Therefore, xtol measures the relative error desired in the approximate solution.";

               public static readonly string OrthoganalityConvergenceCriteriumGtol =
                  "Termination occurs when the cosine of the angle between fvec and any column of the jacobian is at most gtol in absolute value.\nTherefore, gtol measures the orthogonality desired between the function vector and the columns of the jacobian.";

               public static readonly string InitialStepBoundFactor =
                  "Used in determining the initial step bound. This bound is set to the product of factor and the euclidean norm of diag*x if nonzero, or else to factor itself.\nIn most cases factor should lie in the interval [0.1, 100]. 100 is a generally recommended value.";

               public static readonly string MaximumNumberOfFunctionEvaluations = "Termination occurs when the number of calls to objective function is greater or equal this value by the end of an iteration.\nIf the value is set to 0, then the number of evaluations is unlimited.";
               public static readonly string MaximumNumberOfIterationsMonteCarlo = "The maximum number of iterations to perform. If the number of calculation iterations exceeds this number, then the algorithm returns.";

               public static readonly string ConvergenceTolerance = "Relative convergence tolerance";
               public static readonly string MaxEvaluations = "Termination occurs when the number of calls to objective function is greater or equal this value";

               public static readonly string BreakCondition = "Termination occurs when the relative improvement of the error evaluation is less than the break condition.";
               public static readonly string InitialAlpha = "Start value for projection degree. Termination occurs when the minimal alpha is larger than 10 times alpha.";
            }
         }

         public static string ReallyDeleteParameterIdentifications(IReadOnlyList<string> names)
         {
            return $"Really delete {ObjectTypes.ParameterIdentification.ToLowerInvariant().PluralizeIf(names)} {names.ToString(", ", "'")}?";
         }

         public static string SimulationDoesNotContainParameterWithPath(string simulationName, string parameterPath)
         {
            return $"Simulation {simulationName} does not contain a parameter with path {parameterPath}";
         }

         public static string SimulationDoesNotUseObservedData(string simulationName, string observedDataName)
         {
            return $"Simulation {simulationName} does not use observed data {observedDataName}";
         }

         public static string SimulationDoesNotHaveOutputPath(string simulationName, string outputPath)
         {
            return $"Simulation {simulationName} does not have the output path {outputPath}";
         }

         public static string ValueUpdatedFrom(string parameterIdentification, string isoDate)
         {
            return $"Value updated from '{parameterIdentification}' on {isoDate}";
         }
      }

      public static class SensitivityAnalysis
      {
         public static readonly string SensitivityAnalysisDefaultName = "Sensitivity Analysis";
         public static readonly string NumberOfSteps = "Number of Steps";
         public static readonly string VariationRange = "Variation Range";
         public static readonly string FeedbackView = "Sensitivity Analysis Visual Feedback";
         public static readonly string ParameterSelection = "Parameters";
         public static readonly string Results = "Results";
         public static readonly string NoResultsAvailable = "No result available. Please start sensitivity analysis";
         public static readonly string ErrorsDuringPreviousRun = "<b>The last run resulted in one or more errors:</b>";

         public static string SensitivityAnalysisFinished(string duration)
         {
            return $"Sensitivity analysis finished in {duration}";
         }

         public static readonly string SensitivityAnalysisCanceled = "Sensitivity analysis canceled";
         public static readonly string NoSensitivityAnalysisRunning = "No visual feedback available. Please start sensitivity analysis.";
         public static readonly string SensitivityHasNotBeenUpdated = "Sensitivity analysis has not been updated";
         public static readonly string Output = "Output";
         public static readonly string PKParameterName = "PK-Parameter";
         public static readonly string PKParameterDescription = "PK-Parameter Description";
         public static readonly string ParameterName = "Parameter";
         public static readonly string ParameterDisplayPath = "Parameter Path";
         public static readonly string ParameterPath = "Path";
         public static readonly string Value = "Value";
         public static readonly string RemoveAll = "Remove All";
         public static readonly string SensitivityAnalysisPKParameterAnalysis = "Sensitivity Analysis";
         public static readonly string SensitivityAnalysisPKParameterAnalysisDescription = "Create a new chart displaying the sensitivity of the parameters which are responsible for 90% of the cumulated sensitivity";
         public static readonly string PKParameter = "PK-Parameter";
         public static readonly string AddAllConstants = "Add All Constants";
         public static readonly string ExportPKAnalysesSensitivityToExcel = $"Export to {Excel}";
         public static readonly string ExportPKAnalysesSensitivityToExcelTitle = $"Export PK-Analyses Sensitivity to {Excel}";
         public static readonly string OutputPath = "Output Path";
         public static readonly string SensitivityAnalysisCouldNotBeCalculated = "No sensitivity values available for this combination of output path and PK parameter";
         public static readonly string Selection = "Selection";
         public static readonly string All = "All";

         public static string SensitivityProgress(int simulationsCalculated, int totalSimulations)
         {
            return $"Simulation calculation {simulationsCalculated}/{totalSimulations} completed";
         }

         public static string EditSensitivityAnalysis(string name)
         {
            return $"Sensitivity Analysis: '{name}'";
         }

         public static string PkParameterOfOutput(string pkParameterName, string outputPath)
         {
            return $"{pkParameterName} of {outputPath}";
         }

         public static string ReallyDeleteSensitivityAnalyses(IReadOnlyList<string> names)
         {
            return $"Really delete {ObjectTypes.SensitivityAnalysis.ToLowerInvariant().PluralizeIf(names)} {names.ToString(", ", "'")}?";
         }

         public static readonly string ApplyValueToAllSensitivityParameters = "Apply value to all sensitivity parameters";
         public static readonly string ApplyValueToSelectedSensitivityParameters = "Apply value to selected sensitivity parameters";
         public static readonly string NumberOfStepsDescription = "Number of simulation evaluations at each side to the default value";
         public static readonly string VariationRangeDescription = "Parameter will be varied between <current value/(1+variation range)> and <current value*(1+variation range)>";
         public static readonly string AddAllConstantsDescription = "Add all parameters with a constant value to the sensitivity parameters";
         public static readonly string UpdateMultipleSensitivityParameters = "Update multiple sensitivity parameters";

         public static string NumberOfSelectedParameters(int numberOfParameters)
         {
            return $"Number of selected parameters: {numberOfParameters}";
         }

         public static string SensitivityAnalysisErrorMessage(IReadOnlyList<string> errorMessages)
         {
            var stringBuilder = new StringBuilder();
            errorMessages.Each(message =>
            {
               stringBuilder.AppendLine($"{message}");
               stringBuilder.AppendLine();
               stringBuilder.AppendLine();
            });
            
            return stringBuilder.ToString();
         }
      }

      public static class Chart
      {
         public static class DataBrowser
         {
            public static string RepositoryName = "Repository";
            public static string ColumnId = "Column Id";
            public static string DimensionName = "Dimension";
            public static string BaseGridName = "BaseGrid";
            public static string HasRelatedColumns = "Extra Columns";
            public static string Origin = "Origin";
            public static string Date = "Date";
            public static string Category = "Category";
            public static string Source = "Source";
            public static string QuantityName = "Q'Name";
            public static string QuantityType = "Type";
            public static string QuantityPath = "Path";
            public static string OrderIndex = "OrderIndex";
            public static string Used = "Used";
         }

         public static class CurveOptions
         {
            public static string Id = "Id";
            public static string Name = "Curve Name";
            public static string xData = "x-Data";
            public static string yData = "y-Data";
            public static string InterpolationMode = "Interpolation";
            public static string yAxisType = "y-Axis";
            public static string Visible = "Visible";
            public static string VisibleInLegend = "In Legend";
            public static string Color = "Color";
            public static string LineStyle = "Style";
            public static string Symbol = "Symbol";
            public static string LineThickness = "Thickness";
            public static readonly string RenameTemplate = "Rename Template";
         }

         public static class AxisOptions
         {
            public static string AxisType = "Axis";
            public static string Scaling = "Scaling";
            public static string NumberMode = "Numbers";
            public static string Dimension = "Dimension";
            public static string UnitName = "Unit";
            public static string yAxisType = "y-Axis";
            public static string GridLines = "Grid";
            public static string Min = "Min";
            public static string Max = "Max";
            public static string Caption = "Caption";
            public static string DefaultLinestyle = "Default Linestyle";
            public static string DefaultColor = "Default Color";
            public static readonly string NewName = "New Name";
         }

         public static class FontAndSizeSettings
         {
            public static readonly string PreviewSettings = "Preview these settings in Chart Display";
            public static readonly string Pixels = "px";
            public static readonly string Width = "Width";
            public static readonly string Height = "Height";
            public static readonly string FontSizeAxis = "Font Size Axis";
            public static readonly string FontSizeLegend = "Font Size Legend";
            public static readonly string FontSizeTitle = "Font Size Title";
            public static readonly string FontSizeDescription = "Font Size Description";
            public static readonly string FontSizeOrigin = "Font Size Origin";
            public static readonly string FontSizeWatermark = "Font Size Watermark";
            public static readonly string IncludeOriginData = "Include Origin Data";
         }

         public static class ColorGrouping
         {
            public static string ColorGroupingDialogDescription = "Select metadata as criteria for color grouping";
            public static string ApplyColorGroupingButton = "Apply Color Grouping";
         }

         public static class MultiCurveOptions
         {
            public static string CurrentValue = "<Current value>";
         }

         public static class GroupRowFormat
         {
            public static string Simulation = "Simulation";
            public static string Time = "Time";
            public static string Observation = "Observation";
            public static string DeviationLine = "Deviation Lines";
            public static string Undefined = "Undefined"; 
         }

         public static class DeviationLines
         {
            public static string SpecifyFoldValue = "Specify deviation fold value";
            public static string DeviationLineDescription = "Will create two deviation lines according to the given fold value which has to be greater than 1 (foldValue >1). An x-fold deviation range includes simulated values within x-fold and 1/x-fold of observed values.";
            public static string DeviationLineNameUpper(float foldValue) => $"{foldValue}-fold deviation";
            public static string DeviationLineNameLower(float foldValue) => $"{foldValue}-fold deviation Lower";

         }
      }

      private static string numberDisplay(int version) => $"(#{version})";

      private static string projectNameAndVersionAsString(string versionDisplay, int version) => $"V{versionDisplay} {numberDisplay(version)}";

      public static string ProjectVersionCannotBeLoaded(
         int projectVersion, 
         string oldestSupportedDisplayVersion, 
         int oldestSupportedVersion, 
         string currentSupportedDisplayVersion,
         int currentSupportedVersion,
         string downloadUrl)
      {
         var compositeOldestVersion = projectNameAndVersionAsString(oldestSupportedDisplayVersion, oldestSupportedVersion);
         var compositeCurrentVersion = projectNameAndVersionAsString(currentSupportedDisplayVersion, currentSupportedVersion);
         var preamble = $"This application is compatible with projects created between {compositeOldestVersion} and {compositeCurrentVersion}";

         if (projectVersion > currentSupportedVersion)
         {
            return $"{preamble}.\n\n" +
                   $"The version of this project {numberDisplay(projectVersion)} is newer and cannot be loaded.\n\n" +
                   $"Visit our download page at {downloadUrl}";
         }

         return $"{preamble}.\n\n" +
                $"The version of this project {numberDisplay(projectVersion)} is too old and cannot be loaded.\n\n" +
                $"Visit our download page at {downloadUrl} to download an older version of the software compatible with this project.";
      }
   }

   public static class Error
   {
      public static readonly string NameIsRequired = "Name is required.";
      public static readonly string ValueIsRequired = "Value is required.";
      public static readonly string DescriptionIsRequired = "Description is required";
      public static readonly string RenameSameNameError = "The new name is the same as the original one.";
      public static readonly string CanOnlyCompareTwoObjectsAtATime = "Object comparison is only available for two objects at the same time.";
      public static readonly string JournalNotOpen = "Journal is not open";
      public static readonly string NoPagesToExport = "There are no pages to export";
      public static readonly string TemplateShouldContainAtLeastOneCurve = "Template should contain at least one curve.";
      public static readonly string SessionFactoryNotInitialized = "Session factory is not initialized";
      public static readonly string SessionNotInitialized = "Session not initialized";
      public static readonly string SessionDisposed = "Session was disposed";
      public static readonly string OutputMappingHasInconsistentDimension = "Output mapping has inconsistent dimension";
      public static readonly string WeightValueCannotBeNegative = "Weights cannot be negative";
      public static readonly string DifferentXAxisDimension = "Different from X axis dimension";
      public static readonly string DifferentYAxisDimension = "Different from Y axis dimension";
      public static readonly string CannotConvertYAxisUnits = "Cannot convert to Y axis unit";
      public static readonly string MolWeightNotAvailable = "Molecular Weight not available.";
      public static readonly string NoResultsAvailableForExportToCSV = "No results available for export to CSV";
      public static readonly string NamingConventionEmpty = "Column naming conventions cannot be empty.";
      public static readonly string NamingConventionNull = "Column naming conventions cannot be null.";
      public static readonly string InvalidMappingColumn = "An invalid mapping column has been used. Check your import configuration and data file";
      public static readonly string InvalidErrorDimension = "The dimension of the error units must be the same as the dimension of the measurement units.";
      public static readonly string NaNOnData = "Data contains NaN values at imported columns. Select a different action for NaN values or clean your data.";
      public static readonly string UnsupportedFileType = "The type of file that you are trying to open is not currently supported";
      public static readonly string CannotRemoveBaseGridColumnStillInUse = "Cannot remove base grid column still used by other columns";
      public static readonly string SimpleParseErrorMessage = "There were errors while parsing your data. Navigate to the sheets to read the concrete error.";
      public static readonly string FoldValueMustBeGreaterThanOne = "Fold value must be a number greater than one.";
      public static readonly string ImporterEmptyFile = "The file you are trying to load is empty.";

      public static string CannotFindParentContainerWithPath(string parentPath, string containerName, string buildingBlockName, string moduleName) => 
         $"Cannot find parent container '{parentPath}' defined as target of container '{containerName}' in building block '{buildingBlockName}' in module '{moduleName}'";

      public static  string NoUnitColumnValues(string mappingName) => $"No values for the unit were found in the excel column mapped for '{mappingName}' \n";

      public static string ParseErrorMessage(string errors) => $"There were errors while parsing your data: {errors}";

      public static string ErrorWhenPlottingDataRepository(int sheetName, string exceptionMessage) =>
         $"It was not possible to plot the data sets. Please, check your configuration for any missing grouping or meta data parameter. An error occur while plotting data set number:{sheetName + 1} produced the following error: {exceptionMessage}";

      public static string SheetWithDuplicateHeader(string sheetName, IEnumerable<string> duplicateHeaders)
      {
         var sb = new StringBuilder();
         sb.AppendLine($"In sheet {sheetName} the headers \n \n{string.Join("\n", duplicateHeaders)} \nare duplicated.");
         return sb.ToString();
      }

      public static string InvalidObservedDataFile(string exceptionMessage)
      {
         var sb = new StringBuilder();
         sb.AppendLine("An error occurred while reading the file. Please check the content.");

         if (exceptionMessage.IsNullOrEmpty()) return sb.ToString();
         sb.AppendLine("The exception thrown was:");
         sb.AppendLine();
         sb.AppendLine(exceptionMessage);

         return sb.ToString();
      }

      public static string UnsupportedFileFormat(string fileName)
      {
         var sb = new StringBuilder();
         sb.AppendLine($"The file: \n \n'{fileName}' \n ");
         sb.AppendLine("that you are trying to load does not comply to one of the supported formats.");
         sb.AppendLine("You can find a documentation of the supported formats <href =https://docs.open-systems-pharmacology.org/shared-tools-and-example-workflows/import-edit-observed-data#supported-formats > here </href>");
         return sb.ToString();
      }

      public static readonly string InconsistentMoleculeAndMolWeightException = "Molecule and Molecular Weight do not match. Please either edit your Molecule or your Molecular Weight in your project or remove the Molecular Weight from your mappings";
      public static readonly string InvalidMixOfSimulationAndSimulationBatch = "You already have Simulation and SimulationBatch objects and should not mix, please invoke Clear to start adding objects from a fresh start";
      public static readonly string MismatchingArrayLengths = "Arrays should have the same length";
      public static string ColumnNotFound(string columnName) => $"Column {columnName} unexpectedly not found.";
      public static string BaseGridColumnNotFoundException(string columnName) => $"BaseGrid Column {columnName} unexpectedly not found.";
      public static string EmptyDataSet(string dataSetName) => $"Your settings ended up with following empty datasets: '{dataSetName}'. Please remove the data set from your data, filter it out or add at least one observation for it.";

      public static string MissingColumnException(string sheetName, IReadOnlyList<string> missingColumns) => $"The mapped column(s) \n \n '{missingColumns.ToString("\n")}' \n \n is missing at least from the sheet \n \n '{sheetName}' \n \n that you are trying to load.";

      public static string DimensionCannotBeDeterminedFor(string sheetName, string mappingName) => $"The dimension for mapping '{mappingName}' could not be determined in the sheet '{sheetName}'";
      public static string InvalidDimensionException(string invalidUnit, string mappingName) => $"The unit '{invalidUnit}' you are trying to assign to the mapping '{mappingName}' does not belong to a supported dimension of this mapping.";
      public static string InconsistentDimensionBetweenUnitsException(string mappingName) => $"For the mapping '{mappingName}' not all units in the mapped column belong to the same dimension.";
      public static string LinkedParameterIsNotValidInIdentificationParameter(string identificationParameterName) => $"At least one linked parameter is invalid in identification parameter '{identificationParameterName}'";

      public static string CannotDeleteBuildingBlockUsedBy(string buildingBlockType, string buildingBlockName, IReadOnlyList<string> usersOfBuildingBlock)
      {
         var content = usersOfBuildingBlock.Count > 1 ? $"\n{usersOfBuildingBlock.ToString("\n", "  ")}\n" : $"{usersOfBuildingBlock.ToString("", " ")}";

         return $"{buildingBlockType} '{buildingBlockName}' is used by{content}and cannot be deleted.";
      }

      public static string CannotDeleteObservedData(string observedDataName, IReadOnlyList<string> usersOfObservedData)
      {
         return CannotDeleteBuildingBlockUsedBy(ObjectTypes.ObservedData, observedDataName, usersOfObservedData);
      }

      public static string OutputsDoNotAllHaveTheSameScaling(string outputName)
      {
         return $"The output named '{outputName}' has been mapped more than once with different scalings";
      }

      public static string NumberOfCoreToUseShouldBeInferiorAsTheNumberOfProcessor(int processorCount)
      {
         return $"Number of processor to use should greater than 0 and less than or equal to  the number of processor on the machine ({processorCount} processors)";
      }

      public static string NameAlreadyExists(string name)
      {
         return $"'{name}' already exists.";
      }

      public static string NameAlreadyExistsInContainerType(string name, string containerType)
      {
         if(string.IsNullOrEmpty(containerType))
            return NameAlreadyExists(name);

         return $"'{name}' already exists in {containerType}.";
      }

      public static string NameCannotContainIllegalCharacters(IEnumerable<string> illegalCharacters)
      {
         return $"Name cannot contain any of the following characters:\n{illegalCharacters.ToString(", ", "'")}";
      }

      public static string CannotAddMetaDataDuplicateKey(string key, string existingValue)
      {
         return $"Attempting to add a meta data with a key:{key}. That key is already present in the repository with value:{existingValue}";
      }

      public static string ExistingValueInDataRepository(string name, float proposedValue, string units)
      {
         return $"There is already a row with {name} = {proposedValue} {units}";
      }

      public static string CouldNotFindMoleculeInContainer(string moleculeName, string containerPath)
      {
         return $"Could not find molecule '{moleculeName}' in container '{containerPath}'";
      }

      public static readonly string NoOutputMappingDefined = "No output mapping defined";
      public static readonly string NoIdentificationParameterDefined = "No identification parameter defined";
      public static readonly string CannotStartTwoConcurrentParameterIdentifications = "Cannot start two concurrent parameter identifications";
      public static readonly string CannotStartTwoConcurrentSensitivityAnalyses = "Cannot start two concurrent sensitivity analyses";
      public static readonly string NoOptimizationAlgorithmSelected = "No optimization algorithm selected";
      public static readonly string OutputMappingIsInvalid = "Output mapping is invalid";

      public static string SensitivityParameterIsInvalid(string name)
      {
         return $"Sensitivity parameter '{name}' is invalid";
      }

      public static readonly string CovarianceMatrixCannotBeCalculated = "Covariance matrix cannot be calculated";
      public static readonly string CorrelationMatrixCannotBeCalculated = "Correlation matrix cannot be calculated";

      public static string CannotSelectTheSamePartialProcessMoreThanOnce(string name) => $"'{name}' cannot be selected more than once.";

      public static string CannotSelectTheObservedDataMoreThanOnce(string name) => $"'{name}' cannot be selected more than once for the same output.";

      public static string DimensionMismatchError(IEnumerable<string> dimensionNames)
      {
         return $"Cannot mix different dimensions ({dimensionNames.ToString(" vs ", "'")})";
      }

      public static string IdentificationParametersAndValuesDoNotTheSameLength(int numberOfIdentificationParameters, int numberOfValues)
      {
         return $"Identification parameters and values do not have the same length ({numberOfIdentificationParameters} vs {numberOfValues})";
      }

      public static string CannotCreateChartPresenterForChart(Type chartType)
      {
         return $"Cannot create chart presenter for '{chartType}'";
      }

      public static string IdentificationParameterCannotBeFound(string name)
      {
         return $"Identification parameters '{name}' cannot be found.";
      }

      public static string EntityIsInvalid(string entityType, string name)
      {
         return $"{entityType} '{name}' is invalid";
      }

      public static string CannotFindSimulationResultsForOutput(string fullOutputPath)
      {
         return $"Could not retrieve simulation results for output '{fullOutputPath}'";
      }

      public static readonly string InvalidPKValueType = "PK value {0} has invalid type (e.g. complex)";
      public static readonly string TableFormulaWithOffsetUsesNonTableFormulaObject = "Object used in table formula with offset must be based an a table formula";
      public static readonly string ScaleFactorShouldBeGreaterThanZero = "Scale factor should be greater than 0";

      public static string TimeNotStrictlyMonotone(double valueBefore, double valueAfter, string displayUnit)
      {
         var hint = Equals(valueAfter, valueBefore) ? $"{valueBefore} {displayUnit} is duplicated" : $"{valueBefore} {displayUnit} is immediately followed by {valueAfter} {displayUnit}";

         return $"The time column is not strictly monotonically increasing ({hint}).\nEnsure that time always increases (e.g. 0.5, 1, 2, 4 hours).";
      }

      public static string UnableToFindEntityWithAlias(string alias) => $"Unable to find entity with alias '{alias}'";

      public static string WrongColumnDimensions(string columnName, int xDim, int yDim)
      {
         return $"Data column {columnName} has {xDim} values and its base grid has {yDim} values";
      }

      public const string OnlyLocalParametersInPassiveTransports = "Passive transport {0} contains parameters with BuildMode != LOCAL";

      public static string BothNeighborsSatisfying(string neighborhood) => $"Both neighbors of {neighborhood} satisfy the criteria";

      public static string BothNeighborsSatisfyingForTransport(string message, string transporter) => $"{message} for transport {transporter}";

      public static readonly string UnknownParameterBuildMode = "Unknown molecule parameter build mode";
      public static readonly string ConstMoleculeParameterInNeighborhood = "Constant parameters are not allowed in the molecule properties container of the neighborhood";
      public static readonly string NullParameter = "Cannot create parameter value: parameter not found in target container";
      public static readonly string NullMoleculeAmount = "Cannot create molecule initial condition: molecule amount not found in target container";
      public static readonly string NullInitialCondition = "Cannot create molecule initial condition: molecule container not found";
      public static readonly string NullFormulaCachePassedToClone = "Formula cache passed to clone function is null";
      public static readonly string EmptyMoleculeName = "Molecule name is empty";
      public static readonly string TransportMoleculeNamesNotSet = "Transport molecule names object passed is not set";
      public static readonly string TransportMoleculeNamesBothListsNonEmpty = "Molecule names to transport and molecule names not to transport are both nonempty";
      public static readonly string InvalidFile = "Invalid File";
      public static readonly string InvalidAuxiliaryType = "The error type is invalid. Valid types are 'ArithmeticStdDev' and 'GeometricStdDev'";
      public static readonly string MoreThanOneMeasurementColumnFound = "More than one measurement column was found in the data repository.";

      public static string UserDefinedPKParameterNotFound(string pkParameterName) => $"PK-Parameter '{pkParameterName}' not found";

      public static string MissingMoleculeContainerFor(string moleculeName) => $"Global molecule container for '{moleculeName}' was not found in root container";

      public static string UndefinedHelpParameter(string calculationMethod, string category)
      {
         return $"Cannot add undefined (null) help parameter in calculation method '{calculationMethod}' for category '{category}'";
      }

      public static string UndefinedFormulaInHelpParameter(string parameterName, string calculationMethod, string category)
      {
         return $"Cannot add help parameter '{parameterName}' with an undefined formula (null) in calculation method '{calculationMethod}' for category '{category}'";
      }

      public static string HelpParameterAlreadyDefinedWithAnotherFormula(string calculationMethod, string parameterPath)
      {
         return $"There is another parameter defined at '{calculationMethod}' with another formula. Help parameter for calculation method {parameterPath} cannot be created";
      }

      public static string FloatingMoleculeParameterNotDefined(string molecule, string parameter, double value)
      {
         return $"Parameter '{parameter}' defined in molecule '{molecule}' has an invalid value: {value}";
      }

      public static string MoleculeNameNotUnique(string moleculeName) => $"Molecule name '{moleculeName}' is not unique";

      public static string MoleculeNameExistsInAnotherList(string moleculeName)
      {
         return $"Cannot add molecule '{moleculeName}' into both molecules to include and molecules to exclude lists";
      }
      public static string BuildingBlockTypeAlreadyAddedToModule(string objectName, string type) => $"BuildingBlock '{type}' for '{objectName}' was already added to module";

      public const string NotImplemented = "This feature is not implemented yet";

      public static string CouldNotFindAReporterFor(Type type)
      {
         return $"Unable to find a reporter for '{type.Name}'";
      }

      public static string AliasAlreadyUsedInFormula(string alias, string formula)
      {
         return $"Alias '{alias}' is already used in formula '{formula}'";
      }

      public static string CouldNotLoadSimulationFromFile(string pkmlFileFullPath) => CouldNotLoadObjectFromFile(pkmlFileFullPath, ObjectTypes.Simulation);

      public static string CouldNotLoadObjectFromFile(string pkmlFileFullPath, string objectType)
      {
         return $"Could not load {objectType.ToLowerInvariant()}. Make sure that the file '{pkmlFileFullPath}' is a {objectType.ToLowerInvariant()} file.";
      }

      public static string CannotLoadRelatedItemCreatedWithAnotherApplication(string relatedItemType, string relatedItemName, string relatedItemApplication, string currentApplication)
      {
         return $"{relatedItemType} '{relatedItemName}' was created with {relatedItemApplication} and cannot be loaded in {currentApplication}.";
      }

      public static string CouldNotLoadTemplateFromFile(string templateFilePath)
      {
         return $"Could not load chart template. Make sure that the file '{templateFilePath}' is a template file.";
      }

      public static string CannotFindResource(string resourceFullPath) => $"Cannot find resource located at '{resourceFullPath}'";

      public static string IndividualIdDoesNotMatchTheValueLength(int individualId, int count)
      {
         return $"Individual Id '{individualId}' does not match the expected number of individual '{count}'. A reason could be that the results were imported starting with an id of 1 instead of 0.";
      }

      public static string CannotFindSimulationParameterForIdentificationParameter(string fullQuantityPath, string name)
      {
         return $"Cannot find simulation parameter '{fullQuantityPath}' used in identification parameter '{name}'";
      }

      public static string CaptionRowOutOfRange(int captionRow, int lastRowInWorkBook, string sheetName)
      {
         return rowOutOfRange(captionRow, lastRowInWorkBook, "caption", sheetName);
      }

      private static string rowOutOfRange(int captionRow, int lastRowInWorkBook, string rowName, string sheetName)
      {
         return string.Format("Cannot import data from {4} worksheet.{3}The {2} row ({0}) is greater than the total rows in the workbook ({1})", captionRow, lastRowInWorkBook, rowName, Environment.NewLine, sheetName);
      }

      public static string UnitRowOutOfRange(int unitRow, int lastRow, string sheetName)
      {
         return rowOutOfRange(unitRow, lastRow, "unit", sheetName);
      }

      public static string FirstDataRowOutOfRange(int firstDataRow, int lastRow, string sheetName)
      {
         return rowOutOfRange(firstDataRow, lastRow, "first data", sheetName);
      }

      public static string RemoveHigherAxisTypeFirst(string type) => $"Please remove Y-Axis {type} first.";

      public static string LastDataRowLessThanFirstDataRow(int dataEndRow, int dataStartRow, string sheetName)
      {
         return $"Cannot import data from {sheetName} worksheet.{Environment.NewLine}The first data row index is greater than the last data row index";
      }

      public static string ExportToCsvNotSupportedForDifferentBaseGrid = "Export to CSV is only supported for data columns sharing the same base grid.";

      public const string MESSAGE_ERROR_NAN = "Error information has been truncated because invalid values have been replaced by NaN.\n\n An arithmetic error must be at least 0.\n A geometric error must be at least 1.\n";

      public const string QualificationOutputFolderNotDefined = "Qualification output folder not defined.";

      public const string QualificationObservedDataFolderNotDefined = "Qualification observed data folder not defined.";

      public const string QualificationMappingFileNotDefined = "Qualification mapping file not defined.";

      public const string QualificationReportConfigurationFileNotDefined = "Qualification report configuration file not defined.";

      public static string ParameterValuesDoNotHaveTheExpectedCount(string parameterPath, int expectedCount, int actualCount)
      {
         return $"Parameter values for '{parameterPath}' does not have the expected number of elements. (Expected {expectedCount} vs Actual {actualCount})";
      }

      public static string SimulationResultsFileDoesNotHaveTheExpectedFormat
      {
         get
         {
            var sb = new StringBuilder();
            sb.AppendLine("Simulation result files does not have the expected format:");
            sb.AppendLine(" - Column headers are required (e.g. IndividualId;Time;....)");
            sb.AppendLine(" - The 1st column represents the individual id");
            sb.AppendLine(" - The 2nd column represents the time values");
            return sb.ToString();
         }
      }

      public static string SimulationPKAnalysesFileDoesNotHaveTheExpectedFormat
      {
         get
         {
            var sb = new StringBuilder();
            sb.AppendLine("Simulation pk-Analyses files does not have the expected format:");
            sb.AppendLine(" - Column headers are required (e.g. IndividualId;Output;Parameter;Value;Unit)");
            sb.AppendLine(" - The 1st column represents the individual id");
            sb.AppendLine(" - The 2nd column represents the output for which the PK-Parameter was calculated");
            sb.AppendLine(" - The 3rd column represents the Name of the PK-Parameter");
            sb.AppendLine(" - The 4th column represents the Value of the PK-Parameter");
            sb.AppendLine(" - The 5th column represents the Unit in which the value in the 4th column is saved");
            return sb.ToString();
         }
      }

      public const string SimulationDidNotProduceResults = "Simulation did not produce results";

      public static string DuplicatedIndividualResultsForId(int individualId) => $"Individual results for individual with id '{individualId}' were defined more than once!";

      public static string DuplicatedPKParameterSensitivityFor(string id) => $"PKParameter sensitivity results for '{id}' were defined more than once!";

      public static string CouldNotFindQuantityWithPath(string quantityPath) => $"Could not find quantity with path '{quantityPath}'.";
      public static string CouldNotFindContainerWithPath(string containerPath, string referringEntityPath) => $"Could not find container with path '{containerPath}'.\n\nReferenced by '{referringEntityPath}'.";

      public static string IndividualResultsDoesNotHaveTheExpectedQuantity(int individualId, IReadOnlyList<string> expectedQuantities, IReadOnlyList<string> foundQuantities)
      {
         var sb = new StringBuilder();
         sb.AppendLine($"Individual results for individual '{individualId}' does not have the expected results:");
         sb.AppendLine($"Expected: {expectedQuantities.ToString(",")}");
         sb.AppendLine($"Found: {foundQuantities.ToString(",")}");
         return sb.ToString();
      }

      public static string TimeArrayLengthDoesNotMatchFirstIndividual(int id, int expectedLength, int currentLength)
      {
         return $"Time array for individual '{id}' does not have the expected length ({expectedLength} vs {currentLength}).";
      }

      public static string TimeArrayValuesDoesNotMatchFirstIndividual(int id, int index, float expectedValue, float currentValue)
      {
         return $"Time array for individual '{id}' does not have the expected value in row '{index}' ({expectedValue} vs {currentValue}).";
      }

      public static string CouldNotFindDimensionWithUnit(string unit) => $"Could not find dimension containing unit '{unit}'.";

      public static string UnitIsNotDefinedInDimension(string unit, string dimension) => $"Unit '{unit}' is not defined in dimension '{dimension}'.";

      public static string CouldNotFindNeighborhoodBetween(string container1, string container2, string formulaName, string usingFormulaPath) => 
         $"Could not find neighborhood between '{container1}' and '{container2}' referenced by formula '{formulaName}' used by '{usingFormulaPath}'";

      public static string FirstNeighborNotDefinedFor(string neighborhoodName) => $"First neighbor is undefined for neighborhood '{neighborhoodName}'";
      
      public static string SecondNeighborNotDefinedFor(string neighborhoodName) => $"Second neighbor is undefined for neighborhood '{neighborhoodName}'";

      public const string InParentTagCanOnlyBeUsedWithAndOperator = "IN PARENT tag can only be used with AND operator";

      public static string KeywordCannotBeInFirstPosition(string keyword, string path) => $"Keyword '{keyword}' cannot be used in first position in '{path}'";

      public static string KeywordCanOnlyBeUsedOnce(string keyword, string path) => $"Keyword '{keyword}' can only be used once in '{path}'";

      public static string LumenNavigationKeywordLCanOnlyBeUsedOnce(string path) => $"Only one lumen navigation keyword can be used once in '{path}'";

      public static string ContainerIsNotLumenSegment(string path) => $"Container '{path}' is not a lumen segment";

      public static string CannotNavigateBeyondLumenSegment(string keyword, string path) => $"Usage of keyword '{keyword}' is not valid for '{path}'";


      public static class SensitivityAnalysis
      {
         public static readonly string NoSimulationDefined = "No simulation defined";
         public static readonly string NoSensitivityParameterDefined = "No sensitivity parameter defined";

         public static readonly string OnlyInactiveSensitivityParameterDefined =
            "Only parameters equal to zero selected. To prevent accidental structural model changes, the requested sensitivity calculation is not available for those parameters.\nTo override this behavior, please disturb the corresponding parameters in used simulation by a tiny amount.";

         public static string NoOutputAvailableInSelectedSimulation(string simulationName) => $"No output available in simulation '{simulationName}'";
         public static string SimulationDoesNotHaveParameterPath(string simulationName, string path) => $"The simulation '{simulationName}' does not contain a parameter with path '{path}'";

         public static string TheParameterPathCannotBeFoundInTheSimulation(string fullQuantityPath)
         {
            return $"The parameter path '{fullQuantityPath}' could not be found in the simulation";
         }
      }

      public static class MPFit
      {
         public static readonly string GeneralInputParameterError = "General input parameter error";
         public static readonly string UserFunctionNonFinite = "User function produced non-finite values";
         public static readonly string NoUserFunctionSupplied = "No user function was supplied";
         public static readonly string NoUserDataPoints = "No user data points were supplied";
         public static readonly string NoFreeParameters = "No free parameters";
         public static readonly string MemoryAllocationError = "Memory allocation error";
         public static readonly string InitialValuesInconsistent = "Initial values inconsistent with constraints";
         public static readonly string InitialConstraintsInconsistent = "Initial constraints inconsistent";
         public static readonly string NotEnoughDegreesOfFreedom = "Not enough degrees of freedom";
         public static readonly string ErrorObjectiveFunction = "Error in objective function: {0}";
         public static string UnknownStatus(string status) => $"Unknown status: {status}";
         public static readonly string GeneralInputError = "General input error";
         public static string OptimizationFailed(string error) => $"Levenberg-Marquardt optimization failed: {error}";
      }

      public static string FileSizeExceedsMaximumSize(string fileSizeInMegaBytes, string maxSizeInMegaBytes)
      {
         return $"The selected file size is '{fileSizeInMegaBytes} MB' and exceeds the maximum supported size of '{maxSizeInMegaBytes} MB'.";
      }

      public static string WildCardRecursiveCannotBePartOfPath(string wildCardRec, string invalidPath, string correctedPath)
      {
         var sb = new StringBuilder($"The '{wildCardRec}' search pattern cannot be used as part of a path entry:");
         sb.AppendLine();
         sb.AppendLine($"Instead of '{invalidPath}'");
         sb.AppendLine($"use '{correctedPath}'");
         return sb.ToString();
      }

      public static string IndividualWithIdNotFound(int individualId) => $"Individual with id `{individualId}` not found.";

      public static string TypeNotSupported(string typeName) => $"{typeName} is not currently been handled";

      public static string CannotAccessValueByPathUsingWildCard(string path) => $"Accessing value by path is not supported for path containing wildcard ({path})";

      public static string EquivalentNeighborhoodsAreDefinedFor(string firstNeighbor, string secondNeighbor)
      {
         return $"More than one neighborhood is defined connecting '{firstNeighbor}' and '{secondNeighbor}'";
      }

      public static string NeighborhoodDefinition(string neighborhoodName, string firstNeighbor, string secondNeighbor, string spatialStructure)
      {
         return $"Neighborhood '{neighborhoodName}' connects '{firstNeighbor}' and '{secondNeighbor}' in '{spatialStructure}'";
      }

      public static string CouldNotCalculateSensitivity(IReadOnlyList<string> runResultErrorMessages)
      {
         return $"Error calculating sensitivity\n{listErrorMessages(runResultErrorMessages)}";
      }

      private static string listErrorMessages(IReadOnlyList<string> runResultErrorMessages)
      {
         var sb = new StringBuilder();
         foreach (var errorMessage in runResultErrorMessages)
         {
            sb.AppendLine($"- {errorMessage}");
         }
         return sb.ToString();
      }
   }

   public static class Validation
   {
      public const string ValueIsRequired = "Value is required";
      public static readonly string NameIsRequired = "Name is required";
      public static readonly string OutputFileNotValid = "Please specify an output file with full path information.";
      public static readonly string StartTimeLessThanOrEqualToEndTime = "Start time value should be less than end time value.";
      public static readonly string EndTimeGreaterThanOrEqualToStartTime = "End time value should be greater than start time value.";

      public static string FileDoesNotExist(string fileFullPath)
      {
         return $"File '{fileFullPath}' does not exist";
      }

      public static string FormulaIsBlackBoxIn(string entity, string entityPath)
      {
         return $"A formula, which should be replaced during simulation creation, was not replaced. These types of formulas are known as Black Box Formulas. Check parameter definitions for the following entity '{entityPath}'." +
                "See https://tinyurl.com/BlackBoxFormula for more information.";
      }

      public static string PathContainsReservedKeywords(string entity, string entityType, string entityPath, string reference)
      {
         return $"Reference '{reference}' used in {entityType.ToLower()} '{entity}' with path '{entityPath}' contains reserved keywords. Remove them before resolving the path";
      }

      public static string ModelNameCannotBeNamedLikeATopContainer(IReadOnlyCollection<string> topContainerNames)
      {
         return $"The simulation name cannot be one of the following reserved terms:\n\t{topContainerNames.ToString(", ", "'")}.\nPlease rename your simulation.";
      }

      public static string ErrorUnableToFindReference(string entity, string entityType, string entityPath, string reference)
      {
         return string.Format("{1} '{2}' with path '{3}' references an entity with path '{0}' that cannot be found", reference, entityType, entity, entityPath);
      }

      public static string WarningNoReactionCreated(string reactionBuilder)
      {
         return $"Reaction '{reactionBuilder}' was not created.\nOne reactant is either not present and/or a matching physical container was not found.";
      }

      public static string FormulaIsNotValid(string formula, string buildingBlock, string errorMessage)
      {
         return $"Invalid Formula {formula} in Building Block {buildingBlock}: {errorMessage}";
      }

      public static string Removed(string path)
      {
         return $"Entity with path '{path}' not created.";
      }

      public static string CannotCreateApplicationSourceNotFound(string applicationName, string moleculeName, string containerName)
      {
         return cannotCreateApplicationNotFound(applicationName, moleculeName, containerName, "source");
      }

      public static string CannotCreateApplicationTargetNotFound(string applicationName, string moleculeName, string containerName)
      {
         return cannotCreateApplicationNotFound(applicationName, moleculeName, containerName, "target");
      }

      private static string cannotCreateApplicationNotFound(string applicationName, string moleculeName, string containerName, string type)
      {
         return string.Format("Cannot create application '{0}': molecule '{1}' not available in the {3} container '{2}'", applicationName, moleculeName, containerName, type);
      }

      public static string TransportAlreadyCreatorForMolecule(string applicationName, string transportName, string moleculeName)
      {
         return $"Cannot create application '{applicationName}': Transport '{transportName}' already created for molecule '{moleculeName}'. Please check Application configuration (Tags, Source/Target Container, etc.).";
      }

      public static string MultipleNotificationsFor(string notificationType, string builderName)
      {
         return $"Multiple {notificationType}s were found for '{builderName}'";
      }

      public static string ApplicatedMoleculeNotPresent(string moleculeName, string applicationBuilderName)
      {
         return $"Molecule: '{moleculeName}' applicated by Application builder {applicationBuilderName} is not present in simulation";
      }

      public static string ValueGreaterThanMinSizeInPixelAndLessThanMaxSizeIsRequiredOrEmpty(int minSizeInPixel, int maxSizeInPixel)
      {
         return $"A value greater than {minSizeInPixel} px and less than {maxSizeInPixel} px is required. To use current chart size, leave this empty";
      }

      public static string ValueSmallerThanMax(string parameterName, string value, string unit)
      {
         return $"Value for {parameterName} should be less than or equal to {value} {unit}";
      }

      public static string ValueBiggerThanMin(string parameterName, string value, string unit)
      {
         return $"Value for {parameterName} should be greater than or equal to {value} {unit}";
      }

      public static string ValueStrictBiggerThanMin(string parameterName, string value, string unit)
      {
         return $"Value for {parameterName} should be strictly greater than {value} {unit}";
      }

      public static string ValueStrictSmallerThanMax(string parameterName, string value, string unit)
      {
         return $"Value for {parameterName} should be strictly less than {value} {unit}";
      }

      public static string StartValueDefinedForNonPhysicalContainer(string moleculeName, string containerPath)
      {
         return $"Initial condition defined for molecule '{moleculeName}' in non-physical container '{containerPath}'";
      }

      public static string StartValueDefinedForContainerThatCannotBeResolved(string moleculeName, string containerPath)
      {
         return $"Initial condition defined for molecule '{moleculeName}' in a container '{containerPath}' that cannot be resolved";
      }

      public static string CircularReferenceFoundInEventAssignment(string eventName, string changeedEntityName, string entityType, string entityPath, IReadOnlyList<string> references)
      {
         var separator = "\n\t- ";
         return $"A circular reference was found in assignment of a formula to {entityType.ToLower()} '{changeedEntityName}' with path '{entityPath}' in event '{eventName}'\nPlease check the direct or indirect references resulting from the assignment:{separator}{references.ToString(separator)}";
      }

      public static string CircularReferenceFoundInFormula(string entity, string entityType, string entityPath, IReadOnlyList<string> references)
      {
         var separator = "\n\t- ";
         return $"A circular reference was found in formula of {entityType.ToLower()} '{entity}' with path '{entityPath}'\nPlease check the direct or indirect references:{separator}{references.ToString(separator)}";
      }

      public static string AxisMaxMustBeGreaterThanOrEqualToAxisMin(float? axisMinimumValue)
      {
         var preamble = "The axis maximum value should be greater than or equal to the axis minimum value";
         return axisMinimumValue.HasValue ? $"{preamble} '{axisMinimumValue}'" : preamble;
      }

      public static string AxisMinMustBeLessThanOrEqualToAxisMax(float? axisMaximumValue)
      {
         var preamble = "The axis minimum value should be less than or equal to the axis maximum value";
         return axisMaximumValue.HasValue ? $"{preamble} '{axisMaximumValue}'" : preamble;
      }

      public static readonly string LogAxisMaxMustBeGreaterThanZero = "Loagarithmic axis maximum must be greater than 0";
   }

   public static class Rules
   {
      public static class Parameters
      {
         public static readonly string MinLessThanMax = "Minimum value should be less than than maximum value.";
         public static readonly string MaxGreaterThanMin = "Maximum value should be greater than minimum value.";
         public static readonly string ValueShouldBeBetweenMinAndMax = "Value should be greater than minimum value and smaller than maximum value.";
         public static readonly string MinShouldBeStrictlyGreaterThanZeroForLogScale = "Minimum value should be greater than zero or the scaling should be set to linear.";

         public static string ValueShouldBeGreaterThanOrEqualToZero(string parameterName) => $"{parameterName} value should be greater than or equal to 0.";

         public static string MinimumMustBeGreaterThanOrEqualTo(string minDisplayValue, string displayUnit, string fullQuantityPath)
         {
            return $"The minimum value must be greater than or equal to {valueWithUnit(minDisplayValue, displayUnit)} for parameter '{fullQuantityPath}'";
         }

         public static string MinimumMustBeGreaterThan(string minDisplayValue, string displayUnit, string fullQuantityPath)
         {
            return $"The minimum value must be greater than {valueWithUnit(minDisplayValue, displayUnit)} for parameter '{fullQuantityPath}'";
         }

         public static string MaximumMustBeLessThanOrEqualTo(string maxDisplayValue, string displayUnit, string fullQuantityPath)
         {
            return $"The maximum value must be less than or equal to {valueWithUnit(maxDisplayValue, displayUnit)} for parameter '{fullQuantityPath}'";
         }

         public static string MaximumMustBeLessThan(string maxDisplayValue, string displayUnit, string fullQuantityPath)
         {
            return $"The maximum value must be less than {valueWithUnit(maxDisplayValue, displayUnit)} for parameter '{fullQuantityPath}'";
         }

         private static string valueWithUnit(string value, string unit) => string.IsNullOrEmpty(unit) ? value : $"{value} {unit}";
      }
   }

   public static class Messages
   {
      public static readonly string CreatingModel = "Creating Model...";
      public static readonly string ImportingResults = "Importing results...";

      public static string FollowingOutputsWereSuccessfullyImported(int numberOfIndividuals) =>
         $"Simulation results for {numberOfIndividuals} individuals were successfully imported for the following quantities:";

      public static readonly string FollowingPKParameterSensitivityWereSuccessfullyImported = "PKParameter sensitivities were successfully imported for the following quantities:";

      public static string CalculationPopulationSimulation(int number, int total) => $"Simulation {number}/{total}...";

      public static readonly string FollowingPKParametersWereSuccessfullyImported = "Following PK-Parameters were successfully imported:";
   }

   public static class Warning
   {
      public static readonly string OptimizedValueIsCloseToBoundary = "Identified value is close to boundary";
      public static readonly string ImportingParameterIdentificationValuesFromCancelledRun = "This parameter identification run was cancelled.\nDo you really want to import the identified parameters?";
      public static readonly string ImportingParameterIdentificationValuesFromCategorialRun = "Only the VALUES of the identified parameters will be transferred.\nPlease set the calculation methods manually.";
      public static readonly string CurveNameIsMissing = "Curve name is missing";
      public static readonly string PopulationFileFormatIsNotSupported = "Population file format is not supported.";
      public static readonly string SensitivityAnalysisFileFormatIsNotSupported = "Sensitivity analysis file format is not supported.";

      public static string ParameterWithPathNotFoundInBaseIndividual(string parameterPath) => $"Parameter '{parameterPath}' was not found in individual and will be ignored.";

      public static string ContainerNotFoundParameterWillNotBeCreated(string containerPath, string parameterName) => $" Container '{containerPath}' was not found. Parameter '{parameterName}' will be ignored.";

      public static string UserDefinedPKParameterAlreadyExistsAndWillBeReplaced(string pkParameterName) => $"User Defined PK-Parameter '{pkParameterName}' already exists and will be replaced.";

      public static string LargeNumberOfOutputPoints(int numberOfPoints) =>
         $"The selected output resolution will generate {numberOfPoints} points and may severely impact the software performance.\nAre you sure you want to run with these setting? If not, consider changing output resolution in simulations settings";

      public static string NeighborhoodWasNotFoundInModel(string neighborhoodName, string buildingBlockName) => $"The neighborhood '{neighborhoodName}' from building block '{buildingBlockName}' was not added to the simulation";
   }

   public static class RibbonCategories
   {
      public static readonly string ParameterIdentification = "Parameter Identification";
      public static readonly string SensitivityAnalysis = "Sensitivity Analysis";
   }

   public static class RibbonPages
   {
      public static readonly string RunParameterIdentification = "Run & Analyze";
      public static readonly string RunSensitivityAnalysis = "Run & Analyze";
      public static readonly string ParameterIdentificationAndSensitivity = "Parameter Identification & Sensitivity";
   }

   public static class Ribbons
   {
      public static readonly string ParameterIdentification = "Parameter Identification";
      public static readonly string SensitivityAnalysis = "Sensitivity Analysis";
      public static readonly string ParameterIdentificationAnalyses = "Analyses";
      public static readonly string ParameterSensitivityAnalyses = "Analyses";
      public static readonly string ParameterIdentificationConfidenceInterval = "Confidence Intervals";
   }

   public static class MenuNames
   {
      public static string AsDeveloperOnly(string menuName) => $"{menuName} (Developer only)...";

      public static readonly string NewExpressionProfile = "Add &Expression Profile";
      public static readonly string ExportToExcel = "Export to Excel...";
      public static readonly string ExportToPng = "Export to Png...";
      public static readonly string CopyToClipboard = "Copy to Clipboard";
      public static readonly string ResetZoom = "Reset Zoom";
      public static readonly string Delete = "Delete...";
      public static readonly string Edit = "Edit...";
      public static readonly string Remove = "Remove...";
      public static readonly string DeleteSubMenu = "Delete";
      public static readonly string Rename = "Rename...";
      public static readonly string GroupBy = "Create Subfolders By...";
      public static readonly string DeleteSubFoldersAndKeepData = "All Subfolders and Keep Data";
      public static readonly string DeleteSubFoldersAndData = "All Subfolders and Data";
      public static readonly string DeleteFolderAndKeepData = "Folder and Keep Data";
      public static readonly string DeleteFolderAndData = "Folder and Data";
      public static readonly string CreateGroup = "Create Subfolder...";
      public static readonly string EditAllMetaData = "Edit All MetaData...";
      public static readonly string EditMetaData = "Edit MetaData...";
      public static readonly string RemoveEmptyGroups = "Empty Subfolders";
      public static readonly string ManageProjectDisplayUnits = "Manage Project Display Units";
      public static readonly string ManageUserDisplayUnits = "Manage User Display Units";
      public static readonly string UpdateAllToDisplayUnits = "Update All Display Units";
      public static readonly string ApplyChartTemplate = "Apply Chart Template";
      public static readonly string NoTemplateAvailable = "No Template Available";
      public static readonly string ChartTemplate = "Chart Template";
      public static readonly string CreateNewTemplate = "Create New Template";
      public static readonly string UpdateExistingTemplate = "Update Existing Template";
      public static readonly string ManageTemplates = "Manage Templates...";
      public static readonly string Diff = "Show Differences...";
      public static readonly string FromCurrentChart = "From Current Chart";
      public static readonly string SaveFavoriteToFile = "Save";
      public static readonly string LoadFavoritesFromFile = "Load";
      public static readonly string ExportJournal = "Export Journal";
      public static readonly string RefreshJournal = "Refresh Journal";
      public static readonly string Compare = "Compare";
      public static readonly string HideRelatedItems = "Hide Related Items";
      public static readonly string ShowRelatedItems = "Show Related Items";
      public static readonly string Layout = "Layout";
      public static readonly string SaveToUserSettings = "Save to User Settings";
      public static readonly string CustomizeEditorLayout = AsDeveloperOnly("Customize Layout");
      public static readonly string SaveChartLayoutToFile = AsDeveloperOnly("Save Layout to File");
      public static readonly string StartParameterIdentification = "Start Parameter Identification...";
      public static readonly string AddParameterIdentification = "Add Parameter Identification";
      public static readonly string AddSensitivityAnalysis = "Add Sensitivity Analysis";
      public static readonly string CreateParameterIdentification = "Create";
      public static readonly string CreateSensitivityAnalysis = "Create";
      public static readonly string RunParameterIdentification = "Run";
      public static readonly string StopParameterIdentification = "Stop";
      public static readonly string FeedbackView = "Show Visual Feedback";
      public static readonly string ReplaceSimulation = "Replace Simulation";
      public static readonly string ExportForR = "Export for R";
      public static readonly string ExportForMatlab = AsDeveloperOnly("Export for Matlab®");
      public static readonly string ExportForCpp = AsDeveloperOnly("Export to C++ code");
      public static readonly string ExportSimModelXml = AsDeveloperOnly("Export SimModel Xml");
      public static readonly string ExportODEForR = "Export Simulation to R Differential Equations";
      public static readonly string ExportODEForMatlab = "Export Simulation to Matlab® Differential Equations";
      public static readonly string Clone = "Clone";
      public static readonly string RunSensitivityAnalysis = "Run";
      public static readonly string StopSensitivityAnalysis = "Stop";
      public static readonly string StartSensitivityAnalysis = "Start Sensitivity Analysis...";
      public static readonly string ClearHistory = "Clear History";
      public static readonly string ColorGroupObservedData = "Color by folder when adding to chart";
      public static readonly string AddDeviationLines = "Add deviation lines...";

      public static readonly string Help = "Help";

      public static string CompareObjects(string objectType)
      {
         return $"Compare {objectType}s";
      }

      public static readonly string AutoUpdateChart = "Auto-update chart";
      public static readonly string ApplyUpdates = "Apply updates";
   }

   public static class MenuDescriptions
   {
      public static readonly string NewExpressionProfileDescription = "Create a new expression profile...";
      public static readonly string UpdateChartTemplateFromCurrentSettings = "Updates the template from current chart settings";
      public static readonly string ApplyTemplateToCurrentChart = "Applies the template to the current chart";
      public static readonly string CreateNewTemplateFromCurrentChartSettings = "Create new template from current chart settings";
      public static readonly string ManageTemplates = "Manage all simulation templates";
      public static readonly string SaveFavoritesToFile = "Save favorites to file";
      public static readonly string LoadFavoritesFromFile = "Load favorites from file";
      public static readonly string ExportJournalToFile = "Export visible journal pages to a single file. Hide pages using search and filter.";
      public static readonly string RefreshJournal = "Refresh all journal pages.";
      public static readonly string CreateParameterIdentification = "Create a new parameter identification.";
      public static readonly string CreateSensitivityAnalysis = "Create a new sensitivity analysis.";
      public static readonly string RunParameterIdentification = "Run the active parameter identification.";
      public static readonly string StopParameterIdentification = "Stop the current parameter identification.";
      public static readonly string UnlinkParameter = "Unlink the Parameter";
      public static readonly string TimeProfileAnalysisDescription = "Create a new chart displaying the optimized time profile in comparison to the observed data.";
      public static readonly string PredictedVsObservedAnalysisDescription = "Create a new chart displaying the optimized output vs. the observed data.";
      public static readonly string ResidualsVsTimeAnalysisDescription = "Create a new chart displaying the residuals vs. time.";
      public static readonly string ResidualHistogramAnalysisDescription = "Create a new chart displaying the histogram of residuals.";
      public static readonly string ParameterIdentificationFeedbackViewDescription = $"Show or hide the {Captions.ParameterIdentification.FeedbackView.ToLower()}";
      public static readonly string SensitivityAnalysisFeedbackViewDescription = $"Show or hide the {Captions.SensitivityAnalysis.FeedbackView.ToLower()}";
      public static readonly string CorrelationMatrix = "Show the correlation matrix.";
      public static readonly string CovarianceMatrix = "Show the covariance matrix.";
      public static readonly string TimeProfilePredictionInterval = "Plot a pointwise linearization of the 95% prediction interval due to both the parameter uncertainty and the measurement error. This plot is only available for measured quantities.";
      public static readonly string TimeProfileConfidenceInterval = "Plot a pointwise linearization of the 95% confidence interval due to the parameter uncertainty.";
      public static readonly string TimeProfileVPCInterval = "Plot a pointwise linearization of the 95% visual predictive check interval due to the measurement error. This plot is only available for measured quantities.";
      public static readonly string RunSensitivityAnalysis = "Run the active sensitivity analysis.";
      public static readonly string StopSensitivityanalysis = "Stop the current sensitivity analysis.";
   }

   public static class Diagram
   {
      public static class Base
      {
         public static readonly SizeF GridCellSize = new SizeF(10F, 15F);
         public static readonly float PortGravity = 40F;
         public static readonly float MinLimitDocScale = 1 / 5F;
         public static readonly float MaxLimitDocScale = 2F;
         public static readonly PointF InsertLocationOffset = new PointF(0F, 30F);
         public static readonly float ZoomFitToPageFactor = 0F;
         public static readonly int LayoutDepthChildren = 0;
         public static readonly int LayoutDepthGrandChildren = 1;
         public static readonly int LayoutDepthAll = 5;
      }

      public static class Reaction
      {
         public static readonly float ZoomInFactor = 4 / 3F;
         public static readonly PointF OldMoleculeNodeOffsetInRename = new PointF(10F, 15F);
      }
   }

   public static class Command
   {
      public static readonly string CommandTypeAdd = "Add";
      public static readonly string CommandTypeEdit = "Edit";
      public static readonly string CommandTypeCreate = "Create";
      public static readonly string CommandTypeUpdate = "Update";
      public static readonly string CommandTypeDelete = "Delete";
      public static readonly string CommandTypeSwap = "Swap";
      public static readonly string CommandTypeReset = "Reset";
      public static readonly string CommandTypeConfigure = "Configure";
      public static readonly string CommandTypeRename = "Rename";
      public static readonly string CommandTypeMove = "Move";
      public static readonly string CommandTypeCopy = "Copy";

      public static readonly string MetaDataAddedToDataRepositories = "Meta Data added to multiple repositories";
      public static readonly string MetaDataRemovedFromDataRepositories = "Meta Data removed from multiple repositories";
      public static readonly string MetaDataModifiedInDataRepositories = "Meta Data modified in multiple repositories";
      public static readonly string MolecularWeightModifiedInDataRepositories = "Molecular weight modified in multiple repositories";
      public static readonly string ObservedDataDeletedFromProject = "Observed data deleted from project";

      public static string CreateProjectDescription(string version) => $"Project started with version {version}";

      public static string SetMetaDataAddedCommandDescription(string name, string value) => $"New Meta Data added where {name} = {value}";

      public static string SetMetaDataRemovedCommandDescription(string name, string value) => $"Meta Data removed where {name} = {value}";

      public static string SetMetaDataChangedCommandDescription(string oldName, string oldValue, string newName, string newValue)
      {
         return $"Meta Data changed from {oldName}={oldValue} to {newName}={newValue}";
      }

      public static string SetObservedDataParameterCommandDescription(string oldValue, string newValue, string observedDataName, string parameterName)
      {
         return $"{parameterName} set from {oldValue} to {newValue} in {observedDataName}";
      }

      public static string AddObservedDataToProjectDescription(string observedDataName, string projectName)
      {
         return AddEntityToContainer(ObjectTypes.ObservedData, observedDataName, ObjectTypes.Project, projectName);
      }

      public static string RemoveObservedDataFromProjectDescription(string observedDataName, string projectName)
      {
         return RemoveEntityFromContainer(ObjectTypes.ObservedData, observedDataName, ObjectTypes.Project, projectName);
      }

      public static string AddEntityToContainer(string entityType, string entityName, string containerType, string containerName)
      {
         var lowerEntityType = string.IsNullOrEmpty(entityType) ? entityType : entityType.ToLower();
         var lowerContainerType = string.IsNullOrEmpty(containerType) ? containerType : containerType.ToLower();

         return $"Add {lowerEntityType} '{entityName}' to {lowerContainerType} '{containerName}'";
      }

      public static string RemoveEntityFromContainer(string entityType, string entityName, string containerType, string containerName)
      {
         var lowerEntityType = string.IsNullOrEmpty(entityType) ? entityType : entityType.ToLower();
         var lowerContainerType = string.IsNullOrEmpty(containerType) ? containerType : containerType.ToLower();
         return $"Delete {lowerEntityType} '{entityName}' from {lowerContainerType} '{containerName}'";
      }

      private static StringBuilder nameValueUnitListFormatter(string baseGridNameValueUnit, IEnumerable<string> columnNameValueUnits)
      {
         var sb = new StringBuilder();
         columnNameValueUnits.Each(x => sb.Append(x));
         sb.Append($" at {baseGridNameValueUnit}");
         return sb;
      }

      public static string ChangeColorOfCurveTemplate(string curveTemplateName, string newName, string oldName)
      {
         return $"Changed color of curve template {curveTemplateName} from {newName} to {oldName}";
      }

      public static string ChangeLineStyleOfCurveTemplate(string curveTemplateName, string newLineStyle, string oldLineStyle)
      {
         return $"Changed line style of curve template {curveTemplateName} from {newLineStyle} to {oldLineStyle}";
      }

      public static string ChangeLineThicknessOfCurveTemplate(string curveTemplateName, int newLineThickness, int oldLineThickness)
      {
         return $"Changed line thickness of curve template {curveTemplateName} from {newLineThickness} to {oldLineThickness}";
      }

      public static string ChangeSymbolOfCurveTemplate(string curveTemplateName, string newSymbol, string oldSymbol)
      {
         return $"Changed symbol of curve template {curveTemplateName} from {newSymbol} to {oldSymbol}";
      }

      public static string RenameObservedData(string oldName, string newName)
      {
         return $"Renamed observed data from '{oldName}' to '{newName}'";
      }

      public static string RenameSimulation(string oldName, string newName) =>
         $"Renamed simulation from '{oldName}' to '{newName}'";

      public static string UpdateValueOriginFrom(string oldValueOrigin, string newValueOrigin, string withValueOriginType, string withValueOriginDisplay, string containerType, string containerDisplay)
      {
         string withObjectTypeInfo = $"for {withValueOriginType} '{withValueOriginDisplay}' in {containerType} '{containerDisplay}'";

         if (string.IsNullOrEmpty(oldValueOrigin))
            return $"Value origin set to '{newValueOrigin}' {withObjectTypeInfo}";

         if (string.IsNullOrEmpty(newValueOrigin))
            return $"Value origin set to '{oldValueOrigin}' {withObjectTypeInfo}";

         return $"Update value origin from '{oldValueOrigin}' to '{newValueOrigin}' {withObjectTypeInfo}";
      }
   }

   public static class ObjectTypes
   {
      public static readonly string CalculationMethod = "Calculation Method";
      public static readonly string MoleculeBuildingBlock = "Molecule Building Block";
      public static readonly string ReactionBuildingBlock = "Reaction Building Block";
      public static readonly string SpatialStructure = "Spatial Structure";
      public static readonly string PassiveTransportBuildingBlock = "Passive Transport Building Block";
      public static readonly string ObserverBuildingBlock = "Observer Building Block";
      public static readonly string EventGroupBuildingBlock = "Event Group Building Block";
      public static readonly string ParameterValuesBuildingBlock = "Parameter Values Building Block";
      public static readonly string InitialConditionsBuildingBlock = "Initial Conditions Building Block";
      public static readonly string Molecule = "Molecule";
      public static readonly string TransporterMoleculeContainer = "Transporter Molecule";
      public static readonly string ActiveTransport = "Active Transport";
      public static readonly string Parameter = "Parameter";
      public static readonly string DistributedParameter = "Distributed Parameter";
      public static readonly string Container = "Container";
      public static readonly string Reaction = "Reaction";
      public static readonly string AmountObserverBuilder = "Molecule Observer";
      public static readonly string ObserverBuilder = "Observer";
      public static readonly string ContainerObserverBuilder = "Container Observer";
      public static readonly string EventGroupBuilder = "Event Group";
      public static readonly string EventBuilder = "Event";
      public static readonly string Application = "Application";
      public static readonly string EventAssignmentBuilder = "Event Assignment";
      public static readonly string ApplicationMoleculeBuilder = "Application Molecule";
      public static readonly string Formula = "Formula";
      public static readonly string Simulation = "Simulation";
      public static readonly string ExplicitFormula = "Formula";
      public static readonly string Neighborhood = "Neighborhood";
      public static readonly string PassiveTransportBuilder = "Passive Transport";
      public static readonly string ActiveTransportBuilder = "Active Transport";
      public static readonly string TransportBuilder = "Transport";
      public static readonly string Transport = "Transport";
      public static readonly string FormulaUsablePath = "Reference";
      public static readonly string ApplicationTransport = "Transport";
      public static readonly string Project = "Project";
      public static readonly string Default = "Default";
      public static readonly string ReactionPartnerBuilder = "Reaction Partner";
      public static readonly string ConstantFormula = "Constant Formula";
      public static readonly string ParameterValue = "Parameter Value";
      public static readonly string InitialCondition = "Initial Condition";
      public static readonly string ValuePoint = "Value Point";
      public static readonly string TableFormula = "Table";
      public static readonly string BlackBoxFormula = "Calculation Method";
      public static readonly string SumFormula = "Sum Formula";
      public static readonly string Quantities = "Quantities";
      public static readonly string Model = "Model";
      public static readonly string MoleculeList = "Molecule List";
      public static readonly string SolverProperty = "Solver Property";
      public static readonly string SimulationSettings = "Simulation Settings";
      public static readonly string OutputSelections = "Output Selections";
      public static readonly string ChartTemplate = "Chart Template";
      public static readonly string Unit = "Unit";
      public static readonly string Reference = "Reference";
      public static readonly string TagCondition = "Tag conditions";
      public static readonly string History = "History";
      public static readonly string Educt = "Educt";
      public static readonly string Product = "Product";
      public static readonly string Modifier = "Modifier";
      public static readonly string InteractionContainer = "Interaction Container";
      public static readonly string BuildingBlock = "Building Block";
      public static readonly string ObservedData = "Observed Data";
      public static readonly string ImporterConfiguration = "Importer Configuration";
      public static readonly string CurveTemplate = "Curve Template";
      public static readonly string DataTable = "DataTable";
      public static readonly string JournalPage = "Page";
      public static readonly string RelatedItem = "Related Item";
      public static readonly string Journal = "Journal";
      public static readonly string DiagramModel = "Diagram";
      public static readonly string IdentificationParameter = "Identification Parameter";
      public static readonly string ParameterIdentification = "Parameter Identification";
      public static readonly string Formulation = "Formulation";
      public static readonly string SensitivityParameter = "Sensitivity Parameter";
      public static readonly string SensitivityAnalysis = "Sensitivity Analysis";
      public static readonly string ExpressionProfileBuildingBlock = "Expression Profile Building Block";
   }

   public static class ToolTips
   {
      public static readonly string ToolTipForAxis = "Double click to edit axis";
      public static readonly string UseSelectedCurvesToolTip = "Adds or removes all the selected curves at once.";
      public static readonly string EnableOrDisableAutomaticUpdateOfTheChartForEachEdit = "Enable or Disable automatic update of the chart for each edit.";
      public static readonly string LinkSimulationObservedToolTip = "Links the simulation outputs to their mapped observed data, so that when a simulation output is (de)selected the corresponding observed data gets (de)selected as well.";
      public static readonly string AddUnitMap = "Add new default unit for a specific dimension";
      public static readonly string LoadUnits = "Load default units from file";
      public static readonly string SaveUnits = "Save default units to file";
      public static readonly string ManageUserDisplayUnits = "Manage the display units defined in the user settings";
      public static readonly string ManageProjectDisplayUnits = "Manage the display units defined in the current project";
      public static readonly string UpdateAllToDisplayUnits = "Update all entities to use the predefined display units";
      public static readonly string FavoritesToolTip = "Add as favorite";
      public static readonly string ExportSelectedJournalEntriesToWord = $"Export selected journal pages to {Captions.Word}";
      public static readonly string DefaultCurveColor = string.Format("Sets the color for each curve added to this axis.{0}Set white to not use a default color and instead use automatically selected colors.{0}Default color has no effect for X axis", Environment.NewLine);
      public static readonly string DefaultCurveColorTitle = "Default Color for Curve";
      public static readonly string DefaultLineStyle = string.Format("Sets the line style for each curve added to this axis.{0}Setting 'None' will use the default style for the curve.{0}Default line style has no effect for X axis.", Environment.NewLine);
      public static readonly string DefaultLineStyleTitle = "Default Line Style for Curve";
      public static readonly string DoNotShowVersionUpdate = "Ignore this update";
      public static readonly string ClearHistory = "Clear the project history. This action is irreversible";
      public static readonly string UseDerivedValues = "If checked, the first derivative of the entered table values is used";
      public static readonly string RestartSolver = "Select to mark solver restart at t=X. Increases numerical accuracy  but may reduce solver speed";

      public static string ToolTipForLLOQ(string seriesName, string lloq)
      {
         return string.Format($"This line represents the LLOQ '{lloq}' for the series named '{seriesName}'");
      }

      public static string ToolTipForSeriesPoint(string legendText, string xAxisTitle, string yAxisTitle, string numericalArgument, string firstValue, string secondValue, bool editable, string description)
      {
         var sb = startToolTip(legendText);
         sb.AppendLine();

         sb.AppendFormat("{0}: {1}", xAxisTitle, numericalArgument);
         sb.AppendLine();

         if (!string.IsNullOrEmpty(secondValue)) //An area plot has 2 y values and displays the range
            sb.AppendFormat("{0}: [{1},{2}]", yAxisTitle, firstValue, secondValue);
         else
            sb.AppendFormat("{0}: {1}", yAxisTitle, firstValue);

         if (!string.IsNullOrEmpty(description))
         {
            sb.AppendLine();
            sb.AppendLine();
            sb.Append(description);
         }

         if (editable)
            finishToolTip(sb);

         return sb.ToString();
      }

      private static void finishToolTip(StringBuilder sb)
      {
         sb.AppendLine();
         sb.AppendLine();
         sb.AppendFormat("Double click to edit curve settings");
      }

      private static StringBuilder startToolTip(string legendText)
      {
         return new StringBuilder(legendText);
      }

      public static string ToolTipForLegendEntry(string legendText, bool editable)
      {
         var sb = startToolTip(legendText);
         if (editable)
            finishToolTip(sb);
         return sb.ToString();
      }

      public static class ManageTemplates
      {
         public static readonly string Clone = "Clone template";
         public static readonly string Delete = "Delete template";
         public static readonly string SaveToFile = "Save template to file";
      }

      public static string LLOQTooltip(string columnName, string value, string displayUnit, double? lloq)
      {
         return $"The column '{columnName}' has a value '{value} {displayUnit}' that is below the LLOQ '{lloq} {displayUnit}'";
      }

      public static class Commands
      {
         public static readonly string Undo = "Undo last command";
         public static readonly string AddLabel = "Add a special label to the history";
         public static readonly string EditComment = "Enter a comment for the selected line";
         public static readonly string Rollback = "Click on the line to rollback to, then click the rollback arrow";
      }

      public static class BuildingBlockSpatialStructure
      {
         public static readonly string HowToCreateNeighborhood = "To create a neighborhood, draw a connection between containers.";
      }

      public static class BuildingBlockReaction
      {
         public static readonly string HowToCreateReactionLink = "To add a molecule to educts, products or modifiers, draw a connection between reaction and molecule.";
      }

      public static class Journal
      {
         public static readonly string NavigateToNextPage = "Navigate to next page";
         public static readonly string NavigateToPreviousPage = "Navigate to previous page";
         public static readonly string AddRelatedItemFromFile = "Add new related item from a selected file";
         public static readonly string ImportAllRelatedItem = "Import all items into project";
      }
   }

   public static class ObservedData
   {
      public static readonly string ObservedDataOrganDescription = "Organ where the data was measured";
      public static readonly string ObservedDataCompartmentDescription = "Compartment where the data was measured";
      public static readonly string ObservedDataMoleculeDescription = "Molecule for which the data was measured";
      public static readonly string MoleculeNameDescription = "Name of the molecule for which the data were measured";
   }

   public static class Colors
   {
      /// <summary>
      ///    Color used for item missing or added in the comparison view
      /// </summary>
      public static Color ADDED_OR_MISSING = Color.LightYellow;

      public static Color Best = Color.DarkGreen;
      public static Color Current = Color.Blue;
      public static Color ErrorColor = Color.Blue;

      public static Color Blue1 = Color.FromArgb(42, 132, 190);
      public static Color Blue2 = Color.FromArgb(67, 160, 220);

      /// <summary>
      ///    Color used for a plot back color (everything but diagram)
      /// </summary>
      public static Color ChartBack = Color.White;

      /// <summary>
      ///    Color used for a diagram back color
      /// </summary>
      public static Color ChartDiagramBack = Color.White;

      public static Color BelowLLOQ => Color.LightSkyBlue;
      public static Color DefaultRowColor => Color.White;

      public static Color NegativeCorrelation = Color.FromArgb(96, 187, 70);
      public static Color PositiveCorrelation = Color.FromArgb(0, 174, 239);
   }
}