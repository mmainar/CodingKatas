using System;

namespace SudokuSolver
{
    class Program
    {
        static int BLANK = 0; // Square is empty
        static int ONES = 0x3fe; 	// Binary 1111111110 (representing numbers [1..9])

        static int[] InBlock, InRow, InCol;       
        static int[] Entry;	// Records entries 1-9 in the grid, as the corresponding bit set to 1
        static int[] Block, Row, Col;	// Each int is a 9-bit array

        static int SeqPtr;
        static int[] Sequence;

        static int Count;
        static int[] LevelCount;

        static int Main(string[] args)
        {
            // Create arrays
            InBlock = new int[81];
            InRow = new int[81];
            InCol = new int[81];
            Entry = new int[81];
            Sequence = new int[81];
            LevelCount = new int[81];

            Block = new int[9];
            Row = new int[9];
            Col = new int[9];

            // Init arrays
            Console.WriteLine("Initialising arrays");
            int Square;
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    Square = 9 * row + col;
                    InRow[Square] = row;
                    InCol[Square] = col;
                    InBlock[Square] = (row / 3) * 3 + (col / 3);
                }
            }

            for (Square = 0; Square < 81; Square++)
            {
                Sequence[Square] = Square;
                // Initially blank
                Entry[Square] = BLANK; 
                LevelCount[Square] = 0;
            }

            // Mark every number in the range 1..9 as possible for every Block (Region), Row and Column.
            for (int i = 0; i < 9; i++)
            {
                Block[i] = Row[i] = Col[i] = ONES;
            }
            Console.WriteLine("Arrays initialised");


            SeqPtr = 0; Count = 0;
            ConsoleInput();

            Console.WriteLine("Starting at SeqPtr {0}", SeqPtr);
            Place(SeqPtr);

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Total Count = {0}", Count);

