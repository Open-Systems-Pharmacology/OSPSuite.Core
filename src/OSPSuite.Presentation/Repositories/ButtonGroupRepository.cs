using System.Collections.Generic;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;

namespace OSPSuite.Presentation.Repositories
{
   public interface IButtonGroupRepository : IRepository<IButtonGroup>
   {
      IButtonGroup Find(ButtonGroupId buttonGroupIds);
      IButtonGroup this[ButtonGroupId buttonGroupIds] { get; }
   }

   public abstract class ButtonGroupRepository : IButtonGroupRepository
   {
      protected readonly IMenuBarItemRepository _menuBarItemRepository;
      private readonly ICache<string, IButtonGroup> _toolBars = new Cache<string, IButtonGroup>(tb => tb.Id);

      protected ButtonGroupRepository(IMenuBarItemRepository menuBarItemRepository)
      {
         _menuBarItemRepository = menuBarItemRepository;
         _toolBars.AddRange(AllButtonGroups());
      }

      protected abstract IEnumerable<IButtonGroup> AllButtonGroups();

      public IEnumerable<IButtonGroup> All()
      {
         return _toolBars.All();
      }

      public IButtonGroup Find(ButtonGroupId buttonGroupId)
      {
         return _toolBars[buttonGroupId.Id];
      }

      public IButtonGroup this[ButtonGroupId buttonGroupId]
      {
         get { return Find(buttonGroupId); }
      }
   }
}