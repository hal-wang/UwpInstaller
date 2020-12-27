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
            var cerPath = GetFilePath(_cerExtension);
            if (string.IsNullOrEmpty(cerPath)) throw new FileNotFoundException("未找到证书");

            var bundlePath = GetFilePath(_bundleExetension);
            if (string.IsNullOrEmpty(bundlePath)) throw new FileNotFoundException("未找到安装包");

            InstallCertifacate(cerPath);
            OpenInstaller(bundlePath);
        }

        private string GetFilePath(string[] extensions)
        {
            foreach (string extension in extensions)
            {
                var path = Directory.GetFiles(Environment.CurrentDirectory)
                    .Where((name) => name.LastIndexOf(extension) == name.Length - extension.Length)
                    .FirstOrDefault();
                if (!string.IsNullOrEmpty(path)) return path;
            }

            return null;
        }

        private void OpenInstaller(string path)
        {
            var process = new Process();
            process.StartInfo.FileName = path;
            process.StartInfo.Verb = "Open";
            process.StartInfo.CreateNoWindow = true;
            process.Start();
        }

        /// <summary>
        /// 安装CA的根证书到受信任根证书颁发机构
        /// </summary>
        /// <param name="path"></param>
        private void InstallCertifacate(string path)
        {
            var store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadWrite);
            store.Add(new X509Certificate2(path));
            store.Close();
        }
    }
}
