﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dj_hero
{
    class SongSlectionView
    {

        public void Display()
        {
            foreach(Song s in Song.songs)
            {
                Console.WriteLine(s.title);
            }
        }

    }
}
