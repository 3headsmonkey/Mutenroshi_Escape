/* Mutenroshi Escape
 * David Sirvent Candela
 * Clase Pregunta:
 * - Esta clase almacena el acertijo que cada NPC le plantea al jugador
 */

namespace Mutenroshi_Escape {
 class Pregunta {
  /* Atributos */
  string pregunta;
  string respuestaA, respuestaB, respuestaC, respuestaD;
  int respuestaCorrecta;
  Imagen fondo;
  Imagen acierto;
  Imagen fallo;
  Imagen faltaPrevio;
  Imagen pruebaYaSuperada;
  Personaje.Objetos previo; // Este objeto es el requisito previo para plantear el acertijo
  Fuente typoPregunta;
  Fuente typoRespuesta;
  Fuente typoGrande;

  /* Constructores */
  public Pregunta() {
   typoGrande = new Fuente("Minecraft.ttf", 24);
   typoPregunta = new Fuente("Minecraft.ttf", 16);
   typoRespuesta = new Fuente("Minecraft.ttf", 14);
   fondo = new Imagen("conversacion.png", 610, 412);
   fondo.MoverA((800-610)/2, (600-412)/2);
   acierto = new Imagen("acierto.png", 374, 132);
   acierto.MoverA((800 - 374) / 2, (600 - 132) / 2);
   fallo = new Imagen("fallo.png", 610, 412);
   fallo.MoverA((800 - 374) / 2, (600 - 132) / 2);
   faltaPrevio = new Imagen("previo.png", 358, 348);
   faltaPrevio.MoverA((800 - 358) / 2, (600 - 348) / 2);
   pruebaYaSuperada = new Imagen("superada.png", 358, 348);
   pruebaYaSuperada.MoverA((800 - 358) / 2, (600 - 348) / 2);
  }

  public Pregunta(string newPregunta, string newRespuestaA, string newRespuestaB, string newRespuestaC, string newRespuestaD, int newRespuestaCorrecta, Personaje.Objetos newPrevio) : this() {
   pregunta = newPregunta;
   respuestaA = newRespuestaA;
   respuestaB = newRespuestaB;
   respuestaC = newRespuestaC;
   respuestaD = newRespuestaD;
   respuestaCorrecta = newRespuestaCorrecta;
   previo = newPrevio;
  }

  /* Métodos */
  // Verifico que el personaje pasado por parámetro contiene en su inventario el objeto necesario para
  // que el acertijo se lance.
  public bool ComprobarInventario(Personaje jugador) {
   bool retorno = false;
   if (jugador.Inventario.IndexOf(previo) < 0) retorno = false;
   else retorno = true;
   return retorno;
  }

  // Dibujo la ventana del acertijo y gestiono la respuesta
  public bool Conversar(Hardware entorno, Personaje.Objetos premio) {
   bool retorno = false;
   entorno.DibujarImagen(fondo);
   entorno.EscribirTexto(pregunta, 150, 150, typoPregunta);
   entorno.EscribirTexto(respuestaA, 275, 350, typoRespuesta);
   entorno.EscribirTexto(respuestaB, 275, 375, typoRespuesta);
   entorno.EscribirTexto(respuestaC, 275, 400, typoRespuesta);
   entorno.EscribirTexto(respuestaD, 275, 425, typoRespuesta);
   entorno.VisualizarPantalla();
   // Pausa para que el usuario introduzca su respuesta
   while (!entorno.TeclaPulsada(Hardware.TECLA_ESC) && !entorno.TeclaPulsada(Hardware.TECLA_A) && !entorno.TeclaPulsada(Hardware.TECLA_B) && !entorno.TeclaPulsada(Hardware.TECLA_C) && !entorno.TeclaPulsada(Hardware.TECLA_D));
   if (entorno.TeclaPulsada(respuestaCorrecta)) {
    entorno.DibujarImagen(acierto);
    entorno.EscribirTexto(premio.ToString(), 465, 317, typoGrande);
    retorno = true;
   }
   else {
    entorno.DibujarImagen(fallo);
    retorno = false;
   }
   entorno.VaciarEventos();
   entorno.VisualizarPantalla();
   while (!entorno.TeclaPulsada(Hardware.TECLA_ESC) && !entorno.TeclaPulsada(Hardware.TECLA_SPA));
   entorno.VaciarEventos();
   return retorno;
  }

  // Lanzar mensaje si el jugador no cumple pre-requisito para lanzar pregunta
  public void FaltaObjeto(Hardware entorno) {
   entorno.DibujarImagen(faltaPrevio);
   entorno.EscribirTexto(previo.ToString(), 255, 165, typoGrande);
   entorno.VisualizarPantalla();
   while (!entorno.TeclaPulsada(Hardware.TECLA_ESC) && !entorno.TeclaPulsada(Hardware.TECLA_SPA)) ;
   entorno.VaciarEventos();
  }

  // Lanzar mensaje si el jugador ya ha solventado la prueba
  public void YaRealizada(Hardware entorno) {
   entorno.DibujarImagen(pruebaYaSuperada);
   entorno.VisualizarPantalla();
   while (!entorno.TeclaPulsada(Hardware.TECLA_ESC) && !entorno.TeclaPulsada(Hardware.TECLA_SPA)) ;
   entorno.VaciarEventos();
  }
 }
}
