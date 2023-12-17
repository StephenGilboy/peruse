using System.Text.Json;
using Azure;
using Azure.AI.OpenAI;

namespace Peruse.Chat.ChatFeature;

/// <summary>
/// Manages the chat context for the user and the assistant.
/// A lot of work to do here to make this more robust, but for now
/// it proves the concept.
/// </summary>
public class ChatContext
{
    private OpenAIClient _openAIClient { get; set; }
    private IPeruseApi _peruseApi { get; set; }
    public List<ChatRequestMessage> ChatMessages { get; private set; } = new();
    readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
    readonly ChatCompletionsOptions _chatCompletionOptions = new()
    {
        DeploymentName = "gpt-3.5-turbo-1106",
        Tools = { ChatFunctions.OpenBrowser, ChatFunctions.NavigateToPage }
    };

    public ChatContext(OpenAIClient openAIClient, IPeruseApi peruseApi)
    {
        _openAIClient = openAIClient;
        _peruseApi = peruseApi;
        ChatMessages.Add(new ChatRequestSystemMessage("You are an AI assistant that can navigate to websites using the functions given to you. You will use the browser to complete any task you are asked to do."));
    }
    
    /// <summary>
    /// Sends the user's message to OpenAI
    /// </summary>
    /// <param name="userMessage">User's message</param>
    /// <returns>Assistant's Response</returns>
    public async Task<string> SendMessage(string userMessage)
    {
        if(string.IsNullOrEmpty(userMessage))
            return string.Empty;
        var chatRequest = new ChatRequestUserMessage(userMessage);
        ChatMessages.Add(chatRequest);
        _chatCompletionOptions.Messages.Add(chatRequest);
        Response<ChatCompletions> response = await _openAIClient.GetChatCompletionsAsync(_chatCompletionOptions);
        return await HandleAssistantResponse(response);
    }
    
    /// <summary>
    /// Handles the response from the assistant.
    /// </summary>
    /// <param name="response">ChatCompletions Response</param>
    /// <returns>Text content from the assistant</returns>
    private async Task<string> HandleAssistantResponse(Response<ChatCompletions> response)
    {
        var responseChoice = response.Value.Choices.First();
        response = await _openAIClient.GetChatCompletionsAsync(_chatCompletionOptions);
        if (responseChoice is null)
            return string.Empty;

        if (responseChoice.FinishReason == CompletionsFinishReason.ToolCalls)
        {
            var toolResponse = await HandleToolCall(response);
            return await HandleAssistantResponse(toolResponse);
        }
        ChatMessages.Add(new ChatRequestAssistantMessage(responseChoice.Message.Content));
        var chatMessage = response.Value.Choices.First();
        if (chatMessage is null)
            return string.Empty;
        ChatMessages.Add(new ChatRequestAssistantMessage(chatMessage.Message.Content));
        return chatMessage.Message.Content;
    }

    /// <summary>
    /// Handles responses that are tool calls, such as opening a browser.
    /// </summary>
    /// <param name="response">ChatCompletions Response</param>
    /// <returns>A response from calling OpenAI with a function request</returns>
    private async Task<Response<ChatCompletions>> HandleToolCall(Response<ChatCompletions> response)
    {
        var responseChoice = response.Value.Choices.First();
        ChatMessages.Add(new ChatRequestAssistantMessage(responseChoice.Message.Content)
        {
            FunctionCall = responseChoice.Message.FunctionCall
        });

        var tools = responseChoice.Message.ToolCalls.FirstOrDefault();
        if (tools is null)
            return response;
        if (!(tools is ChatCompletionsFunctionToolCall))
            return response;
        var toolCall = tools as ChatCompletionsFunctionToolCall; 
        
        if (toolCall.Name == ChatFunctions.OpenBrowser.Name)
        {
            var peruseResponse = await _peruseApi.OpenBrowser();
            var functionResponseMessage = new ChatRequestFunctionMessage(
                name: toolCall.Name,
                content: JsonSerializer.Serialize(peruseResponse.Content, _jsonSerializerOptions));
            ChatMessages.Add(functionResponseMessage);
            _chatCompletionOptions.Messages.Add(functionResponseMessage);
        }
            
        if (toolCall.Name == ChatFunctions.NavigateToPage.Name)
        {
            string arguments = toolCall.Arguments ?? string.Empty;
            var navigateToPageReq = JsonSerializer.Deserialize<Api.Requests.NavigateToPageRequest>(arguments, _jsonSerializerOptions);
            var peruseResponse = await _peruseApi.NavigateToPage(navigateToPageReq);
            var functionResponseMessage = new ChatRequestFunctionMessage(
                name: toolCall.Name,
                content: JsonSerializer.Serialize(peruseResponse.Content, _jsonSerializerOptions));
            ChatMessages.Add(functionResponseMessage);
            _chatCompletionOptions.Messages.Add(functionResponseMessage);
        }
        var completionsResponse = await _openAIClient.GetChatCompletionsAsync(_chatCompletionOptions);
        return completionsResponse;
    }
}