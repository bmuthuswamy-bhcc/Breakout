/*
 * Breakout game: use a paddle to direct a ball to break a 2D grid of bricks
 * 
 * I recommend that you do a simple single screen 2D game like breakout (or tetris) to understand
 * the concepts of C# that we covered in class.
 * 
 * I do not recommend that you do something like Augmented Reality on a mobile platform
 * because essentially that uses prebuilt designs : you really have to get the prebuilt
 * designs (that is the framework) working correctly, before you get into 
 * computer programming concepts.
 * 
 * STEP 1: Devise a plan: 
 * Using a FUNCTIONAL PROGRAMMING approach.
 * 1.  We did discuss that we have three elements (Actors) in our game:
 * Bricks,
 * Ball,
 * Paddle
 * 
 * 2.  Game world and "engine":  our Console Window, 
 *     keep track of the player's "lives", # of balls, score
 * 
 * 3.  User I/O:  keyboard control
 * 
 * 4.  Physics engine (handle collision, gravity)
 *  
 * STEP 2: Carryout the plan
 * Note that the title is MUTABLE in the sense that actions (such as clicking in
 * the console window) modifies the title (prepends the word "Select" when clicking in the
 * console window). 
 * So, when the program terminates, the debugger completely erases our title.  We could
 * configure the debugger to not change the title, but this looks a "rabbit-hole" that we
 * don't want to go down (i.e..computer science "proverb" - don't want to lose sight of the
 * forest for its trees).  Simple solution of having the user press a key to quit the program
 * (which is going to be default Game behavior anyway) is a good solution.
 * 
 * For other details of the design, please refer to the in-code comments.  Note that
 * in the "real world", you would complete a design document and test plan FIRST, get feedback
 * in design review(s) and THEN start implementation!  But, due to the lack of time in the college
 * course, I am giving you the design steps in the context of C# concepts.
 */

using System;

namespace Breakout {
    class Program {
        // RECALL: show me your data and I will show you the program.
        /* Bricks: 
         * 1.  Visual representation:  
        *        [==] simple 4 char: brick exist
        *       '    '  simple 4 char spaces: brick does not exist!
        *       2 states => use bool!
         * 2.  Underlying data structure:  2D ARRAY (size is fixed at compile time:  this is
         * ok, because we are not going to grow (shrink) brick area dynamically - DESIGN DECISION).
         *     [==][==][==][==][==][==][==][==][==][==]
         *     [==][==][==][==][==][==][==][==][==][==]
         *     [==][==][==][==][==][==][==][==][==][==]
         *     [==][==][==][==][==][==][==][==][==][==]           
         * 
         */

        private const int NUM_COLS = 10;
        private const int NUM_ROWS = 4;
        private static bool[,] bricks = new bool[NUM_ROWS, NUM_COLS];
        
        /* @TODO(Bharath):  we need to dynamically update the bricks as the
         * ball collides with them.  For now, we will just statically draw them and
         * then workout the algorithm for dynamic update
         */
        private static void DrawBricks() {
            for (int row = 0; row < NUM_ROWS; row++) {
                for (int col = 0; col < NUM_COLS; col++) {
                    bricks[row, col] = true;
                    if (bricks[row, col]) {
                        Console.Write("[==]");
                    }                   
                } // NUM_COLS
                Console.WriteLine();
            } // NUM_ROWS
        }
        private static void SetupGamingGrid() {
            /* GamingGrid will have the actual play area
             * and status (player lives, # of balls left, score)
             * 
             */
            Console.Title = "[[[Breakout BHCC CSC 287 Spring 2021]]]";
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Foreground color is: {Console.ForegroundColor}");
        }
        private static void Initialize() {
            Program.SetupGamingGrid();
            Program.DrawBricks();
        }
        static void Main(string[] args) {
            Program.Initialize();
            Console.ReadKey(true);
        }
    }
}
