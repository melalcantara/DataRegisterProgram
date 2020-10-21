using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace V3V4
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
        public static string Capturar(params string[] pcedula) //AHORA SE UTILIZA PCEDULA COMO PARAMETRO QUE PUEDE O NO ESTAR PARA UTILIZAR EL METODO AL EDITAR
        {
            string cedula; //DECLARAMOS LA VARIABLE CEDULA PARA USARLA EN CASO DE QUE SE ESTE EDITANDO O NO
            Console.WriteLine("Captura de Datos");
            if (!pcedula.Any()) //VERIFICA SI EL PARAMETRO PCEDULA NO ES UTILIZADO, OSEA SE ESTA AGREGANDO UN REGISTRO NUEVO
            {
                Console.Write("Cedula: "); //EN CASO DE, PIDE LA CEDULA COMO NORMALMENTE SE HACIA
                cedula = numero(); //VARIABLE QUE CONTIENE SOLO DIGITOS PROVISTOS POR EL METODO PARA INGRESAR DIGITOS
            }
            else
            {
                cedula = "`" + pcedula[0].ToString(); //SINO, SE LE PASA EL VALOR DEL PARAMETRO A LA VARIABLE CEDULA CON UN '`' AL INICIO PARA IDENTIFICARLO MAS TARDE
            }
            Console.Write("Nombre: ");
            var nombre = Console.ReadLine();
            Console.Write("Apellido: ");
            var apellido = Console.ReadLine();
            Console.Write("Edad: ");
            var edad = numero(); //VARIABLE QUE CONTIENE SOLO DIGITOS PROVISTOS POR EL METODO PARA INGRESAR DIGITOS
            Console.Write("Ahorros: "); //AHORA SE PIDEN AHORROS
            var ahorros = ahorro(); //VARIABLE QUE CONTIENE SOLO DIGITOS Y UN PUNTO PROVISTOS POR EL METODO PARA INGRESAR AHORROS
            var contraseña = ""; 
            var repcontraseña = ""; //SE DECLARAN DOS VARIABLES PARA LA CONTRASEÑA
            do //BUCLE QUE PIDE LA CONTRASEÑA
            {
                Console.Write("Contraseña: ");
                contraseña = contra(); //VARIABLE QUE CONTIENE LO INGRESADO POR EL METODO PARA CONTRASEÑAS
                Console.Write("Repita la contraseña: ");
                repcontraseña = contra(); //VARIABLE QUE CONTIENE LO INGRESADO POR EL METODO PARA CONTRASEÑAS
            } while (contraseña != repcontraseña); //MIENTRAS SEA DIGITADA DOS VECES INCORRECTAMENTE

            return $"{cedula},{nombre},{apellido},{edad},{ahorros},{contraseña}"; //AHORA RETORNA TAMBIEN LOS AHORROS Y LA CONTRASEÑA
        }
        public static string MenuRegistro(string registro) //SE CAMBIA EL VOID POR STRING PARA PODER PASAR EL REGISTRO EDITADO EN UN STRING AL METODO EDITAR
        {
            Console.WriteLine("Guardar (G), Reiniciar (R), Salir (S)");
            var opcion = Console.ReadLine();
            switch (opcion.ToUpper())
            {
                case "G":
                    string verifica = ""; //SE DECLARA ESTE STRING PARA GUARDAR EL REGISTRO EDITADO DE FORMA NORMAL SI ES NECESARIO
                    if (registro[0] == '`') //VERIFICA SI LA CEDULA DEL REGISTRO TIENE EL CARACTER '`'
                    {
                        foreach (char item in registro) //PARA CADA CARACTER DEL REGISTRO
                        {
                            if (item == '`') //SI RECONOCE EL '`' SE SALTA ESTA ITERACION DEL FOREACH PARA QUE LA CEDULA QUEDE SIN ESE CARACTER
                                continue;
                            else
                                verifica += item; //EN CASO DE QUE NO LO RECONOZCA PASA EL CARACTER A LA VARIABLE VERIFICA
                        }
                        return verifica; //AL FINALIZAR TODAS LAS ITERACIONES RETORNA EL STRING DEL REGISTRO EDITADO
                    }
                    else
                        Guardar(registro); //SINO, AÑADE EL REGISTRO NORMAL YA QUE NO SE ESTA EDITANDO
                    break;
                case "R":
                    string verificar = ""; //TODO LO MISMO PERO PARA EL CASO DE QUE SE TENGA QUE REINICIAR EL METODO PARA CAPTURAR REGISTROS
                    if (registro[0] == '`')
                    {
                        foreach (char item in registro)
                        {
                            if (item == '`')
                                continue;
                            else
                                verificar += item;
                        }
                        MenuRegistro(Capturar(verificar));
                    }
                    else
                        MenuRegistro(Capturar());
                    break;
                case "S":

                    break;
                default:
                    MenuRegistro(registro);
                    break;
            }
            return null;
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
            Console.WriteLine("4. Editar uno"); //SE AÑADE CORRECTAMENTE EL MENSAJE DE LA OPCION EDITAR
            Console.WriteLine("5. Eliminar uno");
            Console.WriteLine("6. Salir");
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
                    var bced = numero(); //VARIABLE QUE CONTIENE SOLO DIGITOS PROVISTOS POR EL METODO PARA INGRESAR DIGITOS
                    BuscarRegistro(bced);
                    break;
                case "4":
                    Console.Write("Digite identificador: "); //SE AGREGA EL CASO DONDE SE PIDE LA CEDULA PARA LLAMAR EL METODO DE EDITAR
                    var edced = numero(); //VARIABLE QUE CONTIENE SOLO DIGITOS PROVISTOS POR EL METODO PARA INGRESAR DIGITOS
                    EditarRegistro(edced);
                    break;
                case "5":
                    Console.Write("Digite identificador: ");
                    var eced = numero(); //VARIABLE QUE CONTIENE SOLO DIGITOS PROVISTOS POR EL METODO PARA INGRESAR DIGITOS
                    EliminarRegistro(eced);
                    break;
                case "6":
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
        public static void EditarRegistro(string identificador) //SE DECLARA EL METODO VOID PARA EDITAR REGISTROS QUE TOMA COMO PARAMETRO LA CEDULA
        {
            int verif = 0; //SE DECLARA UNA VARIABLE TIPO ENTERO PARA VERIFICAR SI SE ENCONTRO EL REGISTRO QUE SE QUIERE EDITAR
            var data = File.ReadAllLines(fileName); //SE DECLARA LA VARIABLE DATA PARA GUARDAR EL CONTENIDO DEL ARCHIVO, YA QUE SE VA A SOBREESCRIBIR PARA HACER EL CAMBIO
            System.IO.StreamWriter file = new System.IO.StreamWriter(fileName, false); //SE CREA LA INSTANCIA PARA ESCRIBIR EN EL ARCHIVO (FALSE ES PARA SOBREESCRIBIR)
            foreach (var item in data) //SE ITERA POR CADA LINEA EN LA VARIABLE DATA(LOS CONTENIDOS QUE TENIA EL ARCHIVO DE REGISTROS)
            {
                var linea = item.Split(","); //SE DECLARA ESTA VARIABLE PARA PASAR LAS LINEAS DIVIDIDAS EN CADA LUGAR QUE TIENE UNA COMA
                if (linea[0] == identificador) //SI EL PRIMER ELEMENTO QUE ESTA DIVIDIDO ES IGUAL A LA CEDULA QUE RECIBE EL METODO PARA BUSCAR
                {
                    string edited = MenuRegistro(Capturar(identificador)); //SE DECLARA UN STRING QUE COGERA EL REGISTRO EDITADO QUE RETORNARAN LOS METODOS DE CAPTURA
                    if (edited == null) //SI ESTA VACIO(NULL) SIGNIFICA QUE EL USUARIO ELIGIO NO EDITARLO
                    {
                        file.WriteLine(item); //SE AGREGA NUEVAMENTE EL REGISTRO QUE SE IBA A EDITAR
                    }
                    else
                        file.WriteLine(edited); //LA EDICION SE COMPLETO Y POR LO TANTO SE AGREGA EL REGISTRO EDITADO
                    verif = 1; //COMO SE ENCONTRO EL REGISTRO SE CAMBIA EL VALOR DE LA VARIABLE PARA QUE NO MUESTRE EL MENSAJE DE QUE NO SE PUDO ENCONTRAR
                    continue; //CONTINUA CON LA SIGUIENTE ITERACION PARA QUE NO SE PASE AL ARCHIVO EL REGISTRO DOS VECES
                }
                file.WriteLine(item);    //SE AÑADE AL ARCHIVO DE REGISTROS EL REGISTRO EN EL ORDEN QUE ESTABA ANTERIORMENTE (RECUERDA QUE SE ESTA SOBREESCRIBIENDO)          
            }
            if (verif == 0)//LUEGO DE TERMINAR TODAS LAS ITERACIONES QUE AGREGAN LOS REGISTROS AL ARCHIVO, SI LA VARIABLE SIGUE SIENDO CERO NO SE ENCONTRO EL REGISTRO
            {
                Console.WriteLine("No existe el registro"); //LE MUESTRA AL USUARIO QUE NO SE PUDO ENCONTRAR EL ARCHIVO
            }
            file.Close(); //SE CIERRA EL ARCHIVO AL FINALIZAR
        }
        public static string contra() //SE DECLARA EL METODO PARA LEER POR CONSOLA LAS CONTRASEÑAS
        {
            string contenido = ""; //SE DECLARA LA VARIABLE CONTENIDO QUE VA A GUARDAR LO QUE EL USUARIO PUEDA INGRESAR EN LA CONSOLA
            while (true) //BUCLE INFINITO PARA ESPERAR CADA COSA QUE SE INGRESE HASTA QUE SE CUMPLA UNA DE LAS CONDICIONES QUE TERMINAN EL BUCLE
            {
                var tecla = Console.ReadKey(true); //SE DECLARA LA VARIABLE TECLA QUE TENDRA UNA DESCRIPCION DE LA TECLA PRECIONADA || TRUE ES PARA QUE NO SE MUESTRE EN LA CONSOLA NORMAL(INTERCEPTAR)
                if (tecla.Key == ConsoleKey.Enter) //SI LA TECLA QUE SE PRESIONA EQUIVALE A LA TECLA ENTER
                {
                    Console.WriteLine(); //SE IMPRIME UNA LINEA EN BLANCO
                    break; //SE TERMINA EL BUCLE
                }
                else if (tecla.Key == ConsoleKey.Backspace) //SI LA TECLA PRESIONADA ES BACKSPACE(BORRAR)
                {
                    if (Console.CursorLeft == 0) //VERIFICA SI YA NO QUEDA NADA QUE BORRAR
                        continue; //SI YA NO QUEDA NADA QUE BORRAR SE SALTAN LAS OPERACIONES RESTANTES DEL BUCLE PARA CONTINUAR CON DICHO BUCLE DESDE 0 ;)
                    Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop); //SE POSICIONA EL CURSOR(LA RAYITA DE ESCRIBIR) UNA POSICION A LA IZQUIERDA
                    Console.Write(" "); //SE REEMPLAZA LO QUE QUEDA DESPUES POR UN ESPACIO EN BLANCO (SE BORRA EN LA CONSOLA)
                    Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop); //NUEVAMENTE SE POSICIONA A LA IZQUIERDA
                    contenido = string.Join("", contenido.Take(contenido.Length - 1)); //SE BORRA LO QUE ESTABA ESCRITO EN LA VARIABLE CONTENIDO
                }
                else if (char.IsLetterOrDigit(tecla.KeyChar) || char.IsPunctuation(tecla.KeyChar) || char.IsSymbol(tecla.KeyChar)) //EN CASO DE QUE SE PRECIONE UNA LETRA, UN DIGITO O UN SIMBOLO/PUNTUACION
                {
                    if (tecla.KeyChar != 44) //SOLO SI LA TECLA PRECIONADA NO ES UNA COMA(PARA QUE NO SE VEA FEO)
                        contenido += tecla.KeyChar.ToString(); //SE LE AÑADE A LA VARIABLE CONTENIDO LO EQUIVALENTE A STRING DE LA TECLA PRECIONADA
                        Console.Write("*"); //EN LA CONSOLA SE MUESTRA COMO UN ASTERISCO
                }
            }
            return contenido; //LUEGO DE TERMINAR EL BUCLE RETORNA LO INGRESADO POR EL USUARIO
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
                } //HASTA ESTA LINEA ES LO MISMO
                else if (char.IsDigit(tecla.KeyChar)) //SI SE PRECIONA UN DIGITO
                {
                    contenido += tecla.KeyChar.ToString(); //SE AGREGA A LA VARIABLE CONTENIDO EL DIGITO CONVERTIDO EN STRING
                    Console.Write(tecla.KeyChar); //SE MUESTRA EN LA CONSOLA EL DIGITO INGRESADO
                }
            }
            return contenido; //RETORNA LOS DIGITOS QUE INGRESO EL USUARIO
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
                } //HASTA ESTA LINEA ES LO MISMO
                else if (char.IsDigit(tecla.KeyChar) || tecla.KeyChar == 46) //SI SE INGRESA UN DIGITO O LA TECLA QUE EQUIVALE AL PUNTO
                {
                    if (!contenido.Contains('.')) //MIENTRAS NO SE HAYA PUESTO UN PUNTO AUN
                        contenido += tecla.KeyChar.ToString(); //SE AGREGA A LA VARIABLE LO DIGITADO POR EL USUARIO CONVERTIDO A STRING
                        Console.Write(tecla.KeyChar); //SE MUESTRA LO QUE INGRESO EL USUARIO
                }
            }
            return contenido; //RETORNA TODO LO QUE HA INGRESADO EL USUARIO (MENOS LO QUE SE HA BORRADO OBVIAMENTE)
        }
    }
}
