using System.Collections.Generic;
using System;

namespace part_two_poe
{
    // Parent class for chatbot operations
    class Chatbot
    {
        private string username;
        private MemoryHandler memory;
        private SentimentAnalysis sentiment;
        private ResponseManager responseManager;
        private string lastTopic; // Stores last discussed topic

        public Chatbot()
        {
            memory = new MemoryHandler();
            sentiment = new SentimentAnalysis();
            responseManager = new ResponseManager();
            lastTopic = "";
            Start();
        }

        private void Start()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("\nChatbot: Hey, what is your first name? ");
        Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("You: ");
            username = Console.ReadLine()?.Trim();
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"\nChatbot: Hey {username}, how can I assist you today? Ask me anything about cybersecurity!");
        

            if (string.IsNullOrWhiteSpace(username))
            {
                username = "Guest";
            }

            memory.StoreData("name", username);

            string userQuery;
            do
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write($"{username}: ");
                userQuery = Console.ReadLine()?.ToLower().Trim();

                if (!string.IsNullOrEmpty(userQuery) && userQuery != "exit")
                {
                    ProcessQuery(userQuery);
                }

                if (string.IsNullOrEmpty(lastTopic))
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine($"\nChatbot: Hey {username}, how can I assist you today? Ask me anything about cybersecurity!");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine($"Chatbot: Do you have any more questions related to {lastTopic}, {username}?");
                }

            } while (userQuery != "exit");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\nChatbot: Bye for now, {username}! Stay safe online!");
            Console.ResetColor();
        }

        private void ProcessQuery(string query)
        {
            string sentimentResponse = sentiment.AnalyzeSentiment(query);
            if (!string.IsNullOrEmpty(sentimentResponse))
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine($"Chatbot: {sentimentResponse}");
                Console.ResetColor();
                return;
            }

            if (query.StartsWith("interested in"))
            {
                string interest = query.Replace("interested in", "").Trim();
                memory.StoreData("interest", interest);
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine($"Chatbot: I'll remember that you're interested in {interest}. It's an important cybersecurity topic.");
                Console.ResetColor();
                return;
            }

            if (query.StartsWith("remind me"))
            {
                string interest = memory.GetData("interest");
                if (!string.IsNullOrEmpty(interest))
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine($"Chatbot: You previously mentioned that you're interested in {interest}. Want to learn more?");
                    Console.ResetColor();
                    return;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine("Chatbot: I don't have any saved interests for you yet. Let me know what you like!");
                    Console.ResetColor();
                    return;
                }
            }

            string response = responseManager.GetResponse(query);
            if (!response.StartsWith("I'm not sure"))
            {
                lastTopic = query.Contains("password") ? "password security" :
                            query.Contains("phishing") ? "phishing safety" :
                            query.Contains("privacy") ? "privacy protection" :
                            query.Contains("scam") ? "scam awareness" : lastTopic;
            }

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"Chatbot: {response}");
            Console.ResetColor();
        }
    }

    class MemoryHandler
    {
        private Dictionary<string, string> memory;

        public MemoryHandler()
        {
            memory = new Dictionary<string, string>();
        }

        public void StoreData(string key, string value)
        {
            memory[key] = value;
        }

        public string GetData(string key)
        {
            return memory.ContainsKey(key) ? memory[key] : null;
        }
    }

    class SentimentAnalysis
    {
        private List<string> worriedWords = new List<string> { "worried", "scared", "anxious", "fearful" };
        private List<string> curiousWords = new List<string> { "curious", "wondering", "interested" };
        private List<string> frustratedWords = new List<string> { "frustrated", "angry", "annoyed", "upset" };

        public string AnalyzeSentiment(string input)
        {
            if (worriedWords.Exists(word => input.Contains(word)))
                return "I understand that cybersecurity can be concerning. Let me share some tips to help you feel more secure.";

            if (curiousWords.Exists(word => input.Contains(word)))
                return "Great curiosity! Learning about cybersecurity helps protect you online.";

            if (frustratedWords.Exists(word => input.Contains(word)))
                return "I'm sorry you're feeling frustrated. Cybersecurity can be tricky, but I'm here to help.";

            return null;
        }
    }

    class ResponseManager
    {
        private Dictionary<string, List<string>> keywordResponses;
        private Random random;

        public ResponseManager()
        {
            keywordResponses = new Dictionary<string, List<string>>();
            random = new Random();
            StoreResponses();
        }

        private void StoreResponses()
        {
            keywordResponses.Add("password", new List<string>
            {
                "Make sure to use strong, unique passwords for each account. Avoid using personal details.",
                "Consider using a password manager for better security.",
                "A great password should be at least 12 characters long and include a mix of symbols, numbers, and letters."
            });

            keywordResponses.Add("phishing", new List<string>
            {
                "Be cautious of emails asking for personal information. Scammers disguise themselves as trusted entities.",
                "Check for spelling mistakes and suspicious URLs in emails—they often indicate phishing.",
                "Never click on links from unknown senders. Verify the source before sharing sensitive information."
            });

            keywordResponses.Add("privacy", new List<string>
            {
                "Keeping your personal data secure is crucial. Review your privacy settings on social media.",
                "Always use encrypted connections (HTTPS) when browsing online.",
                "Enable two-factor authentication to safeguard your online accounts."
            });

            keywordResponses.Add("scam", new List<string>
            {
                "Online scams trick people into revealing sensitive information. Verify suspicious messages.",
                "Never send money or share data unless you are certain of the recipient's authenticity.",
                "Cybercriminals often use fake giveaways or job offers—stay cautious when clicking unknown links!"
            });
        }

        public string GetResponse(string query)
        {
            foreach (var keyword in keywordResponses.Keys)
            {
                if (query.Contains(keyword))
                {
                    List<string> responses = keywordResponses[keyword];
                    return responses[random.Next(responses.Count)];
                }
            }
            return "I'm not sure I understand. Can you try rephrasing?";
        }
    }
}