public class Persona
{
    private int HiddenData = 0;
    public string Cedula { get; }
    public string Name { get; }
    public string LastName { get; }
    public string FullName => $"{Name} {LastName}";
    public int Edad => HiddenData >> 4;
    public Genero Genero => (Genero) ((HiddenData & 15) >> 3);
    public EstadoCivil Estado => (EstadoCivil) ((HiddenData & 7) >> 2);
    public GradoAcademico Grado => (GradoAcademico) (HiddenData & 3);
    public decimal Ahorros { get; }
    public string Password { get; }

    public Persona(in string ced, in string name, in string lname, in int datos, in decimal ahorros, in string password)
    {
        Cedula = ced;
        Name = name;
        LastName = lname;
        HiddenData = datos;
        Ahorros = ahorros;
        Password = password;
    }

    public static Persona CreateFromLine(string line)
    {
        string[] data = line.Split(";");
        return new Persona(data[0], data[1], data[2], int.Parse(data[3]), decimal.Parse(data[4]), data[5]);
    }

    public override string ToString()
    {
        return $"Cédula: {Cedula}; Nombre: {FullName}; Edad: {Edad}; Género: {Genero}; Estado: {Estado}; Grado: {Grado}; Ahorros: {Ahorros}; Contraseña: {Password}";
    }

    public override bool Equals(object obj)
    {
        if (obj is Persona other){
            return Cedula.Equals(other.Cedula);
        }

        return false;
    }

    public string ToWrite()
    {
        return $"{Cedula};{Name};{LastName};{HiddenData};{Ahorros};{Password}";
    }
}

public enum Genero
{
    Masculino = 0,
    Femenino = 1
}

public enum EstadoCivil
{
    Soltero = 0,
    Casado = 1
}

public enum GradoAcademico
{
    Inicial = 0,
    Media = 1,
    Grado = 2,
    PostGrado = 3
}