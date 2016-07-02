/* Mutenroshi Escape
 * David Sirvent Candela
 * Clase Bordes:
 * - Esta clase aprovecha el sistema de control de colisiones de la clase Personaje para
 *   crear "lineas" transparente que no pueden traspasare y poder delimitar -por ejemplo-
 *   las paredes que vienen dibujadas en la imagen de fondo.
 */

using System;
using System.IO;

namespace Mutenroshi_Escape {
 class Bordes : Personaje {

  /* Atributos */
  Bordes[] objeto;


  /* Propiedades */
  public Bordes[] Objeto {
   get { return objeto; }
  }


  /* Constructor */
  public Bordes() {
   objeto = CargarObjetos();
  }

  public Bordes(short x, short y, short ancho, short alto) : base("vacia.png", ancho, alto) {
   SetPosX(x);
   SetPosY(y);
   Mover();
  }
    

  /* Métodos */

  /* Este método recupera las medidas y posiciones de los diferentes objetos del mapa desde un fichero
     de texto para después ser utilizados en el control de colisiones */
  public Bordes[] CargarObjetos() {
   short contador;
   Bordes[] objeto;
   StreamReader fichero;
   string linea,
          nombreFichero = "objetos.dat";
          /* objetos.dat : almacena en cada linea: posX, posY, ancho, alto de cada objeto */
   string[] valores;


   if (File.Exists(nombreFichero)) {
    contador = 0;

    // Contamos cuantos objetos hay en el mapa (sin contar los NPC)
    fichero = File.OpenText(nombreFichero);
    do {
     linea = fichero.ReadLine();
     if (linea != null) contador++;
    } while (linea != null);
    fichero.Close();

    objeto = new Bordes[contador];

    // Incluimos cada objeto en el array de objetos.
    fichero = File.OpenText(nombreFichero);
    for (int c = 0 ; c < objeto.Length ; c++) {
     linea = fichero.ReadLine();
     valores = linea.Split(',');
     objeto[c] = new Bordes(Convert.ToInt16(valores[0]),
                            Convert.ToInt16(valores[1]),
                            Convert.ToInt16(valores[2]),
                            Convert.ToInt16(valores[3]));
    }
    fichero.Close();
   }
   else {
    objeto = new Bordes[1];
    objeto[0] = new Bordes(0, 0, 800, 600);
   }

   return objeto;
  }

  /* Comprobamos si el personaje pasado colisiona con alguno de los objetos
     cargados en la función anterior */
  public bool CualquierColisionCon(Personaje jugador) {
   bool colision = false;

   for (int c = 0 ; c < objeto.Length ; c++) {
    colision = objeto[c].ColisionaCon(jugador);
    if (colision) break;
   }

   return colision;

  }
 }
}
