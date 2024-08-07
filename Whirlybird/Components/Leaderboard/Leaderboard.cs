using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whirlybird.Debugger;

namespace Whirlybird.Components.Leaderboard
{
    public struct LeaderboardRecord 
    {
        public string Name { get; set; }
        public float Score { get; set; }
        public LeaderboardRecord(string name, float result) 
        {
            Name = name;
            Score = result;
        }
    }
    public class Leaderboard
    {
        public List<LeaderboardRecord> Records { get; private set; }
        public float MinValue { get; private set; }
        
        private LeaderboardRecord minRecord;
        private string filename;
        public Leaderboard(string filename)
        {
            this.filename = filename;
            
            Records = new List<LeaderboardRecord>();
            MinValue = 0;
        }

        public List<LeaderboardRecord> LoadData()
        {
            if (!File.Exists(filename))
            {
                File.Create(filename).Close();
            }
            using (BinaryReader binReader = new BinaryReader(File.Open(filename, FileMode.Open)))
            {
                while (binReader.BaseStream.Position != binReader.BaseStream.Length)
                {
                    int nameLength = binReader.ReadInt32();
                    char[] nameChars = binReader.ReadChars(nameLength);
                    string name = new string(nameChars);
                    float score = binReader.ReadSingle();
                    Records.Add(new LeaderboardRecord(name, score));
                }
            }
            return Records;
        }

        public void AddRecord(string name, float score)
        {
            Records.Add(new LeaderboardRecord(name, score));
            if (Records.Count > 100)
            {
                Records.Remove(minRecord);
                MinValue = Records.Select(d => d.Score).Min();
                minRecord = Records.Where(d => d.Score == MinValue).First();
            }

            using (BinaryWriter binWriter = new BinaryWriter(File.Open(filename, FileMode.Create)))
            {
                foreach (var record in Records)
                {
                    binWriter.Write(record.Name.Length);
                    binWriter.Write(record.Name.ToCharArray());
                    binWriter.Write(record.Score);
                }
            }
        }

        public int GetPosition(float score)
        {
            if (Records.Count == 0)
            {
                return 1;
            }
            return Records.Where(r => r.Score > score).Count() + 1;
        }

        public float GetHighestScore()
        {
            if (Records.Count == 0)
            {
                return 0;
            }
            return Records.Select(r => r.Score).Max();
        }
    }
}
