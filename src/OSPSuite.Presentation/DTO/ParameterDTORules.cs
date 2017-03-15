using System;
using System.Linq.Expressions;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Validation;
using OSPSuite.Core.Domain;

namespace OSPSuite.Presentation.DTO
{
   public class ParameterDTORules
   {
      private static IBusinessRuleSet rulesFor<TProperty>(IParameterDTO parameterDTO, TProperty displayValue)
      {
         var parameter = parameterDTO.Parameter;
         if (parameter == null)
            return new BusinessRuleSet();

         return parameter.Validate(x => x.Value, parameter.ConvertToBaseUnit(displayValue.ConvertedTo<double>()));
      }

      public static IBusinessRule ParameterIsValid()
      {
         return CreateRule.For<IParameterDTO>()
            .Property(p => p.Value)
            .WithRule((p, value) => rulesFor(p, value).IsEmpty)
            .WithError((p, value) => rulesFor(p, value).Message);
      }

      public static IBusinessRule ParameterIsValid<TParameterContainer, TProperty>(Expression<Func<TParameterContainer, TProperty>> parameterValue , Func<TParameterContainer, IParameterDTO> parmeterDelegate)
      {
         return CreateRule.For<TParameterContainer>()
            .Property(parameterValue)
            .WithRule((parameterContainer, value) => rulesFor(parmeterDelegate(parameterContainer), value).IsEmpty)
            .WithError((parameterContainer, value) => rulesFor(parmeterDelegate(parameterContainer), value).Message);
      }
   }
}