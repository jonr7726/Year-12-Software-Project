using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

// util is called from other functions, contains code to read an write to files
public class Util
{
    private static Util instance;
    public static Util Instance {
        get {
            if (instance == null) {
                instance = new Util();
            }
            return instance;
        }
    }

    public Util() {
        userLog = Directory.GetCurrentDirectory() + "/user_log.txt";
    }

    public string userLog; //directory for user log

    public List<string> ReadFile() {
        string file = userLog;
        if (!File.Exists(file)) {
            //File.Create(file);
            File.AppendAllText(file, "0 0 0 0 []\n");
            return (new string[] { "0 0 0 0 []" }).ToList();
        }

        return File.ReadLines(file).ToList();
    }

    public void WriteFile(string text) {
        string file = userLog;

        File.WriteAllText(file, text+"\n");
    }
}
