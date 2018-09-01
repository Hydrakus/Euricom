using System;
using System.Linq;
using Autofac;
using Autofac.Core;

namespace Euricom.Cruise2018.Demo.Infrastructure.Autofac
{
    public class TypeBasedParameterInjectionModule<TParameterType> : Module
    {
        private static Func<Type, TParameterType> _componentFactory;
        public TypeBasedParameterInjectionModule(Func<Type, TParameterType> componentFactory)
        {
            _componentFactory = componentFactory;
        }
        protected override void AttachToComponentRegistration(IComponentRegistry componentRegistry, IComponentRegistration registration)
        {
            registration.Preparing += OnComponentPreparing;
        }

        static void OnComponentPreparing(object sender, PreparingEventArgs e)
        {
            var t = e.Component.Activator.LimitType;
            e.Parameters = e.Parameters.Union(new[]
            {
                new ResolvedParameter((p, i) => p.ParameterType == typeof(TParameterType), (p, i) => _componentFactory(t))
            });
        }
    }
}
