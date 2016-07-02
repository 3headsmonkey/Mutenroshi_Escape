/* Mutenroshi Escape
 * David Sirvent Candela
 * Clase Juego:
 * - Desarrollo de la partida. Esta clase es lanzada por la clase principal
 *   y gestiona gran parte de los aspectos del juego.
 * 
 * Métodos destacables:
 * - Bienvenida() : Intro al juego.
 * - Ayuda() : Instrucciones.
 * - Salvar() : Almacena los datos de la partida.
 * - Cargar() : Recupera los datos de la partida.
 * - Menu() : Opciones.
 * - En general los implicados en gestionar los desplazamientos de los sprites.
 */

using System;
using System.IO;

namespace Mutenroshi_Escape {

 public class Juego {

  /* Atributos */
  bool pantallaCompleta,
       finPartida,
       teclaPulsada,
       mostrarMenu;
  Hardware entorno;
  Imagen fondo;
  Imagen[] techos;
  Imagen menu;
  Personaje jugador;
  Audio sonidos;
  short xmapa,
        ymapa,
        entornoAncho,
        entornoAlto,
        paso,
        xmenu,
        ymenu;
  Bordes borde;
  NPC npcs;


  /* Propiedades */
  public bool FinPartida {
   get { return finPartida; }
   set { finPartida = value; }
  }

  public bool MostrarMenu {
   get { return mostrarMenu; }
   set { /* Vacio */}
  }


  /* Constructores */
  // En el mismo constructor inicializo los componentes
  public Juego() {

   finPartida = false;
   mostrarMenu = false;
   teclaPulsada = false;
   borde = new Bordes();

   // Sonidos del juegos
   sonidos = new Audio(44100, 2, 4096);
   sonidos.AnyadeAudioWAV("hey.wav");
   sonidos.AnyadeMusica("bienvenida.mid");
   sonidos.AnyadeMusica("partida.mid");

   // Dimensiones del entorno
   entornoAncho = 800;
   entornoAlto = 600;
   pantallaCompleta = false;
   entorno = new Hardware(entornoAncho, entornoAlto, 24, pantallaCompleta);

   // Distancia recorrida por el jugador en cada pulsación
   paso = 10;

   // Fondo al inicio
   xmapa = 0;
   ymapa = 0;
   fondo = new Imagen("mapa.png", entornoAncho, entornoAlto);
   fondo.MoverA(0, 0);

   // Imágenes para la sobreimpresión de los techos.
   techos = new Imagen[2];
   techos[0] = new Imagen("techo1.png", 100, 65);
   techos[0].MoverA(1133, 956);
   techos[1] = new Imagen("techo2.png", 67, 101);
   techos[1].MoverA(707, 849);

   // Menu de opciones
   // Inicialmente lo situo fuera de la pantalla para la animación de entrada.
   menu = new Imagen("menu.png", 300, 400);
   xmenu = 0;
   ymenu = 0;
   menu.MoverA((short) (0 - menu.GetAncho()), (short) ((entornoAlto - menu.GetAlto()) / 2));

   // Jugador al inicio
   jugador = new Personaje("personaje.png", 45, 81);
   jugador.Inventario.Add(Personaje.Objetos.Vacio);
   jugador.SetPosX(280);
   jugador.SetPosY(150);
   jugador.Mover();

   // NPCs
   npcs = new NPC();
   npcs.Lista[4] = new NPC("Gohan", "npcs.png", 510, 300, 186, 0, (234 - 186), 96, Personaje.Objetos.Sarten,
                           "De que color es el caballo blanco de Santiago?",
                           "A > Blanco",
                           "B > Negro",
                           "C > Caballo que de quien?",
                           "D > Mira! Un mono con tres cabezas!",
                           Hardware.TECLA_A,
                           Personaje.Objetos.Vacio);
   npcs.Lista[2] = new NPC("Jajiro Bai", "npcs.png", 1865, 695, 93, 0, (141 - 93), 93, Personaje.Objetos.Pijama,
                           "De donde obtiene su nombre el Kame Hame Ha?",
                           "A > Realmente se llama 'Onda Vital' (norrr)",
                           "B > El Follet Tortuga es un cachondo.",
                           "C > Un Rey Hawaiano",
                           "D > Chim-pun! Chim-pun! (mono con platillos en tu cabeza)",
                           Hardware.TECLA_C,
                           Personaje.Objetos.Sarten);
   npcs.Lista[0] = new NPC("Krilin", "npcs.png", 1180, 1540, 0, 0, 48, 87, Personaje.Objetos.Jarra,
                          "Cuanto tardo en reventar Namek?",
                           "A > 5 minutos en la serie = 2 semanas de mi vida",
                           "B > Yo no entiendo de futbol.",
                           "C > Me abuuurrooo...",
                           "D > Deja que revise en el codigo la respuesta OK y te digo algo ",
                           Hardware.TECLA_A,
                           Personaje.Objetos.Pijama);
   npcs.Lista[1] = new NPC("Ulong", "npcs.png", 1465, 1215, 48, 0, (93 - 48), 69, Personaje.Objetos.Mando,
                           "Que objeto le pedi al Dragon Sagrado para evitar que Pilaf conquistara el mundo?",
                           "A > El caballo blanco de Santiago",
                           "B > Unas bragas",
                           "C > El Bosson de Higs, Jigs... che!",
                           "D > Un compilador que tras todos los errores diga: Animo que no es nada!",
                           Hardware.TECLA_B,
                           Personaje.Objetos.Jarra);
   npcs.Lista[3] = new NPC("Vegeta", "npcs.png", 870, 1280, 141, 0, (186 - 141), 96, Personaje.Objetos.Llave,
                           "Sabes de donde proviene mi nombre y el todos los Saiyans?",
                           "A > De la R.A.E.",
                           "B > Son sonidos guturales asincronos",
                           "C > Azul...",
                           "D > Son nombres de verduras",
                           Hardware.TECLA_D,
                           Personaje.Objetos.Mando);
   npcs.Lista[5] = new NPC("Puerta", "vacia.png", 990, 1682, 0, 0, 99, 10, Personaje.Objetos.Salida,
                           "Antes de salir... Que nota tienes en mente?",
                           "A > Suspenso por Friki !! (Noor!)",
                           "B > Sobresaliente por Friki!! (Yeah!)",
                           "C > Aaaaayyy... Macarena!!",
                           "D > Tengo indigestion de pixels, luego lo veo",
                           Hardware.TECLA_D,
                           Personaje.Objetos.Llave);
  }


