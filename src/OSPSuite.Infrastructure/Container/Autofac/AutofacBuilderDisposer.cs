using System;
using Autofac;

namespace OSPSuite.Infrastructure.Container.Autofac
{
   public class AutofacBuilderDisposer : IDisposable
   {
      private readonly AutofacContainer _autofacContainer;

      public AutofacBuilderDisposer(AutofacContainer autofacContainer)
      {
         _autofacContainer = autofacContainer;
      }

      public void Dispose()
      {
         _autofacContainer.Build();
      }
   }
}