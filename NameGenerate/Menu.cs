using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NameGenerate
{
    class Menu
    {
        List<string> options;
        List<int> state_throws;
        int x_start;
        int y_start;
        string title;

        public Menu()
        {

        }

        public Menu(string ptitle,int x,int y)
        {
            title = ptitle;
            x_start = x;
            y_start = y;
            options = new List<string>();
            state_throws = new List<int>();
            options.Add("Create 20 Words");
            state_throws.Add(2);
            options.Add("Clear Parameters");
            state_throws.Add(3);
            options.Add("Add Include Parameters");
            state_throws.Add(4);
            options.Add("Add Exclude Parameters");
            state_throws.Add(5);
            options.Add("Print A Chain");
            state_throws.Add(6);

            options.Add("Quit");
            state_throws.Add(50);
           
           
        }

        public int get_input()
        {
            byte selection = (byte)Console.ReadKey(true).KeyChar;
            if (selection >= 65 && selection <= 90)
            {
                selection -= 65;
            }
            else if (selection >= 97 && selection <= 122)
            {
                selection -= 97;
            }
            else
                return -1;
            if (selection < state_throws.Count)
            {
                //MarkhovBrain.cur_text = "You selected " + options[selection] + " throw case " + state_throws[selection];
                return state_throws[selection];
            }
            else
                return -1;
        }

        public void draw_menu()
        {
            Console.CursorLeft = x_start;
            Console.CursorTop = y_start;
            Console.Write(title);
            for (int x = 0; x < options.Count; x++)
            {
                Console.CursorLeft = x_start;
                Console.CursorTop = y_start + x + 1;
                Console.Write((char)(65 + x));
                Console.Write(". " + options[x]);
            }
        }
    }
}
