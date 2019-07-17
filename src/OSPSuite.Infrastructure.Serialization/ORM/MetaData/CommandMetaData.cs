using System.Collections.Generic;

namespace OSPSuite.Infrastructure.ORM.MetaData
{
   public class CommandMetaData : MetaData<string>
   {
      public CommandMetaData()
      {
         Children = new List<CommandMetaData>();
         Properties = new Dictionary<string, CommandPropertyMetaData>();
      }

      public virtual string CommandId { get; set; }
      public virtual bool Visible { get; set; }
      public virtual string CommandInverseId { get; set; }
      public virtual string CommandType { get; set; }
      public virtual string ObjectType { get; set; }
      public virtual string Description { get; set; }
      public virtual string ExtendedDescription { get; set; }
      public virtual string Comment { get; set; }
      public virtual string Discriminator { get; set; }
      public virtual int? Sequence { get; set; }
      public virtual CommandMetaData Parent { get; set; }

      public virtual IList<CommandMetaData> Children { get; private set; }

      public virtual IDictionary<string, CommandPropertyMetaData> Properties { get; set; }

      public virtual void AddCommand(CommandMetaData commandMetaData)
      {
         Children.Add(commandMetaData);
         commandMetaData.Parent = this;
         commandMetaData.Sequence = Children.Count - 1;
      }

      public virtual void AddProperty(CommandPropertyMetaData commandPropertyMetaData)
      {
         Properties.Add(commandPropertyMetaData.Name, commandPropertyMetaData);
      }
   }
}