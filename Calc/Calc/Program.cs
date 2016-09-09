using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc
{
    
    class Program
    {
        //This stack stores numbers
        private Stack<long> numStack = new Stack<long>();
        //This stack stores operations
        private Stack<String> opStack = new Stack<string>();
        //This is list store all arguments needed in this program
        private List<String> args = new List<string>();
        
        private const int TWO = 2;

        static void Main(string[] args)
        {
            Program program = new Program();
            if(args.Length != 1 ){
                Console.WriteLine("Invalid Input");
                return;
            }
            program.Calculate(args[0]);
           
        }

        /*
         * This method is to calculate and print out the result
         * The parameter arg is the expression the program need to calculate
         */
        public void Calculate(String arg)
        {
            //clear all stack and list
            numStack.Clear();
            opStack.Clear();
            args.Clear();

            //parse the expression and store information into list
            bool result = Parse(arg);
            if (result == false)
            {
                ErrorMessage("Invalid Input");
            }
            

            //move inforamtion from list to numStack and opStack
            InitStack();

            //calculate expression and get result
            GetResult();

            //Check whether the result is valid; If so, print it
            if(numStack.Count == 1)
            {
                Console.WriteLine(numStack.Pop());
            }else
            {
                ErrorMessage("Some errors I don't know");
            }
        }


        /*
         * This method is to calculate and get the result
         */
        private void GetResult()
        {
            //reverse numStack and store into reverseNum
            Stack<long> reverseNum = new Stack<long>();
            while (numStack.Count != 0)
            {
                reverseNum.Push(numStack.Pop());
            }

            //reverse opStack and store into reverseOp
            Stack<String> reverseOp = new Stack<string>();
            while (opStack.Count != 0)
            {
                reverseOp.Push(opStack.Pop());
            }

            //calculate and get the result
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
                        IsOutOfRange(result);
                        reverseNum.Push(result);
                        break;
                    case "-":
                        result = num1 - num2;
                        IsOutOfRange(result);
                        reverseNum.Push(result);
                        break;
                }
            }

            //set value for numStack and opStack
            numStack = reverseNum;
            opStack = reverseOp;
        }


        /*
         * This method is to initialize stacks. move inforamtion from list to numStack and opStack
         */
        private void InitStack()
        {
            for(int i = 0; i < args.Count; i++)
            {
                if(i % TWO == 0)  //deal with numbers
                {
                    InitNumStack(i);
                }else if(i % TWO == 1)  //deal with operations
                {
                    InitOpStack(ref i);
                }
            }
        }

        /*
         * This method is to initialize operation stack
         * The parameter index gives the index in args
         */
        private void InitOpStack(ref int index)
        {
            String operation = args[index];
            switch (operation)
            {
                case "+":
                    opStack.Push(operation);
                    break;
                case "-":
                    opStack.Push(operation);
                    break;
                case "*":
                    Mult(ref index);
                    break;
                case "/":
                    Div(ref index);
                    break;
                case "%":
                    Mod(ref index);
                    break;
            }
        }

        /*
         * This method is to deal with multiplication operation
         * The parameter index gives the index in args
         */
        private void Mult(ref int index)
        {
            long num1 = numStack.Pop();
            index++;
            long num2 = Int64.Parse(args.ElementAt(index));
            long result = num1 * num2;
            IsOutOfRange(result);
            numStack.Push(result);
        }

        /*
         * This method is to deal with division operation
         * The parameter index gives the index in args
         */
        private void Div(ref int index)
        {
            long num1 = numStack.Pop();
            index++;
            long num2 = Int64.Parse(args.ElementAt(index));
            if(num2 == 0)
            {
                ErrorMessage("Division by zero");
            }
            long result = num1 / num2;
            numStack.Push(result);
        }

        /*
         * This method is to deal with modulo operation
         * The parameter index gives the index in args
         */
        private void Mod(ref int index)
        {
            long num1 = numStack.Pop();
            index++;
            long num2 = Int64.Parse(args.ElementAt(index));
            if (num2 == 0)
            {
                ErrorMessage("Division by zero");
            }
            long result = num1 % num2;
            numStack.Push(result);
        }

        /*
         * This method is to initialize numStack 
         * The parameter index gives the index in args
         */
        private void InitNumStack(int index)
        {
            long num = Int64.Parse(args.ElementAt(index));
            if(num > Int32.MaxValue || num < Int32.MinValue)
            {
                ErrorMessage("Invalid Input");
            }
            numStack.Push(num);
        }


        /*
         * This method is to parse the expression 
         * The parameter arg is the expression which need to be calculated
         * It returns whether the expression is parsed successfully
         */
        private bool Parse(String arg)
        {
            bool isNumber = true;

            for(int i = 0; i < arg.Length; i++)
            {
                //parse the number
                if(isNumber == true) 
                {
                    bool isSuccess = ParseNumber(arg, ref i);
                    if(isSuccess == false)
                    {
                        return false;
                    }
                    isNumber = false;
                }
                //parse the operation
                else
                {
                    bool isSuccess = ParseOper(arg, ref i);
                    if(isSuccess == false)
                    {
                        return false;
                    }
                    isNumber = true;
                }
            }

            return !isNumber;
        }

        /*
         * This method is to parse  operations
         * The parameter arg is the expession which needs to be calculated 
         * The parameter index gives the index in arg
         * It returns whether the expression is parsed successfully
         */
        private bool ParseOper(String arg, ref int index)
        {
            if (CheckIsOperation(arg, index))
            {
                String temp = arg.ElementAt(index).ToString();
                args.Add(temp);
            }
            else
            {
                return false;
            }
            return true;
        }


        /*
         * This method is to parse  numbers
         * The parameter arg is the expession which needs to be calculated 
         * The parameter index gives the index in arg
         * It returns whether the expression is parsed successfully
         */
        private bool ParseNumber(String arg, ref int index )
        {
            //deal with the number with sign, such as +1 or -3
            if (CheckIsSign(arg, index))
            {
                //get sign
                String temp = arg.ElementAt(index).ToString();
                index++;

                //check whether it is out of range
                if (index == arg.Length)
                {
                    return false;
                }
                else
                {
                    //get pure number
                    if (CheckIsNumber(arg, index))
                    {
                        GetPureNumber(arg, ref index, ref temp);
                        args.Add(temp);
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            //deal with the number without sign, such as 1 or 3
            else if (CheckIsNumber(arg, index))
            {
                String temp = "";
                GetPureNumber(arg, ref index, ref temp);
                args.Add(temp);
            }
            else
            {
                return false;
            }
            return true;
        }


        /*
         * This method is to check whether it is a operation
         * The parameter arg is the expession which needs to be calculated 
         * The parameter index gives the index in arg
         * It returns whether it is a operation
         */
        private bool CheckIsOperation(String arg, int index)
        {
            if (arg.ElementAt(index) == '+' || arg.ElementAt(index) == '-' )
            {
                return true;
            } else if (arg.ElementAt(index) == '*' || arg.ElementAt(index) == '/' || arg.ElementAt(index) == '%')
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /*
         * This method is to check whether it is a sign
         * The parameter arg is the expession which needs to be calculated 
         * The parameter index gives the index in arg
         * It returns whether it is a sign
         */
        private bool CheckIsSign(String arg, int index)
        {
            if (arg.ElementAt(index) == '+' || arg.ElementAt(index) == '-')
            {
                return true;
            }else
            {
                return false;
            }
        }


        /*
         * This method is to check whether it is a number
         * The parameter arg is the expession which needs to be calculated 
         * The parameter index gives the index in arg
         * It returns whether it is a number
         */
        private bool CheckIsNumber(String arg, int index)
        {
            if (arg.ElementAt(index) >= '0' && arg.ElementAt(index) <= '9')
            {
                return true;
            }else
            {
                return false;
            }
        }


        /*
         * This method is to get pure number
         * The parameter arg is the expession which needs to be calculated 
         * The parameter index gives the index in arg
         * The parameter num store number after executing this method
         */
        private void GetPureNumber(String arg, ref int index, ref String number)
        {
            while (index < arg.Length && CheckIsNumber(arg, index))
            {
                number = number + arg.ElementAt(index).ToString();
                index++;
            }
            index--;
        }


        /*
         * This method is to check whether num is out of range
         * The parameter num is the number which needs to be checked
         */
        private void IsOutOfRange(long num)
        {
            if (num > Int32.MaxValue || num < Int32.MinValue)
            {
                ErrorMessage("Out of Range");
            }
        }

        /*
         * This method is to show the error information and exit the program
         * The parameter errorInfor is the error inforamtion which needs to be displayed
         */
        private void ErrorMessage(String errorInfo)
        {
            Console.WriteLine(errorInfo);
            Environment.Exit(0);
        }
    }
}
