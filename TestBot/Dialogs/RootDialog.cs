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
        public static Dictionary<string, string> dataText = new Dictionary<string, string>{
            {"Nuevo", "Que quieres crear?"},
            {"Ayuda", "Ya te ayudamos!"},
            {"Adios", "Nos vemos!"}
        };

        public static Dictionary<string, Attachment> dataAtt = new Dictionary<string, Attachment>{
            {
          "Coche",
          new Attachment() {
           ContentUrl = "https://media.ed.edmunds-media.com/subaru/impreza/2006/oem/2006_subaru_impreza_sedan_sti_fq_oem_1_500.jpg",
            ContentType = "image/png",
            Name = "Subaru_Impreza.png"
          }
         },
         {
          "Moto",
          new Attachment() {
           ContentUrl = "http://motos.honda.com.co/sites/default/files/motos/cb-1000-r-cc-menu-honda.png",
            ContentType = "image/png",
            Name = "moto.png"
          }
         },
         {
          "Perro",
          new Attachment() {
           ContentUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/6/6b/Taka_Shiba.jpg/1200px-Taka_Shiba.jpg",
            ContentType = "image/png",
            Name = "ShibaInu.png"
          }
         }
        };

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedWithAttachmentAsync);

            return Task.CompletedTask;
        }

        public async Task MessageReceivedWithTextAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            string r = "";

            foreach (var item in dataText)
            {
                if (item.Key == activity.Text)
                {
                    r = item.Value;
                    break;
                }
            }

            if (string.IsNullOrWhiteSpace(r) || r == null)
            {
                r = "No tengo respuesta para eso.";
            }

            // return our reply to the user
            await context.PostAsync(r);

            //context.Wait(MessageReceivedWithTextAsync);
        }

        public async Task MessageReceivedWithAttachmentAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var activity = await result as Activity;

            var r = context.MakeMessage();

            foreach (var item in dataAtt)
            {
                if (item.Key == activity.Text)
                {
                    r.Attachments.Add(item.Value as Attachment);
                    break;
                }
            }


            // return our reply to the user
            await context.PostAsync(r);

            context.Wait(MessageReceivedWithAttachmentAsync);
        }
    }
}