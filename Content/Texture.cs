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
	public struct Texture {

		public String Name;

		public String Path;

	}

	public static class Textures {

		private static Dictionary<String, Texture2D> textures;

		public static void LoadTextures(ContentManager contentManager, List<Texture> _textures) {
			textures = new Dictionary<String, Texture2D>();
			foreach (Texture texture in _textures) {
				Texture2D texture2D = contentManager.Load<Texture2D>(texture.Path);
				textures.Add(texture.Name, texture2D);
			}
		}

		public static void LoadTextures(ContentManager contentManager, String path) {
			Stream fileStream = TitleContainer.OpenStream(Path.Combine(contentManager.RootDirectory, path));
			XmlSerializer fileSerializer = new XmlSerializer(typeof(List<Texture>));
			List<Texture> _textures = (List<Texture>)fileSerializer.Deserialize(fileStream);
			Textures.LoadTextures(contentManager, _textures);
		}

		public static Texture2D GetTexture(String name) {
			return textures[name];
		}

	}

}

