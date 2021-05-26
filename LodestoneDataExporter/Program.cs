using FFXIV;
using FlatSharp;
using Lumina.Data;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Cyalume = Lumina.GameData;

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
                Task.Run(() => ExportAchievementTable(cyalume)),
                Task.Run(() => ExportClassJobTable(cyalume)),
                Task.Run(() => ExportGuardianDeityTable(cyalume)),
                Task.Run(() => ExportGrandCompanyTable(cyalume)),
                Task.Run(() => ExportMinionTable(cyalume)),
                Task.Run(() => ExportMountTable(cyalume)),
                Task.Run(() => ExportRaceTable(cyalume)),
                Task.Run(() => ExportReputationTable(cyalume)),
                Task.Run(() => ExportTitleTable(cyalume)),
                Task.Run(() => ExportTownTable(cyalume)),
                Task.Run(() => ExportTribeTable(cyalume))
            );

            ExportItemTable(cyalume); // Lumping this with the others causes stream depletions somehow.
        }

        private static void ExportAchievementTable(Cyalume cyalume)
        {
            var itemTable = new AchievementTable { Achievements = new List<Achievement>() };
            var languages = new[] { Language.English, Language.Japanese, Language.German, Language.French };
            foreach (var lang in languages)
            {
                var achievementSheet = cyalume.GetExcelSheet<Lumina.Excel.GeneratedSheets.Achievement>(lang);
                Parallel.ForEach(achievementSheet, new ParallelOptions { MaxDegreeOfParallelism = 4 }, achievement =>
                {
                    Achievement curAchievement;
                    lock (itemTable.Achievements)
                    {
                        curAchievement = itemTable.Achievements.FirstOrDefault(i => i.Id == achievement.RowId);
                        if (curAchievement == null)
                        {
                            curAchievement = new Achievement { Id = achievement.RowId };
                            itemTable.Achievements.Add(curAchievement);
                        }
                    }

                    switch (lang)
                    {
                        case Language.English:
                            curAchievement.NameEn = achievement.Name;
                            break;
                        case Language.Japanese:
                            curAchievement.NameJa = achievement.Name;
                            break;
                        case Language.German:
                            curAchievement.NameDe = achievement.Name;
                            break;
                        case Language.French:
                            curAchievement.NameFr = achievement.Name;
                            break;
                    }
                });
            }

            Serialize(Path.Join(OutputDir, "achievement_table.bin"), itemTable);
        }

        private static void ExportClassJobTable(Cyalume cyalume)
        {
            var classJobTable = new ClassJobTable { ClassJobs = new List<ClassJob>() };
            var languages = new[] { Language.English, Language.Japanese, Language.German, Language.French };
            foreach (var lang in languages)
            {
                var classJobSheet = cyalume.GetExcelSheet<Lumina.Excel.GeneratedSheets.ClassJob>(lang);
                Parallel.ForEach(classJobSheet, new ParallelOptions { MaxDegreeOfParallelism = 4 }, classJob =>
                {
                    ClassJob curClassJob;
                    lock (classJobTable.ClassJobs)
                    {
                        curClassJob = classJobTable.ClassJobs.FirstOrDefault(cj => cj.Id == classJob.RowId);
                        if (curClassJob == null)
                        {
                            curClassJob = new ClassJob { Id = classJob.RowId };
                            classJobTable.ClassJobs.Add(curClassJob);
                        }
                    }

                    switch (lang)
                    {
                        case Language.English:
                            curClassJob.NameEn = classJob.Name;
                            break;
                        case Language.Japanese:
                            curClassJob.NameJa = classJob.Name;
                            break;
                        case Language.German:
                            curClassJob.NameDe = classJob.Name;
                            break;
                        case Language.French:
                            curClassJob.NameFr = classJob.Name;
                            break;
                    }
                });
            }

            Serialize(Path.Join(OutputDir, "classjob_table.bin"), classJobTable);
        }

        private static void ExportGuardianDeityTable(Cyalume cyalume)
        {
            var deityTable = new DeityTable { Deities = new List<Deity>() };
            var languages = new[] { Language.English, Language.Japanese, Language.German, Language.French };
            foreach (var lang in languages)
            {
                var deitySheet = cyalume.GetExcelSheet<Lumina.Excel.GeneratedSheets.GuardianDeity>(lang);
                Parallel.ForEach(deitySheet, new ParallelOptions { MaxDegreeOfParallelism = 4 }, deity =>
                {
                    Deity curDeity;
                    lock (deityTable.Deities)
                    {
                        curDeity = deityTable.Deities.FirstOrDefault(d => d.Id == deity.RowId);
                        if (curDeity == null)
                        {
                            curDeity = new Deity { Id = deity.RowId };
                            deityTable.Deities.Add(curDeity);
                        }
                    }

                    switch (lang)
                    {
                        case Language.English:
                            curDeity.NameEn = deity.Name;
                            break;
                        case Language.Japanese:
                            curDeity.NameJa = deity.Name;
                            break;
                        case Language.German:
                            curDeity.NameDe = deity.Name;
                            break;
                        case Language.French:
                            curDeity.NameFr = deity.Name;
                            break;
                    }
                });
            }

            Serialize(Path.Join(OutputDir, "deity_table.bin"), deityTable);
        }

        private static void ExportGrandCompanyTable(Cyalume cyalume)
        {
            var gcTable = new GrandCompanyTable { GrandCompanies = new List<GrandCompany>() };
            var languages = new[] { Language.English, Language.Japanese, Language.German, Language.French };
            foreach (var lang in languages)
            {
                var gcSheet = cyalume.GetExcelSheet<Lumina.Excel.GeneratedSheets.GrandCompany>(lang);
                Parallel.ForEach(gcSheet, new ParallelOptions { MaxDegreeOfParallelism = 4 }, gc =>
                {
                    GrandCompany curGc;
                    lock (gcTable.GrandCompanies)
                    {
                        curGc = gcTable.GrandCompanies.FirstOrDefault(c => c.Id == gc.RowId);
                        if (curGc == null)
                        {
                            curGc = new GrandCompany { Id = gc.RowId };
                            gcTable.GrandCompanies.Add(curGc);
                        }
                    }

                    switch (lang)
                    {
                        case Language.English:
                            curGc.NameEn = gc.Name;
                            break;
                        case Language.Japanese:
                            curGc.NameJa = gc.Name;
                            break;
                        case Language.German:
                            curGc.NameDe = gc.Name;
                            break;
                        case Language.French:
                            curGc.NameFr = gc.Name;
                            break;
                    }
                });
            }

            Serialize(Path.Join(OutputDir, "gc_table.bin"), gcTable);
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

        private static void ExportRaceTable(Cyalume cyalume)
        {
            var raceTable = new RaceTable { Races = new List<Race>() };
            var languages = new[] { Language.English, Language.Japanese, Language.German, Language.French };
            foreach (var lang in languages)
            {
                var raceSheet = cyalume.GetExcelSheet<Lumina.Excel.GeneratedSheets.Race>(lang);
                Parallel.ForEach(raceSheet, new ParallelOptions { MaxDegreeOfParallelism = 4 }, race =>
                {
                    Race curRace;
                    lock (raceTable.Races)
                    {
                        curRace = raceTable.Races.FirstOrDefault(r => r.Id == race.RowId);
                        if (curRace == null)
                        {
                            curRace = new Race { Id = race.RowId };
                            raceTable.Races.Add(curRace);
                        }
                    }

                    switch (lang)
                    {
                        case Language.English:
                            curRace.NameMasculineEn = race.Masculine;
                            curRace.NameFeminineEn = race.Feminine;
                            break;
                        case Language.Japanese:
                            curRace.NameMasculineJa = race.Masculine;
                            curRace.NameFeminineJa = race.Feminine;
                            break;
                        case Language.German:
                            curRace.NameMasculineDe = race.Masculine;
                            curRace.NameFeminineDe = race.Feminine;
                            break;
                        case Language.French:
                            curRace.NameMasculineFr = race.Masculine;
                            curRace.NameFeminineFr = race.Feminine;
                            break;
                    }
                });
            }

            Serialize(Path.Join(OutputDir, "race_table.bin"), raceTable);
        }

        private static void ExportReputationTable(Cyalume cyalume)
        {
            var repTable = new ReputationTable { Reputations = new List<Reputation>() };
            var languages = new[] { Language.English, Language.Japanese, Language.German, Language.French };
            foreach (var lang in languages)
            {
                var repSheet = cyalume.GetExcelSheet<Lumina.Excel.GeneratedSheets.BeastReputationRank>(lang);
                Parallel.ForEach(repSheet, new ParallelOptions { MaxDegreeOfParallelism = 4 }, rep =>
                {
                    Reputation curRep;
                    lock (repTable.Reputations)
                    {
                        curRep = repTable.Reputations.FirstOrDefault(r => r.Id == rep.RowId);
                        if (curRep == null)
                        {
                            curRep = new Reputation { Id = rep.RowId };
                            repTable.Reputations.Add(curRep);
                        }
                    }

                    switch (lang)
                    {
                        case Language.English:
                            curRep.NameEn = rep.Name;
                            break;
                        case Language.Japanese:
                            curRep.NameJa = rep.Name;
                            break;
                        case Language.German:
                            curRep.NameDe = rep.Name;
                            break;
                        case Language.French:
                            curRep.NameFr = rep.Name;
                            break;
                    }
                });
            }

            Serialize(Path.Join(OutputDir, "reputation_table.bin"), repTable);
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

        private static void ExportTownTable(Cyalume cyalume)
        {
            var townTable = new TownTable { Towns = new List<Town>() };
            var languages = new[] { Language.English, Language.Japanese, Language.German, Language.French };
            foreach (var lang in languages)
            {
                var townSheet = cyalume.GetExcelSheet<Lumina.Excel.GeneratedSheets.Town>(lang);
                Parallel.ForEach(townSheet, new ParallelOptions { MaxDegreeOfParallelism = 4 }, town =>
                {
                    Town curTown;
                    lock (townTable.Towns)
                    {
                        curTown = townTable.Towns.FirstOrDefault(t => t.Id == town.RowId);
                        if (curTown == null)
                        {
                            curTown = new Town { Id = town.RowId };
                            townTable.Towns.Add(curTown);
                        }
                    }

                    switch (lang)
                    {
                        case Language.English:
                            curTown.NameEn = town.Name;
                            break;
                        case Language.Japanese:
                            curTown.NameJa = town.Name;
                            break;
                        case Language.German:
                            curTown.NameDe = town.Name;
                            break;
                        case Language.French:
                            curTown.NameFr = town.Name;
                            break;
                    }
                });
            }

            Serialize(Path.Join(OutputDir, "town_table.bin"), townTable);
        }

        private static void ExportTribeTable(Cyalume cyalume)
        {
            var tribeTable = new TribeTable { Tribes = new List<Tribe>() };
            var languages = new[] { Language.English, Language.Japanese, Language.German, Language.French };
            foreach (var lang in languages)
            {
                var tribeSheet = cyalume.GetExcelSheet<Lumina.Excel.GeneratedSheets.Tribe>(lang);
                Parallel.ForEach(tribeSheet, new ParallelOptions { MaxDegreeOfParallelism = 4 }, tribe =>
                {
                    Tribe curTribe;
                    lock (tribeTable.Tribes)
                    {
                        curTribe = tribeTable.Tribes.FirstOrDefault(t => t.Id == tribe.RowId);
                        if (curTribe == null)
                        {
                            curTribe = new Tribe { Id = tribe.RowId };
                            tribeTable.Tribes.Add(curTribe);
                        }
                    }

                    switch (lang)
                    {
                        case Language.English:
                            curTribe.NameMasculineEn = tribe.Masculine;
                            curTribe.NameFeminineEn = tribe.Feminine;
                            break;
                        case Language.Japanese:
                            curTribe.NameMasculineJa = tribe.Masculine;
                            curTribe.NameFeminineJa = tribe.Feminine;
                            break;
                        case Language.German:
                            curTribe.NameMasculineDe = tribe.Masculine;
                            curTribe.NameFeminineDe = tribe.Feminine;
                            break;
                        case Language.French:
                            curTribe.NameMasculineFr = tribe.Masculine;
                            curTribe.NameFeminineFr = tribe.Feminine;
                            break;
                    }
                });
            }

            Serialize(Path.Join(OutputDir, "tribe_table.bin"), tribeTable);
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
