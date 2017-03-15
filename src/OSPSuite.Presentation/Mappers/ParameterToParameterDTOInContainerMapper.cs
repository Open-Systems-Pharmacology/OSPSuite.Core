using System;
using System.Linq.Expressions;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.DTO;

namespace OSPSuite.Presentation.Mappers
{
   public interface IParameterToParameterDTOInContainerMapper<TParameterContainerDTO> where TParameterContainerDTO : IValidatableDTO
   {
      /// <summary>
      ///    Map a parameter to a parameter dto within a container that should support error notification.
      ///    The idea is that the rules defined in the parameter (domain) should be available in the DTO
      /// </summary>
      /// <typeparam name="TPropertyType">Type of property returned by the parameter (e.g. double)</typeparam>
      /// <param name="parameter">Parameter that should be mapped in a parameter dto</param>
      /// <param name="parameterContainerDTO">DTO object containing a parameter dto</param>
      /// <param name="parameterValue">
      ///    Expression representing the function in the dto that returns the value of the parameter. This will be used
      ///    to propagate the notify property changed event handler
      /// </param>
      /// <param name="parameterDelegate">Function accessing the actuel parameter DTO </param>
      /// <returns></returns>
      IParameterDTO MapFrom<TPropertyType>(IParameter parameter, TParameterContainerDTO parameterContainerDTO,
         Expression<Func<TParameterContainerDTO, TPropertyType>> parameterValue,
         Func<TParameterContainerDTO, IParameterDTO> parameterDelegate);
   }

   public class ParameterToParameterDTOInContainerMapper<TParameterContainerDTO> : IParameterToParameterDTOInContainerMapper<TParameterContainerDTO> where TParameterContainerDTO : IValidatableDTO
   {
      private readonly IParameterToParameterDTOMapper _parameterDTOMapper;

      public ParameterToParameterDTOInContainerMapper(IParameterToParameterDTOMapper parameterDTOMapper)
      {
         _parameterDTOMapper = parameterDTOMapper;
      }

      public IParameterDTO MapFrom<TPropertyType>(IParameter parameter, TParameterContainerDTO parameterContainerDTO,
         Expression<Func<TParameterContainerDTO, TPropertyType>> parameterValue,
         Func<TParameterContainerDTO, IParameterDTO> parameterDelegate)

      {
         var parameterDTO = _parameterDTOMapper.MapFrom(parameter);
         parameterDTO.ValueChanged += (o, e) => parameterContainerDTO.AddNotifiableFor(parameterValue);
         parameterContainerDTO.Rules.Add(ParameterDTORules.ParameterIsValid(parameterValue, parameterDelegate));
         return parameterDTO;
      }
   }
}