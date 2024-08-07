using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whirlybird.Components;

namespace Whirlybird.PlayerSettings
{
    public class Player
    {
        public static Settings Preferences { get; set; }
        public int Coins { get; private set; }
        public List<BirdSkin> OwnedSkins { get; private set; }

        public BirdSkin CurrentSkin 
        {
            get
            {
                return currentSkin;
            }
            set
            {
                currentSkin = value;
            } 
        }

        private string filename;
        private BirdSkin currentSkin;
        public Player()
        {
            //string projectDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;
            //filename = projectDirectory + "\\PlayerSettings\\" + filename;
            filename = "savefile.bin";

            Preferences = new Settings();

            OwnedSkins = new List<BirdSkin>();
            Coins = 0;
            OwnedSkins.Add(BirdSkin.common);
            
            
            if (File.Exists(filename))
            {
                LoadSavefile();
            }
            else
            {
                SaveProgress();
            }
            Coins++;


        }

        public void AddCoin()
        {
            Coins++;
        }
        public void SpendMoney(int money)
        {
            Coins -= money;
        }
        public void LoadSavefile()
        {
            if (!File.Exists(filename))
            {
                File.Create(filename).Close();
            }
            using (BinaryReader binReader = new BinaryReader(File.Open(filename, FileMode.Open)))
            {
                Preferences.HardMode = binReader.ReadBoolean();
                Preferences.LanguageCode = binReader.ReadString();

                Preferences.MainColor = new Color(binReader.ReadUInt32());
                Preferences.FontColor = new Color(binReader.ReadUInt32());
                Preferences.SecondFontColor = new Color(binReader.ReadUInt32());
                Preferences.CursorColor = new Color(binReader.ReadUInt32());

                Coins = binReader.ReadInt32();
                currentSkin = (BirdSkin)binReader.ReadInt32();
                while (binReader.BaseStream.Position != binReader.BaseStream.Length)
                {
                    BirdSkin skin = (BirdSkin)binReader.ReadInt32();
                    OwnedSkins.Add(skin);
                }
            }
        }
        public void SaveProgress()
        {
            using (BinaryWriter binWriter = new BinaryWriter(File.Create(filename)))
            {
                binWriter.Write(Preferences.HardMode);

                binWriter.Write(Preferences.LanguageCode);

                binWriter.Write(Preferences.MainColor.PackedValue);
                binWriter.Write(Preferences.FontColor.PackedValue);
                binWriter.Write(Preferences.SecondFontColor.PackedValue);
                binWriter.Write(Preferences.CursorColor.PackedValue);

                binWriter.Write(Coins);
                binWriter.Write((int)currentSkin);
                foreach (var skin in OwnedSkins)
                {
                    binWriter.Write((int)skin);
                }
            }
        }
    }
}
