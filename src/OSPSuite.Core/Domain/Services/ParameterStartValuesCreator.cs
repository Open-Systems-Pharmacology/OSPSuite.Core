using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Domain.Services
{
   /// <summary>
   ///    Service responsible for creation of ParameterStartValues-building blocks
   ///    based on building blocks pair {spatial structure, molecules}
   /// </summary>
   public interface IParameterStartValuesCreator : IEmptyStartValueCreator<ParameterStartValue>
   {
      /// <summary>
      ///    Creates and returns a new parameter value based on <paramref name="parameter">parameter</paramref>
      /// </summary>
      /// <param name="parameterPath">The path of the parameter</param>
      /// <param name="parameter">The Parameter object that has the start value and dimension to use</param>
      /// <returns>A new ParameterStartValue</returns>
      ParameterStartValue CreateParameterStartValue(ObjectPath parameterPath, IParameter parameter);

      /// <summary>
      ///    Creates and returns a new parameter value with <paramref name="startValue">startValue</paramref> as StartValue
      ///    and <paramref name="dimension">dimension</paramref> as dimension
      /// </summary>
      /// <param name="parameterPath">the path of the parameter</param>
      /// <param name="startValue">the value to be used as StartValue</param>
      /// <param name="dimension">the dimension of the parameter</param>
      /// <param name="displayUnit">
      ///    The display unit of the start value. If not set, the default unit of the
      ///    <paramref name="dimension" />will be used
      /// </param>
      /// <param name="valueOrigin">Value origin for this parameter value</param>
      /// <param name="isDefault">Value indicating if the value stored is the default value from the parameter.</param>
      /// <returns>A new ParameterStartValue</returns>
      ParameterStartValue CreateParameterStartValue(ObjectPath parameterPath, double startValue, IDimension dimension, Unit displayUnit = null,
         ValueOrigin valueOrigin = null, bool isDefault = false);
   }

   internal class ParameterStartValuesCreator : IParameterStartValuesCreator
   {
      private readonly IObjectBaseFactory _objectBaseFactory;
      private readonly ObjectPathFactory _objectPathFactory;
      private readonly IIdGenerator _idGenerator;

      public ParameterStartValuesCreator(IObjectBaseFactory objectBaseFactory, ObjectPathFactory objectPathFactory, IIdGenerator idGenerator)
      {
         _objectBaseFactory = objectBaseFactory;
         _objectPathFactory = objectPathFactory;
         _idGenerator = idGenerator;
      }

      
      public ParameterStartValue CreateParameterStartValue(ObjectPath parameterPath, double startValue, IDimension dimension,
         Unit displayUnit = null, ValueOrigin valueOrigin = null, bool isDefault = false)
      {
         var psv = new ParameterStartValue
         {
            Value = startValue,
            Dimension = dimension,
            Id = _idGenerator.NewId(),
            Path = parameterPath,
            DisplayUnit = displayUnit ?? dimension.DefaultUnit,
            IsDefault = isDefault
         };

         psv.ValueOrigin.UpdateAllFrom(valueOrigin);
         return psv;
      }

      public ParameterStartValue CreateParameterStartValue(ObjectPath parameterPath, IParameter parameter)
      {
         return CreateParameterStartValue(parameterPath, parameter.Value, parameter.Dimension, parameter.DisplayUnit, parameter.ValueOrigin,
            parameter.IsDefault);
      }


      public ParameterStartValue CreateEmptyStartValue(IDimension dimension) => CreateParameterStartValue(ObjectPath.Empty, 0.0, dimension);
   }
}