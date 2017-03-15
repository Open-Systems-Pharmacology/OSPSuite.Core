using OSPSuite.Utility.Exceptions;

namespace OSPSuite.Core.Importer
{
   public class MissingDimensionName : OSPSuiteException
   {
      /// <summary>
      ///    This is an exception indicating that there is no name declared for dimension.
      /// </summary>
      public MissingDimensionName() :
         base("The dimension has no name.")
      {
      }
   }

   public class DimensionNotFound : OSPSuiteException
   {
      /// <summary>
      ///    This is an exception indicating that the dimension could not be found.
      ///    <param name="name">Name of the searched dimension.</param>
      /// </summary>
      public DimensionNotFound(string name) :
         base($"The dimension {name} could not be found.")
      {
      }
   }

   public class MissingDefaultDimension : OSPSuiteException
   {
      /// <summary>
      ///    This is an exception indicating that there is no dimension declared as default dimension.
      /// </summary>
      public MissingDefaultDimension() :
         base("There is no dimension defined as default dimension.")
      {
      }
   }

   public class MultipleDefaultDimensionsFound : OSPSuiteException
   {
      /// <summary>
      ///    This is an exception indicating that there are more than dimensions declared as default dimension.
      /// </summary>
      public MultipleDefaultDimensionsFound() :
         base("There are more than one dimensions defined as default dimension.")
      {
      }
   }

   public class MissingUnitName : OSPSuiteException
   {
      /// <summary>
      ///    This is an exception indicating that there is no name declared for unit.
      /// </summary>
      public MissingUnitName() :
         base("The unit has no name.")
      {
      }
   }

   public class UnitNotFound : OSPSuiteException
   {
      /// <summary>
      ///    This is an exception indicating that the dimension had no unit with specified name.
      ///    <param name="dimension">Dimension whith missing unit.</param>
      ///    <param name="name">Name of the searched unit.</param>
      /// </summary>
      public UnitNotFound(Dimension dimension, string name) :
         base($"The dimension {dimension.DisplayName} has no unit with name {name}.")
      {
      }
   }

   public class MissingUnitsForDimension : OSPSuiteException
   {
      /// <summary>
      ///    This is an exception indicating that the there are units declared for dimension.
      /// </summary>
      /// <param name="dimension">Dimension with multiple default units.</param>
      public MissingUnitsForDimension(Dimension dimension) :
         base($"There are no units defined for dimension {dimension.DisplayName}.")
      {
      }
   }

   public class MultipleDefaultUnitsFoundForDimension : OSPSuiteException
   {
      /// <summary>
      ///    This is an exception indicating that the there are more than one units declared as default unit for dimension.
      /// </summary>
      /// <param name="dimension">Dimension with multiple default units.</param>
      public MultipleDefaultUnitsFoundForDimension(Dimension dimension) :
         base($"There are more than one units defined as default unit for dimension {dimension.DisplayName}.")
      {
      }
   }

   public class MissingDefaultUnitForDimension : OSPSuiteException
   {
      /// <summary>
      ///    This is an exception indicating that there is no unit declared as default unit for dimension.
      /// </summary>
      /// <param name="dimension">Dimension with missing default units.</param>
      public MissingDefaultUnitForDimension(Dimension dimension) :
         base($"There is no default unit defined for dimension {dimension.DisplayName}")
      {
      }
   }

   public class DublicatedUnitForDimension : OSPSuiteException
   {
      /// <summary>
      ///    This is an exception indicating that there is already a unit declared for the dimension with this name.
      /// </summary>
      /// <param name="dimension">Dimension with dublicated unit.</param>
      /// <param name="unit">Unit which is dublicated.</param>
      public DublicatedUnitForDimension(Dimension dimension, Unit unit) :
         base($"There is already a unit with name {unit.DisplayName} defined for dimension {dimension.DisplayName}")
      {
      }
   }

   public class DublicatedInputParameterForDimension : OSPSuiteException
   {
      /// <summary>
      ///    This is an exception indicating that there is already a unit declared for the dimension with this name.
      /// </summary>
      /// <param name="dimension">Dimension with dublicated unit.</param>
      /// <param name="inputParameter">Input parameter which is dublicated.</param>
      public DublicatedInputParameterForDimension(Dimension dimension, InputParameter inputParameter) :
         base($"There is already an inputParameter with name {inputParameter.DisplayName} defined for dimension {dimension.DisplayName}")
      {
      }
   }
}