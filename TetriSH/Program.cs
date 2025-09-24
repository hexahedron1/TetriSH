// See https://aka.ms/new-console-template for more information

using System.Net.Http.Json;
using Newtonsoft.Json;
using PopupLib;
using TetriSH;

string[] splashes = [
    "Fun!",
    "Welcome to (s)hell",
    "So you have chosen, pain",
    "The least practical shell!",
    "...",
    "Whose idea was this?"
];
string splash = splashes[new Random().Next(splashes.Length)];
string menu = "main"; // this is probably a dumb way of handling menus but i don't give a damn honestly
string datadir = $"/home/{Environment.UserName}/.local/share/tetrish";
if (!Directory.Exists(datadir)) {
    Directory.CreateDirectory(datadir);
}
Data savedata = new(); 
if (File.Exists(Path.Join(datadir, "data.json"))) {
    string json = File.ReadAllText(Path.Join(datadir, "data.json"));
    Data? notjson = JsonConvert.DeserializeObject<Data>(json);
    if (notjson is not null)
        savedata = notjson;
} else {
    SaveData();
}
while (true) {
    switch (menu) {
        case "main": {
            string select = SelectPopup.Quick(splash, [ "Run", "Settings", "Stats", "Quit" ], title: "TetriSH");
            switch (select) {
                case "Run": menu = "tetris"; break;
                case "Settings": menu = "settings"; break;
                case "Quit": {
                    Console.CursorVisible = true;
                    Console.Clear();
                    Environment.Exit(0);
                    break;
                }
            }
            break;
        }
        case "settings": {
            string select = SelectPopup.Quick("Settings", ["Shell", "Back"], title: "TetriSH");
            switch (select) {
                case "Shell": {
                    savedata.Exec = TextPromptPopup.Quick($"Current: {savedata.Exec}\nThe shell executable path and arguments for running commands. {{0}} will be replaced with the line that has been cleared", placeholder: "type here", title: "TetriSH | Shell");
                    SaveData();
                    break;
                }
                case "Back": menu = "main"; break;
            }
            break;
        }
        case "tetris": {
            Console.Clear();
            for (var i = 0; i < 7; i++) {
                for (var j = 0; j < 4; j++) {
                    for (var k = 0; k < 4; k++) {
                        var off = TetrisEngine.tetrominoes[i, j, k];
                        Console.ForegroundColor = off.Item1 == off.Item2 && off.Item1 == 0 ? ConsoleColor.Yellow : ConsoleColor.White;
                        Console.SetCursorPosition(i*11 + 6 + off.Item1*2, j*6 + 3 + off.Item2);
                        Console.Write("██");
                    }
                }
            }
            Console.ReadKey(true);
            menu = "main";
            break;
        }
    }
}

void SaveData() {
    string json = JsonConvert.SerializeObject(savedata);
    File.WriteAllText(Path.Join(datadir, "data.json"), json);
}

class Data {
    public Data() {
        Exec = "/bin/bash -c {0}";
    }
    public string Exec { get; set; }
}