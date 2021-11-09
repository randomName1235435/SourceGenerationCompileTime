namespace SourceGenerationCompileTime
{
    [RegisterToServiceResolver(typeof(ISampleService))]
    public class SampleService : ISampleService
    {
        public void Sample()
        {
            //sample
        }
    }
}
