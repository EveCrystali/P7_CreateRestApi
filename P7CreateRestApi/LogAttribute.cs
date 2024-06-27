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
            var request = args.Arguments[0] as HttpRequest;
            if (request != null)
            {
                Console.WriteLine($"POSTSHARP: API call to {request.Path} with method {request.Method}");
            }
        }

        public override void OnExit(MethodExecutionArgs args)
        {
            Console.WriteLine("POSTSHARP: API call completed");
        }
    }
}