using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Microsoft.QualityTools.Testing.Fakes;
using TestBot.Dialogs;
using Microsoft.Bot.Connector;
using System.Collections.Generic;
using Microsoft.Bot.Builder.Dialogs.Fakes;
using System.Linq;
using Newtonsoft.Json;

namespace Blog.Tests.Dialogs
{
    [TestClass]
    public class RootDialogTests
    {
        string errorMessage = "No tengo respuesta para eso.";

        [TestMethod]
        public async Task TestShouldReturnText()
        {
            foreach (var item in RootDialog.dataText)
            {
                var question = item.Key;
                var expectedResult = item.Value;

                using (ShimsContext.Create())
                {
                    // Arrange
                    var waitCalled = false;
                    IMessageActivity message = null;

                    var target = new RootDialog();

                    var activity = new Activity(ActivityTypes.Message)
                    {
                        Text = question as string,
                        From = new ChannelAccount("id", "name"),
                        Recipient = new ChannelAccount("recipid", "recipname"),
                        Conversation = new ConversationAccount(false, "id", "name")
                    };

                    var awaiter = new Microsoft.Bot.Builder.Internals.Fibers.Fakes.StubIAwaiter<IMessageActivity>()
                    {
                        IsCompletedGet = () => true,
                        GetResult = () => activity
                    };

                    var awaitable = new Microsoft.Bot.Builder.Dialogs.Fakes.StubIAwaitable<IMessageActivity>()
                    {
                        GetAwaiter = () => awaiter
                    };

                    var context = new Microsoft.Bot.Builder.Dialogs.Fakes.StubIDialogContext();

                    context.PostAsyncIMessageActivityCancellationToken = (messageActivity, token) =>
                    {
                        message = messageActivity;
                        return Task.CompletedTask;
                    };

                    Microsoft.Bot.Builder.Dialogs.Fakes.ShimExtensions.WaitIDialogStackResumeAfterOfIMessageActivity = (stack, callback) =>
                    {
                        if (waitCalled) return;

                        waitCalled = true;

                        // The callback is what is being tested.
                        callback(context, awaitable);
                    };

                    // Act
                    //await target.StartAsync(context);
                    await target.MessageReceivedWithTextAsync(context, awaitable);



                    // Assert
                    Assert.AreEqual(expectedResult, message.Text);
                }
            }
        }


        [TestMethod]
        public async Task TestShouldReturnErrorIfEmptyOrSpaces()
        {
            List<string> strings = new List<string>() { "", "      " };

            foreach (var item in strings)
            {
                var question = item;

                using (ShimsContext.Create())
                {
                    // Arrange
                    var waitCalled = false;
                    IMessageActivity message = null;

                    var target = new RootDialog();

                    var activity = new Activity(ActivityTypes.Message)
                    {
                        Text = question,
                        From = new ChannelAccount("id", "name"),
                        Recipient = new ChannelAccount("recipid", "recipname"),
                        Conversation = new ConversationAccount(false, "id", "name")
                    };

                    var awaiter = new Microsoft.Bot.Builder.Internals.Fibers.Fakes.StubIAwaiter<IMessageActivity>()
                    {
                        IsCompletedGet = () => true,
                        GetResult = () => activity
                    };

                    var awaitable = new Microsoft.Bot.Builder.Dialogs.Fakes.StubIAwaitable<IMessageActivity>()
                    {
                        GetAwaiter = () => awaiter
                    };

                    var context = new Microsoft.Bot.Builder.Dialogs.Fakes.StubIDialogContext();

                    context.PostAsyncIMessageActivityCancellationToken = (messageActivity, token) =>
                    {
                        message = messageActivity;
                        return Task.CompletedTask;
                    };

                    Microsoft.Bot.Builder.Dialogs.Fakes.ShimExtensions.WaitIDialogStackResumeAfterOfIMessageActivity = (stack, callback) =>
                    {
                        if (waitCalled) return;

                        waitCalled = true;

                        // The callback is what is being tested.
                        callback(context, awaitable);
                    };

                    // Act
                    //await target.StartAsync(context);
                    await target.MessageReceivedWithTextAsync(context, awaitable);

                    // Assert
                    Assert.AreEqual(message.Text, errorMessage);
                }
            }
        }

        [TestMethod]
        public async Task TestShouldReturnErrorIfTextNotFound()
        {
            List<string> strings = new List<string>() { "abcdf", "emi" };

            foreach (var item in strings)
            {
                var question = item;

                using (ShimsContext.Create())
                {
                    // Arrange
                    var waitCalled = false;
                    IMessageActivity message = null;

                    var target = new RootDialog();

                    var activity = new Activity(ActivityTypes.Message)
                    {
                        Text = question,
                        From = new ChannelAccount("id", "name"),
                        Recipient = new ChannelAccount("recipid", "recipname"),
                        Conversation = new ConversationAccount(false, "id", "name")
                    };

                    var awaiter = new Microsoft.Bot.Builder.Internals.Fibers.Fakes.StubIAwaiter<IMessageActivity>()
                    {
                        IsCompletedGet = () => true,
                        GetResult = () => activity
                    };

                    var awaitable = new Microsoft.Bot.Builder.Dialogs.Fakes.StubIAwaitable<IMessageActivity>()
                    {
                        GetAwaiter = () => awaiter
                    };

                    var context = new Microsoft.Bot.Builder.Dialogs.Fakes.StubIDialogContext();

                    context.PostAsyncIMessageActivityCancellationToken = (messageActivity, token) =>
                    {
                        message = messageActivity;
                        return Task.CompletedTask;
                    };

                    Microsoft.Bot.Builder.Dialogs.Fakes.ShimExtensions.WaitIDialogStackResumeAfterOfIMessageActivity = (stack, callback) =>
                    {
                        if (waitCalled) return;

                        waitCalled = true;

                        // The callback is what is being tested.
                        callback(context, awaitable);
                    };

                    // Act
                    //await target.StartAsync(context);
                    await target.MessageReceivedWithTextAsync(context, awaitable);

                    // Assert
                    Assert.AreEqual(message.Text, errorMessage);
                }
            }
        }

