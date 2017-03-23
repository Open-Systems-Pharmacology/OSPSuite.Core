﻿using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Journal;
using OSPSuite.Helpers;
using OSPSuite.Infrastructure.Journal.Commands;
using OSPSuite.Infrastructure.Journal.Queries;

namespace OSPSuite.Infrastructure
{
   public class When_retrieving_all_journal_diagram_defined_in_the_database : ContextForJournalDatabase<AllJournalDiagramsQuery>
   {
      [Observation]
      public void should_return_all_available_journal_diagrams()
      {
         var diagram = new JournalDiagram { Name = "TOTO", DiagramModel = new DiagramModelForSpecs() };
         _databaseMediator.ExecuteCommand(new CreateJournalDiagram { Diagram = diagram });

         var allWorkingJournalDiagrams = _databaseMediator.ExecuteQuery(new AllJournalDiagrams());
         var diagramFromDb = allWorkingJournalDiagrams.ElementAt(0);
         diagramFromDb.Name.ShouldBeEqualTo("TOTO");
         diagramFromDb.DiagramModel.ShouldNotBeNull();
      }
   }
}