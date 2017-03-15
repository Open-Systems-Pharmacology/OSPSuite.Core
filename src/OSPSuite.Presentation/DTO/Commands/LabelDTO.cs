using OSPSuite.Utility.Validation;

namespace OSPSuite.Presentation.DTO.Commands
{
    public class LabelDTO : IValidatable
    {
        public string Label{get;set;}
        public string Comment{get;set;}

        public IBusinessRuleSet Rules
        {
            get { return AllRules.Default; }
        }

        private static class AllRules
        {
            public static IBusinessRule NameRequired
            {
                get
                {
                    return CreateRule.For <LabelDTO>()
                        .Property(item => item.Label)
                        .WithRule((labelDTO, label) => !string.IsNullOrEmpty(label))
                        .WithError("Label is required.");
                }
            }

            public static IBusinessRule CommentRequired
            {
                get
                {
                    return CreateRule.For<LabelDTO>()
                        .Property(item => item.Comment)
                        .WithRule((labelDTO, comment) => !string.IsNullOrEmpty(comment))
                        .WithError("Comment is required.");
                }
            }

            public static IBusinessRuleSet Default
            {
                get { return new BusinessRuleSet(NameRequired, CommentRequired); }
            }
        }
    }
}