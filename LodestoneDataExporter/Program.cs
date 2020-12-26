using FFXIV;
using FlatSharp;
using Lumina.Data;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Cyalume = Lumina.Lumina;

namespace LodestoneDataExporter
{
    public static class Program
    {
        private const string OutputDir = "../../../../pack";

        public static async Task Main(string[] args)
        {
            var dataPath = args.Length > 0 ? args[0] : "C:/Program Files (x86)/SquareEnix/FINAL FANTASY XIV - A Realm Reborn/game/sqpack";
            var cyalume = new Cyalume(dataPath);

            await Task.WhenAll(
                Task.Run(() => ExportItemTable(cyalume)),
                Task.Run(() => ExportMinionTable(cyalume)),
                Task.Run(() => ExportMountTable(cyalume)),
                Task.Run(() => ExportTitleTable(cyalume))
            );
        }

        private static void ExportItemTable(Cyalume cyalume)
        {
            var itemTable = new ItemTable {Items = new List<Item>()};
            var languages = new[] { Language.English, Language.Japanese, Language.German, Language.French };
            foreach (var lang in languages)
            {
                var itemSheet = cyalume.GetExcelSheet<Lumina.Excel.GeneratedSheets.Item>(lang);
                Parallel.ForEach(itemSheet, new ParallelOptions{MaxDegreeOfParallelism = 4}, item =>
                {
                    Item curItem;
                    lock (itemTable.Items)
                    {
                        curItem = itemTable.Items.FirstOrDefault(i => i.Id == item.RowId);
                        if (curItem == null)
                        {
                            curItem = new Item {Id = item.RowId};
                            itemTable.Items.Add(curItem);
                        }
                    }

                    switch (lang)
                    {
                        case Language.English:
                            curItem.NameEn = item.Name;
                            break;
                        case Language.Japanese:
                            curItem.NameJa = item.Name;
                            break;
                        case Language.German:
                            curItem.NameDe = item.Name;
                            break;
                        case Language.French:
                            curItem.NameFr = item.Name;
                            break;
                    }
                });
            }

            Serialize(Path.Join(OutputDir, "item_table.bin"), itemTable);
        }

        private static void ExportMinionTable(Cyalume cyalume)
        {
            var minionTable = new MinionTable { Minions = new List<Minion>() };
            var languages = new[] { Language.English, Language.Japanese, Language.German, Language.French };
            foreach (var lang in languages)
            {
                var minionSheet = cyalume.GetExcelSheet<Lumina.Excel.GeneratedSheets.Companion>(lang);
                Parallel.ForEach(minionSheet, new ParallelOptions { MaxDegreeOfParallelism = 4 }, minion =>
                {
                    Minion curMinion;
                    lock (minionTable.Minions)
                    {
                        curMinion = minionTable.Minions.FirstOrDefault(m => m.Id == minion.RowId);
                        if (curMinion == null)
                        {
                            curMinion = new Minion { Id = minion.RowId };
                            minionTable.Minions.Add(curMinion);
                        }
                    }

                    switch (lang)
                    {
                        case Language.English:
                            curMinion.NameEn = minion.Singular;
                            break;
                        case Language.Japanese:
                            curMinion.NameJa = minion.Singular;
                            break;
                        case Language.German:
                            curMinion.NameDe = minion.Singular;
                            break;
                        case Language.French:
                            curMinion.NameFr = minion.Singular;
                            break;
                    }
                });
            }

            Serialize(Path.Join(OutputDir, "minion_table.bin"), minionTable);
        }

        private static void ExportMountTable(Cyalume cyalume)
        {
            var mountTable = new MountTable { Mounts = new List<Mount>() };
            var languages = new[] { Language.English, Language.Japanese, Language.German, Language.French };
            foreach (var lang in languages)
            {
                var mountSheet = cyalume.GetExcelSheet<Lumina.Excel.GeneratedSheets.Mount>(lang);
                Parallel.ForEach(mountSheet, new ParallelOptions { MaxDegreeOfParallelism = 4 }, mount =>
                {
                    Mount curMount;
                    lock (mountTable.Mounts)
                    {
                        curMount = mountTable.Mounts.FirstOrDefault(m => m.Id == mount.RowId);
                        if (curMount == null)
                        {
                            curMount = new Mount { Id = mount.RowId };
                            mountTable.Mounts.Add(curMount);
                        }
                    }

                    switch (lang)
                    {
                        case Language.English:
                            curMount.NameEn = mount.Singular;
                            break;
                        case Language.Japanese:
                            curMount.NameJa = mount.Singular;
                            break;
                        case Language.German:
                            curMount.NameDe = mount.Singular;
                            break;
                        case Language.French:
                            curMount.NameFr = mount.Singular;
                            break;
                    }
                });
            }

            Serialize(Path.Join(OutputDir, "mount_table.bin"), mountTable);
        }

        private static void ExportTitleTable(Cyalume cyalume)
        {
            var titleTable = new TitleTable { Titles = new List<Title>() };
            var languages = new[] { Language.English, Language.Japanese, Language.German, Language.French };
            foreach (var lang in languages)
            {
                var titleSheet = cyalume.GetExcelSheet<Lumina.Excel.GeneratedSheets.Title>(lang);
                Parallel.ForEach(titleSheet, new ParallelOptions { MaxDegreeOfParallelism = 4 }, title =>
                {
                    Title curTitle;
                    lock (titleTable.Titles)
                    {
                        curTitle = titleTable.Titles.FirstOrDefault(t => t.Id == title.RowId);
                        if (curTitle == null)
                        {
                            curTitle = new Title { Id = title.RowId, IsPrefix = title.IsPrefix};
                            titleTable.Titles.Add(curTitle);
                        }
                    }

                    switch (lang)
                    {
                        case Language.English:
                            curTitle.NameMasculineEn = title.Masculine;
                            curTitle.NameFeminineEn = title.Feminine;
                            break;
                        case Language.Japanese:
                            curTitle.NameMasculineJa = title.Masculine;
                            curTitle.NameFeminineJa = title.Feminine;
                            break;
                        case Language.German:
                            curTitle.NameMasculineDe = title.Masculine;
                            curTitle.NameFeminineDe = title.Feminine;
                            break;
                        case Language.French:
                            curTitle.NameMasculineFr = title.Masculine;
                            curTitle.NameFeminineFr = title.Feminine;
                            break;
                    }
                });
            }

            Serialize(Path.Join(OutputDir, "title_table.bin"), titleTable);
        }

        private static void Serialize<T>(string path, T obj) where T : class
        {
            var maxBytesNeeded = FlatBufferSerializer.Default.GetMaxSize(obj);
            var buffer = new byte[maxBytesNeeded];
            var bytesWritten = FlatBufferSerializer.Default.Serialize(obj, buffer);
            var bytesToWrite = buffer[..bytesWritten];
            File.WriteAllBytes(path, bytesToWrite);
        }
    }
}
