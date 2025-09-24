// See https://aka.ms/new-console-template for more information

using PopupLib;

string[] splashes = [
    "Fun!",
    "Welcome to hell",
    "So you have chosen, pain",
    "The least practical shell!",
    "...",
    "Whose idea was this?"
];

string menu = "main"; // this is probably a dumb way of handling menus but i don't give a damn honestly
string exec = "/bin/bash -c {0}";
while (true) {
    switch (menu) {
        case "main": {
            string select = SelectPopup.Quick(splashes[new Random().Next(splashes.Length)], [ "Run", "Settings", "Stats", "Quit" ], title: "TetriSH");
            switch (select) {
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
            string select = SelectPopup.Quick("Settings", ["Shell executable", "Back"], title: "TetriSH");
            switch (select) {
                case "Back": menu = "main"; break;
            }
            break;
        }
    }
}