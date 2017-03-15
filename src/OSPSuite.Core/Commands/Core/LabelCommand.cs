namespace OSPSuite.Core.Commands.Core
{
   public interface ILabelCommand : IInfoCommand
    {
    }

    public class LabelCommand : Command, ILabelCommand
    {
        public LabelCommand()
        {
            CommandType = "Label";
        }
    }
}