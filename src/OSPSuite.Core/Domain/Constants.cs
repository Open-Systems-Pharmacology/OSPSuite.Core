using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Serialization;

namespace OSPSuite.Core.Domain
{
   public static class Constants
   {
      public const int PKML_VERSION = PKMLVersion.Current;
      public const string CVODES = "CVODES";
      public const int SIM_MODEL_XML_VERSION = 4;
      public const int MAX_NUMBER_OF_POINTS_PER_INTERVAL = 5000;
      public const int MIN_NUMBER_OF_POINTS_PER_INTERVAL = 2;
      public const int MAX_NUMBER_OF_CHAR_IN_TABLE_NAME = 29;
      public static readonly IReadOnlyList<string> ILLEGAL_CHARACTERS = new List<string> {ObjectPath.PATH_DELIMITER, ":", "*", "?", "<", ">", "|", "{", "}", "\""}.Distinct().ToList();

      public const string DRUG_MASS = "DrugMass";
      public const string MOLECULE_PROPERTIES = "MoleculeProperties";
      public const string APPLICATION_TRANSPORT_NAME = "ApplicationTransport";
      public const string START_APPLICATION_EVENT = "StartApplication";
      public const string NEIGHBORHOODS = "Neighborhoods";
      public const string EVENTS = "Events";
      public const string ORGANISM = "Organism";
      public const string APPLICATIONS = "Applications";
      public const string ROOT = "ROOT";
      public const string NAME_PROPERTY = "Name";
      public const int NOT_FOUND_INDEX = -1;

      public const string ACTIVE = "Active";
      public const string PASSIVE = "Passive";
      public const string TIME = "Time";
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

      public const string VOLUME_ALIAS = "V";
      public const string START_VALUE_ALIAS = "StartValue";
      public const string TOO_OLD_PKML = "PKML file is too old and cannot be converted";

      public const string MOL_WEIGHT_EXTENDED_PROPERTY = "MolWeight";

      //tolerated precision to relatively compare to double values 
      public const double DOUBLE_RELATIVE_EPSILON = 1e-5;
      public const double DOUBLE_PERCENTILE_RELATIVE_TOLERANCE = 1e-2;

      public const float FLOAT_RELATIVE_EPSILON = 0.00001f;
      public const double CONFIDENCE_INTERVAL_ALPHA = 0.05;

      public static readonly string ProjectUndefined = "Undefined";
      public const string DISPLAY_PATH_SEPARATOR = "-";
      public const string NAN = "<NaN>";
      public const string CHILD = "Child";

      public const string PRODUCT_SITE = "www.open-systems-pharmacology.org";
      public const string PRODUCT_SITE_DOWNLOAD = "http://setup.open-systems-pharmacology.org";
      public const string HELP_NAMESPACE = "http://docs.open-systems-pharmacology.org";
      public const string FORUM_SITE = "forum.open-systems-pharmacology.org";
      public const string SUITE_NAME = "Open Systems Pharmacology Suite";

      public const float DEFAULT_WEIGHT = 1;
      public const double DEFAULT_USE_AS_FACTOR = 1;
      public const double DEFAULT_PARAMETER_RANGE_FACTOR = 10;
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

      //sensitivity values below this value will be set to zero
      public const double SENSITIVITY_THRESHOLD = 1.0e-4;
      public const string STD_DEV_GEOMETRIC = "Geometric Standard Deviation";
      public const string STD_DEV_ARITHMETIC = "Arithmetic Standard Deviation";
      public const string AUXILIARY_TYPE = "AuxiliaryType";
      public const string FILE = "File";
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
         public const string SIM_MODEL_SCHEMA_FILE_NAME = "OSPSuite.SimModel.xsd";
         public const string CHART_LAYOUT_FOLDER_NAME = "ChartLayouts";
         public const string TEX_TEMPLATE_FOLDER_NAME = "TeXTemplates";
      }

      public static class Parameters
      {
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
         public const string PARTICLE_SIZE_DISTRIBUTION = "Particle size distribution";
         public const string NUMBER_OF_BINS = "Number of bins";

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
            ENABLE_SUPERSATURATION
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
            PLASMA_PROTEIN_BINDING_PARTNER
         };

         public static readonly IReadOnlyCollection<string> AllWithListOfValues = new List<string>(Halogens.Union(AllCategorialParameters));

