using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SimpleGameFramework.Content;

namespace WarehouseZombieAttack {

    public enum HUDWeaponType {
        None,
        Gasoline,
        Pistol,
        Shotgun
    }

    public struct HUDWeaponInfo {

        public int IconCellIndex;
        public string Name;
        public HUDWeaponType Type;
        public float Attack;

        public HUDWeaponInfo(int iconCellIndex, string name, HUDWeaponType type, float attack) {
            this.IconCellIndex = iconCellIndex;
            this.Name = name;
            this.Type = type;
            this.Attack = attack;
        }

    }

    public class HUD : IDrawableEntity {

        #region Constants

		static readonly float SURVIVAL_TIME_TEXT_POSITION_OFFSET = 0.0f;
		static readonly float ZOMBIE_KILLS_TEXT_POSITION_OFFSET = 200.0f;
		static readonly float POINTS_TEXT_POSITION_OFFSET = 400.0f;
		static readonly float ZOMBIE_WAVES_TEXT_POSITION_OFFSET = 300.0f;

		static readonly Vector2 WEAPON_ICON_POSITION_OFFSET = new Vector2(52.0f, 52.0f);
		static readonly Vector2 WEAPON_ROUNDS_POSITION_OFFSET = new Vector2(54.0f, 36.0f);
		static readonly Vector2 WEAPON_CLIPS_POSITION_OFFSET = new Vector2(54.0f, 58.0f);

        static Vector2 SURVIVAL_TIME_TEXT_POSITION = new Vector2(1100.0f, 2.0f);
        static Vector2 ZOMBIE_KILLS_TEXT_POSITION = new Vector2(900.0f, 2.0f);
        static Vector2 POINTS_TEXT_POSITION = new Vector2(700.0f, 2.0f);
        static Vector2 ZOMBIE_WAVES_TEXT_POSITION = new Vector2(800.0f, 2.0f);

        static Vector2 WEAPON_ICON_POSITION = new Vector2(1048.0f, 848.0f);
        static Vector2 WEAPON_ROUNDS_POSITION = new Vector2(1046.0f, 864.0f);
        static Vector2 WEAPON_CLIPS_POSITION = new Vector2(1046.0f, 842.0f);

        static readonly Color HEALTH_BAR_BACKGROUND_COLOR = new Color(255, 174, 201);
        static readonly Color HEALTH_BAR_BAR_COLOR = new Color(237, 28, 36);
        static readonly Rectangle HEALTH_BAR_FRAME = new Rectangle(60, 2, 200, 10);

        static readonly Color STAMINA_BAR_BACKGROUND_COLOR = new Color(153, 217, 234);
        static readonly Color STAMINA_BAR_BAR_COLOR = new Color(63, 72, 204);
        static readonly Rectangle STAMINA_BAR_FRAME = new Rectangle(60, 14, 200, 10);

		static readonly float WEAPONS_ENABLED_BASEPOINT_OFFSET = 30.0f;
        static Vector2 WEAPONS_ENABLED_BASEPOINT = new Vector2(2.0f, 870.0f);

        static readonly Vector2 MESSAGE_QUEUE_BASEPOINT = new Vector2(10.0f, 52.0f);

		static Rectangle HUD_TOP_BAR_BACKROUND_FRAME = new Rectangle(0, 0, 0, 50);
		static readonly Color HUD_TOP_BAR_BACKGROUND_COLOR = Color.Black;

        #endregion

        #region Static Fields

		static Texture2D pixelTexture;
        static TextureSheet weaponIconsTextureSheet;
        static TextureSheet smallWeaponIconsTextureSheet;

        #endregion

        #region Instance Fields

        GameOptions gameOptions;
        GameResults gameResults;

        HUDBar healthBar;
        HUDBar staminaBar;

        HUDTextTimedQueue messageQueue;

        #endregion

        #region Properties

        public Game Game {
            get;
            private set;
        }

