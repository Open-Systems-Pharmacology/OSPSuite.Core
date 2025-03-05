using System.Threading.Tasks;
using OSPSuite.Presentation.MenuAndBars;

namespace OSPSuite.Presentation.UICommands
{
   public interface IObjectUICommand<T> : IUICommand, IUICommandAsync where T : class
   {
      IObjectUICommand<T> For(T objectForCommand);
      T Subject { get; set; }
   }

   public abstract class ObjectUICommand<T> : IObjectUICommand<T> where T : class
   {
      public virtual T Subject { get; set; }
      
      public virtual void Execute()
      {
         PerformExecute();
         Subject = null;
      }

      public virtual async Task ExecuteAsync()
      {
         await PerformExecuteAsync();
         Subject = null;
      }

      protected virtual Task PerformExecuteAsync()
      {
         PerformExecute();
         return Task.CompletedTask;
      }

      protected abstract void PerformExecute();

      public virtual IObjectUICommand<T> For(T objectForCommand)
      {
         Subject = objectForCommand;
         return this;
      }
   }
}