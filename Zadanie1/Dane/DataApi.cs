using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

[assembly: InternalsVisibleTo("DataTest")]

namespace Dane
{
    internal class DataApi : DataAbstractAPI
    {
        private ObservableCollection<Ball> _ballList;

        private bool _newSession = true;

        public override bool NewSession
        {
            get => _newSession;
            set => _newSession = value;
        }

        public override ObservableCollection<Ball> CreateBalls(int numberOfBalls)
        {
            _ballList = new ObservableCollection<Ball>();

            double x, y, r, ro, vx, vy, v;
            Random random = new Random();
            for (int i = 0; i < numberOfBalls; i++)
            {
                v = random.Next(1, 6);
                r = random.Next(20, 40);
                x = random.Next(0, (int)(1040 - r)) + random.NextDouble();
                y = random.Next(0, (int)(540 - r)) + random.NextDouble();
                vx = random.NextDouble() * v * (random.Next(0, 1) == 0 ? -1 : 1);
                vy = Math.Sqrt(v * v - vx * vx) * (random.Next(0, 1) == 0 ? -1 : 1);
                ro = random.Next(1, 5);
                _ballList.Add(new Ball(x, y, r, ro, v, vx, vy));
            }
            return _ballList;
        }
        public override void AppendObjectToJSONFile(string filename, string newJsonObject)
        {
            if (NewSession)
            {
                NewSession = false;
                if (File.Exists(filename))
                {
                    File.Delete(filename);
                }
                using (StreamWriter sw = new StreamWriter(filename, true))
                {
                    sw.WriteLine("[]");
                }
            }

            string content;
            using (StreamReader sr = File.OpenText(filename))
            {
                content = sr.ReadToEnd();
            }
            content = content.TrimEnd();
            content = content.Remove(content.Length - 1, 1);
            if (content.Length == 1)
            {
                content = String.Format("{0}\n{1}\n]\n", content.Trim(), newJsonObject);
            }
            else
            {
                content = String.Format("{0},\n{1}\n]\n", content.Trim(), newJsonObject);
            }

            using (StreamWriter sw = File.CreateText(filename))
            {
                sw.Write(content);
            }
        }

        public override CollisionInfo CollisionInfoObject(Vector initial_vel_1, Vector initial_vel_2, Ball ball_1, Ball ball_2)
        {
            return new CollisionInfo(initial_vel_1, initial_vel_2, ball_1, ball_2);
        }
    }
}