using System;

namespace MADGazeSDK
{
    public class HandGestureEvent { }
    public class Click : HandGestureEvent
    {
        public int index { get; }

        public int x { get; }

        public int y { get; }

        public Click(int index, int x, int y)
        {
            this.index = index;
            this.x = x;
            this.y = y;
        }
    }

    public class Grab : HandGestureEvent
    {
        public enum GrabStatus
        {
            START,
            HOLDING,
            RELEASE,
            CANCEL
        }

        public int index { get; }

        public int x { get; }

        public int y { get; }

        public int dx { get; }
        public int dy { get; }

        public GrabStatus status { get; set; }

        public Grab(GrabStatus status, int index, int x, int y, int dx, int dy)
        {
            this.index = index;
            this.status = status;
            this.x = x;
            this.y = y;
            this.dx = dx;
            this.dy = this.dy;
        }
    }

    public class Hold : HandGestureEvent
    {

        public enum HoldStatus
        {
            START,
            CANCEL
        }

        public HoldStatus status { get; }
        public HandType type { get; }
        public int index { get; }
        public int x { get; }
        public int y { get; }

        public Hold(HoldStatus status, int index, string typeName, int x, int y)
        {
            this.status = status;
            this.index = index;
            type = (HandType)Enum.Parse(typeof(HandType), typeName);
            this.x = x;
            this.y = y;
        }
    }

    public class HandDetected : HandGestureEvent
    {
        public Hand hand { get; set; }
        public bool isHand { get; set; }

        public HandDetected(Hand hand, bool isHand)
        {
            this.hand = hand;
            this.isHand = isHand;
        }
    }
}
