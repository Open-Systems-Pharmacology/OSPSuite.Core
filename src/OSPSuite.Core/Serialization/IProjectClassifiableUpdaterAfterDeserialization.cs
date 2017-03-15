using System.Linq;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Serialization
{
   public interface IProjectClassifiableUpdaterAfterDeserialization
   {
      /// <summary>
      ///    Should be called after the deserialization process is over to update the reference of all
      ///    <see cref="IClassifiableWrapper" /> to
      ///    real objects defined in the <paramref name="project" />
      /// </summary>
      void Update(IProject project);
   }

   public abstract class ProjectClassifiableUpdaterAfterDeserializationBase : IProjectClassifiableUpdaterAfterDeserialization
   {
      public void Update(IProject project)
      {
         if (project == null) return;

         project.AllClassifiablesByType<IClassifiableWrapper>().ToList().Each(x =>
         {
            var subject = RetrieveSubjectFor(x, project);
            if (subject == null)
               project.RemoveClassifiable(x);
            else
               x.UpdateSubject(subject);
         });
      }

      protected abstract IWithId RetrieveSubjectFor(IClassifiableWrapper classifiableWrapper, IProject project);
   }
}