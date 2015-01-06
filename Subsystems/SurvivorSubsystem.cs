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

namespace WarehouseZombieAttack {

    public class SurvivorSubsystem : Subsystem {

        #region Fields

        #endregion

        #region Properties

        public SurvivorSprite PlayerOneSurvivorSprite {
            get;
            private set;
        }

        public SurvivorSprite PlayerTwoSurvivorsSprite {
            get;
            private set;
        }

        public ControlManager SurvivorsControlManager {
            get;
            private set;
        }

        public UpdateManager SurvivorsUpdateManger {
            get;
            private set;
        }

        public CollisionManager SurvivorsCollisionManager {
            get;
            private set;
        }

        public AttackManager SurvivorsAttackManager {
            get;
            private set;
        }

        #endregion

        #region Public Methods

        public SurvivorSubsystem(Game game)
            : base(game) {
                this.SurvivorsControlManager = new ControlManager(game);
                this.SurvivorsUpdateManger = new UpdateManager(game);
                this.SurvivorsCollisionManager = new CollisionManager(game);
                this.SurvivorsAttackManager = new AttackManager(game);
        }

        public static void LoadContent(ContentManager contentManager) {
            SurvivorSprite.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime) {
            SurvivorsControlManager.ControlEntities(gameTime);
            SurvivorsUpdateManger.UpdateEntities(gameTime);
        }

        public void AddPlayerOneSurvivorSprite(SurvivorSprite survivorSprite) {
            SurvivorsControlManager.PlayerOneControllableEntity = survivorSprite;
            Game.SpritesDrawingManager.DrawableEntities.Add(survivorSprite);
            SurvivorsUpdateManger.UpdatableEntities.Add(survivorSprite);
            SurvivorsCollisionManager.CollidableEntities.Add(survivorSprite);
            SurvivorsAttackManager.AttackableEntities.Add(survivorSprite);
            PlayerOneSurvivorSprite = survivorSprite;
        }

        public void AddPlayerTwoSurvivorSprite(SurvivorSprite survivorSprite) {
            SurvivorsControlManager.PlayerTwoControllableEntity = survivorSprite;
            Game.SpritesDrawingManager.DrawableEntities.Add(survivorSprite);
            SurvivorsUpdateManger.UpdatableEntities.Add(survivorSprite);
            SurvivorsCollisionManager.CollidableEntities.Add(survivorSprite);
            SurvivorsAttackManager.AttackableEntities.Add(survivorSprite);
            PlayerTwoSurvivorsSprite = survivorSprite;
        }

        #endregion

        #region Private Methods

        #endregion

    }

}
