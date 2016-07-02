/* Mutenroshi Escape
 * David Sirvent Candela
 * Clase NPC:
 * - Esta clase se utiliza para definir los NPC (Non Playable Characters) que 
 *   interactuan con el jugador.
 */

namespace Mutenroshi_Escape {
 class NPC : Personaje {

  /* Atributos */
  NPC[] lista = new NPC[6] ;
  string nombre;
  Pregunta acertijo;

  /* Propiedades */
  public NPC[] Lista {
   get { return lista; }
   set { /* vacio */}
  }

  public string Nombre {
   get { return nombre; }
   set { /* vacio */}
  }

  public Pregunta Acertijo {
   get { return acertijo; }
   set { /* vacio */}
  }

  /* Constructor */
  public NPC() {

  }

  public NPC(string newNombre, string imgSprite, short x, short y, short origX, short origY, short anchoSprite, short altoSprite, Objetos item) : base(imgSprite, anchoSprite, altoSprite, item) {
   nombre = newNombre;
   SetPosX(x);
   SetPosY(y);
   SetOrigenX(origX);
   SetOrigenY(origY);
   Mover();
  }

  public NPC(string newNombre,
             string imgSprite,
             short x,
             short y,
             short origX,
             short origY,
             short anchoSprite,
             short altoSprite,
             Objetos item,
             string newPregunta,
             string newRespuestaA,
             string newRespuestaB,
             string newRespuestaC,
             string newRespuestaD,
             int newRespuestaCorrecta,
             Personaje.Objetos newPrevio)
             : this(newNombre, imgSprite, x, y, origX, origY, anchoSprite, altoSprite, item) {
   acertijo = new Pregunta(newPregunta, newRespuestaA, newRespuestaB, newRespuestaC, newRespuestaD, newRespuestaCorrecta, newPrevio);
  }


  /* Métodos */
  // Comprueba si el personaje pasado por parámetros colisiona con algún NPC de la lista
  // y devuelve su posición en caso afirmativo.
  public int CualquierColisionCon(Personaje jugador) {
   bool colision = false;
   int retorno = -1;

   for (int c = 0 ; c < lista.Length ; c++) {
    colision = lista[c].ColisionaCon(jugador);
    if (colision) {
     retorno = c;
     break;
    }
   }

   return retorno;
  }

  // Verificamos si el NPC tiene el inventario vacio o no.
  public bool ComprobarInventarioPropio() {
   bool retorno = false;
   if (this.Inventario.Count == 0) retorno = false;
   else retorno = true;
   return retorno;
  }
  
  // El NPC "pasa" el primer objeto de su inventario (no deben haber mas)
  // al personaje pasado por parámetro.    
  public void EntregarObjetoA(Personaje jugador) {
   jugador.Inventario.Add(this.Inventario[0]);
   this.Inventario.RemoveAt(0);
  }
 }
}