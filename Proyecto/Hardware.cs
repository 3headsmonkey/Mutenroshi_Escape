/* Mutenroshi Escape
 * David Sirvent Candela
 * Clase Hardware:
 * - Provee de implementaciones amigables para gestionar
 *   el entorno que es necesario crear mediante Tao para
 *   empezar a trabajar.
 */

using System;
using System.Threading;
using Tao.Sdl;

namespace Mutenroshi_Escape {
 class Hardware {

  /* Propiedades */
  // Características de vídeo: anchura, altura y profundidad de color
  short anchoPantalla;
  short altoPantalla;
  short bitsColor;
  IntPtr pantalla;
  string titulo;
  string icono;

  // Constantes para manejo de teclado
  public static int TECLA_ESC = Sdl.SDLK_ESCAPE;
  public static int TECLA_SPA = Sdl.SDLK_SPACE;
  public static int TECLA_RET = Sdl.SDLK_RETURN;
  public static int TECLA_INT = Sdl.SDLK_KP_ENTER;
  public static int TECLA_ARR = Sdl.SDLK_UP;
  public static int TECLA_ABA = Sdl.SDLK_DOWN;
  public static int TECLA_IZQ = Sdl.SDLK_LEFT;
  public static int TECLA_DER = Sdl.SDLK_RIGHT;
  public static int TECLA_A = Sdl.SDLK_a;
  public static int TECLA_B = Sdl.SDLK_b;
  public static int TECLA_C = Sdl.SDLK_c;
  public static int TECLA_D = Sdl.SDLK_d;


  /* Construcctores y destructor */
  // Constructor
  public Hardware(short ancho, short alto, short bits, bool pantallaCompleta) {
   anchoPantalla = ancho;
   altoPantalla = alto;
   bitsColor = bits;
   titulo = "Mutenroshi Escape";
   icono = "icono.ico";

   // Flags para el modo de pantalla
   int flags = Sdl.SDL_HWSURFACE | Sdl.SDL_DOUBLEBUF | Sdl.SDL_ANYFORMAT;
   if (pantallaCompleta)
    flags = flags | Sdl.SDL_FULLSCREEN;

   // Inicializar SDL
   Sdl.SDL_Init(Sdl.SDL_INIT_EVERYTHING);
   pantalla = Sdl.SDL_SetVideoMode(anchoPantalla, altoPantalla, bitsColor, flags);

   // Características del modo gráfico (ancho, alto, profundidad y otros flags)
   Sdl.SDL_Rect rect = new Sdl.SDL_Rect(0, 0, anchoPantalla, altoPantalla);

   // Rectángulo para recortar lo que quede fuera de la pantalla
   Sdl.SDL_SetClipRect(pantalla, ref rect);

   // Inicializar tipografías
   SdlTtf.TTF_Init();

   // Establezco título de la ventana e icono de la aplicación
   Sdl.SDL_WM_SetCaption(titulo, icono);
  }

  // Destructor
  ~Hardware() {
   Sdl.SDL_Quit();
  }


  /* Métodos */
  // Dibuja la imagen en la pantalla, pero aun no se muestra (ver método siguiente)
  public void DibujarImagen(Imagen imagen) {
   Sdl.SDL_Rect origen = new Sdl.SDL_Rect(0, 0, imagen.GetAncho(), imagen.GetAlto());
   Sdl.SDL_Rect dest = new Sdl.SDL_Rect(imagen.GetX(), imagen.GetY(),
   imagen.GetAncho(), imagen.GetAlto());
   Sdl.SDL_BlitSurface(imagen.GetPuntero(), ref origen, pantalla, ref dest);
  }

  // Variante del método anterior útil para extraer imágenes de SpriteSheets
  // x e y no inican posición en ventana, sinó posición dentro de la imagen siendo
  // (0, 0) la esquina superior izquierda.
  public void DibujarImagen(Imagen imagen, short x, short y, short ancho, short alto) {
   Sdl.SDL_Rect origen = new Sdl.SDL_Rect(x, y, ancho, alto);
   Sdl.SDL_Rect dest = new Sdl.SDL_Rect(imagen.GetX(), imagen.GetY(), ancho, alto);
   Sdl.SDL_BlitSurface(imagen.GetPuntero(), ref origen, pantalla, ref dest);
  }

  // Provoca que se vuelque en la pantalla todo lo dibujado
  public void VisualizarPantalla() {
   Sdl.SDL_Flip(pantalla);
  }

  // Borramos la pantalla dibujando un rectangulo negro en su totalidad
  public void BorrarPantalla() {
   Sdl.SDL_Rect origen = new Sdl.SDL_Rect(0, 0, anchoPantalla, altoPantalla);
   Sdl.SDL_FillRect(pantalla, ref origen, 0);
  }

  // Recoger pulsacion del teclado
  public bool TeclaPulsada(int c) {
   bool pulsada = false;
   Sdl.SDL_PumpEvents();
   Sdl.SDL_Event suceso;
   Sdl.SDL_PollEvent(out suceso);
   int numkeys;
   byte[] teclas = Sdl.SDL_GetKeyState(out numkeys);
   if (teclas[c] == 1)
    pulsada = true;
   return pulsada;
  }

  // Vaciar eventos (incluidos las pulsaciones del teclado)
  public void VaciarEventos() {
   Sdl.SDL_Event evento;
   do {
    Sdl.SDL_PumpEvents();
   } while (Sdl.SDL_PollEvent(out evento) != 1);
  }

  // Pausar ejecución del programa
  public void Pausa(int milisegundos) {
   Thread.Sleep(milisegundos);
  }

  // Escribir texto en pantalla
  // texto -> texto a escribir
  // x, y -> Posición en pantalla
  // r,g,b -> Color RGB
  // fuente -> objeto creado previamente con la tipografía.
  public void EscribirTexto(string texto, short x, short y, byte r, byte g, byte b, Fuente fuente) {
   Sdl.SDL_Color color = new Sdl.SDL_Color(r, g, b);
   IntPtr textoComoImagen = SdlTtf.TTF_RenderText_Solid(fuente.GetPuntero(), texto, color);
   if (textoComoImagen == IntPtr.Zero)
    Environment.Exit(5);
   Sdl.SDL_Rect origen = new Sdl.SDL_Rect(0, 0, anchoPantalla, altoPantalla);
   Sdl.SDL_Rect dest = new Sdl.SDL_Rect(x, y, anchoPantalla, altoPantalla);
   Sdl.SDL_BlitSurface(textoComoImagen, ref origen, pantalla, ref dest);
  }

  // Versión del anterior que siempre dibuja en blanco y provoca salto de página según
  // la longitud indicada.
  public void EscribirTexto(string texto, short x, short y, Fuente fuente) {
   string texto1, texto2;
   int corte = 50;
   if (texto.Length > corte) {
    while (texto[corte] != ' ') corte--;
    texto1 = texto.Substring(0, corte);
    EscribirTexto(texto1, x, y, 255, 255, 255, fuente);
    texto2 = texto.Substring(corte, texto.Length - corte);
    EscribirTexto(texto2, x, (short) (y + 25), 255, 255, 255, fuente);
   }
   else {
    EscribirTexto(texto, x, y, 255, 255, 255, fuente);
   }
  }
 }
}
