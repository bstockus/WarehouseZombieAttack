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

    public class GameControl {

        #region Constants

        #endregion

        #region Fields

        GamePadState newGamePadState;
        GamePadState oldGamePadState;
		KeyboardState newKeyboardState;
		KeyboardState oldKeyboardState;
		MouseState newMouseState;
		MouseState oldMouseState;

        #endregion

        #region Properties

        public Single OrientationControlX {
            get {
                return newGamePadState.ThumbSticks.Left.X;
            }
        }

        public Single OrientationControlY {
            get {
                return newGamePadState.ThumbSticks.Left.Y * -1;
            }
        }

        public Single MovementControlX {
            get {
                return newGamePadState.ThumbSticks.Right.X;
            }
        }

		public Single MovementControlY {
			get {
				return newGamePadState.ThumbSticks.Right.Y;
			}
		}

		public Boolean MoveForward {
			get {
				return newKeyboardState.IsKeyDown(Keys.W);
			}
		}

		public Boolean MoveBackwards {
			get {
				return newKeyboardState.IsKeyDown(Keys.S);
			}
		}

		public Boolean StrafeLeft {
			get {
				return newKeyboardState.IsKeyDown(Keys.Q);
			}
		}

		public Boolean StrafeRight {
			get {
				return newKeyboardState.IsKeyDown(Keys.E);
			}
		}

        public Boolean turnLeft {
			get {
				return newKeyboardState.IsKeyDown(Keys.A);
			}
		}

		public Boolean turnRight {
			get {
				return newKeyboardState.IsKeyDown(Keys.D);
			}
		}

        public Boolean RunningControl {
            get {
				return (newGamePadState.Buttons.RightStick == ButtonState.Pressed ? true : false
					|| newKeyboardState.IsKeyDown(Keys.LeftShift));
            }
        }

        public Boolean WeaponPreviousCycleControl {
            get {
				return ((newGamePadState.Buttons.LeftShoulder == ButtonState.Pressed ? true : false) 
					&& !(oldGamePadState.Buttons.LeftShoulder == ButtonState.Pressed ? true : false))
					|| (newKeyboardState.IsKeyDown(Keys.OemTilde) && oldKeyboardState.IsKeyUp(Keys.OemTilde))
					|| (newMouseState.ScrollWheelValue > oldMouseState.ScrollWheelValue);
            }
        }

        public Boolean WeaponNextCycleControl {
            get {
				return ((newGamePadState.Buttons.RightShoulder == ButtonState.Pressed ? true : false)
					&& !(oldGamePadState.Buttons.RightShoulder == ButtonState.Pressed ? true : false))
					|| (newKeyboardState.IsKeyDown(Keys.Tab) && oldKeyboardState.IsKeyUp(Keys.Tab))
					|| (newMouseState.ScrollWheelValue < oldMouseState.ScrollWheelValue);
            }
        }

        public Boolean WeaponReloadControl {
            get {
				return ((newGamePadState.Buttons.LeftStick == ButtonState.Pressed ? true : false)
					&& !(oldGamePadState.Buttons.LeftStick == ButtonState.Pressed ? true : false))
					|| (newKeyboardState.IsKeyDown(Keys.R) && oldKeyboardState.IsKeyUp(Keys.R))
					|| (newMouseState.MiddleButton == ButtonState.Pressed && oldMouseState.MiddleButton != ButtonState.Pressed);

            }
        }

        public Boolean WeaponRightFireControl {
            get {
				return (newGamePadState.Triggers.Right >= 0.4f)
					|| newKeyboardState.IsKeyDown(Keys.Space)
					|| newMouseState.LeftButton == ButtonState.Pressed;
            }
        }

        public Boolean WeaponLeftFireControl {
            get {
				return (newGamePadState.Triggers.Left >= 0.4f)
					|| newKeyboardState.IsKeyDown(Keys.Enter)
					|| newMouseState.RightButton == ButtonState.Pressed;
            }
        }

        #endregion

        #region Methods

        public GameControl(GamePadState newGamePadState, GamePadState oldGamePadState, KeyboardState newKeyboardState, KeyboardState oldKeyboardState, MouseState newMouseState, MouseState oldMouseState) {
            this.newGamePadState = newGamePadState;
            this.oldGamePadState = oldGamePadState;
			this.newKeyboardState = newKeyboardState;
			this.oldKeyboardState = oldKeyboardState;
			this.newMouseState = newMouseState;
			this.oldMouseState = oldMouseState;
        }

        #endregion

    }

}
