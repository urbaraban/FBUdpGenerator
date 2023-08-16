namespace FBUdpGenerator.Services
{
    public static class ShuffleGenerator
    {
        public static byte[] GetByteArray(int sizeInKb)
        {
            Random rnd = new Random();
            byte[] b = new byte[sizeInKb * 1024]; // convert kb to byte
            rnd.NextBytes(b);
            return b;
        }
    }
}
