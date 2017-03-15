using System;
using System.Collections.Generic;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;

namespace OSPSuite.Presentation.DTO.Commands
{
   public interface IHistoryItemDTO : IWithId
   {
      int State { get; set; }
      string User { get; set; }
      DateTime DateTime { get; set; }
      string CommandType { get; set; }
      string ObjectType { get; set; }
      string Description { get; set; }
      string ExtendedDescription { get; set; }
      IList<string> ExtendedProperties { get; }
      bool Loaded { get; set; }
      IHistoryItemDTO Parent { get; set; }
      string Comment { get; set; }
      ICommand Command { get; }
      IEnumerable<IHistoryItemDTO> AllLeafs { get; }
      void AddSubHistory(IHistoryItemDTO historyItemDto);
      void RemoveSubHistory(IHistoryItemDTO historyItemDto);
      IList<IHistoryItemDTO> Children();
   }

   public class NullHistoryItemDTO : IHistoryItemDTO
   {
      public int State { get; set; }
      public string Id { get; set; }
      public string User { get; set; }
      public DateTime DateTime { get; set; }
      public string CommandType { get; set; }
      public string ObjectType { get; set; }
      public string Description { get; set; }
      public string ExtendedDescription { get; set; }
      public IList<string> ExtendedProperties { get; } = new List<string>();
      public bool Loaded { get; set; }
      public IHistoryItemDTO Parent { get; set; }
      public string Comment { get; set; }
      public ICommand Command { get; } = new NullCommand();
      public IEnumerable<IHistoryItemDTO> AllLeafs { get; } = new List<IHistoryItemDTO>();
      private readonly IList<IHistoryItemDTO> _children = new List<IHistoryItemDTO>();

      public void AddSubHistory(IHistoryItemDTO historyItemDto)
      {
      }

      public void RemoveSubHistory(IHistoryItemDTO historyItemDto)
      {
      }

      public IList<IHistoryItemDTO> Children()
      {
         return _children;
      }
   }

   internal class NullCommand : Command
   {
   }
}