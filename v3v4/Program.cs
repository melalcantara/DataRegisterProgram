using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
namespace v3v4
{
    class Program
    {

        private static string fileName;
        static void Main(string[] args)
        {

            fileName = args[0];

            var isNew = !(File.Exists(fileName));
            if (isNew)
            {

                File.Create(fileName).Close();

                File.WriteAllText(fileName, "Cedula,Nombre,Apellido,Edad,Ahorros,Contraseña");
                Console.WriteLine("Archivo Creado exitosamente");
            }
            int cont = 1;

            while (cont == 1)
            {
                Menu();
            }
        }
        public static string Capturar()
        {
            Console.WriteLine("Captura de Datos");
            Console.Write("Cedula: ");
            var cedula = numero();
            Console.Write("Nombre: ");
            var nombre = Console.ReadLine();
            Console.Write("Apellido: ");
            var apellido = Console.ReadLine();
            Console.Write("Edad: ");
            var edad = numero();
            Console.Write("Ahorros: ");
            var ahorros = ahorro();
            var contraseña = "";
            var repcontraseña = "";
            do
            {
                Console.Write("Contraseña: ");
                contraseña = contra();
                Console.Write("Repita la contraseña: ");
                repcontraseña = contra();
            } while (contraseña != repcontraseña);

            return $"{cedula},{nombre},{apellido},{edad},{ahorros},{contraseña}";
        }
        public static void MenuRegistro(string registro)
        {
            Console.WriteLine("Guardar (G), Reiniciar (R), Salir (S)");
            var opcion = Console.ReadLine();
            switch (opcion.ToUpper())
            {
                case "G":
                    Guardar(registro);
                    break;
                case "R":

                    MenuRegistro(Capturar());
                    break;
                case "S":

                    break;
                default:
                    MenuRegistro(registro);
                    break;
            }
        }
        public static void Guardar(string registro)
        {

            registro = Environment.NewLine + registro;
            Console.WriteLine(registro);
            File.AppendAllText(fileName, registro);
            Console.WriteLine("Registro insertado");
        }


        public static void Menu()
        {
            Console.WriteLine("Menu de Opciones");
            Console.WriteLine("1. Capturar");
            Console.WriteLine("2. Lista");
            Console.WriteLine("3. Buscar uno");
            Console.WriteLine("4. Eliminar uno");
            Console.WriteLine("5. Salir");
            var op = Console.ReadLine();
            switch (op)
            {
                case "1":
                    MenuRegistro(Capturar());
                    break;
                case "2":
                    ListaRegistros();
                    break;
                case "3":
                    Console.Write("Digite identificador: ");
                    var bced = numero();
                    BuscarRegistro(bced);
                    break;
                case "4":
                    Console.Write("Digite identificador: ");
                    var eced = numero();
                    EliminarRegistro(eced);
                    break;
                case "5":
                    Environment.Exit(1);
                    break;
                default:
                    Menu();
                    break;


            }

        }
        public static void ListaRegistros()
        {
            string[] datos = File.ReadAllLines(fileName);
            foreach (var data in datos)
            {

                var arr = data.Split(",");
                foreach (var col in arr)
                {
                    Console.Write($"{col}|");
                }
                Console.WriteLine();
            }

        }
        public static void BuscarRegistro(string identificador)
        {
            string[] data = File.ReadAllLines(fileName);
            string[] registro = new string[2];
            registro[0] = data[0];
            registro[1] = "No Existe el registro";
            foreach (var linea in data)
            {
                if (linea.Contains(identificador))
                {
                    var arr = linea.Split(",");
                    if (arr[0].Trim() == identificador)
                        registro[1] = linea;
                }
            }
            if (registro[1].Split(",").Count() == 1)
                Console.WriteLine("No existe el registro");
            else
                foreach (var linea in registro)
                {
                    var arr = linea.Split(",");
                    foreach (var col in arr)
                    {
                        Console.Write($"{col}|");
                    }
                    Console.WriteLine();
                }
        }
        public static void EliminarRegistro(string identificador)
        {
            int verif = 0;
            var data = File.ReadAllLines(fileName);
            System.IO.StreamWriter file = new System.IO.StreamWriter(fileName, false);
            foreach (var item in data)
            {
                var linea = item.Split(",");
                if (linea[0] == identificador)
                {
                    Console.WriteLine("Registro borrado");
                    verif = 1;
                    continue;
                }
                file.WriteLine(item);
            }
            if (verif == 0)
            {
                Console.WriteLine("No existe el registro");
            }
            file.Close();
        }
        public static string contra()
        {
            string contenido = "";
            while (true)
            {
                var tecla = Console.ReadKey(true);
                if (tecla.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    break;
                }
                else if (tecla.Key == ConsoleKey.Backspace)
                {
                    if (Console.CursorLeft == 0)
                        continue;
                    Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                    Console.Write(" ");
                    Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                    contenido = string.Join("", contenido.Take(contenido.Length - 1));
                }
                else if (char.IsLetterOrDigit(tecla.KeyChar))
                {
                    contenido += tecla.KeyChar.ToString();
                    Console.Write("*");
                }
            }
            return contenido;
        }
        public static string numero()
        {
            string contenido = "";
            while (true)
            {
                var tecla = Console.ReadKey(true);
                if (tecla.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    break;
                }
                else if (tecla.Key == ConsoleKey.Backspace)
                {
                    if (Console.CursorLeft == 0)
                        continue;
                    Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                    Console.Write(" ");
                    Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                    contenido = string.Join("", contenido.Take(contenido.Length - 1));
                }
                else if (char.IsDigit(tecla.KeyChar))
                {
                    contenido += tecla.KeyChar.ToString();
                    Console.Write(tecla.KeyChar);
                }
            }
            return contenido;
        }
        public static string ahorro()
        {
            string contenido = "";
            while (true)
            {
                var tecla = Console.ReadKey(true);
                if (tecla.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    break;
                }
                else if (tecla.Key == ConsoleKey.Backspace)
                {
                    if (Console.CursorLeft == 0)
                        continue;
                    Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                    Console.Write(" ");
                    Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                    contenido = string.Join("", contenido.Take(contenido.Length - 1));
                }
                else if (char.IsDigit(tecla.KeyChar) || tecla.KeyChar == 46)
                {
                    contenido += tecla.KeyChar.ToString();
                    Console.Write(tecla.KeyChar);
                }
            }
            return contenido;
        }
    }
}
