using OSPSuite.Core.Commands.Core;

namespace OSPSuite.Core.Domain.Services
{
   public interface ISetParameterTask
   {
      /// <summary>
      /// Sets the <paramref name="value"/> into <paramref name="parameter"/> defined in <paramref name="simulation"/>
      /// </summary>
      /// <returns>The executed command representing the value being set in the parameter</returns>
      ICommand SetParameterValue(IParameter parameter, double value, ISimulation simulation);


      /// <summary>
      /// Updates the parameter value origin in the <paramref name="parameter"/> defined in <paramref name="simulation"/>
      /// </summary>
      /// <returns>The executed command representing the <see cref="ValueOrigin"/> being set in the parameter</returns>
      ICommand UpdateParameterValueOrigin(IParameter parameter, ValueOrigin valueOrigin, ISimulation simulation);
   }
}