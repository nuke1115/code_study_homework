
namespace hw6
{
    public class Program
    {
        public static void Main()
        {
            Ringbuffer<int> buffer = new Ringbuffer<int>(2);

            for(int i = 0; i < 100; i++)
            {
                buffer.Enqueue(i);
            }
        }
    }
}
