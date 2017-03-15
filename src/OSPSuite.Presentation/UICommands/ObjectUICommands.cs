using OSPSuite.Presentation.MenuAndBars;

namespace OSPSuite.Presentation.UICommands
{
   public interface IObjectUICommand<T> : IUICommand where T : class
   {
      IObjectUICommand<T> For(T objectForCommand);
      T Subject { get; set; }
   }

   public abstract class ObjectUICommand<T> : IObjectUICommand<T> where T : class
   {
      public virtual T Subject { get; set; }
      
      public void Execute()
      {
         PerformExecute();
         Subject = null;
      }

      protected abstract void PerformExecute();

      public virtual IObjectUICommand<T> For(T objectForCommand)
      {
         Subject = objectForCommand;
         return this;
      }
   }
}