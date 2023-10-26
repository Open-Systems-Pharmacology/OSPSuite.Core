namespace OSPSuite.Assets
{
   public interface IWithIcon
   {
      string IconName { get; }
   }

   public static class IconNames
   {
      public const string CALCULATION_METHOD = "CoreCalculationMethod";
      public const string SIMULATION_SETTINGS = "SimulationSettings";
      public const string SPATIAL_STRUCTURE = "SpatialStructure";
      public const string EVENT_GROUP = "EventGroup";
      public const string EVENT = "Event";
      public const string MOLECULE = "Molecule";
      public const string INITIAL_CONDITIONS = "InitialConditions";
      public const string OBSERVER = "Observer";
      public const string PARAMETER_VALUES = "ParameterValues";
      public const string REACTION = "Reaction";
      public const string PASSIVE_TRANSPORT = "PassiveTransport";
      public const string PARAMETER = "Parameter";
      public const string OBSERVED_DATA = "ObservedData";
      public const string SIMULATION = "Simulation";
      public const string FILE = "File";
      public const string EXCEL = "Excel";
      public const string JOURNAL_EXPORT_TO_WORD = "JournalExportToWord";
      public const string PDF = "PDF";
      public const string MATLAB = "Matlab";
      public const string R = "R";
      public const string PKML = "PKML";
      public const string RESULTS_IMPORT_FROM_CSV = "ResultsImportFromCSV";
      public const string REPORT = "Report";
      public const string OTHER = "Other";
      public const string ADD = "Add";
      public const string WARNING = "Warning";
      public const string RUN = "Run";
      public const string OK = "OK";
      public const string STOP = "Stop";
      public const string ERROR = "Error";
      public const string SOLVER = "Solver";
      public const string VALUE_ORIGIN_METHOD_ASSUMPTION = "ValueOriginMethodAssumption";
      public const string VALUE_ORIGIN_METHOD_IN_VITRO = "ValueOriginMethodInVitro";
      public const string VALUE_ORIGIN_METHOD_IN_VIVO = "ValueOriginMethodInVivo";
      public const string VALUE_ORIGIN_METHOD_MANUAL_FIT = "ValueOriginMethodManualFit";
      public const string VALUE_ORIGIN_METHOD_OTHER = "ValueOriginMethodOther";
      public const string VALUE_ORIGIN_METHOD_UNKNOWN = "ValueOriginMethodUnknown";
      public const string VALUE_ORIGIN_METHOD_PARAMETER_IDENTIFICATION = "ValueOriginMethodParameterIdentification";
      public const string VALUE_ORIGIN_SOURCE_DATABASE = "ValueOriginSourceDatabase";
      public const string VALUE_ORIGIN_SOURCE_INTERNET = "ValueOriginSourceInternet";
      public const string VALUE_ORIGIN_SOURCE_PUBLICATION = "ValueOriginSourcePublication";
      public const string VALUE_ORIGIN_SOURCE_PARAMETER_IDENTIFICATION = "ValueOriginSourceParameterIdentification";
      public const string VALUE_ORIGIN_SOURCE_OTHER = "ValueOriginSourceOther";
      public const string VALUE_ORIGIN_SOURCE_UNKNOWN = "ValueOriginSourceUnknown";
      public const string PKSIM = "PKSim";
      public const string MOBI = "MoBi";
      public const string PARAMETER_IDENTIFICATION = "ParameterIdentification";
      public const string SENSITIVITY_ANALYSIS = "SensitivityAnalysis";
      public static string Transporter = "Transporter";
      public static string Protein = "Protein";
      public static string Enzyme = "Enzyme";
      public const string Module = "Module";
      public const string PKSimModule = "PKSimModule";
   }
}