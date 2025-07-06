using System;

class Nodo
{
    public int Dato;
    public Nodo Siguiente;

    public Nodo(int dato)
    {
        Dato = dato;
        Siguiente = null;
    }
}

class ListaEnlazada
{
    private Nodo cabeza;

    public void AgregarAlFinal(int dato)
    {
        Nodo nuevo = new Nodo(dato);
        if (cabeza == null)
        {
            cabeza = nuevo;
        }
        else
        {
            Nodo actual = cabeza;
            while (actual.Siguiente != null)
            {
                actual = actual.Siguiente;
            }
            actual.Siguiente = nuevo;
        }
    }

    public void Mostrar()
    {
        Nodo actual = cabeza;
        while (actual != null)
        {
            Console.Write(actual.Dato + " -> ");
            actual = actual.Siguiente;
        }
        Console.WriteLine("null");
    }

    public void Invertir()
    {
        Nodo anterior = null;
        Nodo actual = cabeza;
        Nodo siguiente = null;

        while (actual != null)
        {
            siguiente = actual.Siguiente;
            actual.Siguiente = anterior;
            anterior = actual;
            actual = siguiente;
        }

        cabeza = anterior;
    }

    public int BuscarDato(int valor)
    {
        Nodo actual = cabeza;
        int contador = 0;

        while (actual != null)
        {
            if (actual.Dato == valor)
            {
                contador++;
            }
            actual = actual.Siguiente;
        }

        if (contador == 0)
        {
            Console.WriteLine($"El valor {valor} no fue encontrado en la lista.");
        }
        else
        {
            Console.WriteLine($"El valor {valor} se encuentra {contador} vez/veces en la lista.");
        }

        return contador;
    }
}

class Programa
{
    static void Main()
    {
        ListaEnlazada lista = new ListaEnlazada();

        lista.AgregarAlFinal(10);
        lista.AgregarAlFinal(20);
        lista.AgregarAlFinal(10);
        lista.AgregarAlFinal(30);
        lista.AgregarAlFinal(10);

        Console.WriteLine("Lista actual:");
        lista.Mostrar();

        Console.WriteLine();

        // Prueba de búsqueda
        lista.BuscarDato(10);   // Aparece 3 veces
        lista.BuscarDato(50);   // No aparece
    }
}
