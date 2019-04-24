using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Services;
using OSPSuite.TeXReporting.Builder;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Container.Conventions;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Infrastructure.Reporting
{
   public class ReportingRegister : Register
   {
      public override void RegisterInContainer(IContainer container)
      {
         container.AddScanner(scan =>
         {
            scan.AssemblyContainingType<ReportingRegister>();
            scan.IncludeNamespaceContainingType<ReportingRegister>();
            scan.WithConvention<ReporterRegistrationConvention>();
         });

         container.Register<IOSPSuiteTeXReporterRepository, OSPSuiteTeXReporterRepository>(LifeStyle.Singleton);
         container.Register<IReportingTask, ReportingTask>();
      }
   }

   /// <summary>
   ///    This registration will only apply for component implementing either  <c>IOSPSuiteTEXReporter</c> or <c>ITexBuilder</c>
   ///    or both.  It registers a component matching this condition with all implemented interfaces derived from either
   ///    <c>IOSPSuiteTEXReporter</c> or <c>ITexBuilder</c>.
   ///    The component will also be registered as itself, using the concreteType for resolution.
   /// </summary>
   public class ReporterRegistrationConvention : IRegistrationConvention
   {
      public void Process(Type concreteType, IContainer container, LifeStyle lifeStyle)
      {
         if (!isReporterType(concreteType))
            return;

         var services = new List<Type> { concreteType };
         var allInterfaces = concreteType.GetInterfaces().Where(x => x.IsInterface).ToList();

         var texReporterTypes = allInterfaces.Where(x => x.IsAnImplementationOf<IOSPSuiteTeXReporter>());
         services.AddRange(texReporterTypes);

         var texBuilderTypes = allInterfaces.Where(x => x.IsAnImplementationOf<ITeXBuilder>());
         services.AddRange(texBuilderTypes);

         container.Register(services, concreteType, lifeStyle);
      }

      private static bool isReporterType(Type concreteType)
      {
         return concreteType.IsAnImplementationOf<IOSPSuiteTeXReporter>() ||
                concreteType.IsAnImplementationOf<ITeXBuilder>();
      }
   }
}