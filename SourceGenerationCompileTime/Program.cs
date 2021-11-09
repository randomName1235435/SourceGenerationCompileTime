using SourceGenerationCompileTime.Generator;

namespace SourceGenerationCompileTime
{
    public static class Program
    {
        public static void Main() {

            //startup
            new RegisterServiceGenerator().ToServiceResolver();

            var service = SampleServiceResolver.GetInstance().Resolve<ISampleService>();
        }
    }
}
