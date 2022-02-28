namespace A22_Ex02_Yafit_318861960_Lonnie_316223411
{
    public class GameBoard
    {
        private readonly SequenceAndFeedback[] r_Board;
        private int m_FirstEmptyRowToInsert;

        public GameBoard(int i_Rows, int i_SizeOfSequenceAndFeedback)
        {
            r_Board = new SequenceAndFeedback[i_Rows];

            for (int i = 0; i < NumberOfRows; i++)
            {
                r_Board[i].Sequence = new string(' ', i_SizeOfSequenceAndFeedback);
                r_Board[i].Feedback = new string(' ', i_SizeOfSequenceAndFeedback);
            }

            ClearBoard();
        }

        public void ClearBoard()
        {
            m_FirstEmptyRowToInsert = 0;

            for (int i = 0; i < NumberOfRows; i++)
            {
                r_Board[i].Feedback = new string(' ', r_Board[i].Feedback.Length);
                r_Board[i].Sequence = new string(' ', r_Board[i].Sequence.Length);
            }
        }

        public int NumberOfRows
        {
            get
            {
                return r_Board.Length;
            }
        }

        public int GetFirstEmptyRow()
        {
            return m_FirstEmptyRowToInsert;
        }

        public void InsertNewLineToBoard(string i_Feedback, string i_Sequence)
        {
            if(m_FirstEmptyRowToInsert != 0)
            {
                r_Board[m_FirstEmptyRowToInsert].Feedback = i_Feedback;
            }

            r_Board[m_FirstEmptyRowToInsert].Sequence = i_Sequence;
            m_FirstEmptyRowToInsert++;
        }

        public bool IsBoardFull()
        {
            return m_FirstEmptyRowToInsert >= NumberOfRows;
        }

        public string GetLastGuess()
        {
            return GetSequenceAndFeedbackInIndex(m_FirstEmptyRowToInsert - 1).Sequence;
        }

        public SequenceAndFeedback GetSequenceAndFeedbackInIndex(int i_Index)
        {
            return r_Board[i_Index];
        }
    }
}
