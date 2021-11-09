using System;
using System.Collections.Generic;

namespace SourceGenerationCompileTime
{
    public class SampleServiceResolver
    {
        private static SampleServiceResolver singletonInstance;
        private static Action createInstance = () => { singletonInstance = new SampleServiceResolver(); };
        private Dictionary<Type, object> registrations = new Dictionary<Type, object>();

        private SampleServiceResolver() { }

        public static SampleServiceResolver GetInstance()
        {
            createInstance();
            createInstance = () => { };

            return singletonInstance;
        }

        public T Resolve<T>()
        {
            return (T)registrations[typeof(T)];
        }

        public void Register<T>(T instance)
        {
            this.registrations.Add(typeof(T), instance);
        }
    }
}
