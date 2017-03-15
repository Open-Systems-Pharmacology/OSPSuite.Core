using System;

namespace OSPSuite.Presentation.Core
{
   public interface ISubPresenterItem
   {
      int Index { get; set; }
      Type PresenterType { get; }
   }

   public interface ISubPresenterItem<out T> : ISubPresenterItem
   {
   }

   public class SubPresenterItem<TPresenter> : ISubPresenterItem<TPresenter>
   {
      public int Index { get; set; }

      public Type PresenterType => typeof(TPresenter);
   }
}