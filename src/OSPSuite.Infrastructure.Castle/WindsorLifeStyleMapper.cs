using System;
using Castle.Core;
using OSPSuite.Utility;
using OSPSuite.Utility.Container;

namespace OSPSuite.Infrastructure.Container.Castle
{
    public interface IWindsorLifeStyleMapper : IMapper<LifeStyle, LifestyleType> 
    {
        
    }

    public class WindsorLifeStyleMapper : IWindsorLifeStyleMapper
    {
        public LifestyleType MapFrom(LifeStyle input)
        {
            switch (input)
            {
                case LifeStyle.Singleton:
                    return LifestyleType.Singleton;
                case LifeStyle.Transient:
                    return LifestyleType.Transient;
                default:
                    throw new ArgumentOutOfRangeException("input");
            }
        }
    }
}