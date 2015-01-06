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

    public class TimedQueue<T> {

        #region Fields

        Int32 queueSize;
        Double queueLifetime;

        T[] values;
        Double[] timers;
        Boolean[] exists;
        Int32 topPointer;

        #endregion

        #region Properties

        public T this[Int32 index] {
            get {
                if (exists[index]) {
                    return values[index];
                } else {
                    return default(T);
                }
            }
        }

        public Int32 CurrentSize {
            get {
                return topPointer;
            }
        }

        public T[] CurrentValues {
            get {
                T[] currentValues = new T[topPointer];
                for (Int32 index = 0; index < topPointer; index++) {
                    currentValues[index] = values[index];
                }
                return currentValues;
            }
        }

        #endregion

        #region Methods

        public TimedQueue(Int32 queueSize, Double queueLifetime) {
            this.queueSize = queueSize;
            this.queueLifetime = queueLifetime;
            this.values = new T[queueSize];
            this.timers = new Double[queueSize];
            this.exists = new Boolean[queueSize];
            this.topPointer = 0;
        }

        public void Add(T value) {
            if (topPointer == queueSize) Eject();
            values[topPointer] = value;
            timers[topPointer] = queueLifetime;
            exists[topPointer] = true;
            topPointer++;
        }

        public void Eject() {
            for (Int32 index = 1; index < topPointer; index++) {
                values[index - 1] = values[index];
                timers[index - 1] = timers[index];
                exists[index - 1] = exists[index];
            }
            values[topPointer - 1] = default(T);
            timers[topPointer - 1] = 0.0;
            exists[topPointer - 1] = false;
            topPointer--;
        }

        public void Flush() {
            for (Int32 index = 0; index < topPointer; index++) {
                values[index] = default(T);
                timers[index] = 0.0;
                exists[index] = false;
            }
            topPointer = 0;
        }

        public void Update(Double deltaTime) {
            for (Int32 index = 0; index < topPointer; index++) {
                timers[index] -= deltaTime;
            }
            while (timers[0] <= 0.0 && topPointer > 0) {
                Eject();
            }
        }

        #endregion

    }

}
