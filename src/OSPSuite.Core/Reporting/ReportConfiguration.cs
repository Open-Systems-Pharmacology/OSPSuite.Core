using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Utility;
using OSPSuite.Utility.Reflection;
using OSPSuite.Utility.Validation;

namespace OSPSuite.Core.Reporting
{
   public enum ReportColorStyles
   {
      Color,
      GrayScale,
      BlackAndWhite,
   }

   public enum ReportFont
   {
      Default,
      Helvetica,
      Optima,
      ComputerModernTeletype,
      Courier,
      Bookman,
      Inconsolata,
      LatinModern,
   }

   public class ReportConfiguration : Notifier,  IValidatable
   {
      public ReportTemplate Template { get; set; }
      private string _reportFile;
      public bool Verbose { get; set; }
      public bool OpenReportAfterCreation { get; set; }
      public IBusinessRuleSet Rules { get; }
      public string Author { get; set; }
      public string Title { get; set; }
      public string SubTitle { get; set; }
      public bool Draft { get; set; }
      public bool SaveArtifacts { get; set; }
      public bool DeleteWorkingDir { get; set; }
      public ReportFont Font { get; set; }
      public int NumberOfCompilations { get; set; }

      public ReportConfiguration()
      {
         Rules = new BusinessRuleSet(AllRules.All());
         Verbose = false;
         Author = EnvironmentHelper.UserName();
         Font = ReportFont.Default;
         OpenReportAfterCreation = true;
         NumberOfCompilations = 3;
      }

      public string ReportFile
      {
         get => _reportFile;
         set => SetProperty(ref _reportFile, value);
      }

      public ReportColorStyles ColorStyle { get; set; }

      private static class AllRules
      {
         private static IBusinessRule templateDefined { get; } = CreateRule.For<ReportConfiguration>()
            .Property(item => item.Template)
            .WithRule((dto, template) => dto.Template != null)
            .WithError(Validation.ValueIsRequired);

         public static IEnumerable<IBusinessRule> All()
         {
            yield return templateDefined;
            yield return nonEmpty(x => x.Author);
            yield return nonEmpty(x => x.Title);
            yield return nonEmpty(x => x.SubTitle);
         }

         private static IBusinessRule nonEmpty(Expression<Func<ReportConfiguration, string>> expression)
         {
            return GenericRules.NonEmptyRule(expression);
         }
      }
   }
}