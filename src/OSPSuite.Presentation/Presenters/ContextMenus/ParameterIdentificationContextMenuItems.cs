using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Extensions;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.UICommands;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace OSPSuite.Presentation.Presenters.ContextMenus
{
   public static class ParameterIdentificationContextMenuItems
   {
      public static IMenuBarItem DeleteParameterIdentification(ParameterIdentification parameterIdentification, IContainer container)
      {
         return CreateMenuButton.WithCaption(MenuNames.Delete)
            .WithIcon(ApplicationIcons.Delete)
            .WithCommandFor<DeleteParameterIdentificationUICommand, ParameterIdentification>(parameterIdentification, container);
      }

      public static IMenuBarItem CreateParameterIdentification(IContainer container)
      {
         return CreateMenuButton.WithCaption(MenuNames.AddParameterIdentification)
            .WithDescription(MenuDescriptions.CreateParameterIdentification)
            .WithIcon(ApplicationIcons.ParameterIdentification)
            .WithCommand<CreateParameterIdentificationUICommand>(container);
      }

      public static IMenuBarItem CreateParameterIdentificationFor(IEnumerable<ISimulation> simulations, IContainer container)
      {
         return CreateMenuButton.WithCaption(MenuNames.StartParameterIdentification)
            .WithIcon(ApplicationIcons.ParameterIdentification)
            .WithCommandFor<CreateParameterIdentificationBasedOnSimulationsUICommand, IEnumerable<ISimulation>>(simulations, container);
      }

      public static IMenuBarItem EditParameterIdentification(ParameterIdentification parameterIdentification, IContainer container)
      {
         return CreateMenuButton.WithCaption(MenuNames.Edit)
            .WithIcon(ApplicationIcons.Edit)
            .WithCommandFor<EditParameterIdentificationUICommand, ParameterIdentification>(parameterIdentification, container);
      }

      public static IMenuBarItem RenameParameterIdentification(ParameterIdentification simulation, IContainer container)
      {
         return CreateMenuButton.WithCaption(MenuNames.Rename)
            .WithCommandFor<RenameParameterIdentificationUICommand, ParameterIdentification>(simulation, container)
            .WithIcon(ApplicationIcons.Rename);
      }

      public static IMenuBarButton ExportParameterIdentificationToR(ParameterIdentification parameterIdentification, IContainer container)
      {
         return CreateMenuButton.WithCaption(MenuNames.ExportForR.WithEllipsis())
            .WithIcon(ApplicationIcons.R)
            .WithCommandFor<ExportParameterIdentificationToRUICommand, ParameterIdentification>(parameterIdentification, container);
      }

      public static IMenuBarButton ExportParameterIdentificationToMatlab(ParameterIdentification parameterIdentification, IContainer container)
      {
         return CreateMenuButton.WithCaption(MenuNames.ExportForMatlab.WithEllipsis())
            .WithIcon(ApplicationIcons.Matlab)
            .WithCommandFor<ExportParameterIdentificationToMatlabUICommand, ParameterIdentification>(parameterIdentification, container);
      }

      public static IMenuBarButton CloneParameterIdentification(ParameterIdentification parameterIdentification, IContainer container)
      {
         return CreateMenuButton.WithCaption(MenuNames.Clone)
            .WithCommandFor<CloneParameterIdentificationUICommand, ParameterIdentification>(parameterIdentification, container)
            .WithIcon(ApplicationIcons.Clone);
      }

      public static IMenuBarButton AddParameterIdentificationToJournal(ParameterIdentification parameterIdentification, IContainer container)
      {
         return CreateMenuButton.WithCaption(Captions.Journal.AddToJournal)
            .WithCommandFor<AddParameterAnalysableToActiveJournalPageUICommand, IParameterAnalysable>(parameterIdentification, container)
            .WithIcon(ApplicationIcons.AddToJournal);
      }

      public static IMenuBarItem RunParameterIdentification(ParameterIdentification parameterIdentification, IContainer container)
      {
         return CreateMenuButton.WithCaption(MenuNames.RunParameterIdentification)
            .WithIcon(ApplicationIcons.Run)
            .WithCommandFor<RunParameterIdentificationUICommand, ParameterIdentification>(parameterIdentification, container);
      }

      public static IEnumerable<IMenuBarItem> ContextMenuItemsFor(ParameterIdentification parameterIdentification, IContainer container)
      {
         yield return EditParameterIdentification(parameterIdentification, container);
            

         yield return RunParameterIdentification(parameterIdentification, container)
            .AsGroupStarter(); 

         yield return RenameParameterIdentification(parameterIdentification, container);

         yield return CloneParameterIdentification(parameterIdentification, container)
            .AsGroupStarter();

         yield return AddParameterIdentificationToJournal(parameterIdentification, container);

         yield return ExportParameterIdentificationToMatlab(parameterIdentification, container)
            .ForDeveloper();

         yield return DeleteParameterIdentification(parameterIdentification, container)
            .AsGroupStarter();
      }
   }
}