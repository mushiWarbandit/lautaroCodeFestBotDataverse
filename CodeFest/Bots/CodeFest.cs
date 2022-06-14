// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.6.2

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EchoBot2.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Teams;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Schema.Teams;
using Newtonsoft.Json.Linq;
using static EchoBot2.Models.Dataverse;

namespace EchoBot2.Bots
{
    public class CodeFest : ActivityHandler
    {
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            
            if (turnContext.Activity.Text.ToUpper() == "HI" || turnContext.Activity.Text.ToUpper() == "HELLO")
            {
                
                await turnContext.SendActivityAsync(CreateActivityWithTextAndSpeak($"Hello!, {turnContext.Activity.From.Name} "), cancellationToken);
                await turnContext.SendActivityAsync(CreateActivityWithTextAndSpeak($"I m a virtual agent. I can help with events registrations"), cancellationToken);
                await turnContext.SendActivityAsync(CreateActivityWithTextAndSpeak($"So, what can I help you ?"), cancellationToken);
                await turnContext.SendActivityAsync(CreateActivityWithTextAndSpeak($"0 - List of OC events Available"), cancellationToken);
                await turnContext.SendActivityAsync(CreateActivityWithTextAndSpeak($"1 - Subscribe"), cancellationToken);
                await turnContext.SendActivityAsync(CreateActivityWithTextAndSpeak($"2 - Unsubscribe"), cancellationToken);
               


            }
            else {
                await Flow(turnContext,cancellationToken);
            }
            

        }
        protected  async Task Menu(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken) {
            await turnContext.SendActivityAsync(CreateActivityWithTextAndSpeak($"This is the Main Menu"), cancellationToken);
            await turnContext.SendActivityAsync(CreateActivityWithTextAndSpeak($"0 - List of OC events Available"), cancellationToken);
            await turnContext.SendActivityAsync(CreateActivityWithTextAndSpeak($"1 - Subscribe"), cancellationToken);
            await turnContext.SendActivityAsync(CreateActivityWithTextAndSpeak($"2 - Unsubscribe"), cancellationToken);
        }
        protected async Task Flow(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            string[] words;
            #region SUBS REGION

            if (turnContext.Activity.Text == "1")
            {

                await turnContext.SendActivityAsync(CreateActivityWithTextAndSpeak($"for subscribing to an available event, write SUBS and the number of the event"), cancellationToken);
                await turnContext.SendActivityAsync(CreateActivityWithTextAndSpeak($"ex. SUBS 23"), cancellationToken);
            }
            bool check = turnContext.Activity.Text.ToUpper().Contains("SUBS");
            if (check)
            {
                words = turnContext.Activity.Text.ToUpper().Split(' ');
                await SuscribeToEvent(turnContext,cancellationToken, words[1]);
            }
            #endregion
            #region UNSUBS REGION
            if (turnContext.Activity.Text == "2")
            {

                await turnContext.SendActivityAsync(CreateActivityWithTextAndSpeak($"for unsubscribing to an event, write UNSUB and the number of the event"), cancellationToken);
                await turnContext.SendActivityAsync(CreateActivityWithTextAndSpeak($"ex. UNSUB 23"), cancellationToken);
            }
            check = turnContext.Activity.Text.ToUpper().Contains("UNSUB");
            if (check)
            {
                words = turnContext.Activity.Text.ToUpper().Split(' ');
                await turnContext.SendActivityAsync(CreateActivityWithTextAndSpeak($"You have been unsubscribed to " + words[1] + " event"), cancellationToken);

            }
            #endregion
            #region Event List Region
            if (turnContext.Activity.Text == "0")
            {
                
                await turnContext.SendActivityAsync(CreateActivityWithTextAndSpeak($"Event list :"), cancellationToken);
                await GetEvents(turnContext, cancellationToken);
  

            }
            #endregion
            #region menu Region
            if (turnContext.Activity.Text.ToUpper() == "M" || turnContext.Activity.Text == "MENU")
            {
                await Menu(turnContext, cancellationToken);


            }
            else
            {
                if (!turnContext.Activity.Text.ToUpper().Contains("UNSUB") && !turnContext.Activity.Text.ToUpper().Contains("SUBS") && !turnContext.Activity.Text.ToUpper().Contains("0") && !turnContext.Activity.Text.ToUpper().Contains("1") && !turnContext.Activity.Text.ToUpper().Contains("2"))
                {
                    await turnContext.SendActivityAsync(CreateActivityWithTextAndSpeak($"I'm Sorry!, I could not understand, please try again or "), cancellationToken);
                }

            }
            #endregion
            await turnContext.SendActivityAsync(CreateActivityWithTextAndSpeak($" press M to return to Menu"), cancellationToken);

        }
        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(CreateActivityWithTextAndSpeak($"Hello and welcome!"), cancellationToken);
                }
            }
        }
        protected async Task GetEvents(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            try{
                List<RootFlow> list = (await Events.GetEvent());

                foreach (var item in list)
                {
                    string temp = "Event Code : " + item.crea1_evecodint + ", name: " + item.crea1_evetitulo + ", date: " + item.Crea1Evefchini2ODataCommunityDisplayV1FormattedValue;
                    await turnContext.SendActivityAsync(CreateActivityWithTextAndSpeak(temp), cancellationToken);
                }
                await turnContext.SendActivityAsync(CreateActivityWithTextAndSpeak("If you want to suscribe to events , press 1"), cancellationToken);
            }
                
            catch (Exception er){ er.ToString(); }
           
            
        }

        protected async Task SuscribeToEvent(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken,string eventId)
        {
            try
            {
                IEnumerable<TeamsChannelAccount> members = await TeamsInfo.GetMembersAsync(turnContext, cancellationToken);
                bool resp =await Events.Suscribe(eventId, members.ToList().ElementAt(0).Email, turnContext.Activity.From.Name);
                if (resp) await turnContext.SendActivityAsync(CreateActivityWithTextAndSpeak($"You have been subscribed to " + eventId + " event"), cancellationToken);
                if (!resp ) await turnContext.SendActivityAsync(CreateActivityWithTextAndSpeak($"You was not able to subscribed to " + eventId + " event"), cancellationToken);


            }

            catch (Exception er) { er.ToString(); }


        }

        private IActivity CreateActivityWithTextAndSpeak(string message)
        {
            var activity = MessageFactory.Text(message);
            string speak = @"<speak version='1.0' xmlns='https://www.w3.org/2001/10/synthesis' xml:lang='en-US'>
              <voice name='Microsoft Server Speech Text to Speech Voice (en-US, JessaRUS)'>" +
              $"{message}" + "</voice></speak>";
            activity.Speak = speak;
            return activity;
        }
    }
}
