using System.Collections.Generic;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain.Builder
{
   /// <summary>
   ///    Defines properties of an application
   /// </summary>
   public interface IApplicationBuilder : IEventGroupBuilder
   {
      /// <summary>
      ///    Name of the molecule which will be administered
      /// </summary>
      string MoleculeName { get; set; }

      /// <summary>
      ///    All transports whose source container is subcontainer of the application
      ///    <para></para>
      ///    Target container can also be subcontainer of the application,
      ///    <para></para>
      ///    but also of the spatial structure, where the application will be inserted
      /// </summary>
      IEnumerable<ITransportBuilder> Transports { get; }

      /// <summary>
      ///    Add new application transport
      /// </summary>
      /// <param name="appTransport"></param>
      void AddTransport(ITransportBuilder appTransport);

      /// <summary>
      ///    Remove application transport
      /// </summary>
      /// <param name="appTransport"></param>
      void RemoveTransport(ITransportBuilder appTransport);

      /// <summary>
      ///    Locations and start formulas of the molecule given by <see cref="MoleculeName" />
      /// </summary>
      IEnumerable<IApplicationMoleculeBuilder> Molecules { get; }

      /// <summary>
      ///    Add application molecule
      /// </summary>
      /// <param name="appMoleculeBuilder"></param>
      void AddMolecule(IApplicationMoleculeBuilder appMoleculeBuilder);

      /// <summary>
      ///    Remove application molecule
      /// </summary>
      /// <param name="appMoleculeBuilder"></param>
      void RemoveMolecule(IApplicationMoleculeBuilder appMoleculeBuilder);
   }

   public class ApplicationBuilder : EventGroupBuilder, IApplicationBuilder
   {
      public string MoleculeName { get; set; }

      public ApplicationBuilder()
      {
         MoleculeName = string.Empty;
         ContainerType = ContainerType.Application;
      }

      public IEnumerable<ITransportBuilder> Transports
      {
         get { return GetChildren<ITransportBuilder>(); }
      }

      public void AddTransport(ITransportBuilder appTransport)
      {
         Add(appTransport);
      }

      public void RemoveTransport(ITransportBuilder appTransport)
      {
         RemoveChild(appTransport);
      }

      public IEnumerable<IApplicationMoleculeBuilder> Molecules
      {
         get { return GetChildren<IApplicationMoleculeBuilder>(); }
      }

      public void AddMolecule(IApplicationMoleculeBuilder appMoleculeBuilder)
      {
         Add(appMoleculeBuilder);
      }

      public void RemoveMolecule(IApplicationMoleculeBuilder appMoleculeBuilder)
      {
        RemoveChild(appMoleculeBuilder);
      }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);

         var srcApplicationBuilder = source as IApplicationBuilder;
         if (srcApplicationBuilder == null) return;

         MoleculeName = srcApplicationBuilder.MoleculeName;
      }
   }
}