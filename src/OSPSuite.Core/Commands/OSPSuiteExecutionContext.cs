using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Commands
{
   public interface IOSPSuiteExecutionContext 
   {
      string TypeFor<T>(T obj) where T : class;
      void Register(IWithId objectToRegister);
      void Unregister(IWithId objectToUnregister);
      T Resolve<T>();
      void PublishEvent<T>(T eventToPublish);

      T Get<T>(string id) where T : class, IWithId;
      IWithId Get(string id);

      byte[] Serialize<TObject>(TObject objectToSerialize);
      TObject Deserialize<TObject>(byte[] serializationByte);

      void AddToHistory(ICommand command);
      void ProjectChanged();
      IProject Project { get; }

      T Clone<T>(T objectToClone) where T : class, IObjectBase;

      /// <summary>
      /// Loads the object <paramref name="objectBase"/> if the object is lazy loaded. 
      /// </summary>
      void Load(IObjectBase objectBase);
   }

   public interface IOSPSuiteExecutionContext<out TProject> :IOSPSuiteExecutionContext where TProject:IProject
   {
      TProject CurrentProject { get; }
   }
}