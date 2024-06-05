namespace Bubla
{
    public class Timer
    {
        private float defaultTime;
        private float time;
        public Timer(float time)
        {
            this.defaultTime = time;
            this.time = time;
        }

        public void Add(float delta)
        {
            time += delta;
        }

        public float GetFloat()
        {
            return time;
        }

        public int GetInt()
        {
            return (int)time;
        }

        public void Reset()
        {
            time = defaultTime;
        }

        public void Reset(float time)
        {
            time = defaultTime;
            this.time = time;
        }
    }
}
