using System.Collections.Generic;
using frm_ransomrescure.Core;
using frm_ransomrescure.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RansomRescue.Tests
{
    [TestClass]
    public class IdentifierTests
    {
        // ── Helpers ────────────────────────────────────────────────────────────

        private static RansomFamily MakeFamily(
            string id,
            string[] exts       = null,
            string[] notes      = null,
            string[] keywords   = null,
            string[] magicBytes = null)
        {
            return new RansomFamily
            {
                Id             = id,
                Name           = id,
                Extensions     = new List<string>(exts     ?? new string[0]),
                RansomNotes    = new List<string>(notes    ?? new string[0]),
                NoteKeywords   = new List<string>(keywords ?? new string[0]),
                MagicBytes     = new List<string>(magicBytes ?? new string[0]),
            };
        }

        private static ScanResult ScanWith(
            string ext          = null,
            string notePath     = null,
            string noteContent  = null,
            string magicHex     = null,
            string magicExt     = null)
        {
            var s = new ScanResult();
            if (ext != null)
                s.ExtensionFrequency[ext] = 1;
            if (notePath != null)
            {
                s.RansomNotePaths.Add(notePath);
                if (noteContent != null)
                    s.RansomNoteContents[notePath] = noteContent;
            }
            if (magicHex != null)
                s.MagicByteEvidence[magicHex] = magicExt ?? ext ?? ".enc";
            return s;
        }

        private static List<RansomFamily> Db(params RansomFamily[] families)
            => new List<RansomFamily>(families);

        // ── Null guard ─────────────────────────────────────────────────────────

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void Identify_NullScan_Throws()
            => Identifier.Identify(null, new List<RansomFamily>());

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void Identify_NullFamilies_Throws()
            => Identifier.Identify(new ScanResult(), null);

        // ── Empty DB ───────────────────────────────────────────────────────────

        [TestMethod]
        public void Identify_EmptyDb_ReturnsUnknown()
        {
            var result = Identifier.Identify(ScanWith(ext: ".djvu"), Db());
            Assert.IsTrue(result.IsUnknown);
            Assert.AreEqual(0, result.TopScore);
        }

        // ── Extension scoring (+50) ─────────────────────────────────────────────

        [TestMethod]
        public void Identify_ExtensionMatch_Gives50Points()
        {
            var family = MakeFamily("stop", exts: new[] { ".djvu" });
            var result = Identifier.Identify(ScanWith(ext: ".djvu"), Db(family));

            Assert.AreEqual(50, result.TopScore);
            Assert.AreEqual(family.Id, result.TopFamily.Id);
            Assert.IsFalse(result.IsConfirmed, "50đ alone = NGHI NGỜ, not confirmed");
        }

        [TestMethod]
        public void Identify_NoExtensionMatch_ZeroPoints()
        {
            var family = MakeFamily("stop", exts: new[] { ".djvu" });
            var result = Identifier.Identify(ScanWith(ext: ".locked"), Db(family));

            Assert.IsTrue(result.IsUnknown);
        }

        [TestMethod]
        public void Identify_ExtensionCaseInsensitive()
        {
            var family = MakeFamily("x", exts: new[] { ".DJVU" });
            var result = Identifier.Identify(ScanWith(ext: ".djvu"), Db(family));

            Assert.AreEqual(50, result.TopScore);
        }

        // ── Note scoring (+30) ─────────────────────────────────────────────────

        [TestMethod]
        public void Identify_NoteMatch_Gives30Points()
        {
            var family = MakeFamily("stop", notes: new[] { "_readme.txt" });
            var scan   = ScanWith(notePath: @"C:\Users\test\_readme.txt");
            var result = Identifier.Identify(scan, Db(family));

            Assert.AreEqual(30, result.TopScore);
            Assert.IsFalse(result.IsConfirmed);
        }

        [TestMethod]
        public void Identify_ExtAndNote_Gives80_IsConfirmed()
        {
            var family = MakeFamily("stop",
                exts:  new[] { ".djvu" },
                notes: new[] { "_readme.txt" });
            var scan = ScanWith(ext: ".djvu", notePath: @"C:\Users\test\_readme.txt");
            var result = Identifier.Identify(scan, Db(family));

            Assert.AreEqual(80, result.TopScore);
            Assert.IsTrue(result.IsConfirmed);
        }

        // ── Keyword scoring (+20) ──────────────────────────────────────────────

        [TestMethod]
        public void Identify_KeywordInNote_Gives20Points()
        {
            var family = MakeFamily("stop",
                notes:    new[] { "_readme.txt" },
                keywords: new[] { "datarestorehelper@firemail.cc" });

            var scan = ScanWith(
                notePath:    @"C:\test\_readme.txt",
                noteContent: "Contact us: datarestorehelper@firemail.cc");

            var result = Identifier.Identify(scan, Db(family));
            // note(30) + keyword(20) = 50
            Assert.AreEqual(50, result.TopScore);
        }

        [TestMethod]
        public void Identify_ExtNoteKeyword_Gives100_IsConfirmed()
        {
            var family = MakeFamily("stop",
                exts:     new[] { ".djvu" },
                notes:    new[] { "_readme.txt" },
                keywords: new[] { "datarestorehelper@firemail.cc" });

            var scan = ScanWith(
                ext:         ".djvu",
                notePath:    @"C:\test\_readme.txt",
                noteContent: "Contact: datarestorehelper@firemail.cc");

            var result = Identifier.Identify(scan, Db(family));
            Assert.AreEqual(100, result.TopScore);
            Assert.IsTrue(result.IsConfirmed);
        }

        // ── Magic bytes scoring (+15) ──────────────────────────────────────────

        [TestMethod]
        public void Identify_MagicBytesMatch_Gives15Points()
        {
            var family = MakeFamily("wannacry",
                magicBytes: new[] { "57414E41435259210000000000000000" });

            var scan = ScanWith(magicHex: "57414E41435259210000000000000000", magicExt: ".wnry");
            var result = Identifier.Identify(scan, Db(family));

            Assert.AreEqual(15, result.TopScore);
        }

        // ── Multiple families — top-pick ───────────────────────────────────────

        [TestMethod]
        public void Identify_MultipleMatches_ReturnsBestScore()
        {
            var stop = MakeFamily("stop",
                exts:  new[] { ".djvu" },
                notes: new[] { "_readme.txt" });
            var phobos = MakeFamily("phobos",
                exts: new[] { ".phobos" });

            var scan = ScanWith(ext: ".djvu", notePath: @"C:\test\_readme.txt");
            var result = Identifier.Identify(scan, Db(stop, phobos));

            Assert.AreEqual("stop", result.TopFamily.Id);
            Assert.AreEqual(80, result.TopScore);
            Assert.IsTrue(result.AllScores.Count >= 1);
        }

        [TestMethod]
        public void Identify_AllScores_SortedDescending()
        {
            var high = MakeFamily("high", exts: new[] { ".djvu" }, notes: new[] { "_readme.txt" });
            var low  = MakeFamily("low",  exts: new[] { ".djvu" });

            var scan = ScanWith(ext: ".djvu", notePath: @"C:\test\_readme.txt");
            var result = Identifier.Identify(scan, Db(low, high));

            Assert.AreEqual("high", result.AllScores[0].Family.Id);
            Assert.IsTrue(result.AllScores[0].Score >= result.AllScores[1].Score);
        }

        [TestMethod]
        public void Identify_OtherMatchCount_ExcludesTop()
        {
            var a = MakeFamily("a", exts: new[] { ".enc" }, notes: new[] { "readme.txt" });
            var b = MakeFamily("b", exts: new[] { ".enc" });
            var c = MakeFamily("c", exts: new[] { ".enc" });

            var scan = ScanWith(ext: ".enc", notePath: @"C:\test\readme.txt");
            var result = Identifier.Identify(scan, Db(a, b, c));

            // b and c each score 50, so OtherMatchCount should be 2
            Assert.AreEqual(2, result.OtherMatchCount);
        }

        // ── IsConfirmed threshold ──────────────────────────────────────────────

        [TestMethod]
        public void Identify_Score74_IsNotConfirmed()
        {
            // ext(50) + keyword(20) = 70, note sans note file → 70 < 75
            var family = MakeFamily("x",
                exts:     new[] { ".enc" },
                keywords: new[] { "evil@ransom.cc" });
            var scan = ScanWith(ext: ".enc",
                notePath:    @"C:\test\readme.txt",   // wrong filename → no +30
                noteContent: "evil@ransom.cc");

            var result = Identifier.Identify(scan, Db(family));
            Assert.IsFalse(result.IsConfirmed);
        }

        [TestMethod]
        public void Identify_Score80_IsConfirmed()
        {
            var family = MakeFamily("x",
                exts:  new[] { ".enc" },
                notes: new[] { "HOW_DECRYPT.txt" });
            var scan = ScanWith(ext: ".enc", notePath: @"C:\test\HOW_DECRYPT.txt");

            var result = Identifier.Identify(scan, Db(family));
            Assert.IsTrue(result.IsConfirmed);
        }
    }
}
