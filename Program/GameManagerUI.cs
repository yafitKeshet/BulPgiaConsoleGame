using System;
using Ex02.ConsoleUtils;
using System.Text;

namespace A22_Ex02_Yafit_318861960_Lonnie_316223411
{
    public class GameManagerUI
    {
        private enum eGameModes
        {
            Play,
            Win,
            Loss,
            Quit
        }

        public void PlayBullsAndCleots()
        {
            printWelcome();
            int numOfGuessing = getValidNumOfGuessingFromUser();
            GameLogic gameRound = new GameLogic(numOfGuessing+1);
            startGame(gameRound);
        }

        private void startGame(GameLogic i_GameRound)
        {
            eGameModes stateOfGame = eGameModes.Play;
            string winningSequence = i_GameRound.RandWinningSequence();
            string hiddenChoice = new string('#', winningSequence.Length);

            i_GameRound.WinningSequence = winningSequence;
            i_GameRound.InsertNewGuessAndFeedbackCalculation(hiddenChoice);
            playUntilGameIsOver(i_GameRound, ref stateOfGame);
            printGameStatistics(stateOfGame, i_GameRound.Board);
            if(stateOfGame != eGameModes.Quit)
            {
                if(isWantsOneMoreGame())
                {
                    i_GameRound.NewGame();
                    startGame(i_GameRound);
                }
            }

            printGoodBye();
        }

        private bool isWantsOneMoreGame()
        {
            Console.WriteLine("Would you like to start a new game? < Y / N > ");
            string userInput = getValidUserChoiceYOrN();

            return userInput.Equals("Y");
        }

        private string getValidUserChoiceYOrN()
        {
            bool isValidInput = false;
            string userInput = Console.ReadLine();

            while (!isValidInput)
            {
                isValidInput = userInput.Equals("Y") || userInput.Equals("N");

                if (!isValidInput)
                {
                    Console.WriteLine("Please enter a valid selection. Either Y or N ");
                    userInput = Console.ReadLine();
                }
            }

            return userInput;
        }

        private void printGameStatistics(eGameModes i_StateOfGame, GameBoard i_GameBoard)
        {
            switch(i_StateOfGame)
            {
                case eGameModes.Win:
                    Console.WriteLine("You guessed after {0} steps!", i_GameBoard.GetFirstEmptyRow()-1);
                    break;
                case eGameModes.Loss:
                    Console.WriteLine("No more guesses allowed. You Lost.");
                    break;
            }
        }

        private void playUntilGameIsOver(GameLogic i_GameRound, ref eGameModes io_StateOfGame)
        {
            printBoard(i_GameRound, io_StateOfGame);
            while (io_StateOfGame == eGameModes.Play)
            {
                bool isQuit = promptUserForValidGuessingOrQuit(i_GameRound,out string userGuessing);

                if(!isQuit)
                {
                    i_GameRound.InsertNewGuessAndFeedbackCalculation(userGuessing);
                }

                updateStateOfTheGame(isQuit, out io_StateOfGame, i_GameRound);
                printBoard(i_GameRound, io_StateOfGame);
            }
        }

        private void updateStateOfTheGame(bool i_IsQuit, out eGameModes o_StateOfTheGame,
                                          GameLogic i_GameRound)
        {
            if (i_IsQuit)
            {
                o_StateOfTheGame = eGameModes.Quit;
            }
            else
            {
                if (i_GameRound.IsWin())
                {
                    o_StateOfTheGame = eGameModes.Win;
                }
                else
                {
                    o_StateOfTheGame = i_GameRound.IsTheGameOver() ? eGameModes.Loss : eGameModes.Play;
                }
            }
        }

        private bool promptUserForValidGuessingOrQuit(GameLogic i_GameLogic, out string o_UserGuessing)
        {
            bool isValidInput = false;
            bool isQuit = false;
            o_UserGuessing=string.Empty;
            
            while(!isValidInput && !isQuit)
            {
                Console.WriteLine("Please type your next guess <A B C D> or 'Q' to quit");
                o_UserGuessing = Console.ReadLine();
                isQuit = o_UserGuessing.Equals("Q");
                if(!isQuit)
                {
                    isValidInput = isSequenceValid(ref o_UserGuessing, i_GameLogic);
                }
            }

            return isQuit;
        }

        private bool isSequenceValid(ref string io_Sequence, GameLogic i_GameLogic)
        {
            bool result = checkingTemplateAndDeletingSpacesFromTheGuess(ref io_Sequence);

            if(result)
            {
                result = i_GameLogic.IsSequenceLegal(io_Sequence) && i_GameLogic.IsSequenceInLegalLength(io_Sequence)
                                                                 && i_GameLogic.IsSequenceLettersInRange(io_Sequence);
            }

            return result;
        }

        private bool checkingTemplateAndDeletingSpacesFromTheGuess(ref string io_Sequence)
        {
            bool result = true;
            StringBuilder sequenceWithoutSpaces = new StringBuilder();
            bool isEvenPlace = true;

            foreach(char letter in io_Sequence)
            {
                if(isEvenPlace)
                {
                    sequenceWithoutSpaces.Append(letter);
                }
                else if(letter != ' ')
                {
                    result = false;
                    break;
                }

                isEvenPlace = !isEvenPlace;
            }

            io_Sequence = sequenceWithoutSpaces.ToString();

            return result;
        }

