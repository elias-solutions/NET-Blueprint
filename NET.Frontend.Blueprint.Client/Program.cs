using Microsoft.AspNetCore.SignalR.Client;

Console.WriteLine("Client online");

var connection = new HubConnectionBuilder()
    .WithUrl(new Uri("http://localhost:5105/StatusChangeHub"))
    .WithAutomaticReconnect()
    .Build();

await connection.StartAsync();
connection.On<Guid, string, string>("StatusChange", (id, key, value) 
    => Console.WriteLine($"Change: Entity '{key}' id '{id}' value '{value}'"));

Console.WriteLine("Press any key to shutdown");
Console.ReadLine();
