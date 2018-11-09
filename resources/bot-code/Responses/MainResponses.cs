using System.Threading.Tasks;
using Microsoft.Bot.Builder;

namespace Responses
{
    public class MainResponses
    {
        public static async Task ReplyWithGreeting(ITurnContext context)
        {
            // Add a greeting
            await context.SendActivityAsync($"Hi, I'm CognitiveSearchBot!");
        }
        public static async Task ReplyWithHelp(ITurnContext context)
        {
            await context.SendActivityAsync($"I can retrieve cognitive fields from Azure Search. To start a new search, respond \"search\"");
        }
        public static async Task ReplyWithResumeTopic(ITurnContext context)
        {
            await context.SendActivityAsync($"What can I do for you?");
        }
        public static async Task ReplyWithConfused(ITurnContext context)
        {
            // Add a response for the user if Regex doesn't know
            // What the user is trying to communicate
            await context.SendActivityAsync($"I'm sorry, I don't understand.");
        }
    }
}