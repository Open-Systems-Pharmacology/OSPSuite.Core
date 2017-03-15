namespace OSPSuite.Core.Domain.Services
{
   public interface IProjectRetriever
   {
      IProject CurrentProject { get; }
      string ProjectName { get; }
      string ProjectFullPath { get; }
   }
}