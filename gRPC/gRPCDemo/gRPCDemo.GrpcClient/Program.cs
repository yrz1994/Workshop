using GRPCDemo.Protocol;
using System;

namespace gRPCDemo.GrpcClient
{
    class Program
    {
        static void Main(string[] args)
        {
            GetMsgSumReply msgSum = MsgServiceClient.GetSum(1, 2);
            HelloReply reply = HelloClient.SayHello("edward");
            Console.WriteLine("grpc Client Call GetSum(1, 2) : " + msgSum.Sum);
            Console.WriteLine(reply.Response);
            Console.WriteLine("任意键退出...");
            Console.ReadKey();
        }
    }
}
