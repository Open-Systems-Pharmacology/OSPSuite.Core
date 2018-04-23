using System.Collections.Generic;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain.Formulas;

namespace OSPSuite.Core.Domain.Services
{
   public interface IParameterAnalysableParameterSelector
   {
      /// <summary>
      /// Returns <c>true</c> if the parameter can be used for the analysis otherwise <c>false</c>. The implementation is application specific
      /// </summary>
      /// <param name="parameter"></param>
      bool CanUseParameter(IParameter parameter);

      /// <summary>
      /// Returns the default <see cref="ParameterGroupingMode"/> to be use when selecting parameters (this is app specific)
      /// </summary>
      ParameterGroupingMode DefaultParameterSelectionMode { get; }
   }

   public abstract class AbstractParameterAnalysableParameterSelector : IParameterAnalysableParameterSelector
   {
      private readonly List<string> _categorialParameters;

      protected AbstractParameterAnalysableParameterSelector()
      {
         _categorialParameters = new List<string>(Constants.Parameters.AllCategorialParameters);
      }

      protected virtual bool ParameterIsTable(IParameter parameter)
      {
         return parameter.Formula.IsAnImplementationOf<TableFormula>();
      }

      protected virtual bool ParameterIsCategorial(IParameter parameter)
      {
         return _categorialParameters.Contains(parameter.Name);
      }

      public abstract bool CanUseParameter(IParameter parameter);
      public abstract ParameterGroupingMode DefaultParameterSelectionMode { get; }
   }
}