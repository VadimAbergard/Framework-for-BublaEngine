using SFML.Audio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Bubla
{
    public class Sound
    {
        private static Dictionary<string, Sound> sounds = new Dictionary<string, Sound>();

        private string path;
        private float defaultVolume;
        private float timeAddition;
        private float timeAttenuation;
        private float volumeAddition;
        private bool isLoop;
        private float volume;

        private SFML.Audio.Music sound;

        public Sound() {}

        public Sound(string path, bool isLoop, float volume) {
            if (path.Equals("")) return;
            this.path = path;
            this.isLoop = isLoop;
            this.volume = volume;
            this.defaultVolume = volume;
            try
            {
                sound = new SFML.Audio.Music(path);
                sound.Loop = isLoop;
                sound.Volume = volume;
                sound.Play();
                sound.Pause();
            }
            catch { }
        }

        public void LoadNewFile(string name, string fileName, bool fullPath)
        {
            if (fileName == null || !fileName.EndsWith(".ogg"))
            {
                Console.WriteLine("------------ load new file sound, don't load! path = null or type file not .ogg ------------");
                return;
            }
            if (sounds[name].sound != null) {
                sounds[name].sound.Stop();
                sounds[name].sound.Dispose();
            }
            sounds[name].sound = new SFML.Audio.Music(fullPath ? fileName : $"assets\\{fileName}.bubla");
            sounds[name].sound.Volume = sounds[name].VolumeDefault;
            sounds[name].sound.Loop = sounds[name].IsLoopDefault;
        }

        public void Play(string name)
        {
            try
            {
                PlaySound(sounds[name]);
            } catch(KeyNotFoundException e)
            {
                Console.WriteLine("Упс! \"звука\": \"" + name +"\" не было обноружино!");
            }
        }

        public void Play(string name, float volume, float timeAddition = 0)
        {
            try
            {
                PlaySound(sounds[name], volume, timeAddition);
            }
            catch (KeyNotFoundException e)
            {
                Console.WriteLine("Упс! \"звука\": \"" + name + "\" не было обноружино!");
            }
        }

        public void Pause(string name)
        {
            try
            {
                sounds[name].sound.Pause();
            } catch(KeyNotFoundException e)
            {
                Console.WriteLine("Упс! \"звука\": \"" + name +"\" не было обноружино!");
            }
        }

        public void SetVolume(string name, float volume)
        {
            try
            {
                sounds[name].sound.Volume = volume;
            } catch(KeyNotFoundException e)
            {
                Console.WriteLine("Упс! \"звука\": \"" + name +"\" не было обноружино!");
            }
        }

        public void SetPitch(string name, float pitch)
        {
            try
            {
                sounds[name].sound.Pitch = pitch;
            } catch(KeyNotFoundException e)
            {
                Console.WriteLine("Упс! \"звука\": \"" + name +"\" не было обноружино!");
            }
        }

        public void Stop(string name, float attenuation = 0)
        {
            try
            {
                if(attenuation != 0)
                {
                    sounds[name].timeAttenuation = attenuation;
                } else
                {
                    sounds[name].sound.Stop();
                }
            } catch(KeyNotFoundException e)
            {
                Console.WriteLine("Упс! \"звука\": \"" + name +"\" не было обноружино!");
            }
        }

        public void AllStop()
        {
            foreach (KeyValuePair<string, Sound> listSounds in sounds)
            {
                Sound sound = listSounds.Value;
                sound.sound.Stop();
            }
        }

        public bool IsPlaying(string name)
        {
            try
            {
                return sounds[name].sound.Status.Equals(SoundStatus.Playing);
            } catch(KeyNotFoundException e)
            {
                Console.WriteLine("Упс! \"звука\": \"" + name +"\" не было обноружино!");
                return false;
            }
        }

        public bool IsPlaying()
        {
            return sound == null ? false : sound.Status.Equals(SoundStatus.Playing);
        }



        public SFML.Audio.Music GetSound
        {
            get { return sound; }
        }

        public bool IsLoop
        {
            get { return sound.Loop; }
            set { sound.Loop = value; }
        }

        public bool IsLoopDefault
        {
            get { return isLoop; }
        }

        public float VolumeDefault
        {
            get { return volume; }
        }

        public float Volume
        {
            get { return sound.Volume; }
            set { sound.Volume = value; }
        }

        public string Path
        {
            get { return path; }
            set { path = value; }
        }

        public float TimeAddition
        {
            get { return timeAddition; }
            set { timeAddition = value; }
        }

        public float VolumeAddition
        {
            get { return volumeAddition; }
            set { volumeAddition = value; }
        }

        public float TimeAttenuation
        {
            get { return timeAttenuation; }
            set { timeAttenuation = value; }
        }

        public static Dictionary<string, Sound> GetSounds()
        {
            return sounds;
        }

        private static void PlaySound(Sound sound)
        {
            sound.sound.Volume = sound.defaultVolume;
            sound.sound.Play();
        }

        public static void Clear()
        {
            foreach (KeyValuePair<string, Sound> sounds in Sound.GetSounds())
            {
                Sound sound = sounds.Value;
                sound.sound.Stop();
                sound.sound.Dispose();
            }
            sounds.Clear();
        }

        private static void PlaySound(Sound sound, float volume, float timeAddition = 0)
        {
            if (timeAddition < 0) timeAddition = 0;
            if (timeAddition != 0) {
                Console.WriteLine("asdasdassda");
                sound.sound.Volume = 0;
                sound.timeAddition = timeAddition;
                sound.volumeAddition = volume;
            } else
            {
                sound.sound.Volume = volume;
            }
            sound.defaultVolume = volume;
            
            sound.sound.Play();
        }
    }
}
