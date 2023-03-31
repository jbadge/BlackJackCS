using System;
using System.Collections.Generic;
using System.Linq;

namespace BlackJackCS
{
    // //class Deck
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
            return sum;
        }

        static string FormatHand(List<Card> hand)
        {
            return String.Join(", ", hand.Select(c => c.Description()));
        }

        static void WelcomeToBlackjack()
        {
            // ##### FIX ##### -- BG and FG Color not working
            //Console.BackgroundColor = ConsoleColor.Blue;
            //Console.ForegroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("#################################");
            Console.WriteLine("#################################");
            Console.WriteLine("##### WELCOME TO BLACKJACK! #####");
            Console.WriteLine("#################################");
            Console.WriteLine("#################################");
            Console.WriteLine("#################################");
            Console.ResetColor();
        }

        static void Blackjack()
        {
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
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("You are dealt two cards...");
            Console.ResetColor();
            var playerHand = new List<Card>();
            DealHand(deckOfCards, playerHand, 2, false);

            var dealerHand = new List<Card>();
            DealHand(deckOfCards, dealerHand, 1, false);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"The dealer has {FormatHand(dealerHand)} and an unknown card.");
            Console.ResetColor();
            DealHand(deckOfCards, dealerHand, 1, false);

            // ### GAME ###
            bool hitting = true;
            var playerValue = AddUpHand(playerHand);
            var dealerValue = AddUpHand(dealerHand);

            // ### PLAYER HAND BEGINS ###
            while (hitting)
            {
                // REVEAL HAND TO PLAYER
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"Your cards are {FormatHand(playerHand)}.");
                Console.ResetColor();
                // how could I put line here to work for dealer hand?
                // Press any button to continue...

                // BLACKJACK! 
                if (playerValue == 21)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("### BLACKJACK!! ###");
                    Console.ResetColor();

                    /*" _     _            _    _            _    
| |   | |          | |  (_)          | |   
| |__ | | __ _  ___| | ___  __ _  ___| | __
| '_ \| |/ _` |/ __| |/ / |/ _` |/ __| |/ /
| |_) | | (_| | (__|   <| | (_| | (__|   < 
|_.__/|_|\__,_|\___|_|\_\ |\__,_|\___|_|\_\
                   _/ |                
                  |__/                 !");*/
                    return;
                }

                // PLAYER HAS NOT BUSTED OR HIT BLACKJACK
                Console.WriteLine("Would you like to (H)it or (S)tand? Press (V) to get the point value of your hand.");
                //Console.WriteLine("Press (V) to get the point value of your hand.");
                var userInput = Console.ReadLine().ToLower();

                // ## PLAYER SELECTS HIT ##
                if (userInput == "v")
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine($"Your card total is {playerValue}.");
                    Console.ResetColor();
                    Console.WriteLine("Would you like to (H)it or (S)tand?");
                    userInput = Console.ReadLine().ToLower();
                }
                if (userInput == "h")
                {

                    // PLAYER DEALT ADDITIONAL CARD(S)
                    DealHand(deckOfCards, playerHand);
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("A card is dealt to you...");
                    Console.ResetColor();
                    playerValue = AddUpHand(playerHand);

                    // PLAYER BUSTS
                    if (playerValue > 21)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine($"Your cards are {FormatHand(playerHand)}.");
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine($"You have {playerValue} points.");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("BUST! You lose!");
                        Console.ResetColor();
                        return;
                    }
                }

                // ## PLAYER SELECTS STAND ##
                else if (userInput == "s")
                {

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    // REVEAL DEALER HAND TO PLAYER
                    Console.WriteLine($"The dealer's cards are {FormatHand(dealerHand)}.");
                    Console.ResetColor();
                    hitting = false;
                }

                // IF PLAYER SELECTS ANYTHING ELSE
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
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("The dealer takes a card...");
                Console.WriteLine($"The dealer's cards are {FormatHand(dealerHand)}.");
                Console.ResetColor();
            }

            // DEALER BUSTS
            //Console.WriteLine($"Dealer has {dealerValue} points and you have {playerValue} points...");
            if (dealerValue > 21)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"You have {playerValue} points and the dealer has {dealerValue} points.");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Dealer busts! You win!");
                Console.ResetColor();
            }

            // DEALER AND PLAYER TIE
            else if (playerValue == dealerValue)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"You have {playerValue} points and the dealer has {dealerValue} points.");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ties go to the dealer. Dealer wins!");
                Console.ResetColor();
            }

            // DEALER WINS
            else if (dealerValue > playerValue)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"You have {playerValue} points and the dealer has {dealerValue} points.");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Dealer wins!");
                Console.ResetColor();
            }

            // DEALER LOSES
            else
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"You have {playerValue} points and the dealer has {dealerValue} points.");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("You win! Congratulations!");
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

                /*// ### TRYING TO GET INPUT TO BE READ BUT NOT DISPLAYED....
                //ConsoleKeyInfo cki;
                //var cki = Console.ReadKey();
                Console.WriteLine("Do you want to play again? (Y)es or (No)");
                //var userInput = cki.ToLower();
                //var userInput = Console.ReadKey.ToLower();
                var userInput = Console.Read().ToLower();*/

                Console.WriteLine("Do you want to play again? (Y)es or (No)");
                var userInput = Console.ReadLine().ToLower();
                if (userInput != "y")
                {
                    playing = false;
                    Console.WriteLine("K, bye!");
                }
            }
        }
    }
}
