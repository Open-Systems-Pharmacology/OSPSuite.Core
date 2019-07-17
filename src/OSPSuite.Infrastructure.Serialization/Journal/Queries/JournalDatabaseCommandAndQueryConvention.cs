using System;
using System.Linq;
using OSPSuite.Infrastructure.Journal.Commands;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Container.Conventions;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Infrastructure.Journal.Queries
{
   public class JournalDatabaseCommandAndQueryConvention : IRegistrationConvention
   {
      public void Process(Type concreteType, IContainer container, LifeStyle lifeStyle)
      {
         var queryType = concreteType.GetDeclaredTypesForGeneric(typeof(IJournalDatabaseQuery<,>))
            .FirstOrDefault();

         if (queryType != null)
         {
            container.Register(queryType.GenericType, concreteType, lifeStyle, queryType.DeclaredType.Name);
            return;
         }

         var commandType = concreteType.GetDeclaredTypesForGeneric(typeof(IJournalDatabaseCommand<>))
            .FirstOrDefault();

         if (commandType != null)
         {
            container.Register(commandType.GenericType, concreteType, lifeStyle);
            return;
         }
      }
   }
}