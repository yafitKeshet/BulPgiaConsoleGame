using System;
using System.Linq;
using System.Text;

namespace A22_Ex02_Yafit_318861960_Lonnie_316223411
{
    public class GameLogic
    {
        private readonly int r_SizeOfSequence = 4;
        private static Random s_Random = new Random();
        private readonly GameBoard r_Board;
        private string m_WinningSequence;
        private readonly string r_LettersInGame = "ABCDEFGH";

        public string WinningSequence
        {
            get
            {
                return m_WinningSequence;
            }
            set
            {
                m_WinningSequence = value;
            }
        }

        public GameLogic(int i_NumberOfRows)
        {
            r_Board = new GameBoard(i_NumberOfRows, r_SizeOfSequence);
        }

        public int SizeOfSequence
        {
            get
            {
                return r_SizeOfSequence;
            }
        }

        public GameBoard Board
        {
            get
            {
                return r_Board;
            }
        }

        public void NewGame()
        {
            r_Board.ClearBoard();
            m_WinningSequence = string.Empty;
        }

        public bool IsSequenceInLegalLength(string i_Sequence)
        {
            return i_Sequence.Length == r_SizeOfSequence;
        }

        public bool IsSequenceLettersInRange(string i_Sequence)
        {
            bool result = true;

            foreach(char letter in i_Sequence)
            {
                if(!r_LettersInGame.Contains(letter))
                {
                    result = false;
                }
            }

            return result;
        }

        public bool IsSequenceLegal(string i_Sequence)
        {
            bool result = true;

            foreach (char letter in i_Sequence)
            {
                int count = 0;

                foreach (char c in i_Sequence)
                {
                    if(c == letter)
                        count++;
                }

                if(count > 1)
                {
                    result = false;
                }
            }

            return result;
        }

        public string RandWinningSequence()
        {
            string randomSequence= string.Empty;
            
            for (int i = 0; i < SizeOfSequence; i++)
            {
                char randLetter = r_LettersInGame[s_Random.Next(r_LettersInGame.Length)];

                while(randomSequence.Contains(randLetter))
                {
                    randLetter = r_LettersInGame[s_Random.Next(r_LettersInGame.Length)];
                }

                randomSequence += randLetter;
            }

            return randomSequence;
        }

        public void InsertNewGuessAndFeedbackCalculation(string i_GuessSequence)
        {
            string feedback;

            if (r_Board.GetFirstEmptyRow() != 0)
            {
                calculateAndCreateFeedback(i_GuessSequence, out feedback);
            }
            else
            {
                feedback = null;
            }

            r_Board.InsertNewLineToBoard( feedback, i_GuessSequence);
        }

        private void calculateAndCreateFeedback(string i_Sequence, out string o_Feedback)
        {
            calculateNumOfCharactersInTheSameIndexAndNumOfCharactersInIncorrectIndex(
                i_Sequence,
                out int numOfCharactersInCorrectIndex,
                out int numOfCharactersInIncorrectIndex);
            o_Feedback= getFeedback(numOfCharactersInCorrectIndex, numOfCharactersInIncorrectIndex, i_Sequence.Length);
        }

        private void calculateNumOfCharactersInTheSameIndexAndNumOfCharactersInIncorrectIndex(string i_Sequence,
            out int o_NumOfCharactersInCorrectIndex, out int o_numOfCharactersInIncorrectIndex)
        {
            o_NumOfCharactersInCorrectIndex = 0;
            o_numOfCharactersInIncorrectIndex = 0;
            for (int i = 0; i < i_Sequence.Length; i++)
            {
                if (m_WinningSequence.Contains(i_Sequence[i]))
                {
                    if (i_Sequence[i] == m_WinningSequence[i])
                    {
                        o_NumOfCharactersInCorrectIndex++;
                    }
                    else
                    {
                        o_numOfCharactersInIncorrectIndex++;
                    }
                }
            }

        }

        private string getFeedback(int i_NumOfCharactersInCorrectIndex, int i_NumOfCharactersInIncorrectIndex, int i_SizeOfFeedback)
        {
            StringBuilder feedback = new StringBuilder();

            for (int i = 0; i < i_SizeOfFeedback; i++)
            {
                if (i_NumOfCharactersInCorrectIndex > 0)
                {
                    feedback.Append("V");
                    i_NumOfCharactersInCorrectIndex--;
                }
                else if (i_NumOfCharactersInIncorrectIndex > 0)
                {
                    feedback.Append("X");
                    i_NumOfCharactersInIncorrectIndex--;
                }
                else
                {
                    feedback.Append(" ");
                }
            }

            return feedback.ToString();
        }

        public bool IsWin()
        {
            bool isWin = false;

            if(r_Board.GetFirstEmptyRow() != 1)
            {
                string feedback = r_Board.GetLastGuess();
                isWin=feedback.Equals(m_WinningSequence);
            }

            return isWin;
        }

        public bool IsTheGameOver()
        {
            return r_Board.IsBoardFull();
        }

    }
}
