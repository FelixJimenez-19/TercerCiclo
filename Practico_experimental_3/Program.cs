using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Clase que representa un libro en la biblioteca.
/// Contiene información básica y métodos para comparación y visualización.
/// </summary>
public class Libro
{
    /// <summary>
    /// Identificador único del libro.
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Título del libro.
    /// </summary>
    public string Titulo { get; set; }
    /// <summary>
    /// Autor del libro.
    /// </summary>
    public string Autor { get; set; }
    /// <summary>
    /// Año de publicación del libro.
    /// </summary>
    public int AnioPublicacion { get; set; }
    /// <summary>
    /// Indica si el libro está disponible para préstamo.
    /// </summary>
    public bool EstaDisponible { get; set; }

    /// <summary>
    /// Constructor para inicializar un libro con todos sus datos.
    /// </summary>
    public Libro(int id, string titulo, string autor, int anioPublicacion, bool estaDisponible)
    {
        Id = id;
        Titulo = titulo;
        Autor = autor;
        AnioPublicacion = anioPublicacion;
        EstaDisponible = estaDisponible;
    }

    /// <summary>
    /// Devuelve una representación legible del libro.
    /// </summary>
    public override string ToString()
    {
        string disponibilidad = EstaDisponible ? "Disponible" : "No disponible";
        return $"ID: {Id}, Título: \"{Titulo}\", Autor: {Autor}, Año: {AnioPublicacion}, Estado: {disponibilidad}";
    }

    /// <summary>
    /// Compara dos libros por su ID para unicidad en colecciones.
    /// </summary>
    public override bool Equals(object obj)
    {
        if (obj is Libro otroLibro)
        {
            return this.Id == otroLibro.Id;
        }
        return false;
    }

    /// <summary>
    /// Devuelve el código hash basado en el ID del libro.
    /// </summary>
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}

/// <summary>
/// Clase que gestiona la biblioteca, permitiendo agregar, buscar, editar, eliminar y listar libros.
/// Utiliza colecciones para garantizar unicidad y eficiencia en búsquedas.
/// </summary>
public class Biblioteca
{
    // HashSet para garantizar la unicidad de los libros por su ID
    private HashSet<Libro> librosUnicos;
    // Dictionary para búsqueda rápida por ID
    private Dictionary<int, Libro> librosPorId;
    // Controla el próximo ID a asignar
    private int proximoId = 1;

    /// <summary>
    /// Constructor que inicializa las colecciones de la biblioteca.
    /// </summary>
    public Biblioteca()
    {
        librosUnicos = new HashSet<Libro>();
        librosPorId = new Dictionary<int, Libro>();
    }

    /// <summary>
    /// Agrega un nuevo libro a la biblioteca si no existe uno igual (por título y autor).
    /// </summary>
    public void AgregarLibro(string titulo, string autor, int anioPublicacion)
    {
        // Verifica si el libro ya existe usando el HashSet
        if (librosUnicos.Any(l => l.Titulo.Equals(titulo, StringComparison.OrdinalIgnoreCase) && l.Autor.Equals(autor, StringComparison.OrdinalIgnoreCase)))
        {
            Console.WriteLine("El libro ya existe en la biblioteca.");
            return;
        }
        // Crea y agrega el nuevo libro
        Libro nuevoLibro = new Libro(proximoId++, titulo, autor, anioPublicacion, true);
        librosUnicos.Add(nuevoLibro);
        librosPorId.Add(nuevoLibro.Id, nuevoLibro);
        Console.WriteLine($"Libro agregado: {nuevoLibro.Titulo}");
    }

    /// <summary>
    /// Busca un libro por su ID.
    /// </summary>
    public Libro BuscarLibroPorId(int id)
    {
        if (librosPorId.ContainsKey(id))
        {
            return librosPorId[id];
        }
        return null;
    }

    /// <summary>
    /// Edita los detalles de un libro existente.
    /// </summary>
    public void EditarLibro(int id, string nuevoTitulo, string nuevoAutor, int nuevoAnio)
    {
        Libro libroExistente = BuscarLibroPorId(id);
        if (libroExistente != null)
        {
            // Remueve y vuelve a agregar para actualizar el HashSet
            librosUnicos.Remove(libroExistente);
            libroExistente.Titulo = nuevoTitulo;
            libroExistente.Autor = nuevoAutor;
            libroExistente.AnioPublicacion = nuevoAnio;
            librosUnicos.Add(libroExistente);
            Console.WriteLine("Libro editado exitosamente.");
        }
        else
        {
            Console.WriteLine("Libro no encontrado.");
        }
    }

    /// <summary>
    /// Elimina un libro de la biblioteca por su ID.
    /// </summary>
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

    /// <summary>
    /// Cambia la disponibilidad de un libro (prestado/disponible).
    /// </summary>
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

    /// <summary>
    /// Lista todos los libros de la biblioteca.
    /// </summary>
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

    /// <summary>
    /// Lista solo los libros disponibles para préstamo.
    /// </summary>
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

    /// <summary>
    /// Lista solo los libros que no están disponibles (prestados).
    /// </summary>
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

/// <summary>
/// Clase principal del programa. Gestiona la interacción con el usuario y el menú de opciones.
/// </summary>
class Program
{
    static void Main(string[] args)
    {
        Biblioteca miBiblioteca = new Biblioteca();
        bool salir = false;

        // Bucle principal del menú de la biblioteca
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
                    // Agregar libro
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
                    break;
                case "2":
                    // Buscar libro por ID
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
                    break;
                case "3":
                    // Editar libro
                    Console.Write("ID del libro a editar: ");
                    if (int.TryParse(Console.ReadLine(), out int idEditar))
                    {
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
                    }
                    else
                    {
                        Console.WriteLine("ID inválido. Por favor, intente de nuevo.");
                    }
                    break;
                case "4":
                    // Eliminar libro
                    Console.Write("ID del libro a eliminar: ");
                    if (int.TryParse(Console.ReadLine(), out int idEliminar))
                    {
                        miBiblioteca.EliminarLibro(idEliminar);
                    }
                    else
                    {
                        Console.WriteLine("ID inválido. Por favor, intente de nuevo.");
                    }
                    break;
                case "5":
                    // Cambiar disponibilidad
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
                    break;
                case "6":
                    // Listar todos los libros
                    miBiblioteca.ListarTodosLosLibros();
                    break;
                case "7":
                    // Listar libros disponibles
                    miBiblioteca.ListarLibrosDisponibles();
                    break;
                case "8":
                    // Listar libros no disponibles
                    miBiblioteca.ListarLibrosNoDisponibles();
                    break;
                case "9":
                    // Salir
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
