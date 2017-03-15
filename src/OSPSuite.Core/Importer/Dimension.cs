using System.Collections.Generic;
using System.Linq;

namespace OSPSuite.Core.Importer
{
   public class Dimension
   {
      private string _name;

      public string Name
      {
         get { return _name; }
         set
         {
            _name = value;
            if (string.IsNullOrEmpty(DisplayName))
               DisplayName = Name;
         }
      }

      private string _displayName;

      public string DisplayName
      {
         get { return string.IsNullOrEmpty(_displayName) ? Name : _displayName; }
         set { _displayName = value; }
      }

      public bool IsDefault { get; set; }

      /// <summary>
      ///    This property can be used to define condition of existing metadata on the column which must be
      ///    fullfilled for selecting this dimension.
      /// </summary>
      public Dictionary<string, string> MetaDataConditions { get; set; }

      /// <summary>
      ///    This property is a list of units supported for this dimension.
      /// </summary>
      public IList<Unit> Units { get; set; }

      /// <summary>
      ///    This property is a list of input parameters needed for this dimension.
      /// </summary>
      public IList<InputParameter> InputParameters { get; set; }

      /// <summary>
      ///    This methods tries to find a specified unit of the dimension.
      /// </summary>
      /// <param name="name">Name of the unit to search for.</param>
      /// <returns></returns>
      public Unit FindUnit(string name)
      {
         foreach (var unit in Units)
         {
            if (unit.IsEqual(name))
               return unit;
         }
         throw new UnitNotFound(this, name);
      }

      public bool HasUnit(string name)
      {
         return Units.Any(unit => unit.IsEqual(name));
      }

      /// <summary>
      ///    Method for getting the default unit of a dimension.
      /// </summary>
      /// <returns>Default unit of dimension.</returns>
      public Unit GetDefaultUnit()
      {
         if (Units == null) throw new MissingUnitsForDimension(this);
         foreach (var unit in Units)
         {
            if (!unit.IsDefault) continue;
            return unit;
         }
         throw new MissingDefaultUnitForDimension(this);
      }

      /// <summary>
      ///    This methods checks whether the dimension is well defined.
      /// </summary>
      public bool Check()
      {
         if (string.IsNullOrEmpty(Name)) throw new MissingDimensionName();
         var unitfound = false;
         if (Units == null)
            throw new MissingUnitsForDimension(this);

         // check default settings for units
         foreach (var unit in Units)
         {
            if (unit.Name == null) throw new MissingUnitName();
            if (!unit.IsDefault) continue;
            if (unitfound)
               throw new MultipleDefaultUnitsFoundForDimension(this);
            unitfound = true;
         }
         if (!unitfound)
            throw new MissingDefaultUnitForDimension(this);

         // check uniqueness of unit names
         IDictionary<string, Unit> checkUniqueNames = new Dictionary<string, Unit>(Units.Count);
         IDictionary<string, Unit> checkUniqueDisplayNames = new Dictionary<string, Unit>(Units.Count);
         foreach (var unit in Units)
         {
            try
            {
               checkUniqueNames.Add(unit.Name, unit);
               checkUniqueDisplayNames.Add(unit.DisplayName, unit);
            }
            catch
            {
               throw new DublicatedUnitForDimension(this, unit);
            }
         }

         // check uniqueness of input parameter names
         if (InputParameters == null) return true;
         IDictionary<string, InputParameter> checkUniqueInputParameterNames = new Dictionary<string, InputParameter>(InputParameters.Count);
         IDictionary<string, InputParameter> checkUniqueInputParameterDisplayNames = new Dictionary<string, InputParameter>(InputParameters.Count);
         foreach (var inputParamter in InputParameters)
         {
            try
            {
               checkUniqueInputParameterNames.Add(inputParamter.Name, inputParamter);
               checkUniqueInputParameterDisplayNames.Add(inputParamter.DisplayName, inputParamter);
            }
            catch
            {
               throw new DublicatedInputParameterForDimension(this, inputParamter);
            }
         }

         return true;
      }

      /// <summary>
      ///    Method indicating if input parameters are required.
      /// </summary>
      public bool AreInputParametersRequired()
      {
         if (InputParameters == null) return false;
         return InputParameters.Count != 0;
      }

      /// <summary>
      ///    Method indicating if input parameters are missed.
      /// </summary>
      public bool AreInputParametersMissing()
      {
         if (!AreInputParametersRequired()) return false;

         foreach (var inputParameter in InputParameters)
         {
            if (inputParameter.Value == null) return true;
         }
         return false;
      }

      public bool  IsDimensionless()
      {
         return Units.Count == 1 && string.IsNullOrEmpty(Units[0].Name);
      }
   }
}