         //end of  delete
      }

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
         public static readonly string HISTORY_FILE_FILTER = FileFilter("History Export", XLS_EXTENSION);
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
         public const string MASS_CONCENTRATION = "Concentration (mass)";
         public const string DIMENSIONLESS = "Dimensionless";
         public const string AMOUNT = "Amount";
         public const string MASS_AMOUNT = "Mass";
         public const string TIME = "Time";
         public const string RESOLUTION = "Resolution";
         public const string RHS_DIMENSION_SUFFIX = " per Time";
         public const string AMOUNT_PER_TIME = "Amount per time";
         public const string MOLAR_CONCENTRATION_PER_TIME = "Concentration (molar) per time";
         public const string MOLECULAR_WEIGHT = "Molecular weight";
         public const string LOG_UNITS = "Log Units";
         public const string FRACTION = "Fraction";

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
         internal const string FirstIntervalSuffix = "_t1_t2";
         internal const string LastIntervalSuffix = "_tLast_tEnd";
         internal const string NormSuffix = "_norm";
         public const string C_max = "C_max";
         public const string C_tEnd = "C_tEnd";
         public const string Tmax = "t_max";
         public const string AUC_inf = "AUC_inf";
         public const string AUC = "AUC";
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

         public static readonly string AUC_norm = NormalizedName(AUC);
         public static readonly string C_max_norm = NormalizedName(C_max);
         public static readonly string AUC_inf_norm = NormalizedName(AUC_inf);

         public static readonly string C_max_t1_t2 = Create(C_max, FirstIntervalSuffix);
         public static readonly string C_max_t1_t2_norm = NormalizedName(C_max_t1_t2);

         public static readonly string C_max_tLast_tEnd = Create(C_max, LastIntervalSuffix);
         public static readonly string C_max_tLast_tEnd_norm = NormalizedName(C_max_tLast_tEnd);

         public static readonly string Tmax_t1_t2 = Create(Tmax, FirstIntervalSuffix);
         public static readonly string Tmax_tLast_tEnd = Create(Tmax, LastIntervalSuffix);

         public static readonly string AUC_inf_t1 = Create(AUC_inf, "_t1");
         public static readonly string AUC_inf_t1_norm = NormalizedName(AUC_inf_t1);

         public static readonly string AUC_t1_t2 = Create(AUC, FirstIntervalSuffix);
         public static readonly string AUC_t1_t2_norm = NormalizedName(AUC_t1_t2);

         public const string AUC_tLast_minus_1_tLast = "AUC_tLast_minus_1_tLast";
         public static readonly string AUC_tLast_minus_1_tLast_norm = NormalizedName(AUC_tLast_minus_1_tLast);

         public const string AUC_inf_tLast = "AUC_inf_tLast";
         public static readonly string AUC_inf_tLast_norm = NormalizedName(AUC_inf_tLast);

         public static readonly string Ctrough_t2 = Create(Ctrough, "_t2");
         public static readonly string Ctrough_tLast = Create(Ctrough, "_tLast");

         public static readonly string Thalf_tLast_tEnd = Create(Thalf, LastIntervalSuffix);

         internal static string NormalizedName(string pkParameter)
         {
            return $"{pkParameter}{NormSuffix}";
         }
      }

      public static class Population
      {
         public const string ALL_GENDER = "AllGender";
         public const string TIME_COLUMN = "Time";
         public const string VALUE_COLUMN = "Value";
         public const string PARAMETER_PATH_COLUMN = "ParameterPath";
         public const string INDIVIDUAL_ID_COLUMN = "IndividualId";
      }

      public static class OptimizationAlgorithm
      {
         public const string NELDER_MEAD_PKSIM = "Nelder Mead (PK-Sim)";
         public const string MPFIT = "Levenberg - Marquardt (MPFit)";
         public const string MONTE_CARLO = "Monte - Carlo";
         public const string DEFAULT = MPFIT;
      }

      public class CategoryOptimizations
      {
         public const string COMPOUND = "Compound";
         public const string CATEGORY = "Category";
         public const string CATEGORY_DISPLAY = "Category Display";
         public const string CALCULATION_METHOD = "CalculationMethod";
         public const string CALCULATION_METHOD_DISPLAY = "Calculation Method Display";
         public const string VALUE = "Value";
         public const int WARNING_THRESHOLD = 5;
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

      public static class Serialization
      {
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

      public static readonly string NOT = "Not";
      public static readonly string AND = "and";
      public static readonly string IN_CONTAINER = "In container";
      public static readonly string NOT_IN_CONTAINER = "Not in container";
      public static readonly string LLOQ = "LLOQ";

      public static string NameWithUnitFor(string name, IDimension dimension)
      {
         return NameWithUnitFor(name, dimension?.DefaultUnit);
      }

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
         public static readonly IReadOnlyList<string> AllFontFamilies = new[] {"Arial", "Helvetica", "Tahoma", "Times New Roman"};

         public static readonly string DEFAULT_FONT_FAMILY_NAME = FontFamily.GenericSansSerif.Name;

         public const int DEFAULT_FONT_SIZE_LEGEND = 8;
         public const int DEFAULT_FONT_SIZE_AXIS = 10;
         public const int DEFAULT_FONT_SIZE_TITLE = 16;
         public const int DEFAULT_FONT_SIZE_DESCRIPTION = 12;
         public const int DEFAULT_FONT_SIZE_ORIGIN = 8;
         public const int DEFAULT_FONT_SIZE_WATERMARK = 32;
         public const int DEFAULT_FONT_SIZE_TITLE_FOR_PARAMETER_IDENTIFICATION_FEEDBACK = 12;

         //IMPORTANT: Default font sizes need to be in the list of AllFontSizes otherwise UI binding won't work
         public static readonly IReadOnlyList<int> AllFontSizes = new[] {8, 9, 10, 11, 12, 14, 16, 18, 20, 24, 32, 40, 48, 60};

         public static readonly Color DEFAULT_FONT_COLOR_WATERMARK = Color.Black;
      }
   }
}