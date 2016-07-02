/* Mutenroshi Escape
 * David Sirvent Candela
 * Clase Personaje:
 * - Aquí se establece la base para el propio jugador y los NPCS. También
 *   se aprovecha parte de sus características para las paredes y objetos.
 */

using System.Collections.Generic;

namespace Mutenroshi_Escape {
 class Personaje {

  /* Atributos */
  private struct PosicionImg {
   public short x;
   public short y;
  }

  private struct OrigenSpriteImg {
   public short x;
   public short y;
  }

  private struct TamanyoSprite {
   public short ancho;
   public short alto;
  }

  // Relación de objetos que pueden estar contenidos en el inventario de cada personaje
  public enum Objetos {Vacio, Sarten, Llave, Pijama, Jarra, Mando, Salida};

  List<Objetos> inventario;
  Imagen sprite;
  PosicionImg posicion;
  OrigenSpriteImg origenSprite;
  TamanyoSprite tamanyo;
  
  /* Propiedades */
  public List<Objetos> Inventario {
   get { return inventario; }
   set { /* vacio */ }
  }


  /* Construtores */
  public Personaje() {
   /* Vacio */
  }

  public Personaje(string imgSprite, short anchoSprite, short altoSprite) {
   inventario = new List<Objetos>();
   posicion = new PosicionImg();
   origenSprite = new OrigenSpriteImg();
   tamanyo = new TamanyoSprite();
   tamanyo.ancho = anchoSprite;
   tamanyo.alto = altoSprite;
   sprite = new Imagen(imgSprite, tamanyo.ancho, tamanyo.alto);
  }

  public Personaje(string imgSprite, short anchoSprite, short altoSprite, Objetos item) {
   inventario = new List<Objetos>();
   inventario.Add(item);
   posicion = new PosicionImg();
   origenSprite = new OrigenSpriteImg();
   tamanyo = new TamanyoSprite();
   tamanyo.ancho = anchoSprite;
   tamanyo.alto = altoSprite;
   sprite = new Imagen(imgSprite, tamanyo.ancho, tamanyo.alto);
  }


  /* Métodos */
  public void SetPosX(short newX) {
   posicion.x = newX;
  }

  public void SetPosY(short newY) {
   posicion.y = newY;
  }

  public void SetOrigenX(short newX) {
   origenSprite.x = newX;
  }

  public void SetOrigenY(short newY) {
   origenSprite.y = newY;
  }

  public short GetPosX() {
   return posicion.x;
  }

  public short GetPosY() {
   return posicion.y;
  }

  public short GetOrigenX() {
   return origenSprite.x;
  }

  public short GetOrigenY() {
   return origenSprite.y;
  }

  public void Mover() {
   this.sprite.MoverA(this.posicion.x, this.posicion.y);
  }

  public void DesplazarX(short difX) {
   this.posicion.x += difX;
  }

  public void DesplazarY(short difY) {
   this.posicion.y += difY;
  }

  public void Dibujar(Hardware entorno) {
   entorno.DibujarImagen(sprite, origenSprite.x, origenSprite.y, tamanyo.ancho, tamanyo.alto);
  }

  public bool ColisionaCon(Personaje otro) {
   return (posicion.x + tamanyo.ancho >= otro.posicion.x
           &&
           posicion.x <= otro.posicion.x + otro.tamanyo.ancho
           &&
           posicion.y + tamanyo.alto >= otro.posicion.y
           &&
           posicion.y <= otro.posicion.y + otro.tamanyo.alto);
  }
 }
}
