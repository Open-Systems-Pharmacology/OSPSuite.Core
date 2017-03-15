using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.XtraTreeList;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Presentation.DTO.Commands;
using OSPSuite.Presentation.Presenters.Commands;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.UI.DTO.Commands
{
   public class HistoryItemDTO : TreeList.IVirtualTreeListData, IHistoryItemDTO
   {
      private readonly HistoryItemDTOList _subHistoryItems;

      public int State { get; set; }
      public string Id { get; set; }
      public string User { get; set; }
      public DateTime DateTime { get; set; }
      public string CommandType { get; set; }
      public string ObjectType { get; set; }
      public string Description { get; set; }
      public string ExtendedDescription { get; set; }
      public IList<string> ExtendedProperties { get; private set; }
      public bool Loaded { get; set; }

      public IHistoryItemDTO Parent { get; set; }

      public HistoryItemDTO(ICommand command)
      {
         Command = command;
         _subHistoryItems = new HistoryItemDTOList();
         ExtendedProperties = new List<string>();
      }

      public string Comment
      {
         get { return Command.Comment; }
         set { Command.Comment = value; }
      }

      public ICommand Command { get; }

      public void VirtualTreeGetChildNodes(VirtualTreeGetChildNodesInfo info)
      {
         _subHistoryItems.VirtualTreeGetChildNodes(info);
      }

      public void VirtualTreeGetCellValue(VirtualTreeGetCellValueInfo info)
      {
         info.CellData = getValuesAt(info.Column.AbsoluteIndex);
      }

      public void VirtualTreeSetCellValue(VirtualTreeSetCellValueInfo info)
      {
      }

      private object getValuesAt(int index)
      {
         if (HistoryColumns.State.Index == index)
            return State;
         if (HistoryColumns.User.Index == index)
            return User;
         if (HistoryColumns.Time.Index == index)
            return DateTime.ToIsoFormat();
         if (HistoryColumns.CommandType.Index == index)
            return CommandType;
         if (HistoryColumns.ObjectType.Index == index)
            return ObjectType;
         if (HistoryColumns.Description.Index == index)
            return Description;
         if (HistoryColumns.Id.Index == index)
            return Id;
         if (HistoryColumns.Comment.Index == index)
            return Comment;

         int dynamicIndex = index - HistoryColumns.FixedColumnIndex - 1;
         if (dynamicIndex < 0)
            return string.Empty;

         if (dynamicIndex < ExtendedProperties.Count)
            return ExtendedProperties[dynamicIndex];

         return string.Empty;
      }

      public void AddSubHistory(IHistoryItemDTO historyItemDto)
      {
         if (historyItemDto.IsAnImplementationOf<NullHistoryItemDTO>())
            return;

         _subHistoryItems.Add(historyItemDto);
         historyItemDto.Parent = this;
      }

      public void RemoveSubHistory(IHistoryItemDTO historyItemDto)
      {
         _subHistoryItems.Remove(historyItemDto);
         historyItemDto.Parent = null;
      }

      public IList<IHistoryItemDTO> Children()
      {
         return _subHistoryItems.All();
      }

      public IEnumerable<IHistoryItemDTO> AllLeafs
      {
         get
         {
            //this is a leaf already: return 
            if (_subHistoryItems.Count == 0)
               return new[] {this};

            IEnumerable<IHistoryItemDTO> allChildren = new List<IHistoryItemDTO>();
            foreach (var node in _subHistoryItems.All())
            {
               allChildren = allChildren.Union(node.AllLeafs);
            }
            return allChildren;
         }
      }
   }

}