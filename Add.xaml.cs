using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Twitch_Bot_Builder
{
	/// <summary>
	/// Interaction logic for Add.xaml
	/// </summary>
	public partial class Add : Window
	{
		public Add Instance;
		private bool isAdd;
		private byte type;

		public Add(bool isAdd = true, string item = "")
		{
			this.isAdd = isAdd;
			InitializeComponent();
			Instance = this;
			if (!isAdd)
			{
				if (item != string.Empty)
				{
					MainWindow.Instance.words.TryGetValue(item, out Action action);
					Command.Text = item;
					Type.SelectedIndex = action.type - 1;
					Output.Text = action.output;
					Path.Text = action.path;
					Duration.Text = action.getDuration();
					Position.Text = action.getMousePos();
				}
				Back.Content = "Delete";
				Include.Content = "Change";
			}
		}

		private void Type_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ComboBoxItem item = Type.SelectedItem as ComboBoxItem;
			switch (item.Content)
			{
				case "Typing":
					type = 1;
					Output.Visibility = Visibility.Visible;
					Path.Visibility = Visibility.Hidden;
					Duration.Visibility = Visibility.Hidden;
					Position.Visibility = Visibility.Hidden;
					break;
				case "Key Press":
					type = 2;
					Output.Visibility = Visibility.Visible;
					Duration.Visibility = Visibility.Visible;
					Path.Visibility = Visibility.Hidden;
					Position.Visibility = Visibility.Hidden;
					break;
				case "Proccess":
					type = 3;
					Output.Visibility = Visibility.Hidden;
					Path.Visibility = Visibility.Visible;
					Duration.Visibility = Visibility.Hidden;
					Position.Visibility = Visibility.Hidden;
					break;
				case "Set Mouse Position":
					type = 4;
					Output.Visibility = Visibility.Hidden;
					Path.Visibility = Visibility.Hidden;
					Duration.Visibility = Visibility.Hidden;
					Position.Visibility = Visibility.Visible;
					break;
			}
		}

		private void Include_Click(object sender, RoutedEventArgs e)
		{
			if (type == 0)
			{
				MessageBox.Show("Please Enter Type");
				return;
			}

			Action action = new();
			switch (type)
			{
				case 1:
					action.setTyping(Output.Text);
					break;
				case 2:
					float duration;
					try
					{
						duration = Single.Parse(Duration.Text);
					}
					catch (Exception ex)
					{
						MessageBox.Show(ex.Message);
						MessageBox.Show("Please enter valid duration");
						return;
					}
					action.setKeyPressing(Output.Text, duration);
					break;
				case 3:
					if (!File.Exists(Path.Text))
					{
						MessageBox.Show("Please enter a file path that exists");
						return;
					}
					action.setProcess(Path.Text);
					break;
				case 4:
					int[] point = new int[2];
					string pointStr = Position.Text.Replace('(', ' ');
					pointStr = pointStr.Replace(')', ' ');
					try
					{
						string[] coords = pointStr.Split(',');
						point[0] = Int32.Parse(coords[0]);
						point[1] = Int32.Parse(coords[1]);
					}
					catch (Exception ex)
					{
						MessageBox.Show(ex.Message);
						MessageBox.Show("Please enter valid point. Format like (x, y) or x, y");
					}
					action.setMousePos(point);
					break;
			}
			MainWindow.Instance.words.Remove(Command.Text);
			MainWindow.Instance.words.Add(Command.Text, action);
			WriteData();
			int start = MainWindow.Instance.index * 9;
			int end = Math.Clamp(start + 9, 0, MainWindow.Instance.words.Count);
			MainWindow.Instance.changePage(start, end);
		}

		private void Back_Click(object sender, RoutedEventArgs e)
		{
			if (!isAdd && MainWindow.Instance.words.ContainsKey(Command.Text))
			{
				MessageBox.Show(Command.Text);
				MainWindow.Instance.words.Remove(Command.Text);
				WriteData();
				MainWindow.Instance.index = Math.Clamp(MainWindow.Instance.index, 0, MainWindow.Instance.words.Count);
				int start = MainWindow.Instance.index * 9;
				int end = Math.Clamp(start + 9, 0, MainWindow.Instance.words.Count);
				MainWindow.Instance.changePage(start, end);
			}
			Close();
		}

		private void WriteData()
		{
			Dictionary<string, Action> words = MainWindow.Instance.words;
			string data = "";
			int i = 0;
			foreach (string key in words.Keys)
			{
				words.TryGetValue(key, out Action action);
				data += key + "," + action.convertToString()  + (i < words.Count - 1 ? "\n" : "");
				i++;
			}
			File.WriteAllText(TwitchData.CommandsPath, data);
		}
	}
}