using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;

namespace OSPSuite.Starter.Tasks
{
    public class ParameterValueSetCommand : Command,IReversibleCommand<MyContext> 
    {
        private double _oldValue;
        private readonly Parameter _parameter;
        private readonly double _newValue;

        public ParameterValueSetCommand(Parameter parameter, double newValue)
        {
            CommandType = "Edit";
            ObjectType = "Parameter";
            Description = $"Setting Parameter Value from {parameter.Value} to {newValue}";
            _oldValue = parameter.Value;
            _newValue = newValue;
            _parameter = parameter;

        }

        public void Execute(MyContext context)
        {
            _parameter.Value = _newValue;
        }

        public void RestoreExecutionData(MyContext context)
        {
            
        }

        public IReversibleCommand<MyContext> InverseCommand(MyContext context)
        {
            return new ParameterValueSetCommand(_parameter,_oldValue){_oldValue = _newValue}.AsInverseFor(this);
        }
    }
}