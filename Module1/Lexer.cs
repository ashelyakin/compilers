using System;
using System.Text;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Lexer
{

    public class LexerException : System.Exception
    {
        public LexerException(string msg)
            : base(msg)
        {
        }

    }

    public class Lexer
    {

        protected int position;
        protected char currentCh; // очередной считанный символ
        protected int currentCharValue; // целое значение очередного считанного символа
        protected System.IO.StringReader inputReader;
        protected string inputString;

        public Lexer(string input)
        {
            inputReader = new System.IO.StringReader(input);
            inputString = input;
        }

        public void Error()
        {
            System.Text.StringBuilder o = new System.Text.StringBuilder();
            o.Append(inputString + '\n');
            o.Append(new System.String(' ', position - 1) + "^\n");
            o.AppendFormat("Error in symbol {0}", currentCh);
            throw new LexerException(o.ToString());
        }

        protected void NextCh()
        {
            this.currentCharValue = this.inputReader.Read();
            this.currentCh = (char) currentCharValue;
            this.position += 1;
        }

        public virtual bool Parse()
        {
            return true;
        }
    }

    public class IntLexer : Lexer
    {
        protected System.Text.StringBuilder intString;
        public int parseResult = 0;
        private int sign = 1;

        public IntLexer(string input)
        : base(input)
        {
            intString = new System.Text.StringBuilder();
        }

        public override bool Parse()
        {
            NextCh();
            if (currentCh == '+')
                NextCh();
            else if (currentCh == '-')
            {
                sign = -1;
                NextCh();
            }

            if (char.IsDigit(currentCh))
            {
                parseResult = (int)(currentCh - '0');
                NextCh();
            }
            else
            {
                Error();
            }

            while (char.IsDigit(currentCh))
            {
                parseResult = parseResult * 10 + (int)(currentCh - '0');
                NextCh();
            }


            if (currentCharValue != -1)
            {
                Error();
            }

            parseResult *= sign;
            return true;
        }
    }

    public class IdentLexer : Lexer
    {
        private string parseResult;
        protected StringBuilder builder;
    
        public string ParseResult
        {
            get { return parseResult; }
        }
    
        public IdentLexer(string input) : base(input)
        {
            builder = new StringBuilder();
        }

        public override bool Parse()
        {
            string regex = @"(^[A-Za-z])([A-Za-z0-9_])*$";
            if (Regex.IsMatch(inputString, regex))
            {
                parseResult = inputString;
                return true;
            }
            else
                throw new LexerException("");
        }
       
    }

    public class IntNoZeroLexer : IntLexer
    {
        public IntNoZeroLexer(string input)
            : base(input)
        {
        }

        public override bool Parse()
        {
            Regex regex = new Regex (@"^[-+]?[1-9][0-9]*$");
            if (regex.IsMatch(inputString))
            {
                parseResult = Int32.Parse(inputString);
                return true;
            }
            else
            {             
                throw new LexerException("");
            }
        }
    }

    public class LetterDigitLexer : Lexer
    {
        protected StringBuilder builder;
        protected string parseResult;

        public string ParseResult
        {
            get { return parseResult; }
        }

        public LetterDigitLexer(string input)
            : base(input)
        {
            builder = new StringBuilder();
        }

        public override bool Parse()
        {
            Regex regex = new Regex(@"^[A-Za-z](([A-Za-z][0-9])*|([0-9][A-Za-z])*)[0-9]?$");
            if (regex.IsMatch(inputString))
            {
                parseResult = inputString;
                return true;
            }
            else
                throw new LexerException("");
        }
       
    }

    public class LetterListLexer : Lexer
    {
        protected List<char> parseResult;

        public List<char> ParseResult
        {
            get { return parseResult; }
        }

        public LetterListLexer(string input)
            : base(input)
        {
            parseResult = new List<char>();
        }

        public override bool Parse()
        {
            NextCh();
            if (inputString.Length % 2 == 0)
                Error();
            for (int i = 0; i<inputString.Length; i++)
            {
                if (i % 2 == 0)
                {
                    if (char.IsLetter(currentCh))
                    {
                        parseResult.Add(currentCh);
                        NextCh();
                    }
                    else
                        Error();
                }
                else
                {
                    if (currentCh == ',' | currentCh == ';')
                        NextCh();
                    else
                        Error();
                }
            }
            return true;
        }
    }

    public class DigitListLexer : Lexer
    {
        protected List<int> parseResult;

        public List<int> ParseResult
        {
            get { return parseResult; }
        }

        public DigitListLexer(string input)
            : base(input)
        {
            parseResult = new List<int>();
        }

        public override bool Parse()
        {
            Regex regex = new Regex(@"^([0-9] +)*[0-9]$");
            if (regex.IsMatch(inputString))
            {
                foreach (char c in inputString)
                    if (char.IsDigit(c))
                        parseResult.Add(Int32.Parse(c.ToString()));
                return true;
            }
            else
                throw new LexerException("");
        }
    }

    public class LetterDigitGroupLexer : Lexer
    {
        protected StringBuilder builder;
        protected string parseResult;

        public string ParseResult
        {
            get { return parseResult; }
        }
        
        public LetterDigitGroupLexer(string input)
            : base(input)
        {
            builder = new StringBuilder();
        }

        public override bool Parse()
        {
            if (inputString == "")
                throw new LexerException("");
            Regex regex = new Regex(@"^([A-Za-z]{1,2}[0-9]{1,2})*([A-Za-z]{1,2})?$");
            if (regex.IsMatch(inputString))
            {
                parseResult = inputString;
                return true;
            }
            else
                throw new LexerException("");
        }
       
    }

    public class DoubleLexer : Lexer
    {
        private StringBuilder builder;
        private double parseResult;

        public double ParseResult
        {
            get { return parseResult; }

        }

        public DoubleLexer(string input)
            : base(input)
        {
            builder = new StringBuilder();
        }

        public override bool Parse()
        {
            Regex regex = new Regex(@"^([0-9]+(\.[0-9]+)?)$");
            if (regex.IsMatch(inputString))
            {
                parseResult = double.Parse(inputString, System.Globalization.CultureInfo.InvariantCulture); ;
                return true;
            }
            else
                throw new LexerException("");
        }
       
    }

    public class StringLexer : Lexer
    {
        private StringBuilder builder;
        private string parseResult;

        public string ParseResult
        {
            get { return parseResult; }

        }

        public StringLexer(string input)
            : base(input)
        {
            builder = new StringBuilder();
        }

        public override bool Parse()
        {
            Regex regex = new Regex(@"^\'[^\']*\'$");
            if (regex.IsMatch(inputString))
            {
                parseResult = inputString;
                return true;
            }
            else
                throw new LexerException("");
        }
    }

    public class CommentLexer : Lexer
    {
        private StringBuilder builder;
        private string parseResult;

        public string ParseResult
        {
            get { return parseResult; }

        }

        public CommentLexer(string input)
            : base(input)
        {
            builder = new StringBuilder();
        }

        public override bool Parse()
        {
            int len = inputString.Length;
            if (len < 4)
                throw new LexerException("");
            if (inputString[len - 2] != '*' || inputString[len - 1] != '/')
                throw new LexerException("");
            NextCh();
            if (currentCh == '/')
            {
                NextCh();
                if (currentCh == '*')
                    NextCh();
                else
                    Error();
            }
            else
                Error();
            for (int i = 2; i < len; i++)
            {
                if (currentCh == '*')
                {
                    NextCh();
                    if (currentCh == '/' && i == len - 2)
                        return true;
                    if (currentCh == '/' && i != len - 2)
                        Error();
                }
                else
                    NextCh();
            }
            return true;
        }
    }

    public class IdentChainLexer : Lexer
    {
        private StringBuilder builder;
        private List<string> parseResult;

        public List<string> ParseResult
        {
            get { return parseResult; }

        }

        public IdentChainLexer(string input)
            : base(input)
        {
            builder = new StringBuilder();
            parseResult = new List<string>();
        }

        public override bool Parse()
        {
            Regex regex = new Regex(@"^(([A-Za-z])([A-Za-z0-9_])*\.)*(([A-Za-z])([A-Za-z0-9_])*)$");
            if (!regex.IsMatch(inputString))
            {
                throw new LexerException("");
            }
            else
            {
                parseResult = inputString.Split(new char[] { '.' }).ToList();
                return true;
            }
                
        }
    }

    public class Program
    {
        public static void Main()
        {
            string input = "/* \n";
            Lexer L = new CommentLexer(input);
            try
            {
                Console.WriteLine(L.Parse());
            }
            catch (LexerException e)
            {
                System.Console.WriteLine(e.Message);
            }

        }
    }
}