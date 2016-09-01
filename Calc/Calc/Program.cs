using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc
{

    class Program
    {
        Stack<long> numStack = new Stack<long>();
        Stack<String> opStack = new Stack<string>();
        List<String> args = new List<string>();

        static void Main(string[] args)
        {
            //Console.WriteLine(args[0]);
            Program program = new Program();
            //program.calculate(args[0]);

            /*program.calculate("3+4+5+6");
            program.calculate("3-4+5+6");
            program.calculate("+3+4+5+6");
            program.calculate("-3+4+5+6");
            program.calculate("+3+-4+5+6");
            program.calculate("+33+-44+55+66");
            program.calculate("2--5++6");
            program.calculate("5+4-3-10");
            program.calculate("0+1-2+3-4+5-6+7-8+9");
            program.calculate("-3++3/4--2*4");
            program.calculate("7%3*5");
            program.calculate("7*5%3");*/

            /*program.calculate("2121212121*2020202020");
            program.calculate("3/0");
            program.calculate("3.0+4%6");
            program.calculate("3+4/6+");
            program.calculate("3++ +4-6");
            program.calculate("3+6+3333333333333");
            program.calculate("+3+-4+5+6+");*/

            Console.Read();
        }

        public void calculate(String arg)
        {
            numStack.Clear();
            opStack.Clear();
            args.Clear();
            bool result = parse(arg);
            if (result == false)
            {
                Console.WriteLine("Invalid Input");
                Console.Read();
                Environment.Exit(0);
            }
            else
            {
                for(int i = 0; i < args.Count; i++)
                {
                    Console.Write(args[i] + "  ");
                }
                Console.WriteLine();
            }

            initStack();
            getResult();

            if(numStack.Count == 1)
            {
                Console.WriteLine(numStack.Pop());
            }else
            {
                Console.WriteLine("Some errors I don't know");
                Console.Read();
                Environment.Exit(0);
            }
        }

        public void getResult()
        {
            Stack<long> reverseNum = new Stack<long>();
            while (numStack.Count != 0)
            {
                reverseNum.Push(numStack.Pop());
            }

            Stack<String> reverseOp = new Stack<string>();
            while (opStack.Count != 0)
            {
                reverseOp.Push(opStack.Pop());
            }

            while (reverseOp.Count > 0)
            {
                String op = reverseOp.Pop();
                long num1 = reverseNum.Pop();
                long num2 = reverseNum.Pop();
                long result;
                switch (op)
                {
                    case "+":
                        result = num1 + num2;
                        if (result > Int32.MaxValue || result < Int32.MinValue)
                        {
                            Console.WriteLine("Out of Range");
                            Console.Read();
                            Environment.Exit(0);
                        }
                        reverseNum.Push(result);
                        break;
                    case "-":
                        result = num1 - num2;
                        if (result > Int32.MaxValue || result < Int32.MinValue)
                        {
                            Console.WriteLine("Out of Range");
                            Console.Read();
                            Environment.Exit(0);
                        }
                        reverseNum.Push(result);
                        break;
                }
            }
            numStack = reverseNum;
            opStack = reverseOp;
        }

        public void initStack()
        {
            for(int i = 0; i < args.Count; i++)
            {
                if(i % 2 == 0)  //get num
                {
                    initNumStack(i);
                }else if(i % 2 == 1)  //get op
                {
                    initOpStack(ref i);
                }
            }
        }

        public void initOpStack(ref int i)
        {
            String operation = args[i];
            switch (operation)
            {
                case "+":
                    opStack.Push(operation);
                    break;
                case "-":
                    opStack.Push(operation);
                    break;
                case "*":
                    mult(ref i);
                    break;
                case "/":
                    div(ref i);
                    break;
                case "%":
                    mod(ref i);
                    break;
            }
        }

        public void mult(ref int i)
        {
            long num1 = numStack.Pop();
            i++;
            long num2 = Int64.Parse(args.ElementAt(i));
            long result = num1 * num2;
            if(result > Int32.MaxValue || result < Int32.MinValue)
            {
                Console.WriteLine("Out of Range");
                Console.Read();
                Environment.Exit(0);
            }
            numStack.Push(result);
        }

        public void div(ref int i)
        {
            long num1 = numStack.Pop();
            i++;
            long num2 = Int64.Parse(args.ElementAt(i));
            if(num2 == 0)
            {
                Console.WriteLine("Division by zero");
                Console.Read();
                Environment.Exit(0);
            }
            long result = num1 / num2;
            numStack.Push(result);
        }

        public void mod(ref int i)
        {
            long num1 = numStack.Pop();
            i++;
            long num2 = Int64.Parse(args.ElementAt(i));
            if (num2 == 0)
            {
                Console.WriteLine("Mod by zero");
                Console.Read();
                Environment.Exit(0);
            }
            long result = num1 % num2;
            numStack.Push(result);
        }

        public void initNumStack(int i)
        {
            long temp = Int64.Parse(args.ElementAt(i));
            if(temp > Int32.MaxValue || temp < Int32.MinValue)
            {
                Console.WriteLine("Invalid Input");
                Console.Read();
                Environment.Exit(0);
            }
            numStack.Push(temp);
        }

        public bool parse(String arg)
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
                                while (i < arg.Length && arg.ElementAt(i) >= '0' && arg.ElementAt(i) <= '9')
                                {
                                    temp = temp + arg.ElementAt(i).ToString();
                                    i++;
                                }
                                i--;
                                args.Add(temp);
                                isNumber = false;
                            }else
                            {
                                return false;
                            }
                        }
                    }else if (arg.ElementAt(i) >= '0' && arg.ElementAt(i) <= '9')
                    {
                        String temp = "";
                        while (i < arg.Length && arg.ElementAt(i) >= '0' && arg.ElementAt(i) <= '9' )
                        {
                            temp = temp + arg.ElementAt(i).ToString();
                            i++;
                        }
                        i--;
                        args.Add(temp);
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
                        args.Add(temp);
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
