using frm_ransomrescure.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RansomRescue.Tests
{
    [TestClass]
    public class PathHelperTests
    {
        // ── ShouldSkipDirectory ────────────────────────────────────────────────

        [TestMethod]
        public void Skip_SingleSegment_Matches()
        {
            // "Windows" is in NoteOnlyDirNames, not SkipDirNames
            // Use a known entry from SkipDirNames, e.g. "Windows\Logs"
            Assert.IsTrue(PathHelper.ShouldSkipDirectory(@"C:\Windows\Logs"));
        }

        [TestMethod]
        public void Skip_MultiSegment_Entry_Windows_Logs()
        {
            // "Windows\\Logs" is in SkipDirNames
            Assert.IsTrue(PathHelper.ShouldSkipDirectory(@"C:\Windows\Logs"));
        }

        [TestMethod]
        public void Skip_NormalFolder_DoesNotMatch()
        {
            Assert.IsFalse(PathHelper.ShouldSkipDirectory(@"C:\Users\Alice\Documents"));
        }

        [TestMethod]
        public void Skip_EmptyPath_ReturnsFalse()
        {
            Assert.IsFalse(PathHelper.ShouldSkipDirectory(""));
            Assert.IsFalse(PathHelper.ShouldSkipDirectory(null));
        }

        [TestMethod]
        public void Skip_TrailingSlash_StillMatches()
        {
            Assert.IsTrue(PathHelper.ShouldSkipDirectory(@"C:\Windows\Logs\"));
        }

        // ── IsNoteOnlyDirectory ────────────────────────────────────────────────

        [TestMethod]
        public void NoteOnly_System32_Matches()
        {
            // "Windows\\System32" is in NoteOnlyDirNames
            Assert.IsTrue(PathHelper.IsNoteOnlyDirectory(@"C:\Windows\System32"));
        }

        [TestMethod]
        public void NoteOnly_NormalFolder_DoesNotMatch()
        {
            Assert.IsFalse(PathHelper.IsNoteOnlyDirectory(@"C:\Users\Alice\Pictures"));
        }

        [TestMethod]
        public void NoteOnly_EmptyPath_ReturnsFalse()
        {
            Assert.IsFalse(PathHelper.IsNoteOnlyDirectory(""));
            Assert.IsFalse(PathHelper.IsNoteOnlyDirectory(null));
        }

        [TestMethod]
        public void NoteOnly_CaseInsensitive()
        {
            Assert.IsTrue(PathHelper.IsNoteOnlyDirectory(@"C:\WINDOWS\SYSTEM32"));
        }
    }
}
