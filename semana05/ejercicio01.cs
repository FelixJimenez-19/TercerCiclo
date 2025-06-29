// Ejercicio 1: Mostrar asignaturas
using System;
using System.Collections.Generic;

class Curso
{
    public List<string> Asignaturas { get; set; }

    public Curso()
    {
        Asignaturas = new List<string> { "Matemáticas", "Física", "Química", "Historia", "Lengua" };
    }

    public void MostrarAsignaturas()
    {
        foreach (var asignatura in Asignaturas)
        {
            Console.WriteLine(asignatura);
        }
    }
}

class Program
{
    static void Main()
    {
        Curso curso = new Curso();
        curso.MostrarAsignaturas();
    }
}
