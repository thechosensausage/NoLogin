﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoLogin
{
    class Settings
    {
        
        static string dir = Environment.GetEnvironmentVariable("windir") + @"\NoLogin";
        static string path = dir + @"\Settings";
        public static Dictionary<string, string> Variables = new Dictionary<string, string>();
        public static Dictionary<string, string> Temp = new Dictionary<string, string>();
        public static void Load()
        {
            string[] Directories = { dir };
            foreach (string s in Directories)
            {
                if (!Directory.Exists(s)) { Directory.CreateDirectory(s); }
            }
            try
            {
                string[] lines = File.ReadAllLines(path);
                foreach (string l in lines)
                {
                    try { Set(l.Split(" =:=:=".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[0], l.Split("=:=:=".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[1]); }
                    catch (Exception ex) { LogWriter.Write(ex.Message, "Setting.Load()"); }
                }
            }
            catch (Exception ex) { LogWriter.Write(ex.Message, "Setting.Load()"); }
            FillDefault();
        }
        static void FillDefault()
        {
            string[] DefaultKeys = { "PasswordEnabled" };
            string[] DefaultValues = { "True" };
            for (int i = 0; i < DefaultKeys.Length; i++)
            {
                try
                {
                    string t = Variables[DefaultKeys[i]];
                }
                catch
                {
                    Variables[DefaultKeys[i]] = DefaultValues[i];
                }
            }
        }
        public static void Save()
        {
            StreamWriter s = new StreamWriter(path);
            foreach (KeyValuePair<string, string> kvp in Variables)
            {
                s.WriteLine(kvp.Key + "=:=:=" + kvp.Value);
            }
            s.Close();
        }
        public static void Set(object key, object value)
        {
            try { Variables.Add(key.ToString(), value.ToString()); }
            catch { Variables[key.ToString()] = value.ToString(); }
            Save();

        }
        public static void SetTemp(object key, object value)
        {
            try { Temp.Add(key.ToString(), value.ToString()); }
            catch { Temp[key.ToString()] = value.ToString(); }


        }
        public static void CommitTemp()
        {
            foreach (KeyValuePair<string, string> kvp in Temp)
            {
                Set(kvp.Key, kvp.Value);
            }
        }
        public static void DiscardTemp()
        {
            Temp.Clear();
        }

    }
    class LogWriter
    {
        static string dir = Environment.GetEnvironmentVariable("appdata") + @"\Remote Download Manager";
        static string path = dir + @"\RDMLog.log";
        static string[] Directories = { dir };
        public static void Write(string text)
        {

            foreach (string ss in Directories)
            {
                if (!Directory.Exists(ss)) { Directory.CreateDirectory(ss); }
            }
            StreamWriter s = new StreamWriter(path, true);
            s.WriteLine(DateTime.Now.ToString() + ": \t" + text);
            s.Close();
        }
        public static void Write(string text, string thrower)
        {

            foreach (string ss in Directories)
            {
                if (!Directory.Exists(ss)) { Directory.CreateDirectory(ss); }
            }
            StreamWriter s = new StreamWriter(path, true);
            s.WriteLine(DateTime.Now.ToString() + ": \t|" + thrower + "|\t" + text);
            s.Close();
        }
    }
}