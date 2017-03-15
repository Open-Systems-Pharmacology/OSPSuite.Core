using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Presentation.Presenters;

namespace OSPSuite.Presentation.Core
{
   public interface ISubPresenterManager : IReleasable, IEnumerable<IPresenter>
   {
      void InitializeWith(IPresenter parentPresenter);
      bool CanClose { get; }
      void Add(IPresenter presenter);
      IEnumerable<IPresenter> All { get; }
   }

   public interface ISubPresenterItemManager<out TSubPresenter> : ISubPresenterManager where TSubPresenter : ISubPresenter
   {
      IEnumerable<TSubPresenter> AllSubPresenters { get; }
      T PresenterAt<T>(ISubPresenterItem<T> subPresenterItem);
      void InitializeWith(IContainerPresenter containerPresenter, IReadOnlyList<ISubPresenterItem> subPresenterItems);
      IEventPublisher EventPublisher { get; }
      void Release();
      ISubPresenterItem SubPresenterItemFor(ISubPresenter subPresenter);
      TSubPresenter PresenterAtPosition(int position);
      ISubPresenterItem ItemAtPosition(int position);
   }

   public class SubPresenterManager : ISubPresenterManager
   {
      private readonly List<IPresenter> _allSubPresenters;
      private IPresenter _parentPresenter;

      public SubPresenterManager()
      {
         _allSubPresenters = new List<IPresenter>();
      }

      public virtual void InitializeWith(IPresenter parentPresenter)
      {
         _parentPresenter = parentPresenter;

         All.Each(initializeSubPresenter);

         var commandCollectorPresenter = parentPresenter as ICommandCollectorPresenter;
         var collectors = from presenter in All
            let collector = presenter as IInitializablePresenter<ICommandCollector>
            where collector != null
            select collector;

         if (commandCollectorPresenter != null)
            collectors.Each(p => p.InitializeWith(commandCollectorPresenter));
      }

      private void initializeSubPresenter(IPresenter subPresenter)
      {
         subPresenter.Initialize();
         subPresenter.StatusChanged += raiseViewChanged;
      }

      private void raiseViewChanged(object o, EventArgs e)
      {
         _parentPresenter.ViewChanged();
      }

      public bool CanClose
      {
         get { return (All.All(item => item.CanClose)); }
      }

      public virtual void Add(IPresenter presenter)
      {
         _allSubPresenters.Add(presenter);
      }

      public virtual IEnumerable<IPresenter> All => _allSubPresenters;

      public virtual void ReleaseFrom(IEventPublisher eventPublisher)
      {
         All.Each(x => x.ReleaseFrom(eventPublisher));

         var disposablePresenters = from presenter in All
            let containerPresenter = presenter as IDisposablePresenter
            where containerPresenter != null
            select containerPresenter;

         disposablePresenters.Each(x => x.Dispose());

         All.Each(p => p.StatusChanged -= raiseViewChanged);

         Clear();
      }

      protected virtual void Clear()
      {
         _allSubPresenters.Clear();
      }

      public IEnumerator<IPresenter> GetEnumerator()
      {
         return All.GetEnumerator();
      }

      IEnumerator IEnumerable.GetEnumerator()
      {
         return GetEnumerator();
      }
   }

   public class SubPresenterItemManager<TSubPresenter> : SubPresenterManager, ISubPresenterItemManager<TSubPresenter>
      where TSubPresenter : class, ISubPresenter
   {
      private readonly IContainer _container;
      private readonly IEventPublisher _eventPublisher;
      private readonly ICache<ISubPresenterItem, TSubPresenter> _allSubPresenters;

      public SubPresenterItemManager(IContainer container, IEventPublisher eventPublisher)
      {
         _container = container;
         _eventPublisher = eventPublisher;
         _allSubPresenters = new Cache<ISubPresenterItem, TSubPresenter>();
      }

      public void InitializeWith(IContainerPresenter containerPresenter, IReadOnlyList<ISubPresenterItem> subPresenterItems)
      {
         for (int i = 0; i < subPresenterItems.Count; i++)
         {
            var subPresenterItem = subPresenterItems[i];
            subPresenterItem.Index = i;
            var presenter = _container.Resolve<TSubPresenter>(subPresenterItem.PresenterType);
            _allSubPresenters.Add(subPresenterItem, presenter);
            containerPresenter.AddSubItemView(subPresenterItem, presenter.BaseView);
         }

         //needs to be done last so that the list of sub presenters is available
         base.InitializeWith(containerPresenter);
      }

      public TSubPresenter PresenterAtPosition(int position)
      {
         return _allSubPresenters[ItemAtPosition(position)];
      }

      public ISubPresenterItem ItemAtPosition(int position)
      {
         return _allSubPresenters.Keys.First(x => x.Index == position);
      }

      public IEventPublisher EventPublisher => _eventPublisher;

      public virtual void Release()
      {
         ReleaseFrom(_eventPublisher);
      }

      public ISubPresenterItem SubPresenterItemFor(ISubPresenter subPresenter)
      {
         return _allSubPresenters.KeyValues
            .Where(kv => Equals(kv.Value, subPresenter))
            .Select(kv => kv.Key).FirstOrDefault();
      }

      public IEnumerable<TSubPresenter> AllSubPresenters => _allSubPresenters;

      public override IEnumerable<IPresenter> All => _allSubPresenters.Cast<IPresenter>();

      public T PresenterAt<T>(ISubPresenterItem<T> subPresenterItem)
      {
         var subPresenter = _allSubPresenters[subPresenterItem];
         return subPresenter.DowncastTo<T>();
      }

      protected override void Clear()
      {
         _allSubPresenters.Clear();
         _allSubPresenters.Clear();
      }
   }
}