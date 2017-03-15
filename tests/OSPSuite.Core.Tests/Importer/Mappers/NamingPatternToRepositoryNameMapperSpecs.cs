using System.Collections.Generic;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Importer;
using OSPSuite.Core.Importer.Mappers;

namespace OSPSuite.Importer.Mappers
{
   public abstract class concern_for_NamingPatternToRepositoryNameMapperSpecs : ContextSpecification<NamingPatternToRepositoryNameMapper>
   {
      protected IList<DataRepository> _repositories;
      protected DataImporterSettings _settings;
      protected override void Context()
      {
         _settings = new DataImporterSettings();
         var repository = new DataRepository { Name = "OldName" };
         repository.ExtendedProperties.Add(new ExtendedProperty<string> { Name = "Species", Value = "Pig" });
         repository.ExtendedProperties.Add(new ExtendedProperty<string> { Name = "Organ", Value = "Liver" });
         _repositories = new List<DataRepository> { repository };

         repository = new DataRepository { Name = "OldName" };
         repository.ExtendedProperties.Add(new ExtendedProperty<string> { Name = "DefaultConvention", Value = "DefaultName" });
         repository.ExtendedProperties.Add(new ExtendedProperty<string> { Name = "StudyID", Value = "99" });
         _repositories.Add(repository);

         repository = new DataRepository { Name = "OldName" };
         repository.ExtendedProperties.Add(new ExtendedProperty<string> { Name = "Species", Value = "Pig" });
         repository.ExtendedProperties.Add(new ExtendedProperty<string> { Name = "StudyID", Value = "99" });
         _repositories.Add(repository);

         repository = new DataRepository { Name = "OldName" };
         _repositories.Add(repository);
         sut = new NamingPatternToRepositoryNameMapper();

      }

      protected override void Because()
      {
         sut.RenameRepositories(_repositories.ToList(), "{Molecule}.{Species}.{Organ}", _settings);
      }
   }

   public class when_renaming_repositories_with_default_specified : concern_for_NamingPatternToRepositoryNameMapperSpecs
   {
      protected override void Context()
      {
         base.Context();
         _settings.AddNamingPatternMetaData("DefaultConvention");
      }
   }

   public class when_renaming_repositories : concern_for_NamingPatternToRepositoryNameMapperSpecs
   {
      [Observation]
      public void default_convention_applied_when_first_pattern_fails()
      {
         _repositories.Count(x => x.Name.Equals("DefaultName")).ShouldBeEqualTo(0);
      }

      [Observation]
      public void repository_with_item_not_in_template_is_removed()
      {
         _repositories.Count(repository => repository.Name.Equals("Pig")).ShouldBeEqualTo(1);
      }

      [Observation]
      public void repository_must_be_renamed_according_to_pattern()
      {
         _repositories.First().Name.ShouldBeEqualTo("Pig.Liver");
      }
   }
}