  /* Métodos */

  // Ajustes de posiciones
  /* Le paso el valor de "paso" que es positivo o negativo en función de la
     detección de colisiones para que los desplazamientos ocurran o no       */
  public void AjustarPosicion(short paso) {

   // Pulsación DERECHA
   if (entorno.TeclaPulsada(Hardware.TECLA_DER)) {
    jugador.SetOrigenY(81 * 3);
    teclaPulsada = true;
    // Este if mueve el mapa y sus componentes (objetos, paredes y techos)
    // en caso de que se produzca scroll
    if (xmapa < 1500 && jugador.GetPosX() == 370) {
     xmapa += paso;
     for (int c = 0 ; c < techos.Length ; c++) {
      techos[c].MoverA((short) (techos[c].GetX() - paso), techos[c].GetY());
     }
     for (int c = 0 ; c < npcs.Lista.Length ; c++) {
      npcs.Lista[c].DesplazarX((short) -paso);
     }
     for (int c = 0 ; c < borde.Objeto.Length ; c++) {
      borde.Objeto[c].DesplazarX((short) -paso);
     }
    }
    // Desplazo al jugador en caso de que el mapa ya no pueda hacer scroll
    else if (jugador.GetPosX() < 750)
     jugador.DesplazarX(paso);
   }
   // Pulsación IZQUIERDA
   else if (entorno.TeclaPulsada(Hardware.TECLA_IZQ)) {
    jugador.SetOrigenY(81 * 2);
    teclaPulsada = true;
    if (xmapa > 0 && jugador.GetPosX() == 370) {
     xmapa -= paso;
     for (int c = 0 ; c < techos.Length ; c++) {
      techos[c].MoverA((short) (techos[c].GetX() + paso), techos[c].GetY());
     }
     for (int c = 0 ; c < npcs.Lista.Length ; c++) {
      npcs.Lista[c].DesplazarX(paso);
     }
     for (int c = 0 ; c < borde.Objeto.Length ; c++) {
      borde.Objeto[c].DesplazarX(paso);
     }
    }
    else if (jugador.GetPosX() > 0)
     jugador.DesplazarX((short) -paso);
   }
   // Pulsación ABAJO
   if (entorno.TeclaPulsada(Hardware.TECLA_ABA)) {
    jugador.SetOrigenY(81 * 1);
    teclaPulsada = true;
    if (ymapa < 1300 && jugador.GetPosY() == 300) {
     ymapa += paso;
     for (int c = 0 ; c < techos.Length ; c++) {
      techos[c].MoverA(techos[c].GetX(), (short) (techos[c].GetY() - paso));
     }
     for (int c = 0 ; c < npcs.Lista.Length ; c++) {
      npcs.Lista[c].DesplazarY((short) -paso);
     }
     for (int c = 0 ; c < borde.Objeto.Length ; c++) {
      borde.Objeto[c].DesplazarY((short) -paso);
     }
    }
    else if (jugador.GetPosY() < 550)
     jugador.DesplazarY(paso);
   }
   // Pulsación ARRIBA
   else if (entorno.TeclaPulsada(Hardware.TECLA_ARR)) {
    jugador.SetOrigenY(81 * 4);
    teclaPulsada = true;
    if (ymapa > 0 && jugador.GetPosY() == 300) {
     ymapa -= paso;
     for (int c = 0 ; c < techos.Length ; c++) {
      techos[c].MoverA(techos[c].GetX(), (short) (techos[c].GetY() + paso));
     }
     for (int c = 0 ; c < npcs.Lista.Length ; c++) {
      npcs.Lista[c].DesplazarY(paso);
     }
     for (int c = 0 ; c < borde.Objeto.Length ; c++) {
      borde.Objeto[c].DesplazarY(paso);
     }
    }
    else if (jugador.GetPosY() > 0)
     jugador.DesplazarY((short) -paso);
   }
  }