        [TestMethod]
        public async Task TestShouldReturnAttachment()
        {
            foreach (var item in RootDialog.dataAtt)
            {
                var question = item.Key;
                var expectedAttachment = item.Value;

                using (ShimsContext.Create())
                {
                    var waitCalled = false;
                    IMessageActivity message = null;

                    var target = new RootDialog();

                    var activity = new Activity(ActivityTypes.Message)
                    {
                        Text = question,
                        From = new ChannelAccount("id", "name"),
                        Recipient = new ChannelAccount("recipid", "recipname"),
                        Conversation = new ConversationAccount(false, "id", "name")
                    };

                    var awaiter = new Microsoft.Bot.Builder.Internals.Fibers.Fakes.StubIAwaiter<IMessageActivity>()
                    {
                        IsCompletedGet = () => true,
                        GetResult = () => activity
                    };

                    var awaitable = new Microsoft.Bot.Builder.Dialogs.Fakes.StubIAwaitable<IMessageActivity>()
                    {
                        GetAwaiter = () => awaiter
                    };

                    var context = new Microsoft.Bot.Builder.Dialogs.Fakes.StubIDialogContext();

                    context.PostAsyncIMessageActivityCancellationToken = (messageActivity, token) =>
                    {
                        message = messageActivity;
                        return Task.CompletedTask;
                    };

                    Microsoft.Bot.Builder.Dialogs.Fakes.ShimExtensions.WaitIDialogStackResumeAfterOfIMessageActivity = (stack, callback) =>
                    {
                        if (waitCalled) return;
                        waitCalled = true;
                        callback(context, awaitable);
                    };
                    await target.MessageReceivedWithTextAsync(context, awaitable);


                    Assert.AreEqual(expectedAttachment, message.Attachments[0]);
                }
            }
        }


        [TestMethod]
        public async Task TestShouldReturnTenerifeNumberFlow()
        {
            var choice1 = "ayuda";
            var expectedResultChoice1 = "Necesitas ayuda?";

            var choice2 = "telefono";
            var expectedResultChoice2 = "Que telefono quieres?";

            var choice3 = "oficina";
            var expectedResultChoice3 = "Que oficina quieres llamar?";

            var choice4 = "Tenerife";
            var expectedResultChoice4 = "922920252";


            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();

            keyValuePairs.Add(choice1, expectedResultChoice1);
            keyValuePairs.Add(choice2, expectedResultChoice2);
            keyValuePairs.Add(choice3, expectedResultChoice3);
            keyValuePairs.Add(choice4, expectedResultChoice4);

            List<string> choices = new List<string>() { "" };



            using (ShimsContext.Create())
            {
                var waitCalled = false;
                IMessageActivity message = null;

                var target = new RootDialog();
                int i = 0;
                var activity = new Activity(ActivityTypes.Message)
                {
                    Text = choice1,
                    From = new ChannelAccount("id", "name"),
                    Recipient = new ChannelAccount("recipid", "recipname"),
                    Conversation = new ConversationAccount(false, "id", "name")
                };

                var awaiter = new Microsoft.Bot.Builder.Internals.Fibers.Fakes.StubIAwaiter<IMessageActivity>()
                {
                    IsCompletedGet = () => true,
                    GetResult = () => activity

                };

                var awaitable = new Microsoft.Bot.Builder.Dialogs.Fakes.StubIAwaitable<IMessageActivity>()
                {
                    GetAwaiter = () => awaiter
                };

                var context = new Microsoft.Bot.Builder.Dialogs.Fakes.StubIDialogContext();

                context.PostAsyncIMessageActivityCancellationToken = (messageActivity, token) =>
                {
                    message = messageActivity as Activity;

                    if (message.Attachments.Count > 0)
                    {
                        Assert.AreEqual(keyValuePairs.ElementAt(i).Value, message.Attachments[0].Name);
                    }
                    else
                    {
                        Assert.AreEqual(keyValuePairs.ElementAt(i).Value, message.Text);
                    }
                    return Task.CompletedTask;
                };

                Microsoft.Bot.Builder.Dialogs.Fakes.ShimExtensions.WaitIDialogStackResumeAfterOfIMessageActivity = (stack, callback) =>
                {
                    i++;
                    if (waitCalled) return;
                    waitCalled = true;

                    activity.Text = keyValuePairs.ElementAt(i).Key;
                    var a = awaitable;

                    callback(context, awaitable);
                };

                await target.MessageReceivedWithTextAsync(context, awaitable);

                //Assert.AreEqual(expecteResultChoice1, message.Attachments[0].Content);

            }
        }
    }
}
