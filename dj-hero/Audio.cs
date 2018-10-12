﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dj_hero
{
    public static class Audio
    {
        private static WMPLib.WindowsMediaPlayer Player = new WMPLib.WindowsMediaPlayer();
        private static WMPLib.WindowsMediaPlayer Player2 = new WMPLib.WindowsMediaPlayer();
        public static readonly string libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/DJH_MusicFiles";


        public static void StartSong(Song song, bool isLoop)
        {
            if(isLoop == true)
                Player.settings.setMode("loop", true);
            else
                Player.settings.setMode("loop", false);
            if (song == Song.noisesong)
                Player2.URL = song.GetPath();
            else
                Player.URL = song.GetPath();
          
        }
        public static void Noise()
        {
            Player.settings.volume = 50;
            StartSong(Song.noisesong, true);
        }
        public static void StopSong()
        {
            Player.controls.stop();
        }
    }
}