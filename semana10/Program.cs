
using System; // Importa funcionalidades básicas del sistema
using System.Collections.Generic; // Importa colecciones genéricas como HashSet y List
using System.Linq; // Importa métodos de consulta LINQ
using System.IO; // Importa funcionalidades para manejo de archivos

namespace SistemaVacunacionCovid
{
    // Enumeración para tipos de vacuna
    public enum TipoVacuna
    {
        Pfizer, // Representa la vacuna Pfizer
        AstraZeneca // Representa la vacuna AstraZeneca
    }

    // Clase que representa un ciudadano
    public class Ciudadano
    {
        public int Id { get; set; } // Identificador único del ciudadano
        public string Nombre { get; set; } // Nombre del ciudadano
        public string Cedula { get; set; } // Cédula del ciudadano

        public Ciudadano(int id, string nombre, string cedula)
        {
            Id = id; // Asigna el ID recibido
            Nombre = nombre; // Asigna el nombre recibido
            Cedula = cedula; // Asigna la cédula recibida
        }

        public override bool Equals(object obj)
        {
            // Compara si dos ciudadanos tienen el mismo ID
            return obj is Ciudadano ciudadano && Id == ciudadano.Id;
        }

        public override int GetHashCode()
        {
            // Devuelve el hash basado en el ID
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            // Devuelve una representación en texto del ciudadano
            return $"ID: {Id}, Nombre: {Nombre}, Cédula: {Cedula}";
        }
    }

    // Clase que representa una vacunación
    public class RegistroVacunacion
    {
        public Ciudadano Ciudadano { get; set; } // Ciudadano vacunado
        public TipoVacuna TipoVacuna { get; set; } // Tipo de vacuna aplicada
        public DateTime FechaVacunacion { get; set; } // Fecha de vacunación
        public int NumeroDosis { get; set; } // Número de dosis recibidas

        public RegistroVacunacion(Ciudadano ciudadano, TipoVacuna tipoVacuna, DateTime fecha, int numeroDosis)
        {
            Ciudadano = ciudadano; // Asigna el ciudadano
            TipoVacuna = tipoVacuna; // Asigna el tipo de vacuna
            FechaVacunacion = fecha; // Asigna la fecha de vacunación
            NumeroDosis = numeroDosis; // Asigna el número de dosis
        }

        public override string ToString()
        {
            // Devuelve una representación en texto del registro de vacunación
            return $"{Ciudadano} - Vacuna: {TipoVacuna}, Dosis: {NumeroDosis}, Fecha: {FechaVacunacion:dd/MM/yyyy}";
        }
    }

    // Clase principal del sistema de vacunación
    public class SistemaVacunacion
    {
        // Conjuntos principales
        private HashSet<Ciudadano> _todosCiudadanos; // Todos los ciudadanos registrados
        private HashSet<Ciudadano> _vacunadosPfizer; // Ciudadanos vacunados con Pfizer
        private HashSet<Ciudadano> _vacunadosAstraZeneca; // Ciudadanos vacunados con AstraZeneca
        private List<RegistroVacunacion> _registrosVacunacion; // Lista de registros de vacunación

        public SistemaVacunacion()
        {
            // Inicializa los conjuntos y la lista
            _todosCiudadanos = new HashSet<Ciudadano>();
            _vacunadosPfizer = new HashSet<Ciudadano>();
            _vacunadosAstraZeneca = new HashSet<Ciudadano>();
            _registrosVacunacion = new List<RegistroVacunacion>();
        }

