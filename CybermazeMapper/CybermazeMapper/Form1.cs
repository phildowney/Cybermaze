using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CybermazeMapper
{
    public partial class Form1 : Form
    {

        public string MapData = @"__,__,__,__,__,__,__,__,__,__,__,__,__,__,__
__,__,__,__,__,__,__,__,__,97,__,__,__,__,__
__,__,__,__,__,__,__,__,__,93,94,95,96,__,__
__,__,__,__,__,__,86,__,87,88,89,90,91,92,__
__,__,__,__,__,78,79,80,81,82,83,84,85,__,__
__,__,__,__,__,__,71,72,73,__,74,75,76,77,__
__,__,__,__,__,63,64,65,66,67,68,69,70,__,__
__,__,__,__,__,54,55,56,57,58,59,60,61,62,__
__,__,__,44,45,46,47,48,49,50,__,51,__,52,53
__,__,32,33,34,35,36,37,38,39,40,41,42,43,__
__,__,__,21,22,23,24,25,26,27,28,29,30,31,__
__,__,12,13,14,15,16,__,17,18,19,20,__,__,__
__,__,06,07,08,09,__,__,__,10,11,__,__,__,__
__,__,__,02,03,04,05,__,__,__,__,__,__,__,__
__,__,__,00,01,__,__,__,__,__,__,__,__,__,__
__,__,__,__,__,__,__,__,__,__,__,__,__,__,__
__,__,__,__,__,__,__,__,__,__,__,__,__,__,__";
        string[,] test;
        List<RoomControl> rooms = new List<RoomControl>();
        Map map;

        public Form1()
        {
            InitializeComponent();

            test = ReadFromCSV();

            map = ReadMapFile();

            for (int j = 0; j < test.GetLength(0); j++)
            {
                for (int i = 0; i < test.GetLength(1); i++)
                {
                    var room = test[j, i];

                    RoomControl roomControl = new RoomControl();
                    Controls.Add(roomControl);

                    roomControl.Width = 40;
                    roomControl.Height = 40;
                    roomControl.Empty = room.Contains("__");
                    roomControl.Index = room;
                    roomControl.BackColor = roomControl.Empty ? System.Drawing.Color.Pink : System.Drawing.Color.Red;
                    roomControl.ExitClicked += RoomControl_ExitClicked;
                    roomControl.DuckClicked += RoomControl_DuckClicked;
                    roomControl.Location = new System.Drawing.Point(i * 50 + 5, j * 50 + 5);
                    roomControl.Row = j;
                    roomControl.Col = i;
                    rooms.Add(roomControl);
                }
            }
        }

        private void RoomControl_DuckClicked(object sender, EventArgs e)
        {
            var clickedRoom = (RoomControl)sender;

            map.rooms[int.Parse(clickedRoom.Index)].duckLocation = clickedRoom.HasDuck;
        }

        private void RoomControl_ExitClicked(object sender, ExitClickedArgs e)
        {
            var clickedRoom = (RoomControl)sender;

            Console.WriteLine($"row: {clickedRoom.Row} col: {clickedRoom.Col}");

            if (!clickedRoom.Empty)
            {
                try
                {
                    RoomControl matchingRoom;
                    System.Drawing.Point p;

                    switch (e.Direction)
                    {
                        case ExitClickedArgs.Directions.North:
                            p = new System.Drawing.Point(0, -1);
                            break;
                        case ExitClickedArgs.Directions.South:
                            p = new System.Drawing.Point(0, 1);
                            break;
                        case ExitClickedArgs.Directions.East:
                            p = new System.Drawing.Point(1, 0);
                            break;
                        case ExitClickedArgs.Directions.West:
                            p = new System.Drawing.Point(-1, 0);
                            break;
                        default:
                            return;
                    }

                    matchingRoom = rooms.Single(room => room.Row == clickedRoom.Row + p.Y & clickedRoom.Col + p.X == room.Col) ;

                    if (matchingRoom.Empty) return;

                    matchingRoom.SetConnection((ExitClickedArgs.Directions)((int)e.Direction * -1)); // TODO: Make this less clever and more readable.
                    clickedRoom.SetConnection(e.Direction);

                    switch (e.Direction)
                    {
                        case ExitClickedArgs.Directions.North:
                            map.rooms[int.Parse(clickedRoom.Index)].wallUp = "door";
                            map.rooms[int.Parse(matchingRoom.Index)].wallDown = "door";
                            break;
                        case ExitClickedArgs.Directions.South:
                            map.rooms[int.Parse(clickedRoom.Index)].wallDown = "door";
                            map.rooms[int.Parse(matchingRoom.Index)].wallUp = "door";
                            break;
                        case ExitClickedArgs.Directions.East:
                            map.rooms[int.Parse(clickedRoom.Index)].wallRight = "door";
                            map.rooms[int.Parse(matchingRoom.Index)].wallLeft = "door";
                            break;
                        case ExitClickedArgs.Directions.West:
                            map.rooms[int.Parse(clickedRoom.Index)].wallLeft = "door";
                            map.rooms[int.Parse(matchingRoom.Index)].wallRight = "door";
                            break;
                        default:
                            return;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("error on north click");
                }
            }
        }

        public static Map ReadMapFile()
        {
            string path = @"rooms.json";
            StreamReader reader = new StreamReader(path);
            string jsonData = reader.ReadToEnd();
            reader.Close();

            Console.WriteLine("json is: " + jsonData);


            var options = new JsonSerializerOptions
            {
                IncludeFields = true
            };


            var map = JsonSerializer.Deserialize<Map>(jsonData, options);

            // Fix the single-digit room indexes. DON'T @ ME NERDS.
            map.rooms.ForEach(room =>
            {
                room.id = int.Parse(room.id).ToString();
                
                // TODO: Load these into the UI instead of clearing them, eh?
                //room.roomDown = "";
                //room.roomUp = "";
                //room.roomLeft = "";
                //room.roomRight = "";
                //room.wallDown = "";
                //room.wallUp = "";
                //room.wallRight = "";
                //room.wallLeft = "";
            });

            return map;
        }

        private const string CSV_FILE_PATH = "/Data/map.csv";

        // [0,0]...[0,14] is row 1 (empty)
        // [16,0]...[16,14] is the last row (empty)
        // So starting room (11) is at [12,9].
        public string[,] mapLayout { get; set; }

        public string[,] ReadFromCSV()
        {
            string path = @"C:\Users\Seraph\Documents\Cybermaze" + CSV_FILE_PATH;

            File.WriteAllText(path, MapData);

            string[] mapcsv = File.ReadAllLines(path);

            string[][] mapLayoutJagged = mapcsv.Select(line => 
            {
                var w = line.Split(',');

                //    .Select(r =>
                //    {
                //        if (r.Contains("__")) return r;

                //        return (int.Parse(r) - 1).ToString();
                //    }).ToArray();

                return w;
            }).ToArray();

            string[,] mapLayout = To2D(mapLayoutJagged);

            return mapLayout;
        }

        private static T[,] To2D<T>(T[][] source)
        {
            // lazy https://stackoverflow.com/questions/26291609/converting-jagged-array-to-2d-array-c-sharp
            try
            {
                int FirstDim = source.Length;
                int SecondDim = source.GroupBy(row => row.Length).Single().Key; // throws InvalidOperationException if source is not rectangular

                var result = new T[FirstDim, SecondDim];
                for (int i = 0; i < FirstDim; ++i)
                    for (int j = 0; j < SecondDim; ++j)
                        result[i, j] = source[i][j];

                return result;
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException("The given jagged array is not rectangular.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach(var room in map.rooms)
            {
                var id = room.id;
                var bgimg = room.GetBackgroundImage();
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            var jsonString = JsonSerializer.Serialize(map, new JsonSerializerOptions { IncludeFields = true, WriteIndented = true });
            File.WriteAllText("rooms.json", jsonString);
        }
    }

    
}