            return 0;
        }

        static void ConsoleInput()
        {
             for (int row = 0; row < 9; row++) {
                 Console.Write("Row[{0}] : ", row + 1);
                 string InputString = Console.ReadLine();

                 for (int col = 0; col < 9; col++) 
                 {
                     char ch = InputString[col];
                     if (Char.IsDigit(ch))
                     {
                         InitEntry(row, col, ch - '0');
                     }
                 }
             }

             PrintArray();
        }

        static void InitEntry(int row, int col, int val)
        {
            int Square = 9 * row + col;
            int valbit = 1 << val; // Number represented as the corresponding bit set to 1 (rest are 0s)           

            // add suitable checks for data consistency
            // Set number on this entry
            Entry[Square] = valbit;

            // This number won't be eligible neither on the block of this entry nor on its column nor on its row 
            // (sudoku rules: a number has to be unique on its row, column and block).
            // We perform the AND of the current value and the valbit negated. This clears the only bit set on valbit
            // and leaves the rest of the bits as they were before.
            Block[InBlock[Square]] &= ~valbit;
            Col[InCol[Square]] &= ~valbit; // Simpler Col[col] &= ~valbit;
            Row[InRow[Square]] &= ~valbit; // Simpler Row[row] &= ~valbit;

            // Not very sure yet what SeqPtr is used for
            // This is sorting the seq array by the square numbers that we'll try to solve
            int SeqPtr2 = SeqPtr;
            while (SeqPtr2 < 81 && Sequence[SeqPtr2] != Square)
            {
                SeqPtr2++;
            }

            SwapSeqEntries(SeqPtr, SeqPtr2);
            SeqPtr++;
        }

        static void SwapSeqEntries(int S1, int S2)
        {
            int temp = Sequence[S2];
            Sequence[S2] = Sequence[S1];
            Sequence[S1] = temp;
        }

        // Choose the next cell to guess which minimises the fanout
        static int NextSeq(int S)
        {
            int nextSeq = 0;
            int MinBitCount = 100; // will always be less than this

            for (int T = S; T < 81; T++)
            {
                int Square = Sequence[T];
                // Possible numbers: AND to get a number containing only the bits set to 1 that are
                // common for Block, Row and Col of this square
                int Possibles = Block[InBlock[Square]] & Row[InRow[Square]] & Col[InCol[Square]];

                // Count the number of bits set to 1 on Possibles
                int BitCount = 0;
                while (Possibles != 0)
                {
                    Possibles &= ~(Possibles & -Possibles);
                    BitCount++;
                }

                // Get the square/cell with the min possible numbers
                if (BitCount < MinBitCount)
                {
                    MinBitCount = BitCount;
                    nextSeq = T;
                }
            }

            return nextSeq;
        }

        // Runs main Backtracking search
        static void Place(int S)
        {
            if (S >= 81)
            {
                // All the squares have been placed (9x9)
                Succeed();
                return;
            }

            // Count levels of recursion on this square 
            // and total levels of recursion
            LevelCount[S]++;
            Count++;

            // Get the next cell to guess which minimises the fanout
            int S2 = NextSeq(S);
            SwapSeqEntries(S, S2);

            // Get tue value of the Square/Cell at this min fanout
            int Square = Sequence[S];

            // Get Block, Row and Col indices for the square.
            int BlockIndex = InBlock[Square],
                    RowIndex = InRow[Square],
                    ColIndex = InCol[Square];

            int Possibles = Block[BlockIndex] & Row[RowIndex] & Col[ColIndex];
            while (Possibles != 0)
            {
                //Gets the highest power of two that divides x 
                int valbit = Possibles & (-Possibles); // Lowest 1 bit in Possibles
                Possibles &= ~valbit;
                // Place that number on the square
                Entry[Square] = valbit;

                // That number is not valid anymore for the Block, Row and Col of the square.
                Block[BlockIndex] &= ~valbit;
                Row[RowIndex] &= ~valbit;
                Col[ColIndex] &= ~valbit;

                // Place the next square
                Place(S + 1);

                // Keep going to find all possible solutions
                // Set the Entry again to BLANK as initially.
                Entry[Square] = BLANK; // Could be moved out of the loop

                // Make the number available again for the Block, Row and Col.
                Block[BlockIndex] |= valbit;
                Row[RowIndex] |= valbit;
                Col[ColIndex] |= valbit;
            }

            // Undo the previous swap on Sequence array
            SwapSeqEntries(S, S2);
        }
        static void Succeed()
        {
            PrintArray();
            PrintStats();
        }

        static void PrintArray()
        {
            char ch = ' ';
            int Square = 0;

            for (int row = 0; row < 9; row++)
            {
                if (row % 3 == 0)
                {
                    Console.WriteLine();
                }
                for (int col = 0; col < 9; col++)
                {
                    if (col % 3 == 0)
                    {
                        Console.Write(' ');
                    }
                    int valbit = Entry[Square++];
                    if (valbit == 0)
                    {
                        // No number
                        ch = '-';
                    }
                    else
                    {
                        // Check the number
                        for (int val = 1; val <= 9; val++)
                            if (valbit == (1 << val))
                            {
                                if (!Char.TryParse(val.ToString(), out ch))
                                {
                                    throw new Exception(string.Format("Could not parse {0} to char", val));
                                }
                                break;
                            }
                    }
                    Console.Write(ch);
                }
                Console.WriteLine();
            }
        }

        static void PrintStats()
        {
            Console.WriteLine();
            Console.WriteLine("Level Counts:");
            Console.WriteLine();
            Console.WriteLine();

            int S = 0;
            while (LevelCount[S] == 0)
            {
                S++;
            }

            int i = 0;
            while (S < 81)
            {
                int Seq = Sequence[S];
                int row = Seq / 9 + 1; // + 1 since 0 based
                int col = Seq % 9 + 1;
                Console.Write("({0}, {1}):{2} ", row, col, LevelCount[S]);
                if (i++ > 4)
                {
                    Console.WriteLine();
                    i = 0;
                }
                S++;
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Count = {0}", Count);
        }
    }
}
