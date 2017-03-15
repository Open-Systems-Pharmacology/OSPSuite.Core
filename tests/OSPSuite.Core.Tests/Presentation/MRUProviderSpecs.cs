using System;
using System.Collections.Generic;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility;
using FakeItEasy;
using OSPSuite.Presentation.Services;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_MRUProvider : ContextSpecification<IMRUProvider>
   {
      protected string _registryKeyName;
      protected string _pathThatExists;
      protected string _pathThatDoesNotExist;
      protected IPresentationUserSettings _userSettings;
      private Func<string, bool> _fileExists;
      protected string _newlyAddedFile;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _fileExists = FileHelper.FileExists;
         _pathThatExists = "c:\\toto.txt";
         _pathThatDoesNotExist = "c:\\tata.txt";
         _newlyAddedFile = "C:\\tralala.txt";
         FileHelper.FileExists = s =>
         {
            return string.Equals(s, _pathThatExists) || string.Equals(s, _newlyAddedFile);
         };
      }

      protected override void Context()
      {
         _userSettings = A.Fake<IPresentationUserSettings>();

         _userSettings.ProjectFiles = new List<string> { _pathThatExists, _pathThatDoesNotExist };
         sut = new MRUProvider(_userSettings);
      }

      public override void GlobalCleanup()
      {
         base.GlobalCleanup();
         FileHelper.FileExists = _fileExists;
      }
   }

   public class When_retrieving_the_list_of_all_available_project_files : concern_for_MRUProvider
   {
      protected override void Context()
      {
         base.Context();
         _userSettings.MRUListItemCount = 2;
         sut.Add(_pathThatExists);
         sut.Add(_newlyAddedFile);
      }

      [Observation]
      public void should_return_up_the_the_expected_number_of_files()
      {
         sut.All().ShouldOnlyContain(_pathThatExists, _newlyAddedFile);
      }
   }

   public class When_retrieving_the_list_of_all_available_project_files_and_a_file_does_not_exist : concern_for_MRUProvider
   {
      protected override void Context()
      {
         base.Context();
         _userSettings.MRUListItemCount = 2;
         sut.Add(_pathThatExists);
         sut.Add(_pathThatDoesNotExist);
         sut.Add(_newlyAddedFile);
      }

      [Observation]
      public void should_return_up_the_the_expected_number_of_files()
      {
         sut.All().ShouldOnlyContain(_pathThatExists, _newlyAddedFile);
      }
   }


   public class When_retrieving_the_list_of_all_available_project_files_and_the_file_list_count_is_inferior_to_the_number_of_availalbe_items: concern_for_MRUProvider
   {
      protected override void Context()
      {
         base.Context();
         _userSettings.MRUListItemCount = 1;
         sut.Add(_pathThatExists);
         sut.Add(_pathThatDoesNotExist);
         sut.Add(_newlyAddedFile);
      }

      [Observation]
      public void should_return_up_the_the_expected_number_of_files()
      {
         sut.All().ShouldOnlyContain(_newlyAddedFile);
      }
   }

   public class When_retrieving_the_list_of_all_available_project_files_and_the_file_list_count_is_zero : concern_for_MRUProvider
   {
      protected override void Context()
      {
         base.Context();
         _userSettings.MRUListItemCount = 0;
         sut.Add(_pathThatExists);
         sut.Add(_pathThatDoesNotExist);
         sut.Add(_newlyAddedFile);
      }

      [Observation]
      public void should_return_up_the_the_expected_number_of_files()
      {
         sut.All().ShouldBeEmpty();
      }
   }
}