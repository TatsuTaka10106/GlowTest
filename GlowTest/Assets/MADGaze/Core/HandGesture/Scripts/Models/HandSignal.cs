using System.Collections;
using System.Collections.Generic;

namespace MADGazeSDK {
    public class HandSignal
    {
        public enum Type {
            SIGNAL_ONE,
            SINGAL_TWO,
            SIGNAL_THREE,
            SIGNAL_FOUR,
            SIGNAL_FIVE,
            SIGNAL_OK,
            SIGNAL_FIST,
            UNKNOWN
        }

        public enum Direction {
            LEFT,
            RIGHT
        }

        public enum Action {    
            TRACKED, LOST
        }
    }

}