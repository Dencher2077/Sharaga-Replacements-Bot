using System;
using System.Collections.Generic;
using System.Net;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

namespace SharagaReplacementsBot
{
    class Program
    {
        private static string Token => Environment.GetEnvironmentVariable("REPLACEMENTS_TOKEN");
        private static TelegramBotClient Client { get; set; }
        static void Main(string[] args)
        {
            Client = new TelegramBotClient(Token);
            Client.StartReceiving();
            Client.OnMessage += OnMessage;
            Console.ReadLine();
            Client.StopReceiving();
        }

        private static async void OnMessage(object sender, MessageEventArgs e)
        {
            var msg = e.Message;

            Console.WriteLine($"Message: {msg.Text}");

            Console.WriteLine(Environment.GetEnvironmentVariable("HOME"));
            ;
            
            if(msg.Text == "/start")
                await Client.SendTextMessageAsync(msg.Chat.Id, "Здарова ёпта", replyMarkup: GetButtons());
            
            if (msg.Text == "Замены")
            {
                try
                {
                    await Client.SendTextMessageAsync(msg.Chat.Id, "Погодь...");
                    await Client.SendTextMessageAsync(msg.Chat.Id, await SharagaReplacement.GetReplacementsString("https://cutt.ly/3vA4E5x"));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        private static ReplyKeyboardMarkup GetButtons()
        {
            return new()
            {
                Keyboard = new List<List<KeyboardButton>>
                {
                    new() {new KeyboardButton { Text = "Замены" }}
                }
            };
        }
    }
}