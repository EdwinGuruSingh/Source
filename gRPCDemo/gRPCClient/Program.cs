// See https://aka.ms/new-console-template for more information
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using gRPCClient;
using gRPCDemoClient;


//create channel
GrpcChannel channel = GrpcChannel.ForAddress("https://localhost:7153");
//GrpcChannel channel = GrpcChannel.ForAddress("http://localhost:5153");

////create client for our service
//var client = new Greeter.GreeterClient(channel);

//HelloRequest request=new HelloRequest() { Name="Edwin" };
//HelloReply response=client.SayHello(request);

//create client for our service
var client = new StockServer.StockServerClient(channel);
/*First API*/

ValueByNameRequest request = new ValueByNameRequest() { StockName = "NexTurn" };
StockResult response = client.GetValueByName(request);

Console.WriteLine(String.Format("Stock {0} value is {1}", response.Name, response.Value));
/*Second API*/
var reply = client.GetAllStockValues(new Empty());
foreach (var strResult in reply.Result) {
    Console.WriteLine(String.Format("Stock {0} value is {1}", strResult.Name, strResult.Value));
}

/*Third API*/
ValueByNameRequest streamRequest = new ValueByNameRequest() { StockName = "Wipro" };
var response1 = client.GetStockValue(streamRequest);
while (await response1.ResponseStream.MoveNext())
{
    Console.WriteLine(String.Format("Stock {0} value is {1} on {2}",
        response1.ResponseStream.Current.Name,
        response1.ResponseStream.Current.Value,
        DateTime.Now.ToString()
        ));
}


Console.WriteLine("gRPC completed");
Console.ReadLine();