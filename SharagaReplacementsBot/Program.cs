using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
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
            if (msg.Text == null) return;
            
            Console.WriteLine($"Message: {msg.Text}");

            try
            {
                if (msg.Text.ToLower() is "/reps" or "/reps@sharaga_replacements_bot")
                    await SendReplacements(msg.Chat.Id);
            
                if (msg.Text.ToLower() is "/week_type" or "/week_type@sharaga_replacements_bot")
                    await SendWeekType(msg.Chat.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            
        }

        private static async Task SendWeekType(long chatId)
        {
            int x = (new DateTime(2021, 3, 26) - new DateTime()).Days / 7;
            string result = x % 2 == 0 ? "Числитель" : "Знаменатель";
            await Client.SendTextMessageAsync(chatId, result);
        }
        
        private static async Task SendReplacements(long chatId)
        {
            await Client.SendTextMessageAsync(chatId, "Погодь...");
            await Client.SendTextMessageAsync(chatId, await SharagaReplacement.GetReplacementsString("https://cutt.ly/3vA4E5x"));
        }

        private static ReplyKeyboardMarkup GetButtons()
        {
            return new()
            {
                Keyboard = new List<List<KeyboardButton>>
                {
                    new()
                    {
                        new KeyboardButton("Замены"),
                        new KeyboardButton ("Тип недели"),
                    }
                }
            };
        }
    }
}