        public int ZombieWaves {
            get {
                return gameResults.NumberOfZombieWaves;
            }
        }

        public string ZombieWavesString {
            get {
                return ZombieWaves.ToString("F0");
            }
        }

        public float SurvivorHealth {
            get {
                return Game.SurvivorSubsystem.PlayerOneSurvivorSprite.CurrentHealth;
            }
        }

        public float SurvivorStamina {
            get {
                return Game.SurvivorSubsystem.PlayerOneSurvivorSprite.CurrentStamina;
            }
        }

        public double SurvivalTime {
            get {
                return gameResults.SurvivalTime;
            }
        }

        private string SurvivalTimeString {
            get {
                TimeSpan survivalTimeSpan = new TimeSpan(0, 0, 0, (int)SurvivalTime, 0);
                return survivalTimeSpan.ToString(@"hh\:mm\:ss");
            }
        }

        public HUDWeaponInfo WeaponInfo {
            get {
                return Game.SurvivorSubsystem.PlayerOneSurvivorSprite.CurrentWeapon.HUDInfo;
            }
        }

        public List<HUDWeaponInfo> WeaponsEnabled {
            get {
                List<HUDWeaponInfo> weaponsEnabled = new List<HUDWeaponInfo>();
                for (int index = 0; index < Game.SurvivorSubsystem.PlayerOneSurvivorSprite.Weapons.Length; index++) {
                    if (Game.SurvivorSubsystem.PlayerOneSurvivorSprite.WeaponsEnabled[index]) {
                        weaponsEnabled.Add(Game.SurvivorSubsystem.PlayerOneSurvivorSprite.Weapons[index].HUDInfo);
                    }
                }
                return weaponsEnabled;
            }
        }

        public float ZombieKills {
            get {
                return gameResults.NumberOfZombieKills;
            }
        }

        private string ZombieKillsString {
            get {
                return ZombieKills.ToString("F0");
            }
        }

        public int Points {
            get {
                return gameResults.Points;
            }
        }

        private string PointsString {
            get {
                return Points.ToString("F0");
            }
        }

        private string WeaponInfoString {
            get {
                return "";
            }
        }

        #endregion

        #region Public Methods

        public HUD(Game game, GameOptions gameOptions) {
            this.Game = game;
            this.gameOptions = gameOptions;
            gameResults = new GameResults();
            healthBar = new HUDBar(HEALTH_BAR_BACKGROUND_COLOR, HEALTH_BAR_BAR_COLOR, HEALTH_BAR_FRAME, "Health:");
            staminaBar = new HUDBar(STAMINA_BAR_BACKGROUND_COLOR, STAMINA_BAR_BAR_COLOR, STAMINA_BAR_FRAME, "Stamina:");
            messageQueue = new HUDTextTimedQueue(MESSAGE_QUEUE_BASEPOINT);
        }