  // Método para verificar la tecla pulsada
  public void ComprobarTeclas() {
   teclaPulsada = false;

   AjustarPosicion(paso);

   // Animación del sprite. Gracias como está formado el sprite sólo tengo
   // que cambiar el origen de X para que la animación sea la correcta
   // en cualquier dirección del movimiento.
   if (teclaPulsada == true) {
    if (jugador.GetOrigenX() == 0) jugador.SetOrigenX(45);
    else jugador.SetOrigenX(0);
   }

   // Al pulsar ESC en cualquier momento del juego se muestra el menú.
   if (entorno.TeclaPulsada(Hardware.TECLA_ESC)) {
    mostrarMenu = true;
   }
  }

  // Aquí verifico básicamente dos cosas:
  // 1.  Choque contra la pared.
  // 2.  Choque contra NPC. Lo que provoca que se lanze un acertijo.
  public void ComprobarColisiones() {
   int choque = npcs.CualquierColisionCon(jugador); // Almaceno contra que posición del array de NPCS ha chocado el jugador
   if (choque >= 0) {
    AjustarPosicion((short) -paso);
    sonidos.PlayWAV(0, 1, 0);

    // La condición verifica si al NPCs le queda algún objeo en el inventario.
    // Si le falta es que ya se ha resuelto el acertijo aparejado al NPC
    if (!npcs.Lista[choque].ComprobarInventarioPropio()) {
     npcs.Lista[choque].Acertijo.YaRealizada(entorno);
    }
    else {
     // Este if verifica si el jugador posee el objeto requerido para poder
     // acceder a la prueba
     if (!npcs.Lista[choque].Acertijo.ComprobarInventario(jugador)) {
      npcs.Lista[choque].Acertijo.FaltaObjeto(entorno);
     }
     else {
      // La condición de este IF lanza el acertijo y si devuelve TRUE (resuelto)
      // provoca que el npcs entregue el contenido de su inventario al jugador.
      if (npcs.Lista[choque].Acertijo.Conversar(entorno, npcs.Lista[choque].Inventario[0])) {
       npcs.Lista[choque].EntregarObjetoA(jugador);
      }
     }
    }
   }
   // Esta última condición controla que el jugador no pueda pasar a través de las paredes y objetos del mapa.
   else if (borde.CualquierColisionCon(jugador)) {
    AjustarPosicion((short) -paso);
   }
   else {
    MoverElementos();
   }
  }

