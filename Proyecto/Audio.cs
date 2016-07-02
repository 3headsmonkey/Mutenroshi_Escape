/* Mutenroshi Escape
 * David Sirvent Candela
 * Clase Audio:
 * - Implementaciones de Tao en métodos amigables
 *   para gestión de sonidos.
 */

using System;
using System.Collections.Generic;
using Tao.Sdl;

namespace Mutenroshi_Escape {
 class Audio {
  // Propiedades
  List<IntPtr> audios;
  int canales;

  // Constructor
  // canales -> Cantidad de audios que queremos poder reproducir a la vez
  // frecuencia, bytesPorMuestra -> Características del audio
  // frecuencia: 22050 o 44100
  // bytePorMuestra: 4096
  public Audio(int frecuencia, int canales, int bytesPorMuestra) {
   this.canales = canales;
   SdlMixer.Mix_OpenAudio(frecuencia, (short) SdlMixer.MIX_DEFAULT_FORMAT, canales,
   bytesPorMuestra);
   audios = new List<IntPtr>();
  }

  // Métodos
  // Añadir fichero WAV monocanal
  public bool AnyadeAudioWAV(string nombreFichero) {
   IntPtr archivo = SdlMixer.Mix_LoadWAV(nombreFichero);
   if (archivo == IntPtr.Zero) return false;
   audios.Add(archivo);
   return true;
  }

  // Reproducir fichero WAV monocanal
  // pos -> Posición dentro de la lista de ficheros de audio.
  // canal -> Canal de salida del fichero.
  // repeticiones -> -1 bucle, 0 para 1 repeticion, 1 para 2, etc...
  public void PlayWAV(int pos, int canal, int repeticiones) {
   if (pos >= 0 && pos < audios.Count && canal >= 1 && canal <= canales)
    SdlMixer.Mix_PlayChannel(canal, audios[pos], repeticiones);
  }

  // Añadir ficheros multicanal
  public bool AnyadeMusica(string nombreFichero) {
   IntPtr archivo = SdlMixer.Mix_LoadMUS(nombreFichero);
   if (archivo == IntPtr.Zero)
    return false;
   audios.Add(archivo);
   return true;
  }

  // Reproducir ficheros multicanal
  public void PlayMusica(int pos, int repeticiones) {
   if (pos >= 0 && pos < audios.Count)
    SdlMixer.Mix_PlayMusic(audios[pos], repeticiones);
  }
 }
}