        public static void LoadContent(ContentManager contentManager, int screenWidth, int screenHeight) {
            HUDBar.LoadContent(contentManager);
            HUDTextTimedQueue.LoadContent(contentManager);

			pixelTexture = contentManager.Load<Texture2D>(@"Images/HUD/Pixel");
            weaponIconsTextureSheet = new TextureSheet(contentManager.Load<Texture2D>(@"Images/Weapons/WeaponIcons"), new Point(7, 2), Vector2.Zero);
            smallWeaponIconsTextureSheet = new TextureSheet(contentManager.Load<Texture2D>(@"Images/Weapons/SmallWeaponIcons"), new Point(5, 2), Vector2.Zero);

			//Adjust to Display Size
			HUD_TOP_BAR_BACKROUND_FRAME.Width = screenWidth;
			SURVIVAL_TIME_TEXT_POSITION.X = (float)screenWidth - SURVIVAL_TIME_TEXT_POSITION_OFFSET;
			ZOMBIE_KILLS_TEXT_POSITION.X = (float)screenWidth - ZOMBIE_KILLS_TEXT_POSITION_OFFSET;
			ZOMBIE_WAVES_TEXT_POSITION.X = (float)screenWidth - ZOMBIE_WAVES_TEXT_POSITION_OFFSET;
			POINTS_TEXT_POSITION.X = (float)screenWidth - POINTS_TEXT_POSITION_OFFSET;
			WEAPON_ICON_POSITION.X = (float)screenWidth - WEAPON_ICON_POSITION_OFFSET.X;
			WEAPON_ICON_POSITION.Y = (float)screenHeight - WEAPON_ICON_POSITION_OFFSET.Y;
			WEAPON_ROUNDS_POSITION.X = (float)screenWidth - WEAPON_ROUNDS_POSITION_OFFSET.X;
			WEAPON_ROUNDS_POSITION.Y = (float)screenHeight - WEAPON_ROUNDS_POSITION_OFFSET.Y;
			WEAPON_CLIPS_POSITION.X = (float)screenWidth - WEAPON_CLIPS_POSITION_OFFSET.X;
			WEAPON_CLIPS_POSITION.Y = (float)screenHeight - WEAPON_CLIPS_POSITION_OFFSET.Y;
			WEAPONS_ENABLED_BASEPOINT.Y = (float)screenHeight - WEAPONS_ENABLED_BASEPOINT_OFFSET;
        }

        public void Update(GameTime gameTime, GameResults gameResults) {
            this.gameResults = gameResults;
            healthBar.Value = (float)(SurvivorHealth / 100.0f);
            staminaBar.Value = (float)(SurvivorStamina / 100.0f);
            messageQueue.Update(gameTime);
        }

        public void Draw(GameTime gameTime, Camera camera) {
			camera.SpriteBatch.Draw(pixelTexture, HUD_TOP_BAR_BACKROUND_FRAME, HUD_TOP_BAR_BACKGROUND_COLOR);
            DrawWeaponsStatusArea(camera);
            DrawGameStatusArea(camera);
            DrawSurvivorStatusArea(gameTime, camera);
            messageQueue.Draw(gameTime, camera.SpriteBatch);
        }

        public void AddMessage(String message, Color color, Boolean bold) {
            messageQueue.AddMessage(message, color, bold);
        }

        #endregion

        #region Private Methods

        private void DrawWeaponsEnabled(Camera camera) {
            Vector2 basepoint = WEAPONS_ENABLED_BASEPOINT;
            float width = smallWeaponIconsTextureSheet.CellSourceRectangles[0].Width;
            foreach (HUDWeaponInfo weaponInfo in WeaponsEnabled) {
                smallWeaponIconsTextureSheet.DrawCellAtIndex(camera, basepoint, weaponInfo.IconCellIndex);
                switch (weaponInfo.Type) {
					case HUDWeaponType.Gasoline:
						Vector2 textLocation = new Vector2(basepoint.X + (width / 2.0f), basepoint.Y - 16.0f);
						Fonts.DrawTextTopCenterAligned("DebugSpriteFont", Game.SurvivorSubsystem.PlayerOneSurvivorSprite.Ammunition.GasolineTotal.ToString("F0"), camera.SpriteBatch, textLocation, Color.White);
                        break;
                    case HUDWeaponType.Pistol:
                        textLocation = new Vector2(basepoint.X + (width / 2.0f), basepoint.Y - 16.0f);
						Fonts.DrawTextTopCenterAligned("DebugSpriteFont", Game.SurvivorSubsystem.PlayerOneSurvivorSprite.Ammunition.PistolRoundsTotal.ToString("F0"), camera.SpriteBatch, textLocation, Color.White);
                        break;
                    case HUDWeaponType.Shotgun:
                        textLocation = new Vector2(basepoint.X + (width / 2.0f), basepoint.Y - 16.0f);
						Fonts.DrawTextTopCenterAligned("DebugSpriteFont", Game.SurvivorSubsystem.PlayerOneSurvivorSprite.Ammunition.ShotgunShellsTotal.ToString("F0"), camera.SpriteBatch, textLocation, Color.White);
                        break;
                    default:
                        break;
                }
                basepoint.X += width;
            }
        }

