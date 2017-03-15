using OSPSuite.Core.Commands.Core;

namespace OSPSuite.Core.Domain.Services
{
   public interface ISetParameterTask
   {
      ICommand SetParameterValue(IParameter parameter, double value,ISimulation simulation);
   }
}