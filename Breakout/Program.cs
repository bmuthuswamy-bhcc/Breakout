/*
 * Breakout game: use a paddle to direct a ball to break a 2D grid of bricks
 * Original reference: https://github.com/NikolayIT/CSharpConsoleGames
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
 * Paddle,
 * Ball
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
using System.Collections.Generic;
using System.Threading;

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
         * Paddle:
         * 1.  Visual representation:
         *        -------
         * 2.  Underlying actions:
         *   a.  Ball reflects off paddle (physics engine and ball, not related to the paddle)
         *   b.  Paddle should be user controllable and move only x direction!
         *   
         * Ball:
         * 1. Visual representation: @
         * 2. Underlying actions + 
         * data structure for the ball path (for physics engine, need to keep track of the previous ball 
         * position.  A natural data structure is a Queue)
         * 
         */

        // Bricks
        private const int MAX_WIDTH = 40;
        private const int NUM_COLS = 10;
        private const int NUM_ROWS = 4;
        private static bool[,] bricks = new bool[NUM_ROWS, NUM_COLS];
        
        private static int paddleSize = 7;
        private static int paddleX = 7; // starting position of the paddle

        private static int ballX = 10; // starting (x,y) coordinates of the ball
        private static int ballY = 15;
        private static int ballDirectionX = -1;
        private static int ballDirectionY = 1;

        private static bool GameOver = false;
           

        static Queue<int[]> currentBallPosition = new Queue<int[]>(); // store array of (x,y)

        private static void DrawCharacterAtPosition(char symbol, int x, int y) {
            Console.SetCursorPosition(x, y);
            Console.Write(symbol);
        }

        private static void DrawBall() {
            DrawCharacterAtPosition('@', ballX, ballY);
            currentBallPosition.Enqueue(new int[] { ballX, ballY });
        }

        private static void RemoveBall() {
            // need to make sure that the ballPath Queue is not empty!
            int[] ballPosition;
            while (currentBallPosition.Count > 0) {
                ballPosition = currentBallPosition.Dequeue();
                DrawCharacterAtPosition(' ', ballPosition[0], ballPosition[1]);
            }
        }

        private static void PhysicsEngine() {
            ballX += ballDirectionX;
            ballY += ballDirectionY;
            // Collision detection
            // Bricks @TODO(students):  finish this as part of your final project

            // top
            if (ballY == 0) {
                ballDirectionY = 1;
            }

            // sides. @TODO(students):  resolve the bug with the right side of the screen
            if ((ballX == MAX_WIDTH) || (ballX == 0)) {
                ballDirectionX *= -1;
            }
            // paddle: 
            if (ballY >= Console.WindowHeight - 10) { // ball is at paddle y coordinate
                // has the ball actually hit the paddle?
                if ((paddleX <= ballX) && (paddleX + paddleSize >= ballX)) {
                    ballDirectionY *= -1;
                }
                else {
                    GameOver = true;
                }
            }
        }
        
        private static void DrawPaddle() {
            for(int x = paddleX; x < paddleX + paddleSize;x++) {
                DrawCharacterAtPosition('-', x, Console.WindowHeight - 10);
            }
        }

        private static void RemovePaddle() {
            for (int x = paddleX; x < paddleX + paddleSize; x++) {
                DrawCharacterAtPosition(' ', x, Console.WindowHeight - 10);
            }
        }

        private static void PaddleLeft() {
            if (paddleX > 0) {
                paddleX--;
            }
        }

        private static void PaddleRight() {
            /*
             * paddleX is the x coordinate of the first character in your paddle:
             * paddleX -> '-'------
             */
            if (paddleX < MAX_WIDTH - paddleSize) {
                paddleX++;
            }
        }


        /* @TODO(students):  we need to dynamically update the bricks as the
         * ball collides with them.  For now, we will just statically draw them and
         * then workout the algorithm for dynamic update
         */
        private static void DrawBricks() {
            for (int y = 0; y < NUM_ROWS; y++) {
                for (int x = 0; x < NUM_COLS; x++) {
                    bricks[y, x] = true;
                    // @TODO(students):  Need to replace Write below with DrawCharacterAtPosition                   
                    Console.Write("[==]");
                } // NUM_COLS
                Console.WriteLine();
            } // NUM_ROWS           
        }
        private static void SetupGamingGrid() {
            /* GamingGrid will have the actual play area
             * and status (player lives, # of balls left, score)
             * @TODO(students):  Finish setting up the GamingGrid as 
             * part of your final project solution
             * 
             */
            Console.Title = "[[[Breakout BHCC CSC 287 Spring 2021]]]";
            Console.WindowWidth = MAX_WIDTH;
            Console.BufferWidth = MAX_WIDTH;
            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.Yellow;            
        }
        private static void Initialize() {
            Program.SetupGamingGrid();
            Program.DrawBricks();
            Program.DrawPaddle();
            Program.DrawBall();
        }        

        static void Main(string[] args) {
            Program.Initialize();

            // Gaming loop
            while(!GameOver) {
                if (Console.KeyAvailable) {
                    ConsoleKeyInfo keyInfo = Console.ReadKey();
                    if (keyInfo.Key == ConsoleKey.Q) {
                        break;
                    }
                    RemovePaddle();                    
                    if (keyInfo.Key == ConsoleKey.LeftArrow) {                        
                        PaddleLeft();
                    }
                    if (keyInfo.Key == ConsoleKey.RightArrow) {
                        PaddleRight();
                    }
                    DrawPaddle();
                }
                RemoveBall();
                PhysicsEngine(); // @TODO(Bharath):  Move this into a separate thread for proper Console refresh
                DrawBall();
                Thread.Sleep(500); 
            }
        }
    }
}
