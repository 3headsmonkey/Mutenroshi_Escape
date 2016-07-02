/* Mutenroshi Escape
 * David Sirvent Candela
 * Clase Principal:
 * - Aquí se crea el entorno y se lanza el juego.
 */

namespace Mutenroshi_Escape {
 class Mutenroshi_Escape {
  static void Main(string[] args) {
   Juego partida = new Juego();
   
   partida.Bienvenida();
   partida.IniciaMusica();
   partida.DibujarElementos();
   partida.Ayuda();

   // Bucle del juego
   do {
    if (partida.MostrarMenu) {
     switch (partida.Menu()) {
      case 1:
       partida = new Juego();
       break;
      case 2:
       partida.Salvar();
       break;
      case 3:
       partida.Cargar();
       break;
      case 4:
       partida.FinPartida = true;
       break;
      default:
       break;
     }
    }
    partida.ComprobarTeclas();
    partida.ComprobarColisiones();
    partida.DibujarElementos();
    partida.TiempoFotograma();
    partida.GameOver();
   } while (!partida.FinPartida);
   partida.Fundido();
  }
 }
}