  // Tras haber reasignado las posiciones en el método AjustarPosiciones() muevo todos los
  // elementos de la pantalla.
  public void MoverElementos() {
   jugador.Mover();
   for (int c = 0 ; c < npcs.Lista.Length ; c++) {
    npcs.Lista[c].Mover();
   }
   for (int c = 0 ; c < borde.Objeto.Length ; c++) {
    borde.Objeto[c].Mover();
   }
  }

  // Volcado en pantalla
  public void DibujarElementos() {
   entorno.BorrarPantalla();
   entorno.DibujarImagen(fondo, xmapa, ymapa, entornoAncho, entornoAlto);
   jugador.Dibujar(entorno);
   for (int c = 0 ; c < techos.Length ; c++) {
    entorno.DibujarImagen(techos[c]);
   }
   for (int c = 0 ; c < npcs.Lista.Length ; c++) {
    npcs.Lista[c].Dibujar(entorno);
   }
   for (int c = 0 ; c < borde.Objeto.Length ; c++) {
    borde.Objeto[c].Dibujar(entorno);
   }   
   // Aunque siempre se dibuja el menú este está fuera de los límites de la ventana hasta
   // que es invocado.
   entorno.DibujarImagen(menu, xmenu, ymenu, menu.GetAncho(), menu.GetAlto());
   entorno.VisualizarPantalla();
  }
  
  public void TiempoFotograma() {
   entorno.Pausa(40);
  }

  // Efecto de fundido a negro.
  // He intentado darle un efecto de aceleración de inicio a fin pero no me
  // ha quedado muy fino que digamos.
  public void Fundido() {
   Imagen cortinilla = new Imagen("fundido.png", 800, 600);
   cortinilla.MoverA(0, 0);
   int iteraciones = 30;
   double decremento = 0.50;
   int pausa = 200;
   int c = 1;
   do {
    entorno.DibujarImagen(cortinilla);
    entorno.VisualizarPantalla();
    c++;
    pausa = (int) (pausa - (pausa * decremento));
    entorno.Pausa(pausa);
   } while (c < iteraciones);
  }

  // Pantalla de bienvenida
  public void Bienvenida() {
   Imagen bienvenida = new Imagen("bienvenida.png", 800, 2400);
   bienvenida.MoverA(0, 0);
   short y = 0;
   int pausa = 100;
   sonidos.PlayMusica(1, 1000);

   // Animación de la intro recorriendo los frames del sprite.
   do {
    entorno.BorrarPantalla();
    entorno.DibujarImagen(bienvenida, 0, y, entornoAncho, entornoAlto);
    entorno.VisualizarPantalla();
    y += 600;
    if (y > 1800) {
     y = 0;
    }
    entorno.Pausa(pausa);
   } while (!entorno.TeclaPulsada(Hardware.TECLA_INT) && !entorno.TeclaPulsada(Hardware.TECLA_RET));
   entorno.VaciarEventos();
  }

  // Pantalla de ayuda
  public void Ayuda() {
   Imagen ayuda = new Imagen("instrucciones.png", 717, 515);
   ayuda.MoverA((short) ((entornoAncho - 717) / 2), (short) ((entornoAlto - 515) / 2));
   entorno.DibujarImagen(ayuda);
   entorno.VisualizarPantalla();
   while (!entorno.TeclaPulsada(Hardware.TECLA_ESC)) ;
   entorno.VaciarEventos();
  }

