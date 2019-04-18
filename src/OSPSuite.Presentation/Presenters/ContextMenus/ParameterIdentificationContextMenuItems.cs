using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Extensions;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.UICommands;
using MenuBarItemExtensions = OSPSuite.Presentation.MenuAndBars.MenuBarItemExtensions;

namespace OSPSuite.Presentation.Presenters.ContextMenus
{
   public static class ParameterIdentificationContextMenuItems
   {
      public static IMenuBarItem DeleteParameterIdentification(ParameterIdentification parameterIdentification)
      {
         return CreateMenuButton.WithCaption(MenuNames.Delete)
            .WithIcon(ApplicationIcons.Delete)
            .WithCommandFor<DeleteParameterIdentificationUICommand, ParameterIdentification>(parameterIdentification);
      }

      public static IMenuBarItem CreateParameterIdentification()
      {
         return MenuBarItemExtensions.WithDescription(CreateMenuButton.WithCaption(MenuNames.AddParameterIdentification), MenuDescriptions.CreateParameterIdentification)
            .WithIcon(ApplicationIcons.ParameterIdentification)
            .WithCommand<CreateParameterIdentificationUICommand>();
      }

      public static IMenuBarItem CreateParameterIdentificationFor(IEnumerable<ISimulation> simulations)
      {
         return CreateMenuButton.WithCaption(MenuNames.StartParameterIdentification)
            .WithIcon(ApplicationIcons.ParameterIdentification)
            .WithCommandFor<CreateParameterIdentificationBasedOnSimulationsUICommand, IEnumerable<ISimulation>>(simulations);
      }

      public static IMenuBarItem EditParameterIdentification(ParameterIdentification parameterIdentification)
      {
         return CreateMenuButton.WithCaption(MenuNames.Edit)
            .WithIcon(ApplicationIcons.Edit)
            .WithCommandFor<EditParameterIdentificationUICommand, ParameterIdentification>(parameterIdentification);
      }

      public static IMenuBarItem RenameParameterIdentification(ParameterIdentification simulation)
      {
         return CreateMenuButton.WithCaption(MenuNames.Rename)
            .WithCommandFor<RenameParameterIdentificationUICommand, ParameterIdentification>(simulation)
            .WithIcon(ApplicationIcons.Rename);
      }

      public static IMenuBarButton ExportParameterIdentificationToR(ParameterIdentification parameterIdentification)
      {
         return CreateMenuButton.WithCaption(MenuNames.ExportForR.WithEllipsis())
            .WithIcon(ApplicationIcons.R)
            .WithCommandFor<ExportParameterIdentificationToRUICommand, ParameterIdentification>(parameterIdentification);
      }

      public static IMenuBarButton ExportParameterIdentificationToMatlab(ParameterIdentification parameterIdentification)
      {
         return CreateMenuButton.WithCaption(MenuNames.ExportForMatlab.WithEllipsis())
            .WithIcon(ApplicationIcons.Matlab)
            .WithCommandFor<ExportParameterIdentificationToMatlabUICommand, ParameterIdentification>(parameterIdentification);
      }

      public static IMenuBarButton CloneParameterIdentification(ParameterIdentification parameterIdentification)
      {
         return CreateMenuButton.WithCaption(MenuNames.Clone)
            .WithCommandFor<CloneParameterIdentificationUICommand, ParameterIdentification>(parameterIdentification)
            .WithIcon(ApplicationIcons.Clone);
      }

      public static IMenuBarButton AddParameterIdentificationToJournal(ParameterIdentification parameterIdentification)
      {
         return CreateMenuButton.WithCaption(Captions.Journal.AddToJournal)
            .WithCommandFor<AddParameterAnalysableToActiveJournalPageUICommand, IParameterAnalysable>(parameterIdentification)
            .WithIcon(ApplicationIcons.AddToJournal);
      }

      public static IEnumerable<IMenuBarItem> ContextMenuItemsFor(ParameterIdentification parameterIdentification)
      {
         yield return EditParameterIdentification(parameterIdentification);

         yield return RenameParameterIdentification(parameterIdentification);

         yield return CloneParameterIdentification(parameterIdentification)
            .AsGroupStarter();
         
         yield return AddParameterIdentificationToJournal(parameterIdentification);

         yield return ExportParameterIdentificationToMatlab(parameterIdentification);

         yield return DeleteParameterIdentification(parameterIdentification)
          .AsGroupStarter();
      }
   }
}