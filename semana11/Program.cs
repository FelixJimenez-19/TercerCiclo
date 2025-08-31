using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Programa traductor de español a inglés que permite traducir frases palabra por palabra
/// y agregar nuevas palabras al diccionario de traducciones.
/// 
/// Características principales:
/// - Traducción de frases completas de español a inglés palabra por palabra
/// - Diccionario predefinido con palabras comunes en español
/// - Capacidad de agregar nuevas traducciones al diccionario
/// - Manejo inteligente de signos de puntuación
/// - Interfaz de usuario mediante menú interactivo
/// - Búsquedas insensibles a mayúsculas/minúsculas
/// </summary>
class Program
{
    /// <summary>
    /// Diccionario estático que almacena las traducciones de español a inglés.
    /// Utiliza StringComparer.OrdinalIgnoreCase para hacer búsquedas insensibles a mayúsculas/minúsculas.
    /// 
    /// Estructura del diccionario:
    /// - Clave: palabra en español (string) - palabra a traducir
    /// - Valor: traducción en inglés (string) - palabra traducida
    /// 
    /// Contiene 21 palabras comunes predefinidas para facilitar las traducciones básicas.
    /// </summary>
    private static Dictionary<string, string> spanishToEnglish = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        {"tiempo", "time"},
        {"persona", "person"},
        {"año", "year"},
        {"camino", "way"},
        {"dia", "day"},
        {"cosa", "thing"},
        {"hombre", "man"},
        {"mundo", "world"},
        {"vida", "life"},
        {"mano", "hand"},
        {"parte", "part"},
        {"niño/a", "child"},
        {"ojo", "eye"},
        {"mujer", "woman"},
        {"lugar", "place"},
        {"trabajo", "work"},
        {"semana", "week"},
        {"caso", "case"},
        {"punto", "point"},
        {"gobierno", "government"},
        {"empresa", "company"}
    };

    /// <summary>
    /// Punto de entrada principal del programa.
    /// Ejecuta un bucle que muestra el menú de opciones hasta que el usuario decida salir.
    /// 
    /// Funcionalidades disponibles:
    /// 1. Traducir frases completas de español a inglés
    /// 2. Agregar nuevas palabras al diccionario
    /// 0. Salir del programa
    /// </summary>
    /// <param name="args">Argumentos de línea de comandos (no utilizados en este programa)</param>
    static void Main(string[] args)
    {
        string option; // Variable para almacenar la opción seleccionada por el usuario
        
        // Bucle principal del programa que se ejecuta hasta que el usuario elija salir
        do
        {
            ShowMenu(); // Mostrar las opciones disponibles
            option = Console.ReadLine(); // Leer la opción del usuario
            Console.WriteLine(); // Línea en blanco para mejorar la legibilidad

            // Evaluar la opción seleccionada y ejecutar la acción correspondiente
            switch (option)
            {
                case "1":
                    TranslatePhrase(); // Traducir una frase de español a inglés
                    break;
                case "2":
                    AddWord(); // Agregar una nueva palabra al diccionario
                    break;
                case "0":
                    Console.WriteLine("Saliendo del programa. ¡Hasta pronto!");
                    break;
                default:
                    Console.WriteLine("Opción no válida. Por favor, seleccione una opción del menú.");
                    break;
            }
            Console.WriteLine(); // Línea en blanco para separar las operaciones
        } while (option != "0"); // Continuar hasta que el usuario elija salir
    }

    /// <summary>
    /// Muestra el menú de opciones al usuario.
    /// Presenta una interfaz clara con las opciones disponibles numeradas
    /// y solicita al usuario que seleccione una opción.
    /// </summary>
    static void ShowMenu()
    {
        Console.WriteLine("==================== MENÚ ====================");
        Console.WriteLine("1. Traducir una frase");
        Console.WriteLine("2. Agregar palabras al diccionario");
        Console.WriteLine("0. Salir");
        Console.Write("Seleccione una opción: ");
    }

    /// <summary>
    /// Solicita al usuario una frase en español y la traduce palabra por palabra al inglés.
    /// 
    /// Proceso de traducción:
    /// 1. Solicita al usuario que ingrese una frase en español
    /// 2. Valida que la entrada no esté vacía
    /// 3. Divide la frase en palabras individuales usando múltiples separadores
    /// 4. Limpia cada palabra de signos de puntuación y convierte a minúsculas
    /// 5. Busca cada palabra limpia en el diccionario de traducciones
    /// 6. Reemplaza las palabras encontradas con su traducción en inglés
    /// 7. Mantiene las palabras no encontradas en su idioma original
    /// 8. Une todas las palabras y muestra la frase traducida al usuario
    /// </summary>
    static void TranslatePhrase()
    {
        Console.Write("Ingrese la frase en español a traducir: ");
        string phrase = Console.ReadLine();
        
        // Validación de entrada: verificar que se haya ingresado una frase válida
        if (string.IsNullOrWhiteSpace(phrase))
        {
            Console.WriteLine("No se ingresó ninguna frase. Volviendo al menú.");
            return;
        }

        // Dividir la frase en palabras individuales
        // Se utilizan múltiples separadores: espacio, comas, puntos, punto y coma, dos puntos, signos de exclamación y interrogación
        // StringSplitOptions.RemoveEmptyEntries elimina entradas vacías que pueden resultar de espacios múltiples o separadores consecutivos
        string[] words = phrase.Split(new char[] { ' ', ',', '.', ';', ':', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);

        // Procesar cada palabra de la frase original usando LINQ
        string[] translatedWords = words.Select(word =>
        {
            // Limpiar la palabra de signos de puntuación y convertir a minúsculas
            // Se mantienen solo letras y dígitos para la búsqueda en el diccionario
            // Esto permite manejar palabras con apostrofes, guiones, etc.
            string cleanWord = new string(word.Where(c => char.IsLetterOrDigit(c)).ToArray()).ToLower();

            // Buscar la palabra limpia en español en el diccionario y devolver la traducción en inglés
            if (spanishToEnglish.ContainsKey(cleanWord))
            {
                // Si se encuentra la palabra, devolver la traducción en inglés
                return spanishToEnglish[cleanWord];
            }
            else
            {
                // Si no se encuentra en el diccionario, mantener la palabra original
                // Esto permite que el programa maneje palabras no traducidas de manera elegante
                return word;
            }
        }).ToArray();

        // Unir todas las palabras (traducidas y originales) para formar la frase final
        string translatedPhrase = string.Join(" ", translatedWords);
        Console.WriteLine($"Traducción: {translatedPhrase}");
    }

    /// <summary>
    /// Permite al usuario agregar una nueva pareja de palabras al diccionario.
    /// 
    /// Proceso de agregado:
    /// 1. Solicita la palabra en español al usuario
    /// 2. Solicita la traducción en inglés correspondiente
    /// 3. Valida que ambas palabras no estén vacías ni contengan solo espacios
    /// 4. Verifica que la palabra en español no exista ya en el diccionario para evitar duplicados
    /// 5. Agrega la nueva traducción al diccionario si cumple todas las validaciones
    /// 6. Informa al usuario el resultado de la operación (éxito o fallo)
    /// </summary>
    static void AddWord()
    {
        // Solicitar la palabra en español y preprocesarla
        Console.Write("Ingrese la palabra en español: ");
        string spanishWord = Console.ReadLine().ToLower().Trim(); // Convertir a minúsculas y eliminar espacios

        // Solicitar la traducción en inglés y preprocesarla
        Console.Write("Ingrese la traducción en inglés: ");
        string englishWord = Console.ReadLine().ToLower().Trim(); // Convertir a minúsculas y eliminar espacios

        // Validación de entrada: verificar que ninguna de las palabras esté vacía o contenga solo espacios
        if (string.IsNullOrWhiteSpace(spanishWord) || string.IsNullOrWhiteSpace(englishWord))
        {
            Console.WriteLine("Las palabras no pueden estar vacías. No se agregó nada.");
            return;
        }

        // Verificar si la palabra en español ya existe en el diccionario para evitar duplicados
        if (spanishToEnglish.ContainsKey(spanishWord))
        {
            Console.WriteLine($"La palabra '{spanishWord}' ya existe en el diccionario. No se agregó.");
        }
        else
        {
            // Agregar la nueva pareja de traducción al diccionario
            spanishToEnglish.Add(spanishWord, englishWord);
            Console.WriteLine($"Palabra '{spanishWord}' agregada al diccionario con la traducción '{englishWord}'.");
        }
    }
}


