using Grpc.Core;
using GRPCDemo.Protocol;
using System.Threading.Tasks;

namespace gRPCDemo.Impl
{
    public class HelloServiceImpl : HelloService.HelloServiceBase
    {
        public override async Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            var result = new HelloReply();

            result.Response = "Hi " + request.Name;

            return result;
        }
    }
}
