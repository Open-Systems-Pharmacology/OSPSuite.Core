using System.Collections.Generic;
using System.Linq;
using NHibernate;
using OSPSuite.Core.Events;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Infrastructure.ORM.MetaData
{
   public interface ICommandMetaDataRepository : 
      IRepository<CommandMetaData>,
      IListener<ProjectClosedEvent>
   {
      CommandMetaData FindById(string commandMetaDataId);
      IEnumerable<CommandMetaData> AllChildrenOf(CommandMetaData commandMetaData);
      void LoadFromSession(ISession session);
      void Save(CommandMetaData commandMetaData, ISession session);
      void Clear();
   }

   public class CommandMetaDataRepository : ICommandMetaDataRepository
   {
      private readonly Cache<string, CommandMetaData> _allCommandMetaData = new Cache<string, CommandMetaData>(x => x.Id);
      private readonly Cache<CommandMetaData, IEnumerable<CommandMetaData>> _allCommandMetaDataWithChildren = new Cache<CommandMetaData, IEnumerable<CommandMetaData>>(x => Enumerable.Empty<CommandMetaData>());

      public CommandMetaData FindById(string commandMetaDataId)
      {
         return _allCommandMetaData[commandMetaDataId];
      }

      public IEnumerable<CommandMetaData> AllChildrenOf(CommandMetaData commandMetaData)
      {
         return _allCommandMetaDataWithChildren[commandMetaData];
      }

      public void LoadFromSession(ISession session)
      {
         Clear();
         var allCommandMetaData = session.CreateCriteria<CommandMetaData>().List<CommandMetaData>();
         _allCommandMetaData.AddRange(allCommandMetaData);
         cacheParentChildrenRelationship();
      }

      private void cacheParentChildrenRelationship()
      {
         foreach (var commandMetaDataByParent in _allCommandMetaData.Where(x => x.Parent != null).GroupBy(x => x.Parent))
         {
            _allCommandMetaDataWithChildren.Add(commandMetaDataByParent.Key, commandMetaDataByParent.OrderBy(x => x.Sequence).ToList());
         }
      }

      public void Save(CommandMetaData commandMetaData, ISession session)
      {
         session.Save(commandMetaData);
         commandMetaData.Children.Each(x => Save(x, session));
      }

      public void Clear()
      {
         _allCommandMetaData.Clear();
         _allCommandMetaDataWithChildren.Clear();
      }

      public IEnumerable<CommandMetaData> All()
      {
         return _allCommandMetaData;
      }

      public void Handle(ProjectClosedEvent eventToHandle)
      {
         //Ensure that we clear the command cache when the project is closed
         Clear();
      }
   }
}