using System.Collections.Generic;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain.Builder
{
   public class ApplicationBuilder : EventGroupBuilder
   {
      /// <summary>
      ///    Name of the molecule which will be administered
      /// </summary>
      public string MoleculeName { get; set; }

      public ApplicationBuilder()
      {
         MoleculeName = string.Empty;
         ContainerType = ContainerType.Application;
      }

      /// <summary>
      ///    All transports whose source container is subcontainer of the application
      ///    <para></para>
      ///    Target container can also be subcontainer of the application,
      ///    <para></para>
      ///    but also of the spatial structure, where the application will be inserted
      /// </summary>
      public IEnumerable<TransportBuilder> Transports => GetChildren<TransportBuilder>();

      /// <summary>
      ///    Add new application transport
      /// </summary>
      public void AddTransport(TransportBuilder appTransport) => Add(appTransport);

      /// <summary>
      ///    Remove application transport
      /// </summary>
      public void RemoveTransport(TransportBuilder appTransport) => RemoveChild(appTransport);

      /// <summary>
      ///    Locations and start formulas of the molecule given by <see cref="MoleculeName" />
      /// </summary>
      public IEnumerable<ApplicationMoleculeBuilder> Molecules => GetChildren<ApplicationMoleculeBuilder>();

      /// <summary>
      ///    Add application molecule
      /// </summary>
      /// <param name="appMoleculeBuilder"></param>
      public void AddMolecule(ApplicationMoleculeBuilder appMoleculeBuilder) => Add(appMoleculeBuilder);

      /// <summary>
      ///    Remove application molecule
      /// </summary>
      /// <param name="appMoleculeBuilder"></param>
      public void RemoveMolecule(ApplicationMoleculeBuilder appMoleculeBuilder) => RemoveChild(appMoleculeBuilder);

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);

         var srcApplicationBuilder = source as ApplicationBuilder;
         if (srcApplicationBuilder == null) return;

         MoleculeName = srcApplicationBuilder.MoleculeName;
      }
   }
}