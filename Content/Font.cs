using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SimpleGameFramework.Content {

	[Serializable]
	public struct Font {

		public String Name;

		public String Path;

	}

	public enum TextVerticalAlignment {
		Top,
		Middle,
		Bottom
	}

	public enum TextHorizantialAlignment {
		Left,
		Center,
		Right
	}

	public static class Fonts {

		#region Font Loading and Retrieval Methods

		private static Dictionary<String, SpriteFont> fonts;

		public static void LoadFonts(ContentManager contentManager, List<Font> _fonts) {
			fonts = new Dictionary<String, SpriteFont>();
			foreach (Font font in _fonts) {
				SpriteFont spriteFont = contentManager.Load<SpriteFont>(font.Path);
				fonts.Add(font.Name, spriteFont);
			}
		}

		public static void LoadFonts(ContentManager contentManager, String path) {
			Stream fileStream = TitleContainer.OpenStream(Path.Combine(contentManager.RootDirectory, path));
			XmlSerializer fileSerializer = new XmlSerializer(typeof(List<Font>));
			List<Font> _fonts = (List<Font>)fileSerializer.Deserialize(fileStream);
			Fonts.LoadFonts(contentManager, _fonts);
		}

		public static SpriteFont GetFont(String name) {
			return fonts[name];
		}

		#endregion

		#region Font Drawing Methods

		public static void DrawTextAligned(String fontName, String text, SpriteBatch spriteBatch, Vector2 location, TextVerticalAlignment verticalAlignment, TextHorizantialAlignment horizantialAlignment, Color color) {
			SpriteFont spriteFont = Fonts.GetFont(fontName);
			Vector2 textSize = spriteFont.MeasureString(text);

			Vector2 alignedLocation = new Vector2();

			// Calculate Horizantial Aligned Location
			switch (horizantialAlignment) {
				case TextHorizantialAlignment.Left:
					alignedLocation.X = location.X;
					break;
				case TextHorizantialAlignment.Center:
					alignedLocation.X = location.X - (textSize.X / 2.0f);
					break;
				case TextHorizantialAlignment.Right:
					alignedLocation.X = location.X - textSize.X;
					break;
			}

			// Calculate Vertical Aligned Location
			switch (verticalAlignment) {
				case TextVerticalAlignment.Top:
					alignedLocation.Y = location.Y;
					break;
				case TextVerticalAlignment.Middle:
					alignedLocation.Y = location.Y - (textSize.Y / 2.0f);
					break;
				case TextVerticalAlignment.Bottom:
					alignedLocation.Y = location.Y - textSize.Y;
					break;
			}

			spriteBatch.DrawString(spriteFont, text, alignedLocation, color);

		}

		public static void DrawTextAligned(String fontName, String text, SpriteBatch spriteBatch, Point location, TextVerticalAlignment verticalAlignment, TextHorizantialAlignment horizantialAlignment, Color color) {
			Fonts.DrawTextAligned(fontName, text, spriteBatch, new Vector2((float)location.X, (float)location.Y), verticalAlignment, horizantialAlignment, color);
		}

		public static void DrawTextTopLeftAligned(String fontName, String text, SpriteBatch spriteBatch, Vector2 location, Color color) {
			Fonts.DrawTextAligned(fontName, text, spriteBatch, location, TextVerticalAlignment.Top, TextHorizantialAlignment.Left, color);
		}

		public static void DrawTextTopCenterAligned(String fontName, String text, SpriteBatch spriteBatch, Vector2 location, Color color) {
			Fonts.DrawTextAligned(fontName, text, spriteBatch, location, TextVerticalAlignment.Top, TextHorizantialAlignment.Center, color);
		}

		public static void DrawTextTopRightAligned(String fontName, String text, SpriteBatch spriteBatch, Vector2 location, Color color) {
			Fonts.DrawTextAligned(fontName, text, spriteBatch, location, TextVerticalAlignment.Top, TextHorizantialAlignment.Right, color);
		}

		public static void DrawTextMiddleLeftAligned(String fontName, String text, SpriteBatch spriteBatch, Vector2 location, Color color) {
			Fonts.DrawTextAligned(fontName, text, spriteBatch, location, TextVerticalAlignment.Middle, TextHorizantialAlignment.Left, color);
		}

		public static void DrawTextMiddleCenterAligned(String fontName, String text, SpriteBatch spriteBatch, Vector2 location, Color color) {
			Fonts.DrawTextAligned(fontName, text, spriteBatch, location, TextVerticalAlignment.Middle, TextHorizantialAlignment.Center, color);
		}

		public static void DrawTextMiddleRightAligned(String fontName, String text, SpriteBatch spriteBatch, Vector2 location, Color color) {
			Fonts.DrawTextAligned(fontName, text, spriteBatch, location, TextVerticalAlignment.Middle, TextHorizantialAlignment.Right, color);
		}

		public static void DrawTextBottomLeftAligned(String fontName, String text, SpriteBatch spriteBatch, Vector2 location, Color color) {
			Fonts.DrawTextAligned(fontName, text, spriteBatch, location, TextVerticalAlignment.Bottom, TextHorizantialAlignment.Left, color);
		}

		public static void DrawTextBottomCenterAligned(String fontName, String text, SpriteBatch spriteBatch, Vector2 location, Color color) {
			Fonts.DrawTextAligned(fontName, text, spriteBatch, location, TextVerticalAlignment.Bottom, TextHorizantialAlignment.Center, color);
		}

		public static void DrawTextBottomRightAligned(String fontName, String text, SpriteBatch spriteBatch, Vector2 location, Color color) {
			Fonts.DrawTextAligned(fontName, text, spriteBatch, location, TextVerticalAlignment.Bottom, TextHorizantialAlignment.Right, color);
		}

		public static void DrawTextTopLeftAligned(String fontName, String text, SpriteBatch spriteBatch, Point location, Color color) {
			Fonts.DrawTextAligned(fontName, text, spriteBatch, location, TextVerticalAlignment.Top, TextHorizantialAlignment.Left, color);
		}

		public static void DrawTextTopCenterAligned(String fontName, String text, SpriteBatch spriteBatch, Point location, Color color) {
			Fonts.DrawTextAligned(fontName, text, spriteBatch, location, TextVerticalAlignment.Top, TextHorizantialAlignment.Center, color);
		}

		public static void DrawTextTopRightAligned(String fontName, String text, SpriteBatch spriteBatch, Point location, Color color) {
			Fonts.DrawTextAligned(fontName, text, spriteBatch, location, TextVerticalAlignment.Top, TextHorizantialAlignment.Right, color);
		}

		public static void DrawTextMiddleLeftAligned(String fontName, String text, SpriteBatch spriteBatch, Point location, Color color) {
			Fonts.DrawTextAligned(fontName, text, spriteBatch, location, TextVerticalAlignment.Middle, TextHorizantialAlignment.Left, color);
		}

		public static void DrawTextMiddleCenterAligned(String fontName, String text, SpriteBatch spriteBatch, Point location, Color color) {
			Fonts.DrawTextAligned(fontName, text, spriteBatch, location, TextVerticalAlignment.Middle, TextHorizantialAlignment.Center, color);
		}

		public static void DrawTextMiddleRightAligned(String fontName, String text, SpriteBatch spriteBatch, Point location, Color color) {
			Fonts.DrawTextAligned(fontName, text, spriteBatch, location, TextVerticalAlignment.Middle, TextHorizantialAlignment.Right, color);
		}

		public static void DrawTextBottomLeftAligned(String fontName, String text, SpriteBatch spriteBatch, Point location, Color color) {
			Fonts.DrawTextAligned(fontName, text, spriteBatch, location, TextVerticalAlignment.Bottom, TextHorizantialAlignment.Left, color);
		}

		public static void DrawTextBottomCenterAligned(String fontName, String text, SpriteBatch spriteBatch, Point location, Color color) {
			Fonts.DrawTextAligned(fontName, text, spriteBatch, location, TextVerticalAlignment.Bottom, TextHorizantialAlignment.Center, color);
		}

		public static void DrawTextBottomRightAligned(String fontName, String text, SpriteBatch spriteBatch, Point location, Color color) {
			Fonts.DrawTextAligned(fontName, text, spriteBatch, location, TextVerticalAlignment.Bottom, TextHorizantialAlignment.Right, color);
		}

		#endregion

		#region Text Helper Methods

		/// <summary>
		/// Adds newline characters to a string so that it fits within a certain size.
		/// </summary>
		/// <param name="text">The text to be modified.</param>
		/// <param name="maximumCharactersPerLine">
		/// The maximum length of a single line of text.
		/// </param>
		/// <param name="maximumLines">The maximum number of lines to draw.</param>
		/// <returns>The new string, with newline characters if needed.</returns>
		public static string BreakTextIntoLines(string text, int maximumCharactersPerLine, int maximumLines) {
			// NOTE: Not my code: Copied from Microsoft Code, from MonoGame Sample Solution, RolePlayingGame Project, Fonts.cs File
			if (maximumLines <= 0) {
				throw new ArgumentOutOfRangeException("maximumLines");
			}
			if (maximumCharactersPerLine <= 0) {
				throw new ArgumentOutOfRangeException("maximumCharactersPerLine");
			}

			// if the string is trivial, then this is really easy
			if (String.IsNullOrEmpty(text)) {
				return String.Empty;
			}

			// if the text is short enough to fit on one line, then this is still easy
			if (text.Length < maximumCharactersPerLine) {
				return text;
			}

			// construct a new string with carriage returns
			StringBuilder stringBuilder = new StringBuilder(text);
			int currentLine = 0;
			int newLineIndex = 0;
			while (((text.Length - newLineIndex) > maximumCharactersPerLine) && (currentLine < maximumLines)) {
				text.IndexOf(' ', 0);
				int nextIndex = newLineIndex;
				while ((nextIndex >= 0) && (nextIndex < maximumCharactersPerLine)) {
					newLineIndex = nextIndex;
					nextIndex = text.IndexOf(' ', newLineIndex + 1);
				}
				stringBuilder.Replace(' ', '\n', newLineIndex, 1);
				currentLine++;
			}

			return stringBuilder.ToString();
		}

		/// <summary>
		/// Adds new-line characters to a string to make it fit.
		/// </summary>
		/// <param name="text">The text to be drawn.</param>
		/// <param name="maximumCharactersPerLine">
		/// The maximum length of a single line of text.
		/// </param>
		public static string BreakTextIntoLines(string text, int maximumCharactersPerLine) {
			// NOTE: Not my code: Copied from Microsoft Code, from MonoGame Sample Solution, RolePlayingGame Project, Fonts.cs File
			// check the parameters
			if (maximumCharactersPerLine <= 0) {
				throw new ArgumentOutOfRangeException("maximumCharactersPerLine");
			}

			// if the string is trivial, then this is really easy
			if (String.IsNullOrEmpty(text)) {
				return String.Empty;
			}

			// if the text is short enough to fit on one line, then this is still easy
			if (text.Length < maximumCharactersPerLine) {
				return text;
			}

			// construct a new string with carriage returns
			StringBuilder stringBuilder = new StringBuilder(text);
			int currentLine = 0;
			int newLineIndex = 0;
			while (((text.Length - newLineIndex) > maximumCharactersPerLine)) {
				text.IndexOf(' ', 0);
				int nextIndex = newLineIndex;
				while ((nextIndex >= 0) && (nextIndex < maximumCharactersPerLine)) {
					newLineIndex = nextIndex;
					nextIndex = text.IndexOf(' ', newLineIndex + 1);
				}
				stringBuilder.Replace(' ', '\n', newLineIndex, 1);
				currentLine++;
			}

			return stringBuilder.ToString();
		}

		/// <summary>
		/// Break text up into separate lines to make it fit.
		/// </summary>
		/// <param name="text">The text to be broken up.</param>
		/// <param name="font">The font used ot measure the width of the text.</param>
		/// <param name="rowWidth">The maximum width of each line, in pixels.</param>
		public static List<string> BreakTextIntoList(String fontName, string text, int rowWidth) {
			// NOTE: Not my code: Copied from Microsoft Code, from MonoGame Sample Solution, RolePlayingGame Project, Fonts.cs File
			// check parameters
			SpriteFont font = Fonts.GetFont(fontName);
			if (rowWidth <= 0) {
				throw new ArgumentOutOfRangeException("rowWidth");
			}

			// create the list
			List<string> lines = new List<string>();

			// check for trivial text
			if (String.IsNullOrEmpty("text")) {
				lines.Add(String.Empty);
				return lines;
			}

			// check for text that fits on a single line
			if (font.MeasureString(text).X <= rowWidth) {
				lines.Add(text);
				return lines;
			}

			// break the text up into words
			string[] words = text.Split(' ');

			// add words until they go over the length
			int currentWord = 0;
			while (currentWord < words.Length) {
				int wordsThisLine = 0;
				string line = String.Empty;
				while (currentWord < words.Length) {
					string testLine = line;
					if (testLine.Length < 1) {
						testLine += words[currentWord];
					} else if ((testLine[testLine.Length - 1] == '.') ||
						(testLine[testLine.Length - 1] == '?') ||
						(testLine[testLine.Length - 1] == '!')) {
						testLine += "  " + words[currentWord];
					} else {
						testLine += " " + words[currentWord];
					}
					if ((wordsThisLine > 0) &&
						(font.MeasureString(testLine).X > rowWidth)) {
						break;
					}
					line = testLine;
					wordsThisLine++;
					currentWord++;
				}
				lines.Add(line);
			}
			return lines;
		}

		#endregion

	}

}

