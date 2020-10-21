using System;
using System.IO;
using static System.Console;
using System.Collections.Generic;

namespace v6
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0) Environment.Exit(0);

            while (true){
                FileExists(args[0]);

                StreamReader reader = new StreamReader(args[0]);
                string[] read = reader.ReadToEnd().Split(Environment.NewLine);
                reader.Close();

                List<Persona> people = new List<Persona>();
                foreach (var i in read){
                    if (i != "") people.Add(Persona.CreateFromLine(i));
                }

                WriteLine("\n[1] = Capturar\n[2] = Listar\n[3] = Buscar\n[4] = Editar\n[5] = Eliminar\n[6] = Salir");
                string opt = ReadLine();

                switch (opt){
                    case "1":
                        Capturar(args[0], people);
                        break;
                    case "2":
                        Listar(people);
                        break;
                    case "3":
                        Buscar(people);
                        break;
                    case "4":
                        Editar(args[0], people);
                        break;
                    case "5":
                        Eliminar(args[0], people);
                        break;
                    case "6":
                        Environment.Exit(0);
                        break;
                    default:
                        WriteLine("\nLa opción elegida no es válida.");
                        break;
                }
            }
        }

        #region Registro de Datos v1
        static void FileExists(string path)
        {
            if (!File.Exists(path)){
                var creator = File.Create(path);
                creator.Close();
            }
        }

        static void Capturar(string path, List<Persona> people)
        {
            while(true){
                string ced = ReadCedula("\nCedula: ");
                Write("\nNombre: ");
                string name = ReadLine();
                Write("Apellidos: ");
                string ape = ReadLine();

                if (name == "" && ape == "")
                    break;

                int age = ReadAge("Edad (7 - 120): ");;
                while (age < 7 || age > 120){
                    age = ReadAge("\nEdad (7 - 120): ");
                }

                char gender, state, grade;
                do{
                    gender = ReadChar("\nGénero (M/F): ");
                } while (gender != 'M' && gender != 'F');

                do{
                    state = ReadChar("\nEstado Civil (S/C): ");
                } while (state != 'S' && state != 'C');

                do{
                    grade = ReadChar("\nGrado Académico (I/M/G/P): ");
                } while (grade != 'I' && grade != 'M' && grade != 'G' && grade != 'P');

                decimal ahorros = ReadMoney("\nAhorros: ");
                string password = ReadPassword("\nContraseña: ");

                bool success = password == ReadPassword("\nConfirme contraseña: ");

                int datos = ToBits(age, gender, state, grade);

                Console.WriteLine();
                if (!success) continue;

                Persona nuevo = new Persona(ced, name, ape, datos, ahorros, password);
                if (people.Contains(nuevo)){
                    WriteLine("La cédula ya existe.");
                } else{
                    while (true){
                        WriteLine("\nGuardar (G); Rehacer (R); Salir (S)");
                        string opt = ReadLine().ToUpper();

                        if (opt == "G"){
                            StreamWriter writer = File.AppendText(path);
                            writer.WriteLine(nuevo.ToWrite());
                            writer.Close();
                            break;
                        } else if (opt == "R") break;
                        else if (opt == "S") Environment.Exit(0);
                        else continue;
                    }
                }
            }
        }
        #endregion

        #region Registros de Datos v2
        static void Listar(List<Persona> people)
        {
            foreach (var i in people){
                WriteLine(i);
            }
        }

        static Persona Buscar(List<Persona> people)
        {
            string ced = ReadCedula("\nIntroduzca la cédula a buscar: ");
            Persona persona = new Persona("","","",0,0,"");

            WriteLine();
            foreach (var i in people){
                if (i.Cedula == ced){
                    persona = i;
                    WriteLine(persona);
                }
            }

            if (persona.Cedula == "") WriteLine("No se ha podido encontrar la persona.");

            return persona;
        }
        #endregion

        #region Registro de Datos v3
        static void Editar(string path, List<Persona> people)
        {
            Persona persona = Buscar(people);

            if (persona.Cedula == "") return;

            while (true){
                string ced = ReadCedula("\nCedula: ");
                Write("\nNombre: ");
                string name = ReadLine();
                Write("Apellidos: ");
                string ape = ReadLine();

                if (name == "" && ape == "")
                    break;

                int age = ReadAge("Edad (7 - 120): ");;
                while (age < 7 || age > 120){
                    age = ReadAge("\nEdad (7 - 120): ");
                }

                char gender, state, grade;
                do{
                    gender = ReadChar("\nGénero (M/F): ");
                } while (gender != 'M' && gender != 'F');

                do{
                    state = ReadChar("\nEstado Civil (S/C): ");
                } while (state != 'S' && state != 'C');

                do{
                    grade = ReadChar("\nGrado Académico (I/M/G/P): ");
                } while (grade != 'I' && grade != 'M' && grade != 'G' && grade != 'P');

                decimal ahorros = ReadMoney("\nAhorros: ");
                string password = ReadPassword("\nContraseña: ");

                bool success = password == ReadPassword("\nConfirme contraseña: ");

                int datos = ToBits(age, gender, state, grade);

                Console.WriteLine();
                if (!success) continue;

                Persona nuevo = new Persona(ced, name, ape, datos, ahorros, password);
                if (nuevo.Equals(persona)){
                    File.Delete(path);
                    foreach (var i in people){
                        Persona line = i;
                        if (line.Equals(nuevo)) line = nuevo;

                        StreamWriter writer = File.AppendText(path);
                        writer.WriteLine(line.ToWrite());
                        writer.Close();
                    }
                    WriteLine();
                    break;
                } else if (people.Contains(nuevo)) WriteLine("\nLa cédula ya existe.");
                else {
                    File.Delete(path);
                    foreach (var i in people){
                        Persona line = i;
                        if(line.Equals(persona)) line = nuevo;

                        StreamWriter writer = File.AppendText(path);
                        writer.WriteLine(line.ToWrite());
                        writer.Close();
                    }
                    WriteLine();
                    break;
                }
            }
        }

        static void Eliminar(string path, List<Persona> people)
        {
            Persona persona = Buscar(people);

            if (persona.Cedula == "") return;

            while (true){
                WriteLine("¿Desea eliminarlo (Y/N)?");
                string opt = ReadLine().ToUpper();

                if (opt == "Y"){
                    File.Delete(path);

                    foreach(var i in people){
                        if (i.Equals(persona)) continue;

                        StreamWriter writer = File.AppendText(path);
                        writer.WriteLine(i.ToWrite());
                        writer.Close();
                    }
                    WriteLine();
                    break;
                } else if (opt == "N") break;
                else continue;
            }
        }
        #endregion

        #region Registros de v4
        static string ReadPassword(string text)
        {
            Write(text);

            while(true){
                string password = "";
                ConsoleKey key;

                do{
                    var keyInfo = ReadKey(intercept: true);
                    key = keyInfo.Key;

                    if(key == ConsoleKey.Backspace && password.Length > 0){
                        Write("\b \b");
                        password = password.Remove(password.Length - 1);
                    }
                    else if(!char.IsControl(keyInfo.KeyChar)){
                        Write("*");
                        password += keyInfo.KeyChar;
                    }
                } while(key != ConsoleKey.Enter);

                if (password == "")
                    continue;

                return password;
            }
        }

        static string ReadCedula(string text)
        {
            Write(text);
            while(true){
                string data = "";
                ConsoleKey key;

                do{
                    var keyInfo = ReadKey(intercept: true);
                    key = keyInfo.Key;

                    int value;
                    bool success = int.TryParse(keyInfo.KeyChar.ToString(), out value);

                    if(key == ConsoleKey.Backspace && data.Length > 0){
                        Write("\b \b");
                        data = data.Remove(data.Length - 1);
                    } else if(!char.IsControl(keyInfo.KeyChar) && success){
                        Write(keyInfo.KeyChar);
                        data += keyInfo.KeyChar;
                    }

                } while(key != ConsoleKey.Enter);

                if(data == "")
                    continue;

                return data;
            }
        }

        static int ReadAge(string text)
        {
            Write(text);
            while (true){
                string data = "";
                ConsoleKey key;

                do{
                    var keyInfo = ReadKey(intercept: true);
                    key = keyInfo.Key;

                    int value;
                    bool success = int.TryParse(keyInfo.KeyChar.ToString(), out value);

                    if(key == ConsoleKey.Backspace && data.Length > 0){
                        Write("\b \b");
                        data = data.Remove(data.Length - 1);
                    } else if(!char.IsControl(keyInfo.KeyChar) && success){
                        Write(keyInfo.KeyChar);
                        data += keyInfo.KeyChar;
                    }
                } while(key != ConsoleKey.Enter);

                if(data == "")
                    continue;

                return int.Parse(data);
            }
        }

        static decimal ReadMoney(string text)
        {
            Write(text);
            while (true){
                string data = "";
                ConsoleKey key;
                int c = 0;

                do{
                    var keyInfo = ReadKey(intercept: true);
                    key = keyInfo.Key;

                    int value;
                    bool success = int.TryParse(keyInfo.KeyChar.ToString(), out value) || (keyInfo.KeyChar == '.' && c == 0);

                    if(keyInfo.KeyChar == '.')
                        c++;

                    if(key == ConsoleKey.Backspace && data.Length > 0){
                        Write("\b \b");
                        data = data.Remove(data.Length - 1);
                    } else if(!char.IsControl(keyInfo.KeyChar) && success){
                        Write(keyInfo.KeyChar);
                        data += keyInfo.KeyChar;
                    }
                } while(key != ConsoleKey.Enter);

                if(data == "")
                    continue;

                return Math.Round(decimal.Parse(data), 2);
            }
        }
        #endregion

        #region Registros de v5
        static int ToBits(int edad, char genero, char estado, char grado)
        {
            int datos = edad << 4;

            if (genero == 'F') datos = datos | 8;

            if (estado == 'C') datos = datos | 4;

            if (grado == 'M') datos = datos | 1;
            else if (grado == 'G') datos = datos | 2;
            else if (grado == 'P') datos = datos | 3;

            return datos;
        }

        static void ReadBits(int datos, out int edad, out char genero, out char estado, out char grado)
        {
            edad = datos >> 4;

            datos = datos & 15;
            if ((datos >> 3) == 1) genero = 'F';
            else genero = 'M';

            datos = datos & 7;
            if ((datos >> 2) == 1) estado = 'C';
            else estado = 'S';

            datos = datos & 3;
            if (datos == 0) grado = 'I';
            else if (datos == 1) grado = 'M';
            else if (datos == 2) grado = 'G';
            else grado = 'P';
        }

        static char ReadChar(string text)
        {
            Write(text);
            string value = "";
            ConsoleKey key;

            do{
                var keyInfo = ReadKey(intercept: true);
                key = keyInfo.Key;

                if(key == ConsoleKey.Backspace && value.Length > 0){
                    Write("\b \b");
                    value = value.Remove(value.Length - 1);
                }
                else if(!char.IsControl(keyInfo.KeyChar) && value.Length < 1){
                    Write(keyInfo.KeyChar);
                    value += keyInfo.KeyChar;
                }
            } while (key != ConsoleKey.Enter);

            return Convert.ToChar(value);
        }
        #endregion
    }
}