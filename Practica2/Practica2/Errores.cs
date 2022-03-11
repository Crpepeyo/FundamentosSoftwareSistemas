using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;

namespace Practica2
{
    // private tipoError arrErr = new tipoError();
    public class ErroresUno : BaseErrorListener, IAntlrErrorListener<int>
    {
        public static ErroresUno INSTANCE = new ErroresUno();
        public void SyntaxError([NotNull] IRecognizer recognizer, [Nullable] int offendingSymbol, int line, int charPositionInLine, [NotNull] string msg, [Nullable] RecognitionException e)
        {
            String sourceName = recognizer.InputStream.SourceName;
            //Console.WriteLine("Error de lexer: " + msg + " line:  " + line + " position: " + charPositionInLine);
            string newCP = Program.cp.Last()+"*";
            Program.cp.RemoveAt(Program.cp.Count - 1);
            Program.cp.Add(newCP); 
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(Environment.CurrentDirectory + @"\" + Program.name + ".err", true))
            {
                file.WriteLine("Error de lexer: " + msg + " line:  " + line + " position: " + charPositionInLine);

            }
        }
    }

    public class ErroresToken : BaseErrorListener, IAntlrErrorListener<IToken>
     {
        public static ErroresToken INSTANCE = new ErroresToken();
        public void SyntaxError([NotNull] IRecognizer recognizer, [Nullable] IToken offendingSymbol, int line, int charPositionInLine, [NotNull] string msg, [Nullable] RecognitionException e)
        {
            //Console.WriteLine("Error de sintaxis: " + msg + " line:  " + line + " position: " + charPositionInLine);
            string newCP = Program.cp.Last() + "*";
            Program.cp.RemoveAt(Program.cp.Count - 1);
            Program.cp.Add(newCP);
            
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(Environment.CurrentDirectory + @"\" + Program.name + ".err", true))
            {
               file.WriteLine("Error de sintaxis: " + msg + " line:  " + line + " position: " + charPositionInLine);
            }
        }
     }
}
