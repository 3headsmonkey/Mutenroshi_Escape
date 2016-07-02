/* Mutenroshi Escape
 * David Sirvent Candela
 * Clase Imagen:
 * - Provee de implementaciones amigables sobre Tao para gestionar
 *   las imágenes.
 */

using System;
using Tao.Sdl;

namespace Mutenroshi_Escape {
 class Imagen {
  // Referencia a la imagen con que trabajar
  IntPtr imagen;
  // Coordenadas X e Y de la esquina superior izquierda donde poner la imagen
  protected short x, y;
  // Anchura y altura de la imagen en la pantalla
  protected short ancho, alto;

  // Getters
  public IntPtr GetPuntero() { return imagen; }
  public short GetX() { return x; }
  public short GetY() { return y; }
  public short GetAncho() { return ancho; }
  public short GetAlto() { return alto; }

  
  // Constructor
  public Imagen(string nombreFichero, short ancho, short alto) {
   imagen = SdlImage.IMG_Load(nombreFichero);
   if (imagen == IntPtr.Zero) {
    System.Console.WriteLine("Imagen inexistente!");
    Environment.Exit(1);
   }
   this.ancho = ancho;
   this.alto = alto;
  }

  // Métodos
  // Mover
  public void MoverA(short x, short y) {
   this.x = x;
   this.y = y;
  }

  // Detectar colisión con otra imagen
  public bool ColisionaCon(Imagen img) {
   return (x + ancho >= img.x && x <= img.x + img.ancho && y + alto >= img.y && y <= img.y + img.alto);
  }
 }
}
