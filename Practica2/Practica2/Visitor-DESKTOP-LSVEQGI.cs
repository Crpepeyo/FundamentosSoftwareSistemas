using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

namespace Practica2
{
    class Visitor : sicxeBaseListener
    {
        public override void ExitProposicion([NotNull] sicxeParser.ProposicionContext context)
        {
            var i = " ";
            tabsim obj = new tabsim();
            string cpFinal = "";
            var instruccion = context.instruccion();
            var directiva = context.directiva();
            if (instruccion != null)
            { //Es una instruccion
                //sacar el ultimo elemento del arreglo CP y convertirlo en numero
                bool cpFinalContieneError = Program.cp.Last().Contains("*");
                if (cpFinalContieneError) //si es true, se remplaza por una cadena vacia
                {
                    cpFinal = Program.cp.Last().Replace("*", String.Empty);
                }
                else
                {
                    cpFinal = Program.cp.Last();
                }
                int lastCP = Convert.ToInt32(cpFinal, 16); //el numero hexadecimal lo pasa a entero
                if (!cpFinalContieneError)
                {
                    //checar el formato f1,f2,f3.r4
                    if (context.instruccion().opinstruccion().formato().formatoUno() != null)
                    {
                        string prueba = context.instruccion().opinstruccion().formato().formatoUno().Stop.Text;
                        if (prueba == "RSUB")
                        {
                            lastCP += 3;
                        }
                        else
                        {
                            lastCP += 1;
                        }
                    }
                    else if (context.instruccion().opinstruccion().formato().formatoDos() != null)
                    {
                        //aumentar el contador en 2 bytes
                        lastCP += 2;
                    }
                    else if (context.instruccion().opinstruccion().formato().formatoTres() != null)
                    {
                        //aumentar el contador en 3 bytes
                        lastCP += 3;
                    }
                    else if (context.instruccion().opinstruccion().formato().formatoCuatro() != null)
                    {
                        //aumentar el contador en 4 bytes
                        lastCP += 4;
                    }
                }
                //Convertir el numero a hexadecimal
                string cpHex = lastCP.ToString("X");
                string newCP = "";
                //checar el numero de caracteres para completar con 0's
                if (cpHex.Length < 4)
                {
                    int j = 4 - cpHex.Length;
                    string completa = "";
                    for (int k = 0; k < j; k++)
                    {
                        completa += "0";
                    }
                    newCP = completa + cpHex;
                }
                else
                {
                    newCP = cpHex;
                }
                Program.cp.Add(newCP);

                //Checar si existe etiqueta
                if (context.instruccion().etiqueta() != null)
                {
                    try
                    {
                        i = Convert.ToString(context.instruccion().etiqueta().Stop.Text);//almacena la etiqueta si existe
                        obj.simbolo = i;
                        obj.direccion = Program.cp.ElementAt(Program.cp.Count - 2);
                        //checar si no existe en tabsim, para agregarla
                        if (!Program.tabsim.Contains(obj))
                        {
                            Program.tabsim.Add(obj);
                        }
                    }
                    catch
                    {
                        i = "no existe";
                    }
                }
            }
            else
            {
                if (context.directiva() != null)
                {
                    //sacar el ultimo elemento del arreglo CP y convertirlo en numero
                    bool cpFinalContieneError = Program.cp.Last().Contains("*");
                    if (cpFinalContieneError) //si es true, se remplaza por una cadena vacia
                    {
                        cpFinal = Program.cp.Last().Replace("*", String.Empty);
                        Program.cp.Add(cpFinal);
                    }
                    else
                    {
                        cpFinal = Program.cp.Last();
                    }
                    int lastCP = Convert.ToInt32(cpFinal, 16); //el numero hexadecimal lo pasa a entero
                        //checar la directiva
                        if (context.directiva().tipoDirectiva().BYTE() != null)
                        {
                            //Checar si el operador de la directiva es de tipo texto
                            if (context.directiva().opDirectiva().CONSTCAD() != null)
                            {
                                string temp = context.directiva().opDirectiva().Stop.Text.Replace("C'", String.Empty);
                                string temp2 = temp.Replace("'", String.Empty);
                                //contar numero de caracteres
                                int numCar = temp2.Length;
                                //aumentar el contador en numCar bytes
                                lastCP += numCar;
                            }
                            //checar si el operador de la directiva es de tipo numero
                            if (context.directiva().opDirectiva().CONSTHEX() != null)
                            {
                                string temp = context.directiva().opDirectiva().Stop.Text.Replace("X'", String.Empty);
                                string temp2 = temp.Replace("'", String.Empty);
                                //contar numero de caracteres
                                double numCar = Math.Round(Convert.ToDouble(temp2.Length) / Convert.ToDouble(2));
                                //aumentar el contador en numCar bytes
                                lastCP += Convert.ToInt32(numCar);
                            }
                        }
                        else if (context.directiva().tipoDirectiva().WORD() != null)
                        {
                            if (context.directiva().opDirectiva() != null) //asegurar que tiene parametros
                            {
                                //aumenta el contador en 3 bytes que es el tamaño de la palabra
                                lastCP += 3;
                            }
                        }
                        else if (context.directiva().tipoDirectiva().RESB() != null)
                        {
                        //checar si el numero es Hexadecimal
                        
                            if (context.directiva().opDirectiva() !=null)
                            {
                            string temp = context.directiva().opDirectiva().Stop.Text;
                                    if (context.directiva().opDirectiva().Stop.Text.Contains("H"))//si contiene H es hexadecimal
                                    {
                                        //convertir el numero a decimal
                                        temp = context.directiva().opDirectiva().Stop.Text.Replace("H", String.Empty);
                                        lastCP += Convert.ToInt32(temp, 16);
                                    }
                                    else
                                    {
                                        lastCP += Convert.ToInt32(context.directiva().opDirectiva().Stop.Text);
                                    }
                            }                   
                        }
                        else if (context.directiva().tipoDirectiva().RESW() != null)
                        {
                            if (context.directiva().opDirectiva() != null)
                            {
                                //checar si el numero es Hexadecimal, si es asi solo se multiplica *3 y se suma
                                if (context.directiva().opDirectiva().Stop.Text.Contains("H"))//si contiene H es hexadecimal
                                {
                                    //convertir el numero a decimal
                                    string temp = context.directiva().opDirectiva().Stop.Text.Replace("H", String.Empty);
                                    lastCP += Convert.ToInt32(temp, 16) * 3;
                                }
                                else
                                {
                                    lastCP += Convert.ToInt32(context.directiva().opDirectiva().Stop.Text) * 3;
                                }
                            }
                        }
                        else if (context.directiva().tipoDirectiva().BASE() != null)
                        {
                            //lastCP se queda igual
                        }
                   
                    //Convertir el numero a hexadecimal
                    string cpHex = lastCP.ToString("X");
                    string newCP = "";
                    //checar el numero de caracteres para completar con 0's
                    if (cpHex.Length < 4)
                    {
                        int j = 4 - cpHex.Length;
                        string completa = "";
                        for (int k = 0; k < j; k++)
                        {
                            completa += "0";
                        }
                        newCP = completa + cpHex;
                    }
                    else
                    {
                        newCP = cpHex;
                    }
                    Program.cp.Add(newCP);

                    //Checar si existe etiqueta
                    if (context.directiva().etiqueta() != null)
                    {
                        try
                        {
                            i = Convert.ToString(context.directiva().etiqueta().Stop.Text);//almacena la etiqueta si existe
                            obj.simbolo = i;
                            obj.direccion = Program.cp.ElementAt(Program.cp.Count-2);
                            //checar si no existe en tabsim, para agregarla
                            if (!Program.tabsim.Contains(obj))
                            {
                                Program.tabsim.Add(obj);
                            }
                        }
                        catch
                        {
                            i = "no existe";
                        }
                    }
                }
            }
            
        }
    }
}
