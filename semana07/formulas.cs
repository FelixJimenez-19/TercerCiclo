// semana07/formulas.cs// Programa para verificar si una expresión matemática está balanceada
// en términos de paréntesis, corchetes y llaves.
// Utiliza una pila para rastrear los símbolos de apertura y cierre.
using System;
using System.Collections.Generic;

class Program
{
    // Método para verificar si una expresión está balanceada
    // Retorna true si está balanceada, false en caso contrario.
    static bool EstaBalanceada(string expresion)
    {
        // Utilizamos una pila para almacenar los símbolos de apertura
        // y verificar su correspondencia con los de cierre.
        Stack<char> pila = new Stack<char>();
        // Recorremos cada carácter de la expresión
        // y verificamos si es un símbolo de apertura o cierre.
        // Si es de apertura, lo agregamos a la pila.
        foreach (char c in expresion)
        {
            if (c == '(' || c == '[' || c == '{')
                pila.Push(c);
            else if (c == ')' || c == ']' || c == '}')
            {
                if (pila.Count == 0) return false;

                char top = pila.Pop();
                if ((c == ')' && top != '(') ||
                    (c == ']' && top != '[') ||
                    (c == '}' && top != '{'))
                    return false;
            }
        }

        return pila.Count == 0;
    }

    // Método principal para ejecutar el programa
    static void Main()
    {
        // Ejemplo de expresión matemática para verificar
        // si está balanceada. Puedes cambiarla por cualquier otra.
        string expresion = "{7 + (8 * 5) - [(9 - 7) + (4 + 1)]}";
        // Verificamos si la expresión está balanceada
        if (EstaBalanceada(expresion))
            Console.WriteLine("Fórmula balanceada.");
        else
            Console.WriteLine("Fórmula no balanceada.");
    }
}
