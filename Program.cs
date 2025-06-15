using System;

namespace RegistroEstudiantes
{
    class Estudiante
    {
        // Propiedades del estudiante
        public int Id { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Direccion { get; set; }
        public string[] Telefonos { get; set; }

        // Constructor
        public Estudiante(int id, string nombres, string apellidos, string direccion, string[] telefonos)
        {
            Id = id;
            Nombres = nombres;
            Apellidos = apellidos;
            Direccion = direccion;
            Telefonos = telefonos;
        }

        // Método para mostrar los datos del estudiante
        public void MostrarInformacion()
        {
            Console.WriteLine("\n--- Información del Estudiante ---");
            Console.WriteLine($"ID: {Id}");
            Console.WriteLine($"Nombre Completo: {Nombres} {Apellidos}");
            Console.WriteLine($"Dirección: {Direccion}");
            Console.WriteLine("Teléfonos:");
            for (int i = 0; i < Telefonos.Length; i++)
            {
                Console.WriteLine($"  - Teléfono {i + 1}: {Telefonos[i]}");
            }
        }
    }
}




namespace RegistroEstudiantes
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Registro de Estudiante ===");

            // Solicita los datos básicos
            Console.Write("Ingrese el ID: ");
            int id = int.Parse(Console.ReadLine());

            Console.Write("Ingrese los nombres: ");
            string nombres = Console.ReadLine();

            Console.Write("Ingrese los apellidos: ");
            string apellidos = Console.ReadLine();

            Console.Write("Ingrese la dirección: ");
            string direccion = Console.ReadLine();

            // Crea el array para los teléfonos
            string[] telefonos = new string[3];
            for (int i = 0; i < 3; i++)
            {
                Console.Write($"Ingrese el teléfono {i + 1}: ");
                telefonos[i] = Console.ReadLine();
            }

            // Crear el objeto Estudiante
            Estudiante estudiante = new Estudiante(id, nombres, apellidos, direccion, telefonos);

            // Mostrar los datos
            estudiante.MostrarInformacion();

            Console.WriteLine("\nPresione cualquier tecla para salir...");
            Console.ReadKey();
        }
    }
}
