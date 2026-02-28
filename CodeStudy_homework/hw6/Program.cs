
namespace hw6
{
    public class Program
    {
        public static void Main()
        {
            Ringbuffer<int> buffer = new Ringbuffer<int>(4);

            const int siz = 10;
            int []a = new int[siz];

            for(int p = 0; p < 10000000; p++)
            {
                for (int i = 0; i < siz; i++)
                {
                    buffer.Enqueue(i);
                }



                while (buffer.IsEmpty() == false)
                {
                    a[buffer.Dequeue()]++;
                }
            }

            foreach(var i in a)
            {
                Console.WriteLine(i);
            }
        }
    }
}
