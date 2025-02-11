using System;

namespace OSPSuite.Core.Domain.Data
{
   public enum ColumnOrigins
   {
      Undefined,

      /// <summary>
      ///    Time grid
      /// </summary>
      BaseGrid,

      /// <summary>
      ///    Calculated value from model
      /// </summary>
      Calculation,

      /// <summary>
      ///    Observed Data
      /// </summary>
      Observation,

      /// <summary>
      ///    Column is used to extend the information on another observed data (typically error)
      /// </summary>
      ObservationAuxiliary, // if Observation and AuxiliaryType defined

      /// <summary>
      ///    Column is based on calculation values but is used in a different context (for example ParameterIdentification plots)
      /// </summary>
      CalculationAuxiliary,

      /// <summary>
      ///    Column is an identity curve or deviation line to facilitate the viewing of a plot 
      /// </summary>
      DeviationLine
   }

   public enum AuxiliaryType
   {
      Undefined,
      ArithmeticStdDev,
      GeometricStdDev,

      //Use to specify that the related column is the arithmetic mean data of a range plot
      ArithmeticMeanPop,

      //Use to specify that the related column is the geometric mean data of a range plot
      GeometricMeanPop
   }

   public class DataInfo
   {
      public ColumnOrigins Origin { get; set; }

      /// <summary>
      ///    AuxiliaryType only relevant if origin is ObservationAuxiliary. Is set to undefined otherwise
      /// </summary>
      public AuxiliaryType AuxiliaryType { get; set; }

      /// <summary>
      ///    Name of unit in which the column should be displayed per default (this is typically the unit in which observed data
      ///    were imported)
      /// </summary>
      public string DisplayUnitName { get; set; }

      /// <summary>
      ///    Extra information that can be used to group the data in a project specific fashion. (only displayed in
      ///    DataColumn.Category)
      /// </summary>
      public string Category { get; set; }

      /// <summary>
      ///    Mol weight value related to the column.
      ///    A null value means that the information is not relevant for the column or that the value was not defined.
      ///    Since the value might be set from the application itself, a set is necessary
      /// </summary>
      public double? MolWeight { get; set; }

      /// <summary>
      ///    Meta Information on the current column (organ, compartment for observed data etc.)
      /// </summary>
      public ExtendedProperties ExtendedProperties { get; }

      public float? LLOQ { get; set; }

      /// <summary>
      ///    Wrapper around the LLOQ Value defined as float so that it can be set via R (float not available in rClr)
      /// </summary>
      public double? LLOQAsDouble
      {
         get => LLOQ;
         set => LLOQ = value.HasValue ? Convert.ToSingle(value.Value) : (float?) null;
      }

      /// <summary>
      ///    Indicates the threshold that should be used to compare two output columns. This will only be set for calculate
      ///    columns
      /// </summary>
      public float? ComparisonThreshold { get; set; }

      [Obsolete("For serialization")]
      public DataInfo() : this(ColumnOrigins.Undefined)
      {
      }

      public DataInfo(ColumnOrigins columnOrigins)
         : this(columnOrigins, AuxiliaryType.Undefined, string.Empty, string.Empty, null)
      {
      }

      public DataInfo(ColumnOrigins origin, AuxiliaryType auxiliaryType, string displayUnitName, string category, double? molWeight)
      {
         Origin = origin;
         AuxiliaryType = auxiliaryType;
         DisplayUnitName = displayUnitName;
         Category = category;
         MolWeight = molWeight;
         ExtendedProperties = new ExtendedProperties();
      }

      public DataInfo Clone()
      {
         var dataInfo = new DataInfo(Origin, AuxiliaryType, DisplayUnitName, Category, MolWeight);
         dataInfo.ExtendedProperties.UpdateFrom(ExtendedProperties);
         dataInfo.LLOQ = LLOQ;
         dataInfo.ComparisonThreshold = ComparisonThreshold;
         return dataInfo;
      }
   }
}