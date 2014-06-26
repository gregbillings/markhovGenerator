using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NameGenerate
{
    class MarkhovBrain
    {
        Random rand = new Random((int)System.DateTime.Now.Ticks);
        
        List<KeyValuePair<string,List<Markhov>>> markhov_chains;
        public int num_add = 0;
        int frame_number = 0;
        int state;
        public static string cur_text = "What would you like to do?";
        string last_text = "";
        Menu main_menu = new Menu("Main Menu", 50, 0);
        string[] cur_words = new string[0];
        string include_piece = "";
        string exclude_piece = "";
        string[] include;
        string[] exclude;

        public MarkhovBrain()
        {

        }

        public MarkhovBrain(System.IO.StreamReader r)
        {
            markhov_chains = new List<KeyValuePair<string, List<Markhov>>>();
            string temp_break_down = "";
            while (!r.EndOfStream)
            {
                temp_break_down = r.ReadLine().ToLower();
                add_string_to_chain(temp_break_down);

            }
        }

        public void program_loop()
        {
            bool cont = true;
            while (cont)
            {
                frame_number++;
                if (Console.KeyAvailable)
                {
                    switch (state)
                    {
                        case -1:
                            cur_text = "Unknown input.";
                            state = 0;
                            break;
                        case 0: state = main_menu.get_input();
                            break;
                        case 50:
                            break;
                        default:
                            break;
                    }
                }

                switch (state)
                {
                    case 2: //create 20 words
                        cur_words = new string[20];
                        Console.CursorLeft = 0;
                        Console.CursorTop = 0;
                        include = include_piece.Split(',');
                        exclude = exclude_piece.Split(',');
                        Console.WriteLine("Words Created:".PadRight(40,' '));
                        int num_fail = 0;
                        int cur_word = 0;
                        string temp_word = "";
                        while (num_fail < 1000000 && cur_word < cur_words.Length)
                        {
                            temp_word = create_name();
                            if (name_acceptable(temp_word))
                            {
                                cur_words[cur_word++] = temp_word;
                                Console.CursorLeft = 1;
                                Console.CursorTop = cur_word;
                                Console.Write(cur_words[cur_word-1]);
                            }
                            else
                            {
                                num_fail++;
                            }
                        }
                        if (cur_word < cur_words.Length)
                        {
                            Console.CursorLeft = 1;
                            Console.CursorTop = 1 + cur_word;
                            Console.Write("Failed after " + num_fail + " attempts.");
                        }
                        else
                        {
                            Console.CursorLeft = 1;
                            Console.CursorTop = 1 + cur_word;
                            Console.Write("Failed " + num_fail + " times.");
                        }
                        Console.WriteLine("");
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.Write("Add to output.txt? Y/N: ");
                        char res = Console.ReadKey().KeyChar;
                        Console.WriteLine("");
                        if (res == 'Y' || res == 'y')
                        {
                            System.IO.FileStream fs = new System.IO.FileStream(@AppDomain.CurrentDomain.BaseDirectory + "/output.txt", System.IO.FileMode.Append);
                            System.IO.StreamWriter w = new System.IO.StreamWriter(fs);
                            for (int x = 0; x < cur_word; x++)
                            {
                                w.WriteLine(cur_words[x]);
                            }
                            w.Close();
                        }
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        last_text = "";
                        state = 0;
                        break;
                    case 3: //clear
                        include_piece = "";
                        exclude_piece = "";
                        state = 0;
                        last_text = "";
                        break;
                    case 4: //include
                        Console.Clear();
                        Console.WriteLine("Setting include groups. (This will set, not update includes) Previous: "+include_piece);
                        Console.WriteLine("Words generated without include groups will be rejected.");
                        Console.WriteLine("Multiple requirements may be made by using commas. e.g. rt,n");
                        Console.WriteLine("Type your includes, then press enter.");
                        include_piece = Console.ReadLine();
                        state = 0;
                        last_text = "";
                        break;
                    case 5: //exclude
                        Console.Clear();
                        Console.WriteLine("Setting exclude groups. (This will set, not update excludes) Previous: "+exclude_piece);
                        Console.WriteLine("Words generated with exclude groups will be rejected.");
                        Console.WriteLine("Multiple requirements may be made by using commas. e.g. rt,n");
                        Console.WriteLine("Type your exclude, then press enter.");
                        exclude_piece = Console.ReadLine();
                        state = 0;
                        last_text = "";
                        break;
                    case 6: //view markhov chain
                        Console.Clear();
                        Console.Write("What Chain would you like to see? e.g.(ab,ea,rt)");
                        string text = Console.ReadLine();
                        string ret = print_markhov(text);
                        if (ret != "")
                        {
                            Console.WriteLine(ret);
                        }
                        else
                            Console.WriteLine("");
                        Console.WriteLine("Press any key to continue.");
                        last_text = "";
                        Console.ReadKey();
                        state = 0;
                        break;
                    case 50:
                        cont = false;
                        break;
                    default:
                        break;
                }

                if (cur_text != last_text)
                {
                    Console.Clear();
                    Console.CursorLeft = 0;
                    Console.CursorTop = 0;
                    Console.WriteLine(cur_text);
                    last_text = cur_text;
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    main_menu.draw_menu();
                    Console.CursorLeft = Math.Min(50, 80 - ("Include: " + include_piece).ToString().Length);
                    Console.CursorTop = 10;
                    Console.Write("Include: " + include_piece);
                    Console.CursorLeft = Math.Min(50, 80 - ("Exclude: " + exclude_piece).ToString().Length);
                    Console.CursorTop = 11;
                    Console.Write("Exclude: " + exclude_piece);
                    Console.CursorLeft = 0;
                    Console.CursorTop = 1;
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                }
            }
        }

        public string print_markhov(string from)
        {
            int id_use = -1;
            for (int i = 0; i < markhov_chains.Count; i++)
            {
                if (markhov_chains[i].Key == from)
                {
                    id_use = i;
                    i = markhov_chains.Count;
                }
            }
            if (id_use!= -1)
            {
                Console.CursorLeft = 1;
                Console.CursorTop = 1;
                Console.Write("Chain generated for " + from + ".");
                for (int x = 0; x < markhov_chains[id_use].Value.Count; x++)
                {
                    Console.CursorLeft = 1;
                    Console.CursorTop = x + 2;
                    Console.Write(from + markhov_chains[id_use].Value[x].ToString());
                }
            }
            else
            {
                return "No such Markhov chain exists.";
            }
            return "";
        }

        public bool name_acceptable(string name)
        {
            if (include_piece != "")
            {
                for (int x = 0; x < include.Length; x++)
                {
                    if (!name.Contains(include[x]))
                        return false;
                }
            }
            if (exclude_piece != "")
            {
                for (int x = 0; x < exclude.Length; x++)
                {
                    if (name.Contains(exclude[x]))
                        return false;
                }
            }
            return true;
        }

        public string create_name()
        {
            string build = "";
            int num_use = markhov_chains.Count;
            int to_id = 0;
            string last_to = "";
            int last_num_use = -1;
            num_use = rand.Next(0, num_use);
            
            to_id = markhov_chains[num_use].Value.Count;
            to_id = rand.Next(0, to_id);
            while (!markhov_chains[num_use].Value[to_id].at_beginning)
            {
                num_use = rand.Next(0, markhov_chains.Count);
                to_id = markhov_chains[num_use].Value.Count;
                to_id = rand.Next(0, to_id);
            }
            build = markhov_chains[num_use].Key;
            while (markhov_chains[num_use].Value[to_id].to.Length > 1 && last_num_use != num_use)
            {
                last_to = markhov_chains[num_use].Value[to_id].to;
                build += last_to;
                last_num_use = num_use;
                num_use = -1;
                for (int x = 0; x < markhov_chains.Count; x++)
                {
                    if (markhov_chains[x].Key == last_to)
                    {
                        num_use = x;
                        x = markhov_chains.Count;
                    }
                }
                if (num_use == -1)
                    break;
                int tot_num = 0;
                for (int x = 0; x < markhov_chains[num_use].Value.Count; x++)
                {
                    tot_num += markhov_chains[num_use].Value[x].instances;
                }
                int rand_choose = rand.Next(0, tot_num);
                tot_num = 0;
                for (int x = 0; x < markhov_chains[num_use].Value.Count; x++)
                {
                    tot_num += markhov_chains[num_use].Value[x].instances;
                    if (rand_choose <= tot_num)
                    {
                        to_id = x;
                        x = markhov_chains[num_use].Value.Count;
                    }
                }
            }
            if(num_use!= -1)
            build += markhov_chains[num_use].Value[to_id].to;
            return build;
        }

        public void add_string_to_chain(string pass)
        {
            string from = "";
            string to = "";
            bool at_beginning = true;
            for (int x = 0; x < pass.Length; x++)
            {
                from = "";
                to = "";
                if (pass.Length >= x + 2)
                {
                    from = pass.Substring(x, 2);
                }
                else
                { //terminator.
                    if (pass.Length >= x + 1)
                    {
                        from = pass.Substring(x);
                    }
                    else
                        return;
                }
                if (pass.Length >= x + 4)
                {
                    to = pass.Substring(x + 2, 2);
                }
                else
                { //terminator
                    if (pass.Length >= x + 3)
                    {
                        to = pass.Substring(x + 2);
                    }
                }
                int id_add = -1;
                for (int i = 0; i < markhov_chains.Count; i++)
                {
                    if (markhov_chains[i].Key == from)
                    {
                        id_add = i;
                        i = markhov_chains.Count;
                    }
                }
                if (id_add != -1)
                {
                    bool was_in = false;
                    for (int i = 0; i < markhov_chains[id_add].Value.Count; i++)
                    {
                        if (markhov_chains[id_add].Value[i].to == to)
                        {
                            if (at_beginning)
                            {
                                at_beginning = false;
                                markhov_chains[id_add].Value[i].at_beginning = true;
                            }
                            markhov_chains[id_add].Value[i].instances++;
                            was_in = true;
                            num_add++;
                            continue;
                        }
                    }
                    if (!was_in)
                    {
                        markhov_chains[id_add].Value.Add(new Markhov(to,at_beginning));
                        at_beginning = false;
                        num_add++;
                    }
                }
                else
                {
                    markhov_chains.Add(new KeyValuePair<string,List<Markhov>>(from,new List<Markhov>()));
                    markhov_chains[markhov_chains.Count-1].Value.Add(new Markhov(to,at_beginning));
                    at_beginning = false;
                    num_add++;
                }
            }
        }
    }
}
