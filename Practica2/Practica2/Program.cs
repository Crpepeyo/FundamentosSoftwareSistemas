using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practica2
{
    class Program
    {
        public static List<string> cp = new List<string>(); //almacena los CP
        public static List<tabsim> tabsim = new List<tabsim>(); //almacena tabsim
        public static List<Registro> registros = new List<Registro>(); //almacena los registros      
        public static string name = ""; //variable para almacenar la cadena de entrada que es el nombre del archivo
        public static void Main(string[] args) 
        {
            //Funcion para inicializar los registros
            inicializaRegistros();
            while (true)
            {
                Console.WriteLine("Ingresa el nombre del archivo sin su extension o ingresa # para salir\n"); //linea en consola
                string line = "";
                List<int> lineNumber = new List<int>();
                Visitor visitorNew = new Visitor();
                Program p1 = new Program();
                name = Console.ReadLine();
                if (String.Compare(name, "#") == 1) //si no detecta un # continua con el programa
                {
                    //Borra archivos anteriores para no sobreescribir informacion 
                    if (File.Exists(Environment.CurrentDirectory + @"\" + Program.name + ".err"))
                        File.Delete(Environment.CurrentDirectory + @"\" + Program.name + ".err");
                    if (File.Exists(Environment.CurrentDirectory + @"\" + Program.name + ".int"))
                        File.Delete(Environment.CurrentDirectory + @"\" + Program.name + ".int");
                    if (File.Exists(Environment.CurrentDirectory + @"\" + Program.name + ".tabsim"))
                        File.Delete(Environment.CurrentDirectory + @"\" + Program.name + ".tabsim");
                    if (File.Exists(Environment.CurrentDirectory + @"\" + Program.name + ".xe"))
                    {
                        //abrir archivo 
                        line = File.ReadAllText(name + ".xe"); // se completa el nombre del archivo con la extension .xe
                        line = line.Replace("\r", String.Empty); //quitar \r del programa 
                        sicxeLexer lex = new sicxeLexer(new AntlrInputStream(line + Environment.NewLine)); //lexer
                        lex.RemoveErrorListeners(); //quitar listeners para poder usar el metodo de errores propio
                        lex.AddErrorListener(ErroresUno.INSTANCE);//agregar metodo de manejo de errores
                        CommonTokenStream tokens = new CommonTokenStream(lex); //crear tokens segun el lexer
                        sicxeParser parser = new sicxeParser(tokens); //parser con tokens creados
                        parser.RemoveErrorListeners();//quitar listeners para poder usar el metodo de errores propio
                        parser.AddErrorListener(ErroresToken.INSTANCE); //agregar metodo de manejo de errores
                        parser.RemoveParseListeners();
                        parser.AddParseListener(visitorNew);
                        cp.Add("0000");//Inicio de programa
                        cp.Add("0000");//Inicio de primer instruccion
                        try
                        {
                            parser.programa(); //llamada a parser en programa
                            Console.WriteLine("//-------------------------------------Archivo Intermedio--------------------");
                            imprimeContador(cp, name, tabsim); //imprimir contador de programa
                            Console.WriteLine("\n");
                            Console.WriteLine("//-------------------------------------TABSIM--------------------");
                            imprimeTabSim(tabsim);
                            Console.WriteLine("\n");
                            Console.WriteLine("//-------------------------------------Lista de registros --------------------");
                            imprimeRegistros();
                        }
                        catch (RecognitionException e)
                        {
                            Console.WriteLine(e);
                        }
                        //paso 1 del ensamblador
                    }
                    else
                    {
                        Console.WriteLine("Archivo no existe");
                    }
                }
                else { Environment.Exit(1); }
                Console.ReadLine();
            }
        }

        public static void inicializaRegistros()
        {
            //Anade todos los registros a la lista de registros
            Registro r;
            r = new Registro("A", 0);
            registros.Add(r);
            r = new Registro("X", 1);
            registros.Add(r);
            r = new Registro("L", 2);
            registros.Add(r);
            r = new Registro("B", 3);
            registros.Add(r);
            r = new Registro("S", 4);
            registros.Add(r);
            r = new Registro("T", 5);
            registros.Add(r);
            r = new Registro("F", 6);
            registros.Add(r);
            r = new Registro("CP", 7);
            registros.Add(r);
            r = new Registro("SW", 8);
            registros.Add(r);
        }

        public static void imprimeContador(List<string> cp, string name, List<tabsim> tabsim)
        {
            int count = 0;
            string[] arr;
            string show = "";
            string header = "CP\tEtiqueta\tCodOp\tArgumento";
            bool writeHeader = false; 
            Console.WriteLine(header);
            StreamReader sr = new StreamReader(Environment.CurrentDirectory + @"\" + name+".xe");
            string line = sr.ReadLine();
            while (line != null)
            {
                if (count < cp.Count)
                {
                    show = cp[count] + "\t" + line;
                    Console.WriteLine(show);
                    count++;
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(Environment.CurrentDirectory + @"\" + name + ".int", true))
                    {
                        if (!writeHeader) //si es false, no se ha escrito el encabezado
                        {
                            file.WriteLine(header);
                            writeHeader = true;
                        }
                        file.WriteLine(show);
                    }
                }
                line = sr.ReadLine();
            }
            
            /*foreach (string ob in cp)
            {
                Console.WriteLine(ob);
            }*/
            //close the file
            sr.Close();
        }

        public static void imprimeTabSim(List<tabsim> tabsim)
        {
            string header = "Simbolo\tDirección";
            string show = ""; 
            Console.WriteLine("Simbolo\tDirección");
            bool writeHeader = false; 
            foreach (tabsim obj in tabsim)
            {
                show = obj.simbolo + "\t" + obj.direccion;
                Console.WriteLine(show);
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(Environment.CurrentDirectory + @"\" + name + ".tabsim", true))
                {
                    if (!writeHeader) //si es false, no se ha escrito el encabezado
                    {
                        file.WriteLine(header);
                        writeHeader = true;
                    }
                    file.WriteLine(show);
                }
            }
            Console.WriteLine("\n");
            Console.WriteLine("//-------------------------------------Tamaño del programa--------------------");
            Console.WriteLine("Tamaño del programa:");
            //Calcula tamaño del programa
            //int tam = Int32.Parse(cp[cp.Count - 1], System.Globalization.NumberStyles.HexNumber) - Int32.Parse(cp[0], System.Globalization.NumberStyles.HexNumber);
            //string tamHex = tam.ToString("X") ;
            string tamHex = cp[cp.Count - 1];
            string tamHexTotal = "";
            //Completa con 0s el tamaño 
            if (tamHex.Length < 4)
            {
                int j = 4 - tamHex.Length;
                string completa = "";
                for (int k = 0; k < j; k++)
                {
                    completa += "0";
                }
                tamHexTotal = completa + tamHex;
            }
            else
            {
                tamHexTotal = tamHex;
            }
            //Imprime tamaño en el archivo
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(Environment.CurrentDirectory + @"\" + name + ".tabsim", true))
            {
                file.WriteLine("Tamaño del programa:");
                file.WriteLine(tamHexTotal+"H");
            }
            //imprime tamaño del programa en HEX
            Console.WriteLine(tamHexTotal+"H");
            //Lista de registros 
            
        }

        public static void imprimeRegistros()
        {
            
            Console.WriteLine("Registro\tNumero\t\tValor");
            Registro r_cp = registros.Find(x => x.nemonico == "CP");
            r_cp.direccion = cp[cp.Count - 1];
            foreach (var r in registros)
            {
                Console.WriteLine(r.nemonico + "\t\t" + r.numero.ToString() + "\t\t" + r.direccion);
            }
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(Environment.CurrentDirectory + @"\" + name + ".tabsim", true))
            {
                file.WriteLine("Lista de registros");
                file.WriteLine("Registro\tNumero\t\tValor");
                foreach (var r in registros)
                {
                    file.WriteLine(r.nemonico + "\t\t" + r.numero.ToString() + "\t\t" + r.direccion);
                }
            }
        }
    }
 }

