using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fruit_Game.Controllers
{
    public class HomeController : Controller
    {
        static int rowsCount = 3; //Rows
        static int colsCount = 9; //Columns
        static string[,] fruits = GenerateRandomFruits(); //Matrix with fruits (game field)
        static int score = 0; //Points accumulated by the player
        static bool gameOver = false; //Checks if the game has ended

        public ActionResult Index() //Preparing the game field by adding the game elements in the ViewBag structure and uses the view
        //that puts them on the page of the game as HTML in the web browser.
        {
            ViewBag.rowsCount = rowsCount;
            ViewBag.colsCount = colsCount;
            ViewBag.fruits = fruits;
            ViewBag.score = score;
            ViewBag.gameOver = gameOver;
            return View();
        }

        public ActionResult Reset() //Starts a new game by generating new random game field 
            //with fruits and dynamites and resets the player's points.
        {
            fruits = GenerateRandomFruits();
            gameOver = false;
            score = 0;
            return RedirectToAction("Index"); //Redirects to method Index()
        }

        public ActionResult FireTop(int position)//Shoots row 0 on position (number from 0 to 100).
        //Shoots downwards (+1) from row 0 (the top row).
        {
            return Fire(position, 0, 1);
        }

        public ActionResult FireBottom(int position)//Shoots row 2 on position (number from 0 to 100).
        //Shoots upwards (-1) from row 2 (the bottom row).
        {
            return Fire(position, rowsCount - 1, -1);
        }

        public ActionResult Fire(int position, int startRow, int step)
        {
            var col = position * (colsCount - 1) / 100; //Calculates the number of the column 
            //to which the player is aiming.
            //The input number from the scrollbar (between 0 and 100) is reduced to a
            //number between 0 and 8 (for each of the nine columns).
            var row = startRow;  //The number of the row is either 0 (if the shooting is downwards)
            //or the number of the rows minus one (if the shooting is upwards).
            //The direction of the shooting is 1 (downwards) or -1 (upwards).

            while (row >= 0 && row < rowsCount) //In order to find where the shot hits a fruit or a dynamite, a loop is used to go through
            //all the cells in the game field in the targeted column and from the first to the last attacked row.
            {
                var fruit = fruits[row, col];
                if (fruit == "apple" || fruit == "banana" || fruit == "orange" || fruit == "kiwi")
                    //If a fruit is encountered, it disappears (it's replaced with empty) and points are given to the player.
                {
                    score++;
                    fruits[row, col] = "empty";
                    break;
                }
                else if (fruit == "dynamite")
                //If a dynamite is encountered, the game ends.
                {
                    gameOver = true;
                    break;
                }
                row = row + step;
            }
            return RedirectToAction("Index"); //Redirects to method Index()
        }

        static string[,] GenerateRandomFruits() //Generating random fruits
         //This code adds the names of random pictures with fruits in the matrix and builds the game field.
         //In every cell of the matrix is added one of the following values: apple, banana, orange, kiwi, 
         //empty or dynamite.
        {
            Random rand = new Random(); //Creating object Random
            fruits = new string[rowsCount, colsCount];
            for (var row = 0; row < rowsCount; row++)
            {
                for (var col = 0; col < colsCount; col++)
                {
                    var r = rand.Next(9);//To generate random fruits,
                    //for every cell must be generated random number between 0 and 8.
                    if (r < 2) //If the number is 0 or 1, put apple.
                    {
                        fruits[row, col] = "apple";
                    }
                    else if (r < 4) //If the number is 2 or 3, put banana.
                    {
                        fruits[row, col] = "banana";
                    }
                    else if (r < 6) //If the number is 4 or 5, put orange.
                    {
                        fruits[row, col] = "orange";
                    }
                    else if (r < 8) //If the number is 6 or 7, put kiwi.
                    {
                        fruits[row, col] = "kiwi";
                    }
                    else //If the number is 8, put dynamite.
                    {
                        fruits[row, col] = "dynamite";
                    }
                }
            }
            return fruits;
        }
    }
}
