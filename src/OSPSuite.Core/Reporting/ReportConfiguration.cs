using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.TeXReporting;
using OSPSuite.Utility;
using OSPSuite.Utility.Validation;

namespace OSPSuite.Core.Reporting
{
   public class ReportConfiguration : ReportSettings, IValidatable
   {
      public ReportTemplate Template { get; set; }
      private string _reportFile;
      public bool Verbose { get; set; }
      public bool OpenReportAfterCreation { get; set; }
      public IBusinessRuleSet Rules { get; private set; }

      public ReportConfiguration()
      {
         Rules = new BusinessRuleSet(AllRules.All());
         Verbose = false;
         Author = EnvironmentHelper.UserName();
         Font = ReportFont.Default;
         OpenReportAfterCreation = true;
      }

      public string ReportFile
      {
         get { return _reportFile; }
         set
         {
            _reportFile = value;
            OnPropertyChanged(() => ReportFile);
         }
      }

      private static class AllRules
      {
         private static IBusinessRule templateDefined
         {
            get
            {
               return CreateRule.For<ReportConfiguration>()
                  .Property(item => item.Template)
                  .WithRule((dto, template) => dto.Template != null)
                  .WithError(Validation.ValueIsRequired);
            }
         }

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