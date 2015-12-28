using System;
using System.IO;
using System.Linq;
using log4net;

namespace ConsoleApplication1
{
    class Program
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Program));

        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            var is64BitProcess = Environment.Is64BitOperatingSystem;
            var is64BitOS = Environment.Is64BitOperatingSystem;
            var intPointerSize = IntPtr.Size;
            var osVersion = Environment.OSVersion;
            var processorCount = Environment.ProcessorCount;
            var pageSize = Environment.SystemPageSize;
            var drives = DriveInfo.GetDrives();
            Logger.InfoFormat("64 bit process {0} ; 64 bit OS {1} ; IntPtr size {2}B ; {3} processors ; {4}KB page size ; {5} OS version" + 
                               "{6} drives", 
                               is64BitProcess, is64BitOS, intPointerSize, processorCount, pageSize / 1024, osVersion, drives.Count());

            foreach (var drive in drives.Where(d => d.DriveType == DriveType.Fixed))
            {
                Logger.InfoFormat("Drive {0} {1} {2} {3} {4}",  drive.Name, drive.DriveType, drive.DriveFormat, drive.TotalSize, drive.RootDirectory);
            }
            Logger.InfoFormat("Press any key to finish");
            Console.ReadKey();
        }
    }
}
