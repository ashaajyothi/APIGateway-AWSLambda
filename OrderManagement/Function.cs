using System.Text.Json;
using System.Text.Json.Nodes;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace OrderManagement;

public class Function
{

    /// <summary>
    /// A simple function that takes a string and does a ToUpper
    /// </summary>
    /// <param name="request">The event for the Lambda function handler to process.</param>
    /// <param name="context">The ILambdaContext that provides methods for logging and describing the Lambda environment.</param>
    /// <returns></returns>
    public async Task<APIGatewayHttpApiV2ProxyResponse> FunctionHandler(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
    {
        AmazonDynamoDBClient client = new AmazonDynamoDBClient();
        DynamoDBContext contextDb = new DynamoDBContext(client);    
        if(request != null && request.RouteKey != null)
        {
            if(request.RouteKey.Contains("GET"))
            {
                var data = await contextDb.ScanAsync<Order>(default).GetRemainingAsync();
                return new APIGatewayHttpApiV2ProxyResponse
                {
                    StatusCode = 200,
                    Body = JsonSerializer.Serialize(data)
                };
            }
            else if (request.RouteKey.Contains("POST") && request.Body != null)
            {
                var newOrder = JsonSerializer.Deserialize<Order>(request.Body);
                await contextDb.SaveAsync(newOrder);
                return new APIGatewayHttpApiV2ProxyResponse
                {
                    StatusCode = 200,
                    Body = JsonSerializer.Serialize(newOrder)
                };
            }
        }
        return new APIGatewayHttpApiV2ProxyResponse
        {
            StatusCode = 400,
            Body = "Invalid request"
        };
    }
}
