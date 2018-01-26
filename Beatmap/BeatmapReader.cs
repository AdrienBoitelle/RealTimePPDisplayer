using OsuRTDataProvider.Mods;
using RealTimePPDisplayer.PP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RealTimePPDisplayer.Beatmap
{
    class BeatmapReader
    {
        struct BeatmapHeader
        {
            public int Offset;
            public int Length;
        }

        private BeatmapHeader m_beatmap_header;

        private byte[] m_beatmap_raw;
        public byte[] BeatmapRaw => m_beatmap_raw;

        private List<BeatmapObject> m_object_list = new List<BeatmapObject>();

        private Oppai.pp_params m_cache=new Oppai.pp_params();
        public int FullCombo => m_cache.max_combo;

        public BeatmapReader(string file)
        {
            m_beatmap_header.Offset = 0;
            m_beatmap_header.Length = 0;

            using (var fs = File.OpenRead(file))
            {
                using (var reader = new StreamReader(fs))
                {
                    m_beatmap_raw=Encoding.UTF8.GetBytes(reader.ReadToEnd());
                }
            }
            Parser();
        }

        void ReadLine(out int offset,out int length,ref int position)
        {
            int count = 0;
            while((position+count)<m_beatmap_raw.Length)
            {
                if (m_beatmap_raw[position + count] == '\n')
                {
                    count++;
                    break;
                };
                count++;
            }
            length = count;
            offset = position;
            position = offset + count;
        }

        public void Parser()
        {
            int position = 0;
            int len=Array.LastIndexOf(m_beatmap_raw,(byte)']');
            m_beatmap_header.Length=(m_beatmap_raw[len + 1] == '\n') ? len+2 : len+3;

            position = m_beatmap_header.Length;

            while(position<m_beatmap_raw.Length)
            {
                ReadLine(out int offset, out int length,ref position);
                string line = Encoding.UTF8.GetString(m_beatmap_raw, offset, length);
                var obj = new BeatmapObject(line, offset, length);
                m_object_list.Add(obj);
            }
        }

        private int GetPosition(int end_time)
        {
            int pos = m_beatmap_header.Length;
            foreach(var obj in m_object_list)
            {
                if (obj.Time > end_time) break;
                pos+=(obj.Length);
            }

            return pos;
        }

        private ModsInfo _max_mods = ModsInfo.Empty;
        private double _max_result;


        [StructLayout(LayoutKind.Sequential)]
        public struct ppmania_params
        {
            public UInt32 mods;
            public int score;
            public double acc;
        }

        public void get_ppmania(byte[] data, UInt32 data_size, ref ppmania_params args, ref double result)
        {
            result = 1337.0;
        }

        public double GetMaxPPMania(ModsInfo mods)
        {
            bool need_update = false;
            need_update = need_update || mods != _max_mods;

            if (need_update)
            {
                _max_mods = mods;

                ppmania_params args;
                args.mods = (uint)mods.Mod;
                args.score = 1000000;
                args.acc = 100.0;
                
                get_ppmania(m_beatmap_raw, (uint)m_beatmap_raw.Length, ref args, ref _max_result);
            }
            return _max_result;
        }
       

        public double GetIfFCPPMania(ModsInfo mods, int curr_score, double curr_acc)
        {
            bool need_update = false;
            need_update = need_update || mods != _max_mods;

            if (need_update)
            {
                _max_mods = mods;

                ppmania_params args;
                args.mods = (uint)mods.Mod;
                args.score = curr_score; // Provide current score
                args.acc = curr_acc;

                get_ppmania(m_beatmap_raw, (uint)m_beatmap_raw.Length, ref args, ref _max_result);
            }
            return _max_result;
        }


        public double GetRealTimePPMania(ModsInfo mods, int offset, int curr_score, double curr_acc)
        {
            bool need_update = false;
            need_update = need_update || mods != _max_mods;

            if (need_update)
            {
                _max_mods = mods;

                ppmania_params args;
                args.mods = (uint)mods.Mod;
                args.score = curr_score; // Provide current score
                args.acc = curr_acc;

                // m_beatmap_raw should be limited to the given offset
                get_ppmania(m_beatmap_raw, (uint)m_beatmap_raw.Length, ref args, ref _max_result);
            }
            return _max_result;
        }

        public void Clear()
        {
            _max_mods = ModsInfo.Empty;
            _max_result = 0.0;
        }
    }
}
