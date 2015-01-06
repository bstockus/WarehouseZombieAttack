using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SimpleGameFramework.Content {

	[Serializable]
	public struct Sound {

		public String Name;

		public String Path;

	}

	public static class Sounds {

		private static Dictionary<String, SoundEffect> sounds;

		public static void LoadSounds(ContentManager contentManager, List<Sound> _sounds) {
			sounds = new Dictionary<String, SoundEffect>();
			foreach (Sound sound in _sounds) {
				SoundEffect soundEffect = contentManager.Load<SoundEffect>(sound.Path);
				sounds.Add(sound.Name, soundEffect);
			}
		}

		public static void LoadSounds(ContentManager contentManager, String path) {
			Stream fileStream = TitleContainer.OpenStream(Path.Combine(contentManager.RootDirectory, path));
			XmlSerializer fileSerializer = new XmlSerializer(typeof(List<Sound>));
			List<Sound> _sounds = (List<Sound>)fileSerializer.Deserialize(fileStream);
			Sounds.LoadSounds(contentManager, _sounds);
		}

		public static SoundEffect GetSound(String name) {
			return sounds[name];
		}

	}

}

