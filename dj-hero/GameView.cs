﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dj_hero
{
    public class GameView : View
    {
        private ViewElement timer;
        private ViewElement progressBar;
        private ViewElement[] characters;

        private bool[,] vacancy;

        private int characterIndex;
        private int charactersNo;

        protected ViewElement points;

        public GameView()
        {
            characterIndex = 0;
            charactersNo = 3;

            characters = new ViewElement[charactersNo];
            vacancy = new bool[Console.WindowHeight, Console.WindowWidth];

            timer = new ViewElement(Console.WindowWidth - 8, 1, 5, 3,
                new List<string>()
                {
                    "TIMER",
                    "",
                    "00:00"
                });
            progressBar = new ViewElement(3, 1, 25, 5, new List<string>() { "" });
            points = new ViewElement((Console.WindowWidth - 2) / 2, 1, 5, 1, new List<string>() { "0" });

            Elements.Add("ProgressBar", progressBar);
            Elements.Add("Points", points);
            Elements.Add("Timer", timer);
            InitCharacters();
        }

        private void LockPos(ViewElement element)
        {
            if(element.PosX + element.Width <= Console.WindowWidth && element.PosY + element.Height <= Console.WindowHeight)
            {
                for(int i = 0; i < element.Height; i++)
                {
                    for(int j = 0; j < element.Width; j++)
                    {
                        vacancy[element.PosY + i, element.PosX + j] = false;
                    }
                }
            }
        }

        private void ReleasePos(ViewElement element)
        {
            if (element.PosX + element.Width <= Console.WindowWidth && element.PosY + element.Height <= Console.WindowHeight)
            {
                for (int i = 0; i < element.Height; i++)
                {
                    for (int j = 0; j < element.Width; j++)
                    {
                        vacancy[element.PosY + i, element.PosX + j] = true;
                    }
                }
            }
        }

        private void InitCharacters()
        {
            for(int i = 0; i < charactersNo; i++)
            {
                characters[i] = new ViewElement(-1, -1, 16, 7, new List<string>() { "" });
                Elements.Add("Character" + i, characters[i]);
            }

            for(int i = 3; i < 30; i++)
            {
                for(int j = 10; j < 120; j++)
                {
                    vacancy[i, j] = true;
                }
            }
        }
        
        public void DisplayTime(int time)
        {
            int minutes = time / 60;
            int seconds = time % 60;

            string sTime = minutes < 10 ? "0" + minutes.ToString() : minutes.ToString();
            sTime += ":";
            sTime += seconds < 10 ? "0" + seconds.ToString() : seconds.ToString();

            Elements["Timer"].Lines[2] = sTime;
            Elements["Timer"].Update(2);
        }

        public void DisplayProgressBar(int percent)
        {
            string ret = "[";
            for (int i = 0; i < 25; i++)
            {
                if (percent / 4 > i)
                    ret = ret + "|";
                else
                    ret = ret + " ";
            }
            ret = ret + "]";

            Elements["ProgressBar"].Lines[0] = ret;

            if (percent <= 30)
            {
                Elements["ProgressBar"].ForegroundColor = ConsoleColor.Red;
            }
            else if (percent <= 70)
                Elements["ProgressBar"].ForegroundColor = ConsoleColor.Yellow;
            else
                Elements["ProgressBar"].ForegroundColor = ConsoleColor.Green;

            Elements["ProgressBar"].Update();
        }

        public void DisplayPoints(int points)
        {
            Elements["Points"].Lines[0] = points.ToString();
            Elements["Points"].Update();
        }


        public void Add(AppearingChar character)
        {
            Random rand = new Random();
            List<string> lines = new List<string>(Ascii[character.character.ToString()]);
            lines.Add(character.counter.ToString());
            bool vac = true;
            do
            {
                vac = true;
                character.PosX = rand.Next(10, 102);
                character.PosY = rand.Next(3, 24);
                for(int i = 0; i < 7; i++)
                {
                    for(int j = 0; j < 16; j++)
                    {
                        if(vacancy[character.PosY + i, character.PosX + j] == false)
                        {
                            vac = false;
                            break;
                        }
                    }

                    if (vac == false)
                        break;
                }
            } while (vac == false);

            Elements["Character" + characterIndex % 3].Clear();
            if(characterIndex > 3)
                ReleasePos(Elements["Character" + characterIndex % 3]);
            Elements["Character" + characterIndex % 3].PosX = character.PosX;
            Elements["Character" + characterIndex % 3].PosY = character.PosY;
            LockPos(Elements["Character" + characterIndex % 3]);
            Elements["Character" + characterIndex % 3].Lines = lines;
            if(characterIndex == 0)
            {
                Elements["Character" + characterIndex % 3].ForegroundColor = ConsoleColor.Green;
                Elements["Character" + characterIndex % 3].Update();
            }
            else
            {
                Elements["Character" + characterIndex % 3].ForegroundColor = ConsoleColor.White;
                Elements["Character" + characterIndex % 3].Update();
                Elements["Character" + (characterIndex + 1) % 3].ForegroundColor = ConsoleColor.Green;
                Elements["Character" + (characterIndex + 1) % 3].Update();
                Elements["Character" + (characterIndex + 2) % 3].ForegroundColor = ConsoleColor.Blue;
                Elements["Character" + (characterIndex + 2) % 3].Update();
            }

            characterIndex++;
        }


        public void UpdateCharacter(AppearingChar character)
        {
            if (character.counter == 0)
            {
                Elements["Character" + (characterIndex + 1) % 3].Clear();
            }
            else
            {
                Elements["Character" + characterIndex % 3].Lines[6] = character.counter.ToString();
                Elements["Character" + characterIndex % 3].ForegroundColor = ConsoleColor.Green;
                Elements["Character" + characterIndex % 3].Update();
            }
        }

        public void DisplayEndGame()
        {
            Console.Clear();
            Console.WriteLine("THATS IT! GAME OVER, MAN! GAME OVER! ");
        }
        
    }
}