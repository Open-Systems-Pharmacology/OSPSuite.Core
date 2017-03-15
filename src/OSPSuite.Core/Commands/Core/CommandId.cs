using System;

namespace OSPSuite.Core.Commands.Core
{
   public class CommandId
   {
      private readonly string _id;
      private readonly string _inverseId;

      public CommandId() : this(Guid.NewGuid().ToString(), null)
      {
      }

      public CommandId(string id, string inverseId)
      {
         _id = id;
         _inverseId = inverseId;
      }

      public CommandId CreateInverseId()
      {
         if (string.IsNullOrEmpty(_inverseId))
            return new CommandId(Guid.NewGuid().ToString(), _id);

         return new CommandId(_inverseId, string.Empty);
      }

      public bool IsInverseFor(CommandId commandId)
      {
         return commandId != null
                && (
                      string.Equals(commandId._id, _inverseId) ||
                      string.Equals(commandId._inverseId, _id)
                   );
      }

      public override bool Equals(object obj)
      {
         var commandId = obj as CommandId;
         return commandId != null && string.Equals(commandId._id, _id);
      }

      public override int GetHashCode()
      {
         return _id.GetHashCode();
      }

      public string Id
      {
         get { return _id; }
      }

      public string InverseId
      {
         get { return _inverseId; }
      }

      public override string ToString()
      {
         return Id;
      }
   }
}