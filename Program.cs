using System;
using System.Collections.Generic;
using System.Linq;

namespace BlackJackCS
{
    class Card
    {
        public string Suit { get; set; }
        public string Face { get; set; }
        public int Value { get; set; }
        public string Description()
        {
            return $"{Face} of {Suit}";
        }
    }

    class Program
    {

        static void ShuffleDeck(List<Card> deck)
        {
            var randomNumberGenerator = new Random();
            for (int rightIndex = deck.Count - 1; rightIndex >= 1; rightIndex--)
            {
                var leftIndex = randomNumberGenerator.Next(deck.Count - 1);
                var leftCard = deck[leftIndex];
                var rightCard = deck[rightIndex];
                deck[rightIndex] = leftCard;
                deck[leftIndex] = rightCard;
            }
        }

        static void DealHand(List<Card> from, List<Card> to, int num = 1, bool print = true)
        // FIX IN HERE
        {
            for (int i = 0; i < num; i++)
            {
                var card = from[0];
                to.Add(card);
                from.RemoveAt(0);
                /*if (print)
                {
                    Console.WriteLine("A card is dealt...");
                    //Console.WriteLine($"The dealer has dealt {card.Description()}.");
                }*/
            }
        }

        static int AddUpHand(List<Card> hand)
        {
            var sum = 0;
            for (int i = 0; i < hand.Count; i++)
            {
                sum += hand[i].Value;
            }
            var numberOfAcesLeft = HowManyAces(hand);
            while (sum > 21 && numberOfAcesLeft > 0)
            {
                sum -= 10;
                numberOfAcesLeft--;
            }
            return sum;
        }

        static int HowManyAces(List<Card> hand)
        {
            var numberOfAces = 0;
            for (int i = 0; i < hand.Count; i++)
            {
                if (hand[i].Face == "Ace")
                {
                    numberOfAces++;
                }
            }
            return numberOfAces;
        }

        static string FormatHand(List<Card> hand)
        {
            return String.Join(", ", hand.Select(c => c.Description()));
        }

        static void WelcomeToBlackjack()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(@"    
            Welcome to...");
            Console.WriteLine(@"
    /$$$$$$$ /$$       /$$$$$$   /$$$$$$ /$$   /$$    /$$$$$  /$$$$$$   /$$$$$$ /$$   /$$
   | $$__  $| $$      /$$__  $$ /$$__  $| $$  /$$/   |__  $$ /$$__  $$ /$$__  $| $$  /$$/
   | $$  \ $| $$     | $$  \ $$| $$  \__| $$ /$$/       | $$| $$  \ $$| $$  \__| $$ /$$/ 
   | $$$$$$$| $$     | $$$$$$$$| $$     | $$$$$/        | $$| $$$$$$$$| $$     | $$$$$/ 
   | $$__  $| $$     | $$__  $$| $$     | $$  $$   /$$  | $$| $$__  $$| $$     | $$  $$ 
   | $$  \ $| $$     | $$  | $$| $$    $| $$\  $$ | $$  | $$| $$  | $$| $$    $| $$\  $$ 
   | $$$$$$$| $$$$$$$| $$  | $$|  $$$$$$| $$ \  $$|  $$$$$$/| $$  | $$|  $$$$$$| $$ \  $$
   |_______/|________|__/  |__/ \______/|__/  \__/ \______/ |__/  |__/\______/ |__/  \__/
   ");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true).Key.ToString();
            Console.Clear();
        }

        /*static Card Split(List<Card> hand)
        {
            var playerHand1 = hand[0];
            var playerHand2 = hand[1];
            for (int i = 1; i < 3; i++)
            {
                hand = playerHand(i);
                return hand;
            }
        }*/

        static void YouGotBlackjack()
        {
            Console.WriteLine(@" 
 _     _            _    _            _      _
| |   | |          | |  (_)          | |    | |
| |__ | | __ _  ___| | ___  __ _  ___| | __ | |
| '_ \| |/ _` |/ __| |/ / |/ _` |/ __| |/ / | |
| |_) | | (_| | (__|   <| | (_| | (__|   <  |_|
|_.__/|_|\__,_|\___|_|\_\ |\__,_|\___|_|\_\ (_)
                       _/ |                
                      |__/                 ");
        }

        static void Blackjack()
        {
            // is there way to streamline the color changes?
            // is there way to streamline displaying hand values for each instance?

            // ### LOCAL VARIABLES ###
            var deckOfCards = new List<Card>();
            var suits = new List<string>() { "Clubs", "Diamonds", "Hearts", "Spades" };
            var faces = new List<string>() { "Ace", "2", "3", "4", "5", "6", "7", "8", "9", "10", "Jack", "Queen", "King" };

            // ### GENERATE DECK OF CARDS
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 13; j++)
                {
                    var card = new Card();
                    card.Suit = suits[i];
                    card.Face = faces[j];
                    if (j == 0)
                    {
                        card.Value = 11;
                    }
                    else if (j > 9)
                    {
                        card.Value = 10;
                    }
                    else
                    {
                        card.Value = j + 1;
                    }
                    deckOfCards.Add(card);
                }
            }

            // ### SHUFFLING DECK ###
            ShuffleDeck(deckOfCards);

            // ### DEALING HANDS ###
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("You are dealt two cards.");
            Console.ResetColor();
            var playerHand = new List<Card>();
            DealHand(deckOfCards, playerHand, 2, false);
            // E1
            // how can I put print player hand but then not print the first time in game loop? See "E2"

            var dealerHand = new List<Card>();
            DealHand(deckOfCards, dealerHand, 1, false);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"The dealer has the {FormatHand(dealerHand)} and an unknown card.");
            Console.ResetColor();
            DealHand(deckOfCards, dealerHand, 1, false);

            // ### GAME ###
            bool hitting = true;
            var playerValue = AddUpHand(playerHand);
            var dealerValue = AddUpHand(dealerHand);

            /*// SPLIT
            if (playerHand[0].Value == playerHand[1].Value && playerHand[0].Value == 10)
            {
                Split(playerHand);
            }*/

            // ### PLAYER HAND BEGINS ###
            while (hitting)
            {
                // REVEAL HAND TO PLAYER
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"Your cards are the {FormatHand(playerHand)}.");
                Console.ResetColor();
                // E2
                // how could I put line here to work for dealer hand but only first time? See "E1"

                // BLACKJACK! 
                if (playerValue == 21)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    YouGotBlackjack();
                    Console.ResetColor();
                    break;
                }

                // PLAYER HAS NOT BUSTED OR HIT BLACKJACK
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Would you like to (H)it or (S)tand? Press (V) to get the point value of your hand.");
                var userInput = Console.ReadKey(true).Key.ToString().ToLower();

                // PLAYER SELECTS CHECK VALUE
                if (userInput == "v")
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine($"Your card total is {playerValue}.");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Would you like to (H)it or (S)tand?");
                    userInput = Console.ReadKey(true).Key.ToString().ToLower();
                    Console.ResetColor();
                }

                // ## PLAYER SELECTS HIT ##
                if (userInput == "h")
                {
                    // PLAYER DEALT ADDITIONAL CARD(S)
                    DealHand(deckOfCards, playerHand);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("A card is dealt to you.");
                    Console.ResetColor();
                    playerValue = AddUpHand(playerHand);

                    // PLAYER BUSTS
                    if (playerValue > 21)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine($"Your cards are {FormatHand(playerHand)}.");
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine($"You have {playerValue} points.");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("BUST! You lose!");
                        Console.WriteLine("### Dealer wins! ###");
                        Console.ResetColor();
                        return;
                    }
                }

