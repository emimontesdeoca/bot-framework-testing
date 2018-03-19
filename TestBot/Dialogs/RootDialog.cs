using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.IO;
using System.Collections.Generic;

namespace TestBot.Dialogs
{

    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public static Dictionary<string, string> data = new Dictionary<string, string>{
            {"Nuevo", "Que quieres crear?"},
            {"Ayuda", "Ya te ayudas!"},
            {"Adios", "Nos vemos!"}
        };

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            // calculate something for us to return
            int length = (activity.Text ?? string.Empty).Length;

            string text = "";

            foreach (var item in data)
            {
                if (item.Key == activity.Text)
                {
                    text = item.Value;
                    break;
                }
            }

            // return our reply to the user
            await context.PostAsync(text);

            context.Wait(MessageReceivedAsync);
        }
    }
}