        // Método para generar datos ficticios
        public void GenerarDatosFicticios()
        {
            var random = new Random(); // Generador de números aleatorios
            
            // Generar 500 ciudadanos
            for (int i = 1; i <= 500; i++)
            {
                var ciudadano = new Ciudadano(i, $"Ciudadano {i}", $"17{i:D8}"); // Crea un ciudadano ficticio
                _todosCiudadanos.Add(ciudadano); // Agrega el ciudadano al conjunto
            }

            // Seleccionar aleatoriamente 75 ciudadanos para Pfizer
            var ciudadanosParaPfizer = _todosCiudadanos.OrderBy(x => random.Next()).Take(75).ToList(); // Selecciona 75 ciudadanos aleatorios
            foreach (var ciudadano in ciudadanosParaPfizer)
            {
                _vacunadosPfizer.Add(ciudadano); // Agrega al conjunto de vacunados con Pfizer
                
                // Agregar registros de vacunación (primera dosis)
                _registrosVacunacion.Add(new RegistroVacunacion(
                    ciudadano, 
                    TipoVacuna.Pfizer, 
                    DateTime.Now.AddDays(-random.Next(30, 180)), 
                    1
                ));
                
                // Algunos tendrán segunda dosis
                if (random.NextDouble() > 0.3) // 70% probabilidad de segunda dosis
                {
                    _registrosVacunacion.Add(new RegistroVacunacion(
                        ciudadano, 
                        TipoVacuna.Pfizer, 
                        DateTime.Now.AddDays(-random.Next(1, 120)), 
                        2
                    ));
                }
            }

            // Seleccionar aleatoriamente 75 ciudadanos para AstraZeneca (excluyendo los ya vacunados con Pfizer)
            var ciudadanosDisponibles = _todosCiudadanos.Except(_vacunadosPfizer).ToList(); // Excluye los vacunados con Pfizer
            var ciudadanosParaAstraZeneca = ciudadanosDisponibles.OrderBy(x => random.Next()).Take(75).ToList(); // Selecciona 75 ciudadanos aleatorios
            
            foreach (var ciudadano in ciudadanosParaAstraZeneca)
            {
                _vacunadosAstraZeneca.Add(ciudadano); // Agrega al conjunto de vacunados con AstraZeneca
                
                // Agregar registros de vacunación (primera dosis)
                _registrosVacunacion.Add(new RegistroVacunacion(
                    ciudadano, 
                    TipoVacuna.AstraZeneca, 
                    DateTime.Now.AddDays(-random.Next(30, 180)), 
                    1
                ));
                
                // Algunos tendrán segunda dosis
                if (random.NextDouble() > 0.25) // 75% probabilidad de segunda dosis
                {
                    _registrosVacunacion.Add(new RegistroVacunacion(
                        ciudadano, 
                        TipoVacuna.AstraZeneca, 
                        DateTime.Now.AddDays(-random.Next(1, 120)), 
                        2
                    ));
                }
            }
        }

        // 1. Ciudadanos que NO se han vacunado (usando diferencia de conjuntos)
        public HashSet<Ciudadano> ObtenerCiudadanosNoVacunados()
        {
            var vacunados = new HashSet<Ciudadano>(_vacunadosPfizer); // Copia los vacunados con Pfizer
            vacunados.UnionWith(_vacunadosAstraZeneca); // Une los vacunados con AstraZeneca
            
            return new HashSet<Ciudadano>(_todosCiudadanos.Except(vacunados)); // Devuelve los no vacunados
        }

        // 2. Ciudadanos que han recibido ambas dosis
        public HashSet<Ciudadano> ObtenerCiudadanosConAmbasDosis()
        {
            var conAmbasDosis = new HashSet<Ciudadano>(); // Conjunto de ciudadanos con ambas dosis
            
            var ciudadanosConDosDosisPfizer = _registrosVacunacion
                .Where(r => r.TipoVacuna == TipoVacuna.Pfizer)
                .GroupBy(r => r.Ciudadano)
                .Where(g => g.Count() >= 2)
                .Select(g => g.Key); // Ciudadanos con dos dosis de Pfizer
            
            var ciudadanosConDosDosisAstraZeneca = _registrosVacunacion
                .Where(r => r.TipoVacuna == TipoVacuna.AstraZeneca)
                .GroupBy(r => r.Ciudadano)
                .Where(g => g.Count() >= 2)
                .Select(g => g.Key); // Ciudadanos con dos dosis de AstraZeneca
            
            conAmbasDosis.UnionWith(ciudadanosConDosDosisPfizer); // Une ambos conjuntos
            conAmbasDosis.UnionWith(ciudadanosConDosDosisAstraZeneca);
            
            return conAmbasDosis; // Devuelve el conjunto
        }