                // ## PLAYER SELECTS STAND ##
                else if (userInput == "s")
                {

                    // REVEAL DEALER HAND TO PLAYER
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"The dealer's cards are {FormatHand(dealerHand)}.");
                    Console.ResetColor();
                    hitting = false;
                }

                // PLAYER SELECTS ANYTHING ELSE
                else
                {
                    Console.WriteLine("Please pick a valid option.");
                }
            }

            // ### DEALER TURN BEGINS ###
            while (dealerValue < 17)
            {

                // DEALER DEALT ADDITIONAL CARD(S)
                DealHand(deckOfCards, dealerHand);
                dealerValue = AddUpHand(dealerHand);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("The dealer takes a card...");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"The dealer's cards are {FormatHand(dealerHand)}.");
                Console.ResetColor();
            }

            // DEALER BUSTS
            if (dealerValue > 21)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"You have {playerValue} points and the dealer has {dealerValue} points.");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Dealer busts!");
                Console.WriteLine("### You win! ###");
                Console.ResetColor();
            }

            // DEALER AND PLAYER TIE
            else if (playerValue == dealerValue && playerValue != 21)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"You have {playerValue} points and the dealer has {dealerValue} points.");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Push.");
                Console.WriteLine("### No winner. ###");
                Console.ResetColor();
            }

            // DEALER WINS
            else if (dealerValue > playerValue)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"You have {playerValue} points and the dealer has {dealerValue} points.");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("### Dealer wins! ###");
                Console.ResetColor();
            }

            // DEALER LOSES
            else
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"You have {playerValue} points and the dealer has {dealerValue} points.");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("### You win! ###");
                Console.ResetColor();
            }
        }

        static void Main(string[] args)
        {
            WelcomeToBlackjack();
            var playing = true;
            while (playing)
            {
                Blackjack();

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Do you want to play again? (Y)es or any key to exit.");
                var userInput = Console.ReadKey(true).Key.ToString().ToLower();

                if (userInput != "y")
                {
                    playing = false;
                    Console.WriteLine("K, bye!");
                }
            }
        }
    }
}
