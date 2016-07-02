/* Mutenroshi Escape
 * David Sirvent Candela
 * Clase Fuentes:
 * - Gestión de tipografías mediante Tao.
 */

using System;
using Tao.Sdl;

namespace Mutenroshi_Escape {
 class Fuente {

  // Propiedades
  IntPtr tipoDeLetra;

  // Getters
  public IntPtr GetPuntero() {
   return tipoDeLetra;
  }

  // Constructores
  // nombreFichero -> archivo TTF con la tipografía
  // tamanyo -> tamaño de presentación de la tipografía
  public Fuente(string nombreFichero, int tamanyo) {
   tipoDeLetra = SdlTtf.TTF_OpenFont(nombreFichero, tamanyo);
   if (tipoDeLetra == IntPtr.Zero) {
    System.Console.WriteLine("Tipo de letra inexistente!");
    Environment.Exit(2);
   }
  }
 }
}
