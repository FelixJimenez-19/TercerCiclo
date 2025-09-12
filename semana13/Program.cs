using System;
using System.Collections.Generic;
using System.Linq;

// Clase principal que gestiona el catálogo y la búsqueda
public class CatalogoRevistas
{
    private List<string> revistas;

    public CatalogoRevistas()
    {
        revistas = new List<string>();
        //  títulos de revistas
        revistas.Add("Don Quijote");
        revistas.Add("La Casa de Papel");
        revistas.Add("Harry Potter");
        revistas.Add("El País");
        revistas.Add("The New York Times");
        revistas.Add("la Barrera del no ");
        revistas.Add("Cien años de soledad");
        revistas.Add("El Señor de los Anillos");
        revistas.Add("1984");
        revistas.Add("la culpa es de la vaca");
        revistas.Add("Pollito "); 
    }

    // Método para realizar la búsqueda iterativa de un título
    public bool BuscarTituloIterativo(string tituloBuscado)
    {
        // Convertimos el título buscado a minúsculas para una búsqueda insensible a mayúsculas/minúsculas
        string tituloNormalizado = tituloBuscado.ToLower();

        // Iteramos sobre cada título en el catálogo
        foreach (string revista in revistas)
        {
            // Normalizamos el título de la revista en el catálogo para comparar
            if (revista.ToLower().Contains(tituloNormalizado)) // Usamos Contains para búsqueda parcial
            {
                return true; // Título encontrado
            }
        }
        return false; // Título no encontrado después de revisar todo el catálogo
    }

    // Método para realizar la búsqueda recursiva de un título (Opcional, pero se puede añadir)
    // Para una búsqueda recursiva, normalmente se necesita un índice y una función auxiliar.
    public bool BuscarTituloRecursivo(string tituloBuscado)
    {
        return BuscarTituloRecursivoHelper(tituloBuscado.ToLower(), 0);
    }

    private bool BuscarTituloRecursivoHelper(string tituloNormalizado, int indice)
    {
        // Caso base: si el índice excede el número de revistas, no se encontró
        if (indice >= revistas.Count)
        {
            return false;
        }

        // Si el título actual contiene el título buscado
        if (revistas[indice].ToLower().Contains(tituloNormalizado))
        {
            return true; // Título encontrado
        }

        // Llamada recursiva con el siguiente índice
        return BuscarTituloRecursivoHelper(tituloNormalizado, indice + 1);
    }


    public void MostrarCatalogo()
    {
        Console.WriteLine("\n--- Catálogo de Revistas ---");
        foreach (string revista in revistas)
        {
            Console.WriteLine($"- {revista}");
        }
        Console.WriteLine("---------------------------\n");
    }
}

// Clase principal del programa para la interacción con el usuario
class Program
{
    static void Main(string[] args)
    {
        CatalogoRevistas miCatalogo = new CatalogoRevistas();
        bool salir = false;

        while (!salir)
        {
            Console.WriteLine("--- Menú de Catálogo de Revistas ---");
            Console.WriteLine("1. Buscar título de revista (Iterativo)");
            Console.WriteLine("2. Buscar título de revista (Recursivo)"); // Opción para el método recursivo
            Console.WriteLine("3. Mostrar catálogo completo");
            Console.WriteLine("4. Salir");
            Console.Write("Seleccione una opción: ");

            string opcion = Console.ReadLine();
            Console.WriteLine();

            switch (opcion)
            {
                case "1":
                    Console.Write("Ingrese el título a buscar: ");
                    string tituloABuscarIterativo = Console.ReadLine();
                    if (miCatalogo.BuscarTituloIterativo(tituloABuscarIterativo))
                    {
                        Console.WriteLine($"'{tituloABuscarIterativo}' -> Encontrado.");
                    }
                    else
                    {
                        Console.WriteLine($"'{tituloABuscarIterativo}' -> No encontrado.");
                    }
                    break;
                case "2":
                    Console.Write("Ingrese el título a buscar (recursivo): ");
                    string tituloABuscarRecursivo = Console.ReadLine();
                    if (miCatalogo.BuscarTituloRecursivo(tituloABuscarRecursivo))
                    {
                        Console.WriteLine($"'{tituloABuscarRecursivo}' -> Encontrado.");
                    }
                    else
                    {
                        Console.WriteLine($"'{tituloABuscarRecursivo}' -> No encontrado.");
                    }
                    break;
                case "3":
                    miCatalogo.MostrarCatalogo();
                    break;
                case "4":
                    salir = true;
                    Console.WriteLine("Saliendo de la aplicación. ¡Hasta luego!");
                    break;
                default:
                    Console.WriteLine("Opción inválida. Por favor, intente de nuevo.");
                    break;
            }
            Console.WriteLine();
        }
    }
}

