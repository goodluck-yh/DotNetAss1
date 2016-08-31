using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(args[0]);
            List<String> list = new List<string>();
            Program program = new Program();
            bool result = program.parse(args[0], list);
            if(result == false)
            {
                Console.WriteLine("Invalid Input");
            }else
            {
                Console.WriteLine("Valid Input");
            }
            //Console.Read();
        }

        public bool parse(String arg, List<String> list)
        {
            bool isNumber = true;
            for(int i = 0; i < arg.Length; i++)
            {
                if(isNumber == true)
                {
                    if(arg.ElementAt(i) == '+' || arg.ElementAt(i) == '-')
                    {
                        String temp = arg.ElementAt(i).ToString();
                        i++;
                        if(i == arg.Length)
                        {
                            return false;
                        }
                        else
                        {
                            if(arg.ElementAt(i) >= '0' && arg.ElementAt(i) <= '9')
                            {
                                temp = temp + arg.ElementAt(i).ToString();
                                list.Add(temp);
                                isNumber = false;
                            }else
                            {
                                return false;
                            }
                        }
                    }else if (arg.ElementAt(i) >= '0' && arg.ElementAt(i) <= '9')
                    {
                        String temp = arg.ElementAt(i).ToString();
                        list.Add(temp);
                        isNumber = false;
                    }else
                    {
                        return false;
                    }
                }else
                {
                    if(arg.ElementAt(i) == '+' || arg.ElementAt(i) == '-' || arg.ElementAt(i) == '*' || arg.ElementAt(i) == '/' || arg.ElementAt(i) == '%')
                    {
                        String temp = arg.ElementAt(i).ToString();
                        list.Add(temp);
                        isNumber = true;
                    }else
                    {
                        return false;
                    }
                }
            }
            if(isNumber == true)
            {
                return false;
            }else
            {
                return true;
            }
            
        }
    }
}
