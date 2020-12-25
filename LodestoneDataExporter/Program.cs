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

        public static void Main(string[] args)
        {
            var dataPath = args.Length > 0 ? args[0] : "C:/Program Files (x86)/SquareEnix/FINAL FANTASY XIV - A Realm Reborn/game/sqpack";
            var cyalume = new Cyalume(dataPath);

            ExportItemTable(cyalume);
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

        private static void Serialize<T>(string path, T obj) where T : class
        {
            var maxBytesNeeded = FlatBufferSerializer.Default.GetMaxSize(obj);
            var buffer = new byte[maxBytesNeeded];
            var bytesWritten = FlatBufferSerializer.Default.Serialize(obj, buffer);
            var bytesToWrite = buffer[..bytesWritten];
            File.WriteAllBytes(path, bytesToWrite);
            FlatBufferSerializer.Default.Parse<ItemTable>(bytesToWrite);
        }
    }
}
