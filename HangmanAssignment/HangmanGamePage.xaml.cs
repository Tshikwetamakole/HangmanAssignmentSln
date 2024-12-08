namespace HangmanAssignment
{
    public partial class HangmanGamePage : ContentPage
    {
        private string wordToGuess = "CORRESPONDENCE"; // The target word
        private char[] displayedWord = Array.Empty<char>(); // Word display with blanks
        private HashSet<char> guessedLetters = new HashSet<char>(); // Tracks guessed letters
        private int incorrectGuesses = 0; // Counter for wrong guesses
        private const int MaxGuesses = 6; // Maximum number of allowed wrong guesses

        public HangmanGamePage()
        {
            InitializeComponent();
            InitializeGame();
        }

        private void InitializeGame()
        {
            // Set up the displayed word with underscores
            displayedWord = new char[wordToGuess.Length];
            for (int i = 0; i < wordToGuess.Length; i++)
            {
                displayedWord[i] = wordToGuess[i] == ' ' ? ' ' : '_';
            }

            UpdateDisplayedWord();
            UpdateHangmanImage();
        }

        private void OnGuessButtonClicked(object sender, EventArgs e)
        {
            string guess = GuessEntry.Text?.ToUpper() ?? string.Empty;
            GuessEntry.Text = string.Empty; // Clear the input field

            if (string.IsNullOrWhiteSpace(guess) || guess.Length != 1 || !char.IsLetter(guess[0]))
            {
                DisplayAlert("Invalid Input", "Please enter a single valid letter.", "OK");
                return;
            }

            char guessedChar = guess[0];

            if (guessedLetters.Contains(guessedChar))
            {
                DisplayAlert("Already Guessed", $"You have already guessed '{guessedChar}'. Try another letter.", "OK");
                return;
            }

            guessedLetters.Add(guessedChar);

            if (wordToGuess.Contains(guessedChar))
            {
                for (int i = 0; i < wordToGuess.Length; i++)
                {
                    if (wordToGuess[i] == guessedChar)
                    {
                        displayedWord[i] = guessedChar;
                    }
                }
                UpdateDisplayedWord();

                if (new string(displayedWord) == wordToGuess)
                {
                    EndGame(true);
                }
            }
            else
            {
                incorrectGuesses++;
                UpdateHangmanImage();

                if (incorrectGuesses >= MaxGuesses)
                {
                    EndGame(false);
                }
            }
        }

        private void UpdateDisplayedWord()
        {
            // Display the current state of the word with spaces between characters
            WordLabel.Text = string.Join(" ", displayedWord);
        }

        private void UpdateHangmanImage()
        {
            // Update the hangman image based on incorrect guesses
            HangmanImage.Source = $"hang{Math.Min(incorrectGuesses + 1, MaxGuesses + 1)}.png";
        }

        private void EndGame(bool survived)
        {
            if (survived)
            {
                DisplayAlert("Congratulations", "You survived! The word was: " + wordToGuess, "OK");
            }
            else
            {
                DisplayAlert("Game Over", "You lost! The word was: " + wordToGuess, "OK");
            }

            // Reset game
            InitializeGame();
        }
    }
}
