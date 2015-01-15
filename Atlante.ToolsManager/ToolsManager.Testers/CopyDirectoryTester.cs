using Atlante.Common;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolsManager.Testers
{
    [TestFixture]
    public class CopyDirectoryTester
    {
        [Test]
        public static void CopyEmptyDirectoryTest()
        {
            string pathDir1 = Path.Combine(Path.GetTempPath(), "EmptyDirectory1");
            string pathDir2 = Path.Combine(Path.GetTempPath(), "EmptyDirectory2");

            try
            {
                Assert.IsFalse(Directory.Exists(pathDir1), "EmptyDirectory1 must not exists at the begining");
                Assert.IsFalse(Directory.Exists(pathDir2), "EmptyDirectory2 must not exists at the begining");

                Directory.CreateDirectory(pathDir1);

                FileUtilities.CopyDirectory(pathDir1, pathDir2, "*.*", true, true);

                Assert.IsTrue(Directory.Exists(pathDir2), "EmptyDirectory2 was not created");
            }
            finally
            {
                Directory.Delete(pathDir1);
                Directory.Delete(pathDir2);
            }
        }

        [Test]
        public static void CopyOneLevelDirectoryTest()
        {
            string pathDir1 = Path.Combine(Path.GetTempPath(), "DirectoryLevel1");
            string pathDir2 = Path.Combine(pathDir1, "DirectoryLevel2");
            string pathFile1 = Path.Combine(pathDir1, "File1.txt");
            string pathFile2 = Path.Combine(pathDir2, "File2.txt");
            string pathDirTarget = Path.Combine(Path.GetTempPath(), "DirectoryTarget");

            try
            {
                Assert.IsFalse(Directory.Exists(pathDir1), "Directory level 1 must not exists at the begining");
                Assert.IsFalse(Directory.Exists(pathDir2), "Directory level 2 must not exists at the begining");
                Assert.IsFalse(File.Exists(pathFile1), "File 1 must not exists at the begining");
                Assert.IsFalse(File.Exists(pathFile2), "File 2 must not exists at the begining");
                Assert.IsFalse(Directory.Exists(pathDirTarget), "Directory target must not exists at the begining");

                Directory.CreateDirectory(pathDir1);
                Directory.CreateDirectory(pathDir2);
                File.Create(pathFile1);
                File.Create(pathFile2);

                FileUtilities.CopyDirectory(pathDir1, pathDirTarget, "*.*", true, true);

                Assert.IsTrue(Directory.Exists(pathDirTarget), "Directory level 1 was not created");
                Assert.IsTrue(Directory.Exists(Path.Combine(pathDirTarget, "DirectoryLevel1")), "Directory level 2 was not created");
                Assert.IsTrue(Directory.Exists(Path.Combine(pathDirTarget, "DirectoryLevel2")), "Directory level 2 was not created");
                Assert.IsTrue(File.Exists(Path.Combine(pathDirTarget, "DirectoryLevel1", "File1.txt")), "File level 1 was not created");
                Assert.IsTrue(File.Exists(Path.Combine(pathDirTarget, "DirectoryLevel2", "File2.txt")), "File level 2was not created");
            }
            finally
            {
                if (Directory.Exists(pathDir1))
                    Directory.Delete(pathDir1, true);

                if (Directory.Exists(pathDirTarget))
                    Directory.Delete(pathDirTarget, true);
            }
        }

        [Test]
        public static void CopyTwoLevelDirectoryTest()
        {
        }

        [Test]
        public static void OverrideDirectoryTest()
        {
        }

        [Test]
        public static void CopyReadOnlyDirectoryTest()
        {
        }
    }
}
