#if BROWSER
// Minimal stubs: only the attributes and types referenced by benchmark source files
// when compiled for the browser target. The real BenchmarkDotNet is never referenced.

namespace BenchmarkDotNet.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class BenchmarkAttribute : Attribute
    {
        public bool Baseline { get; set; }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class BenchmarkCategoryAttribute : Attribute
    {
        public BenchmarkCategoryAttribute(string category) { }
        public string[] Categories { get; set; } = [];
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class GlobalSetupAttribute : Attribute { }
}
#endif
