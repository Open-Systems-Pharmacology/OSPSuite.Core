using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Serialization;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain
{
   public static class Constants
   {
      public const int PKML_VERSION = PKMLVersion.CURRENT;
      public const string CVODES = "CVODES";
      public const int SIM_MODEL_XML_VERSION = 4;
      public const int MIN_NUMBER_OF_POINTS_PER_INTERVAL = 2;
      public const int MAX_NUMBER_OF_CHAR_IN_TABLE_NAME = 29;
      public const string WILD_CARD = "*";
      public const string WILD_CARD_RECURSIVE = "**";
      public static readonly IReadOnlyList<string> ILLEGAL_CHARACTERS = new List<string> {ObjectPath.PATH_DELIMITER, ":", "*", "?", "<", ">", "|", "{", "}", "\""}.Distinct().ToList();

      public const string DRUG_MASS = "DrugMass";
      public const string MOLECULE_PROPERTIES = "MoleculeProperties";
      public const string APPLICATION_TRANSPORT_NAME = "ApplicationTransport";
      public const string START_APPLICATION_EVENT = "StartApplication";
      public const string NEIGHBORHOODS = "Neighborhoods";
      public const string EVENTS = "Events";
      public const string ORGANISM = "Organism";
      public const string ROOT = "ROOT";
      public const string NAME_PROPERTY = "Name";
      public const int NOT_FOUND_INDEX = -1;

      public const string ACTIVE = "Active";
      public const string INFLUX = "Influx";
      public const string NOT_INFLUX = "NotInflux";
      public const string PASSIVE = "Passive";
      public const string TIME = "Time";
      public const string MEASUREMENT = "Measurement";
      public const string ERROR = "Error";
      public const string ROOT_CONTAINER_TAG = "RootContainer";
      public const string ALL_TAG = "All";
      public const string CONCENTRATION_FORMULA = "ConcFormula";
      public const string MOLECULE_CONCENTRATION_IN_AMOUNT_FORMULA = "Molecule concentration in amount";
      public const string MOLECULE_AMOUNT_FORMULA = "Molecule amount";

      public const string OUTPUT_SCHEMA = "Output schema";
      public const string OUTPUT_INTERVAL = "Output interval";
      public const string ONTOGENY_FACTOR = "Ontogeny factor";
      public const string HALF_LIFE = "t1/2";
      public const string DEGRADATION_COEFF = "Degradation coefficient";
      public const string DATA_REPOSITORY_COLUMN_ID = "DataRepositoryColumnId";
      public const int DEFAULT_TEMPLATE_WARNING_THRESHOLD = 10;
      public const double DEFAULT_SCALE_DIVISOR = 1;
      public const double DEFAULT_SCALE_DIVISOR_MIN_VALUE = 1e-15;
      public const string DEFAULT_CHART_LAYOUT = "Standard View";
      public const string DEFAULT_SIMULATION_RESULTS_NAME = "Simulation Results";

      public const string VOLUME_ALIAS = "V";
      public const string START_VALUE_ALIAS = "StartValue";
      public const string TOO_OLD_PKML = "PKML file is too old and cannot be converted. Please install the version 8.0 of the OSPSuite to convert the file to a version that can be supported";

      public const string MOL_WEIGHT_EXTENDED_PROPERTY = "MolWeight";

      //tolerated precision to relatively compare to double values 
      public const double DOUBLE_RELATIVE_EPSILON = 1e-5;
      public const double DOUBLE_PERCENTILE_RELATIVE_TOLERANCE = 1e-2;

      //For fractions only, the minimum threshold used for comparison.  (<= 0.01%)
      public const float MIN_FRACTION_RELATIVE_COMPARISON_THRESHOLD = 1e-4F;

      //Max suggested output points. User can decide to still go ahead with the simulation
      public const float MAX_NUMBER_OF_SUGGESTED_OUTPUT_POINTS = 50000;

      public const float FLOAT_RELATIVE_EPSILON = 0.00001f;
      public const double CONFIDENCE_INTERVAL_ALPHA = 0.05;

      public const string PROJECT_UNDEFINED = "Undefined";
      public const string DISPLAY_PATH_SEPARATOR = "-";
      public const string NAN = "<NaN>";
      public const string UNKNOWN = "Unknown";
      public const string CHILD = "Child";

      public const string PRODUCT_SITE = "www.open-systems-pharmacology.org";
      public const string PRODUCT_SITE_DOWNLOAD = "http://setup.open-systems-pharmacology.org";
      public const string HELP_NAMESPACE = "http://docs.open-systems-pharmacology.org";
      public const string FORUM_SITE = "http://forum.open-systems-pharmacology.org";
      public const string SUITE_NAME = "Open Systems Pharmacology Suite";

      public const float DEFAULT_WEIGHT = 1;
      public const double DEFAULT_USE_AS_FACTOR = 1;
      public const double DEFAULT_PARAMETER_RANGE_FACTOR = 10;
      public const double DEFAULT_PERCENTILE = 0.5;
      public const int DEFAULT_NUMBER_OF_RUNS_FOR_MULTIPLE_MODE = 10;
      public const string X = "X";
      public const string Y = "Y";
      public const double TOO_CLOSE_TO_BOUNDARY_FACTOR = 0.01;
      public const int NUMBER_OF_RUNS_WITH_VISIBLE_LEGEND = 10;
      public const int MAX_NUMBER_OF_CURVES_TO_SHOW_AT_ONCE = 10;
      public const float LOG_SAFE_EPSILON = 1e-20F;
      public const byte RANGE_AREA_OPACITY = 55;
      public const byte RANGE_AREA_TRANSPARENCY = 255 - RANGE_AREA_OPACITY;
      public const int FEEDBACK_REFRESH_TIME = 1000; //refresh time in ms
      public const int DEFAULT_SENSITIVITY_NUMBER_OF_STEPS = 2;
      public const double DEFAULT_SENSITIVITY_VARIATION_RANGE = 0.1;

      //sensitivity values below this value will be set to zero
      public const double SENSITIVITY_THRESHOLD = 1.0e-4;
      public const string STD_DEV_GEOMETRIC = "Geometric Standard Deviation";
      public const string STD_DEV_ARITHMETIC = "Arithmetic Standard Deviation";
      public const string AUXILIARY_TYPE = "AuxiliaryType";
      public const string FILE = "Source";
      public const string SHEET = "Sheet";
      public const string DEFAULT_WATERMARK_TEXT = "DRAFT";

      public const int MB_TO_BYTES = 1024 * 1024; //1 MB = 1024 * 1024 bytes
      public const int RELATIVE_ITEM_FILE_SIZE_WARNING_THRESHOLD_IN_BYTES = 5 * MB_TO_BYTES;
      public const int RELATIVE_ITEM_MAX_FILE_SIZE_IN_BYTES = 50 * MB_TO_BYTES;
      public const string RELATIVE_ITEM_FILE_ITEM_TYPE = "File";

      public static class Files
      {
         public const string LICENSE_AGREEMENT_FILE_NAME = "Open Systems Pharmacology Suite License.pdf";
         public const string PK_PARAMETERS_FILE_NAME = "OSPSuite.PKParameters.xml";
         public const string COMPANY_FOLDER_NAME = "Open Systems Pharmacology";
         public const string DIMENSIONS_FILE_NAME = "OSPSuite.Dimensions.xml";
         public const string CHART_LAYOUT_FOLDER_NAME = "ChartLayouts";
         public const string TEX_TEMPLATE_FOLDER_NAME = "TeXTemplates";
      }

      public static class Organ
      {
         public const string LUMEN = "Lumen";
         public const string MUCOSA = "Mucosa";
      }

      public static class Compartment
      {
         public const string STOMACH = "Stomach";
         public const string DUODENUM = "Duodenum";
         public const string UPPER_JEJUNUM = "UpperJejunum";
         public const string LOWER_JEJUNUM = "LowerJejunum";
         public const string UPPER_ILEUM = "UpperIleum";
         public const string LOWER_ILEUM = "LowerIleum";
         public const string CAECUM = "Caecum";
         public const string COLON_ASCENDENS = "ColonAscendens";
         public const string COLON_TRANSVERSUM = "ColonTransversum";
         public const string COLON_DESCENDENS = "ColonDescendens";
         public const string COLON_SIGMOID = "ColonSigmoid";
         public const string RECTUM = "Rectum";

         public static readonly IReadOnlyList<string> LumenSegmentsDuodenumToLowerIleum = new List<string>
         {
            DUODENUM,
            UPPER_JEJUNUM,
            LOWER_JEJUNUM,
            UPPER_ILEUM,
            LOWER_ILEUM
         };

         public static readonly IReadOnlyList<string> LumenSegmentsDuodenumToCaecum = new List<string>(LumenSegmentsDuodenumToLowerIleum)
         {
            CAECUM
         };

         public static readonly IReadOnlyList<string> LumenSegmentsDuodenumToRectum = new List<string>(LumenSegmentsDuodenumToCaecum)
         {
            COLON_ASCENDENS,
            COLON_TRANSVERSUM,
            COLON_DESCENDENS,
            COLON_SIGMOID,
            RECTUM
         };

         public static readonly IReadOnlyList<string> AllLumenSegments =
            new List<string>(new[] {STOMACH}.Concat(LumenSegmentsDuodenumToRectum));
      }

      public static class Processes
      {
         public static readonly string RenalClearances = "Renal Clearances";
         public static readonly string GlomerularFiltration = "Glomerular Filtration";
      }

      public static class Parameters
      {
         public const string REL_EXP = "Relative expression";
         public const string REL_EXP_BLOOD_CELLS = "Relative expression in blood cells";
         public const string REL_EXP_PLASMA = "Relative expression in plasma";
         public const string REL_EXP_VASCULAR_ENDOTHELIUM = "Relative expression in vascular endothelium";

         public const string RESOLUTION = "Resolution";
         public const string START_VALUE = "Start value";
         public const string START_TIME = "Start time";
         public const string END_TIME = "End time";
         public const string MOL_WEIGHT = "Molecular weight";
         public const string VOLUME = "Volume";
         public const string PROCESS_RATE = "ProcessRate";
         public const string CONCENTRATION = "Concentration";
         public const string ABS_TOL = "Absolute tolerance";
         public const string REL_TOL = "Relative tolerance";
         public const string USE_JACOBIAN = "Use Jacobian";
         public const string H0 = "H0";
         public const string H_MIN = "HMin";
         public const string H_MAX = "HMax";
         public const string MX_STEP = "MxStep";
         public const string MIN_VALUE = "MinValue";
         public const string MAX_VALUE = "MaxValue";
         public const string NUMBER_OF_STEPS = "Number of steps";
         public const string VARIATION_RANGE = "Variation range";
         public const string INFUSION_TIME = "Infusion time";
         public const string WEIGHT = "Weight";
         public const string DRUG_MASS = "DrugMass";

         //todo delete when flag is categorial is defined in core for parameter
         public const string HAS_HALOGENS = "Has halogens";
         public const string EHC_ENABLED = "Gallbladder emptying enabled";
         public const string USE_PENALTY_FACTOR = "Use pH- and pKa-dependent penalty factor for charged molecule fraction";
         public const string IS_SMALL_MOLECULE = "Is small molecule";
         public const string IS_LIVER_ZONATED = "Is liver zonated";
         public const string USE_AS_SUSPENSION = "Use as suspension";
         public const string ENABLE_SUPERSATURATION = "Enable supersaturation";
         public const string URINE_EMPTYING_ENABLE = "Urine emptying enabled";
         public const string PARTICLE_SIZE_DISTRIBUTION = "Particle size distribution";
         public const string CHILD_PUGH_SCORE = "Child-Pugh score";
         public const string NUMBER_OF_BINS = "Number of bins";
         public const string TOTAL_DRUG_MASS = "Total drug mass";
         public const string ParameterCompoundTypeBase = "Compound type ";

         public static string ParameterCompoundType(int index) => $"{ParameterCompoundTypeBase}{index}";

         public static readonly string COMPOUND_TYPE1 = ParameterCompoundType(0);
         public static readonly string COMPOUND_TYPE2 = ParameterCompoundType(1);
         public static readonly string COMPOUND_TYPE3 = ParameterCompoundType(2);
         public const string PARTICLE_DISPERSE_SYSTEM = "Type of particle size distribution";
         public const string PRECIPITATED_DRUG_SOLUBLE = "Treat precipitated drug as";
         public const string PARA_ABSORPTION_SINK = "Paracellular absorption sink condition";
         public const string TRANS_ABSORPTION_SINK = "Transcellular absorption sink condition";
         public const string GESTATIONAL_AGE = "Gestational age";
         public const string PLASMA_PROTEIN_BINDING_PARTNER = "Plasma protein binding partner";
         public const string BR = "Br";
         public const string CL = "Cl";
         public const string F = "F";
         public const string I = "I";
         public static readonly IReadOnlyCollection<string> Halogens = new List<string> {CL, BR, F, I};

         public static readonly IReadOnlyCollection<string> AllBooleanParameters = new List<string>
         {
            USE_JACOBIAN,
            HAS_HALOGENS,
            EHC_ENABLED,
            USE_PENALTY_FACTOR,
            IS_SMALL_MOLECULE,
            IS_LIVER_ZONATED,
            USE_AS_SUSPENSION,
            ENABLE_SUPERSATURATION,
            URINE_EMPTYING_ENABLE
         };

         public static readonly IReadOnlyCollection<string> AllCategorialParameters = new List<string>(AllBooleanParameters)
         {
            PARTICLE_SIZE_DISTRIBUTION,
            NUMBER_OF_BINS,
            COMPOUND_TYPE1,
            COMPOUND_TYPE2,
            COMPOUND_TYPE3,
            PARTICLE_DISPERSE_SYSTEM,
            PRECIPITATED_DRUG_SOLUBLE,
            TRANS_ABSORPTION_SINK,
            PARA_ABSORPTION_SINK,
            GESTATIONAL_AGE,
            PLASMA_PROTEIN_BINDING_PARTNER,
            CHILD_PUGH_SCORE
         };

         public static readonly IReadOnlyCollection<string> AllWithListOfValues = new List<string>(Halogens.Union(AllCategorialParameters));
      }
      //end of  delete

      public static class RegistryPaths
      {
         public const string PKSIM_REG_PATH = @"Open Systems Pharmacology\PK-Sim\";
         public const string MOBI_REG_PATH = @"Open Systems Pharmacology\MoBi\";
         public const string INSTALL_DIR = "InstallDir";
         public const string INSTALL_PATH = "InstallPath";
      }

      public static class Groups
      {
         public const string UNDEFINED = "UNDEFINED";
         public const string SIMULATION_SETTINGS = "SIMULATION_SETTINGS";
         public const string ORGAN_VOLUMES = "ORGAN_VOLUMES";
         public const string MOBI = "MOBI";
         public const string SOLVER_SETTINGS = "SOLVER_SETTINGS";
      }

      public static class Filter
      {
         public const string PNG_EXTENSION = ".png";
         public const string MATLAB_EXTENSION = ".m";
         public const string MAT_EXTENSION = ".mat";
         public const string FIG_EXTENSION = ".fig";
         public const string R_EXTENSION = ".r";
         public const string RD_EXTENSION = ".rd";
         public const string XML_EXTENSION = ".xml";
         public const string JOURNAL_EXTENSION = ".sbj";
         public const string CSV_EXTENSION = ".csv";
         public const string NONMEM_EXTENSION = ".nmdat";
         public const string PDF_EXTENSION = ".pdf";
         public const string PKML_EXTENSION = ".pkml";
         public const string XLS_EXTENSION = ".xls";
         public const string XLSX_EXTENSION = ".xlsx";
         public const string TEXT_EXTENSION = ".txt";
         public const string JSON_EXTENSION = ".json";
         public const string DOCX_EXTENSION = ".docx";
         public const string DOC_EXTENSION = ".doc";
         public const string MARKDOWN_EXTENSION = ".md";
         public const string ANY_EXTENSION = ".*";
         
         public static readonly string DIAGRAM_IMAGE_FILTER = FileFilter("Diagram Image", PNG_EXTENSION);
         public static readonly string UNITS_FILE_FILTER = XmlFilter("Units");
         public static readonly string FAVORITES_FILE_FILTER = XmlFilter("Favorites");
         public static readonly string CHART_TEMPLATE_FILTER = XmlFilter("Chart Template");
         public static readonly string CHART_LAYOUT_FILTER = XmlFilter("Chart Layout");
         public static readonly string PDF_FILE_FILTER = FileFilter("Report", PDF_EXTENSION);
         public static readonly string XML_FILE_FILTER = XmlFilter("Xml");
         public static readonly string EXCEL_SAVE_FILE_FILTER = string.Format("Excel file (*{0})|*{0}|Excel file  (*{1})|*{1}", XLSX_EXTENSION, XLS_EXTENSION);
         public static readonly string EXCEL_OPEN_FILE_FILTER = string.Format("Excel file (*{0};*{1})|*{0};*{1}", XLS_EXTENSION, XLSX_EXTENSION);
         public static readonly string JOURNAL_FILE_FILTER = FileFilter("Journal File", JOURNAL_EXTENSION);
         public static readonly string WORD_SAVE_FILE_FILTER = FileFilter("Word", DOCX_EXTENSION);
         public static readonly string PKML_FILE_FILTER = FileFilter("Shared Modeling", PKML_EXTENSION);
         public static readonly string HISTORY_FILE_FILTER = FileFilter("History Export", XLSX_EXTENSION);
         public static readonly string TEXT_FILE_FILTER = FileFilter("Text", TEXT_EXTENSION);
         public static readonly string UNIT_FILE_FILTER = XmlFilter("Unit");
         public static readonly string MATLAB_FILTER = FileFilter("Matlab®", MATLAB_EXTENSION);
         public static readonly string CSV_FILE_FILTER = FileFilter("CSV", CSV_EXTENSION);
         public static readonly string JSON_FILE_FILTER = FileFilter("Json", JSON_EXTENSION);
         public static readonly string MARKDOWN_FILE_FILTER = FileFilter("Markdown", MARKDOWN_EXTENSION);
         public static readonly string JSON_FILTER = filter(JSON_EXTENSION);
         public static readonly string XML_FILTER = filter(XML_EXTENSION);

         public static string XmlFilter(string caption) => FileFilter(caption, XML_EXTENSION);

         public static string FileFilter(string caption, string extension)
         {
            return $"{caption} File ({filter(extension)})|{filter(extension)}";
         }

         private static string filter(string extension) => $"*{extension}";
      }

      public static class DirectoryKey
      {
         public const string PROJECT = "Project";
         public const string MODEL_PART = "ModelPart";
         public const string OBSERVED_DATA = "ObservedData";
         public const string SIM_MODEL_XML = "SimModelXml";
         public const string XLS_IMPORT = "xlsImport";
         public const string POPULATION = "Population";
         public const string REPORT = "Report";
         public const string TEMPLATE = "Template";
      }

      public static class Dimension
      {
         public const string VOLUME = "Volume";
         public const string MOLAR_CONCENTRATION = "Concentration (molar)";
         public const string LENGTH = "Length";
         public const string MASS_CONCENTRATION = "Concentration (mass)";
         public const string DIMENSIONLESS = "Dimensionless";
         public const string MOLAR_AMOUNT = "Amount";
         public const string MASS_AMOUNT = "Mass";
         public const string TIME = "Time";
         public const string RHS_DIMENSION_SUFFIX = " per time";
         public const string RESOLUTION = "Resolution";
         public const string AMOUNT_PER_TIME = "Amount per time";
         public const string MOLAR_CONCENTRATION_PER_TIME = "Concentration (molar) per time";
         public const string MOLECULAR_WEIGHT = "Molecular weight";
         public const string LOG_UNITS = "Log Units";
         public const string FRACTION = "Fraction";
         public const string MOLAR_AUC = "AUC (molar)";
         public const string MOLAR_AUCM = "AUCM (molar)";
         public const string MASS_AUC = "AUC (mass)";
         public const string VOLUME_PER_BODY_WEIGHT = "Volume per body weight";

         public static readonly IDimension NO_DIMENSION = new UnitSystem.Dimension(new BaseDimensionRepresentation(), DIMENSIONLESS, string.Empty);

         public static class Units
         {
            public const string Seconds = "s";
            public const string Minutes = "min";
            public const string Hours = "h";
            public const string Days = "day(s)";
            public const string Weeks = "week(s)";
            public const string Months = "month(s)";
            public const string Years = "year(s)";
         }
      }

      public static class SimulationResults
      {
         public const string INDIVIDUAL_ID = "IndividualId";
         public const string TIME = "Time";
         public const string QUANTITY_PATH = "QuantityPath";
         public const string PARAMETER = "Parameter";
         public const string VALUE = "Value";
         public const string UNIT = "Unit";
      }

      public static class SensitivityAnalysisResults
      {
         public const string QUANTITY_PATH = "QuantityPath";
         public const string PARAMETER = "Parameter";
         public const string VALUE = "Value";
         public const string PK_PARAMETER = "PKParameter";
         public const string PARAMETER_PATH = "ParameterPath";
      }

      public static class Distribution
      {
         public const string DEVIATION = "Deviation";
         public const string GEOMETRIC_DEVIATION = "GeometricDeviation";
         public const string MEAN = "Mean";
         public const string PERCENTILE = "Percentile";
         public const string MINIMUM = "Minimum";
         public const string MAXIMUM = "Maximum";
      }

      public static class PKParameters
      {
         internal const string FirstIntervalSuffix = "_tD1_tD2";
         internal const string LastIntervalSuffix = "_tDLast_tEnd";
         internal const string NormSuffix = "_norm";
         public const string C_max = "C_max";
         public const string C_tEnd = "C_tEnd";
         public const string Tmax = "t_max";
         public const string AUC_inf = "AUC_inf";
         public const string AUC_tEnd = "AUC_tEnd";
         public const string MRT = "MRT";
         public const string Thalf = "Thalf";
         public const string CL = "CL";
         public const string Vss = "Vss";
         public const string Vd = "Vd";
         internal const string Ctrough = "C_trough";
         public const string FractionAucEndToInf = "FractionAucLastToInf";

         internal static string Create(string param, string suffix = null)
         {
            return $"{param}{suffix ?? string.Empty}";
         }

         public static readonly string AUC_tEnd_norm = NormalizedName(AUC_tEnd);
         public static readonly string C_max_norm = NormalizedName(C_max);
         public static readonly string AUC_inf_norm = NormalizedName(AUC_inf);

         public static readonly string C_max_tD1_tD2 = Create(C_max, FirstIntervalSuffix);
         public static readonly string C_max_tD1_tD2_norm = NormalizedName(C_max_tD1_tD2);

         public static readonly string C_max_tDLast_tDEnd = Create(C_max, LastIntervalSuffix);
         public static readonly string C_max_tDLast_tDEnd_norm = NormalizedName(C_max_tDLast_tDEnd);

         public static readonly string Tmax_tD1_tD2 = Create(Tmax, FirstIntervalSuffix);
         public static readonly string Tmax_tDLast_tDEnd = Create(Tmax, LastIntervalSuffix);

         public static readonly string AUC_inf_tD1 = Create(AUC_inf, "_tD1");
         public static readonly string AUC_inf_tD1_norm = NormalizedName(AUC_inf_tD1);

         public static readonly string AUC_tD1_tD2 = Create("AUC", FirstIntervalSuffix);
         public static readonly string AUC_tD1_tD2_norm = NormalizedName(AUC_tD1_tD2);

         public const string AUC_tDLast_minus_1_tDLast = "AUC_tDLast_minus_1_tDLast";
         public static readonly string AUC_tDLast_minus_1_tDLast_norm = NormalizedName(AUC_tDLast_minus_1_tDLast);

         public const string AUC_inf_tDLast = "AUC_inf_tDLast";
         public static readonly string AUC_inf_tLast_norm = NormalizedName(AUC_inf_tDLast);

         public static readonly string Ctrough_tD2 = Create(Ctrough, "_tD2");
         public static readonly string Ctrough_tDLast = Create(Ctrough, "_tDLast");

         public static readonly string Thalf_tDLast_tEnd = Create(Thalf, LastIntervalSuffix);

         internal static string NormalizedName(string pkParameter)
         {
            return $"{pkParameter}{NormSuffix}";
         }
      }

      public static class Population
      {
         public const string ALL_GENDERS = "AllGenders";
         public const string TIME_COLUMN = "Time";
         public const string VALUE_COLUMN = "Value";
         public const string PARAMETER_PATH_COLUMN = "ParameterPath";
         public const string INDIVIDUAL_ID_COLUMN = "IndividualId";
         public const string RACE_INDEX = "RaceIndex";
         public const string POPULATION = "Population";
         public const string GENDER = "Gender";
      }

      public static class OptimizationAlgorithm
      {
         public const string NELDER_MEAD_PKSIM = "Nelder Mead (PK-Sim)";
         public const string MPFIT = "Levenberg - Marquardt (MPFit)";
         public const string MONTE_CARLO = "Monte - Carlo";
         public const string DEFAULT = MPFIT;
      }

      public static class CategoryOptimizations
      {
         public const string COMPOUND = "Compound";
         public const string CATEGORY = "Category";
         public const string CATEGORY_DISPLAY = "Category Display";
         public const string CALCULATION_METHOD = "CalculationMethod";
         public const string CALCULATION_METHOD_DISPLAY = "Calculation Method Display";
         public const string VALUE = "Value";
         public const int WARNING_THRESHOLD = 5;
      }

      public static class ObservedData
      {
         public static readonly string ORGAN = "Organ";
         public static readonly string COMPARTMENT = "Compartment";
         public static readonly string MOLECULE = "Molecule";
         public static readonly string MOLECULAR_WEIGHT = "Molecular Weight";
         public static readonly string SPECIES = "Species";
         public static readonly string STUDY_ID = "Study Id";
         public static readonly string SUBJECT_ID = "Subject Id";
         public static readonly string ROUTE = "Route";
         public static readonly string GENDER = "Gender";
         public static readonly string DOSE = "Dose";
         public static readonly string PLASMA_COMPARTMENT = "Plasma";
         public static readonly string PERIPHERAL_VENOUS_BLOOD_ORGAN = "Peripheral Venous Blood";
         public static readonly string VENOUS_BLOOD_ORGAN = "VenousBlood";
      }

      public static class LLOQModes
      {
         public const string ONLY_OBSERVED_DATA = "OnlyObservedData";
         public const string SIMULATION_OUTPUT_AS_OBSERVED_DATA_LLOQ = "SimulationOutputAsObservedDataLLOQ";
      }

      public static class RemoveLLOQMode
      {
         public const string NEVER = "Never";
         public const string NO_TRAILING = "NoTrailing";
         public const string ALWAYS = "Always";
      }

      public static class ContainerName
      {

         
         public static string ExpressionProfileName(string moleculeName, string species, string category)
            => compositeNameFor(char.Parse(ObjectPath.PATH_DELIMITER), moleculeName, species, category);

         public static (string moleculeName, string speciesName, string category) NamesFromExpressionProfileName(string expressionProfileName)
         {
            var names = NamesFromCompositeName(expressionProfileName, char.Parse(ObjectPath.PATH_DELIMITER));
            if (names.Count != 3)
               return (string.Empty, string.Empty, string.Empty);

            return (names[0], names[1], names[2]);
         }
      }

      public const char COMPOSITE_SEPARATOR = '-';

      public static IReadOnlyList<string> NamesFromCompositeName(string compositeName, char separator = COMPOSITE_SEPARATOR)
      {
         return compositeName.Split(separator);
      }

      private static string compositeNameFor(char separator, params string[] names)
      {
         if (names == null || names.Length == 0)
            return string.Empty;

         var nonEmptyNames = names.ToList();
         nonEmptyNames.RemoveAll(string.IsNullOrEmpty);

         return nonEmptyNames.Select(x => x.Trim()).ToString($"{separator}");
      }

      public static string CompositeNameFor(params string[] names) => compositeNameFor(COMPOSITE_SEPARATOR, names);

      public static class Serialization
      {
         public const string INITIAL_CONDITIONS = "InitialConditions";
         public const string MACRO_COMMAND = "MacroCommand";
         public const string SIMPLE_COMMAND = "SimpleCommand";
         public const string LABEL_COMMAND = "LabelCommand";
         public const string INFO_COMMAND = "InfoCommand";
         public const string SIMULATION = "Simulation";
         public const string SIMULATION_LIST = "SimulationList";
         public const string TIME_POINT = "TimePoint";
         public const string TIME_POINTS = "TimePoints";
         public const string FORMULA = "Formula";
         public const string FORMULA_USABLE_PATH = "Path";
         public const string OBJECT_PATHS = "Paths";
         public const string STRING_MAP_LIST = "StringMap";
         public const string STRING_MAP = "Map";
         public const string FORMULAS = "Formulas";
         public const string VALUE = "Value";
         public const string VALUES = "Values";
         public const string QUANTITY_INFO = "McQuantityInfo";
         public const string DATA_INFO = "McDataInfo";
         public const string DATA_COLUMN = "McDataColumn";
         public const string BASE_GRID = "McBaseGrid";
         public const string DATA_REPOSITORY = "McDataRepository";
         public const string COLUMN_CACHE = "ColumnCache";
         public const string BUILDERS = "Builders";
         public const string TOP_CONTAINERS = "TopContainers";
         public const string GROUP_REPOSITORY = "GroupRepository";
         public const string FAVORITE = "Favorite";
         public const string ORIGIN = "Origin";
         public const string DIAGRAM_MODEL = "DiagramModel";
         public const string ALL = "All";
         public const string DESCRIPTOR_CONDITIONS = "DescriptorConditions";
         public const string KEYS = "Keys";
         public const string VALUE_ORIGIN = "ValueOrigin";
         public const string PERCENTILES = "Percentiles";
         public const string SIMULATION_CONFIGURATION = "SimulationConfiguration";

         public static class Attribute
         {
            public const string PATH = "path";
            public const string STRING = "s";
            public const string FACTOR = "factor";
            public const string FORMULA = "formula";
            public const string INTERNAL_FORMULA = "internalFormula";
            public const string ORIGIN = "origin";
            public const string Dimension = "dim";
            public const string VALUE = "value";
            public const string X_DIMENSION = "xDim";
            public const string ALIAS = "as";
            public const string ID = "id";
            public const string X_DISPLAY_UNIT = "xDisplayUnit";
            public const string Y_DISPLAY_UNIT = "yDisplayUnit";
            public const string VERSION = "version";
            public const string COLUMN_ID = "ColumnId";
            public const string DISPLAY_UNIT = "displayUnit";
            public const string BUILDING_BLOCK_VERSION = "bbVersion";
            public const string BASE_UNIT = "baseUnit";
            public const string DEFAULT_UNIT = "defaultUnit";
            public const string IS_FIXED_VALUE = "isFixedValue";
            public const string MAX_VALUE = "max";
            public const string MIN_VALUE = "min";
            public const string PARAMETER_FLAG = "flag";
            public const string SEQUENCE = "seq";
            public const string REFERENCE_ID = "ref";
            public const string GROUP_ID = "group";
            public const string BUILDING_BLOCK = "bb";
            public const string NAME = "name";
            public const string PARAMETER_ID = "para";
            public const string BUILDING_BLOCK_ID = "bb";
            public const string LLOQ = "lloq";
            public const string DEFAULT_VALUE = "default";
            public const string UNIT_NAME = "unitName";
            public const string VALUE_DESCRIPTION = "valueDescription";
            public const string X = "x";
            public const string Y = "y";
            public const string Width = "width";
            public const string Height = "height";
            public const string DESCRIPTION = "description";
         }
      }

      public static class ParameterExport
      {
         public const string PARAMETER_PATH = "ParameterPath";
         public const string VALUE = "Value";
         public const string FORMULA = "Formula";
         public const string RHS_FORMULA = "RHSFormula";
      }

      public const string NOT = "Not";
      public const string AND = "And";
      public const string OR = "Or";
      public const string IN_CONTAINER = "In container";
      public const string NOT_IN_CONTAINER = "Not in container";
      public const string IN_PARENT = "In parent";
      public const string IN_CHILDREN = "In children";
      public const string LLOQ = "LLOQ";

      public static string NameWithUnitFor(string name, IDimension dimension) => NameWithUnitFor(name, dimension?.DefaultUnit);

      public static string NameWithUnitFor(string name, Unit unit)
      {
         if (unit == null)
            return name;

         return NameWithUnitFor(name, unit.Name);
      }

      public static string NameWithUnitFor(string name, string unit)
      {
         if (string.IsNullOrWhiteSpace(unit))
            return name;

         return $"{name} [{unit}]";
      }

      public static class Command
      {
         public static readonly string BUILDING_BLOCK_TYPE = "BuildingBlockType";
         public static readonly string BUILDING_BLOCK_NAME = "BuildingBlockName";
      }

      public static class ChartFontOptions
      {
         public const string DEFAULT_FONT_FAMILY_NAME = "Microsoft Sans Serif";

         public const int DEFAULT_FONT_SIZE_LEGEND = 8;
         public const int DEFAULT_FONT_SIZE_AXIS = 10;
         public const int DEFAULT_FONT_SIZE_TITLE = 16;
         public const int DEFAULT_FONT_SIZE_DESCRIPTION = 12;
         public const int DEFAULT_FONT_SIZE_ORIGIN = 8;
         public const int DEFAULT_FONT_SIZE_WATERMARK = 32;
         public const int DEFAULT_FONT_SIZE_TITLE_FOR_PARAMETER_IDENTIFICATION_FEEDBACK = 12;

         //IMPORTANT: Default font sizes need to be in the list of AllFontSizes otherwise UI binding won't work
         public static readonly IReadOnlyList<int> AllFontSizes = new[] {DEFAULT_FONT_SIZE_LEGEND, 9, 10, 11, 12, 14, 16, 18, 20, 24, 32, 40, 48, 60};

         public static readonly IReadOnlyList<string> AllFontFamilies = new[] {"Arial", "Helvetica", "Tahoma", "Times New Roman", DEFAULT_FONT_FAMILY_NAME};

         public static readonly Color DEFAULT_FONT_COLOR_WATERMARK = Color.Black;
      }

      public static class MultiCurveOptions
      {
         public static readonly IReadOnlyList<bool?> AllBooleanOptions = new bool?[] {null, false, true};
      }

      public static class ImporterConstants
      {
         public static readonly string[] NAMING_PATTERN_SEPARATORS = {".", ",", "-", "_"};
         public static readonly string Undefined = "Undefined";
         public static readonly string GroupingBySuffix = "_GroupBy";
      }

      public static class LoggerConstants
      {
         public const string DEFAULT_LOG_ENTRY_TEMPLATE = "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff} {SourceContext:l} {Level:u}] {Message:l} {NewLine:l} {Exception}";
      }
   }
}