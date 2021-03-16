using Grpc.Core;
using GRPCDemo.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace gRPCDemo.GrpcClient
{
    public static class HelloClient
    {
        private static Channel _channel;
        private static HelloService.HelloServiceClient _client;

        static HelloClient()
        {
            _channel = new Channel("127.0.0.1:40001", ChannelCredentials.Insecure);
            _client = new HelloService.HelloServiceClient(_channel);
        }

        public static HelloReply SayHello(string name)
        {
            return _client.SayHello(new HelloRequest
            {
                Name = name
            });
        }
    }
}
