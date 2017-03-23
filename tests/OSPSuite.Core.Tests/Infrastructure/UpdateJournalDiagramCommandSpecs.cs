﻿using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Journal;
using OSPSuite.Helpers;
using OSPSuite.Infrastructure.Journal.Commands;
using OSPSuite.Infrastructure.Journal.Queries;

namespace OSPSuite.Infrastructure
{
   public class When_upadting_an_existing_journal_in_the_database : ContextForJournalDatabase<UpdateJournalDiagramCommand>
   {
      [Observation]
      public void should_return_all_available_journal_diagrams()
      {
         var diagram = new JournalDiagram {Name = "TOTO", DiagramModel = new DiagramModelForSpecs()};
         _databaseMediator.ExecuteCommand(new CreateJournalDiagram {Diagram = diagram});

         diagram.Name = "TATA";
         _databaseMediator.ExecuteCommand(new UpdateJournalDiagram {Diagram = diagram});

         var allWorkingJournalDiagrams = _databaseMediator.ExecuteQuery(new AllJournalDiagrams());
         var diagramFromDb = allWorkingJournalDiagrams.ElementAt(0);
         diagramFromDb.Name.ShouldBeEqualTo("TATA");
         diagramFromDb.DiagramModel.ShouldNotBeNull();
      }
   }
}