        private void printBoard(GameLogic i_GameLogic, eGameModes i_GameMode)
        {
            StringBuilder boardOutput = new StringBuilder();

            boardOutput.Append(makeTopLabel(i_GameLogic.SizeOfSequence));
            boardOutput.Append(makeMainBoard(i_GameLogic, i_GameMode));
            Screen.Clear();
            Console.WriteLine(boardOutput);
        }

        private StringBuilder createSeparationLineInBoard(int i_NumOfCols)
        {
            StringBuilder separationLine = new StringBuilder();

            separationLine.AppendFormat("|");
            for(int i = 0; i < i_NumOfCols; i++)
            {
                separationLine.AppendFormat("==");
                if(i == i_NumOfCols / 2-1)
                {
                    separationLine.AppendFormat("=|");
                }
            }

            separationLine.AppendFormat("|{0}", Environment.NewLine);

            return separationLine;
        }

        private StringBuilder createGuessingLineInBoard(SequenceAndFeedback i_RowInBoard)
        {
            StringBuilder guessingLine = new StringBuilder();

            guessingLine.AppendFormat("|");
            foreach(char letter in i_RowInBoard.Sequence)
            {
                guessingLine.AppendFormat(" {0}", letter);
            }

            guessingLine.AppendFormat(" |");
            foreach (char letter in i_RowInBoard.Feedback)
            {
                guessingLine.AppendFormat("{0} ", letter);
            }

            guessingLine.AppendFormat("|{0}", Environment.NewLine);

            return guessingLine;
        }

        private StringBuilder makeMainBoard(GameLogic i_GameLogic, eGameModes i_GameMode)
        {
            StringBuilder mainBoard = new StringBuilder();
            GameBoard gameBoard = i_GameLogic.Board;
            int numOfCols = i_GameLogic.SizeOfSequence;
            StringBuilder separationLine = createSeparationLineInBoard(numOfCols*2);

            for(int i = 0; i < gameBoard.NumberOfRows; i++)
            {
                SequenceAndFeedback row = gameBoard.GetSequenceAndFeedbackInIndex(i);
                string sequence = (i == 0 && i_GameMode == eGameModes.Loss) ? i_GameLogic.WinningSequence : row.Sequence;
                StringBuilder guessingLine = createGuessingLineInBoard(new SequenceAndFeedback(sequence, row.Feedback));

                mainBoard.AppendFormat("{0}{1}", separationLine,guessingLine);
            }

            mainBoard.AppendFormat("{0}", separationLine);

            return mainBoard;
        }

        private StringBuilder makeTopLabel(int i_NumOfCols)
        {
            StringBuilder topLabel = new StringBuilder();
            string[] stringsOfTopTable = { "Current board status:", "|Pines:", "Result:" };
            int numOfSpaceInPinesCol = i_NumOfCols * 2 - stringsOfTopTable[1].Length;

            topLabel.AppendFormat("{0} {1}{2} ", stringsOfTopTable[0], Environment.NewLine, stringsOfTopTable[1]);
            for(int i = 0; i < numOfSpaceInPinesCol; i++)
            {
                topLabel.AppendFormat(" ");
            }

            topLabel.AppendFormat(" |{0}", stringsOfTopTable[2]);
            for(int i = 0; i < numOfSpaceInPinesCol; i++)
            {
                topLabel.AppendFormat(" ");
            }

            topLabel.AppendFormat("|{0}", Environment.NewLine);

            return topLabel;
        }

        private void printGoodBye()
        {
            Screen.Clear();
            Console.WriteLine(@"Thanks for playing,
GoodBye :)");
        }

        private int getValidNumOfGuessingFromUser()
        {
            Console.WriteLine("Please enter num of guessing - an integer between 4 to 10");
            string stringSizeInput = Console.ReadLine();

            return checkIfNumIsValidAndReceivesANumFromUserUntilTheNumIsValidAndConvertToInt(stringSizeInput);
        }

        private int checkIfNumIsValidAndReceivesANumFromUserUntilTheNumIsValidAndConvertToInt(string i_StringNumInput)
        {
            bool isValidNum = false;
            const int k_MinNum = 4;
            const int k_MaxNum = 10;
            int userInput = 0;

            while (!isValidNum)
            {
                bool isNum = int.TryParse(i_StringNumInput, out userInput);

                if (isNum)
                {
                    isValidNum = userInput >= k_MinNum && userInput <= k_MaxNum;
                }

                if (!isValidNum)
                {
                    Console.WriteLine("Please enter an integer between 4 to 10");
                    i_StringNumInput = Console.ReadLine();
                }
            }

            return userInput;
        }

        private void printWelcome()
        {
            string outPutScreen =
                @"  ___      _ _        _           _    ___ _         _      
 | _ )_  _| | |___   /_\  _ _  __| |  / __| |___ ___| |_ ___
 | _ \ || | | (_-<  / _ \| ' \/ _` | | (__| / -_) _ \  _(_-<
 |___/\_,_|_|_/__/ /_/ \_\_||_\__,_|  \___|_\___\___/\__/__/
                                                            ";
            Console.WriteLine(outPutScreen);
        }

    }
}
