using System.Collections.Generic;

namespace OSPSuite.Core.Commands.Core
{
   /// <summary>
   /// Represents an object that can collect commands
   /// </summary>
   public interface ICommandCollector
   {
      /// <summary>
      /// Add a command to the collector
      /// </summary>
      /// <param name="command">command to add</param>
      void AddCommand(ICommand command);

      /// <summary>
      /// Returns all the command added to the collector
      /// </summary>
      /// <returns></returns>
      IEnumerable<ICommand> All();
   }
}