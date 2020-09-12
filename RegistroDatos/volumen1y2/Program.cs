using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace volumen1y2
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
                File.WriteAllText(fileName, "Cedula,Nombre,Apellido,Edad");
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
            var cedula = Console.ReadLine();
            Console.Write("Nombre: ");
            var nombre = Console.ReadLine();
            Console.Write("Apellido: ");
            var apellido = Console.ReadLine();
            Console.Write("Edad: ");
            var edad = Console.ReadLine();

            return $"{cedula},{nombre},{apellido},{edad}";
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
            Console.WriteLine("4. Salir");
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
                    var ced = Console.ReadLine();
                    BuscarRegistro(ced);
                    break;
                case "4":
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

    }
}
