using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace SistemaVacunacionCovid
{
    // Enumeración para tipos de vacuna
    public enum TipoVacuna
    {
        Pfizer,
        AstraZeneca
    }

    // Clase que representa un ciudadano
    public class Ciudadano
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Cedula { get; set; }

        public Ciudadano(int id, string nombre, string cedula)
        {
            Id = id;
            Nombre = nombre;
            Cedula = cedula;
        }

        public override bool Equals(object obj)
        {
            return obj is Ciudadano ciudadano && Id == ciudadano.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return $"ID: {Id}, Nombre: {Nombre}, Cédula: {Cedula}";
        }
    }

    // Clase que representa una vacunación
    public class RegistroVacunacion
    {
        public Ciudadano Ciudadano { get; set; }
        public TipoVacuna TipoVacuna { get; set; }
        public DateTime FechaVacunacion { get; set; }
        public int NumeroDosis { get; set; } // 1 o 2

        public RegistroVacunacion(Ciudadano ciudadano, TipoVacuna tipoVacuna, DateTime fecha, int numeroDosis)
        {
            Ciudadano = ciudadano;
            TipoVacuna = tipoVacuna;
            FechaVacunacion = fecha;
            NumeroDosis = numeroDosis;
        }

        public override string ToString()
        {
            return $"{Ciudadano} - Vacuna: {TipoVacuna}, Dosis: {NumeroDosis}, Fecha: {FechaVacunacion:dd/MM/yyyy}";
        }
    }

    // Clase principal del sistema de vacunación
    public class SistemaVacunacion
    {
        // Conjuntos principales
        private HashSet<Ciudadano> _todosCiudadanos;
        private HashSet<Ciudadano> _vacunadosPfizer;
        private HashSet<Ciudadano> _vacunadosAstraZeneca;
        private List<RegistroVacunacion> _registrosVacunacion;

        public SistemaVacunacion()
        {
            _todosCiudadanos = new HashSet<Ciudadano>();
            _vacunadosPfizer = new HashSet<Ciudadano>();
            _vacunadosAstraZeneca = new HashSet<Ciudadano>();
            _registrosVacunacion = new List<RegistroVacunacion>();
        }

        // Método para generar datos ficticios
        public void GenerarDatosFicticios()
        {
            var random = new Random();
            
            // Generar 500 ciudadanos
            for (int i = 1; i <= 500; i++)
            {
                var ciudadano = new Ciudadano(i, $"Ciudadano {i}", $"17{i:D8}");
                _todosCiudadanos.Add(ciudadano);
            }

            // Seleccionar aleatoriamente 75 ciudadanos para Pfizer
            var ciudadanosParaPfizer = _todosCiudadanos.OrderBy(x => random.Next()).Take(75).ToList();
            foreach (var ciudadano in ciudadanosParaPfizer)
            {
                _vacunadosPfizer.Add(ciudadano);
                
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
            var ciudadanosDisponibles = _todosCiudadanos.Except(_vacunadosPfizer).ToList();
            var ciudadanosParaAstraZeneca = ciudadanosDisponibles.OrderBy(x => random.Next()).Take(75).ToList();
            
            foreach (var ciudadano in ciudadanosParaAstraZeneca)
            {
                _vacunadosAstraZeneca.Add(ciudadano);
                
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
            var vacunados = new HashSet<Ciudadano>(_vacunadosPfizer);
            vacunados.UnionWith(_vacunadosAstraZeneca);
            
            return new HashSet<Ciudadano>(_todosCiudadanos.Except(vacunados));
        }

        // 2. Ciudadanos que han recibido ambas dosis
        public HashSet<Ciudadano> ObtenerCiudadanosConAmbasDosis()
        {
            var conAmbasDosis = new HashSet<Ciudadano>();
            
            var ciudadanosConDosDosisPfizer = _registrosVacunacion
                .Where(r => r.TipoVacuna == TipoVacuna.Pfizer)
                .GroupBy(r => r.Ciudadano)
                .Where(g => g.Count() >= 2)
                .Select(g => g.Key);
            
            var ciudadanosConDosDosisAstraZeneca = _registrosVacunacion
                .Where(r => r.TipoVacuna == TipoVacuna.AstraZeneca)
                .GroupBy(r => r.Ciudadano)
                .Where(g => g.Count() >= 2)
                .Select(g => g.Key);
            
            conAmbasDosis.UnionWith(ciudadanosConDosDosisPfizer);
            conAmbasDosis.UnionWith(ciudadanosConDosDosisAstraZeneca);
            
            return conAmbasDosis;
        }

        // 3. Ciudadanos que SOLO han recibido Pfizer (intersección y diferencia)
        public HashSet<Ciudadano> ObtenerCiudadanosSoloPfizer()
        {
            return new HashSet<Ciudadano>(_vacunadosPfizer.Except(_vacunadosAstraZeneca));
        }

        // 4. Ciudadanos que SOLO han recibido AstraZeneca (intersección y diferencia)
        public HashSet<Ciudadano> ObtenerCiudadanosSoloAstraZeneca()
        {
            return new HashSet<Ciudadano>(_vacunadosAstraZeneca.Except(_vacunadosPfizer));
        }

        // Método para generar estadísticas
        public void GenerarEstadisticas()
        {
            Console.WriteLine("=== ESTADÍSTICAS DEL SISTEMA DE VACUNACIÓN COVID-19 ===\n");
            
            var noVacunados = ObtenerCiudadanosNoVacunados();
            var conAmbasDosis = ObtenerCiudadanosConAmbasDosis();
            var soloPfizer = ObtenerCiudadanosSoloPfizer();
            var soloAstraZeneca = ObtenerCiudadanosSoloAstraZeneca();
            
            Console.WriteLine($"Total de ciudadanos registrados: {_todosCiudadanos.Count}");
            Console.WriteLine($"Total vacunados con Pfizer: {_vacunadosPfizer.Count}");
            Console.WriteLine($"Total vacunados con AstraZeneca: {_vacunadosAstraZeneca.Count}");
            Console.WriteLine();
            
            Console.WriteLine("=== RESULTADOS DE OPERACIONES DE CONJUNTOS ===\n");
            
            Console.WriteLine($"1. Ciudadanos NO vacunados: {noVacunados.Count}");
            MostrarPrimeros10(noVacunados);
            
            Console.WriteLine($"\n2. Ciudadanos con AMBAS dosis: {conAmbasDosis.Count}");
            MostrarPrimeros10(conAmbasDosis);
            
            Console.WriteLine($"\n3. Ciudadanos SOLO con Pfizer: {soloPfizer.Count}");
            MostrarPrimeros10(soloPfizer);
            
            Console.WriteLine($"\n4. Ciudadanos SOLO con AstraZeneca: {soloAstraZeneca.Count}");
            MostrarPrimeros10(soloAstraZeneca);
        }

        private void MostrarPrimeros10(HashSet<Ciudadano> conjunto)
        {
            var primeros10 = conjunto.Take(10);
            foreach (var ciudadano in primeros10)
            {
                Console.WriteLine($"   - {ciudadano}");
            }
            if (conjunto.Count > 10)
            {
                Console.WriteLine($"   ... y {conjunto.Count - 10} más");
            }
        }

        // Método para exportar a archivo de texto
        public void ExportarReporte(string nombreArchivo = "reporte_vacunacion.txt")
        {
            using (var writer = new StreamWriter(nombreArchivo))
            {
                writer.WriteLine("REPORTE DE VACUNACIÓN COVID-19");
                writer.WriteLine($"Fecha de generación: {DateTime.Now}");
                writer.WriteLine("================================\n");
                
                var noVacunados = ObtenerCiudadanosNoVacunados();
                var conAmbasDosis = ObtenerCiudadanosConAmbasDosis();
                var soloPfizer = ObtenerCiudadanosSoloPfizer();
                var soloAstraZeneca = ObtenerCiudadanosSoloAstraZeneca();
                
                writer.WriteLine($"RESUMEN EJECUTIVO:");
                writer.WriteLine($"- Total ciudadanos: {_todosCiudadanos.Count}");
                writer.WriteLine($"- No vacunados: {noVacunados.Count} ({(noVacunados.Count * 100.0 / _todosCiudadanos.Count):F1}%)");
                writer.WriteLine($"- Con esquema completo: {conAmbasDosis.Count} ({(conAmbasDosis.Count * 100.0 / _todosCiudadanos.Count):F1}%)");
                writer.WriteLine($"- Solo Pfizer: {soloPfizer.Count}");
                writer.WriteLine($"- Solo AstraZeneca: {soloAstraZeneca.Count}\n");
                
                EscribirSeccionReporte(writer, "CIUDADANOS NO VACUNADOS", noVacunados);
                EscribirSeccionReporte(writer, "CIUDADANOS CON AMBAS DOSIS", conAmbasDosis);
                EscribirSeccionReporte(writer, "CIUDADANOS SOLO CON PFIZER", soloPfizer);
                EscribirSeccionReporte(writer, "CIUDADANOS SOLO CON ASTRAZENECA", soloAstraZeneca);
            }
            
            Console.WriteLine($"\nReporte exportado exitosamente a: {nombreArchivo}");
        }

        private void EscribirSeccionReporte(StreamWriter writer, string titulo, HashSet<Ciudadano> conjunto)
        {
            writer.WriteLine($"{titulo} ({conjunto.Count} registros):");
            writer.WriteLine(new string('-', titulo.Length + 20));
            
            foreach (var ciudadano in conjunto)
            {
                writer.WriteLine(ciudadano.ToString());
            }
            writer.WriteLine();
        }
    }

    // Clase principal para ejecutar el programa
    class Program
    {
        static void Main(string[] args)
        {
            var sistema = new SistemaVacunacion();
            
            Console.WriteLine("Generando datos ficticios...");
            sistema.GenerarDatosFicticios();
            
            Console.WriteLine("Procesando datos con operaciones de conjuntos...\n");
            sistema.GenerarEstadisticas();
            
            Console.WriteLine("\n¿Desea exportar el reporte a archivo? (s/n): ");
            var respuesta = Console.ReadLine();
            
            if (respuesta?.ToLower() == "s")
            {
                sistema.ExportarReporte();
            }
            
            Console.WriteLine("\nPresione cualquier tecla para salir...");
            Console.ReadKey();
        }
    }
}