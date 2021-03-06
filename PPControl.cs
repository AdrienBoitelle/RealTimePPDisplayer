﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OsuRTDataProvider.Listen;
using RealTimePPDisplayer.Displayer.View;
using OsuRTDataProvider.Mods;
using RealTimePPDisplayer.Beatmap;
using System.Threading;
using static OsuRTDataProvider.Listen.OsuListenerManager;
using System.IO;
using System.Windows.Media;
using System.Windows;
using RealTimePPDisplayer.Displayer;

namespace RealTimePPDisplayer
{
    class PPControl
    {
        private OsuListenerManager m_listener_manager;

        private BeatmapReader m_beatmap_reader;
        private ModsInfo m_cur_mods = ModsInfo.Empty;

        private OsuStatus m_status;

        private int m_combo = 0;
        private int m_max_combo = 0;

        private int m_n300 = 0;
        private int m_n100 = 0;
        private int m_n50 = 0;
        private int m_nmiss = 0;
        private int m_time = 0;

        private Dictionary<string,DisplayerBase> m_displayers = new Dictionary<string,DisplayerBase>();

        public PPControl(OsuListenerManager mamger,int? id)
        {
            m_listener_manager = mamger;

            m_listener_manager.OnModsChanged += (mods) => m_cur_mods = mods;
            m_listener_manager.On300HitChanged += c => m_n300 = c;
            m_listener_manager.On100HitChanged += c => m_n100 = c;
            m_listener_manager.On50HitChanged += c => m_n50 = c;
            m_listener_manager.OnMissHitChanged += c => m_nmiss = c;
            m_listener_manager.OnStatusChanged += (last, cur) =>
            {
                m_status = cur;
                if (cur == OsuStatus.Listening || cur == OsuStatus.Editing)//Clear Output
                {
                    m_combo = 0;
                    m_max_combo = 0;
                    m_n100 = 0;
                    m_n50 = 0;
                    m_nmiss = 0;
                    foreach (var p in m_displayers)
                        p.Value.Clear();
                    m_beatmap_reader?.Clear();
                }
            };

            m_listener_manager.OnComboChanged += (combo) =>
            {
                if (m_status != OsuStatus.Playing) return;
                if(combo<=m_beatmap_reader?.FullCombo)
                {
                    m_combo = combo;
                    m_max_combo = Math.Max(m_max_combo, m_combo);
                }
            };

            m_listener_manager.OnBeatmapChanged += RTPPOnBeatmapChanged;
            m_listener_manager.OnPlayingTimeChanged += RTPPOnPlayingTimeChanged;
        }

        private void RTPPOnBeatmapChanged(OsuRTDataProvider.BeatmapInfo.Beatmap beatmap)
        {
            if (string.IsNullOrWhiteSpace(beatmap.Difficulty))
            {
                m_beatmap_reader = null;
                return;
            }

            string file = beatmap.FilenameFull;
            if (string.IsNullOrWhiteSpace(file))
            {
                Sync.Tools.IO.CurrentIO.WriteColor($"[RealTimePPDisplayer]No found .osu file(Set:{beatmap.BeatmapSetID} Beatmap:{beatmap.BeatmapID}])", ConsoleColor.Yellow);
                m_beatmap_reader = null;
                return;
            }

            if (Setting.DebugMode)
                Sync.Tools.IO.CurrentIO.WriteColor($"[RealTimePPDisplayer]File:{file}", ConsoleColor.Blue);
            m_beatmap_reader = new BeatmapReader(file);
        }
        private void RTPPOnPlayingTimeChanged(int time)
        {
            if (time < 0) return;
            if (m_beatmap_reader == null) return;
            if (m_status != OsuStatus.Playing) return;
            if (m_cur_mods == ModsInfo.Mods.Unknown) return;

            if (m_time > time)//Reset
            {
                m_combo = 0;
                m_max_combo = 0;
                m_n100 = 0;
                m_n50 = 0;
                m_nmiss = 0;
            }
            
            PPTuple pp_tuple = PPTuple.Empty;
            var result=m_beatmap_reader.GetMaxPPMania(m_cur_mods);
            pp_tuple.MaxPP = result;

            result = m_beatmap_reader.GetIfFCPPMania(m_cur_mods, 900000,99.0);
            pp_tuple.FullComboPP = result;

            result = m_beatmap_reader.GetRealTimePPMania(m_cur_mods, time, 50000,95.0);
            pp_tuple.RealTimePP = result;

            if (double.IsNaN(pp_tuple.RealTimePP)) pp_tuple.RealTimePP = 0.0;
            if (Math.Abs(pp_tuple.RealTimePP) > pp_tuple.MaxPP) pp_tuple.RealTimePP = 0.0;
            if (m_max_combo > m_beatmap_reader.FullCombo) m_max_combo = 0;

            foreach(var p in m_displayers)
            {
                p.Value.OnUpdatePP(pp_tuple);
                p.Value.Display();
            }

            m_time = time;
        }

        /// <summary>
        /// Add a displayer to update list
        /// </summary>
        /// <param name="name"></param>
        /// <param name="displayer"></param>
        public void AddDisplayer(string name,DisplayerBase displayer)
        {
            m_displayers[name]=displayer;
        }

        /// <summary>
        /// Remove a displayer from update list
        /// </summary>
        /// <param name="name"></param>
        public void RemoveDisplayer(string name)
        {
            if (m_displayers.ContainsKey(name))
            {
                m_displayers.Remove(name);
            }
        }
    }


    
}
