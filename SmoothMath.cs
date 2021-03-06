﻿using RealTimePPDisplayer.Displayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimePPDisplayer
{
    public static class SmoothMath
    {
        //From: http://devblog.aliasinggames.com/inertialdamp-unity-smoothdamp-alternative/
        public static double SmoothDamp(double previousValue, double targetValue, ref double speed, double smoothTime, double dt)
        {
            double T1 = 0.36f * smoothTime;
            double T2 = 0.64f * smoothTime;
            double x = previousValue - targetValue;
            double newSpeed = speed + dt * (-1f / (T1 * T2) * x - (T1 + T2) / (T1 * T2) * speed);
            double newValue = x + dt * speed;
            speed = newSpeed;
            return targetValue + newValue;
        }

        public static PPTuple SmoothDampPPTuple(PPTuple previousValue, PPTuple targetValue, ref PPTuple speed, double dt)
        {
            double smoothTime = Setting.SmoothTime * 0.001;

            PPTuple result;
            result.RealTimePP = SmoothDamp(previousValue.RealTimePP, targetValue.RealTimePP, ref speed.RealTimePP, smoothTime, dt);
            result.RealTimeAimPP = SmoothDamp(previousValue.RealTimeAimPP, targetValue.RealTimeAimPP, ref speed.RealTimeAimPP, smoothTime, dt);
            result.RealTimeSpeedPP = SmoothDamp(previousValue.RealTimeSpeedPP, targetValue.RealTimeSpeedPP, ref speed.RealTimeSpeedPP, smoothTime, dt);
            result.RealTimeAccuracyPP = SmoothDamp(previousValue.RealTimeAccuracyPP, targetValue.RealTimeAccuracyPP, ref speed.RealTimeAccuracyPP, smoothTime, dt);

            result.FullComboPP = SmoothDamp(previousValue.FullComboPP, targetValue.FullComboPP, ref speed.FullComboPP, smoothTime, dt);
            result.FullComboAimPP = SmoothDamp(previousValue.FullComboAimPP, targetValue.FullComboAimPP, ref speed.FullComboAimPP, smoothTime, dt);
            result.FullComboSpeedPP = SmoothDamp(previousValue.FullComboSpeedPP, targetValue.FullComboSpeedPP, ref speed.FullComboSpeedPP, smoothTime, dt);
            result.FullComboAccuracyPP = SmoothDamp(previousValue.FullComboAccuracyPP, targetValue.FullComboAccuracyPP, ref speed.FullComboAccuracyPP, smoothTime, dt);

            result.MaxPP = targetValue.MaxPP;
            result.MaxAimPP = targetValue.MaxAimPP;
            result.MaxSpeedPP = targetValue.MaxSpeedPP;
            result.MaxAccuracyPP = targetValue.MaxAccuracyPP;

            return result;
        }
    }
}
