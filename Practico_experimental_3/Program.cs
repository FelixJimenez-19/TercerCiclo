

using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

// Clase principal que representa un libro
public class Libro
{
    // Atributos
    public int Id { get; set; }
    public string Titulo { get; set; }
    public string Autor { get; set; }
    public int AnioPublicacion { get; set; }
    public bool EstaDisponible { get; set; }

    // Constructor
    public Libro(int id, string titulo, string autor, int anioPublicacion, bool estaDisponible)
    {
        Id = id;
        Titulo = titulo;
        Autor = autor;
        AnioPublicacion = anioPublicacion;
        EstaDisponible = estaDisponible;
    }

    // Sobrescribir el método ToString para una representación legible del objeto
    public override string ToString()
    {
        string disponibilidad = EstaDisponible ? "Disponible" : "No disponible";
        return $"ID: {Id}, Título: \"{Titulo}\", Autor: {Autor}, Año: {AnioPublicacion}, Estado: {disponibilidad}";
    }

    // Sobrescribir Equals y GetHashCode para que los objetos Libro puedan ser usados en un HashSet
    public override bool Equals(object obj)
    {
        if (obj is Libro otroLibro)
        {
            return this.Id == otroLibro.Id;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}

// Clase que gestiona la biblioteca, usando colecciones para la gestión de libros
public class Biblioteca
{
    // Usamos un HashSet para garantizar la unicidad de los libros por su ID
    private HashSet<Libro> librosUnicos;
    // Usamos un Dictionary para una búsqueda rápida por ID
    private Dictionary<int, Libro> librosPorId;
    private int proximoId = 1;

    public Biblioteca()
    {
        librosUnicos = new HashSet<Libro>();
        librosPorId = new Dictionary<int, Libro>();
    }

    // Método para agregar un nuevo libro
    public void AgregarLibro(string titulo, string autor, int anioPublicacion)
    {
        // Verificar si el libro ya existe usando el HashSet
        if (librosUnicos.Any(l => l.Titulo.Equals(titulo, StringComparison.OrdinalIgnoreCase) && l.Autor.Equals(autor, StringComparison.OrdinalIgnoreCase)))
        {
            Console.WriteLine("El libro ya existe en la biblioteca.");
            return;
        }

        Libro nuevoLibro = new Libro(proximoId++, titulo, autor, anioPublicacion, true);
        librosUnicos.Add(nuevoLibro);
        librosPorId.Add(nuevoLibro.Id, nuevoLibro);
        Console.WriteLine($"Libro agregado: {nuevoLibro.Titulo}");
    }

    // Método para buscar un libro por su ID
    public Libro BuscarLibroPorId(int id)
    {
        if (librosPorId.ContainsKey(id))
        {
            return librosPorId[id];
        }
        return null;
    }

    // Método para editar los detalles de un libro
    public void EditarLibro(int id, string nuevoTitulo, string nuevoAutor, int nuevoAnio)
    {
        Libro libroExistente = BuscarLibroPorId(id);
        if (libroExistente != null)
        {
            // Remover y agregar para asegurar la actualización en el HashSet
            librosUnicos.Remove(libroExistente);
            libroExistente.Titulo = nuevoTitulo;
            libroExistente.Autor = nuevoAutor;
            libroExistente.AnioPublicacion = nuevoAnio;
            librosUnicos.Add(libroExistente); // Vuelve a agregar el libro al HashSet
            Console.WriteLine("Libro editado exitosamente.");
        }
        else
        {
            Console.WriteLine("Libro no encontrado.");
        }
    }

    // Método para eliminar un libro
    public void EliminarLibro(int id)
    {
        Libro libroAEliminar = BuscarLibroPorId(id);
        if (libroAEliminar != null)
        {
            librosUnicos.Remove(libroAEliminar);
            librosPorId.Remove(id);
            Console.WriteLine($"Libro eliminado: {libroAEliminar.Titulo}");
        }
        else
        {
            Console.WriteLine("Libro no encontrado.");
        }
    }

    // Método para cambiar la disponibilidad del libro
    public void CambiarDisponibilidad(int id, bool estaDisponible)
    {
        Libro libro = BuscarLibroPorId(id);
        if (libro != null)
        {
            libro.EstaDisponible = estaDisponible;
            Console.WriteLine($"La disponibilidad de \"{libro.Titulo}\" ha sido actualizada a {(estaDisponible ? "Disponible" : "No disponible")}.");
        }
        else
        {
            Console.WriteLine("Libro no encontrado.");
        }
    }

    // Método de reportería para listar todos los libros
    public void ListarTodosLosLibros()
    {
        if (librosUnicos.Count == 0)
        {
            Console.WriteLine("No hay libros en la biblioteca.");
            return;
        }

        Console.WriteLine("\n--- Lista de todos los libros ---");
        foreach (var libro in librosUnicos)
        {
            Console.WriteLine(libro);
        }
        Console.WriteLine("----------------------------------");
    }

    // Método de reportería para listar libros disponibles
    public void ListarLibrosDisponibles()
    {
        var disponibles = librosUnicos.Where(l => l.EstaDisponible).ToList();
        if (disponibles.Count == 0)
        {
            Console.WriteLine("No hay libros disponibles en este momento.");
            return;
        }

        Console.WriteLine("\n--- Libros disponibles ---");
        foreach (var libro in disponibles)
        {
            Console.WriteLine(libro);
        }
        Console.WriteLine("--------------------------");
    }

    // Método de reportería para listar libros no disponibles
    public void ListarLibrosNoDisponibles()
    {
        var noDisponibles = librosUnicos.Where(l => !l.EstaDisponible).ToList();
        if (noDisponibles.Count == 0)
        {
            Console.WriteLine("No hay libros prestados en este momento.");
            return;
        }

        Console.WriteLine("\n--- Libros no disponibles ---");
        foreach (var libro in noDisponibles)
        {
            Console.WriteLine(libro);
        }
        Console.WriteLine("-----------------------------");
    }
}

// Clase principal del programa para la interacción con el usuario
class Program
{
    // Método auxiliar para medir el tiempo de ejecución de cualquier acción
    static void MeasureExecutionTime(Action action, string operationName)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        action();
        stopwatch.Stop();
        Console.WriteLine($"\nOperación '{operationName}' ejecutada en {stopwatch.ElapsedMilliseconds} ms.");
    }

    static void Main(string[] args)
    {
        Biblioteca miBiblioteca = new Biblioteca();
        bool salir = false;

        // Bucle principal del menú
        while (!salir)
        {
            Console.WriteLine("\n--- Menú de la Biblioteca ---");
            Console.WriteLine("1. Agregar libro");
            Console.WriteLine("2. Buscar libro por ID");
            Console.WriteLine("3. Editar libro");
            Console.WriteLine("4. Eliminar libro");
            Console.WriteLine("5. Cambiar disponibilidad de libro");
            Console.WriteLine("6. Listar todos los libros");
            Console.WriteLine("7. Listar libros disponibles");
            Console.WriteLine("8. Listar libros no disponibles");
            Console.WriteLine("9. Salir");
            Console.Write("Seleccione una opción: ");

            string opcion = Console.ReadLine();
            Console.WriteLine();

            switch (opcion)
            {
                case "1":
                    MeasureExecutionTime(() => {
                        Console.Write("Título: ");
                        string titulo = Console.ReadLine();
                        Console.Write("Autor: ");
                        string autor = Console.ReadLine();
                        Console.Write("Año de publicación: ");
                        if (int.TryParse(Console.ReadLine(), out int anio))
                        {
                            miBiblioteca.AgregarLibro(titulo, autor, anio);
                        }
                        else
                        {
                            Console.WriteLine("Año inválido. Por favor, intente de nuevo.");
                        }
                    }, "Agregar libro");
                    break;
                case "2":
                    MeasureExecutionTime(() => {
                        Console.Write("ID del libro a buscar: ");
                        if (int.TryParse(Console.ReadLine(), out int idBuscar))
                        {
                            Libro libroEncontrado = miBiblioteca.BuscarLibroPorId(idBuscar);
                            if (libroEncontrado != null)
                            {
                                Console.WriteLine($"Libro encontrado: {libroEncontrado}");
                            }
                            else
                            {
                                Console.WriteLine("Libro no encontrado.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("ID inválido. Por favor, intente de nuevo.");
                        }
                    }, "Buscar libro por ID");
                    break;
                case "3":
                    Console.Write("ID del libro a editar: ");
                    if (int.TryParse(Console.ReadLine(), out int idEditar))
                    {
                        MeasureExecutionTime(() => {
                            Libro libroAEditar = miBiblioteca.BuscarLibroPorId(idEditar);
                            if (libroAEditar != null)
                            {
                                Console.WriteLine($"Editando: {libroAEditar}");
                                Console.Write("Nuevo título: ");
                                string nuevoTitulo = Console.ReadLine();
                                Console.Write("Nuevo autor: ");
                                string nuevoAutor = Console.ReadLine();
                                Console.Write("Nuevo año de publicación: ");
                                if (int.TryParse(Console.ReadLine(), out int nuevoAnio))
                                {
                                    miBiblioteca.EditarLibro(idEditar, nuevoTitulo, nuevoAutor, nuevoAnio);
                                }
                                else
                                {
                                    Console.WriteLine("Año inválido.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Libro no encontrado.");
                            }
                        }, "Editar libro");
                    }
                    else
                    {
                        Console.WriteLine("ID inválido. Por favor, intente de nuevo.");
                    }
                    break;
                case "4":
                    MeasureExecutionTime(() => {
                        Console.Write("ID del libro a eliminar: ");
                        if (int.TryParse(Console.ReadLine(), out int idEliminar))
                        {
                            miBiblioteca.EliminarLibro(idEliminar);
                        }
                        else
                        {
                            Console.WriteLine("ID inválido. Por favor, intente de nuevo.");
                        }
                    }, "Eliminar libro");
                    break;
                case "5":
                    MeasureExecutionTime(() => {
                        Console.Write("ID del libro para cambiar su disponibilidad: ");
                        if (int.TryParse(Console.ReadLine(), out int idDisponibilidad))
                        {
                            Console.Write("¿Está disponible? (s/n): ");
                            string disponibleInput = Console.ReadLine().ToLower();
                            if (disponibleInput == "s")
                            {
                                miBiblioteca.CambiarDisponibilidad(idDisponibilidad, true);
                            }
                            else if (disponibleInput == "n")
                            {
                                miBiblioteca.CambiarDisponibilidad(idDisponibilidad, false);
                            }
                            else
                            {
                                Console.WriteLine("Entrada inválida. Use 's' para sí y 'n' para no.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("ID inválido. Por favor, intente de nuevo.");
                        }
                    }, "Cambiar disponibilidad");
                    break;
                case "6":
                    MeasureExecutionTime(() => {
                        miBiblioteca.ListarTodosLosLibros();
                    }, "Listar todos los libros");
                    break;
                case "7":
                    MeasureExecutionTime(() => {
                        miBiblioteca.ListarLibrosDisponibles();
                    }, "Listar libros disponibles");
                    break;
                case "8":
                    MeasureExecutionTime(() => {
                        miBiblioteca.ListarLibrosNoDisponibles();
                    }, "Listar libros no disponibles");
                    break;
                case "9":
                    salir = true;
                    Console.WriteLine("Saliendo de la aplicación. ¡Hasta luego!");
                    break;
                default:
                    Console.WriteLine("Opción inválida. Por favor, seleccione una opción del 1 al 9.");
                    break;
            }
        }
    }
}