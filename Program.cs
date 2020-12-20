using System;

namespace UwpInstaller
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                new Installer().Run();
            }
            catch (System.Security.Cryptography.CryptographicException)
            {
                Console.WriteLine("请以管理员身份运行");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
        }
    }
}