        // 3. Ciudadanos que SOLO han recibido Pfizer (intersección y diferencia)
        public HashSet<Ciudadano> ObtenerCiudadanosSoloPfizer()
        {
            return new HashSet<Ciudadano>(_vacunadosPfizer.Except(_vacunadosAstraZeneca)); // Devuelve los vacunados solo con Pfizer
        }

        // 4. Ciudadanos que SOLO han recibido AstraZeneca (intersección y diferencia)
        public HashSet<Ciudadano> ObtenerCiudadanosSoloAstraZeneca()
        {
            return new HashSet<Ciudadano>(_vacunadosAstraZeneca.Except(_vacunadosPfizer)); // Devuelve los vacunados solo con AstraZeneca
        }

        // Método para generar estadísticas
        public void GenerarEstadisticas()
        {
            Console.WriteLine("=== ESTADÍSTICAS DEL SISTEMA DE VACUNACIÓN COVID-19 ===\n"); // Muestra título de estadísticas
            
            var noVacunados = ObtenerCiudadanosNoVacunados(); // Obtiene ciudadanos no vacunados
            var conAmbasDosis = ObtenerCiudadanosConAmbasDosis(); // Obtiene ciudadanos con ambas dosis
            var soloPfizer = ObtenerCiudadanosSoloPfizer(); // Obtiene ciudadanos solo con Pfizer
            var soloAstraZeneca = ObtenerCiudadanosSoloAstraZeneca(); // Obtiene ciudadanos solo con AstraZeneca
            
            Console.WriteLine($"Total de ciudadanos registrados: {_todosCiudadanos.Count}"); // Muestra total de ciudadanos
            Console.WriteLine($"Total vacunados con Pfizer: {_vacunadosPfizer.Count}"); // Muestra total vacunados con Pfizer
            Console.WriteLine($"Total vacunados con AstraZeneca: {_vacunadosAstraZeneca.Count}"); // Muestra total vacunados con AstraZeneca
            Console.WriteLine();
            
            Console.WriteLine("=== RESULTADOS DE OPERACIONES DE CONJUNTOS ===\n"); // Muestra título de resultados
            
            Console.WriteLine($"1. Ciudadanos NO vacunados: {noVacunados.Count}"); // Muestra cantidad de no vacunados
            MostrarPrimeros10(noVacunados); // Muestra primeros 10 no vacunados
            
            Console.WriteLine($"\n2. Ciudadanos con AMBAS dosis: {conAmbasDosis.Count}"); // Muestra cantidad con ambas dosis
            MostrarPrimeros10(conAmbasDosis); // Muestra primeros 10 con ambas dosis
            
            Console.WriteLine($"\n3. Ciudadanos SOLO con Pfizer: {soloPfizer.Count}"); // Muestra cantidad solo con Pfizer
            MostrarPrimeros10(soloPfizer); // Muestra primeros 10 solo con Pfizer
            
            Console.WriteLine($"\n4. Ciudadanos SOLO con AstraZeneca: {soloAstraZeneca.Count}"); // Muestra cantidad solo con AstraZeneca
            MostrarPrimeros10(soloAstraZeneca); // Muestra primeros 10 solo con AstraZeneca
        }

        private void MostrarPrimeros10(HashSet<Ciudadano> conjunto)
        {
            var primeros10 = conjunto.Take(10); // Toma los primeros 10 elementos del conjunto
            foreach (var ciudadano in primeros10)
            {
                Console.WriteLine($"   - {ciudadano}"); // Muestra cada ciudadano
            }
            if (conjunto.Count > 10)
            {
                Console.WriteLine($"   ... y {conjunto.Count - 10} más"); // Indica si hay más de 10
            }
        }

