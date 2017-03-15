using System;
using System.Linq;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Presentation.DTO.Commands;
using OSPSuite.Presentation.Mappers;
using OSPSuite.Presentation.Presenters.Commands;
using OSPSuite.UI.DTO.Commands;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.UI.Mappers
{
   public class HistoryToHistoryDTOMapper : IHistoryToHistoryDTOMapper
   {
      private readonly IHistoryBrowserConfiguration _historyBrowserConfiguration;
      public bool EnableHistoryPruning { get; set; }

      public HistoryToHistoryDTOMapper(IHistoryBrowserConfiguration historyBrowserConfiguration)
      {
         _historyBrowserConfiguration = historyBrowserConfiguration;
      }

      public IHistoryItemDTO MapFrom(IHistoryItem historyItem)
      {
         var historyItemDTO = mapFrom(historyItem, historyItem.Command);
         return simplifiedHistoryItemDTOFrom(historyItemDTO);
      }

      private IHistoryItemDTO simplifiedHistoryItemDTOFrom(IHistoryItemDTO historyItemDTO)
      {
         pruneHistory(historyItemDTO);
         if (historyItemDTO.Children().Count() == 1)
            return historyItemDTO.Children().ElementAt(0);

         return historyItemDTO;
      }

      private void pruneHistory(IHistoryItemDTO rootNode)
      {
         if(!EnableHistoryPruning)
            return;

         var allLeafNodes = rootNode.AllLeafs.Where(parentIsAMacroWithOneChild).ToList();

         if (allLeafNodes.Count == 0)
            return;

         bool changed = false;
         foreach (var node in allLeafNodes)
         {
            var parentHistory = node.Parent;
            //remove parent history from its own parent and add the node underneath
            var grandParent = parentHistory.Parent;
            if (grandParent != null)
            {
               parentHistory.RemoveSubHistory(node);
               grandParent.RemoveSubHistory(parentHistory);
               grandParent.AddSubHistory(node);
               changed = true;
            }
         }
         if (changed)
            pruneHistory(rootNode);
      }

      private bool parentIsAMacroWithOneChild(IHistoryItemDTO historyItemDTO)
      {
         if (historyItemDTO == null) return false;
         if (historyItemDTO.Parent == null) return false;
         return historyItemDTO.Parent.Children().Count() == 1;
      }

      private IHistoryItemDTO mapFrom(IHistoryItem historyItem, ICommand command)
      {
         if (!command.Visible)
            return new NullHistoryItemDTO();

         var historyDto = new HistoryItemDTO(command);

         historyDto.Id = Guid.NewGuid().ToString();
         historyDto.State = historyItem.State;
         historyDto.User = historyItem.User;
         historyDto.DateTime = historyItem.DateTime;
         historyDto.ObjectType = command.ObjectType;
         historyDto.CommandType = command.CommandType;
         historyDto.Description = command.Description;
         historyDto.Loaded = command.Loaded;

         historyDto.ExtendedDescription = command.ExtendedDescription;

         foreach(var dynamicColumn in _historyBrowserConfiguration.AllDynamicColumnNames)
         {
            historyDto.ExtendedProperties.Add(command.ExtendedPropertyValueFor(dynamicColumn));
         }

         //Add sub commands in the reverse order to display them in the chronological order
         var macroCommand = command as IMacroCommand;
         if (macroCommand == null) return historyDto;

         macroCommand.All().Reverse().Where(subCommand => subCommand.Visible)
            .Each(subCommand => historyDto.AddSubHistory(mapFrom(historyItem, subCommand)));

         return historyDto;
      }

   }
}