        private void DrawWeaponsStatusArea(Camera camera) {
            DrawWeaponsEnabled(camera);
            weaponIconsTextureSheet.DrawCellAtIndex(camera, WEAPON_ICON_POSITION, WeaponInfo.IconCellIndex);
            switch (WeaponInfo.Type) {
				case HUDWeaponType.Gasoline:
					Fonts.DrawTextTopRightAligned("MediumLEDSpriteFont", Game.SurvivorSubsystem.PlayerOneSurvivorSprite.Ammunition.GasolineCans.ToString("F0"), camera.SpriteBatch, WEAPON_CLIPS_POSITION, Color.White);
					Fonts.DrawTextTopRightAligned("MediumLEDSpriteFont", Game.SurvivorSubsystem.PlayerOneSurvivorSprite.Ammunition.GasolineInCurrentCan.ToString("F0"), camera.SpriteBatch, WEAPON_ROUNDS_POSITION, Color.White);
                    break;
                case HUDWeaponType.Pistol:
					Fonts.DrawTextTopRightAligned("MediumLEDSpriteFont", Game.SurvivorSubsystem.PlayerOneSurvivorSprite.Ammunition.PistolClips.ToString("F0"), camera.SpriteBatch, WEAPON_CLIPS_POSITION, Color.White);
					Fonts.DrawTextTopRightAligned("MediumLEDSpriteFont", Game.SurvivorSubsystem.PlayerOneSurvivorSprite.Ammunition.PistolRoundsInCurrentClip.ToString("F0"), camera.SpriteBatch, WEAPON_ROUNDS_POSITION, Color.White);
                    break;
                case HUDWeaponType.Shotgun:
					Fonts.DrawTextTopRightAligned("MediumLEDSpriteFont", Game.SurvivorSubsystem.PlayerOneSurvivorSprite.Ammunition.ShotgunShellsInBandolier.ToString("F0"), camera.SpriteBatch, WEAPON_CLIPS_POSITION, Color.White);
					Fonts.DrawTextTopRightAligned("MediumLEDSpriteFont", Game.SurvivorSubsystem.PlayerOneSurvivorSprite.Ammunition.ShotgunShellsInMagazine.ToString("F0"), camera.SpriteBatch, WEAPON_ROUNDS_POSITION, Color.White);
                    break;
                default:
                    break;
            }
        }

        private void DrawGameStatusText(Camera camera, String title, String value, Vector2 baseline) {
			Fonts.DrawTextTopRightAligned("LargeLEDSpriteFont", value, camera.SpriteBatch, baseline, Color.White);
			Fonts.DrawTextTopRightAligned("HUDLabelSpriteFont", title, camera.SpriteBatch, baseline, Color.White);
        }

        private void DrawGameStatusArea(Camera camera) {
            DrawGameStatusText(camera, "Survival Time", SurvivalTimeString, SURVIVAL_TIME_TEXT_POSITION);
            DrawGameStatusText(camera, "Points", PointsString, POINTS_TEXT_POSITION);
            DrawGameStatusText(camera, "Kills", ZombieKillsString, ZOMBIE_KILLS_TEXT_POSITION);
            DrawGameStatusText(camera, "Waves", ZombieWavesString, ZOMBIE_WAVES_TEXT_POSITION);
        }

        private void DrawSurvivorStatusArea(GameTime gameTime, Camera camera) {
            staminaBar.Draw(gameTime, camera.SpriteBatch);
            healthBar.Draw(gameTime, camera.SpriteBatch);
        }

        #endregion

    }

}