        // Método para exportar a archivo de texto
        public void ExportarReporte(string nombreArchivo = "reporte_vacunacion.txt")
        {
            using (var writer = new StreamWriter(nombreArchivo)) // Crea el archivo de reporte
            {
                writer.WriteLine("REPORTE DE VACUNACIÓN COVID-19"); // Escribe el título
                writer.WriteLine($"Fecha de generación: {DateTime.Now}"); // Escribe la fecha
                writer.WriteLine("================================\n"); // Escribe separador
                
                var noVacunados = ObtenerCiudadanosNoVacunados(); // Obtiene ciudadanos no vacunados
                var conAmbasDosis = ObtenerCiudadanosConAmbasDosis(); // Obtiene ciudadanos con ambas dosis
                var soloPfizer = ObtenerCiudadanosSoloPfizer(); // Obtiene ciudadanos solo con Pfizer
                var soloAstraZeneca = ObtenerCiudadanosSoloAstraZeneca(); // Obtiene ciudadanos solo con AstraZeneca
                
                writer.WriteLine($"RESUMEN EJECUTIVO:"); // Escribe resumen
                writer.WriteLine($"- Total ciudadanos: {_todosCiudadanos.Count}"); // Total ciudadanos
                writer.WriteLine($"- No vacunados: {noVacunados.Count} ({(noVacunados.Count * 100.0 / _todosCiudadanos.Count):F1}%)"); // No vacunados
                writer.WriteLine($"- Con esquema completo: {conAmbasDosis.Count} ({(conAmbasDosis.Count * 100.0 / _todosCiudadanos.Count):F1}%)"); // Esquema completo
                writer.WriteLine($"- Solo Pfizer: {soloPfizer.Count}"); // Solo Pfizer
                writer.WriteLine($"- Solo AstraZeneca: {soloAstraZeneca.Count}\n"); // Solo AstraZeneca
                
                EscribirSeccionReporte(writer, "CIUDADANOS NO VACUNADOS", noVacunados); // Escribe sección no vacunados
                EscribirSeccionReporte(writer, "CIUDADANOS CON AMBAS DOSIS", conAmbasDosis); // Escribe sección ambas dosis
                EscribirSeccionReporte(writer, "CIUDADANOS SOLO CON PFIZER", soloPfizer); // Escribe sección solo Pfizer
                EscribirSeccionReporte(writer, "CIUDADANOS SOLO CON ASTRAZENECA", soloAstraZeneca); // Escribe sección solo AstraZeneca
            }
            
            Console.WriteLine($"\nReporte exportado exitosamente a: {nombreArchivo}"); // Muestra mensaje de éxito
        }

        private void EscribirSeccionReporte(StreamWriter writer, string titulo, HashSet<Ciudadano> conjunto)
        {
            writer.WriteLine($"{titulo} ({conjunto.Count} registros):"); // Escribe título de la sección
            writer.WriteLine(new string('-', titulo.Length + 20)); // Escribe separador
            
            foreach (var ciudadano in conjunto)
            {
                writer.WriteLine(ciudadano.ToString()); // Escribe cada ciudadano
            }
            writer.WriteLine(); // Línea en blanco
        }
    }

    // Clase principal para ejecutar el programa
    class Program
    {
        static void Main(string[] args)
        {
            var sistema = new SistemaVacunacion(); // Instancia el sistema de vacunación
            
            Console.WriteLine("Generando datos ficticios..."); // Mensaje de inicio de generación de datos
            sistema.GenerarDatosFicticios(); // Genera los datos ficticios
            
            Console.WriteLine("Procesando datos con operaciones de conjuntos...\n"); // Mensaje de inicio de procesamiento
            sistema.GenerarEstadisticas(); // Genera y muestra estadísticas
            
            Console.WriteLine("\n¿Desea exportar el reporte a archivo? (s/n): "); // Pregunta al usuario si desea exportar
            var respuesta = Console.ReadLine(); // Lee la respuesta del usuario
            
            if (respuesta?.ToLower() == "s") // Si la respuesta es 's'
            {
                sistema.ExportarReporte(); // Exporta el reporte
            }
            
            Console.WriteLine("\nPresione cualquier tecla para salir..."); // Mensaje de salida
            Console.ReadKey(); // Espera a que el usuario presione una tecla
        }
    }
}