  // Comprobar que el jugador ha finalizado la última prueba viendo si tiene el objeto "salida"
  // en su inventario.
  public void GameOver() {
   if (jugador.Inventario.IndexOf(Personaje.Objetos.Salida) >= 0) {
    finPartida = true;
   }
  }

  // Lanzar música
  public void IniciaMusica() {
   sonidos.PlayMusica(2, 1000);
  }


  // Menu de opciones **********************************************************************************************/
  public int Menu() {
   ymenu = 0;
   int retorno = 1;
   short velocidad = 30;

   // Animación de entrada
   for (short c = (short) (0 - menu.GetAncho()) ; c < ((entornoAncho - menu.GetAncho()) / 2) ; c += velocidad) {
    menu.MoverA(c, menu.GetY());
    DibujarElementos();
   }

   // Bucle del menu
   do {
    entorno.VaciarEventos();
    if (entorno.TeclaPulsada(Hardware.TECLA_ARR)) {
     if (ymenu > 0) ymenu -= menu.GetAlto();
    }
    else if (entorno.TeclaPulsada(Hardware.TECLA_ABA)) {
     if (ymenu < 1200) ymenu += menu.GetAlto();
    }

    DibujarElementos();
   } while (!entorno.TeclaPulsada(Hardware.TECLA_ESC) && !entorno.TeclaPulsada(Hardware.TECLA_RET) && !entorno.TeclaPulsada(Hardware.TECLA_INT));

   mostrarMenu = false;

   // En caso de que se haya salido del menu pulsando INTRO o RETURN (no ESC)
   // Asigno el valor de retorno del menu según la porción del sprite
   // que estoy mostrando
   // - Valores devueltos:
   //   1 . Partida Nueva
   //   2 . Cargar Partida
   //   3 . Salvar Partida
   //   4 . Salir
   if (!entorno.TeclaPulsada(Hardware.TECLA_ESC)) {
    switch (ymenu) {
     case 0:
      retorno = 1;
      break;
     case 400:
      retorno = 2;
      break;
     case 800:
      retorno = 3;
      break;
     case 1200:
      retorno = 4;
      break;
     default:
      break;
    }
   }
   else {
    retorno = -1;
   }

   // Animación de salida
   for (short c = (short) ((entornoAncho - menu.GetAncho()) / 2) ; c <= entornoAncho + velocidad ; c += velocidad) {
    menu.MoverA(c, menu.GetY());
    DibujarElementos();
   }

   entorno.VaciarEventos();
   return retorno;
  }


  /** Salvar y cargar partida ***************************************************************************/

  // Salvar partida
  public void Salvar() {

   string lineaMapa;
   string lineaJugador;
   string lineaNPC;
   string lineaBorde;
   string lineaTecho;

   // Siempre sobreescribo la partida guardada anterior.
   StreamWriter ficEscritura = new StreamWriter("partida.dat", false);

   // Almacena posición del mapa
   lineaMapa = "mapa," + xmapa + "," + ymapa;
   ficEscritura.WriteLine(lineaMapa);

   // Almaceno datos del jugador
   lineaJugador = "jugador," + jugador.GetPosX() + "," + jugador.GetPosY();
   for (int c = 0 ; c < jugador.Inventario.Count ; c++) {
    lineaJugador += "," + jugador.Inventario[c];
   }
   ficEscritura.WriteLine(lineaJugador);

   // Almaceno datos de los npc
   for (int c = 0 ; c < npcs.Lista.Length ; c++) {
    lineaNPC = "npc," + c + "," + npcs.Lista[c].GetPosX() + "," + npcs.Lista[c].GetPosY();
    if (npcs.Lista[c].Inventario.Count != 0) {
     lineaNPC += "," + npcs.Lista[c].Inventario[0];
    }
    ficEscritura.WriteLine(lineaNPC);
   }

   // Almaceno paredes y objetos
   for (int c = 0 ;  c < borde.Objeto.Length ; c++) {
    lineaBorde = "b," + borde.Objeto[c].GetPosX() + "," + borde.Objeto[c].GetPosY();
    ficEscritura.WriteLine(lineaBorde);
   }

   // Almaceno datos de los techos sobreimpresos
   for (int c = 0 ; c < techos.Length ; c++) {
    lineaTecho = "t," + techos[c].GetX() + "," + techos[c].GetY();
    ficEscritura.WriteLine(lineaTecho);
   }

   ficEscritura.Close();
  }

