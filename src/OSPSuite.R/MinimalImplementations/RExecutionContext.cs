using System;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace OSPSuite.R.MinimalImplementations
{
   public class RExecutionContext : IOSPSuiteExecutionContext
   {
      public string TypeFor<T>(T obj) where T : class => obj?.GetType().Name;

      public void Register(IWithId objectToRegister)
      {
         //nothing to do
      }

      public void Unregister(IWithId objectToUnregister)
      {
         //nothing to do
      }

      public T Resolve<T>()
      {
         throw new NotImplementedException();
      }

      public void PublishEvent<T>(T eventToPublish)
      {
         //nothing to do
      }

      public T Get<T>(string id) where T : class, IWithId
      {
         throw new NotImplementedException();
      }

      public IWithId Get(string id)
      {
         throw new NotImplementedException();
      }

      public byte[] Serialize<TObject>(TObject objectToSerialize)
      {
         throw new NotImplementedException();
      }

      public TObject Deserialize<TObject>(byte[] serializationByte)
      {
         throw new NotImplementedException();
      }

      public void AddToHistory(ICommand command)
      {
         //nothing to do
      }

      public void ProjectChanged()
      {
         //nothing to do
      }

      public IProject Project { get; } = new RProject();

      public T Clone<T>(T objectToClone) where T : class, IObjectBase
      {
         throw new NotImplementedException();
      }

      public void Load(IObjectBase objectBase)
      {
         //nothing to do
      }

      public IContainer Container => throw new NotImplementedException();
   }
}