using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raylib_cs;

namespace FlappyBirdMultiplayer
{
    class FlappyBirdGame
    {
        const int screenWidth = 800;
        const int screenHeight = 450;
        static Vector2 birdPos1 = new Vector2(100, 200);
        static Vector2 birdPos2 = new Vector2(150, 200);
        static float birdYVelocity1 = 0;
        static float birdYVelocity2 = 0;
        static List<Rectangle> pipes = new List<Rectangle>();
        static bool gameOver = false;
        static int score1 = 0;
        static int score2 = 0;

        public static void Main()
        {
            Raylib.InitWindow(screenWidth, screenHeight, "Flappy Bird Multiplayer");
            Raylib.SetTargetFPS(60);

            pipes.Add(new Rectangle(screenWidth, -100, 50, 300));
            pipes.Add(new Rectangle(screenWidth, 200, 50, 300));

            while (!Raylib.WindowShouldClose() && !gameOver)
            {
                if (Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE))
                {
                    birdYVelocity1 = -10;
                    birdYVelocity2 = -10;
                }

                birdYVelocity1 += 1;
                birdYVelocity2 += 1;
                birdPos1.y += birdYVelocity1;
                birdPos2.y += birdYVelocity2;

                if (birdPos1.y > screenHeight || birdPos1.y < 0) gameOver = true;
                if (birdPos2.y > screenHeight || birdPos2.y < 0) gameOver = true;

                foreach (var pipe in pipes)
                {
                    pipe.x -= 2;
                    if (pipe.x + pipe.width < 0)
                    {
                        pipe.x = screenWidth;
                        pipe.height = Raylib.GetRandomValue(100, 300);
                        pipe.y = -pipe.height + 300;
                    }

                    if (CheckCollisionRecs(new Rectangle(birdPos1.x, birdPos1.y, 20, 20), pipe)) gameOver = true;
                    if (CheckCollisionRecs(new Rectangle(birdPos2.x, birdPos2.y, 20, 20), pipe)) gameOver = true;

                    if (pipe.x + pipe.width < birdPos1.x && !pipe.Equals(pipes.First()))
                    {
                        score1++;
                    }
                    if (pipe.x + pipe.width < birdPos2.x && !pipe.Equals(pipes.First()))
                    {
                        score2++;
                    }
                }

                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.RAYWHITE);

                Raylib.DrawCircleV(birdPos1, 10, Color.RED);
                Raylib.DrawCircleV(birdPos2, 10, Color.BLUE);

                foreach (var pipe in pipes)
                {
                    Raylib.DrawRectangleRec(pipe, Color.GREEN);
                    Raylib.DrawRectangleRec(new Rectangle(pipe.x, pipe.y + pipe.height, pipe.width, screenHeight - (pipe.height + pipe.y)), Color.GREEN);
                }

                Raylib.DrawText("Player 1 Score: " + score1, 10, 10, 20, Color.BLACK);
                Raylib.DrawText("Player 2 Score: " + score2, 10, 40, 20, Color.BLACK);

                if (gameOver)
                {
                    Raylib.DrawText("GAME OVER", screenWidth / 2 - 50, screenHeight / 2 - 20, 40, Color.RED);
                }

                Raylib.EndDrawing();
            }

            Raylib.CloseWindow();
        }

        static bool CheckCollisionRecs(Rectangle rec1, Rectangle rec2)
        {
            return !(rec1.x >= rec2.x + rec2.width ||
                     rec1.x + rec1.width <= rec2.x ||
                     rec1.y >= rec2.y + rec2.height ||
                     rec1.y + rec1.height <= rec2.y);
        }
    }
}