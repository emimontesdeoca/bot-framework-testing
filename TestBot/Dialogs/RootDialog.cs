using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.IO;
using System.Collections.Generic;
using System.Threading;

namespace TestBot.Dialogs
{

    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public static Dictionary<string, string> dataText = new Dictionary<string, string>{
            {"Nuevo", "Que quieres crear?"},
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
            context.Wait(MessageReceivedWithTextAsync);

            return Task.CompletedTask;
        }

        public async Task AfterAskingHelp(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            Activity reply = new Activity();

            if (activity.Text.ToString().ToLower().Contains("email"))
            {
                reply.Text = "Has elegido email";
                await context.PostAsync(reply);

            }
            else if (activity.Text.ToString().ToLower().Contains("telefono"))
            {
                List<CardAction> cardActions = new List<CardAction>();

                CardAction button1 = new CardAction()
                {
                    Type = "imBack",
                    Title = "Oficina",
                    Value = "Oficina"
                };
                CardAction button2 = new CardAction()
                {
                    Type = "imBack",
                    Title = "Hotel",
                    Value = "Hotel"
                };
                CardAction button3 = new CardAction()
                {
                    Type = "imBack",
                    Title = "Emergencia",
                    Value = "Emergencia"
                };

                cardActions.Add(button1);
                cardActions.Add(button2);
                cardActions.Add(button3);

                HeroCard h = new HeroCard();
                h.Text = "Que telefono quieres?";
                h.Buttons = cardActions;

                var ac = activity.CreateReply("");

                Attachment att = h.ToAttachment();
                att.Name = "Necesitas ayuda?";

                ac.Attachments.Add(att);

                await context.PostAsync(ac);
                context.Wait(AfterAskingHelpTelefono);
            }
            else
            {
                reply.Text = "Has elegido persona";
                await context.PostAsync(reply);
            }
        }

        public async Task AfterAskingHelpTelefono(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            Activity reply = new Activity();

            if (activity.Text.ToString() == "Oficina")
            {
                List<CardAction> cardActions = new List<CardAction>();

                CardAction button = new CardAction()
                {
                    Type = "imBack",
                    Title = "Madrid",
                    Value = "Madrid"
                };

                CardAction button1 = new CardAction()
                {
                    Type = "imBack",
                    Title = "Tenerife",
                    Value = "Tenerife"
                };

                cardActions.Add(button);
                cardActions.Add(button1);

                HeroCard h = new HeroCard();
                h.Text = "Que oficina quieres llamar?";
                h.Buttons = cardActions;

                var ac = activity.CreateReply();
                Attachment att = h.ToAttachment();
                att.Name = "Que oficina quieres llamar";

                ac.Attachments.Add(att);

                await context.PostAsync(ac);
                context.Wait(AfterAskingHelpTelefonoLugar);

                return;
            }
            else if (activity.Text.ToString() == "Hotel")
            {
                reply.Text = "Has elegido hotel";
                await context.PostAsync(reply);
            }
            else
            {
                reply.Text = "Has elegido emergencia";
                await context.PostAsync(reply);
            }


        }

        public async Task AfterAskingHelpTelefonoLugar(IDialogContext context, IAwaitable<object> activity)
        {

            var result = await activity as Activity;
            string res = "";

            if (result.Text.ToString() == "Madrid")
            {
                res = "902242526";
                await context.PostAsync(res);
            }
            else if (result.Text.ToString() == "Tenerife")
            {
                res = "922920252";
                await context.PostAsync(res);
            }

        }


        public async Task MessageReceivedWithTextAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            var reply = activity.CreateReply();

            /// null or spaces
            if ((string.IsNullOrWhiteSpace(activity.Text) || activity.Text == null) && reply.Attachments.Count == 0)
            {
                reply.Text = "No tengo respuesta para eso.";
                await context.PostAsync(reply);
                return;
            }

            /// help
            if (activity.Text.ToLower().Contains("ayuda"))
            {
                //PromptDialog.PromptChoice<string> a = new PromptDialog.PromptChoice<string>(new List<string>() { "Email", "Persona", "Telefono" }, "Necesitas ayuda?", "", 3, promptStyle: PromptStyle.Auto);
                List<CardAction> cardActions = new List<CardAction>();

                CardAction button = new CardAction()
                {
                    Type = "imBack",
                    Title = "Telefono",
                    Value = "Telefono"
                };

                cardActions.Add(button);

                HeroCard h = new HeroCard();
                h.Text = "Necesitas ayuda?";
                h.Buttons = cardActions;

                var ac = activity.CreateReply();
                Attachment att = h.ToAttachment();
                att.Name = "Necesitas ayuda?";

                ac.Attachments.Add(att);

                await context.PostAsync(ac);
                context.Wait(AfterAskingHelp);

                return;
            }

            /// else
            foreach (var item in dataText)
            {
                if (item.Key == activity.Text)
                {
                    reply.Text = item.Value;
                    break;
                }
            }

            foreach (var item in dataAtt)
            {
                if (item.Key == activity.Text)
                {
                    reply.Attachments.Add(item.Value);
                    reply.Text = "attachment";
                    break;
                }
            }

            if (reply.Text == "" && reply.Attachments.Count == 0)
            {
                reply.Text = "No tengo respuesta para eso.";
            }
            // return our reply to the user
            await context.PostAsync(reply);
        }

    }
}
