using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using gRPCDemoServer;
using gRPCServer;
using static gRPCDemoServer.StockServer;

namespace gRPCServer.Services
{
    public class StockService : StockServerBase
    {
        public Dictionary<string,int> Source = new Dictionary<string, int>()
        {
            { "TCS",1200 },
            { "Infy",1500 },
            { "Wipro",1000 },
            { "NexTurn",800 }
        };

        public override Task<StockResult> GetValueByName(
            ValueByNameRequest request ,ServerCallContext context)
        {
            StockResult response = new StockResult
            {
                Value = Source[request.StockName],
                Name = request.StockName
            };
        return Task.FromResult(response);
        }

        public override Task<StockResponses> GetAllStockValues(
           Empty request, ServerCallContext context)
        {
            StockResponses response = new StockResponses();
            foreach (var key in Source)
            {
                response.Result.Add(new StockResult()
                {
                    Value = key.Value,
                    Name = key.Key
                });

            }
            return Task.FromResult(response);
        }

        public override async Task GetStockValue(
          ValueByNameRequest request,
          IServerStreamWriter<StockResult> responseStream,
          ServerCallContext context)
        {
            Random rnd = new Random();
            for (int i = 0; i <= 10; i++)
            {
                await responseStream.WriteAsync(
                    new StockResult
                    {
                        Value = Source[request.StockName] + rnd.Next(1, 25),
                        Name = request.StockName
                    }
                    );
                await Task.Delay(1000);
            }
        }
    }
}
