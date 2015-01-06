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

    [Serializable]
    public struct AmmunitionSaveStruct {
        public int PistolRoundsInCurrentClip;
        public int PistolClips;
        public float GasolineInCurrentCan;
        public int GasolineCans;
        public int ShotgunShellsInMagazine;
        public int ShotgunShellsInBandolier;
    }

    public class Ammunition {

        #region Pistol Ammo

        static readonly int PISTOL_ROUNDS_PER_CLIP = 11;
        static readonly int MAXIMUM_PISTOL_CLIPS = 10;

        public int PistolRoundsInCurrentClip {
            get;
            set;
        }

        public int PistolClips {
            get;
            set;
        }

        public int PistolRoundsTotal {
            get {
                return (PistolClips * PISTOL_ROUNDS_PER_CLIP) + PistolRoundsInCurrentClip;
            }
        }

        public Boolean ReloadPistolClip() {
            if (PistolClips > 0) {
                PistolClips--;
                PistolRoundsInCurrentClip = PISTOL_ROUNDS_PER_CLIP;
                return true;
            } else {
                return false;
            }
        }

        public Boolean CollectPistolClip() {
            if (PistolClips < MAXIMUM_PISTOL_CLIPS) {
                PistolClips++;
                return true;
            } else {
                return false;
            }
        }

        #endregion

        #region Gasoline

        static readonly float GASOLINE_PER_CAN = 100.0f;
        static readonly int MAXIMUM_GASOLINE_CANS = 3;

        public float GasolineInCurrentCan {
            get;
            set;
        }

        public int GasolineCans {
            get;
            set;
        }

        public float GasolineTotal {
            get {
                return (GASOLINE_PER_CAN * GasolineCans) + GasolineInCurrentCan;
            }
        }

        public void ReloadGasolineCan() {
            if (GasolineCans > 0) {
                GasolineCans--;
                GasolineInCurrentCan = GASOLINE_PER_CAN;
            }
        }

        public void CollectGasolineCan() {
            if (GasolineCans < MAXIMUM_GASOLINE_CANS) {
                GasolineCans++;
            }
        }

        #endregion

        #region Shotgun Ammo

        static readonly int SHOTGUN_SHELLS_PER_MAGAZINE = 6;
        static readonly int MAXIMUM_SHOTGUN_SHELLS_IN_BANDOLIER = 30;

        public int ShotgunShellsInMagazine {
            get;
            set;
        }

        public int ShotgunShellsInBandolier {
            get;
            set;
        }

        public int ShotgunShellsTotal {
            get {
                return ShotgunShellsInMagazine + ShotgunShellsInBandolier;
            }
        }

        public int ReloadShotgunMagazine() {
            if (ShotgunShellsInMagazine < SHOTGUN_SHELLS_PER_MAGAZINE) {
                int shellsNeeded = SHOTGUN_SHELLS_PER_MAGAZINE - ShotgunShellsInMagazine;
                if (shellsNeeded > ShotgunShellsInBandolier) {
                    ShotgunShellsInMagazine += ShotgunShellsInBandolier;
                    ShotgunShellsInBandolier = 0;
                    return ShotgunShellsInBandolier;
                } else {
                    ShotgunShellsInMagazine += shellsNeeded;
                    ShotgunShellsInBandolier -= shellsNeeded;
                    return shellsNeeded;
                }
            } else {
                return 0;
            }
        }

        public void CollectShotgunShells(int numberOfShells) {
            ShotgunShellsInBandolier += numberOfShells;
            if (ShotgunShellsInBandolier > MAXIMUM_SHOTGUN_SHELLS_IN_BANDOLIER) {
                ShotgunShellsInBandolier = MAXIMUM_SHOTGUN_SHELLS_IN_BANDOLIER;
            }
        }

        #endregion

        public Ammunition(int pistolRoundsInCurrentClip, int pistolClips, float gasolineInCurrentCan, int gasolineCans, int shotgunShellsInMagazine, int shotgunShellsInBandolier) {
            this.PistolRoundsInCurrentClip = pistolRoundsInCurrentClip;
            this.PistolClips = pistolClips;
            this.GasolineInCurrentCan = gasolineInCurrentCan;
            this.GasolineCans = gasolineCans;
            this.ShotgunShellsInMagazine = shotgunShellsInMagazine;
            this.ShotgunShellsInBandolier = shotgunShellsInBandolier;
        }

        public Ammunition(AmmunitionSaveStruct saveStruct)
            : this(saveStruct.PistolRoundsInCurrentClip, saveStruct.PistolClips, saveStruct.GasolineInCurrentCan, saveStruct.GasolineCans, saveStruct.ShotgunShellsInMagazine, saveStruct.ShotgunShellsInBandolier) {

        }

        public Ammunition()
            : this(11, 5, 100.0f, 1, 6, 30) {

        }

        public AmmunitionSaveStruct AmmunitionSaveStruct {
            get {
                return new AmmunitionSaveStruct() {
                    PistolClips = this.PistolClips,
                    PistolRoundsInCurrentClip = this.PistolRoundsInCurrentClip,
                    GasolineInCurrentCan = this.GasolineInCurrentCan,
                    GasolineCans = this.GasolineCans,
                    ShotgunShellsInMagazine = this.ShotgunShellsInMagazine,
                    ShotgunShellsInBandolier = this.ShotgunShellsInBandolier
                };
            }
        }

    }

}
