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
            
            if(msg.Text == "/start")
                await Client.SendTextMessageAsync(msg.Chat.Id, "Здарова ёпта", replyMarkup: GetButtons());
            
            if (msg.Text.ToLower() is "/reps" or "замены")
                SendReplacements(msg.Chat.Id);
            
            if (msg.Text.ToLower() is "/week_type" or "тип недели")
                SendWeekType(msg.Chat.Id);
        }

        private static async void SendWeekType(long chatId)
        {
            int x = (new DateTime(2021, 3, 26) - new DateTime()).Days / 7;
            string result = x % 2 == 0 ? "Числитель" : "Знаменатель";
            await Client.SendTextMessageAsync(chatId, result);
        }
        
        private static async void SendReplacements(long chatId)
        {
            try
            {
                await Client.SendTextMessageAsync(chatId, "Погодь...");
                await Client.SendTextMessageAsync(chatId, await SharagaReplacement.GetReplacementsString("https://cutt.ly/3vA4E5x"));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static ReplyKeyboardMarkup GetButtons()
        {
            return new()
            {
                Keyboard = new List<List<KeyboardButton>>
                {
                    new()
                    {
                        new KeyboardButton { Text = "Замены" },
                        new KeyboardButton { Text = "Тип недели" },
                    }
                }
            };
        }
    }
}