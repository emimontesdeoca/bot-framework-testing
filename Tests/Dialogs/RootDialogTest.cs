using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Microsoft.QualityTools.Testing.Fakes;
using TestBot.Dialogs;
using Microsoft.Bot.Connector;
using System.Collections.Generic;
using Microsoft.Bot.Builder.Dialogs.Fakes;

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
                var preg = item.Key;
                var resp = item.Value;

                using (ShimsContext.Create())
                {
                    // Arrange
                    var waitCalled = false;
                    var message = string.Empty;

                    var target = new RootDialog();

                    var activity = new Activity(ActivityTypes.Message)
                    {
                        Text = preg
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

                    Microsoft.Bot.Builder.Dialogs.Fakes.ShimExtensions.PostAsyncIBotToUserStringStringCancellationToken = (user, s1, s2, token) =>
                    {

                        message = s1;
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
                    Assert.AreEqual(resp, message);
                }
            }
        }

        [TestMethod]
        public async Task TestShouldReturnErrorIfEmptyOrSpaces()
        {
            List<string> strings = new List<string>() { "", "      ", null };

            foreach (var item in strings)
            {
                var preg = item;

                using (ShimsContext.Create())
                {
                    // Arrange
                    var waitCalled = false;
                    var message = string.Empty;

                    var target = new RootDialog();

                    var activity = new Activity(ActivityTypes.Message)
                    {
                        Text = preg
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

                    Microsoft.Bot.Builder.Dialogs.Fakes.ShimExtensions.PostAsyncIBotToUserStringStringCancellationToken = (user, s1, s2, token) =>
                    {

                        message = s1;
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
                    Assert.AreEqual(message, errorMessage);
                }
            }
        }

        [TestMethod]
        public async Task TestShouldReturnErrorIfTextNotFound()
        {
            List<string> strings = new List<string>() { "abcdf", "emi" };

            foreach (var item in strings)
            {
                var preg = item;

                using (ShimsContext.Create())
                {
                    // Arrange
                    var waitCalled = false;
                    var message = string.Empty;

                    var target = new RootDialog();

                    var activity = new Activity(ActivityTypes.Message)
                    {
                        Text = preg
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

                    Microsoft.Bot.Builder.Dialogs.Fakes.ShimExtensions.PostAsyncIBotToUserStringStringCancellationToken = (user, s1, s2, token) =>
                    {

                        message = s1;
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
                    Assert.AreEqual(message, errorMessage);
                }
            }
        }

        [TestMethod]
        public async Task Bot_Test_Attachments()
        {
            foreach (var item in RootDialog.dataAtt)
            {
                var preg = item.Key;
                var att = item.Value;

                using (ShimsContext.Create())
                {
                    // Arrange
                    var waitCalled = false;
                    var message = new object();

                    var target = new RootDialog();

                    var activity = new Activity(ActivityTypes.Message)
                    {
                        Text = preg
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

                    Microsoft.Bot.Builder.Dialogs.Fakes.ShimExtensions.PostAsyncIBotToUserStringStringCancellationToken = (user, s1, s2, token) =>
                    {
                        message = s1;
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
                    // await target.StartAsync(context);
                    await target.MessageReceivedWithAttachmentAsync(context, awaitable);

                    // Assert
                    Assert.AreEqual(att, message);
                }
            }
        }
    }
}
