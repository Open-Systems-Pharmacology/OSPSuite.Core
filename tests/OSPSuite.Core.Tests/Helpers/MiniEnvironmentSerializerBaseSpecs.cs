using System;
using OSPSuite.Core.Domain;
using OSPSuite.Utility.Container;

namespace OSPSuite.Helpers
{
   public abstract class MiniEnvironmentSerializerBaseSpecs : ModellingXmlSerializerBaseSpecs
   {
      protected internal IObjectPathFactory ObjectPathFactory { get; private set; }

      protected internal Parameter P { get; private set; }

      protected internal Parameter P0 { get; private set; }

      protected internal Parameter P1 { get; private set; }

      protected internal Parameter P2 { get; private set; }

      protected internal Container C0 { get; private set; }

      protected internal Container C1 { get; private set; }

      protected internal Container C2 { get; private set; }

      protected override void Context()
      {
         base.Context();
         ObjectPathFactory = IoC.Resolve<IObjectPathFactory>();

         //create Formula environment
         C0 = CreateObject<Container>().WithName("Conrad").WithMode(ContainerMode.Physical);
         P0 = CreateObject<Parameter>().WithParentContainer(C0).WithName("Paul").WithDimension(DimensionLength);
         C1 = CreateObject<Container>().WithParentContainer(C0).WithName("Carolin").WithMode(ContainerMode.Logical);
         P = CreateObject<Parameter>().WithParentContainer(C1).WithName("Pascal").WithDimension(DimensionLength);
         P1 = CreateObject<Parameter>().WithParentContainer(C1).WithName("Peter").WithDimension(DimensionLength);
         C2 = CreateObject<Container>().WithParentContainer(C1).WithName("Cleo").WithMode(ContainerMode.Physical);
         P2 = CreateObject<Parameter>().WithParentContainer(C2).WithName("Pit");
      }


      public override void Cleanup()
      {
         base.Cleanup();
         GC.Collect();

         P0 = null;
         P1 = null;
         P = null;
         P2 = null;
         C2 = null;
         C1 = null;
         C0 = null;
         ObjectPathFactory = null;

         GC.Collect();
      }
   }
}