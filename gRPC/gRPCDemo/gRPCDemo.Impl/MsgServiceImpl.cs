using Grpc.Core;
using GRPCDemo.Protocol;
using System.Threading.Tasks;

namespace gRPCDemo.Impl
{
    public class MsgServiceImpl : MsgService.MsgServiceBase
    {
        public MsgServiceImpl()
        {
        }

        public override async Task<GetMsgSumReply> GetSum(GetMsgNumRequest request, ServerCallContext context)
        {
            var result = new GetMsgSumReply();

            result.Sum = request.Num1 + request.Num2;

            return result;
        }
    }
}