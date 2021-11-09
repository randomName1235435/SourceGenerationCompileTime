using System;

namespace SourceGenerationCompileTime
{
    [AttributeUsage(AttributeTargets.Class)]
    internal class RegisterToServiceResolverAttribute : Attribute
    {
        public RegisterToServiceResolverAttribute(Type type)
        {

        }
    }
}