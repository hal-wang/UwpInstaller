using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace UwpInstaller
{
    public class Installer
    {
        private readonly string[] _cerExtension = { ".cer" };
        private readonly string[] _bundleExetension = { ".msixbundle", ".appxbundle", ".appx" };

        public void Run()
        {
            var cerPath = GetExetension(_cerExtension);
            if (string.IsNullOrEmpty(cerPath))
            {
                throw new FileNotFoundException("未找到证书");
            }

            var bundlePath = GetExetension(_bundleExetension);
            if (string.IsNullOrEmpty(bundlePath))
            {
                throw new FileNotFoundException("未找到安装包");
            }

            InstallCertifacate(cerPath);
            OpenInstaller(bundlePath);
        }

        private string GetExetension(string[] exetensions)
        {
            foreach (string exetension in exetensions)
            {
                var path = Directory.GetFiles(Environment.CurrentDirectory)
                    .Where((name) => name.LastIndexOf(exetension) == name.Length - exetension.Length)
                    .FirstOrDefault();
                if (!string.IsNullOrEmpty(path)) return path;
            }

            return null;
        }

        private void OpenInstaller(string path)
        {
            Process MyProcess = new Process();
            MyProcess.StartInfo.FileName = path;
            MyProcess.StartInfo.Verb = "Open";
            MyProcess.StartInfo.CreateNoWindow = true;
            MyProcess.Start();
        }

        private void InstallCertifacate(string path)
        {
            //安装CA的根证书到受信任根证书颁发机构
            X509Certificate2 certificate = new X509Certificate2(path);
            X509Store store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadWrite);
            store.Add(certificate);
            store.Close();
        }
    }
}
