
// ejercicio08.cs
// Ejercicio 08: Simulación de un congreso con registro de asistentes en un auditorio
// Este programa simula el registro de asistentes en un auditorio con un máximo de 100 asientos.
using System;
using System.Collections.Generic;
using System.Diagnostics;


// Clase principal que simula el registro de asistentes en un auditorio
class CongresoAuditorio
{
    // Definimos el número total de asientos y un arreglo para almacenar los registros
    const int TOTAL_ASIENTOS = 100;
    // Arreglo para almacenar los asientos ocupados
    static string[] asientos = new string[TOTAL_ASIENTOS];
    // Variable para llevar el control del siguiente asiento disponible
    static int siguienteAsiento = 0;

    // Simula un registro hecho por una persona
    static void RegistrarAsistente(string persona, Queue<string> cola)
    {
        // Verificamos si la cola de asistentes está vacía

        while (cola.Count > 0)
        {
            // Tomamos el siguiente asistente de la cola
            string asistente = cola.Dequeue();
            // Si hay asientos disponibles, asignamos el asiento al asistente
            // y mostramos un mensaje de confirmación
            if (siguienteAsiento < TOTAL_ASIENTOS)
            {
                // Asignamos el asiento al asistente y registramos quién lo hizo
                asientos[siguienteAsiento] = $"{asistente} (Registrado por {persona})";
                // Mostramos el mensaje de confirmación
                Console.WriteLine($"Asiento #{siguienteAsiento + 1}: {asistente} asignado por {persona}");
                siguienteAsiento++;
            }
            else
            {
                // Si no hay asientos disponibles, mostramos un mensaje de error
                Console.WriteLine($"Auditorio lleno. {asistente} no pudo entrar.");
                break;
            }
        }
    }
    // Método para mostrar los asientos asignados
    static void MostrarAsientos()
    {
        // Mostramos los asientos asignados
        Console.WriteLine("\n--- Asientos Asignados ---");
        // Recorremos el arreglo de asientos y mostramos quién está sentado en cada uno
        for (int i = 0; i < TOTAL_ASIENTOS; i++)
        {
            // Si el asiento está ocupado, mostramos el nombre del asistente
            // Si el asiento está vacío, mostramos que está vacío
            if (asientos[i] != null)
                Console.WriteLine($"Asiento #{i + 1}: {asientos[i]}");
            else
                Console.WriteLine($"Asiento #{i + 1}: [Vacío]");
        }
    }
    // Método principal para iniciar la simulación
    static void Main()
    {
        // Iniciamos un cronómetro para medir el tiempo de ejecución
        Stopwatch reloj = Stopwatch.StartNew();
        // Simulamos 60 asistentes para cada línea de ingreso
        Queue<string> fila1 = new Queue<string>();
        Queue<string> fila2 = new Queue<string>();
        // Llenamos las colas de asistentes con nombres ficticios
        // Puedes cambiar los nombres o la cantidad de asistentes según sea necesario
        for (int i = 1; i <= 60; i++) fila1.Enqueue("AsistenteF1_" + i);
        for (int i = 1; i <= 60; i++) fila2.Enqueue("AsistenteF2_" + i);

        Console.WriteLine("Iniciando registro...\n");
        reloj.Stop();

        // Simular registro secuencial (puedes usar hilos para hacerlo concurrente)
        RegistrarAsistente("Persona 1", fila1);
        RegistrarAsistente("Persona 2", fila2);

        MostrarAsientos();
        // Detenemos el cronómetro y mostramos el tiempo de ejecución
        Console.WriteLine($"\nTiempo de ejecución: {reloj.Elapsed.TotalMilliseconds} ms");

    }
}
