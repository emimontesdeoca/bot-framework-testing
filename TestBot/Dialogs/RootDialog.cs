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
        public static Dictionary<string, object> data = new Dictionary<string, object>{
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

            var r = context.MakeMessage();
            string s = "";
            foreach (var item in data)
            {
                if (item.Key == activity.Text)
                {
                    if (item.Value is Attachment)
                    {
                        r.Attachments = new List<Attachment>() { item.Value as Attachment };
                    }
                    if (item.Value is string)
                    {
                        s = item.Value.ToString();
                    }

                    break;
                }
            }

            if (r != null)
            {
                await context.PostAsync(s);
            }
            else
            {
                await context.PostAsync(r);
            }
            // return our reply to the user

            context.Wait(MessageReceivedAsync);
        }

        //private async Task MessageReceivedReturnAttachmentAsync(IDialogContext context, IAwaitable<object> result)
        //{
        //    var activity = await result as Activity;

        //    var r = context.MakeMessage();

        //    foreach (var item in dataAttachments)
        //    {
        //        if (item.Key == activity.Text)
        //        {
        //            r.Attachments = new List<Attachment>() { item.Value as Attachment };
        //            break;
        //        }
        //    }

        //    // return our reply to the user
        //    await context.PostAsync(r);

        //    context.Wait(MessageReceivedReturnAttachmentAsync);
        //}
    }
}