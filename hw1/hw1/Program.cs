﻿using System;

namespace Homework1
{
    public class Program
    {
        public static void Main(string[] args)  // 该程序用于Debug
        {
            IProgress progress = new Progress();
            for (int i = 1; i <= 20; i++)
            {
                try
                {
                    progress.Start(i*((i%2==0)?1:-1));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                if (i % 3 != 0)
                {
                    progress.Add(i-1);
                }
                else progress.Sub(i-1);

                if (i == 6 || i == 7)
                {
                    progress.Double();
                }
                Console.WriteLine(progress.GetProgress().ToString());
            }
        }
    }

    public interface IProgress
    {
        public int Num { get; } // Progress的序号，表明是第几个实例化的Progress
        public int RequiredProgress { get; } // Progress加载完成所需进度
        public int FinishedProgress { get; } // FinishedProgress指其中已完成的进度, FinishedProgress应当在[0,RequiredProgress]中

        /// <summary>
        /// 尝试加载下一次进度条，requiredProgress指再次加载进度条所需进度
        /// 如果之前进度条已经加载完成，则将进度清零开始下一次加载，返回true，但如果requiredProgress<0，应当报错
        /// 如果之前进度条尚未加载完成，返回false
        /// </summary>
        public bool Start(int requiredProgress); 
        
        public void Add(int addProgress); //增加addProgress的进度
        public void Sub(int subProgress); //减少subProgress的进度
        public void Double(); //进度翻倍

        /// <summary>
        ///  FinishedProgress指其中已完成的进度，RequiredProgress指当前Progress完成所需进度
        /// </summary>
        public (int FinishedProgress, int RequiredProgress) GetProgress();
    }

    public class Progress : IProgress
    {
        public static int count = 0;
        // 一个进度条
        // 只允许修改Progress类中的代码
        // 要求实现IProgress中的要求
        public int Num { get; set; } // Progress的序号，表明是第几个实例化的Progress
        public int RequiredProgress { get; set; } // Progress加载完成所需进度
        public int FinishedProgress { get; set; } // FinishedProgress指其中已完成的进度, FinishedProgress应当在[0,RequiredProgress]中

        public Progress()
        {
            Num = count++;
        }

        public bool Start(int requiredProgress)
        {
            if(FinishedProgress == RequiredProgress)
            {
                if (requiredProgress < 0)
                {
                    throw new Exception("RequiredProgress must be positive!");
                }
                FinishedProgress = 0;
                RequiredProgress = requiredProgress;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Add(int addProgress)
        {
            FinishedProgress = FinishedProgress + addProgress < RequiredProgress ? FinishedProgress + addProgress : RequiredProgress;
        }
        public void Sub(int subProgress)
        {
            FinishedProgress = FinishedProgress - subProgress >= 0 ? FinishedProgress - subProgress : 0;
        }
        public void Double()
        {
            FinishedProgress = FinishedProgress * 2 < RequiredProgress ? FinishedProgress * 2 : RequiredProgress;
        }

        /// <summary>
        ///  FinishedProgress指其中已完成的进度，RequiredProgress指当前Progress完成所需进度
        /// </summary>
        public (int FinishedProgress, int RequiredProgress) GetProgress()
        {
            return (FinishedProgress, RequiredProgress);
        }
    }

/*
 * 输出示例：
RequiredProgress must be positive. (Parameter 'Homework1.Progress')
(0, 0)
(1, 2)
(0, 2)
(2, 2)
RequiredProgress must be positive. (Parameter 'Homework1.Progress')
(2, 2)
(0, 6)
(6, 6)
(7, 8)
(0, 8)
(8, 8)
RequiredProgress must be positive. (Parameter 'Homework1.Progress')
(8, 8)
(0, 12)
(12, 12)
(13, 14)
(0, 14)
(14, 14)
RequiredProgress must be positive. (Parameter 'Homework1.Progress')
(14, 14)
(0, 18)
(18, 18)
(19, 20)
 */
}