using System;
using System.Collections.Generic;
using System.Windows;
using TwitchLib.Client;
using TwitchLib.Client.Models;
using TwitchLib.Client.Events;
using System.Windows.Controls;
using System.Net.Http;

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
		private int index = 0;

		public MainWindow()
		{
			InitializeComponent();
			Instance = this;
		}

		private void Next_Click(object sender, RoutedEventArgs e)
		{
			index++;
			index = Math.Clamp(index, 0, words.Count);
			int start = index * 11;
			int end = Math.Clamp(start + 11, 0, words.Count);
			changePage(start, end);
		}

		private void Back_Click(object sender, RoutedEventArgs e)
		{
			index--;
			index = Math.Clamp(index, 0, words.Count);
			int start = index * 11;
			int end = Math.Clamp(start + 11, 0, words.Count);
			changePage(start, end);
		}

		private void Add_Click(object sender, RoutedEventArgs e)
		{
			Add add = new();
			add.Show();
		}

		private void Start_Click(object sender, RoutedEventArgs e)
		{
			try
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
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
				throw;
			}
		}

		private void Received_Message(object sender, OnMessageReceivedArgs e)
		{
			MessageBox.Show(e.ChatMessage.Message);
		}

		private void changePage(int start, int stop)
		{
			Button[] buttons = new Button[] {one, two, three, four, five, six, seven, eight, nine};
			int i = 0;
			foreach (var key in words.Keys)
			{
				buttons[i].Content = key;
			}
		}

		private void one_Click(object sender, RoutedEventArgs e)
		{

		}

		private void two_Click(object sender, RoutedEventArgs e)
		{

		}

		private void three_Click(object sender, RoutedEventArgs e)
		{

		}

		private void four_Click(object sender, RoutedEventArgs e)
		{

		}

		private void five_Click(object sender, RoutedEventArgs e)
		{

		}

		private void six_Click(object sender, RoutedEventArgs e)
		{

		}

		private void seven_Click(object sender, RoutedEventArgs e)
		{

		}

		private void eight_Click(object sender, RoutedEventArgs e)
		{

		}

		private void nine_Click(object sender, RoutedEventArgs e)
		{

		}
	}
}