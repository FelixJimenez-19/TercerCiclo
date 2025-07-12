// semana07/torre_hanoi.cs
// Programa para resolver el problema de las Torres de Hanoi.
// Utiliza recursión para mover discos entre torres.
using System;
using System.Collections.Generic;

// Clase que representa una torre en el juego de las Torres de Hanoi
// Contiene una pila de discos y un nombre para identificarla.
class Torre
{
    // Pila de discos, donde el disco más grande está en la parte inferior.
    // El nombre de la torre es un identificador para mostrar los movimientos.
    public Stack<int> Discos { get; private set; } = new Stack<int>();
    public string Nombre { get; private set; }

    // Constructor de la clase Torre
    public Torre(string nombre)
    {
        Nombre = nombre;
    }

    // Método para mover un disco de esta torre a otra torre destino.
    // Imprime el movimiento realizado.
    public void MoverDiscoA(Torre destino)
    {
        int disco = Discos.Pop();
        destino.Discos.Push(disco);
        Console.WriteLine($"Mover disco {disco} de {Nombre} a {destino.Nombre}");
    }
}

// Método recursivo para resolver el problema de las Torres de Hanoi
class Hanoi
{
    // Método que resuelve el problema de las Torres de Hanoi
    // n es el número de discos, origen es la torre de origen, destino es la torre de destino
    static void Resolver(int n, Torre origen, Torre destino, Torre auxiliar)
    {
        // Caso base: si hay un solo disco, lo movemos directamente
        // de la torre de origen a la torre de destino.
        if (n == 1)
        {
            origen.MoverDiscoA(destino);
            return;
        }
        // Mover n-1 discos de la torre de origen a la torre auxiliar
        Resolver(n - 1, origen, auxiliar, destino);
        origen.MoverDiscoA(destino);
        // Mover los discos de la torre auxiliar a la torre de destino
        Resolver(n - 1, auxiliar, destino, origen);
    }

    // Método principal para ejecutar el programa
    static void Main()
    {
        // Definimos el número de discos a mover.
        // Puedes cambiar este valor para probar con más discos.
        int numDiscos = 3;

        Torre origen = new Torre("A");
        Torre destino = new Torre("C");
        Torre auxiliar = new Torre("B");

        // Cargar discos (mayor abajo)
        for (int i = numDiscos; i >= 1; i--)
            origen.Discos.Push(i);

        Console.WriteLine("Solución de las Torres de Hanoi:");
        Resolver(numDiscos, origen, destino, auxiliar);
    }
}
