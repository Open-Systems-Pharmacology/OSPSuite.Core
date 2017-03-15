namespace OSPSuite.Core.Commands.Core
{
   public interface IInfoCommand : ICommand
   {
   }

   public class InfoCommand : Command, IInfoCommand
   {
      public InfoCommand()
      {
         CommandType = "Info";
      }
   }
}