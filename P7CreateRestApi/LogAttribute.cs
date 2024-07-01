using PostSharp.Aspects;
using PostSharp.Serialization;

namespace Dot.Net.WebApi
{
    [PSerializable]
    public class LogAspect : OnMethodBoundaryAspect
    {
        public override void OnEntry(MethodExecutionArgs args)
        {
            Console.WriteLine($"POSTSHARP: Entering {args.Method.Name}");
        }

        public override void OnExit(MethodExecutionArgs args)
        {
            Console.WriteLine($"POSTSHARP: Exiting {args.Method.Name}");
        }

        public override void OnException(MethodExecutionArgs args)
        {
            Console.WriteLine($"POSTSHARP: Exception in {args.Method.Name}: {args.Exception.Message}");
        }
    }

    [PSerializable]
    public class LogApiCallAspect : OnMethodBoundaryAspect
    {
        public override void OnEntry(MethodExecutionArgs args)
        {
            var httpContext = (HttpContext)args.Instance.GetType()
                .GetProperty("HttpContext")
                ?.GetValue(args.Instance, null);

            if (httpContext != null)
            {
                var request = httpContext.Request;
                Console.WriteLine($"POSTSHARP: API call to {request.Path} with method {request.Method}");
            }
            else
            {
                Console.WriteLine("POSTSHARP: No HttpRequest argument found.");
            }
            Console.WriteLine($"POSTSHARP: Entering {args.Method.Name}");
        }


        public override void OnExit(MethodExecutionArgs args)
        {
            Console.WriteLine($"POSTSHARP: Exiting {args.Method.Name}");
        }

        public override void OnException(MethodExecutionArgs args)
        {
            Console.WriteLine($"POSTSHARP: Exception in {args.Method.Name}: {args.Exception.Message}");
        }
    }
}