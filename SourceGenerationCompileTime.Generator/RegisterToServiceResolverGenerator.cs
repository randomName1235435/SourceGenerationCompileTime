using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Diagnostics;

namespace SourceGenerationCompileTime.Generator
{
    [Generator]
    public class RegisterToServiceResolverGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            var syntaxTrees = context.Compilation.SyntaxTrees;
            var handlers = syntaxTrees.Where(x => x.GetText().ToString().Contains("[RegisterToServiceResolver"));

            var usingDirectiveList = new List<UsingDirectiveSyntax>();
            var registrationTextList = new List<string>();

            foreach (var handler in handlers)
            {
                var usingDirectives = handler.GetRoot().DescendantNodes().OfType<UsingDirectiveSyntax>();
                var classDeclarationSyntax = handler.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>().First();

                var typeParameterFromAttribute = classDeclarationSyntax.AttributeLists.First().Attributes.First().ArgumentList.Arguments;

                var interfaceName = typeParameterFromAttribute.ToString().Replace("typeof(", "").Replace(")", "");

                var className = classDeclarationSyntax.Identifier.ToString();

                usingDirectiveList.AddRange(usingDirectives);
                registrationTextList.Add($"instance.Register<{interfaceName}>(new {className}());");
            }

            var sourceBuilder = new StringBuilder(string.Join("\r\n", usingDirectiveList));

            sourceBuilder.Append($@"
using SourceGenerationCompileTime;

namespace SourceGenerationCompileTime.Generator
{{
public class RegisterServiceGenerator
    {{
        public void ToServiceResolver() {{
   var instance = SampleServiceResolver.GetInstance();
   ");

            sourceBuilder.Append(string.Join("\r\n", usingDirectiveList.Distinct()));
            sourceBuilder.Append(string.Join("\r\n", registrationTextList.Distinct()));
            sourceBuilder.Append(@"}}}");
            context.AddSource("RegisterService", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
            
            Debug.Write(sourceBuilder.ToString());
        }
        public void Initialize(GeneratorInitializationContext context)
        {
//#if DEBUG
//            if (!Debugger.IsAttached)
//            {
//                Debugger.Launch();
//            }
//#endif
        }
    }
    //public class RegisterService
    //{
    //    public void ToServiceResolver()
    //    {
    //        var instance = SampleServiceResolver.GetInstance();
    //        instance.Register<ISampleService>(new SampleService());
    //    }
    //}
}

