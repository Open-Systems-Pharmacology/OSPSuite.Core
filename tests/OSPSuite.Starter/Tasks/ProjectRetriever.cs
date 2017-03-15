using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Helpers;

namespace OSPSuite.Starter.Tasks
{
   internal class ProjectRetriever : IProjectRetriever
   {
      public ProjectRetriever()
      {
         CurrentProject = new TestProject();
      }

      public IProject CurrentProject { get; private set; }
      public string ProjectName { get; private set; }
      public string ProjectFullPath { get; private set; }
   }
}