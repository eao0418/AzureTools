// Program.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.MCP.Client;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using ModelContextProtocol.Client;
using System;

internal class Program
{
    internal static async Task Main(string[] args)
    {
        var builder = Kernel.CreateBuilder();
        builder.Services.AddOpenAIChatCompletion(
            modelId: "llama3.2",
            apiKey: null, // No API key needed locally
            endpoint: new Uri("http://localhost:11434/v1") // Ollama server endpoint
        );
        var kernel = builder.Build();

        await using IMcpClient mcpClient = await McpClientFactory.CreateAsync(
            new SseClientTransport(
                new SseClientTransportOptions
                {
                    Endpoint = new Uri("http://localhost:7182/runtime/webhooks/mcp/sse")
                }
            ));

        // Retrieve and load tools from the server
        IList<McpClientTool> tools = await mcpClient.ListToolsAsync().ConfigureAwait(false);

#pragma warning disable SKEXP0001 // Suppress diagnostics for experimental features
        kernel.Plugins.AddFromFunctions("McpTools", tools.Select(t => t.AsKernelFunction()));
#pragma warning restore SKEXP0001

        Console.WriteLine("Start interacting with your AI agent. Type 'help' to get assistance with commands..");
        var history = new ChatHistory();
        history.AddSystemMessage("You are an AI agent that can call MCP tools to process user queries. If the request does not match a MCP tool, let the user know and answer to the best of your ability.");

        var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
        OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new()
        {
            ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
        };

        while (true)
        {
            Console.Write("Input > ");
            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Please enter a valid input.");
                continue;
            }

            // Handle special commands
            if (input.Equals("help", StringComparison.InvariantCultureIgnoreCase))
            {
                Console.WriteLine("Available commands:\n 'exit' to quit\n 'help' for this message\n 'clear' to clear the console\n 'reload' to reload MCP tools.");
                continue;
            }
            if (input.Equals("clear", StringComparison.InvariantCultureIgnoreCase))
            {
                Console.Clear();
                Console.WriteLine("Console cleared.");
                continue;
            }
            if (input.Equals("exit", StringComparison.InvariantCultureIgnoreCase))
            {
                Console.WriteLine("Exiting chat. Goodbye!");
                break;
            }
            if (input.Equals("list tools", StringComparison.InvariantCultureIgnoreCase))
            {
                Console.WriteLine("Listing MCP tools...");
                var toolRequest = await mcpClient.ListToolsAsync();
                if (toolRequest == null || toolRequest.Count == 0)
                {
                    Console.WriteLine("No tools available.");
                    continue;
                }
                Console.WriteLine("Available MCP Tools:");
                foreach (var tool in toolRequest)
                {
                    Console.WriteLine($"{tool.Name}: {tool.Description}");
                }
                continue;
            }

            history.AddUserMessage(input);

            // Get the response from the AI
            var result = await chatCompletionService.GetChatMessageContentAsync(
                history,
                executionSettings: openAIPromptExecutionSettings,
                kernel: kernel);

            Console.WriteLine($"Agent Response > {result.Content}");
            history.AddMessage(result.Role, result.Content ?? string.Empty);
        }
    }
}