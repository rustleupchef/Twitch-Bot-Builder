using System;
using System.Collections.Generic;
using System.Windows;
using TwitchLib.Client;
using TwitchLib.Client.Models;
using TwitchLib.Client.Events;
using System.Windows.Controls;
using System.Diagnostics;
using System.IO;

namespace Twitch_Bot_Builder
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	/// 

	public partial class MainWindow : Window
	{
		public static MainWindow Instance;
		private ConnectionCredentials credentials = new(TwitchData.User, TwitchData.UserToken);
		private TwitchClient client = new();
		private bool run = false;
		public Dictionary<string, Action> words = new();
		public int index = 0;

		public MainWindow()
		{
			InitializeComponent();
			Instance = this;
			Button[] buttons = new Button[] { one, two, three, four, five, six, seven, eight, nine };
			foreach (Button button in buttons) button.Content = "";
			readActions();
		}


		private void Next_Click(object sender, RoutedEventArgs e)
		{
			index++;
			index = Math.Clamp(index, 0, (words.Count > 0) ? (int)((((float)words.Count) / 9f) + 0.9f) - 1 : 0);
			int start = index * 9;
			int end = Math.Clamp(start + 9, 0, words.Count);
			changePage(start, end);
		}

		private void Back_Click(object sender, RoutedEventArgs e)
		{
			index--;
			index = Math.Clamp(index, 0, (words.Count > 0) ? (int)((((float)words.Count) / 9f) + 0.9f) - 1 : 0);
			int start = index * 9;
			int end = Math.Clamp(start + 9, 0, words.Count);
			changePage(start, end);
		}

		private void Add_Click(object sender, RoutedEventArgs e)
		{
			Add add = new();
			add.Show();
		}

		private void Start_Click(object sender, RoutedEventArgs e)
		{
			run = !run;
			if (!run)
			{
				start.Content = "Start";
				client.Disconnect();
				return;
			}
			start.Content = "Stop";
			client.Initialize(credentials, TwitchData.User);
			client.Connect();
			client.OnMessageReceived += new EventHandler<OnMessageReceivedArgs>(Received_Message);
		}

		private void Received_Message(object sender, OnMessageReceivedArgs e)
		{
			if (!words.TryGetValue(e.ChatMessage.Message, out Action action)) return;
			switch((int)action.type)
			{
				case 1:
					System.Windows.Forms.SendKeys.SendWait(action.output);
					break;
				case 2:
					runProcess("cmd.exe", $"/c python \"{TwitchData.PythonPath}main.py\" \"{action.output}\" {action.getDuration()}", true);
					break;
				case 3:
					runProcess("cmd.exe", $"start \"{action.path}\"");
					break;
				case 4:
					System.Windows.Forms.Cursor.Position = new(action.point[0], action.point[1]);
					break;
			}
		}

		private void runProcess(string file, string arguments = "", bool createNoWindow = false)
		{

			Process process = new Process();
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.RedirectStandardOutput = true;
			process.StartInfo.RedirectStandardInput = true;
			process.StartInfo.FileName = file;
			if (arguments != string.Empty) process.StartInfo.Arguments = arguments;
			process.StartInfo.CreateNoWindow = createNoWindow;
			process.Start();
			process.WaitForExit();

		}

		private void readActions()
		{
			string text = File.ReadAllText(TwitchData.CommandsPath);
			if (text == string.Empty) return;
			string[] data = File.ReadAllText(TwitchData.CommandsPath).Split('\n');
			foreach(string line in data)
			{
				Action action = new Action();
				action.toAction(line);
				words.Add(line.Split(',')[0], action);
			}
			changePage(0, Math.Clamp(9, 0, words.Count));
		}

		public void changePage(int start, int stop)
		{
			//MessageBox.Show(stop.ToString());
			Button[] buttons = new Button[] {one, two, three, four, five, six, seven, eight, nine};
			foreach (Button button in buttons) button.Content = "";
			int i = 0;
			foreach (string key in words.Keys)
			{
				//MessageBox.Show($"{i}<{start}");
				if (i < start) { i++; continue; }
				if (i >= stop) break;
				buttons[i - start].Content = key;
				i++;
			}
			if (i != 0) return;
			foreach(Button button in buttons) button.Content = "";
		}

		private void one_Click(object sender, RoutedEventArgs e)
		{
			if (one.Content == "") return;
			Add add = new(false, (string) one.Content);
			add.Show();
		}

		private void two_Click(object sender, RoutedEventArgs e)
		{
			if (two.Content == "") return;
			Add add = new(false, (string) two.Content);
			add.Show();
		}

		private void three_Click(object sender, RoutedEventArgs e)
		{
			if (three.Content == "") return;
			Add add = new(false, (string) three.Content);
			add.Show();
		}

		private void four_Click(object sender, RoutedEventArgs e)
		{
			if (four.Content == "") return;
			Add add = new(false, (string) four.Content);
			add.Show();
		}

		private void five_Click(object sender, RoutedEventArgs e)
		{
			if (five.Content == "") return;
			Add add = new(false, (string) five.Content);
			add.Show();
		}

		private void six_Click(object sender, RoutedEventArgs e)
		{
			if (six.Content == "") return;
			Add add = new(false, (string) six.Content);
			add.Show();
		}

		private void seven_Click(object sender, RoutedEventArgs e)
		{
			if (seven.Content == "") return;
			Add add = new(false, (string) seven.Content);
			add.Show();
		}

		private void eight_Click(object sender, RoutedEventArgs e)
		{
			if (eight.Content == "") return;
			Add add = new(false, (string) eight.Content);
			add.Show();
		}

		private void nine_Click(object sender, RoutedEventArgs e)
		{
			if (nine.Content == "")return;
			Add add = new(false, (string) nine.Content);
			add.Show();
		}
	}
}