  public void Cargar() {

   if (File.Exists("partida.dat")) {

    string linea;
    string[] argumentos;
    int contObjetos = 0;
    int contTechos = 0;
    StreamReader ficLectura = new StreamReader("partida.dat");

    linea = ficLectura.ReadLine();
    while (linea != null) {

     argumentos = linea.Split(',');

     // Según el primer valor de cada línea escogo que tipo de objeto estoy recuperando.
     // Realmente no haría falta ya que se recuperan secuencialmente en el mismo orden
     // que se guardan, pero me ha parecido más correcto hacerlo así.
     switch (argumentos[0]) {

      case "mapa":
       xmapa = Convert.ToInt16(argumentos[1]);
       ymapa = Convert.ToInt16(argumentos[2]);
       break;

      case "jugador":
       jugador.SetPosX(Convert.ToInt16(argumentos[1]));
       jugador.SetPosY(Convert.ToInt16(argumentos[2]));

       Personaje.Objetos item = new Personaje.Objetos();
       jugador.Inventario.Clear();
       for (int c = 3 ; c < argumentos.Length ; c++) {
        switch (argumentos[c]) {
         case "Sarten":
          item = Personaje.Objetos.Sarten;
          break;
         case "Jarra":
          item = Personaje.Objetos.Jarra;
          break;
         case "Mando":
          item = Personaje.Objetos.Mando;
          break;
         case "Pijama":
          item = Personaje.Objetos.Pijama;
          break;
         case "Llave":
          item = Personaje.Objetos.Llave;
          break;
         case "Salida":
          item = Personaje.Objetos.Vacio;
          break;
         case "Vacio":
         default:
          item = Personaje.Objetos.Vacio;
          break;
        }
        jugador.Inventario.Add(item);
       }
       break;

      case "npc":
       int cont = Convert.ToInt32(argumentos[1]);
                            
       npcs.Lista[cont].SetPosX((short) (Convert.ToInt16(argumentos[2])));
       npcs.Lista[cont].SetPosY((short) (Convert.ToInt16(argumentos[3])));
       npcs.Lista[cont].Mover();

       npcs.Lista[cont].Inventario.Clear();
       // Como pueden haber NPCs sin objetos en el inventario compruebo por la longitud
       // de los datos guardados que efectivamente se les va a asignar un item.
       if (argumentos.Length == 5) {
        switch (argumentos[4]) {
         case "Sarten":
          item = Personaje.Objetos.Sarten;
          break;
         case "Jarra":
          item = Personaje.Objetos.Jarra;
          break;
         case "Mando":
          item = Personaje.Objetos.Mando;
          break;
         case "Pijama":
          item = Personaje.Objetos.Pijama;
          break;
         case "Llave":
          item = Personaje.Objetos.Llave;
          break;
         case "Salida":
          item = Personaje.Objetos.Vacio;
          break;
         case "Vacio":
         default:
          item = Personaje.Objetos.Vacio;
          break;
        }
        npcs.Lista[cont].Inventario.Add(item);
       }
       break;

      case "b": // Bordes
       if (contObjetos < borde.Objeto.Length) {
        borde.Objeto[contObjetos].SetPosX(Convert.ToInt16(argumentos[1]));
        borde.Objeto[contObjetos].SetPosY(Convert.ToInt16(argumentos[2]));
        borde.Objeto[contObjetos].Mover();
        contObjetos++;
       }
       break;

      case "t": // Techos
       if (contTechos < techos.Length) {
        if (contTechos == 0) techos[contTechos].MoverA((short) (1133 - xmapa), (short) (956 - ymapa));
        if (contTechos == 1) techos[contTechos].MoverA((short) (707 - xmapa), (short) (849 - ymapa));
        contTechos++;
       }
       break;

      default:
       break;
     }
     // Siguiente linea
     linea = ficLectura.ReadLine();
    }
    ficLectura.Close();
   }
  }
 }
}